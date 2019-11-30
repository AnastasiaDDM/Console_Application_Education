using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Data;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;


//+Contract() DONE
//+Contract(ID: Int) DONE
//+ add(): String DONE
//+ del(): String DONE
//+ edit(): String DONE
//+ Cancellation(): String DONE
//+ addPay(Payment: Double): String DONE


namespace Test
{
    public class Contract
    {
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public double Cost { get; set; }
        public double PayofMonth { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }
        public Nullable<System.DateTime> Canceldate { get; set; }
        

        public int StudentID { get; set; }
        public Student Student { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }

        public int ManagerID { get; set; }
        public Worker Manager { get; set; }

        public int BranchID { get; set; }
        public Branch Branch { get; set; }


        public Contract()
        { }
       
        public string Add()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    StudentsCourses stpar = new StudentsCourses();
                    stpar.StudentID = this.StudentID;
                    stpar.CourseID = this.CourseID;
                    context.Contracts.Add(this);
                    context.StudentsCourses.Add(stpar);
                    context.SaveChanges();

                    answer = "Добавление договора прошло успешно";
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
                o = "Удаление договора прошло успешно";
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
                    answer = "Редактирование договора прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public string Cancellation()
        {
            string o;
            using (SampleContext context = new SampleContext())
            {
                this.Canceldate = DateTime.Now;
                context.Entry(this).State = EntityState.Modified;
                context.SaveChanges();
                o = " Расторжение договора прошло успешно";
            }
            return o;
        }
        public string Сheck(Contract st)
        {
            //if (st.FIO == "")
            //{ return "Введите ФИО ученика. Это поле не может быть пустым"; }
            //if (st.Phone == "")
            //{ return "Введите номер телефона ученика. Это поле не может быть пустым"; }
            //using (SampleContext context = new SampleContext())
            //{
            //    Worker v = new Worker();
            //    v = context.Workers.Where(x => x.FIO == st.FIO && x.Phone == st.Phone).FirstOrDefault<Worker>();
            //    if (v != null)
            //    { return "Такой ученик уже существует в базе под номером " + v.ID; }
            //}
            return "Данные корректны!";
        }

        public string addPay(Pay p)
        {
            p.ContractID = this.ID;
            string ans = p.Add();
            return ans;
        }

    }

    public static class Contracts
    {
        public static Contract ContractID(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                Contract v = context.Contracts.Where(x => x.ID == id).FirstOrDefault<Contract>();
                return v;
            }
        }

        //public static List<Contract> GetCo()      // Просто так, для получения нефильтрованного списка
        //{
        //    //      var context = new SampleContext();
        //    using (SampleContext db = new SampleContext())
        //    {
        //        var contracts = db.Contracts.ToList();
        //        return contracts;
        //    }
        //}

        //////////////////// ОДИН БОЛЬШОЙ ПОИСК !!! Если не введены никакие параметры, функция должна возвращать все договоры //////////////////
        public static List<Contract> FindAll(Boolean deldate, Student student, Worker manager, Branch branch, Course course, DateTime mindate, DateTime maxdate, int min, int max, String sort, String asсdesс, int page, int count) //deldate =false - все и удал и неудал!
        {
            List<Contract> list = new List<Contract>();
            using (SampleContext db = new SampleContext())
            {

                //var query = from b in db.Branches
                //            join w in db.Workers on b.DirectorBranch equals w.ID
                //            select new { BID = b.ID, BName = b.Name, BAddress = b.Address, BDeldate = b.Deldate, BEditdate = b.Editdate, BDirectorID = b.DirectorBranch, WID = w.ID };

                var query = from c in db.Contracts
                            select c;

                                // Последовательно просеиваем наш список 

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    query = query.Where(x => x.Deldate == null);
                }

                if (branch.ID != 0)
                {
                    query = query.Where(x => x.BranchID == branch.ID);
                }

                if (student.ID != 0)
                {
                    query = query.Where(x => x.StudentID == student.ID);
                }
                if (manager.ID != 0)
                {
                    query = query.Where(x => x.ManagerID == manager.ID);
                }

                if (course.ID != 0)
                {
                    query = query.Where(x => x.CourseID == course.ID);
                }

                if (mindate != DateTime.MinValue)
                {
                    query = query.Where(x => x.Date >= mindate);
                }

                if (maxdate != DateTime.MaxValue)
                {
                    query = query.Where(x => x.Date <= maxdate);
                }

                if (min != 0)
                {
                    query = query.Where(x => x.Cost >= min);
                }

                if (max != 0)
                {
                    query = query.Where(x => x.Cost <= max);
                }

                if (sort != null)  // Сортировка, если нужно
                {
                    query = Utilit.OrderByDynamic(query, sort, asсdesс);
                }

                // Я перепроверила все варианты - это должно работать правильно!
                int countrecord = query.GroupBy(u => u.ID).Count();

                query = query.Skip((page - 1) * count).Take(count);
                query = query.Distinct();

                foreach (var p in query)
                {
                    list.Add(new Contract { ID = p.ID, Date = p.Date, StudentID = p.StudentID, CourseID = p.CourseID, BranchID = p.BranchID, ManagerID = p.ManagerID, Cost = p.Cost, PayofMonth = p.PayofMonth, Canceldate = p.Canceldate, Deldate = p.Deldate, Editdate = p.Editdate });
                }
                return list;
            }
        }
    }
}
