using System.Windows;

namespace bicycles
{
    public partial class Registr : Window
    {
        public string Role;

        public Registr()
        {
           InitializeComponent();
        }

        private void AcceptAdd(object sender, RoutedEventArgs e)
        {
            string Name = NameText.Text, MiddleName = MiddleNameText.Text, LastName = LastNameText.Text, Login = LoginText.Text, Password = PasswordText.Text, 
                Post = PostBox.Text == "Менеджер" ? "manager" : "buhgalter";
            string empty = string.Empty;

            if (Name != empty && MiddleName != empty && LastName != empty && Login != empty && Password != empty)
            {
                MySQLItems.RunCommand($"INSERT INTO users (login, password, first_name, last_name, thirdname, role)" +
                    $" VALUES ('{Login}', '{Password}', '{Name}', '{MiddleName}', '{LastName}', '{Post}')");
                Role = Post;
                Close();
            }
            else
                MessageBox.Show("Заполнены не все поля", "Ошибка");
        }
    }
}