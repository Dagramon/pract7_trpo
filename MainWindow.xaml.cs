using System.Text;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pract7_trpo
{
    
    public partial class MainWindow : Window
    {
        Random rng = new Random();
        private Doctor currentDoctor = new Doctor();
        private Doctor registeredDoctor = new Doctor();
        private Patient currentPatient = new Patient();
        private Patient addedPatient = new Patient();
        private Patient foundPatient = new Patient();
        Info info = new Info();
        public MainWindow()
        {
            InitializeComponent();
            InfoForm.DataContext = currentDoctor;
            LoginForm.DataContext = currentDoctor;
            RegForm.DataContext = registeredDoctor;
            AddForm.DataContext = addedPatient;
            FindForm.DataContext = foundPatient;
            DataContext = info;
            UpdateInfo();
        }
        private void UpdateInfo()
        {
            if (File.Exists("IDS.txt") && File.Exists("Patient_IDS.txt"))
            {
                string[] DoctorIDS = File.ReadAllLines("IDS.txt");
                foreach (string ID in DoctorIDS)
                {
                    if (File.Exists($"D_{ID}"))
                    {
                        info.JSONFiles++;
                        info.Doctors++;
                    }
                }
                string[] PatientIDS = File.ReadAllLines("Patient_IDS.txt");
                foreach (string ID in PatientIDS)
                {
                    if (File.Exists($"P_{ID}"))
                    {
                        info.JSONFiles++;
                        info.Patients++;
                    }
                }
            }
            
        }
        private string CreateID()
        {
            if (!File.Exists("IDS.txt"))
            {
                StreamWriter sw1 = new StreamWriter("IDS.txt");
                sw1.Close();
            }
            string[] takenIDS = File.ReadAllLines("IDS.txt");
            int newID;
            bool taken = false;
            do
            {
                newID = rng.Next(10000, 100000);
                foreach (string id in takenIDS)
                {
                    if (Convert.ToInt32(id) == newID)
                    {
                        taken = true;
                    }
                }

            } while (taken == true);

            StreamWriter sw = File.AppendText("IDS.txt");
            sw.WriteLine(newID);
            sw.Close();

            return newID.ToString();
        }
        private string CreatePatientID()
        {
            if (!File.Exists("Patient_IDS.txt"))
            {
                StreamWriter sw1 = new StreamWriter("Patient_IDS.txt");
                sw1.Close();
            }
            string[] takenIDS = File.ReadAllLines("Patient_IDS.txt");
            int newID;
            bool taken = false;
            do
            {
                newID = rng.Next(1000000, 10000000);
                foreach (string id in takenIDS)
                {
                    if (Convert.ToInt32(id) == newID)
                    {
                        taken = true;
                    }
                }

            } while (taken == true);

            StreamWriter sw = File.AppendText("Patient_IDS.txt");
            sw.WriteLine(newID);
            sw.Close();

            return newID.ToString();
        }
        private string GetDoctorByID(string ID)
        {
            if (File.Exists($"D_{ID}"))
            {
                string jsonString = File.ReadAllText($"D_{ID}");
                Doctor doctor = JsonSerializer.Deserialize<Doctor>(jsonString);
                return doctor.Name;
            }

            return "Не найден";
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (registeredDoctor.Name != null &&
                registeredDoctor.LastName != null &&
                registeredDoctor.MiddleName != null &&
                registeredDoctor.Specialization != null &&
                registeredDoctor.Password != null)
            {
                if (registeredDoctor.Password == registeredDoctor.RepeatPassword)
                {
                    string ID = CreateID();
                    registeredDoctor.ID = ID;
                    string jsonString = JsonSerializer.Serialize(registeredDoctor);
                    File.WriteAllText($"D_{ID}", jsonString);
                    MessageBox.Show($"Доктор зарегестрирован с идентификатором {ID}");
                    info.JSONFiles++;
                    info.Doctors++;
                }
                else
                {
                    MessageBox.Show("Пароли не совпадают");
                }
            }
            else
            {
                MessageBox.Show("Все поля должны быть заполнены");
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (currentDoctor.ID != string.Empty)
            {
                if (File.Exists($"D_{currentDoctor.ID}"))
                {
                    string jsonString = File.ReadAllText($"D_{currentDoctor.ID}");
                    Doctor tempDoctor = JsonSerializer.Deserialize<Doctor>(jsonString);
                    if (currentDoctor.Password == tempDoctor.Password)
                    {
                        InfoForm.DataContext = tempDoctor;
                        currentDoctor = tempDoctor;
                    }
                    else
                    {
                        currentDoctor = new Doctor();
                        LoginForm.DataContext = currentDoctor;
                        MessageBox.Show("Неверный пароль");
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не найден");
                }
            }
            else
            {
                MessageBox.Show("Все поля должны быть заполнены");
            }
        }

        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {
            if (currentDoctor.ID != null)
            {
                if (addedPatient.Name != null &&
                    addedPatient.LastName != null &&
                    addedPatient.MiddleName != null &&
                    addedPatient.Birthday != null)
                {
                    addedPatient.LastAppointment = DateTime.Now.Date.ToString("G");
                    addedPatient.LastDoctor = Convert.ToInt32(currentDoctor.ID);
                    string ID = CreatePatientID();
                    addedPatient.LastDoctorName = GetDoctorByID(addedPatient.LastDoctor.ToString());
                    addedPatient.ID = ID;
                    string jsonString = JsonSerializer.Serialize(addedPatient);
                    File.WriteAllText($"P_{ID}", jsonString);
                    MessageBox.Show($"Пациент добавлен с идентификатором {ID}");
                    info.JSONFiles++;
                    info.Patients++;

                    currentPatient = addedPatient;
                    EditPatientForm.DataContext = currentPatient;
                    AppointmentForm.DataContext = currentPatient;
                }

                else
                {
                    MessageBox.Show("Все поля должны быть заполнены");
                }
            }
            else
            {
                MessageBox.Show("Врач должен войти");
            }
        }

        private void EditPatient_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists($"P_{currentPatient.ID}"))
            {
                string jsonString = JsonSerializer.Serialize(currentPatient);
                currentPatient.LastAppointment = DateTime.Today.Date.ToString("G");
                currentPatient.LastDoctorName = GetDoctorByID(addedPatient.LastDoctor.ToString());
                File.WriteAllText($"P_{currentPatient.ID}", jsonString);
                MessageBox.Show($"Данные о пациенте обновлены");
                currentPatient = new Patient();
                AppointmentForm.DataContext = currentPatient;
                EditPatientForm.DataContext = currentPatient;
            }
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentDoctor.ID != null)
            {
                if (File.Exists($"P_{foundPatient.ID}"))
                {
                    string jsonString = File.ReadAllText($"P_{foundPatient.ID}");
                    foundPatient = JsonSerializer.Deserialize<Patient>(jsonString);
                    currentPatient = foundPatient;
                    currentPatient.LastDoctorName = GetDoctorByID(currentPatient.LastDoctor.ToString());
                    AppointmentForm.DataContext = currentPatient;
                    EditPatientForm.DataContext = currentPatient;
                }
                else
                {
                    MessageBox.Show("Пациент не найден");
                }
            }
            else
            {
                MessageBox.Show("Сначала нужно войти");
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (currentDoctor.ID != null && currentPatient.ID != null)
            {
                if (File.Exists($"P_{currentPatient.ID}"))
                {
                    string jsonString = File.ReadAllText($"P_{currentPatient.ID}");
                    currentPatient = JsonSerializer.Deserialize<Patient>(jsonString);
                    AppointmentForm.DataContext = currentPatient;
                    EditPatientForm.DataContext = currentPatient;
                }
            }
        }

    }
}