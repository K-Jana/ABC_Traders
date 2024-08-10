using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_Traders
{
    public class PartOrder
    {
        public int OrderID { get; set; }
        public int PartID { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserID { get; set; }
        public string Status {  get; set; }
    }
}
