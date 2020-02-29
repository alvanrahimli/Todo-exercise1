using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ex1_ToDo.Models;
using ex1_ToDo.Repos.Todos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo_exercise1.Models.Dtos;
using ToDo_exercise1.Utilities;

namespace ex1_ToDo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodosRepo _repo;

        public TodosController(ITodosRepo repo)
        {
            this._repo = repo;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(int rn, int c)
        {
            if (rn < 0 || c <= 0)
                return BadRequest("Request number or count is less than zero.");

            string email = Helper.GetEmail(HttpContext);
            var todos = await _repo.GetTodos(rn, c, email);

            if (todos == null)
                return NotFound();

            return Ok(todos);
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 0)
                return BadRequest("id must be greater than zero");

            string email = Helper.GetEmail(HttpContext);
            var todo = await _repo.GetTodo(id, email);
            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]TodoCreateDto newTodo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string email = Helper.GetEmail(HttpContext);
            var isSuccesful = await _repo.CreateTodo(newTodo, email);
            if (isSuccesful)
                return Ok("Todo added succesful");

            return StatusCode(500);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(TodoUpdateDto todo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string email = Helper.GetEmail(HttpContext);
            var isSuccesful = await _repo.UpdateTodo(todo, email);
            if (isSuccesful)
                return Ok("Todo updated succesfully");

            return StatusCode(500);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0)
                return BadRequest("Id must be greater than 0");

            string email = Helper.GetEmail(HttpContext);
            var result = await _repo.DeleteTodo(id, email);

            if (result.res)
                return Ok("Todo is deleted succesfully");
            else if (!result.res && result.code == 0)
                return NotFound();
            else
                return StatusCode(500);
        }
    }
}