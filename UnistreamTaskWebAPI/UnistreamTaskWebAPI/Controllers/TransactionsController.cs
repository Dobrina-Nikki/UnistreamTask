using Microsoft.AspNetCore.Mvc;
using UnistreamTaskWebAPI.Models;
using UnistreamTaskWebAPI.Services.Interfaces;

namespace UnistreamTaskWebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TransactionsController: ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transaction inputTransaction)
        {
            var result = await _service.CreateTransactionAsync(inputTransaction);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var transaction = await _service.GetTransactionAsync(id);
            return transaction != null ? Ok(transaction) : NotFound();
        }
    }
}
