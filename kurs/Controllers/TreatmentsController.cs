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
    public class TreatmentsController : Controller
    {
        private readonly ClinicContext _context;

        public TreatmentsController(ClinicContext context)
        {
            _context = context;
        }

        // GET: Treatments
        public async Task<IActionResult> Index()
        {
            dynamic tasks = (from p in _context.Patients
                             join c in _context.Treatments on p.CodePatient equals c.CodePatient
                             join o in _context.Diagnoses on c.NumberDiagnosis equals o.NumberDiagnosis
                             select new { Name = p.Name, NumberDiagnosis = o.NameDiagnosis, TreatmentCode = c.TreatmentCode, CodePatient = c.CodePatient, HowDeliveredPatient = c.HowDeliveredPatient, ReceiptDate = c.ReceiptDate, DischargeDate = c.DischargeDate, DischargeReason = c.DischargeReason }).AsEnumerable().Select(c => c.ToExpando()); ;

            return View(tasks);
        }

        // GET: Treatments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            dynamic tasks = (from p in _context.Patients
                             join c in _context.Treatments on p.CodePatient equals c.CodePatient
                             join o in _context.Diagnoses on c.NumberDiagnosis equals o.NumberDiagnosis
                             where (id == c.TreatmentCode)
                             select new { Name = p.Name, TreatmentCode = c.TreatmentCode, NumberDiagnosis = o.NameDiagnosis, CodePatient = c.CodePatient, HowDeliveredPatient = c.HowDeliveredPatient, ReceiptDate = c.ReceiptDate, DischargeDate = c.DischargeDate, DischargeReason = c.DischargeReason }).AsEnumerable().Select(c => c.ToExpando()); ;

            return View(tasks);
        }

        // GET: Treatments/Create
        public IActionResult Create()
        {
            dynamic tasks = (from c in _context.Patients
                             join p in _context.PatientInHospitalRooms on c.CodePatient equals p.CodePatient
                             select new { Name = c.Name, Age = c.Age, SpecialSigns = c.SpecialSigns, ColorHair = c.ColorHair, CodePatient = c.CodePatient }).AsEnumerable().Select(c => c.ToExpando());
            dynamic tasks2 = (from o in _context.Diagnoses
                              select new { NameDiagnosis = o.NameDiagnosis }).AsEnumerable().Select(c => c.ToExpando());
            var a = tasks;
            Dictionary<int, string> Patients = new Dictionary<int, string>();
            List<string> NameDiagnosis = new List<string>();
            foreach (dynamic item in tasks)
            {
                string? add = ("Name=" + item.Name + " Age=" + (item.Age ?? "none") + " ColorHair=" + (item.ColorHair ?? "none") + " SpecialSigns = " + (item.SpecialSigns ?? "none"));
                Patients.Add(item.CodePatient, add);
            }
            foreach (dynamic task in tasks2)
            {
                NameDiagnosis.Add(task.NameDiagnosis);
            }
            ViewBag.Patients = Patients;
            ViewBag.NameDiagnosis = NameDiagnosis;
            return View();
        }

        // POST: Treatments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,NameDiagnosis,CodePatient,ReceiptDate,DischargeDate,HowDeliveredPatient,DischargeReason,")] ForTreament treatment)
        {
            if (ModelState.IsValid)
            {
                Treatment treatment1 = new Treatment();
                treatment1.HowDeliveredPatient = treatment.HowDeliveredPatient;
                treatment1.DischargeReason = treatment.DischargeReason;
                treatment1.DischargeDate = treatment.DischargeDate;
                treatment1.ReceiptDate = treatment.ReceiptDate;
                treatment1.CodePatient = treatment.CodePatient;
                string? af = null;
                string? af2 = null;
                af = Convert.ToString(treatment.NameDiagnosis);//null
                af2 = Convert.ToString(treatment.Name);//null
                var a = (from ar in _context.Diagnoses

                         where (ar.NameDiagnosis == af)
                         select new { NumberDiagnosis = ar.NumberDiagnosis }).ToList();
                foreach (var item in a)
                { treatment1.NumberDiagnosis = item.NumberDiagnosis; }

               
                _context.Add(treatment1);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else { return RedirectToAction(nameof(Index)); }
          
        }

        // GET: Treatments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            dynamic tasks = (from p in _context.Patients
                             join c in _context.Treatments on p.CodePatient equals c.CodePatient
                             join o in _context.Diagnoses on c.NumberDiagnosis equals o.NumberDiagnosis
                             where (c.TreatmentCode == id)
                             select new { Name = p.Name, TreatmentCode = c.TreatmentCode, NumberDiagnosis = o.NameDiagnosis, CodePatient = c.CodePatient, HowDeliveredPatient = c.HowDeliveredPatient, ReceiptDate = c.ReceiptDate, DischargeDate = c.DischargeDate, DischargeReason = c.DischargeReason }).AsEnumerable().Select(c => c.ToExpando()); ;

            dynamic tasks2 = (from o in _context.Diagnoses
                              select new { NameDiagnosis = o.NameDiagnosis }).AsEnumerable().ToList();
            List<string> NameDiagnosis = new List<string>();



            foreach (dynamic task in tasks2)
            {
                NameDiagnosis.Add(task.NameDiagnosis);
            }
            foreach(dynamic d in tasks)
            {
               
                string a = d.NumberDiagnosis;
               int index= NameDiagnosis.IndexOf(a);
                NameDiagnosis.RemoveAt(index);
                NameDiagnosis.Insert(0, a);

            }
            foreach (dynamic task in tasks)
            {
                ViewBag.Name = task.Name;
                ViewBag.HowDeliveredPatient = task.HowDeliveredPatient;
                ViewBag.ReceiptDate = task.ReceiptDate;
                ViewBag.DischargeDate = task.DischargeDate;
                ViewBag.DischargeReason = task.DischargeReason;
                ViewBag.TreatmentCode = task.TreatmentCode;

            }

            ViewBag.NameDiagnosis = NameDiagnosis;

            return View(tasks);
        }

        // POST: Treatments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,TreatmentCode,NameDiagnosis,ReceiptDate,DischargeDate,HowDeliveredPatient,DischargeReason,")] ForTreament treatment)
        {
            if (ModelState.IsValid)
            {
                Treatment treatment1 = new Treatment();
                treatment1.HowDeliveredPatient = treatment.HowDeliveredPatient;
                treatment1.DischargeReason = treatment.DischargeReason;
                treatment1.DischargeDate = treatment.DischargeDate;
                treatment1.ReceiptDate = treatment.ReceiptDate;
                treatment1.TreatmentCode = treatment.TreatmentCode;
                string? af = null;
                string? af2 = null;
                af = Convert.ToString(treatment.NameDiagnosis);//null
                af2 = Convert.ToString(treatment.Name);
                var a = (from ar in _context.Diagnoses
                         where (ar.NameDiagnosis == af)
                         select new { NumberDiagnosis = ar.NumberDiagnosis }).ToList();
                foreach (var item in a)
                { treatment1.NumberDiagnosis = item.NumberDiagnosis; }

                var a2 = (from ar in _context.Patients
                          where (ar.Name == af2)
                          select new { CodePatient = ar.CodePatient }).ToList();
                foreach (var item in a2)
                { treatment1.CodePatient = item.CodePatient; }



                _context.Update(treatment1);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View();


        }

        // GET: Treatments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id == null) { return View(); }
            var treatment = await _context.Treatments.FindAsync(id);


            return View(treatment);

        }

        // POST: Treatments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatment = await _context.Treatments.FindAsync(id);
            _context.Treatments.Remove(treatment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreatmentExists(int id)
        {
            return _context.Treatments.Any(e => e.TreatmentCode == id);
        }
    }
}
