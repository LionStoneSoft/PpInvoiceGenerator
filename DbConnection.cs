using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace PpInvoiceGenerator
{
    class DbConnection
    {
        MySqlConnection con;

        //open db connection
        public void connect(string userName, string passWord)
        {
            string connStr = "SERVER=localhost;" +
                 "DATABASE=paypoint_rs;" +
                 $"UID={userName};" +
                 $"PWD={passWord};";

            con = new MySqlConnection(connStr);
            try
            {
                con.Open();
            } catch (Exception e)
            {
                Console.WriteLine("Error connecting to database. \n\n" + e);
                Environment.Exit(1);
            }
            
        }

        //close db connection
        public void disconnect()
        {
            try
            {
                con.Close();
            } catch (Exception e)
            {
                Console.WriteLine("Failed to close database connection. \n\n" + e);
                Environment.Exit(1);
            }
            
        }
        //retrieve the amount of customers in the database for the initial for loop.
        public int customerCount()
        {
            int customerCount = 0;
            string sql = $"SELECT COUNT(*) FROM customer";
            using var cmd = new MySqlCommand(sql, con);
            using MySqlDataReader rdr = cmd.ExecuteReader();
            try
            {
                while (rdr.Read())
                {
                    customerCount = rdr.GetInt32(0);
                }
            } catch (Exception e)
            {
                Console.WriteLine("Failed to retrieve amount of customers. \n\n" + e);
                Environment.Exit(1);
            }

            if (customerCount == 0)
            {
                Console.WriteLine("No customers in database.");
                Environment.Exit(1);
            }
                return customerCount;
        }

        //builds the object data for the current invoice from SQL
        public InvoiceModel customerObject(int customerNumber)
        {
            InvoiceModel customer = new InvoiceModel();

            string sql = $"SELECT * FROM customer WHERE customer_id = {customerNumber}";
            using var cmd = new MySqlCommand(sql, con);
            using MySqlDataReader rdr = cmd.ExecuteReader();
            try
            {
                while (rdr.Read())
                {
                    customer.CustomerNumber = rdr.GetInt32(0).ToString();
                    customer.CustomerName = rdr.GetString(1);
                }
            } catch (Exception e)
            {
                Console.WriteLine("Failed to populate customer object. \n\n" + e);
                Environment.Exit(1);
            } finally
            {
                rdr.Close();
            }
            customer = invoiceItems(customer);
            return customer;
        }
        
        //puts the customers purchased items into the invoice object
        private InvoiceModel invoiceItems(InvoiceModel customer)
        {
            InvoiceModel c = customer;
            List<InvoiceItemModel> iList = new List<InvoiceItemModel>();
            
            string sql = $"SELECT DATE_FORMAT(customerGameCharge.transaction_date, '%Y-%m-%d') AS transaction_date, gameItems.game_name, sum(gameItems.price) AS price FROM customerGameCharge INNER JOIN gameItems ON customerGameCharge.item_id = gameItems.item_id WHERE customer_id = {customer.CustomerNumber} AND CAST(transaction_date as date) = '20210525' GROUP BY game_name";
            using var cmd = new MySqlCommand(sql, con);
            using MySqlDataReader rdr = cmd.ExecuteReader();
            try
            {
                while (rdr.Read())
                {
                    InvoiceItemModel item = new InvoiceItemModel();
                    item.Date = rdr.GetString(0);
                    item.GameName = rdr.GetString(1);
                    item.Cost = rdr.GetDecimal(2);
                    iList.Add(item);
                }
            } catch (Exception e)
            {
                Console.WriteLine("Failed to populate invoice items into customer model. \n\n" + e);
                Environment.Exit(1);
            } finally
            {
                rdr.Close();
            }
            c.ItemList = iList;
            return c;
        }
        
    }
}
