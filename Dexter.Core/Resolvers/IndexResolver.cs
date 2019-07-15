using Dexter.Core.Interfaces;
using System.Collections.Generic;
using System.Configuration;

namespace Dexter.Core.Resolvers
{
    public class IndexResolver
    {
        public static string GetIndexName(string alias)
        {
            // Look for a web config setting named Dexter:Environment, if one isn't found throw an exception
            var dexterEnvironment = ConfigurationManager.AppSettings["Dexter:Environment"];

            if(string.IsNullOrEmpty(dexterEnvironment))
            {
                return alias;
            }

            return $"{dexterEnvironment}_{alias}";
        }
    }
}
