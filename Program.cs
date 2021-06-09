using System;
using System.Diagnostics;
using PdfSharp;
using PdfSharp.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace PpInvoiceGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);            

            Stopwatch timePerParse = Stopwatch.StartNew();
            DbConnection newdb = new DbConnection();
            HtmlInvoiceGenerator generate = new HtmlInvoiceGenerator();
            InvoiceModel currentCustomer;

            Console.WriteLine("Database Username: ");
            string userName = Console.ReadLine();
            Console.WriteLine("Database Password: ");
            string passWord = Console.ReadLine();

            newdb.connect(userName, passWord);
            int customerCount = newdb.customerCount();
            Console.WriteLine("Invoices being generated...");

            for (int i = 1; i <= customerCount; i++)
            {
                currentCustomer = newdb.customerObject(i);
                string content = generate.generateTable(currentCustomer);
                if (currentCustomer.ItemList.Count != 0)
                {
                    PdfDocument pdfDocument = PdfGenerator.GeneratePdf(content, PageSize.A4);
                    pdfDocument.Save($@"C:\generatedpdf\pdfinvoice{i}.pdf");
                }
            }
            newdb.disconnect();
            timePerParse.Stop();
            Console.WriteLine(timePerParse.ElapsedMilliseconds);
            Console.WriteLine("Invoice generation complete.");
        }
    }
}
