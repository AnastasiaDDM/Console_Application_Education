using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

//+ Worker()  DONE
//+ WorkerID(ID : Int) DONE
//+ del(): String DONE
//+ add(): String DONE
//+ edit(): String DONE
//+ getContracts(): List<Contract> DONE
//+ getTimetable(Start: Datetime, End: Datetime): List<Timetable>
//+ Salary(Start: Datetime, End: Datetime, Rate: Double): Double

namespace Test
{
    public class Worker
    {
        public int ID { get; set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public int Type { get; set; } // 1 - это директор, 2 - менеджер, 3 -преподаватель

        public /*Branch*/ Nullable<int> BranchID { get; set; }
        public string Password { get; set; }
        public Nullable<double> Rate { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }


        public ICollection<Contract> Contracts { get; set; }


        public Worker()
        { }
        public string Add()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.Workers.Add(this);
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

        public string Сheck(Worker st)
        {
            if (st.FIO == "")
            { return "Введите ФИО ученика. Это поле не может быть пустым"; }
            if (st.Phone == "")
            { return "Введите номер телефона ученика. Это поле не может быть пустым"; }
            using (SampleContext context = new SampleContext())
            {
                Worker v = new Worker();
                v = context.Workers.Where(x => x.FIO == st.FIO && x.Phone == st.Phone).FirstOrDefault<Worker>();
                if (v != null)
                { return "Такой ученик уже существует в базе под номером " + v.ID; }
            }
            return "Данные корректны!";
        }


        public static List<Contract> GetContracts(Worker st)
        {
            using (SampleContext context = new SampleContext())
            {
                var v = context.Contracts.Where(x => x.ManagerID == st.ID).OrderBy(u => u.ID).ToList<Contract>();
                return v;
            }
        }
    }
    public static class Workers
    {
        public static Worker WorkerID(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                Worker v = context.Workers.Where(x => x.ID == id).FirstOrDefault<Worker>();

                return v;
            }
        }


        public static List<Worker> GetWo(SampleContext context)
        {
            //      var context = new SampleContext();

            var workers = context.Workers.ToList();
            return workers;
        }


        //////////////////// ОДИН БОЛЬШОЙ ПОИСК !!! Если не введены никакие параметры, функция должна возвращать все филиалы //////////////////
        public static List<Worker> FindAll(Boolean deldate, Worker worker, Branch branch, String sort, String askdesk, int page, int count) //deldate =false - все и удал и неудал!
        {
            List<Worker> list = new List<Worker>();
            using (SampleContext db = new SampleContext())
            {

                //var query = from w in db.Workers
                //            join b in db.Branches on w.ID equals  b.DirectorBranch
                //            select w;

                var query = from w in db.Workers
                            select w;

                // Последовательно просеиваем наш список 

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    query = query.Where(x => x.Deldate == null);
                }

                if (worker.FIO != null)
                {
                    query = query.Where(x => x.FIO == worker.FIO);
                }

                if (worker.Position != null)
                {
                    query = query.Where(x => x.Position == worker.Position);
                }

                if (worker.Type != 0)
                {
                    query = query.Where(x => x.Type == worker.Type);
                }

                if (branch.ID != 0)
                {
                    query = query.Where(x => x.BranchID == branch.ID);
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
                else { query = query.OrderBy(u => u.ID); }

                query = query.Skip((page - 1) * count).Take(count);

                foreach (var p in query)
                {
                    list.Add(new Worker { ID = p.ID, FIO = p.FIO, Type = p.Type, Position = p.Position, BranchID = p.BranchID, Deldate = p.Deldate, Editdate = p.Editdate });
                }
                return list;
            }
        }
    }
}
