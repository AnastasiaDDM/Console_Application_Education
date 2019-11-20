using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

//+ Course()
//+ Course(ID: Int)
//+ add(): String
//+ del(): String
//+ edit(): String
//+ getStudents(): List<Student> DONE
//+ getTeachers(): List<Worker> DONE
//+ getTimetable(Start: Datetime, End: Datetime): List<Timetable>
//+ addTeachers(Worker: Worker): String DONE
//+ editTeachers(Worker: Worker): String DONE

namespace Test
{
    public class Course
    {
        public int ID { get; set; }
        public string nameGroup { get; set; }
        public double Cost { get; set; }
        public int BranchID { get; set; }
        public Branch Branch { get; set; }

        public int TypeID { get; set; }
        public Type Type { get; set; }

        public Nullable<System.DateTime> Start { get; set; }
        public Nullable<System.DateTime> End { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }

        public Course()
        {

            //DateTime Start = Convert.ToDateTime(this.Start);
            //DateTime End = Convert.ToDateTime(this.End);
        }

            public string Add()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.Courses.Add(this);
                    context.SaveChanges();
                    answer = "Добавление курса прошло успешно";
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
                o = "Удаление курса прошло успешно";
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
                    answer = "Редактирование курса прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public string Сheck(Course st)
        {
            if (st.nameGroup == "")
            { return "Введите наименование группы. Это поле не может быть пустым"; }
            if (st.Cost == 0)
            { return "Введите стоимость обучения по данному курса. Это поле не может быть пустым"; }
            //using (SampleContext context = new SampleContext())
            //{
            //    Student v = new Student();
            //    v = context.Students.Where(x => x.FIO == st.FIO && x.Phone == st.Phone).FirstOrDefault<Student>();
            //    if (v != null)
            //    { return "Такой ученик уже существует в базе под номером " + v.ID; }
            //}
            return "Данные корректны!";
        }

        public static string СheckTeac(TeachersCourses stpar)
        {
            using (SampleContext context = new SampleContext())
            {
                TeachersCourses v = new TeachersCourses();
                v = context.TeachersCourses.Where(x => x.TeacherID == stpar.TeacherID && x.CourseID == stpar.CourseID).FirstOrDefault<TeachersCourses>();
                if (v != null)
                { return "Этот преподаватель уже числится за этим курсом"; }
                Worker t = Workers.WorkerID(stpar.TeacherID);
                if(t.Type !=3)
                { return " Вам нужно было выбрать преподавателя (тип 3)"; }
            }
            return "Данные корректны!";
        }

        public static List<Student> GetStudents(Course course)    // Получение списка  учеников этого курса
        {
            List<Student> liststudents = new List<Student>();
            using (SampleContext db = new SampleContext())
            {
                var students = from c in db.Courses
                              join sc in db.StudentsCourses on c.ID equals sc.CourseID
                              join s in db.Students on sc.StudentID equals s.ID
                              select new { SID = s.ID, SPhone = s.Phone, SFIO = s.FIO, SDelDate = s.Deldate, StID = sc.StudentID, CoID = sc.CourseID };

                students = students.Where(x => x.CoID == course.ID);
                students = students.Where(x => x.SID == x.StID);

                foreach (var p in students)
                {
                    liststudents.Add(new Student { ID = p.SID, Phone = p.SPhone, Deldate = p.SDelDate, FIO = p.SFIO });
                }
                return liststudents;
            }
        }

        public static List<Worker> GetTeachers(Course course)    // Получение списка  преподавателей этого курса
        {
            List<Worker> liststeachers = new List<Worker>();
            using (SampleContext db = new SampleContext())
            {
                var teachers = from c in db.Courses
                               join tc in db.TeachersCourses on c.ID equals tc.CourseID
                               join w in db.Workers on tc.TeacherID equals w.ID
                               select new { SID = w.ID, SPhone = w.Phone, SFIO = w.FIO, SDelDate = w.Deldate, TecID = tc.TeacherID, CoID = tc.CourseID };

                teachers = teachers.Where(x => x.CoID == course.ID);
                teachers = teachers.Where(x => x.SID == x.TecID);

                foreach (var p in teachers)
                {
                    liststeachers.Add(new Worker { ID = p.SID, Phone = p.SPhone, Deldate = p.SDelDate, FIO = p.SFIO });
                }
                return liststeachers;
            }
        }

        public static string addTeacher(Course c, Worker w)
        {
            TeachersCourses cw = new TeachersCourses();
            cw.CourseID = c.ID;
            cw.TeacherID = w.ID;
            string answer = СheckTeac(cw);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.TeachersCourses.Add(cw);
                    context.SaveChanges();
                    answer = "Добавление преподавателя на курс прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public static string delTeacher(Course c, Worker w)
        {
            TeachersCourses cw = new TeachersCourses();
            cw.CourseID = c.ID;
            cw.TeacherID = w.ID;
            string answer = "";

            using (SampleContext context = new SampleContext())
            {
                TeachersCourses v = new TeachersCourses();
                v = context.TeachersCourses.Where(x => x.TeacherID == cw.TeacherID && x.CourseID == cw.CourseID).FirstOrDefault<TeachersCourses>();
                context.TeachersCourses.Remove(v);
                context.SaveChanges();

                answer = "Удаление преподавателя с курса прошло успешно";
            }
            return answer;
        }
    }

    public class Courses
    {
        public static Course CourseID(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                Course v = context.Courses.Where(x => x.ID == id).FirstOrDefault<Course>();
                return v;
            }
        }
    }
}
