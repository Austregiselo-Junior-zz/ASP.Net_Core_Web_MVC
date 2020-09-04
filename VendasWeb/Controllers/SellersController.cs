using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VendasWeb.Services;

namespace VendasWeb.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService; // Criação de dependencia para injeção abaixo

        public SellersController(SellerService sellerService) //Injeção de dependencia 
        {
            _sellerService = sellerService;
        }

        public IActionResult Index() //Chamada do controlador
        {
            var list = _sellerService.FindAll(); //Retorna uma lista de sellers/ Controlador acessa o model/ pega o dado da lista 
            return View(list); // Encaminhamento para View

            //OBS (MVC acontecendo logo acima!)
        }
    }
}
