using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        readonly IReactionRepository _repository;

        public ReactionController(IReactionRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAccount([FromBody] Reaction created)
        {
            await _repository.Create(created);
            return Ok();
        }

        //[Authorize(Roles ="Admin")]
        [HttpGet]
        [Route("read-all")]
        public async Task<IActionResult> ReadAllAccounts()
        {
            var result = await _repository.ReadAll();
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("single/{id:guid}")]
        public async Task<IActionResult> ReadSingleAccount([FromRoute] int id)
        {
            var result = await _repository.ReadSingle(id);
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateAccount(Reaction updated)
        {
            await _repository.Update(updated);
            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteAccount(int id, string name)
        {
            await _repository.Delete(id);
            return Ok($"Kontot med ID={id} och namnet {name} har raderats från databasen.");
        }
    }
}
