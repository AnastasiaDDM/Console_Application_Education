using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Parent1
    {
        public int ID { get; set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }

        public Parent1()
        {
        }
        public Parent1(int ID)
        {

        }
    }
    public class Parents1
    {
        public static List<Parent> GetPa(SampleContext context)
        {
            //      var context = new SampleContext();

            var parents = context.Parents.ToList();
            return parents;
        }

        public static List<Parent> FindAll(Boolean deldate, String fio, String phone, Student student, String sort, String askdesk) //deldate =false - удален!
        {

            List<Parent> parentList = new List<Parent>();

            using (SampleContext db = new SampleContext())
            {
                IQueryable<Parent> parents;
                
                if (student.FIO != null)
                {
                    parents = from p in db.Parents
                              join sp in db.StudentsParents on p.ID equals sp.ParentID
                              join s in db.Students on sp.StudentID equals s.ID
                              where s.FIO == student.FIO
                              select p; // или select new Parent { Phone = p.Phone , ... }; // можно вообще новый объект другого класса создать (который будет содержать и родителя, и студента), если нужно отдельно искать по фамилии студента, телефону студента и пр. , тогда и else ниже будет не нужно
                    // https://metanit.com/sharp/entityframework/4.4.php
                    // https://docs.microsoft.com/ru-ru/dotnet/framework/data/adonet/ef/language-reference/method-based-query-syntax-examples-join-operators
                }
                else
                {
                    parents = from p in db.Parents
                              select p; 
                }

                // Последовательно просеиваем наш список 

                if (fio != null)
                {
                    parents = parents.Where(x => x.FIO == fio);
                }

                if (phone != null)
                {
                    parents = parents.Where(x => x.Phone == phone);
                }

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    parents = parents.Where(x => x.Deldate == null);
                }

                if (sort != null)  // Сортировка, если нужно
                {
                    if (askdesk == "desk")
                    {
                        parents = parents.OrderByDescending(u => sort);
                    }
                    else
                    {
                        parents = parents.OrderBy(u => sort);
                    }

                }

                foreach (var p in parents) // Заносим все в итоговый список, хотя просто можно  parentList = parents.ToList<Parent>(); Этот цикл нужен, если мы ранее создали новый объект, который содержал и родителя, и студента.
                {
                    parentList.Add(p);
                }
                          

                return parentList;

            }
        }
    }
}
