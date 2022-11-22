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
    public class PatientsController : Controller
    {
        private readonly ClinicContext _context;

        public PatientsController(ClinicContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {


            ViewBag.a = TempData["messagee"];
            return View(await _context.Patients.ToListAsync());
        }
        public async Task<IActionResult> Filter(string name)
        {

            if (name == null)
            {
                return NotFound();
            }

            int count = _context.Patients.Where(m => m.Name == name).Count();
            dynamic tasks = new System.Dynamic.ExpandoObject();
            if (count == 0)
            {
                TempData["messagee"] = "put the patient in the room";

                return RedirectToAction(nameof(Index));
            }
            if (count > 1)
            {
                ViewBag.Age = "age";

                tasks = (from p in _context.Patients
                         join c in _context.PatientInHospitalRooms on p.CodePatient equals c.CodePatient
                         join o in _context.HospitalRooms on c.NumberHospitalRoom equals o.NumberHospitalRoom
                         where (p.Name == name)
                         select new { Name = p.Name, NumberRoom = c.NumberHospitalRoom, Age = p.Age, Phone = o.MobileNumberHospitalRoom }).AsEnumerable().ToList();
                if (tasks.Count != count)
                {
                    TempData["messagee"] = "put the patient in the room";
                    return RedirectToAction(nameof(Index));
                }
                return View(tasks);
            }


            else
            {
                ViewBag.Age = null;
                tasks = (from p in _context.Patients
                         join c in _context.PatientInHospitalRooms on p.CodePatient equals c.CodePatient
                         join o in _context.HospitalRooms on c.NumberHospitalRoom equals o.NumberHospitalRoom
                         where (p.Name == name)
                         select new { Name = p.Name, NumberRoom = c.NumberHospitalRoom, Phone = o.MobileNumberHospitalRoom }).AsEnumerable().ToList();
                if (tasks.Count != count)
                {
                    TempData["messagee"] = "put the patient in the room";

                    return RedirectToAction(nameof(Index));
                }
                return View(tasks);
            }

        }
        public async Task<IActionResult> FilterDaysWereFemale(DateTime filter, int age)
        {
            var a = DateTime.Now.Date;

            if (a > filter)
            {
                TempData["messagee"] = "Date error";
                return RedirectToAction(nameof(Index));
            }
            dynamic tasks = new System.Dynamic.ExpandoObject();
            tasks = (from p in _context.Patients
                     join c in _context.PatientInHospitalRooms on p.CodePatient equals c.CodePatient
                     join o in _context.Treatments on p.CodePatient equals o.CodePatient
                     where (p.Gender == "Female" && p.Age > age)
                     select new { CodePatient = p.CodePatient, Name = p.Name, Gender = p.Gender, ApproximateHeight = p.ApproximateHeight, ColorHair = p.ColorHair, NumberRoom = c.NumberHospitalRoom, SpecialSigns = p.SpecialSigns, Age = p.Age, GivenDate = filter, DaysInHospital = (o.DischargeDate ?? filter) }).AsEnumerable().Where(o => o.DaysInHospital == filter).ToList(); ;
            if (tasks.Count == 0)
            {
                TempData["messagee"] = "Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(tasks);
        }
        public async Task<IActionResult> FilterDays(DateTime filter)
        {

            dynamic tasks = new System.Dynamic.ExpandoObject();
            tasks = (from p in _context.Patients
                     join c in _context.PatientInHospitalRooms on p.CodePatient equals c.CodePatient
                     join o in _context.Treatments on p.CodePatient equals o.CodePatient
                     where(filter>=o.ReceiptDate)
                     select new { Name = p.Name, NumberRoom = c.NumberHospitalRoom, Age = p.Age, ReceiptDate = o.ReceiptDate, Date = (o.DischargeDate ?? filter) }).AsEnumerable().Where(o => o.Date >= filter).ToList();
            if (tasks.Count == 0)
            {
                TempData["messagee"] = "Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(tasks);
        }
        public async Task<IActionResult> FilterMaxDays()
        {
            
            var result = (from p in _context.Patients
                          join o in _context.Treatments on p.CodePatient equals o.CodePatient
                          select new { CodePatient = p.CodePatient, Name = p.Name, Gender = p.Gender, SpecialSigns = p.SpecialSigns, Age = p.Age, ColorHair = p.ColorHair, ApproximateHeight = p.ApproximateHeight, DaysInHospital = (int)((o.DischargeDate ?? DateTime.Now) - o.ReceiptDate).TotalDays }).AsEnumerable().GroupBy(o => o.CodePatient)
                 .Select(g => new { CodePatient = g.Key, Gender = g.Select(x => x.Gender).FirstOrDefault(), SpecialSigns = g.Select(x => x.SpecialSigns).FirstOrDefault(), DaysInHospital = g.Sum(x => x.DaysInHospital), Age = g.Select(x => x.Age).FirstOrDefault(), Name = g.Select(x => x.Name).FirstOrDefault(), ApproximateHeight = g.Select(x => x.ApproximateHeight).FirstOrDefault(), ColorHair = g.Select(x => x.ColorHair).FirstOrDefault() }).ToList();

            if (result.Count == 0)
            {
                TempData["messagee"] = "No Patient Found ,Add Patient Please";
                return RedirectToAction(nameof(Index));
            }


            return View(result);
        }
        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.CodePatient == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();

        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Age,Gender,SpecialSigns,ColorHair,ApproximateHeight")] Patient patient)
        {

            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Age,Gender,CodePatient,SpecialSigns,ColorHair,ApproximateHeight")] Patient patient)
        {
            if (id != patient.CodePatient)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.CodePatient))
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
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.CodePatient == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.CodePatient == id);
        }
    }
}
