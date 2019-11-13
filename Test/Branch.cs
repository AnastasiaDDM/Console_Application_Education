using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test
{
    public class Branch
    {

        [Key]
        public int ID { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }
        //[Required]
        public /*Worker*/  int DirectorBranch { get; set; }
    }

    public class Branches
    {
        //////////////////// ОДИН БОЛЬШОЙ ПОИСК !!! Если не введены никакие параметры, функция должна возвращать всех филиалов //////////////////
        public static List<Branch> FindAll(Boolean deldate, Branch branch, Worker director, String sort, String askdesk, int page, int count) //deldate =false - все и удал и неудал!
        {
            List<Branch> list = new List<Branch>();
            using (SampleContext db = new SampleContext())
            {

                var query = from b in db.Branches
                            join w in db.Workers on b.DirectorBranch equals w.ID
                            select new { BID = b.ID, BName = b.Name, BAddress = b.Address, BDeldate = b.Deldate, BEditdate = b.Editdate, BDirectorID = b.DirectorBranch, WID = w.ID };

                // Последовательно просеиваем наш список 

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    query = query.Where(x => x.BDeldate == null);
                }

                if (branch.Name != null)
                {
                    query = query.Where(x => x.BName == branch.Name);
                }

                if (branch.Address != null)
                {
                    query = query.Where(x => x.BAddress == branch.Address);
                }

                if (director.ID != 0)
                {
                    query = query.Where(x => x.BDirectorID == director.ID);
                }

                if (sort != null)  // Сортировка, если нужно
                {
                    if (askdesk == "desk")
                    {
                        query = query.OrderByDescending(u => sort);
                    }
                    else
                    {
                        query = query.OrderBy(u => sort);
                    }
                }
                else { query = query.OrderByDescending(u => u.BID); }

                query = query.Skip((page - 1) * count).Take(count);
                query = query.Distinct();

                foreach (var p in query)
                {
                    list.Add(new Branch { ID = p.BID, Name = p.BName, Address = p.BAddress, DirectorBranch = p.BDirectorID, Deldate = p.BDeldate, Editdate = p.BEditdate });
                }
                return list;
            }
        }
    }
}
