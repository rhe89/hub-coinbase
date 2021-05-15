using System;

namespace Coinbase.Core.Exceptions
{
    public class CoinbaseApiConnectorException : Exception
    {
        public CoinbaseApiConnectorException(string message) : base(message)
        {
            
        }
        public CoinbaseApiConnectorException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}