using System;
using System.Collections.Generic;

namespace kurs.Models
{
    public partial class Treatment
    {
        public DateTime ReceiptDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string? DischargeReason { get; set; }
        public string HowDeliveredPatient { get; set; } = null!;
        public int TreatmentCode { get; set; }
        public int CodePatient { get; set; }
        public int NumberDiagnosis { get; set; }

        public virtual Patient? CodePatientNavigation { get; set; } = null!;
        public virtual Diagnosis? NumberDiagnosisNavigation { get; set; } = null!;
    }
}
