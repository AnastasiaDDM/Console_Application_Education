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
//+ delTeachers(Worker: Worker): String DONE
//+ addStudents(): String DONE
//+ delStudents(): String DONE

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

            if (st.Start >= st.End)
            { return "Дата начала курса не может быть позже или такой же, как дата окончания обучения"; }
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
        public static string СheckStud(StudentsCourses stpar)
        {
            using (SampleContext context = new SampleContext())
            {
                StudentsCourses v = new StudentsCourses();
                v = context.StudentsCourses.Where(x => x.StudentID == stpar.StudentID && x.CourseID == stpar.CourseID).FirstOrDefault<StudentsCourses>();
                if (v != null)
                { return "Этот ученик уже числится на этом курсе"; }
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

        public static string addStudent(Course c, Student s)
        {
            StudentsCourses sc = new StudentsCourses();
            sc.CourseID = c.ID;
            sc.StudentID = s.ID;
            string answer = СheckStud(sc);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.StudentsCourses.Add(sc);
                    context.SaveChanges();
                    answer = "Добавление ученика на курс прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public static string delStudent(Course c, Student s)
        {
            StudentsCourses cw = new StudentsCourses();
            cw.CourseID = c.ID;
            cw.StudentID = s.ID;
            string answer = "";

            using (SampleContext context = new SampleContext())
            {
                StudentsCourses v = new StudentsCourses();
                v = context.StudentsCourses.Where(x => x.StudentID == cw.StudentID && x.CourseID == cw.CourseID).FirstOrDefault<StudentsCourses>();
                context.StudentsCourses.Remove(v);
                context.SaveChanges();

                answer = "Удаление ученика с курса прошло успешно";
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

        //////////////////// ОДИН БОЛЬШОЙ ПОИСК !!! Если не введены никакие параметры, функция должна возвращать все филиалы //////////////////
        public static List<Course> FindAll(Boolean deldate, Course course, Type type, Worker teacher, Branch branch, DateTime mindate, DateTime maxdate, int min, int max, String sort, String askdesk, int page, int count) //deldate =false - все и удал и неудал!
        {
            List<Course> list = new List<Course>();
            using (SampleContext db = new SampleContext())
            {

                var query = from c in db.Courses
                            join w in db.TeachersCourses on c.ID equals w.CourseID
                            select new { ID = c.ID, nameGroup = c.nameGroup, Cost = c.Cost, Deldate = c.Deldate, Editdate = c.Editdate, TypeID = c.TypeID, BranchID = c.BranchID, Start =c.Start, End = c.End, TeacherID = w.TeacherID };


                // Последовательно просеиваем наш список 

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    query = query.Where(x => x.Deldate == null);
                }

                if (course.nameGroup != null) 
                {
                    query = query.Where(x => x.nameGroup == course.nameGroup);
                }

                if (branch.ID != 0)
                {
                    query = query.Where(x => x.BranchID == branch.ID);
                }

                if (type.ID != 0)
                {
                    query = query.Where(x => x.TypeID == type.ID);
                }
                if (teacher.ID != 0)
                {
                    query = query.Where(x => x.TeacherID == teacher.ID);
                }

                if (mindate != DateTime.MinValue)
                {
                    query = query.Where(x => x.Start >= mindate);
                }

                if (maxdate != DateTime.MaxValue)
                {
                    query = query.Where(x => x.End <= maxdate);
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
                query = query.Distinct();

                foreach (var p in query)
                {
                    list.Add(new Course { ID = p.ID, nameGroup = p.nameGroup, BranchID = p.BranchID, TypeID = p.TypeID, Cost = p.Cost, Start = p.Start, End = p.End, Deldate = p.Deldate, Editdate = p.Editdate });
                }
                return list;
            }
        }
    }
}
