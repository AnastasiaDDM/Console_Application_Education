using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

//+ Cabinet() DONE
//+ Cabinet(ID: Int) DONE
//+ add(): String DONE
//+ del(): String DONE
//+ edit(): String DONE
//+ getTimetable(Start: Datetime, End: Datetime): List<Timetable> DONE

//+getFree(Start: Datetime, End: Datetime, Branch: Branch): List<Cabinet>

namespace Test
{
    public class Cabinet
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public int Capacity { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }

        public int BranchID { get; set; }
        public Branch Branch { get; set; }


        public string Add()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.Cabinets.Add(this);
                    context.SaveChanges();
                    answer = "Добавление кабинета прошло успешно";
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
                o = "Удаление кабинета прошло успешно";
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
                    answer = "Редактирование кабинета прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public string Сheck(Cabinet st)
        {
            if (st.Number == "")
            { return "Введите номер кабинета. Это поле не может быть пустым"; }

            if (st.Capacity < 0)
            { return "Вместимость кабинета не может быть отрицательным числом. Введите этот параметр корректно"; }

            if (st.BranchID == 0)
            { return "Вероятно вы не выбрали филиал. Это поле не может быть пустым"; }

            //using (SampleContext context = new SampleContext())
            //{
            //    Student v = new Student();
            //    v = context.Students.Where(x => x.FIO == st.FIO && x.Phone == st.Phone).FirstOrDefault<Student>();
            //    if (v != null)
            //    { return "Такой ученик уже существует в базе под номером " + v.ID; }
            //}
            return "Данные корректны!";
        }

        public List<Timetable> getTimetables(DateTime date, bool deldate, int count, int page, string sort, string ascdesc, ref int countrecord)
        {

            Branch branch = new Branch();
            Worker teacher = new Worker();

            Student student = new Student();
            Course course = new Course();

            List<Timetable> timetables = new List<Timetable>();
            return timetables = Timetables.FindAll(deldate, branch, this, teacher, course, student, date, sort, ascdesc, page, count, ref countrecord);
        }

    }

    public static class Cabinets
    {

        public static Cabinet CabinetID(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                Cabinet v = context.Cabinets.Where(x => x.ID == id).FirstOrDefault<Cabinet>();

                return v;
            }
        }


        public static List<Cabinet> getFree()
        {
            List<Cabinet> free = new List<Cabinet>(); // Лист свободных 
            using (SampleContext context = new SampleContext())
            {



                //StringBuilder s = new StringBuilder("Select Distinct Cabinets.* from Cabinets where Workers.Type = 3 and Workers.ID not in (Select Distinct Workers.ID from Workers join TimetablesTeachers on TimetablesTeachers.TeacherID = Workers.ID and Workers.Type = 3 join Timetables on TimetablesTeachers.TimetableID = Timetables.ID where ");

                //List<string> sql = new List<string>();
                //string format = "yyyy-MM-dd HH:mm:ss";

                //foreach (TimeRange t in listtimerange)
                //{

                //    sql.Add(String.Format("(Startlesson >= '{0}' and Endlesson <= '{1}' and Startlesson <= '{1}')", t.Start.ToString(format), t.End.ToString(format))); // Внутри
                //    sql.Add(String.Format("(Startlesson <= '{0}' and Endlesson >= '{1}' and Startlesson <= '{1}')", t.Start.ToString(format), t.End.ToString(format))); // Снаружи
                //    sql.Add(String.Format("(Startlesson >= '{0}' and Endlesson >= '{1}' and Startlesson <= '{1}')", t.Start.ToString(format), t.End.ToString(format))); // верхняя граница
                //    sql.Add(String.Format("(Startlesson <= '{0}' and Endlesson <= '{1}' and Endlesson >= '{0}')", t.Start.ToString(format), t.End.ToString(format)));// нижняя граница

                //}

                //s.Append(String.Join(" or ", sql));

                //var query = context.Workers.SqlQuery(s.ToString() + " group by Workers.ID order by Workers.ID)");

                //foreach (Worker t in query)
                //{
                //    free.Add(t);
                //}
            }

            using (SampleContext db = new SampleContext())
            {
                var cabinets = db.Cabinets.Join(db.Timetables, // второй набор
        p => p.ID, // свойство-селектор объекта из первого набора
        c => c.CabinetID, // свойство-селектор объекта из второго набора
        (p, c) => new // результат
        {
            ID = p.ID,
            Number = p.Number,
            Capacity = p.Capacity,
            BranchID = p.BranchID,
            Editdate = p.Editdate,
            Deldate = p.Deldate,
            Startlesson = c.Startlesson,
            Endlesson = c.Endlesson,
        });

                var cab = cabinets.Where(p => p.Deldate == null);
                var freecab = cab.Where(p => p.Startlesson > DateTime.Now | p.Endlesson < DateTime.Now).GroupBy(s => new { s.ID, s.Number, s.BranchID, s.Capacity, s.Deldate, s.Editdate }, (key, group) => new
                {
                    ID = key.ID,
                    Number = key.Number,
                    Capacity = key.Capacity,
                    BranchID = key.BranchID,
                    Editdate = key.Editdate,
                    Deldate = key.Deldate
                }).Distinct();

                foreach (var p in freecab)
                {
                    free.Add(new Cabinet { ID = p.ID, Number = p.Number, Capacity = p.Capacity, BranchID = p.BranchID, Deldate = p.Deldate, Editdate = p.Editdate });
                }
            }
            return free;
        }
        //////////////////// ОДИН БОЛЬШОЙ ПОИСК !!! Если не введены никакие параметры, функция должна возвращать все кабинеты //////////////////
        public static List<Cabinet> FindAll(Boolean deldate, Cabinet cabinet, Branch branch, int min, int max, String sort, String asсdesс, int page, int count, ref int countrecord) //deldate =false - все и удал и неудал!
        {
            List<Cabinet> list = new List<Cabinet>();
            using (SampleContext db = new SampleContext())
            {

                var query = from b in db.Branches
                            join c in db.Cabinets on b.ID equals c.BranchID
                            //select new { BID = b.ID, BName = b.Name, BAddress = b.Address, BDeldate = b.Deldate, BEditdate = b.Editdate, BDirectorID = b.DirectorBranch, WID = w.ID };
                            select c;

                // Последовательно просеиваем наш список 

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    query = query.Where(x => x.Deldate == null);
                }

    //            if (cabinet.Number != "")
                if (cabinet.Number != null)
                {
                    query = query.Where(x => x.Number == cabinet.Number);
                }

                if (min != 0)
                {
                    query = query.Where(x => x.Capacity >= min);
                }

                if (max != 0)
                {
                    query = query.Where(x => x.Capacity <= max);
                }

                if (branch.ID != 0)
                {
                    query = query.Where(x => x.BranchID <= branch.ID);
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
                    list.Add(new Cabinet { ID = p.ID, Number = p.Number, Capacity = p.Capacity, BranchID = p.BranchID, Deldate = p.Deldate, Editdate = p.Editdate });
                }
                return list;
            }
        }
    }
}

