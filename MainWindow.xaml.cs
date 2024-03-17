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
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;

namespace pokemon
{
    public partial class MainWindow : Window
    {
        private DataTable pokemonsData; // Поле класса для хранения данных о покемонах
        DataBasePokemon dataBase = new DataBasePokemon(); //Экземпляр класса для доступа к методам рабоы с бд

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string query = "SELECT Pokemon_id,Name,Type_1,Type_2,Total,HP,Attack,Defense,Sp_Atk,Sp_Def,Speed,Generation FROM Pokemon1";
            pokemonsData = dataBase.ExecuteQuery(query);
            dataGrid1.ItemsSource = pokemonsData.DefaultView;
        }

        private void btn_Changed_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid1.SelectedItem != null)
            {
                dataGrid1.IsReadOnly = false;
            }
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (dataGrid1.SelectedItem != null)
                {
                    DataRowView rowView = dataGrid1.SelectedItem as DataRowView;
                    if (rowView != null)
                    {
                        DataRow row = rowView.Row;
                        if (row.RowState == DataRowState.Added) // Если строка новая
                        {
                            dataBase.AddRowToDatabase(row);
                        }
                        else if (row.RowState == DataRowState.Modified) // Если строка изменена
                        {
                            dataBase.UpdateRowInDatabase(row, "Pokemon1");
                        }
                        else if (row.RowState == DataRowState.Deleted) // Если строка удалена
                        {
                            int pokemonId = Convert.ToInt16(row["Pokemon_id", DataRowVersion.Original]);
                            dataBase.DeleteRowFromDatabase(pokemonId);
                        }
                    }
                }

                dataGrid1.IsReadOnly = true; // Заблокировать редактирование после сохранения
            }
            else
            {
                MessageBox.Show("Save operation canceled.", "Canceled", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            // Добавление новой пустой строки в DataTable
            DataRow newRow = pokemonsData.NewRow();
            pokemonsData.Rows.Add(newRow);
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            // Проверка наличия выбранной строки в DataGrid
            if (dataGrid1.SelectedItem != null)
            {
                DataRowView rowView = dataGrid1.SelectedItem as DataRowView;
                if (rowView != null)
                {
                    DataRow row = rowView.Row;
                    // Запрос подтверждения перед удалением записи из базы данных
                    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        // Вызов метода удаления записи из базы данных
                        Int16 pokemonId = Convert.ToInt16(row["Pokemon_id"]);
                        dataBase.DeleteRowFromDatabase(pokemonId);
                        pokemonsData.Rows.Remove(row);
                    }
                }
            }
        }

        private void txb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txb_search.Text.ToLower(); // Получаем текст из TextBox для поиска (приводим к нижнему регистру)

            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Если TextBox пустой, отображаем все данные
                dataGrid1.ItemsSource = pokemonsData.DefaultView;
            }
            else
            {
                // Фильтруем данные по столбцу 'Name' с учетом введенного текста для поиска
                var filteredRows = pokemonsData.AsEnumerable() //Преобразовываем datatable в последовательность из строк
                                              .Where(row => row.Field<string>("Name").ToLower().Contains(searchText)); //LINQ запрос, который фильтрует последовательность(по столбцу Name)
                                              
                if (filteredRows.Any())
                {
                    DataTable filteredTable = filteredRows.CopyToDataTable();
                    dataGrid1.ItemsSource = filteredTable.DefaultView; // Устанавливаем отфильтрованные данные в DataGrid
                }
                else
                {
                    dataGrid1.ItemsSource = null; //Показывает пустую таблицу
                }
            }
        }

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(); // Вызов метода обновления данных в DataGrid
        }

        private void UpdateDataGrid()
        {
            // Очистка таблицы
            pokemonsData.Clear();

            // Получение данных из базы данных
            DataTable newData = dataBase.GetDataFromDatabase();

            // Копирование данных в таблицу DataGrid
            foreach (DataRow row in newData.Rows)
            {
                pokemonsData.Rows.Add(row.ItemArray);
            }
        }
    }
}
