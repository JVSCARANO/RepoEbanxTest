using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EbanxTest.Web.API.Models;

namespace EbanxTest.Web.API.Services
{    
    public class BalanceService
    {
       
        public Balance ExecutaOperacao( Balance balance,string tipoOperacao, double valorOperacao)
        {
            if (tipoOperacao == "withdraw")
                return Retirada(balance,valorOperacao);
            if (tipoOperacao == "deposito")
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
            List<Balance> balance = new List<Balance>();
            balance.Add(Deposito(contaDestino, valor));
            balance.Add(Retirada(contaDestino, valor));

            return balance;
        }

        
    }

    
}
