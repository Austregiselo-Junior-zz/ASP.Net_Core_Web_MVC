using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendasWeb.Data;
using VendasWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            _context.Add(obj); // Serviço de adicionar vendedor
            _context.SaveChanges(); //Confirmar adição no DB
        }

        public Seller FindById(int id)
        {
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);// Pesquisa o departamento e o ID do vendedor
            //Carregamento de um objeto associado a um objeto principal "eager loading"
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();

        }

        public void Update(Seller obj)
        {
            if (!_context.Seller.Any(x => x.Id == obj.Id))
            {
                throw new DllNotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);
                _context.SaveChanges();
                //OBJ: Quando é executado uma atualizaçãpo no DB, p DB pode gerar uma exerção de conflito de concorrência. 
                //Para isso se faz:
            }
            catch (DBConcurrencyException e){ //Interceptando essa exceção de acesso a dados e jogando uma exceção em nivel se serviço, assim consewguimos segregar as camadas,
            ;// Assim o Selles controller só tem que dar conta da exceção em nivel de serviço (Respeito de arquitetura pensada (imagem do MVC no PDF do capitolo) )
            {

                throw new DBConcurrencyException(e.Message);
            }

            }
        }
    } }
