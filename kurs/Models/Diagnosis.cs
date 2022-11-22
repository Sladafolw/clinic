using System;
using System.Collections.Generic;

namespace kurs.Models
{
    public partial class Diagnosis
    {
        public Diagnosis()
        {
            Treatments = new HashSet<Treatment>();
        }

        public int NumberDiagnosis { get; set; }
        public string NameDiagnosis { get; set; } = null!;

        public virtual ICollection<Treatment>? Treatments { get; set; }
    }
}
