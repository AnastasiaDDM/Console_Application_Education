using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

//+ edit(Theme: Theme): String DONE
//+ add(Theme: Theme): Int DONE
//+ del(ID: Int): String DONE
//+ getGrade(ID:Int): List<Grade> DONE
//+ findAll(Start:	DateTime, End:	DateTime, themeNumber: Int, Date:	DateTime, 
//                                  editDate:	DateTime, delDate:	DateTime, Course: Course,
//                                  Theme: String, sorting : String, ASKorDESK : String, count : Int, page : Int): List<Theme>

namespace Test
{
    public class Theme
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Tema { get; set; }
        public string Homework { get; set; }
        public Nullable<System.DateTime> Deadline { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }

        public string Add()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.Themes.Add(this);
                    context.SaveChanges();
                    answer = "Добавление темы прошло успешно";
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
                o = "Удаление темы прошло успешно";
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
                    answer = "Редактирование темы прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public string Сheck(Theme st)
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

        public List<Grade> GetGrades()
        {
            using (SampleContext context = new SampleContext())
            {
                var v = context.Grades.Where(x => x.ThemeID == this.ID).OrderBy(u => u.ID).ToList<Grade>();
                return v;
            }
        }
    }


    public static class Themes
    {
        public static Theme ThemeID(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                Theme v = context.Themes.Where(x => x.ID == id).FirstOrDefault<Theme>();
                return v;
            }
        }

        //////////////////// ОДИН БОЛЬШОЙ ПОИСК !!! Если не введены никакие параметры, функция должна возвращать все темы //////////////////
        public static List<Theme> FindAll(Boolean deldate, Theme theme, Course course, DateTime mindate, DateTime maxdate, String sort, String asсdesс, int page, int count) //deldate =false - все и удал и неудал!
        {
            List<Theme> list = new List<Theme>();
            using (SampleContext db = new SampleContext())
            {           // Left jion для соединения таблиц, чтобы высвечивались все темы, не зависимо от того, если ли по этим темам занятия(Timetable)
                var query = from t in db.Themes
                            join tt in db.TimetablesThemes on t.ID equals tt.ThemeID
                            into theme_time_temp
                            from theme_time in theme_time_temp.DefaultIfEmpty()
                            join s in db.Timetables on theme_time.TimetableID equals s.ID
                            into time_temp
                            from time in time_temp.DefaultIfEmpty()
                            select new { ID = t.ID, Date = t.Date, Tema = t.Tema, Homework = t.Homework, Deadline = t.Deadline, Deldate = t.Deldate, Editdate = t.Editdate, Course = (time == null ? 0 : time.CourseID) /*, ThTi = (theme_time == null ? 0 : theme_time.ID) */};

                // Последовательно просеиваем наш список 

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    query = query.Where(x => x.Deldate == null);
                }

                if (theme.Tema != null)
                {
                    query = query.Where(x => x.Tema == theme.Tema);
                }

                if (course.ID != 0)
                {
                    query = query.Where(x => x.Course == course.ID);
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
                    list.Add(new Theme { ID = p.ID, Date = p.Date, Tema = p.Tema, Homework = p.Homework, Deadline = p.Deadline, Deldate = p.Deldate, Editdate = p.Editdate });
                }
                return list;
            }
        }
    }
}
