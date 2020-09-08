using System;
using System.Collections.Generic;
using System.Linq;

namespace VendasWeb.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        public Department()
        {
        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime initial, DateTime final) // Calcula o total de vendas no departamento dado as datas
        {
            return Sellers.Sum(Seller => Seller.TotalSales(initial, final));// O processo foi delegado para o departamento e o total de vendas do vendedor foi delegado ao vendedor
        }
    }
}
