using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Entity;


namespace Test
{
    public class Student
    {
        public int ID { get;  set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }


        public Student()
        { }
        public Student StudentID(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                Student v = context.Students.Where(x => x.ID == id).FirstOrDefault<Student>();

                return v;
            }
        }

        public string Add()
        {
            string o;
            using (SampleContext context = new SampleContext())
            {
                context.Students.Add(this);
                context.SaveChanges();
                o="Добавление ученика прошло успешно";
            }
            return o;
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
            string o;
            using (SampleContext context = new SampleContext())
            {
    //            Student v = context.Students.Where(x => x.ID == id).FirstOrDefault<Student>();
                this.Editdate = DateTime.Now;
                context.Entry(this).State = EntityState.Modified;
                context.SaveChanges();
                o = "Редактирование ученика прошло успешно";
            }
            return o;
        }

        public static List<Parent> GetParents(int id)    // Получение списка родителей этого ученика
        {
            List<Parent> listparents = new List<Parent>();
            using (SampleContext db = new SampleContext())
            {
                var parents = from p in db.Parents
                               join sp in db.StudentsParents on p.ID equals sp.ParentID   
                               select new { PID = p.ID, PPhone = p.Phone, PFIO = p.FIO, PDelDate = p.Deldate, ParID = sp.ParentID, StID = sp.StudentID };

                //             Parent par = ParentID(); запрос на получение текущего родителя (ParentID) будет проходить из формы !!!!!!! 
                parents = parents.Where(x => x.StID == id);
                parents = parents.Where(x => x.PID == x.ParID);

                foreach (var p in parents)
                {
                    listparents.Add(new Parent { ID = p.PID, Phone = p.PPhone, Deldate = p.PDelDate, FIO = p.PFIO });
                }

                return listparents;
            }
        }
    }

    public  class Students
    {
        public static List<Student> GetSt()
        {
            //      var context = new SampleContext();
            using (SampleContext context = new SampleContext())
            {
                var students = context.Students.ToList();
                return students;
            }
        }
    }
}
