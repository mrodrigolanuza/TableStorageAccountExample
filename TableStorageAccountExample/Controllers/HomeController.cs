using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TableStorageAccountExample.Data;
using TableStorageAccountExample.Models;
using TableStorageAccountExample.Services;

namespace TableStorageAccountExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAzureTableStorage<Log> _logStorage;

        public HomeController(ApplicationDbContext context, IAzureTableStorage<Log> logStorage) {
            _context = context;
            _logStorage = logStorage;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> Privacy() {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var date = DateTime.UtcNow;  //UTC para evitar problemas
            var reverseTicks = string.Format("{0:D19}", DateTime.MaxValue.Ticks - date.Ticks);  //Formateado a la misma cantidad de dígitos

            var log = new Log(currentUser.Id, reverseTicks) {
                Message = $"User has opened Privacy page at ({date.ToLongDateString()})"
            };
            await _logStorage.Insert(log);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
