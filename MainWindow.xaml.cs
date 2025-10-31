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
        private User registeredUser = new User();
        public MainWindow()
        {
            InitializeComponent();
            InfoForm.DataContext = currentUser;
            LoginForm.DataContext = currentUser;
            RegForm.DataContext = registeredUser;
        }
        private int CreateID()
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

            return newID;
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (registeredUser.Name != string.Empty &&
                registeredUser.LastName != string.Empty &&
                registeredUser.MiddleName != string.Empty &&
                registeredUser.Specialization != string.Empty &&
                registeredUser.Password != string.Empty)
            {
                if (registeredUser.Password == registeredUser.RepeatPassword)
                {
                    int ID = CreateID();
                    string jsonString = JsonSerializer.Serialize(registeredUser);
                    File.WriteAllText($"D_{ID}", jsonString);
                    MessageBox.Show($"Пользователь зарегестрирован с идентификатором {ID}");
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

            if (currentUser.ID != string.Empty)
            {
                if (File.Exists($"D_{currentUser.ID}"))
                {
                    string jsonString = File.ReadAllText($"D_{currentUser.ID}");
                    User tempUser = JsonSerializer.Deserialize<User>(jsonString);
                    MessageBox.Show(tempUser.Name);
                    if (currentUser.Password == tempUser.Password)
                    {
                        InfoForm.DataContext = tempUser;
                    }
                    else
                    {
                        currentUser = new User();
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
    }
}