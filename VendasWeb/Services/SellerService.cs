using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendasWeb.Data;
using VendasWeb.Models;

namespace VendasWeb.Services
{
    public class SellerService
    {
        private readonly VendasWebContext _context;

        public SellerService(VendasWebContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList(); //Acessa a fonte de dados relacionados a vendedores (Sellers) e transforma em uma lista
        }

        public void Inser(Seller obj)
        {
            obj.Department = _context.Department.First(); //Coloca o primeiro departamento cadastrado (só pra não dar erro)
            _context.Add(obj); // Serviço de adicionar vendedor
            _context.SaveChanges(); //Confirmar adição no DB
        }
    }
}
