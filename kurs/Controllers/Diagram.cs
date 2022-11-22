using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using kurs.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace kurs.Controllers
{
    public class Diagram : Controller
    {
        private readonly ClinicContext _context;

        public Diagram(ClinicContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> DiagramC(DateTime begin, DateTime end)
        {
            var taskss = (from o in _context.Treatments
                          group o by o.DischargeDate into g
                          where (g.Key >= begin && g.Key <= end)
                          select new { Date = g.Key, DateCount = g.Count() }

                          ).AsEnumerable().Select(c => c.ToExpando());

            return View(taskss);

        }
        public async Task<IActionResult> DiagramI()
        {
            return View();

        }

    }

}

