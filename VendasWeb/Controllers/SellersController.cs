using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Utilities;
using VendasWeb.Models;
using VendasWeb.Services;
using VendasWeb.Models.ViewModels;
using VendasWeb.Services.Exceptions;

namespace VendasWeb.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService; // Criação de dependencia para injeção abaixo
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService) //Injeção de dependencia 
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index() //Chamada do controlador
        {
            var list = _sellerService.FindAll(); //Retorna uma lista de sellers/ Controlador acessa o model/ pega o dado da lista 
            return View(list); // Encaminhamento para View

            //OBS (MVC acontecendo logo acima!)
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll(); // Busca do DB todos os departamentos
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost] //Garante que o método é um post
        [ValidateAntiForgeryToken]// Prevenção de ataque CSRF => Aproveita a seção de autenticação e envia dados maliciosos
        public IActionResult Create(Seller seller) //Recebe a requisição do view
        {
            _sellerService.Inser(seller); //Adição
            return RedirectToAction(nameof(Index)); //Redireciona a resposta pro view chamando o método "IActionResult Index()" e desse jeiro proteje caso futuramente tenha que mudar o método Index()
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value); //Pesquisa o id no DB
            if (obj == null) // Se o ID no DB for null retorna NotFound()
            {
                return NotFound();
            }

            return View(obj);
            ;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value); //Pesquisa o id no DB
            if (obj == null) // Se o ID no DB for null retorna NotFound()
            {
                return NotFound();
            }

            return View(obj);
        }

        public IActionResult Edit(int? id) //Abrir a tela pra editar o vendedor
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }

            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)
            {
                return BadRequest();
            }
            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotfoundException)
            {
                return NotFound();
            }
            catch (DbConcurrencyException)
            {
                return BadRequest();
            } 

        }
    }
}
