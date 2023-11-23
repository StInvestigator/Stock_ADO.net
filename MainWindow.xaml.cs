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
                using (SqlConnection _sqlConnection = new SqlConnection(ConnectionString))
                {
                    if (_sqlConnection.State == System.Data.ConnectionState.Closed)
                    {
                        _sqlConnection.OpenAsync();
                    }
                    SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                }
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
            //public int ExecuteReaderInt(string query)
            //{
            //    int result = 0;
            //    using (SqlConnection _sqlConnection = new SqlConnection(ConnectionString))
            //    {
            //        if (_sqlConnection.State == System.Data.ConnectionState.Closed)
            //        {
            //            _sqlConnection.OpenAsync();
            //        }
            //        SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);
            //        using (SqlDataReader reader = sqlCommand.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                result = reader.GetInt32(0);
            //            }
            //        }
            //    }
            //    CloseConnection();
            //    return result;
            //}

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
                            result.Add(new Good(reader.GetString(1), reader.GetString(8), reader.GetString(10), reader.GetInt32(4), reader.GetSqlMoney(5), reader.GetDateTime(6)));
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

        private void BMain_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CBMain.Text[0] == 'P')
                {
                    if (CBMain.Text[1] == '1')
                    {
                        if (CBMain.Text[3] == '1')
                        {
                            List<Good> result = database.ExecuteReaderGoods("select * from Goods g join GoodTypes gt on g.TypeId=gt.Id join Providers p on p.Id = g.ProviderId");
                            DGMain.ItemsSource = result;
                        }
                        else if (CBMain.Text[3] == '2')
                        {
                            List<NPCforString> result = database.ExecuteReaderString("select Name from GoodTypes");
                            DGMain.ItemsSource = result;
                        }
                        else if (CBMain.Text[3] == '3')
                        {
                            List<NPCforString> result = database.ExecuteReaderString("select Name from Providers");
                            DGMain.ItemsSource = result;
                        }
                        else if (CBMain.Text[3] == '4')
                        {
                            List<Good> result = database.ExecuteReaderGoods("select * from Goods g join GoodTypes gt on g.TypeId=gt.Id join Providers p on p.Id = g.ProviderId " +
                                "where Count = (select max(Count) from Goods)");
                            DGMain.ItemsSource = result;
                        }
                        else if (CBMain.Text[3] == '5')
                        {
                            List<Good> result = database.ExecuteReaderGoods("select * from Goods g join GoodTypes gt on g.TypeId=gt.Id join Providers p on p.Id = g.ProviderId " +
                                "where Count = (select min(Count) from Goods)");
                            DGMain.ItemsSource = result;
                        }
                        else if (CBMain.Text[3] == '6')
                        {
                            List<Good> result = database.ExecuteReaderGoods("select * from Goods g join GoodTypes gt on g.TypeId=gt.Id join Providers p on p.Id = g.ProviderId " +
                                "where Price = (select min(Price) from Goods)");
                            DGMain.ItemsSource = result;
                        }
                        else if (CBMain.Text[3] == '7')
                        {
                            List<Good> result = database.ExecuteReaderGoods("select * from Goods g join GoodTypes gt on g.TypeId=gt.Id join Providers p on p.Id = g.ProviderId " +
                                "where Price = (select max(Price) from Goods)");
                            DGMain.ItemsSource = result;
                        }
                    }
                    else if (CBMain.Text[1] == '2')
                    {
                        if (CBMain.Text[3] == '1')
                        {
                            List<Good> result = database.ExecuteReaderGoods("select * from Goods g join GoodTypes gt on g.TypeId=gt.Id join Providers p on p.Id = g.ProviderId " +
                                "where gt.Name = \'GT1\'");
                            DGMain.ItemsSource = result;
                        }
                        else if (CBMain.Text[3] == '2')
                        {
                            List<Good> result = database.ExecuteReaderGoods("select * from Goods g join GoodTypes gt on g.TypeId=gt.Id join Providers p on p.Id = g.ProviderId " +
                                "where p.Name = \'Provider1\'");
                            DGMain.ItemsSource = result;
                        }
                        else if (CBMain.Text[3] == '3')
                        {
                            List<Good> result = database.ExecuteReaderGoods("select * from Goods g join GoodTypes gt on g.TypeId=gt.Id join Providers p on p.Id = g.ProviderId " +
                                "where Date = (select min(Date) from Goods)");
                            DGMain.ItemsSource = result;
                        }
                        else if (CBMain.Text[3] == '4')
                        {
                            List<NPCforInt> result = database.ExecuteReaderIntList("select avg(c) from (select count(*) as c from Goods group by TypeId) as g");
                            DGMain.ItemsSource = result;
                        }
                    }
                }
                else if (CBMain.Text[0] == 'H')
                {
                    if (CBMain.Text[1] == '1')
                    {
                        if (CBMain.Text[3] == '1')
                        {
                            database.ExecuteNonQuery("insert into Goods " +
                                "values ('IGood',(select Id from GoodTypes where Name = 'GT2'),(select Id from Providers where Name = 'Provider2'),500,99.99,'2023-10-10')");
                            MessageBox.Show("1 Good was added to the Stock","Insert",MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else if (CBMain.Text[3] == '2')
                        {
                            database.ExecuteNonQuery("insert into GoodTypes values('IGT')");
                            MessageBox.Show("1 Good Type was added to the Stock", "Insert", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else if (CBMain.Text[3] == '3')
                        {
                            database.ExecuteNonQuery("insert into Providers values('IProvider')");
                            MessageBox.Show("1 Provider was added to the Stock", "Insert", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else if (CBMain.Text[1] == '2')
                    {
                        if (CBMain.Text[3] == '1')
                        {
                            database.ExecuteNonQuery("update Goods set Name = 'UpName' where Name = 'Good3'");
                            MessageBox.Show("1 Good updated", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else if (CBMain.Text[3] == '2')
                        {
                            database.ExecuteNonQuery("update GoodTypes set Name = 'UpGT' where Name = 'GT3'");
                            MessageBox.Show("1 Good updated", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else if (CBMain.Text[3] == '3')
                        {
                            database.ExecuteNonQuery("update Providers set Name = 'UpProvider' where Name = 'Provider3'");
                            MessageBox.Show("1 Good updated", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else if (CBMain.Text[1] == '3')
                    {
                        if (CBMain.Text[3] == '1')
                        {
                            database.ExecuteNonQuery("delete from Goods where Name = 'Good3'");
                            MessageBox.Show("1 Good updated", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else if (CBMain.Text[3] == '2')
                        {
                            database.ExecuteNonQuery("delete from GoodTypes where Name = 'GT3'");
                            MessageBox.Show("1 Good updated", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else if (CBMain.Text[3] == '3')
                        {
                            database.ExecuteNonQuery("delete from GoodTypes where Name = 'Provider3'");
                            MessageBox.Show("1 Good updated", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else if (CBMain.Text[1] == '4')
                    {
                        if (CBMain.Text[3] == '1')
                        {
                            List<NPCforString> result = database.ExecuteReaderString("select Name " +
                                "from Providers p join (select ProviderId, count(Name) as c from Goods group by ProviderId ) t on t.ProviderId = p.Id " +
                                "where c = (select max(c) from (select count(Name) as c from Goods group by ProviderId ) t)");
                            DGMain.ItemsSource = result;
                        }
                        else if (CBMain.Text[3] == '2')
                        {
                            List<NPCforString> result = database.ExecuteReaderString("select Name " +
                                "from Providers p join (select ProviderId, count(Name) as c from Goods group by ProviderId ) t on t.ProviderId = p.Id " +
                                "where c = (select min(c) from (select count(Name) as c from Goods group by ProviderId ) t)");
                            DGMain.ItemsSource = result;
                        }
                        else if (CBMain.Text[3] == '3')
                        {
                            List<NPCforString> result = database.ExecuteReaderString("select Name " +
                                "from GoodTypes p join (select TypeId, count(Name) as c from Goods group by TypeId ) t on t.TypeId = p.Id " +
                                "where c = (select max(c) from (select count(Name) as c from Goods group by TypeId ) t)");
                            DGMain.ItemsSource = result;
                        }
                        else if (CBMain.Text[3] == '4')
                        {
                            List<NPCforString> result = database.ExecuteReaderString("select Name " +
                                "from GoodTypes p join (select TypeId, count(Name) as c from Goods group by TypeId ) t on t.TypeId = p.Id " +
                                "where c = (select min(c) from (select count(Name) as c from Goods group by TypeId ) t)");
                            DGMain.ItemsSource = result;
                        }
                        else if (CBMain.Text[3] == '5')
                        {
                            List<Good> result = database.ExecuteReaderGoods("select * " +
                                "from Goods g join GoodTypes gt on g.TypeId=gt.Id join Providers p on p.Id = g.ProviderId  " +
                                "where CONVERT(VARCHAR(10), date, 101) = CONVERT(VARCHAR(10),  (GETDATE()-day(30)-day(12)), 101)");
                            DGMain.ItemsSource = result;
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Combo Box is empty!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}


// Показати товари, з постачання яких минула задана кількість 
//днів