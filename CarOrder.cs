using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_Traders
{
    internal class CarOrder
    {
        public int OrderID { get; set; }
        public int CarID { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserID { get; set; }
        public string Status { get; set; }
    }
}
