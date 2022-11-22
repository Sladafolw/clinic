using System;
using System.Collections.Generic;

namespace kurs.Models
{
    public partial class TransferToAnotherRoom
    {
        public int NumberHospitalRoom { get; set; }
        public DateTime Date { get; set; }
        public int CodePatient { get; set; }
        public int CodeTransfer { get; set; }
        public virtual Patient? CodePatientNavigation { get; set; } = null!;
        public virtual HospitalRoom? NumberHospitalRoomNavigation { get; set; } = null!;
    }
}
