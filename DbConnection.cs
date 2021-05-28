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
        public void connect()
        {
            string connStr = "SERVER=localhost;" +
                 "DATABASE=paypoint_rs;" +
                 "UID=devuser;" +
                 "PWD=Treacle657;";

            con = new MySqlConnection(connStr);
            con.Open();
        }

        //close db connection
        public void disconnect()
        {
            con.Close();
        }

        //builds the object data for the current invoice from SQL
        public InvoiceModel customerObject(int customerNumber)
        {
            InvoiceModel customer = new InvoiceModel();

            string sql = $"SELECT * FROM customer WHERE customer_id = {customerNumber}";
            using var cmd = new MySqlCommand(sql, con);
            using MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                customer.CustomerNumber = rdr.GetInt32(0).ToString();
                customer.CustomerName = rdr.GetString(1);
            }
            rdr.Close();
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

            while (rdr.Read())
            {
                InvoiceItemModel item = new InvoiceItemModel();
                item.Date = rdr.GetString(0);
                item.GameName = rdr.GetString(1);
                item.Cost = rdr.GetDecimal(2);
                iList.Add(item);
            }
            rdr.Close();
            c.ItemList = iList;

            return c;
        }
        
    }
}
