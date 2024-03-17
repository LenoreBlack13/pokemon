using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Data.Entity;

namespace pokemon
{
    class DataBasePokemon
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=(local);Initial Catalog=pokemon;Integrated Security=True");

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection GetConnection()
        {
            return sqlConnection;
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                openConnection();
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                closeConnection();
            }
            return dt;
        }

        public void UpdateRowInDatabase(DataRow row, string tableName)
        {
            try
            {
                openConnection();
                string updateQuery = "UPDATE " + tableName + " SET Name = @Name, Type_1 = @Type_1, Type_2 = @Type_2, Total = @Total, HP = @HP, Attack = @Attack, Defense = @Defense, Sp_Atk = @Sp_Atk, Sp_Def = @Sp_Def, Speed = @Speed, Generation = @Generation WHERE Pokemon_id=@ID";
                using (SqlCommand cmd = new SqlCommand(updateQuery, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Name", row["Name"]);
                    cmd.Parameters.AddWithValue("@Type_1", row["Type_1"]);
                    cmd.Parameters.AddWithValue("@Type_2", row["Type_2"]);
                    cmd.Parameters.AddWithValue("@Total", row["Total"]);
                    cmd.Parameters.AddWithValue("@HP", row["HP"]);
                    cmd.Parameters.AddWithValue("@Attack", row["Attack"]);
                    cmd.Parameters.AddWithValue("@Defense", row["Defense"]);
                    cmd.Parameters.AddWithValue("@Sp_Atk", row["Sp_Atk"]);
                    cmd.Parameters.AddWithValue("@Sp_Def", row["Sp_Def"]);
                    cmd.Parameters.AddWithValue("@Speed", row["Speed"]);
                    cmd.Parameters.AddWithValue("@Generation", row["Generation"]);
                    cmd.Parameters.AddWithValue("@ID", row["Pokemon_id"]);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                closeConnection();
            }
        }
        public void AddRowToDatabase(DataRow row)
        {
            try
            {
                openConnection();
                string insertQuery = "INSERT INTO Pokemon1 (Name, Type_1, Type_2, Total, HP, Attack, Defense, Sp_Atk, Sp_Def, Speed, Generation) VALUES (@Name, @Type_1, @Type_2, @Total, @HP, @Attack, @Defense, @Sp_Atk, @Sp_Def, @Speed, @Generation)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Name", row["Name"]);
                    cmd.Parameters.AddWithValue("@Type_1", row["Type_1"]);
                    cmd.Parameters.AddWithValue("@Type_2", row["Type_2"]);
                    cmd.Parameters.AddWithValue("@Total", row["Total"]);
                    cmd.Parameters.AddWithValue("@HP", row["HP"]);
                    cmd.Parameters.AddWithValue("@Attack", row["Attack"]);
                    cmd.Parameters.AddWithValue("@Defense", row["Defense"]);
                    cmd.Parameters.AddWithValue("@Sp_Atk", row["Sp_Atk"]);
                    cmd.Parameters.AddWithValue("@Sp_Def", row["Sp_Def"]);
                    cmd.Parameters.AddWithValue("@Speed", row["Speed"]);
                    cmd.Parameters.AddWithValue("@Generation", row["Generation"]);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                closeConnection();
            }
        }

        public void DeleteRowFromDatabase(int pokemonId)
        {
            try
            {
                openConnection();
                string deleteQuery = "DELETE FROM Pokemon1 WHERE Pokemon_id=@ID";
                using (SqlCommand cmd = new SqlCommand(deleteQuery, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@ID", pokemonId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                closeConnection();
            }
        }

        public DataTable GetDataFromDatabase() //Метод для обновления таблицы
        {
            DataTable data = new DataTable();

            try
            {
                openConnection();  // Открываем соединение с базой данных

                string selectQuery = "SELECT * FROM Pokemon1"; // Запрос для выборки данных из вашей таблицы
                data = ExecuteQuery(selectQuery); // Выполняем запрос и получаем результат в виде DataTable
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                closeConnection(); 
            }

            return data;
        }

    }
}
