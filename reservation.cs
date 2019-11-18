using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Management_System
{
    public class reservation
    {
        public int id, slot;
        public KeyValuePair<int, string> patient, secretary;
        public DateTime visit_date, date;

        public reservation(int id, int patient_id, string patient_name, int secretary_id, string secretary_name, int slot, DateTime visit_date, DateTime date)
        {
            this.id = id;
            patient = new KeyValuePair<int, string>(patient_id, patient_name);
            secretary = new KeyValuePair<int, string>(secretary_id, secretary_name);
            this.visit_date = visit_date;
            this.date = date;
            this.slot = slot;
        }

        public override string ToString()
        {
            return id.ToString() + ":" + patient.Value + ":" + visit_date.Date.ToString() + " => " + utils.getSlots()[slot];
        }

    }
}
