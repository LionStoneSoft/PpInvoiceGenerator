using System;
using System.Collections.Generic;
using System.Text;

namespace PpInvoiceGenerator
{
    class HtmlInvoiceGenerator
    {
        //generates the html layout for the current invoice
        public string generateTable(InvoiceModel model)
        {
            StringBuilder sb = new StringBuilder();
            decimal totalCost = 0;

            sb.Append($"<!DOCTYPE html><html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\"><body><div><p>Customer Number: {model.CustomerNumber}</p><p>Customer Name: {model.CustomerName}</p></div><div><p style=\"font-weight: bold\">CHARGES</p></div><div><table style=\"width:100%; border-top: solid; border-bottom: solid\"><tr style=\"margin-left: auto; margin-right: auto\"><th align=\"left\">Date</th><th align=\"left\">Game Name</th><th align=\"right\">Price</th></tr>");

            foreach (var item in model.ItemList)
            {
                sb.Append($"<tr><td align=\"left\">{item.Date}</td><td align=\"left\">{item.GameName}</td><td align=\"right\">{item.Cost}</td></tr>");
                totalCost = totalCost + item.Cost;
            }
            sb.Append($"</table></div><div style=\"text-align: right\"><p style=\"font-weight: bold\">Total: £{totalCost.ToString()}</p></div></body></html>");
            string finalString = sb.ToString();
            return finalString;
        }
    }
}
