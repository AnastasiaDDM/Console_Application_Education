using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

//+ edit(Grade: Grade): String DONE
//+ add(Grade: Grade): Int DONE
//+ del(ID: Int): String DONE
//+ findAll(Start:	DateTime, End:	DateTime, visitNumber: Int, Date:	DateTime,
//                    editDate:	DateTime, delDate:	DateTime, Gradeot: Int, Gradedo: Int, 
//                    Student: Student, Course: Course, Theme: String, sorting : String, ASKorDESK : String, count : Int, page : Int): List<Grade>

namespace Test
{
    public class Grade
    {
        public int ID { get; set; }
        public int Mark { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }

        public int StudentID { get; set; }
        public Student Student { get; set; }

        public int ThemeID { get; set; }
        public Theme Theme { get; set; }

        public string Add()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.Grades.Add(this);
                    context.SaveChanges();
                    answer = "Добавление оценки прошло успешно";
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
                o = "Удаление оценки прошло успешно";
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
                    answer = "Редактирование оценки прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public string Сheck(Grade st)
        {
            if (st.Mark <=0 || st.Mark >5)
            { return "Это поле не может быть отрицательным. Это поле принимает значения - 1,2,3,4,5."; }

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

    public static class Grades
    {
        public static Grade GradeID(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                Grade v = context.Grades.Where(x => x.ID == id).FirstOrDefault<Grade>();
                return v;
            }
        }

        //////////////////// ОДИН БОЛЬШОЙ ПОИСК !!! Если не введены никакие параметры, функция должна возвращать все темы //////////////////
        public static List<Grade> FindAll(Boolean deldate, Grade grade, Theme theme, Course course, Student student, DateTime mindate, DateTime maxdate, int min, int max, String sort, String asсdesс, int page, int count) //deldate =false - все и удал и неудал!
        {
            List<Grade> list = new List<Grade>();
            using (SampleContext db = new SampleContext())
            {
                var query = from t in db.Themes
                            join g in db.Grades on t.ID equals g.ThemeID
                            join s in db.Students on g.StudentID equals s.ID
                            join sc in db.StudentsCourses on s.ID equals sc.StudentID
                            join tt in db.TimetablesThemes on t.ID equals tt.ThemeID


                            join time in db.Timetables on tt.TimetableID equals time.ID
                            select new { ID = g.ID, StudentID = g.StudentID, ThemeID = g.ThemeID, Mark = g.Mark, Deldate = g.Deldate, Editdate = g.Editdate, Date = t.Date, Course = sc.CourseID, TimetableCourse =time.CourseID };


                // Left jion для соединения таблиц, чтобы высвечивались все оценки 
                //var query = from g in db.Grades
                //            join t in db.Themes on g.ThemeID equals t.ID
                //            into theme_grade_temp
                //            from theme_grade in theme_grade_temp.DefaultIfEmpty()
                //            join s in db.Students on g.StudentID equals s.ID
                //            into st_grade_temp
                //            from st_grade in st_grade_temp.DefaultIfEmpty()
                //            join sc in db.StudentsCourses on st_grade.ID equals sc.StudentID
                //            into st_cour_grade_temp
                //            from st_cour_grade in st_cour_grade_temp.DefaultIfEmpty()
                //            join tt in db.TimetablesThemes on theme_grade.ID equals tt.ThemeID
                //            into time_theme_temp
                //            from time_theme in time_theme_temp.DefaultIfEmpty()

                //            join time in db.Timetables on time_theme.TimetableID equals time.ID
                //            into time_time_theme_temp
                //            from time_time_theme in time_time_theme_temp.DefaultIfEmpty()
                //            select new { ID = g.ID, StudentID = g.StudentID, ThemeID = g.ThemeID, Mark = g.Mark, Deldate = g.Deldate, Editdate = g.Editdate, Date = (theme_grade == null ? DateTime.MinValue : theme_grade.Date), Course = (st_cour_grade == null ? 0 : st_cour_grade.CourseID), TimetableCourse = (time_time_theme == null ? 0 : time_time_theme.CourseID) };

  //              query = g.GroupBy(x => x.ID);
                // Последовательно просеиваем наш список 

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    query = query.Where(x => x.Deldate == null);
                }

                if (theme.ID != 0)
                {
                    query = query.Where(x => x.ThemeID == theme.ID);
                }

                if (course.ID != 0)
                {
                    query = query.Where(x => x.Course == course.ID & x.Course == x.TimetableCourse); // Второе условие для того, чтобы при выборе курса выбирались
                                                                                                     //не просто все ученика этого курса со всеми своими оценками(даже по другим курсам),
                                                                                                     //а чтобы выбирались ученики и их оценки только за один выбранный курс
                }

                if (student.ID != 0)
                {
                    query = query.Where(x => x.StudentID == student.ID);
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
                    query = query.Where(x => x.Mark >= min);
                }

                if (max != 0)
                {
                    query = query.Where(x => x.Mark <= max);
                }
                query = query.Distinct();


                var query2 = query.GroupBy(s => new { s.ID, s.StudentID, s.ThemeID, s.Mark, s.Editdate, s.Deldate }, (key, group) => new
                {
                    ID = key.ID,
                    Mark = key.Mark,
                    StudentID = key.StudentID,
                    ThemeID = key.ThemeID,
                    Editdate = key.Editdate,
                    Deldate = key.Deldate,
                });

                if (sort != null) // Сортировка, если нужно
                {
                    query2 = Utilit.OrderByDynamic(query2, sort, asсdesс);
                }


                // Я перепроверила все варианты - это должно работать правильно!
                int countrecord = query2.GroupBy(u => u.ID).Count();

                query2 = query2.Skip((page - 1) * count).Take(count); // Формирование страниц и кол-во записей на странице

                foreach (var p in query2)
                {
                    list.Add(new Grade { ID = p.ID, StudentID = p.StudentID, ThemeID = p.ThemeID, Mark = p.Mark, Deldate = p.Deldate, Editdate = p.Editdate });
                }
                return list;
            }
        }
    }
}
