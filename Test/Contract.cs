using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace Test
{
    public class Contract
    {
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public int StudentID { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }
        public Nullable<System.DateTime> Canceldate { get; set; }
        public int ManagerID { get; set; }
        public double Cost { get; set; }
        public double PayofMonth { get; set; }
    }

    public class Contracts
    {
        public static List<Contract> GetCo(SampleContext context)
        {
            //      var context = new SampleContext();

            var contracts = context.Contracts.ToList();
            return contracts;
        }
    }
}
