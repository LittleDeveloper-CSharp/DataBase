using System.Data;
using MySql.Data.MySqlClient;

namespace bicycles
{
    internal class MySQLItems
    {
        public static MySqlConnection connection = new MySqlConnection("server = localhost; port = 3306; username = root; password = 1234; database = bicycles");
        public static MySqlDataAdapter adapter = new MySqlDataAdapter();
        public static MySqlCommand command = new MySqlCommand();
        public static DataTable table;

        public static void RunCommand(string textCommand)
        {
            table = new DataTable();

            command.CommandText = textCommand;
            command.Connection = connection;

            if (textCommand[0] == 'S')
            {
                adapter.SelectCommand = command;
                adapter.Fill(table);
            }
            else
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
