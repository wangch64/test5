// See https://aka.ms/new-console-template for more information

using System;
using MySql.Data.MySqlClient;
using System.Data;
class Program
{
    static void Main()
    {
        MaliciousCode mytest = new MaliciousCode();
        mytest.Main2100();
    }
}


public class SqlQueries
{
    public object UnsafeQuery(
       string connection, string name, string password)
    {
        MySqlConnection someConnection = new MySqlConnection(connection);
        MySqlCommand someCommand = new MySqlCommand();
        someCommand.Connection = someConnection;

        someCommand.CommandText = "SELECT AccountNumber FROM Users " +
           "WHERE Username='" + name +
           "' AND Password='" + password + "'";

        someConnection.Open();
        object accountNumber = someCommand.ExecuteScalar();
        Console.WriteLine("The answer = {0:d}", accountNumber);
        someConnection.Close();
        return accountNumber;
    }

    public object SaferQuery(
       string connection, string name, string password)
    {
        MySqlConnection someConnection = new MySqlConnection(connection);
        MySqlCommand someCommand = new MySqlCommand();
        someCommand.Connection = someConnection;

        someCommand.Parameters.Add(
           "@username", MySqlDbType.VarChar).Value = name;
        someCommand.Parameters.Add(
           "@password", MySqlDbType.VarChar).Value = password;
        someCommand.CommandText = "SELECT AccountNumber FROM Users " +
           "WHERE Username=@username AND Password=@password";

        someConnection.Open();
        object accountNumber = someCommand.ExecuteScalar();
        Console.WriteLine("The answer = {0:d}", accountNumber);
        someConnection.Close();
        return accountNumber;
    }
}

class MaliciousCode
{
    public void Main2100()
    {
        string connString = "server=localhost; port=3306;user id=root;password=Myroot1234;database=mydb1;";
        SqlQueries queries = new SqlQueries();
        queries.UnsafeQuery(connString, "' OR 1=1 -- ", "[PLACEHOLDER]");
        // Resultant query (which is always true):
        // SELECT AccountNumber FROM Users WHERE Username='' OR 1=1

        queries.SaferQuery(connString, "' OR 1=1 -- ", "[PLACEHOLDER]");
        // Resultant query (notice the additional single quote character):
        // SELECT AccountNumber FROM Users WHERE Username=''' OR 1=1 --'
        //                                   AND Password='[PLACEHOLDER]'

    }
}
