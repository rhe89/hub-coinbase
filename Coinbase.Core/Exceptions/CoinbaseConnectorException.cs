using System;

namespace Coinbase.Core.Exceptions
{
    public class CoinbaseConnectorException : Exception
    {
        public CoinbaseConnectorException(string message) : base(message)
        {
            
        }
        public CoinbaseConnectorException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}