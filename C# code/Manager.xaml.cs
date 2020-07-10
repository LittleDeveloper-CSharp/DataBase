using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace bicycles
{
    public partial class Manager : Window
    {
        string textFillTable = "SELECT bicycle_id as 'Номер', producer as 'Производитель', model as 'Модель', classname as 'Класс', trunk as 'Наличие багажника'," +
                                    " wheel_width as 'Диаметр колес', road_transmission as 'Дорожная трансмиссия', price_per_day as 'Стоимость за сутки', start_price as 'Начальная цена'," +
                                    " available as 'Наличие', expiration as 'Кол-во использования', current_price as 'Цена' FROM bicycles" +
                                    " join classes on classes.class = bicycles.class";
        public Manager()
        {
            InitializeComponent();

            ComboBox[] boxes = new ComboBox[2] { ProisvodBox, TarivBox };
            foreach (ComboBox box in boxes)
                FillBox(box);
        }

        void FillBox(ComboBox box)
        {

            string textCommand = $"SELECT DISTINCT {box.Tag} FROM bicycles";
            MySQLItems.RunCommand(textCommand);
            for (int i = 0; i < MySQLItems.table.Rows.Count; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = MySQLItems.table.Rows[i][0].ToString();
                box.Items.Add(item);
            }
        }

        private void AcceptFilter(object sender, RoutedEventArgs e)
        {
            string textCommand = textFillTable;

            foreach (ComboBox box in PanelBox.Children.OfType<ComboBox>())
            {
                if (box.SelectedItem != null)
                {
                    ComboBoxItem item = (ComboBoxItem)box.SelectedItem;
                    string textItem = (string)item.Content;
                    if (box.Tag.ToString() == "road_transmission" || box.Tag.ToString() == "trunk")
                        textItem = textItem == "Да" ? "1" : "0";

                    if (textCommand.IndexOf("WHERE") == -1)
                        textCommand += $" WHERE {box.Tag} = '{textItem}'";
                    else
                        textCommand += $" AND {box.Tag} = '{textItem}'";
                }
            }

            FillDataGrid(textCommand);
        }

        void FillDataGrid(string textCommand)
        {
            MySQLItems.RunCommand(textCommand);
            infoTable.ItemsSource = MySQLItems.table.DefaultView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (ComboBox box in PanelBox.Children.OfType<ComboBox>())
                box.Text = "";

            FillDataGrid(textFillTable);
        }


        string idBicycle, Price, Deposit;

        private void NewArenda(object sender, RoutedEventArgs e)
        {
            if (infoTable.SelectedIndex != -1)
            {
                string textCommand = $"SELECT deposit FROM classes where classname = '{Deposit}'";
                MySQLItems.RunCommand(textCommand);
                Deposit = MySQLItems.table.Rows[0][0].ToString();
                NewArenda arenda = new NewArenda(idBicycle, Price, Deposit);
                arenda.ShowDialog();
            }
            else
                MessageBox.Show("Значение не выбрано");
        }

        private void infoTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataRowView row1 = (DataRowView)infoTable.SelectedItems[0];
            idBicycle = row1[0].ToString();
            Price = row1[7].ToString();
            Deposit = row1[3].ToString();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}