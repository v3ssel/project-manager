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
        private readonly IProjectsService _projectsService;
        private readonly IProjectFilesService _projectFilesService;

        public ClientsController(IClientsService clientsService, IProjectsService projectsService, IProjectFilesService projectFilesService)
        {
            _clientsService = clientsService;
            _projectsService = projectsService;
            _projectFilesService = projectFilesService;
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
                return RedirectToAction("Error", "Home");
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

                var projects = await _projectsService.GetByClientAsync(client);
                foreach (var project in projects)
                {
                    await _projectFilesService.DeleteAllProjectFilesAsync(project);
                }
                
                await _clientsService.DeleteAsync(client);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
