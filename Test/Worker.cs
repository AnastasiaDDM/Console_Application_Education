using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test
{
    public class Worker
    {
        [Key]
        public int ID { get; set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }
        public string Position { get; set; }
        public bool Type { get; set; } // true – если преподаватель, false – если управляющая должность
                                       //[ForeignKey("Branch")]
        //[Required]
        public /*Branch*/ int BranchID { get; set; }

    }
    public class Workers
    {
        public static List<Worker> GetWo(SampleContext context)
        {
            //      var context = new SampleContext();

            var workers = context.Workers.ToList();
            return workers;
        }
    }
}
