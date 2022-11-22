using System;
using System.Collections.Generic;

namespace kurs.Models
{
    public partial class PatientInHospitalRoom
    {
        public int NumberHospitalRoom { get; set; }
        public int CodePatient { get; set; }

        public virtual Patient? CodePatientNavigation { get; set; } = null!;
        public virtual HospitalRoom? NumberHospitalRoomNavigation { get; set; } = null!;
    }
}
