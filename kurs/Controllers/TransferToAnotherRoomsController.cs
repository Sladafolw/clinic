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
    public class TransferToAnotherRoomsController : Controller
    {
        private readonly ClinicContext _context;

        public TransferToAnotherRoomsController(ClinicContext context)
        {
            _context = context;
        }

        // GET: TransferToAnotherRooms
        public async Task<IActionResult> Index()
        {
           
            dynamic tasks = (from c in _context.TransferToAnotherRooms
                             join p in _context.Patients on c.CodePatient equals p.CodePatient
                             join o in _context.PatientInHospitalRooms on p.CodePatient equals o.CodePatient
                             select new { Name = p.Name, Date = c.Date, CodeTransfer = c.CodeTransfer, CodePatient = c.CodePatient, NumberHospitalRoom = c.NumberHospitalRoom }).AsEnumerable().Select(c => c.ToExpando()); ;
          
            ViewBag.a = TempData["messagee"];

            return View(  tasks);
        }

        // GET: TransferToAnotherRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            dynamic tasks = (from p in _context.Patients
                             join c in _context.TransferToAnotherRooms on p.CodePatient equals c.CodePatient
                             join o in _context.PatientInHospitalRooms on p.CodePatient equals o.CodePatient
                             where(c.CodeTransfer==id)
                             select new { Name = p.Name, Date = c.Date, CodeTransfer=c.CodeTransfer,CodePatient = c.CodePatient, NumberHospitalRoom = c.NumberHospitalRoom }).AsEnumerable().Select(c => c.ToExpando());

            return View(tasks);
        }

        // GET: TransferToAnotherRooms/Create
        public IActionResult Create()
        {
            dynamic patient = (from p in _context.Patients
                               join l in _context.PatientInHospitalRooms on p.CodePatient equals l.CodePatient
                               select new { Name = p.Name, Age = p.Age, ColorHair=p.ColorHair, SpecialSigns=p.SpecialSigns, CodePatient = p.CodePatient }).AsEnumerable().Select(c => c.ToExpando());
            var Room = (from o in _context.HospitalRooms
                        select new { NumberHospitalRoom = o.NumberHospitalRoom }).AsEnumerable().ToList();

            List<int> NumberHospitalRoom = new List<int>();
            Dictionary<int, string> Patients = new Dictionary<int, string>();
            foreach (var r in Room)
            { NumberHospitalRoom.Add(r.NumberHospitalRoom); }

            foreach (dynamic item in patient)
            {
                string? add = ("Name=" + item.Name + " Age=" + (item.Age ?? "none") + " ColorHair=" + (item.ColorHair ?? "none") + " SpecialSigns = " + (item.SpecialSigns ?? "none"));
                Patients.Add(item.CodePatient, add);
               


            }
            ViewBag.Patients = Patients;
            ViewBag.NumberHospitalRoom = NumberHospitalRoom;

            return View();
        }

        // POST: TransferToAnotherRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodePatient,NumberHospitalRoom,Date,CodeTransfer")] TransferToAnotherRoom transferToAnotherRoom)
        {
           
           int code= transferToAnotherRoom.CodePatient;
      
         
            if (ModelState.IsValid)
            {
                
                _context.Add(transferToAnotherRoom);

                var patient = _context.PatientInHospitalRooms.First(m => m.CodePatient == code);

                patient.NumberHospitalRoom = transferToAnotherRoom.NumberHospitalRoom;
                _context.Update(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transferToAnotherRoom);
        }



        // GET: TransferToAnotherRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferToAnotherRoom = await _context.TransferToAnotherRooms
                .Include(t => t.CodePatientNavigation)
                .Include(t => t.NumberHospitalRoomNavigation)
                .FirstOrDefaultAsync(m => m.CodeTransfer == id);
            if (transferToAnotherRoom == null)
            {
                return NotFound();
            }

            return View(transferToAnotherRoom);
        }

        // POST: TransferToAnotherRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transferToAnotherRoom = await _context.TransferToAnotherRooms.FindAsync(id);
            _context.TransferToAnotherRooms.Remove(transferToAnotherRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransferToAnotherRoomExists(int id)
        {
            return _context.TransferToAnotherRooms.Any(e => e.CodePatient == id);
        }
    }
}
