using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BalanceAPI.Models;

namespace BalanceAPI.Services
{    
    public class BalanceService
    {
        private static readonly string[] Types = new[]
       {
            "withdraw", "deposit", "transfer"
        };

        public Balance ExecutaAtualizacao( Balance balance,string tipoOperacao, double valorOperacao)
        {
            if (tipoOperacao == Types[0].ToString())
                return Retirada(balance,valorOperacao);
            if (tipoOperacao == Types[1].ToString())
                return Deposito(balance, valorOperacao);
            return balance;

        }

        public Balance Retirada(Balance balance, double valorOperacao)
        {
            balance.BalanceValue -= valorOperacao;
            return balance;
        }
        public Balance Deposito(Balance balance, double valorOperacao)
        {
            balance.BalanceValue += valorOperacao;
            return balance;
        }

        public List<Balance> Transferencia ( Balance contaOrigem, Balance contaDestino, double valor)
        {
            List<Balance> balanceList = new List<Balance>();
            balanceList.Add(Deposito(contaDestino, valor));
            balanceList.Add(Retirada(contaOrigem, valor));

            return balanceList;
        }

        private bool BalanceExists(int id, BalanceContext context)
        {
            return context.Balance.Any(e => e.BalanceID == id);
        }

        public bool Atualizar(int id, BalanceContext context, string type)
        {
            return BalanceExists(id, context) && type != Types[2].ToString() && id != 0;
        }

        public bool Criar(int id, BalanceContext context,string type)
        {
            return !BalanceExists(id, context ) && type == Types[1].ToString() && id != 0;
        }

        public bool Transferir(int origin, int destination, string type, BalanceContext context)
        {
            return BalanceExists(destination,context) && BalanceExists(origin,context) && type == Types[2].ToString() && origin !=0 && destination != 0;
                
        }
    }

    
}
