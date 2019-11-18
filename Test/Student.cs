using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Entity;

//+Student() DONE
//+StudentID(ID: Int) DONE
//+ add(): String DONE
//+ del(): String DONE
//+ edit(): String DONE
//+ getParents(): List<Parent> DONE
//+ getContracts(): List<Contract> DONE
//+ getCourses(Finished: Bolean): List<Course>
//+ getTimetable(Start: Datetime, End: Datetime): List<Timetable> 
//+ getDebt(Contract: Contract): Double



namespace Test
{
    public class Student
    {
        public int ID { get; set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }


        public ICollection<Contract> Contracts { get; set; }

        public Student()
        {
            Contracts = new List<Contract>();
        }

        public string Add()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.Students.Add(this);
                    context.SaveChanges();
                    answer = "Добавление ученика прошло успешно";
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
                o = "Удаление ученика прошло успешно";
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
                    answer = "Редактирование ученика прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public string Сheck(Student st)
        {
            if (st.FIO == "")
            { return "Введите ФИО ученика. Это поле не может быть пустым"; }
            if (st.Phone == "")
            { return "Введите номер телефона ученика. Это поле не может быть пустым"; }
            using (SampleContext context = new SampleContext())
            {
                Student v = new Student();
                v = context.Students.Where(x => x.FIO == st.FIO && x.Phone == st.Phone).FirstOrDefault<Student>();
                if (v != null)
                { return "Такой ученик уже существует в базе под номером " + v.ID; }
            }
            return "Данные корректны!";
        }


        public static List<Contract> GetContracts(Student st)
        {
            //      var context = new SampleContext();
            using (SampleContext context = new SampleContext())
            {
                var v = context.Contracts.Where(x => x.StudentID == st.ID).OrderBy(u => u.ID).ToList<Contract>();
                return v;
            }
        }

        public static List<Parent> GetParents(Student s)    // Получение списка родителей этого ученика
        {
            List<Parent> listparents = new List<Parent>();
            using (SampleContext db = new SampleContext())
            {
                var parents = from p in db.Parents
                              join sp in db.StudentsParents on p.ID equals sp.ParentID
                              select new { PID = p.ID, PPhone = p.Phone, PFIO = p.FIO, PDelDate = p.Deldate, ParID = sp.ParentID, StID = sp.StudentID };

                parents = parents.Where(x => x.StID == s.ID);
                parents = parents.Where(x => x.PID == x.ParID);

                foreach (var p in parents)
                {
                    listparents.Add(new Parent { ID = p.PID, Phone = p.PPhone, Deldate = p.PDelDate, FIO = p.PFIO });
                }
                return listparents;
            }
        }
    }

    public static class Students
    {

        public static Student StudentID(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                Student v = context.Students.Where(x => x.ID == id).FirstOrDefault<Student>();

                return v;
            }
        }

        public static List<Student> GetSt()
        {
            //      var context = new SampleContext();
            using (SampleContext context = new SampleContext())
            {
                var students = context.Students.ToList();
                return students;
            }
        }

        //////////////////// ОДИН БОЛЬШОЙ ПОИСК !!! Если не введены никакие параметры, функция должна возвращать всех учеников //////////////////
        public static List<Student> FindAll(Boolean deldate, Parent parent, Student student, Contract contracnt, String sort, String askdesk, int page, int count) //deldate =false - все и удал и неудал!
        {
            List<Student> stList = new List<Student>();

            using (SampleContext db = new SampleContext())
            {

                //////////////////////////////////////////////// НИКАК НЕ МОГУ СДЕЛАТЬ ЛЕВОЕ СОЕДИНЕНИЕ !!!!!!!!!!!!!!!!! //////////////////////////////////


                // Соединение необходимых таблиц
                var query = from s in db.Students
                            join sp in db.StudentsParents on s.ID equals sp.StudentID
                            join p in db.Parents on sp.StudentID equals p.ID
                            join c in db.Contracts on s.ID equals c.StudentID
                            select new { SID = s.ID, SPhone = s.Phone, SFIO = s.FIO, SDelDate = s.Deldate, PID = p.ID, CID = c.ID };

                //IQueryable<Student> query = from s in db.Students
                //             join sp in db.StudentsParents
                //                  on s.ID equals sp.StudentID into studentGroup
                //             from m in studentGroup.DefaultIfEmpty()
                //             join c in db.Contracts
                //                  on m.StudentID equals c.StudentID into contractGroup
                //             from co in contractGroup.DefaultIfEmpty()
                //             join p in db.Parents
                //                  on sp.ParentID equals p.ID into contractGroup
                //             from co in contractGroup.DefaultIfEmpty()
                //                            select new { SID = s.ID, SPhone = s.Phone, SFIO = s.FIO, SDelDate = s.Deldate, PID = p.ID, CID = c.ID };





                //             IQueryable<Student> v = db.Database.SqlQuery
                //                 ("select * from Contracts Where Contracts.StudentID =" + "'" + id + "'" + "and Contracts.ManagerID =" + "'" + idm + "'");

                //from s in db.Students
                //join sp in db.StudentsParents
                //     on s.ID equals sp.StudentID into studentGroup
                //from m in studentGroup.DefaultIfEmpty()
                //join c in db.Contracts
                //     on m.StudentID equals c.StudentID into contractGroup
                //from p in contractGroup.DefaultIfEmpty()




                //from s in context.dc_tpatient_bookingd
                //join bookingm in context.dc_tpatient_bookingm
                //     on d.bookingid equals bookingm.bookingid into bookingmGroup
                //from m in bookingmGroup.DefaultIfEmpty()
                //join patient in dc_tpatient
                //     on m.prid equals patient.prid into patientGroup
                //from p in patientGroup.DefaultIfEmpty()




                // Последовательно просеиваем наш список

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    query = query.Where(x => x.SDelDate == null);
                }

                if (parent.ID != 0)
                {
                    query = query.Where(x => x.PID == parent.ID);
                }

                if (student.FIO != null)
                {
                    query = query.Where(x => x.SFIO == student.FIO);
                }

                if (student.Phone != null)
                {
                    query = query.Where(x => x.SPhone == student.Phone);
                }

                if (contracnt.ID != 0)
                {
                    query = query.Where(x => x.CID == contracnt.ID);
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
                else { query = query.OrderBy(u => u.PID); }

                query = query.Skip((page - 1) * count).Take(count);  // Формирование страниц и кол-во записей на странице

                foreach (var p in query)
                {
                    if (stList.Find(x => x.ID == p.SID) == null)
                    {
                        stList.Add(new Student { ID = p.SID, Phone = p.SPhone, Deldate = p.SDelDate, FIO = p.SFIO }); // Добавление ученика в лист, если такого еще нет, это для предохранения от дубликатов
                    }
                }
                return stList;
            }
        }

        }
}
