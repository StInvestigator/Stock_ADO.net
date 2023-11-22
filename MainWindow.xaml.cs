using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Stock_ADO.net.DBEntity;

namespace Stock_ADO.net
{
    namespace DBEntity
    {
        public class DatabaseManager
        {
            public string ConnectionString { get; set; }

            private SqlConnection sqlConnection { get; set; }

            public DatabaseManager(string connectionString)
            {
                ConnectionString = connectionString;
                sqlConnection = new SqlConnection(ConnectionString);
            }
            public bool OpenConnection()
            {
                try
                {
                    sqlConnection.Open();
                    return true;
                }
                catch (SqlException sql_ex)
                {
                    throw new Exception(sql_ex.Message);
                }
            }
            public bool CloseConnection()
            {
                try
                {
                    sqlConnection.Close();
                    return true;
                }
                catch (SqlException sql_ex)
                {
                    throw new Exception(sql_ex.Message);
                }
            }
            public void ExecuteNonQuery(string query)
            {
                if (sqlConnection.State != System.Data.ConnectionState.Open)
                {
                    OpenConnection();
                }
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                CloseConnection();
            }
            public List<NPCforString> ExecuteReaderString(string query)
            {
                List<NPCforString> result = new List<NPCforString>();
                using (SqlConnection _sqlConnection = new SqlConnection(ConnectionString))
                {
                    if (_sqlConnection.State == System.Data.ConnectionState.Closed)
                    {
                        _sqlConnection.OpenAsync();
                    }
                    SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new NPCforString(reader.GetString(0)));
                        }
                    }
                }
                CloseConnection();
                return result;
            }
            public int ExecuteReaderInt(string query)
            {
                int result = 0;
                using (SqlConnection _sqlConnection = new SqlConnection(ConnectionString))
                {
                    if (_sqlConnection.State == System.Data.ConnectionState.Closed)
                    {
                        _sqlConnection.OpenAsync();
                    }
                    SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = reader.GetInt32(0);
                        }
                    }
                }
                CloseConnection();
                return result;
            }

            public List<NPCforInt> ExecuteReaderIntList(string query)
            {
                List<NPCforInt> result = new List<NPCforInt>();
                using (SqlConnection _sqlConnection = new SqlConnection(ConnectionString))
                {
                    if (_sqlConnection.State == System.Data.ConnectionState.Closed)
                    {
                        _sqlConnection.OpenAsync();
                    }
                    SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new NPCforInt(reader.GetInt32(0)));
                        }
                    }
                }
                CloseConnection();
                return result;
            }

            public List<Good> ExecuteReaderGoods(string query)
            {
                List<Good> result = new List<Good>();
                using (SqlConnection _sqlConnection = new SqlConnection(ConnectionString))
                {
                    if (_sqlConnection.State == System.Data.ConnectionState.Closed)
                    {
                        _sqlConnection.OpenAsync();
                    }
                    SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Good(reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4),reader.GetDateTime(5)));
                        }
                    }
                }
                CloseConnection();
                return result;
            }
        }
    }
    public partial class MainWindow : Window
    {
        DatabaseManager database;
        public MainWindow()
        {
            InitializeComponent();
            database = new DatabaseManager(@"Data Source=DESKTOP-OF66R01\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            if (database.OpenConnection())
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Error");
            }
        }
    }
}
