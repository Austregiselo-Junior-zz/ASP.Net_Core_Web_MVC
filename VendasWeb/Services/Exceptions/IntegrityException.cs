using System;

namespace VendasWeb.Services.Exceptions
{
    public class IntegrityException : ApplicationException
    {
        public IntegrityException(string message) : base(message)
        {

        }
    }
}// exerção perssonalizada de deleção de vendendor com vendas
