using Microsoft.AspNetCore.Mvc;
using ProjectManager.Domain.Entities;
using ProjectManager.Application.Services.Interfaces;
using Web.Models;
using Microsoft.AspNetCore.Authorization;
using Application.Services.Interfaces;

namespace ProjectManager.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectsService _projectsService;
        private readonly IClientsService _clientsService;
        private readonly IFileManagerService _fileService;
        private readonly string _uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public ProjectsController(IProjectsService projectsService, IClientsService clientsService, IFileManagerService fileService)
        {
            _projectsService = projectsService;
            _clientsService = clientsService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _projectsService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectsService.GetByIdAsync((Guid)id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.ClientList = await _clientsService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(ProjectViewModel projectVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ClientList = await _clientsService.GetAllAsync();
                    return View(projectVm);
                }

                var project = projectVm.Project!;

                project.Id = Guid.NewGuid();
                project.Client = await _clientsService.GetByIdAsync((Guid)project.ClientId!);
                if (project.Client is null)
                {
                    return BadRequest();
                }

                await _fileService.SaveProjectFilesAsync(_uploadDir, projectVm.ProjectFiles!, projectVm.Project);
                await _projectsService.AddAsync(project);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectsService.GetByIdAsync((Guid)id);
            if (project == null)
            {
                return NotFound();
            }

            var projectVm = new ProjectViewModel()
            {
                Project = project,
            };

            ViewBag.ClientList = await _clientsService.GetAllAsync();
            return View(projectVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(Guid id, ProjectViewModel projectVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ClientList = await _clientsService.GetAllAsync();
                    return View(projectVm);
                }

                var project = await _projectsService.GetByIdAsync(id);
                if (project == null)
                {
                    return NotFound();
                }

                await _fileService.SaveProjectFilesAsync(_uploadDir, projectVm.ProjectFiles!, projectVm.Project);

                projectVm.Project.Id = id;
                await _projectsService.UpdateAsync(projectVm.Project);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectsService.GetByIdAsync((Guid)id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var project = await _projectsService.GetByIdAsync(id);
                if (project == null)
                {
                    return NotFound();
                }

                var files = await _projectsService.GetProjectFilesAsync(id);
                foreach(var file in files)
                {
                    _fileService.DeleteProjectFile(file.FilePath!);
                }

                await _projectsService.DeleteAsync(project);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            return Json(await _projectsService.SearchByNameAsync(query));
        }

        [HttpGet]
        public async Task<IActionResult> Sort(string? sortColumn, bool? desc, DateTime? start, DateTime? end)
        {
            start ??= DateTime.MinValue;
            end ??= DateTime.MaxValue;

            var projects = await _projectsService.GetInDateRange(start.Value, end.Value);

            if (sortColumn != null)
            {
                projects = sortColumn.ToLower() switch
                {
                    "client" => desc == null || !(bool)desc ? projects.OrderBy(x => x.Client!.Name) : projects.OrderByDescending(x => x.Client!.Name),
                    "name" => desc == null || !(bool)desc ? projects.OrderBy(x => x.Name) : projects.OrderByDescending(x => x.Name),
                    "sdate" => desc == null || !(bool)desc ? projects.OrderBy(x => x.StartTime) : projects.OrderByDescending(x => x.StartTime),
                    "edate" => desc == null || !(bool)desc ? projects.OrderBy(x => x.EndTime) : projects.OrderByDescending(x => x.EndTime),
                    _ => desc == null || !(bool)desc ? projects.OrderBy(x => x.Priority) : projects.OrderByDescending(x => x.Priority)
                };
            }

            return Json(projects);
        }

        [HttpGet]
        public async Task<IActionResult> Download(Guid projectId, int fileId)
        {
            var file = await _projectsService.GetProjectFileAsync(projectId, fileId);
            if (file == null)
            {
                return NotFound();
            }

            return File(new FileStream(file.FilePath!, FileMode.Open), "application/octet-stream", file.FileName);
        }

        [HttpDelete]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> DeleteFile(Guid projectId, int fileId)
        {
            try
            {
                var project = await _projectsService.GetByIdAsync(projectId);
                var projectFile = await _projectsService.GetProjectFileAsync(projectId, fileId);
                if (project == null || projectFile == null)
                {
                    return NotFound();
                }

                project.ProjectFiles.Remove(projectFile);
                await _projectsService.UpdateAsync(project);

                _fileService.DeleteProjectFile(projectFile.FilePath!);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

            return Ok();
        }
    }
}
