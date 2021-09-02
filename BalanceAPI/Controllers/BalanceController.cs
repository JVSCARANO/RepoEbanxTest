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
        private BalanceContext _context;

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
        [HttpGet("{id}")]
        public async Task<ActionResult<Balance>> GetBalance(int id)
        {
            var balance = await _context.Balance.FindAsync(id);

            if (balance == null)
            {
                return NotFound();
            }

            return balance;
        }

        [HttpPost("reset")]
        public async Task<ActionResult<Balance>> PostBalance()
        {
            foreach (Balance b in _context.Balance)
            {
                _context.Entry(b).State = EntityState.Deleted;
            }


            await _context.SaveChangesAsync();
            return NoContent();
        }

            // POST: api/Balances
        [HttpPost]
        public async Task<ActionResult<Balance>> PostBalance(string type, int origin, int destination, double amount)
        {
            Balance balanceDest;
            Balance balanceOri;
            BalanceService balanceService = new BalanceService();
           
            if (balanceService.Criar(destination,_context,type))
            {
                balanceDest = new Balance(destination, amount);
                _context.Balance.Add(balanceDest);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetBalance", new { id = balanceDest.BalanceID }, balanceDest);
            }
            if (balanceService.Atualizar(destination, _context, type))
            {
                balanceDest = balanceService.ExecutaAtualizacao(_context.Balance.Find(destination), type, amount);
                    _context.Entry(balanceDest).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                return CreatedAtAction("GetBalance", new { id = balanceDest.BalanceID }, balanceDest);
            }
            if (balanceService.Transferir(origin, destination, type, _context))
            {

                balanceDest = _context.Balance.Find(destination);
                balanceOri = _context.Balance.Find(origin);


                List<Balance> balanceList =  balanceService.Transferencia(balanceOri, balanceDest, amount);

                foreach (Balance b in balanceList)
                {
                    _context.Entry(b).State = EntityState.Modified;
                }
                
                
                await _context.SaveChangesAsync();
                return NoContent();

            }

            return NotFound();

        }

   
       
    }
}
