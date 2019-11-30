using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

//+ edit(Visit: Visit): String DONE
//+ add(Visit: Visit): Int DONE
//+ del(ID:Int): String DONE
//+ findAll(Start:	DateTime, End:	DateTime, visitNumber: Int, Date:	DateTime,
//                   editDate:	DateTime, delDate:	DateTime, Visit: Bool, Student: Student, 
//                   Course: Course, sorting : String, ASKorDESK : String, count : Int, page : Int): List<Visit>

namespace Test
{
    public class Visit
    {
        public int ID { get; set; }
        public int Vis { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }

        public int StudentID { get; set; }
        public Student Student { get; set; }

        public int TimetableID { get; set; }
        public Timetable Timetable { get; set; }

        public string Add()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.Visits.Add(this);
                    context.SaveChanges();
                    answer = "Добавление посещения прошло успешно";
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
                o = "Удаление посещения прошло успешно";
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
                    answer = "Редактирование посещения прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public string Сheck(Visit st)
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

    public static class Visits
    {
        public static Visit VisitID(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                Visit v = context.Visits.Where(x => x.ID == id).FirstOrDefault<Visit>();
                return v;
            }
        }

        //////////////////// ОДИН БОЛЬШОЙ ПОИСК !!! Если не введены никакие параметры, функция должна возвращать все темы //////////////////
        public static List<Visit> FindAll(Boolean deldate, Visit visit, Theme theme, Course course, Student student, DateTime mindate, DateTime maxdate, String sort, String asсdesс, int page, int count) //deldate =false - все и удал и неудал!
        {
            List<Visit> list = new List<Visit>();
            using (SampleContext db = new SampleContext())
            {
                var query = from tt in db.TimetablesThemes
                            join t in db.Timetables on tt.TimetableID equals t.ID
                            join v in db.Visits on t.ID equals v.TimetableID
                            join s in db.Students on v.StudentID equals s.ID
                            join sc in db.StudentsCourses on s.ID equals sc.StudentID
                            select new { ID = v.ID, StudentID = v.StudentID, TimetableID = v.TimetableID, Vis = v.Vis, Deldate = v.Deldate, Editdate = v.Editdate, Theme = tt.ThemeID, Course = sc.CourseID, Date = t.Startlesson };

                // Последовательно просеиваем наш список 

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    query = query.Where(x => x.Deldate == null);
                }

                if (visit.ID != 0)
                {
                    query = query.Where(x => x.Vis == visit.Vis);
                }

                if (course.ID != 0)
                {
                    query = query.Where(x => x.Course == course.ID);
                }

                if (student.ID != 0)
                {
                    query = query.Where(x => x.StudentID == student.ID);
                }

                if (theme.ID != 0)
                {
                    query = query.Where(x => x.Theme == theme.ID);
                }

                if (mindate != DateTime.MinValue)
                {
                    query = query.Where(x => x.Date >= mindate);
                }

                if (maxdate != DateTime.MaxValue)
                {
                    query = query.Where(x => x.Date <= maxdate);
                }

                if (sort != null)  // Сортировка, если нужно
                {
                    query = Utilit.OrderByDynamic(query, sort, asсdesс);
                }

                int countrecord = query.GroupBy(u => u.ID).Count();

                query = query.Skip((page - 1) * count).Take(count);
                query = query.Distinct();

                foreach (var p in query)
                {
                    list.Add(new Visit { ID = p.ID, StudentID = p.StudentID, TimetableID = p.TimetableID, Vis = p.Vis, Deldate = p.Deldate, Editdate = p.Editdate });
                }
                return list;
            }
        }
    }
}
