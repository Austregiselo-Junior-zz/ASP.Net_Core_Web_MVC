using System.Collections.Generic;
using System.Linq;
using VendasWeb.Data;
using VendasWeb.Models;

namespace VendasWeb.Services
{
    public class DepartmentService
    {
        private readonly VendasWebContext _context;

        public DepartmentService(VendasWebContext context)
        {
            _context = context;
        }

        public List<Department> FindAll() // Método para retornar os departamentos ordenados por nome
        {
            return _context.Department.OrderBy(x=>x.Name).ToList();
        }


    }
}
