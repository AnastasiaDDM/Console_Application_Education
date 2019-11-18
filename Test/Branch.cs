using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//+ Branch() DONE
//+ Branch(ID: Int) DONE
//+ add(): String DONE                                   ПРОБЛЕМА С ПРОВЕРКОЙ!
//+ del(): String DONE
//+ edit(): String DONE
//+ getWorkers():List<Worker> 
//+ getContracts(): List<Contract>
//+ getCourses(): List<Course>
//+ getCabinets(): List<Cabinet>
//+ profit(Start: Datetime, End: Datetime): Double
//+ revenue(Start: Datetime, End: Datetime): Double

namespace Test
{
    public class Branch
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }
        //[Required]
        public /*Worker*/  int DirectorBranch { get; set; }


        public ICollection<Contract> Contracts { get; set; }

        public Branch()
        { }

        public string Add()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.Branches.Add(this);
                    context.SaveChanges();
                    answer = "Добавление филиала прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public string Del()
        {
            string o;
            using (SampleContext context = new SampleContext())
            {
                this.Deldate = DateTime.Now;
                context.Entry(this).State = EntityState.Modified;
                context.SaveChanges();
                o = "Удаление филиала прошло успешно";
            }
            return o;
        }

        public string Edit()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    this.Editdate = DateTime.Now;
                    context.Entry(this).State = EntityState.Modified;
                    context.SaveChanges();
                    answer = "Редактирование филиала прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public string Сheck(Branch st)           // Перепроверить проверку с уже существующими !
        {
            if (st.Name == "")
            { return "Введите название филиала. Это поле не может быть пустым"; }
            if (st.Address == "")
            { return "Введите адрес филиала. Это поле не может быть пустым"; }
            if (st.DirectorBranch == 0)
            { return "Выберите начальника филиала. Это поле не может быть пустым"; }
            using (SampleContext context = new SampleContext())
            {
                Branch v = new Branch();
                if (st.ID ==0)       // если мы добавляем новый филиал 
                {
                    v = context.Branches.Where(x => x.Name == st.Name && x.Address == st.Address && x.DirectorBranch == st.DirectorBranch || x.Address == st.Address || x.DirectorBranch == st.DirectorBranch).FirstOrDefault<Branch>();
                    if (v != null)
                    { return "Такой филиал уже существует в базе под номером " + v.ID; }   
                }
                else
                {
                    v = context.Branches.Where(x => x.Name == st.Name && x.Address == st.Address && x.DirectorBranch == st.DirectorBranch).FirstOrDefault<Branch>();
                    if (v != null && v != st)
                    { return "Такой филиал уже существует в базе под номером " + v.ID; }
                }
            }
            return "Данные корректны!";
        }

    }

    public static class Branches
    {
        public static Branch BranchID(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                Branch v = context.Branches.Where(x => x.ID == id).FirstOrDefault<Branch>();

                return v;
            }
        }

        //////////////////// ОДИН БОЛЬШОЙ ПОИСК !!! Если не введены никакие параметры, функция должна возвращать все филиалы //////////////////
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
