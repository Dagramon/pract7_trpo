using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace pract7_trpo
{
    class Info : INotifyPropertyChanged
    {
        private int jsonfiles { get; set ; }
        private int patients { get; set; }
        private int doctors { get; set; }

        public int JSONFiles
        {
            get { return jsonfiles; }
            set { jsonfiles = value; OnPropertyChanged() ; }
        }
        public int Patients
        {
            get { return patients; }
            set { patients = value; OnPropertyChanged(); }
        }
        public int Doctors
        {
            get { return doctors; }
            set { doctors = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs (propname));
        }
    }
}
