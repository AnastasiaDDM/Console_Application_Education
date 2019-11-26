using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Test
{
    public class Pay
    {
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public int Indicator { get; set; }           // 1 - договор, 2 - преподаватель
        public double Payment { get; set; }
        public string Purpose { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }



        public Nullable<int> ContractID { get; set; }
        public Contract Contract { get; set; }

        public Nullable<int> WorkerID { get; set; }
        public Worker Worker { get; set; }

        public Nullable<int> TimetableID { get; set; }
        //       public Timetable Timetable { get; set; }

        public Nullable<int> BranchID { get; set; }
        public Branch Branch { get; set; }

        public Pay()
        { }
        public string Add()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.Pays.Add(this);
                    context.SaveChanges();
                    answer = "Добавление оплаты прошло успешно";
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
                o = "Удаление оплаты прошло успешно";
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
                    answer = "Редактирование оплаты прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public string Сheck(Pay st)
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
    }

    public static class Pays
    {

        public static Pay PayID(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                Pay v = context.Pays.Where(x => x.ID == id).FirstOrDefault<Pay>();

                return v;
            }
        }

        //////////////////// ОДИН БОЛЬШОЙ ПОИСК !!! Если не введены никакие параметры, функция должна возвращать все филиалы //////////////////
        public static List<Pay> FindAll(Boolean deldate, Pay pay, Contract contract, Worker teacher, int timetable, Branch branch, DateTime mindate, DateTime maxdate, int min, int max, String sort, String askdesk, int page, int count) //deldate =false - все и удал и неудал!
        {
            List<Pay> list = new List<Pay>();
            using (SampleContext db = new SampleContext())
            {

                //var query = from b in db.Branches
                //            join w in db.Workers on b.DirectorBranch equals w.ID
                //            select new { BID = b.ID, BName = b.Name, BAddress = b.Address, BDeldate = b.Deldate, BEditdate = b.Editdate, BDirectorID = b.DirectorBranch, WID = w.ID };


                var query = from p in db.Pays
                          
                            select p;

                // Последовательно просеиваем наш список 

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    query = query.Where(x => x.Deldate == null);
                }

                if (pay.Type != null)
                {
                    query = query.Where(x => x.Type == pay.Type);
                }

                if (pay.Indicator != 0)
                {
                    query = query.Where(x => x.Indicator == pay.Indicator);
                }

                if (branch.ID != 0)
                {
                    query = query.Where(x => x.BranchID == branch.ID);
                }

                if (contract.ID != 0)
                {
                    query = query.Where(x => x.ContractID == contract.ID);
                }
                if (teacher.ID != 0)
                {
                    query = query.Where(x => x.WorkerID == teacher.ID);
                }

                if (timetable != 0)
                {
                    query = query.Where(x => x.TimetableID == timetable);
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
                    query = query.Where(x => x.Payment >= min);
                }

                if (max != 0)
                {
                    query = query.Where(x => x.Payment <= max);
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
                else { query = query.OrderByDescending(u => u.ID); }

                query = query.Skip((page - 1) * count).Take(count);
                query = query.Distinct();

                foreach (var p in query)
                {
                    list.Add(new Pay { ID = p.ID, Date = p.Date, BranchID = p.BranchID, ContractID = p.ContractID, WorkerID = p.WorkerID, TimetableID = p.TimetableID, Indicator = p.Indicator, Payment = p.Payment, Purpose = p.Purpose, Type = p.Type, Deldate = p.Deldate, Editdate = p.Editdate });
                }
                return list;
            }
        }
    }
}
