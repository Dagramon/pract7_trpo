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
        private User currentUser = new User();
        public MainWindow()
        {
            InitializeComponent();
        }
        private int CreateID()
        {
            if (!File.Exists("IDS.txt"))
            {
                File.CreateText("IDS.txt");
            }
            string[] takenIDS = File.ReadAllLines("IDS.txt");
            int newID;
            bool taken = false;
            do
            {
                newID = rng.Next(10000, 100000);
                foreach (string id in takenIDS)
                {
                    if (Convert.ToInt16(id) == newID)
                    {
                        taken = true;
                    }
                }

            } while (taken == true);

            StreamWriter sw = new StreamWriter("IDS.txt");
            sw.WriteLine(newID);

            return newID;
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            User registeredUser = new User();
            DataContext = registeredUser;
            if (registeredUser.Name != string.Empty &&
                registeredUser.LastName != string.Empty &&
                registeredUser.MiddleName != string.Empty &&
                registeredUser.Specialization != string.Empty &&
                registeredUser.Password != string.Empty &&
                registeredUser.Password == RepeatPass.Text)
            {
                int ID = CreateID();
                string jsonString = JsonSerializer.Serialize(registeredUser);
                File.WriteAllText($"D_{ID}", jsonString);
                MessageBox.Show($"Пользователь зарегестрирован с идентификатором {ID}");
            }
            else
            {
                MessageBox.Show("Все поля должны быть заполнены");
            }

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            User currentUser = new User();
            
        }
    }
}