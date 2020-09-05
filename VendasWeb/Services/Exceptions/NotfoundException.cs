using System;

namespace VendasWeb.Services.Exceptions
{
    public class NotfoundException : ApplicationException
    {
        public NotfoundException(string message) : base(message)
        {

        }
    }
}
