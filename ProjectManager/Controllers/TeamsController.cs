using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Services.Interfaces;
using ProjectManager.Domain.Entities;

namespace Web.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamsService _teamsService;
        private readonly IEmployeesService _employeesService;

        public TeamsController(ITeamsService teamsService, IEmployeesService employeesService)
        {
            _teamsService = teamsService;
            _employeesService = employeesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _teamsService.GetAllAsync());
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Create(Team team)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(team);
                }

                await _teamsService.AddAsync(team);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var team = await _teamsService.GetByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int id, Team team)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(team);
                }

                if (id != team.Id)
                {
                    return BadRequest();
                }


                if (!await _teamsService.ExistsByIdAsync(id))
                {
                    return NotFound();
                }

                await _teamsService.UpdateAsync(team);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _teamsService.GetByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var team = await _teamsService.GetByIdAsync(id);
                if (team == null)
                {
                    return NotFound();
                }

                await _teamsService.DeleteAsync(team);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var team = await _teamsService.GetByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> AddMember(int teamId, Guid employeeId)
        {
            try
            {
                var team = await _teamsService.GetByIdAsync(teamId);
                var employee = await _employeesService.GetByIdAsync(employeeId);
                if (team == null || employee == null)
                {
                    return NotFound();
                }

                await _teamsService.AddEmployeeToTeamAsync(team, employee);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Details), new { id = teamId });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> RemoveMember(int teamId, Guid employeeId)
        {
            try
            {
                var team = await _teamsService.GetByIdAsync(teamId);
                var employee = await _employeesService.GetByIdAsync(employeeId);
                if (team == null || employee == null)
                {
                    return NotFound();
                }

                await _teamsService.RemoveEmployeeFromTeamAsync(team, employee);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

            return Ok();
        }
    }
}
