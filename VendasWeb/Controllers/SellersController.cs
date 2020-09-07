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
using System.Diagnostics;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

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

        public async Task<IActionResult> Index() //Chamada do controlador
        {
            var list = await _sellerService.FindAllAsync(); //Retorna uma lista de sellers/ Controlador acessa o model/ pega o dado da lista 
            return View(list); // Encaminhamento para View

            //OBS (MVC acontecendo logo acima!)
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync(); // Busca do DB todos os departamentos
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost] //Garante que o método é um post
        [ValidateAntiForgeryToken]// Prevenção de ataque CSRF => Aproveita a seção de autenticação e envia dados maliciosos
        public async Task<IActionResult> Create(Seller seller) //Recebe a requisição do view
        {
            if (!ModelState.IsValid) //Validação caso o Java Scripti do navegador esteja desabilitado, assim o sistema não deixa cadastrar nada vazio
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            } // Isso valida a requisição

            await _sellerService.InserAsync(seller); //Adição
            return RedirectToAction(nameof(Index)); //Redireciona a resposta pro view chamando o método "IActionResult Index()" e desse jeiro proteje caso futuramente tenha que mudar o método Index()
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value); //Pesquisa o id no DB
            if (obj == null) // Se o ID no DB for null retorna NotFound()
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
            ;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value); //Pesquisa o id no DB
            if (obj == null) // Se o ID no DB for null retorna NotFound()
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id) //Abrir a tela pra editar o vendedor
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid) //Validação caso o Java Scripti do navegador esteja desabilitado, assim o sistema não deixa cadastrar nada vazio
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }
            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotfoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e) //Também podemos deletar esse tratamento se caso no catch acima usamos um super tipo, (Up Casting) 
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier//Pega o id interno da requisição
            };
            return View(viewModel);
        }
    }
}
