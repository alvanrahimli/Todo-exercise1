using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ex1_ToDo.Data;
using ex1_ToDo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDo_exercise1.Models.Dtos;
using ToDo_exercise1.Utilities;

namespace ToDo_exercise1.Repos.Auth
{
    public class AuthRepo : IAuthRepo
    {
        private readonly TodoDbContext _context;
        private readonly IConfiguration _config;

        public AuthRepo(TodoDbContext context, IConfiguration config)
        {
            this._context = context;
            this._config = config;
        }

        public async Task<(UserDto userCreds, string token)> Login(UserLogin loginCreds)
        {
            var loggedUser = await AuthenticateUser(loginCreds);

            if (loggedUser != null)
            {
                string tokentStr = GenerateToken(loggedUser);
                return (loggedUser, tokentStr);
            }

            return (null, null);
        }

        public async Task<(UserDto userCreds, string token)> Register(UserRegister newUser)
        {
            var exists = await UsernameExists(newUser.Username) || await EmailExists(newUser.Email);
            if (exists)
                return (null, null);

            User user = new User()
            {
                Username = newUser.Username,
                Email = newUser.Email,
                NormalizedUsername = newUser.Username.ToLower(),
                NormalizedEmail = newUser.Email.ToLower(),
                PasswordHash = Helper.ComputeHash(newUser.Password)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var loginResult = await Login(new UserLogin()
            {
                Parameter = user.NormalizedEmail,
                Password = newUser.Password
            });
            return loginResult;
        }

        // TODO: Implement "password recovery"

        private string GenerateToken(UserDto userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, _config["Jwt:audience"])
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:issuer"],
                audience: _config["Jwt:audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }

        private async Task<UserDto> AuthenticateUser(UserLogin loginCreds)
        {
            var usr = await _context.Users
                .FirstOrDefaultAsync(u => loginCreds.IsEmail
                ? u.NormalizedEmail == loginCreds.Parameter.ToLower()
                && u.PasswordHash.SequenceEqual(Helper.ComputeHash(loginCreds.Password))
                : u.NormalizedUsername == loginCreds.Parameter.ToLower()
                && u.PasswordHash.SequenceEqual(Helper.ComputeHash(loginCreds.Password)));
            // Bilirem, SequenceEqual() methodu boyuk arraylarda loop-a nisbeten 
            // 10 defe yavasdi, amma bizde de boyuk loop deyil diye ele bele saxladim :D

            if (usr != null)
            {
                UserDto usrToReturn = new UserDto()
                {
                    Id = usr.Id,
                    Username = usr.Username,
                    Email = usr.Email,
                    TodoCount = usr.TodoCount
                };
                return usrToReturn;
            }
            return null;
        }

        private async Task<bool> UsernameExists(string username)
        {
            var exists = await _context.Users.AnyAsync(u => u
                .NormalizedUsername == username.ToLower());

            return exists;
        }

        private async Task<bool> EmailExists(string email)
        {
            var exists = await _context.Users.AnyAsync(u => u
                .NormalizedEmail == email.ToLower());

            return exists;
        }
    }
}
