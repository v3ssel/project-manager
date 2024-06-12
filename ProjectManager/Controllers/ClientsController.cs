using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Services.Interfaces;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Controllers
{
    [Authorize(Roles = "admin")]
    public class ClientsController : Controller
    {
        private readonly IClientsService _clientsService;

        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _clientsService.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(client);
                }

                client.Id = Guid.NewGuid();
                await _clientsService.AddAsync(client);
            }
            catch
            {
                RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientsService.GetByIdAsync((Guid)id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Client client)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(client);
                }

                if (id != client.Id)
                {
                    return BadRequest();
                }

                if (!await _clientsService.ExistsByIdAsync(id))
                {
                    return NotFound();
                }

                await _clientsService.UpdateAsync(client);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
                
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientsService.GetByIdAsync((Guid)id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var client = await _clientsService.GetByIdAsync(id);
                if (client == null)
                {
                    return NotFound();
                }

                await _clientsService.DeleteAsync(client);
            }
            catch
            {
                RedirectToAction("Error", "Home");
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
