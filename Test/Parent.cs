using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

//+ Parent() DONE
//+ ParentID(ID : Int) DONE
//+ del(): String DONE
//+ add(): String DONE
//+ edit(): String DONE
//+getStudents(): List<Student> DONE
//+ addStudent() DONE
//+ delStudent() DONE

namespace Test
{
    public class Parent
    {
        public int ID { get; set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }

        public Parent()
        { }
        public string Add()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.Parents.Add(this);
                    context.SaveChanges();
                    answer = "Добавление ответственного лица прошло успешно";
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
                o = "Удаление ответственного лица прошло успешно";
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
                    answer = "Редактирование ответственного лица прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public List<Student> GetStudents()   // Получение списка учеников этого родителя
        {
            List<Student> liststudents = new List<Student>();
            using (SampleContext db = new SampleContext())
            {
                var students = from p in db.Parents
                               join sp in db.StudentsParents on p.ID equals sp.ParentID
                               join s in db.Students on sp.StudentID equals s.ID
                               select new { SID = s.ID, SPhone = s.Phone, SFIO = s.FIO, SDelDate = s.Deldate, PID = p.ID, ParID = sp.ParentID, StID = sp.StudentID };


                students = students.Where(x => x.ParID == this.ID);
                students = students.Where(x => x.SID == x.StID);

                foreach (var p in students)
                {
                    liststudents.Add(new Student { ID = p.SID, Phone = p.SPhone, Deldate = p.SDelDate, FIO = p.SFIO });  // Добавление учеников в список
                }
                return liststudents;
            }
        }

        public string Сheck(Parent st)
        {
            if (st.FIO == "")
            { return "Введите ФИО ответственного лица. Это поле не может быть пустым"; }
            if (st.Phone == "")
            { return "Введите номер телефона ответственного лица. Это поле не может быть пустым"; }
            using (SampleContext context = new SampleContext())
            {
                Student v = new Student();
                v = context.Students.Where(x => x.FIO == st.FIO && x.Phone == st.Phone).FirstOrDefault<Student>();
                if (v != null)
                { return "Такое ответственное лицо уже существует в базе под номером " + v.ID; }
            }
            return "Данные корректны!";
        }

        public static string СheckSt(StudentsParents stpar)
        {
            using (SampleContext context = new SampleContext())
            {
                StudentsParents v = new StudentsParents();
                v = context.StudentsParents.Where(x => x.StudentID == stpar.StudentID && x.ParentID == stpar.ParentID).FirstOrDefault<StudentsParents>();
                if (v != null)
                { return "Этот ученик уже числится за этим отв. лицом"; }
            }
            return "Данные корректны!";
        }

        public string addStudent(Student st)
        {
            StudentsParents stpar = new StudentsParents();
            stpar.StudentID = st.ID;
            stpar.ParentID = this.ID;
            string answer = СheckSt(stpar);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.StudentsParents.Add(stpar);
                    context.SaveChanges();
                    answer = "Добавление ученика к отв. лицу прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public string delStudent(Student st)
        {
            StudentsParents stpar = new StudentsParents();
            stpar.StudentID = st.ID;
            stpar.ParentID = this.ID;
            string answer = "";

            using (SampleContext context = new SampleContext())
            {
                StudentsParents v = new StudentsParents();
                v = context.StudentsParents.Where(x => x.StudentID == stpar.StudentID && x.ParentID == stpar.ParentID).FirstOrDefault<StudentsParents>();
                context.StudentsParents.Remove(v);
                context.SaveChanges();

                answer = "Удаление ученика у отв. лица прошло успешно";
            }
            return answer;
        }
    }

    public static class Parents
    {
        public static Parent ParentID(int id)
        {
            using (SampleContext db = new SampleContext())
            {
                Parent v = db.Parents.Where(x => x.ID == id).FirstOrDefault<Parent>();

                return v;
            }
        }

        //////////////////// ОДИН БОЛЬШОЙ ПОИСК !!! Если не введены никакие параметры, функция должна возвращать всех родителей //////////////////
        public static List<Parent> FindAll(Boolean deldate, Parent parent, Student student, String sort, String asсdesс, int page, int count, ref int countrecord) //deldate =false - все и удал и неудал!
        {
            List<Parent> parentList = new List<Parent>();

            using (SampleContext db = new SampleContext())
            {
                // Соединение необходимых таблиц
                var parents = from p in db.Parents
                              join sp in db.StudentsParents on p.ID equals sp.ParentID
                              join s in db.Students on sp.StudentID equals s.ID
                              select new { ID = p.ID, Phone = p.Phone, FIO = p.FIO, Deldate = p.Deldate, Editdate = p.Editdate, SPhone = s.Phone, SFIO = s.FIO, SID = s.ID };


                // Последовательно просеиваем наш список

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    parents = parents.Where(x => x.Deldate == null);
                }

                if (student.Phone != null)
                {
                    parents = parents.Where(x => x.SPhone == student.Phone);
                }

                if (student.FIO != null)
                {
                    parents = parents.Where(x => x.SFIO == student.FIO);
                }

                if (student.ID != 0)
                {
                    parents = parents.Where(x => x.SID == student.ID);
                }

                if (parent.FIO != null)
                {
                    parents = parents.Where(x => x.FIO == parent.FIO);
                }

                if (parent.Phone != null)
                {
                    parents = parents.Where(x => x.Phone == parent.Phone);
                }

                var query2 = parents.GroupBy(s => new { s.ID, s.FIO, s.Phone, s.Editdate, s.Deldate }, (key, group) => new
                {
                    ID = key.ID,
                    FIO = key.FIO,
                    Phone = key.Phone,
                    Editdate = key.Editdate,
                    Deldate = key.Deldate,
                });

                if (sort != null) // Сортировка, если нужно
                {
                    query2 = Utilit.OrderByDynamic(query2, sort, asсdesс);
                }


                countrecord = query2.GroupBy(u => u.ID).Count();

                query2 = query2.Skip((page - 1) * count).Take(count); // Формирование страниц и кол-во записей на странице

                foreach (var p in query2)

                    //if (sort != null)  // Сортировка, если нужно
                    //{
                    //    parents = Utilit.OrderByDynamic(parents, sort, askdesk);
                    //}

                    //parents = parents.Skip((page-1) * count).Take(count);  // Формирование страниц и кол-во записей на странице

                    //foreach (var p in parents) 
                    //{
                    //    if (parentList.Find(x => x.ID == p.ID) == null)
                    //    {
                    parentList.Add(new Parent { ID = p.ID, Phone = p.Phone, Deldate = p.Deldate, FIO = p.FIO, Editdate = p.Editdate }); // Добавление родителя в лист, если такого еще нет, это для предохранения от дубликатов
                        //}
            }
            return parentList;
        }
    }
}


