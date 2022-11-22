
using Microsoft.AspNetCore.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

using Microsoft.EntityFrameworkCore;
using kurs.Models;
using System.Text;
using Microsoft.Office.Interop.Word;
using TemplateEngine.Docx;
using System.IO;
using System;
namespace kurs.Controllers
{
    public class Print : Controller
    {
        private readonly ClinicContext _context;

        public Print(ClinicContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> WordIndex()
        {
            dynamic tasks = (from r in _context.Treatments
                             join p in _context.PatientInHospitalRooms on r.CodePatient equals p.CodePatient
                             join c in _context.Patients on r.CodePatient equals c.CodePatient
                             join m in _context.Diagnoses on r.NumberDiagnosis equals m.NumberDiagnosis
                             where (r.DischargeDate != null)
                             select new { Name = c.Name, Date = r.ReceiptDate, Age = c.Age, Diagnos = m.NameDiagnosis, SpecialSigns = c.SpecialSigns, ColorHair = c.ColorHair, TreatmentCode = r.TreatmentCode }).AsEnumerable().Select(c => c.ToExpando());


            List<string> Patients = new List<string>();
            List<string> TreatmentCode = new List<string>();


            foreach (dynamic item in tasks)
            {
                string? add = ("Diagnos=" + item.Diagnos + " Date=" + item.Date.ToShortDateString() + " Name=" + item.Name + " Age=" + (item.Age ?? "none") + " ColorHair=" + (item.ColorHair ?? "none") + " SpecialSigns = " + (item.SpecialSigns ?? "none"));
                Patients.Add(add);
                TreatmentCode.Add(item.TreatmentCode.ToString());
            }
            ViewBag.Patients = Patients;
            ViewBag.CodePatient = TreatmentCode;
            return View();
        }
        public async Task<IActionResult> Excel()
        {
            var tasks = await (from p in _context.Patients
                               join c in _context.PatientInHospitalRooms on p.CodePatient equals c.CodePatient into room
                               from patientInHospitalRoom in room.DefaultIfEmpty()
                               join o in _context.Treatments on patientInHospitalRoom.CodePatient equals o.CodePatient into treatment
                               from patientTreatment in treatment.DefaultIfEmpty()
                               join l in _context.Diagnoses on patientTreatment.NumberDiagnosis equals l.NumberDiagnosis into diagnosis
                               from diagnos in diagnosis.DefaultIfEmpty()
                               select new
                               {
                                   Name = p.Name,
                                   Diagnos = diagnos.NameDiagnosis != null ? diagnos.NameDiagnosis.ToString() : String.Empty,

                                   NumberRoom = patientInHospitalRoom != null ? patientInHospitalRoom.NumberHospitalRoom.ToString() : string.Empty,
                                   Age = p.Age
                               }).ToListAsync();
            int count = tasks.Count();
            var excelApp = new Excel.Application();
            excelApp.Visible = true;
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            workSheet.Cells[1, 1] = "name";
            workSheet.Cells[1, 2] = "Diagnos";
            workSheet.Cells[1, 3] = "Age";

            int i = 2;
            foreach (var task in tasks)
            {
                workSheet.Cells[i, 1] = task.Name;
                workSheet.Cells[i, 2] = task.Diagnos;
                workSheet.Cells[i, 3] = task.Age.ToString();



                i++;
            } ((Excel.Range)workSheet.Rows[1]).Font.Bold = true;
            ((Excel.Range)workSheet.Columns[1]).AutoFit();
            ((Excel.Range)workSheet.Columns[2]).AutoFit();
            ((Excel.Range)workSheet.Columns[3]).AutoFit();
            workSheet.PrintOutEx();
            return View(tasks);
        }
        public async Task<IActionResult> Word([Bind("TreatmentCodee")] ForTreament TreatmentCode)
        {


            var a = TreatmentCode.TreatmentCodee;
            Word._Document document;
            Word._Application application;
            application = new Word.Application();

            FileInfo fileInfo = new FileInfo("wwwroot/Выписка.docx");
            FileInfo fileInfo2 = new FileInfo("wwwroot/Выписка2.docx");
            string? name = fileInfo2.FullName;
            fileInfo2.Delete();
            fileInfo.CopyTo("wwwroot/Выписка2.docx");
            var tasks = (from l in _context.Treatments
                         join c in _context.PatientInHospitalRooms on l.CodePatient equals c.CodePatient
                         join o in _context.HospitalRooms on c.NumberHospitalRoom equals o.NumberHospitalRoom
                         join p in _context.Patients on l.CodePatient equals p.CodePatient
                         join e in _context.Diagnoses on l.NumberDiagnosis equals e.NumberDiagnosis
                         where (l.TreatmentCode == (Convert.ToInt32(a)))
                         select new { Name = p.Name, DateR = l.ReceiptDate, DateD = l.DischargeDate, Gender = p.Gender, Diagnos = e.NameDiagnosis, NumberRoom = c.NumberHospitalRoom, Age = p.Age, Phone = o.MobileNumberHospitalRoom }).AsEnumerable().ToList();
            var content = new Content();
            foreach (var t in tasks)
            {

                var alias = "Name";
                var value = t.Name.ToString();
                content.Fields.Add(new FieldContent(alias, value));
                var alias2 = "Text2";
                var value2 = t.Gender;
                content.Fields.Add(new FieldContent(alias2, value2));
                var alias3 = "Text3";
                var value3 = t.Diagnos;
                content.Fields.Add(new FieldContent(alias3, value3));
                var alias4 = "Time";
                var value4 = DateTime.Now.ToString();
                content.Fields.Add(new FieldContent(alias4, value4));
                var alias5 = "DateR";
                var value5 = t.DateR.ToShortDateString();
                content.Fields.Add(new FieldContent(alias5, value5));
                var alias6 = "DateD";
                var value6 = t.DateD?.ToShortDateString();
                content.Fields.Add(new FieldContent(alias6, value6));
            }

            using (var outputDocument = new
TemplateProcessor("wwwroot/Выписка2.docx").SetRemoveContentControls(true))
            {


                outputDocument.FillContent(content);
                outputDocument.SaveChanges();
            }
            document = application.Documents.Add(name);
            application.Visible = true;

            document.PrintOut();
            return View(tasks);
        }
    }
}
