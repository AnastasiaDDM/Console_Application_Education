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
//+ getWorkers():List<Worker> DONE
//+ getContracts(): List<Contract> DONE
//+ getCourses(): List<Course> DONE
//+ getCabinets(): List<Cabinet> DONE
//+ profit(Start: Datetime, End: Datetime): Double    DONE  Я сделала одну функцию, которая сразу считает выручку, прибыль и кол-во договоров  Statistic !
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
        public ICollection<Cabinet> Cabinets { get; set; }


        public Branch()
        {
            //Contracts = new List<Contract>();
            //Cabinets = new List<Cabinet>();
        }

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

        public List<Cabinet> GetCabinets() // можно вводить только существующий ID филиала
        {
            using (SampleContext context = new SampleContext())
            {
                var v = context.Cabinets.Where(x => x.BranchID == this.ID).OrderBy(u => u.ID).ToList<Cabinet>();
                return v;
            }
        }

        public List<Contract> GetContracts() // можно вводить только существующий ID филиала
        {
            using (SampleContext context = new SampleContext())
            {
                var v = context.Contracts.Where(x => x.BranchID == this.ID).OrderBy(u => u.ID).ToList<Contract>();
                return v;
            }
        }

        public List<Worker> GetWorkers() // можно вводить только существующий ID филиала
        {
            using (SampleContext context = new SampleContext())
            {
                var v = context.Workers.Where(x => x.BranchID == this.ID).OrderBy(u => u.ID).ToList<Worker>();
                return v;
            }
        }

        public List<Course> GetCourses() // можно вводить только существующий ID филиала
        {
            using (SampleContext context = new SampleContext())
            {
                var v = context.Courses.Where(x => x.BranchID == this.ID).OrderBy(u => u.ID).ToList<Course>();
                return v;
            }
        }

        public int Statistic(DateTime start, DateTime end, out double profit, out double revenue)
        {
            using (SampleContext db = new SampleContext())
            {
                var paysbr = db.Pays.Where(p => p.BranchID == this.ID & p.Date >= start & p.Date <= end)/* == null ? 0 : pays.Where(p => p.StudentID == this.ID)*/;
                if (paysbr.Count() == 0)
                {
                    profit = 0;
                    revenue = 0; 
                }
                else
                {
                    revenue = ((paysbr.Where(p => p.ContractID != null).Count() == 0 ? 0 : paysbr.Where(p => p.ContractID != null).Sum(p => p.Payment)));
                    profit = revenue + (paysbr.Where(p => p.WorkerID != null).Count() == 0 ? 0 : (paysbr.Where(p => p.WorkerID != null).Sum(p => p.Payment))); // Сумма, потому что оплаты зп числятся с - (отрицательные). поэтому + на - равно -
                    //profit = revenue + (paysbr.Where(p => p.WorkerID != null).Sum(p => p.Payment)); // Сумма, потому что оплаты зп числятся с - (отрицательные). поэтому + на - равно -
                }
                int v = db.Contracts.Where(x => x.BranchID == this.ID).OrderBy(u => u.ID).Count();
                return v;
            }

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
        public static List<Branch> FindAll(Boolean deldate, Branch branch, Worker director, String sort, String asсdesс, int page, int count, ref int countrecord) //deldate =false - все и удал и неудал!
        {
            List<Branch> list = new List<Branch>();
            using (SampleContext db = new SampleContext())
            {

                var query = from b in db.Branches
                            join w in db.Workers on b.DirectorBranch equals w.ID
                            select new { ID = b.ID, Name = b.Name, Address = b.Address, Deldate = b.Deldate, Editdate = b.Editdate, DirectorID = b.DirectorBranch, WID = w.ID };

                // Последовательно просеиваем наш список 

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    query = query.Where(x => x.Deldate == null);
                }

                if (branch.Name != null)
                {
                    query = query.Where(x => x.Name == branch.Name);
                }

                if (branch.Address != null)
                {
                    query = query.Where(x => x.Address == branch.Address);
                }

                if (director.ID != 0)
                {
                    query = query.Where(x => x.DirectorID == director.ID);
                }

                if (sort != null)  // Сортировка, если нужно
                {
                    query = Utilit.OrderByDynamic(query, sort, asсdesс);
                }

                // Я перепроверила все варианты - это должно работать правильно!
                countrecord = query.GroupBy(u => u.ID).Count();

                query = query.Skip((page - 1) * count).Take(count);
                query = query.Distinct();

                foreach (var p in query)
                {
                    list.Add(new Branch { ID = p.ID, Name = p.Name, Address = p.Address, DirectorBranch = p.DirectorID, Deldate = p.Deldate, Editdate = p.Editdate });
                }
                return list;
            }
        }
    }
}
