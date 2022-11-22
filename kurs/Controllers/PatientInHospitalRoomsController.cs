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
    public class PatientInHospitalRoomsController : Controller
    {
        private readonly ClinicContext _context;

        public PatientInHospitalRoomsController(ClinicContext context)
        {
            _context = context;
        }

        // GET: PatientInHospitalRooms
        public async Task<IActionResult> Index()
        {

            dynamic tasks = (from p in _context.Patients
                             join o in _context.PatientInHospitalRooms on p.CodePatient equals o.CodePatient

                             select new { Name = p.Name, Age = p.Age, CodePatient = o.CodePatient, NumberHospitalRoom = o.NumberHospitalRoom }).AsEnumerable().Select(c => c.ToExpando()); ;

            return View(tasks);
        }

        // GET: PatientInHospitalRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            dynamic tasks = (from p in _context.Patients
                             join o in _context.PatientInHospitalRooms on p.CodePatient equals o.CodePatient
                             where (o.CodePatient == id)
                             select new { Name = p.Name, Age = p.Age, CodePatient = o.CodePatient, NumberHospitalRoom = o.NumberHospitalRoom }).AsEnumerable().Select(c => c.ToExpando()); ;

            return View(tasks);



        }

        // GET: PatientInHospitalRooms/Create
        public IActionResult Create()
        {
            var Patient = (from c in _context.Patients
                           join p in _context.PatientInHospitalRooms on c.CodePatient equals p.CodePatient into ps
                           from p in ps.DefaultIfEmpty()
                           where p.CodePatient != c.CodePatient
                           select new { Name = c.Name, Age = c.Age, SpecialSigns=c.SpecialSigns , ColorHair=c.ColorHair,CodePatient = c.CodePatient }).AsEnumerable().GroupBy(test => test.CodePatient)
      .Select(grp => grp.First()).ToList();
            var Room = (from o in _context.HospitalRooms
                        select new { NumberHospitalRoom = o.NumberHospitalRoom }).AsEnumerable().ToList();
            List<int> Age = new List<int>();
            List<int> NumberHospitalRoom = new List<int>();
            Dictionary<int, string> Patients = new Dictionary<int, string>();
            foreach (var r in Room)
            { NumberHospitalRoom.Add(r.NumberHospitalRoom); }
            foreach (dynamic item in Patient)
            {
               
                string? add =("Name="+item.Name+" Age="+(item.Age ?? "none") + " ColorHair="+(item.ColorHair?? "none") + " SpecialSigns = " + (item.SpecialSigns ?? "none"));
                Patients.Add(item.CodePatient, add);

               
            }
            ViewBag.Patients = Patients;
            ViewBag.NumberHospitalRoom = NumberHospitalRoom;
            

            return View();
        }

        // POST: PatientInHospitalRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumberHospitalRoom,CodePatient")] PatientInHospitalRoom patientInHospitalRoom)
        {
            if (ModelState.IsValid)
            {

                _context.Add(patientInHospitalRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodePatient"] = new SelectList(_context.Patients, "CodePatient", "CodePatient", patientInHospitalRoom.CodePatient);
            ViewData["NumberHospitalRoom"] = new SelectList(_context.HospitalRooms, "NumberHospitalRoom", "NumberHospitalRoom", patientInHospitalRoom.NumberHospitalRoom);
            return View(patientInHospitalRoom);
        }

        // GET: PatientInHospitalRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientInHospitalRoom = await _context.PatientInHospitalRooms.FindAsync(id);
            if (patientInHospitalRoom == null)
            {
                return NotFound();
            }
            ViewData["CodePatient"] = new SelectList(_context.Patients, "CodePatient", "CodePatient", patientInHospitalRoom.CodePatient);
            ViewData["NumberHospitalRoom"] = new SelectList(_context.HospitalRooms, "NumberHospitalRoom", "NumberHospitalRoom", patientInHospitalRoom.NumberHospitalRoom);
            return View(patientInHospitalRoom);
        }

        // POST: PatientInHospitalRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumberHospitalRoom,CodePatient")] PatientInHospitalRoom patientInHospitalRoom)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientInHospitalRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientInHospitalRoomExists(patientInHospitalRoom.CodePatient))
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
            ViewData["CodePatient"] = new SelectList(_context.Patients, "CodePatient", "CodePatient", patientInHospitalRoom.CodePatient);
            ViewData["NumberHospitalRoom"] = new SelectList(_context.HospitalRooms, "NumberHospitalRoom", "NumberHospitalRoom", patientInHospitalRoom.NumberHospitalRoom);
            return View(patientInHospitalRoom);
        }

        // GET: PatientInHospitalRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var patientInHospitalRoom = await _context.PatientInHospitalRooms
              .Include(p => p.CodePatientNavigation)
              .Include(p => p.NumberHospitalRoomNavigation)
              .FirstOrDefaultAsync(m => m.CodePatient == id);
            if (patientInHospitalRoom == null)
            {
                return NotFound();
            }

            return View(patientInHospitalRoom);
        }

        // POST: PatientInHospitalRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientInHospitalRoom = await _context.PatientInHospitalRooms.FindAsync(id);
            _context.PatientInHospitalRooms.Remove(patientInHospitalRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientInHospitalRoomExists(int id)
        {
            return _context.PatientInHospitalRooms.Any(e => e.CodePatient == id);
        }
    }
}
