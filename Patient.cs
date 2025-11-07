using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace pract7_trpo
{
    class Patient
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        public string MiddleName { get; set; }
        public string Birthday { get; set; }
        public string LastAppointment { get; set; }
        public int LastDoctor { get; set; }
        public string Diagnosis { get; set; }
        public string Recomendations { get; set; }
        public string ID { get; set; }

        [JsonIgnore]
        public string LastDoctorName { get; set; }

    }
}
