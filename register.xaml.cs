using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace pokemon
{
    /// <summary>
    /// Логика взаимодействия для register.xaml
    /// </summary>
    public partial class register : Window
    {
        DataBasePokemon dataBase = new DataBasePokemon();
        public register()
        {
            InitializeComponent();
        }

        private void btn_sign_in_Click(object sender, RoutedEventArgs e)
        {
            var loginUser = txb_login.Text;
            var passUser = txb_password.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            //string querystring = $"select id_user, login_user, password_user from register where login_user = '{loginUser}' and password_user = '{passUser}'";
            string querystring = "select id_user, login_user, password_user from register where login_user = @LoginUser and password_user = @PasswordUser";

            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
            //добавляем параметр `@LoginUser` и связываем его со значением переменной `loginUser`
            command.Parameters.Add(new SqlParameter("@LoginUser", loginUser));
            //добавляем параметр `@PasswordUser` и связываем его со значением переменной `passUser`
            command.Parameters.Add(new SqlParameter("@PasswordUser", passUser));

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {
                MessageBox.Show("Successfully", "Success", MessageBoxButton.OK);
                MainWindow main = new MainWindow();
                //this.Close();
                //main.ShowDialog();
                main.Show();
                this.Close(); //Закрываем текущее окно
            }
            else
                MessageBox.Show("That account doesnt't exist", "Enter a different account or get a new one", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void btn_sign_up_Click(object sender, RoutedEventArgs e)
        {
            var loginUserNew = txb_login_new.Text;
            var passUserNew = txb_password_new.Text;
            //Проверяем, существует ли пользователь
            if (checkUser())
            {
                //Если существует, выводим сообщение и завершаем метод
                return;
            }

            string querystring = "insert into register(login_user, password_user) values (@LoginUser, @PasswordUser)";

            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
            command.Parameters.Add(new SqlParameter("@LoginUser", loginUserNew));
            command.Parameters.Add(new SqlParameter("@PasswordUser", passUserNew));

            dataBase.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Registration Successful", "Success", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Something went wrong, please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
            dataBase.closeConnection();
        }

        private Boolean checkUser()
        {
            var loginUserNew = txb_login_new.Text;
            var passUserNew = txb_password_new.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = "select id_user, login_user, password_user FROM register where login_user = @LoginUser and password_user = @PasswordUser";

            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
            command.Parameters.Add(new SqlParameter("@LoginUser", loginUserNew));
            command.Parameters.Add(new SqlParameter("@PasswordUser", passUserNew));

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if(table.Rows.Count > 0)
            {
                MessageBox.Show("Account already exists");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
