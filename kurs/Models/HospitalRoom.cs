using System;
using System.Collections.Generic;

namespace kurs.Models
{
    public partial class HospitalRoom
    {
        public HospitalRoom()
        {
            PatientInHospitalRooms = new HashSet<PatientInHospitalRoom>();
            TransferToAnotherRooms = new HashSet<TransferToAnotherRoom>();
        }

        public int NumberHospitalRoom { get; set; }
        public int MobileNumberHospitalRoom { get; set; }

        public virtual ICollection<PatientInHospitalRoom>? PatientInHospitalRooms { get; set; }
        public virtual ICollection<TransferToAnotherRoom> ?TransferToAnotherRooms { get; set; }
    }
}
