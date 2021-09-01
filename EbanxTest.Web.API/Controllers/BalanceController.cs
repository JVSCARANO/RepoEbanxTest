using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EbanxTest.Web.API.Models;
using EbanxTest.Web.API.Services;

namespace EbanxTest.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly BalanceContext _context;

        public BalanceController(BalanceContext context)
        {
            _context = context;
        }

        // GET: api/Balances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Balance>>> GetBalance()
        {
            return await _context.Balance.ToListAsync();
        }

        // GET: api/Balances/5
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

        
        // POST: api/Balances
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Balance>> PostBalance(string type, int destination, double amount)
        {
            Balance balance;
           
            if (!BalanceExists(destination))
            {
                balance = new Balance(destination, amount);
                _context.Balance.Add(balance);
                await _context.SaveChangesAsync();                
            }
            else
            {
                try
                {
                    BalanceService service = new BalanceService();
                    balance = service.ExecutaOperacao( _context.Balance.Find(destination), type, amount);
                    _context.Entry(balance).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    if (!BalanceExists(destination))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }                
            }
            return CreatedAtAction("GetBalance", new { id = balance.BalanceID }, balance);
        }

        // POST: api/Balances
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Balance>> PostBalance(string type, int origin, double amount, int destination)
        {
            if (!BalanceExists(destination) || !BalanceExists(origin))
            {
                return NotFound();
            }
            else
            {
                try
                {
                    BalanceService service = new BalanceService();
                    List<Balance> balance = service.Transferencia(_context.Balance.Find(origin), _context.Balance.Find(destination), amount);
                    _context.Entry(balance).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    if (!BalanceExists(destination))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return NoContent();
        }

        private bool BalanceExists(int id)
        {
            return _context.Balance.Any(e => e.BalanceID == id);
        }
    }
}
