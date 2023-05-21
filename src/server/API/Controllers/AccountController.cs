using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace API.Controllers
{
    public class AccountController : Controller
    {
        readonly IReaction<Account> _repository;

        public AccountController(IReaction<Account> repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAccount([FromBody] Account newAccount)
        {
            await _repository.Create(newAccount);
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
        public async Task<IActionResult> ReadSingleAccount([FromRoute] Guid id)
        {
            var result = await _repository.ReadSingle(id);
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateAccount(Account updated)
        {
            await _repository.Update(updated);
            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteAccount(Guid id, string name)
        {
            await _repository.Delete(id);
            return Ok($"Kontot med ID={id} och namnet {name} har raderats från databasen.");
        }
    }
}
