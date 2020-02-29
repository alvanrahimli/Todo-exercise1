using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ToDo_exercise1.Models.Dtos;
using ToDo_exercise1.Repos.Auth;

namespace ex1_ToDo.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepo _repo;
        public AuthController(IAuthRepo repo)
        {
            this._repo = repo;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]UserLogin usrCreds)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _repo.Login(usrCreds);

            if (result.userCreds != null)
            {
                return Ok(new
                {
                    token = result.token,
                    userCredentials = result.userCreds
                });
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]UserRegister newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _repo.Register(newUser);

            if (result.userCreds != null)
            {
                return Ok(new
                {
                    token = result.token,
                    userCredentials = result.userCreds
                });
            }

            return BadRequest("Username or Email is registered already.");
        }
    }
}