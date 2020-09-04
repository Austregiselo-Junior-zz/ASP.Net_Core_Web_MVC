using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Utilities;
using VendasWeb.Models;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] //Garante que o método é um post
        [ValidateAntiForgeryToken]// Prevenção de ataque CSRF => Aproveita a seção de autenticação e envia dados maliciosos
        public IActionResult Create(Seller seller) //Recebe a requisição do view
        {
            _sellerService.Inser(seller); //Adição
            return RedirectToAction(nameof(Index)); //Redireciona a resposta pro view chamando o método "IActionResult Index()" e desse jeiro proteje caso futuramente tenha que mudar o método Index()
        }
        
    }
}
