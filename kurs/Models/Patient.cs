using System;
using System.Collections.Generic;

namespace kurs.Models
{
    public partial class Patient
    {
        public Patient()
        {
            TransferToAnotherRooms = new HashSet<TransferToAnotherRoom>();
            Treatments = new HashSet<Treatment>();
        }

        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public int CodePatient { get; set; }
        public string? SpecialSigns { get; set; }
        public string? ColorHair { get; set; }
        public int? ApproximateHeight { get; set; }

        public virtual PatientInHospitalRoom? PatientInHospitalRoom { get; set; } = null!;
        public virtual ICollection<TransferToAnotherRoom>? TransferToAnotherRooms { get; set; }
        public virtual ICollection<Treatment>? Treatments { get; set; }
    }
}
