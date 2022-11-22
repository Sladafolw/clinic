namespace kurs.Models
{
    public class ForTreament
    {
        public DateTime ReceiptDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string? DischargeReason { get; set; }
        public string HowDeliveredPatient { get; set; } = null!;
        public string NameDiagnosis { get; set; }
        public int TreatmentCode { get; set; }
        public string? Name { get; set; }
        public string ? TreatmentCodee { get; set; }
        public int CodePatient { get; set; }

    }
}
