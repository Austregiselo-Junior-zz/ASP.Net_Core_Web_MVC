using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendasWeb.Data;
using VendasWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VendasWeb.Services.Exceptions;

namespace VendasWeb.Services
{
    public class SellerService
    {
        private readonly VendasWebContext _context;

        public SellerService(VendasWebContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync(); //Acessa a fonte de dados relacionados a vendedores (Sellers) e transforma em uma lista
        }// OBS: Conexôes com DB são açõee pesadas e lentas, por isso essa operação dever assincrona, ou seja, o APP não fica bloqueado durante o acesso.
        // Em casos de conexões sincronas a aplicação fica travada e há perda de performace

        public async Task InserAsync(Seller obj)
        {
            _context.Add(obj); // Serviço de adicionar vendedor
            await _context.SaveChangesAsync(); //Confirmar adição no DB
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);// Pesquisa o departamento e o ID do vendedor
            //Carregamento de um objeto associado a um objeto principal "eager loading"
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {

                throw new IntegrityException("Can't delete seller because he/she has sales");
            }
        }

        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new DllNotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
                //OBJ: Quando é executado uma atualizaçãpo no DB, p DB pode gerar uma exerção de conflito de concorrência. 
                //Para isso se faz:
            }
            catch (DBConcurrencyException e)
            { //Interceptando essa exceção de acesso a dados e jogando uma exceção em nivel se serviço, assim consewguimos segregar as camadas,
                ;// Assim o Selles controller só tem que dar conta da exceção em nivel de serviço (Respeito de arquitetura pensada (imagem do MVC no PDF do capitolo) )
                {

                    throw new DBConcurrencyException(e.Message);
                }

            }
        }
    }
}
