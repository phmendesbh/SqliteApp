using System;
using Microsoft.Data.Sqlite;

namespace sqliteApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try{
                //Create Connection
                var connectioStringBuilder = new SqliteConnectionStringBuilder();
                //Create DataBase File
                connectioStringBuilder.DataSource = "./myDb.db";

                using (var connection = new SqliteConnection(connectioStringBuilder.ConnectionString)){
                    connection.Open();

                    //Create Table
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS palavras(id INTEGER PRIMARY KEY, palavra VARCHAR(15))";

                    //Execute query without data return
                    tableCmd.ExecuteNonQuery();

                    //Insert Some Records (Opening a transaction)
                    //If not especified a value for a PRIMARY KEY, a increment value is inserted
                    using(var transaction = connection.BeginTransaction()) {
                        var insertCmd = connection.CreateCommand();
                        insertCmd.CommandText = "INSERT INTO palavras (palavra) VALUES('TESTE')";
                        insertCmd.ExecuteNonQuery();

                        insertCmd.CommandText = "INSERT INTO palavras (palavra) VALUES('BATATA')";
                        insertCmd.ExecuteNonQuery();

                        insertCmd.CommandText = "INSERT INTO palavras (palavra) VALUES('CARRO')";
                        insertCmd.ExecuteNonQuery();

                        transaction.Commit();
                    }

                    //Read Records
                    var selectCmd = connection.CreateCommand();
                    selectCmd.CommandText = "SELECT * FROM palavras";
                    
                    //Use ExecuteReader to work with the data return
                    using(var reader = selectCmd.ExecuteReader()) {
                        while(reader.Read()) {
                            var result = $"{reader.GetInt64(0)} - {reader.GetString(1)}";
                            Console.WriteLine(result);
                        }
                    }
                }
            }catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
