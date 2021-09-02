using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BalanceAPI.Services
{
    public class Utils
    {
        public static Dictionary<string, string> ParseParameters(string query)
        {            
            Dictionary<string, string> dicQueryString =
                    query.Split('&')
                         .ToDictionary(c => c.Split('=')[0],
                                       c => Uri.UnescapeDataString(c.Split('=')[1]));

            return dicQueryString;
          
        }

    }
}
