using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo_exercise1.Models.Dtos;
using ToDo_exercise1.Repos.Items;
using ToDo_exercise1.Utilities;

namespace ex1_ToDo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepo _repo;

        public ItemsController(IItemsRepo repo)
        {
            this._repo = repo;
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 0)
                return BadRequest();

            string email = Helper.GetEmail(HttpContext);
            var item = await _repo.GetItem(id, email);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost("addrange")]
        public async Task<IActionResult> AddRange([FromBody]List<ItemCreateDto> items)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string email = Helper.GetEmail(HttpContext);
            var result = await _repo.AddRange(items, email);

            if (result)
                return Ok();

            return StatusCode(500);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]ItemCreateDto item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string email = Helper.GetEmail(HttpContext);
            var result = await _repo.Add(item, email);

            if (result)
                return Ok(item);

            return StatusCode(500);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]ItemUpdateDto item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string email = Helper.GetEmail(HttpContext);
            var result = await _repo.Update(item, email);

            if (result)
            {
                var updatedItem = await _repo.GetItem(item.Id, email);
                return Ok(updatedItem);
            }
            return StatusCode(500);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0)
                return BadRequest("is is less than 0");

            string email = Helper.GetEmail(HttpContext);
            var result = await _repo.Delete(id, email);

            if (result)
                return Ok();

            return StatusCode(500);
        }

        [HttpDelete("deleterange")]
        public async Task<IActionResult> DeleteRange([FromBody]int[] ids)
        {
            foreach (var id in ids)
            {
                if (id < 0)
                    return BadRequest("id is less than 0");
            }

            string email = Helper.GetEmail(HttpContext);
            var result = await _repo.DeleteRange(ids, email);

            if (result)
                return Ok();

            return StatusCode(500);
        }
    }
}