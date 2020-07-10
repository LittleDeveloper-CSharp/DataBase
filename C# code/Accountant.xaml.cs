using System;
using System.Data;
using System.Windows;
using System.Collections.Generic;

namespace bicycles
{
    public partial class Accountant : Window
    {
        public Accountant()
        {
            InitializeComponent();
        }

        private void OutInfo(object sender, RoutedEventArgs e)
        {
            DateTime startTime, endTime;

            if (DateTime.TryParse(DateStartBox.Text, out startTime) && DateTime.TryParse(DateEndBox.Text, out endTime))
            {
                string textCommand = "SELECT classname as 'Наименование класса', SUM(rent) as 'Сумма аренды' from bicycles"+
                                    " JOIN offers ON bicycle = bicycle_id"+
                                    $" JOIN classes ON classes.class = bicycles.class where start_date >= '{startTime:yyyy-MM-dd}' and end_date <= '{endTime:yyyy-MM-dd}' group by classname";
                MySQLItems.RunCommand(textCommand);
                InfoRating.ItemsSource = MySQLItems.table.DefaultView;
            }
            else
                MessageBox.Show("Некорректные значения");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void Armotiz(object sender, RoutedEventArgs e)
        {
            string textCommand = "SELECT bicycle, start_price, expiration, start_date, end_date FROM offers"+
                                    " JOIN bicycles ON bicycle_id = bicycle order by bicycle";
            MySQLItems.RunCommand(textCommand);
            int StartPrice, Expiration, idBicyle;
            DateTime StartDate, EndDate;
            int CountDay = 0;
            DataTable data = MySQLItems.table;

            for (int Row = 0; Row < data.Rows.Count; Row++)
            {
                List<int> DateMas = new List<int>();

                int NormalA, Depreciation, CostWithWears, idBicyleNext, CurrentPrice;
                bool SeveralItem = false;

                idBicyle = int.Parse(data.Rows[Row][0].ToString());
                idBicyleNext = int.Parse(data.Rows[Row + 1][0].ToString());

                StartPrice = int.Parse(data.Rows[Row][1].ToString());
                Expiration = int.Parse(data.Rows[Row][2].ToString());

                StartDate = DateTime.Parse(data.Rows[Row][3].ToString());
                EndDate = DateTime.Parse(data.Rows[Row][4].ToString());

                while (idBicyle == idBicyleNext && Row != data.Rows.Count - 1) 
                {

                    idBicyle = int.Parse(data.Rows[Row][0].ToString());
                    idBicyleNext = int.Parse(data.Rows[Row + 1][0].ToString());

                    Row++;

                    DateMas.Add(Convert.ToInt32((DateTime.Parse(data.Rows[Row][4].ToString()) - DateTime.Parse(data.Rows[Row][3].ToString())).TotalDays));

                    SeveralItem = true;
                }


                if (!SeveralItem)
                    CountDay = Convert.ToInt32((EndDate - StartDate).TotalDays);
                else
                    foreach (int i in DateMas)
                        CountDay += i;
                NormalA = StartPrice / Expiration;
                Depreciation = CountDay * NormalA;
                CostWithWears = StartPrice - Depreciation;

                textCommand = $"SELECT current_price FROM bicycles.bicycles Where bicycle_id = {idBicyle}";
                MySQLItems.RunCommand(textCommand);
                CurrentPrice = int.Parse(MySQLItems.table.Rows[0][0].ToString());

                if (CostWithWears < CurrentPrice && Row ==  data.Rows.Count - 1)
                    MessageBox.Show($"{CostWithWears} < {CurrentPrice}");
            }

            InfoBicycle(null, null);
        }

        private void InfoBicycle(object sender, RoutedEventArgs e)
        {
            string textCommand = "SELECT producer, model, classname, trunk, wheel_width, road_transmission, start_price, current_price FROM bicycles.bicycles" +
                                " JOIN classes ON classes.class = bicycles.class";
            FillTable(textCommand);
        }

        void FillTable(string textCommand)
        {
            MySQLItems.RunCommand(textCommand);
            infoRazt.ItemsSource = MySQLItems.table.DefaultView;
        }
    }       
}