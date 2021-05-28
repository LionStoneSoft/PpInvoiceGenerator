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
            newdb.connect();

            for (int i = 1; i < 40001; i++)
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
        }
    }
}
