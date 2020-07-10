using System.Data;
using System.Windows;

namespace bicycles
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MySQLItems.RunCommand("SELECT login, password, role, user_id FROM users");
        }

        private void OpenForm(object sender, RoutedEventArgs e)
        {
            string role = string.Empty;

            foreach (DataRow row in MySQLItems.table.Rows)
            {
                if (row[0].ToString() == LoginText.Text)
                    if (row[1].ToString() == PasswordText.Text)
                    {
                        role = row[2].ToString();
                        NewArenda.idManager = row[3].ToString();
                        break;
                    }
            }

            if (role != string.Empty)
                OpenForm(role);
            else
                MessageBox.Show("Пароль не верен! Обратитесь к администратору");
        }

        private void OpenForm(string role)
        {
            switch (role)
            {
                case "buhgalter":
                    Accountant accountant = new Accountant();
                    Hide();
                    accountant.ShowDialog();
                    break;

                case "manager":
                    Manager manager = new Manager();
                    Hide();
                    manager.ShowDialog();
                    break;
            }
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Registr registr = new Registr();
            
            registr.ShowDialog();
            OpenForm(registr.Role);
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}