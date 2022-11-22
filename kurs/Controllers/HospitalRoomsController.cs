#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using kurs.Models;

namespace kurs.Controllers
{
    public class HospitalRoomsController : Controller
    {
        private readonly ClinicContext _context;

        public HospitalRoomsController(ClinicContext context)
        {
            _context = context;
        }

        // GET: HospitalRooms
        public async Task<IActionResult> Index()
        {
            ViewBag.a = TempData["message"];


            return View(await _context.HospitalRooms.ToListAsync());
        }

        // GET: HospitalRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalRoom = await _context.HospitalRooms
                .FirstOrDefaultAsync(m => m.NumberHospitalRoom == id);
            if (hospitalRoom == null)
            {
                return NotFound();
            }

            return View(hospitalRoom);
        }

        // GET: HospitalRooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HospitalRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumberHospitalRoom,MobileNumberHospitalRoom")] HospitalRoom hospitalRoom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hospitalRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hospitalRoom);
        }

        // GET: HospitalRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalRoom = await _context.HospitalRooms.FindAsync(id);
            if (hospitalRoom == null)
            {
                return NotFound();
            }
            return View(hospitalRoom);
        }

        // POST: HospitalRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumberHospitalRoom,MobileNumberHospitalRoom")] HospitalRoom hospitalRoom)
        {

            var ad = hospitalRoom.NumberHospitalRoom;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hospitalRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalRoomExists(hospitalRoom.NumberHospitalRoom))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hospitalRoom);
        }

        // GET: HospitalRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var a = (from exist in _context.PatientInHospitalRooms
                     where (id == exist.NumberHospitalRoom)
                     select new { ec = exist.NumberHospitalRoom }).Count();
            if (a > 0)
            {

                TempData["message"] = "В палате находятся пациенты , сначала переместите их , затем удалите ";

                return RedirectToAction(nameof(Index));
            }
            var hospitalRoom = await _context.HospitalRooms
                .FirstOrDefaultAsync(m => m.NumberHospitalRoom == id);
            if (hospitalRoom == null)
            {
                return NotFound();
            }

            return View(hospitalRoom);
        }

        // POST: HospitalRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hospitalRoom = await _context.HospitalRooms.FindAsync(id);
            _context.HospitalRooms.Remove(hospitalRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalRoomExists(int id)
        {
            return _context.HospitalRooms.Any(e => e.NumberHospitalRoom == id);
        }
    }
}
