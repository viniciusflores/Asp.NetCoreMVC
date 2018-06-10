using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//Metodos importados pós nugget
using MySql.Data.MySqlClient;
using System.Data;

namespace MyFinance.Util
{
    public class DAL
    {
        //Dados de Conexão
        private static string server = "localhost";
        private static string database = "financeiro";
        private static string user = "root";
        private static string password = "";
        private string connectionString = $"Server={server};Database={database};Uid={user};Pwd={password};SslMode=none";
        private MySqlConnection connection;

        //metodo construtor
        public DAL()
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }



        //EXECUTA SELECTs
        //DataTable é uma classe especifica do c# para esses fins
        public DataTable RetDataTable(String sql)
        {
            DataTable dataTable = new DataTable();
            MySqlCommand command = new MySqlCommand(sql, connection);

            //é necessário traduzir a informação do banco para o c# com o coteudo a seguir
            MySqlDataAdapter da = new MySqlDataAdapter(command);

            //após convertido, a consulta retorna em modo DataTable
            da.Fill(dataTable);
            return dataTable;
        }

        //EXECUTA INSERTs, UPDATEs e DELETEs
        public void ExecutarComandoSQL(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.ExecuteNonQuery();

        }


    }
}
