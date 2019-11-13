using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Parentold
    {
        public int ID { get; set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }

        public Parentold()
        {
        }
        public Parentold(int ID)
        {

        }
    }
    public class Parentsold
    {
        public static List<Parent> GetPa(SampleContext context)
        {
            //      var context = new SampleContext();

            var parents = context.Parents.ToList();
            return parents;
        }

        public static List<Parent> FindAll(Boolean deldate, String fio, String phone, Student student, String sort, String askdesk) //deldate =false - удален!
        {
            List<Parent> par = new List<Parent>();
            if (student.FIO != null && phone ==null && deldate== false && fio == null) // Поиск по одному полю - ученик
            {
                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        //                       dataGridView.Rows.Clear();
                        List<Parent> parent = new List<Parent>();
                        Student w = db.Students.Where(x => x.FIO == student.FIO).FirstOrDefault(); //Нашли ученика, которого передали - в текст бокс
                        List<StudentsParents> wp = db.StudentsParents.Where(x => x.StudentID == w.ID).ToList(); // Ищем в таблице ID родителей по ID ученика
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var va = db.Parents.Where(x => x.ID == q.ParentID).ToList<Parent>().OrderByDescending(u => sort);
                                    foreach (Parent p in va)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                            else
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var vs = db.Parents.Where(x => x.ID == q.ParentID).ToList<Parent>().OrderBy(u => sort);
                                    //var v = db.Parents.Find(q.ParentID).ToList<Parent>().OrderBy(u => u.FIO);
                                    foreach (Parent p in vs)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                        }
                        else // сортировки нет
                        {
                            foreach (StudentsParents q in wp)
                            {
                                parent.Add(db.Parents.Find(q.ParentID));
                            }
                            return parent;
                        }

                        //foreach (Parent p in parent)
                        //{
                        //    dataGridView.Rows.Add(пареметры);
                        //}
                        //if (parent.Count == 0)
                        //{
                        //    MessageBox.Show("Ответственные лица не найдены", "Образовательный центр");
                        //}
                    }
                }
                catch
                {
                }
            }


            if (deldate != false && phone == null && student.FIO == null && fio == null)// Поиск по одному полю - неудаленные
            {
                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                var vq = db.Parents.Where(x => x.Deldate == null).ToList<Parent>().OrderByDescending(u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                            else
                            {
                                var vq = db.Parents.Where(x => x.Deldate == null).ToList<Parent>().OrderBy( u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                        else
                        {
                            parent = db.Parents.Where(x => x.Deldate == null).ToList<Parent>();
                            return parent;
                        }
                    }
                }
                catch
                {
                }
            }

            if (fio !=null && phone == null && student.FIO == null && deldate == false)// Поиск по одному полю - фио
            {
                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                var vq = db.Parents.Where(x => x.FIO == fio).ToList<Parent>().OrderByDescending(u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                            else
                            {
                                var vq = db.Parents.Where(x => x.FIO == fio).ToList<Parent>().OrderBy(u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                        else
                        {
                            parent = db.Parents.Where(x => x.FIO == fio).ToList<Parent>();
                            return parent;
                        }
                    }
                }
                catch
                {
                }
            }

            if (phone != null && student.FIO == null && deldate == false && fio == null)// Поиск по одному полю - телефон
            {
                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                var vq = db.Parents.Where(x => x.Phone == phone).ToList<Parent>().OrderByDescending(u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                            else
                            {
                                var vq = db.Parents.Where(x => x.Phone == phone).ToList<Parent>().OrderBy(u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                        else
                        {
                            parent = db.Parents.Where(x => x.Phone == phone).ToList<Parent>();
                            return parent;
                        }
                    }
                }
                catch
                {
                }
            }

            ////////////// Поиски по 2 параметрам. 6 штук
            if (student.FIO != null && deldate != false && phone == null && fio == null) // Поиск по  полям - ученик и неудален
            {
                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        Student w = db.Students.Where(x => x.FIO == student.FIO).FirstOrDefault(); //Нашли ученика по фио, которого передали - в текст бокс
                        List<StudentsParents> wp = db.StudentsParents.Where(x => x.StudentID == w.ID).ToList(); // Ищем в таблице ID родителей по ID ученика
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var va = db.Parents.Where(x => x.Deldate == null && x.ID == q.ParentID).ToList<Parent>().OrderByDescending(u => sort);
                                    foreach (Parent p in va)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                            else
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var vs = db.Parents.Where(x => x.Deldate == null && x.ID == q.ParentID).ToList<Parent>().OrderBy(u => sort);
                                    //var v = db.Parents.Find(q.ParentID).ToList<Parent>().OrderBy(u => u.FIO);
                                    foreach (Parent p in vs)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                        }
                        else // сортировки нет
                        {
                            foreach (StudentsParents q in wp)
                            {
                                var vs = db.Parents.Where(x => x.Deldate == null && x.ID == q.ParentID).ToList<Parent>();
                                foreach (Parent p in vs)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                    }
                }
                catch
                {
                }
            }


            if (student.FIO != null && fio != null && phone == null && deldate == false) // Поиск по  полям - ученик и фио
            {

                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        Student w = db.Students.Where(x => x.FIO == student.FIO).FirstOrDefault(); //Нашли ученика по фио, которого передали - в текст бокс
                        List<StudentsParents> wp = db.StudentsParents.Where(x => x.StudentID == w.ID).ToList(); // Ищем в таблице ID родителей по ID ученика
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var va = db.Parents.Where(x => x.FIO == fio && x.ID == q.ParentID).ToList<Parent>().OrderByDescending(u => sort);
                                    foreach (Parent p in va)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                            else
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var vs = db.Parents.Where(x => x.FIO == fio && x.ID == q.ParentID).ToList<Parent>().OrderBy(u => sort);
                                    foreach (Parent p in vs)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                        }
                        else // сортировки нет
                        {
                            foreach (StudentsParents q in wp)
                            {
                                var vs = db.Parents.Where(x => x.FIO == fio && x.ID == q.ParentID).ToList<Parent>();
                                foreach (Parent p in vs)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            if (student.FIO != null && phone != null && deldate == false && fio == null) // Поиск по  полям - ученик и телефон
            {

                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        Student w = db.Students.Where(x => x.FIO == student.FIO).FirstOrDefault(); //Нашли ученика по фио, которого передали - в текст бокс
                        List<StudentsParents> wp = db.StudentsParents.Where(x => x.StudentID == w.ID).ToList(); // Ищем в таблице ID родителей по ID ученика
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var va = db.Parents.Where(x => x.Phone == phone && x.ID == q.ParentID).ToList<Parent>().OrderByDescending(u => sort);
                                    foreach (Parent p in va)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                            else
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var vs = db.Parents.Where(x => x.Phone == phone && x.ID == q.ParentID).ToList<Parent>().OrderBy(u => sort);
                                    foreach (Parent p in vs)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                        }
                        else // сортировки нет
                        {
                            foreach (StudentsParents q in wp)
                            {
                                var vs = db.Parents.Where(x => x.Phone == phone && x.ID == q.ParentID).ToList<Parent>();
                                foreach (Parent p in vs)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            if (deldate != false && fio != null && phone == null && student.FIO == null)// Поиск по  полям - неудаленные и фио
            {
                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                var vq = db.Parents.Where(x => x.FIO == fio && x.Deldate == null).ToList<Parent>().OrderByDescending(u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                            else
                            {
                                var vq = db.Parents.Where(x => x.FIO == fio && x.Deldate == null).ToList<Parent>().OrderBy(u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                        else
                        {
                            parent = db.Parents.Where(x => x.FIO == fio && x.Deldate == null).ToList<Parent>();
                            return parent;
                        }
                    }
                }
                catch
                {
                }
            }

            if (deldate != false && phone != null && student.FIO == null && fio == null)// Поиск по  полям - неудаленные и телефон
            {
                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                var vq = db.Parents.Where(x => x.Phone == phone && x.Deldate == null).ToList<Parent>().OrderByDescending(u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                            else
                            {
                                var vq = db.Parents.Where(x => x.Phone == phone && x.Deldate == null).ToList<Parent>().OrderBy(u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                        else
                        {
                            parent = db.Parents.Where(x => x.Phone == phone && x.Deldate == null).ToList<Parent>();
                            return parent;
                        }
                    }
                }
                catch
                {
                }
            }

            if (fio != null && phone != null && student.FIO == null && deldate == false)// Поиск по  полям - фио и телефон
            {
                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                var vq = db.Parents.Where(x => x.Phone == phone && x.FIO == fio).ToList<Parent>().OrderByDescending(u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                            else
                            {
                                var vq = db.Parents.Where(x => x.Phone == phone && x.FIO == fio).ToList<Parent>().OrderBy(u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                        else
                        {
                            parent = db.Parents.Where(x => x.Phone == phone && x.FIO == fio).ToList<Parent>();
                            return parent;
                        }
                    }
                }
                catch
                {
                }
            }


            ////////////// Поиски по 3 параметрам. 4 штуки
            if (student.FIO != null && deldate != false && fio != null && phone == null) // Поиск по  полям - ученик, неудален и фио
            {
                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        Student w = db.Students.Where(x => x.FIO == student.FIO).FirstOrDefault(); //Нашли ученика по фио, которого передали - в текст бокс
                        List<StudentsParents> wp = db.StudentsParents.Where(x => x.StudentID == w.ID).ToList(); // Ищем в таблице ID родителей по ID ученика
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var va = db.Parents.Where(x => x.Deldate == null && x.ID == q.ParentID && x.FIO == fio).ToList<Parent>().OrderByDescending(u => sort);
                                    foreach (Parent p in va)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                            else
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var vs = db.Parents.Where(x => x.Deldate == null && x.ID == q.ParentID && x.FIO == fio).ToList<Parent>().OrderBy(u => sort);
                                    //var v = db.Parents.Find(q.ParentID).ToList<Parent>().OrderBy(u => u.FIO);
                                    foreach (Parent p in vs)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                        }
                        else // сортировки нет
                        {
                            foreach (StudentsParents q in wp)
                            {
                                var vs = db.Parents.Where(x => x.Deldate == null && x.ID == q.ParentID && x.FIO == fio).ToList<Parent>();
                                foreach (Parent p in vs)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            if (student.FIO != null && deldate != false && phone != null && fio == null) // Поиск по  полям - ученик, неудален и телефон
            {
                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        Student w = db.Students.Where(x => x.FIO == student.FIO).FirstOrDefault(); //Нашли ученика по фио, которого передали - в текст бокс
                        List<StudentsParents> wp = db.StudentsParents.Where(x => x.StudentID == w.ID).ToList(); // Ищем в таблице ID родителей по ID ученика
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var va = db.Parents.Where(x => x.Deldate == null && x.ID == q.ParentID && x.Phone == phone).ToList<Parent>().OrderByDescending(u => sort);
                                    foreach (Parent p in va)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                            else
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var vs = db.Parents.Where(x => x.Deldate == null && x.ID == q.ParentID && x.Phone == phone).ToList<Parent>().OrderBy(u => sort);
                                    //var v = db.Parents.Find(q.ParentID).ToList<Parent>().OrderBy(u => u.FIO);
                                    foreach (Parent p in vs)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                        }
                        else // сортировки нет
                        {
                            foreach (StudentsParents q in wp)
                            {
                                var vs = db.Parents.Where(x => x.Deldate == null && x.ID == q.ParentID && x.Phone == phone).ToList<Parent>();
                                foreach (Parent p in vs)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            if (student.FIO != null && fio != null && phone != null && deldate == false) // Поиск по  полям - ученик, фио и телефон
            {
                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        Student w = db.Students.Where(x => x.FIO == student.FIO).FirstOrDefault(); //Нашли ученика по фио, которого передали - в текст бокс
                        List<StudentsParents> wp = db.StudentsParents.Where(x => x.StudentID == w.ID).ToList(); // Ищем в таблице ID родителей по ID ученика
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var va = db.Parents.Where(x => x.FIO == fio && x.ID == q.ParentID && x.Phone == phone).ToList<Parent>().OrderByDescending(u => sort);
                                    foreach (Parent p in va)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                            else
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var vs = db.Parents.Where(x => x.FIO == fio && x.ID == q.ParentID && x.Phone == phone).ToList<Parent>().OrderBy(u => sort);
                                    //var v = db.Parents.Find(q.ParentID).ToList<Parent>().OrderBy(u => u.FIO);
                                    foreach (Parent p in vs)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                        }
                        else // сортировки нет
                        {
                            foreach (StudentsParents q in wp)
                            {
                                var vs = db.Parents.Where(x => x.FIO == fio && x.ID == q.ParentID && x.Phone == phone).ToList<Parent>();
                                foreach (Parent p in vs)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            if (fio != null && phone != null && deldate != false && student.FIO == null)// Поиск по  полям - фио, неудален и телефон
            {
                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                var vq = db.Parents.Where(x => x.Phone == phone && x.FIO == fio && x.Deldate == null).ToList<Parent>().OrderByDescending(u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                            else
                            {
                                var vq = db.Parents.Where(x => x.Phone == phone && x.FIO == fio && x.Deldate == null).ToList<Parent>().OrderBy(u => sort);
                                foreach (Parent p in vq)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                        else
                        {
                            parent = db.Parents.Where(x => x.Phone == phone && x.FIO == fio && x.Deldate == null).ToList<Parent>();
                            return parent;
                        }
                    }
                }
                catch
                {
                }
            }

            ////////////// Поиски по 4 параметрам. 1 штука
            if (student.FIO != null && deldate != false && fio != null && phone != null) // Поиск по  полям - ученик, неудален, телефон и фио
            {
                try
                {
                    using (SampleContext db = new SampleContext())
                    {
                        List<Parent> parent = new List<Parent>();
                        Student w = db.Students.Where(x => x.FIO == student.FIO).FirstOrDefault(); //Нашли ученика по фио, которого передали - в текст бокс
                        List<StudentsParents> wp = db.StudentsParents.Where(x => x.StudentID == w.ID).ToList(); // Ищем в таблице ID родителей по ID ученика
                        if (sort != null)  //сортировка
                        {
                            if (askdesk == "desk")
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var va = db.Parents.Where(x => x.Deldate == null && x.ID == q.ParentID && x.FIO == fio && x.Phone == phone).ToList<Parent>().OrderByDescending(u => sort);
                                    foreach (Parent p in va)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                            else
                            {
                                foreach (StudentsParents q in wp)
                                {
                                    var vs = db.Parents.Where(x => x.Deldate == null && x.ID == q.ParentID && x.FIO == fio && x.Phone == phone).ToList<Parent>().OrderBy(u => sort);
                                    //var v = db.Parents.Find(q.ParentID).ToList<Parent>().OrderBy(u => u.FIO);
                                    foreach (Parent p in vs)
                                    {
                                        parent.Add(p);
                                    }
                                }
                                return parent;
                            }
                        }
                        else // сортировки нет
                        {
                            foreach (StudentsParents q in wp)
                            {
                                var vs = db.Parents.Where(x => x.Deldate == null && x.ID == q.ParentID && x.FIO == fio && x.Phone == phone).ToList<Parent>();
                                foreach (Parent p in vs)
                                {
                                    parent.Add(p);
                                }
                                return parent;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            return par;
        }
    }
}
