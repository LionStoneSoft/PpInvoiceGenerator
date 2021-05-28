using System;
using System.Collections.Generic;
using System.Text;

namespace PpInvoiceGenerator
{
    class InvoiceModel
    {
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public List<InvoiceItemModel> ItemList { get; set; }
    }
}
