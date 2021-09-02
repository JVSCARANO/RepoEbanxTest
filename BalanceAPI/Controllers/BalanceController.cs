using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BalanceAPI.Models;
using BalanceAPI.Services;

namespace BalanceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly BalanceContext _context;

        public BalanceController(BalanceContext context)
        {
            _context = context;
        }

        // GET: api/Balance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Balance>>> GetBalance()
        {           
            return await _context.Balance.ToListAsync();
        }

        // GET: api/Balance/5
        [HttpGet("account_id={id}")]
        public async Task<ActionResult<Balance>> GetBalance(int id)
        {
            var balance = await _context.Balance.FindAsync(id);

            if (balance == null)
            {
                return NotFound();
            }

            return balance;
        }

       

        
        // POST: api/Balances
        [HttpPost]
        public async Task<ActionResult<Balance>> PostBalance(string type, int destination, double amount)
        {
            Balance balance;
            BalanceService balanceService = new BalanceService();
           
            if (balanceService.Criar(destination,_context,type))
            {
                balance = new Balance(destination, amount);
                _context.Balance.Add(balance);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetBalance", new { id = balance.BalanceID }, balance);
            }
            if (balanceService.Atualizar(destination, _context, type))
            {
                    balance = balanceService.ExecutaAtualizacao(_context.Balance.Find(destination), type, amount);
                    _context.Entry(balance).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                return CreatedAtAction("GetBalance", new { id = balance.BalanceID }, balance);
            }

            return NotFound();

        }

        // POST: api/Balances
        [HttpPost]
        public async Task<ActionResult<Balance>> PostBalance(string type, int origin, double amount, int destination)
        {
            BalanceService service = new BalanceService();
            if (service.Transferir(origin,destination,type,_context ))
            {
                List<Balance> balance = service.Transferencia(_context.Balance.Find(origin), _context.Balance.Find(destination), amount);
                _context.Entry(balance).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return NoContent();
               
            }
            return NotFound();


        }

       
    }
}
