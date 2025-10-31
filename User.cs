using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace pract7_trpo
{
    class User
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        public string MiddleName { get; set; }
        public string Specialization { get; set; }

        public string Password { get; set; }
        [JsonIgnore]
        public string RepeatPassword { get; set; }
        public string ID { get; set; }
    }
}
