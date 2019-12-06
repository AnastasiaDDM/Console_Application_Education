using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Itenso.TimePeriod;

//+ edit(): String
//+ add(): String
//+ del(): String
//+ addTeachers(): String DONE
//+ delTeachers(): String DONE
//+ GetFreeteachers(DateTime date) : List<Worker> 
//+ findAll(): List<Timetable> 

namespace Test
{
    public class Timetable
    {
        public int ID { get; set; }
        public DateTime Startlesson { get; set; }
        public DateTime Endlesson { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }

        public int CabinetID { get; set; }
        public Cabinet Cabinet { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }

        public string Add()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.Timetables.Add(this);
                    context.SaveChanges();
                    answer = "Добавление элемента расписания прошло успешно";
                }
            }
            return answer;
        }

        public string Add(DateTime Endrepeat, string period, Timetable timetable)
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                List<Timetable> listtimetable = new List<Timetable>();
                List<TimeRange> listtimerange = new List<TimeRange>();

                //Ежедневно
                //Еженедельно
                //Ежемесячно
                //Каждый год
                //Каждый будний день(пн - пт)

                DateTime newstart = timetable.Startlesson; // Присваиваем дате начала занятия пока что начальное значение переданное извне, далее эта переменная будет изменяться
                DateTime newend = timetable.Endlesson; // Присваиваем дате окончания занятия пока что начальное значение переданное извне, далее эта переменная будет изменяться


                while (newend <= Endrepeat) // Организуем цикл для перебора всех дат в заданном диапазоне, т.е. до Endrepeat
                {
                    using (SampleContext context = new SampleContext())
                    {
                        Timetable v = context.Timetables.Where(x => x.Startlesson == newstart).FirstOrDefault<Timetable>();

                        // В первом проходе добавляется или не добавляется начальная дата, а дальше уже происходит увеличение дат
                        if (v == null & (period != "Каждый будний день(пн - пт)")) // Добавление для всех вариантов, кроме будних дней, т.к. не нужно учитывать выходные!
                        {
                            Timetable newtimetable = new Timetable(); // Создаем новый экземпляр класса
                            newtimetable.CabinetID = timetable.CabinetID; // Добавляем неизменные атрибуты в новый объект из переданного объекта
                            newtimetable.CourseID = timetable.CourseID;  // Добавляем неизменные атрибуты в новый объект из переданного объекта
                            newtimetable.Note = timetable.Note;  // Добавляем неизменные атрибуты в новый объект из переданного объекта
                            newtimetable.Startlesson = newstart; // Добавляем изменяемые атрибуты (дата начала занятия) в новый объект
                            newtimetable.Endlesson = newend; // Добавляем изменяемые атрибуты (дата окончания занятия) в новый объект
                            listtimetable.Add(newtimetable); // Добавление объекта в лист


                            TimeRange Range = new TimeRange(newstart, newend);
                            listtimerange.Add(Range);


                        }

                        if ((period == "Каждый будний день(пн - пт)") & v == null & (newstart.DayOfWeek != DayOfWeek.Saturday & newstart.DayOfWeek != DayOfWeek.Sunday)) // Добавление для варианта будних дней, т.к. нужно учитывать выходные и не добавлять такие дни в список!
                        {
                            Timetable newtimetable = new Timetable(); // Создаем новый экземпляр класса
                            newtimetable.CabinetID = timetable.CabinetID; // Добавляем неизменные атрибуты в новый объект из переданного объекта
                            newtimetable.CourseID = timetable.CourseID;  // Добавляем неизменные атрибуты в новый объект из переданного объекта
                            newtimetable.Note = timetable.Note;  // Добавляем неизменные атрибуты в новый объект из переданного объекта
                            newtimetable.Startlesson = newstart; // Добавляем изменяемые атрибуты (дата начала занятия) в новый объект
                            newtimetable.Endlesson = newend; // Добавляем изменяемые атрибуты (дата окончания занятия) в новый объект
                            listtimetable.Add(newtimetable); // Добавление объекта в лист


                            TimeRange Range = new TimeRange(newstart, newend);
                            listtimerange.Add(Range);
                        }
                    }

                    if (period == "Ежедневно") // Изменение дат исходя из условия
                    {
                        newstart = newstart.AddDays(1);

                        newend = newend.AddDays(1);
                    }

                    if (period == "Еженедельно") // Изменение дат исходя из условия
                    {
                        newstart = newstart.AddDays(7);

                        newend = newend.AddDays(7);
                    }

                    if (period == "Ежемесячно") // Изменение дат исходя из условия
                    {
                        newstart = newstart.AddMonths(1);

                        newend = newend.AddMonths(1);
                    }

                    if (period == "Каждый год") // Изменение дат исходя из условия
                    {
                        newstart = newstart.AddYears(1);

                        newend = newend.AddYears(1);
                    }

                    if (period == "Каждый будний день(пн - пт)") // Изменение дат исходя из условия
                    {
                        //if (newstart.DayOfWeek != DayOfWeek.Saturday || newend.DayOfWeek != DayOfWeek.Sunday)
                        {
                            newstart = newstart.AddDays(1);

                            newend = newend.AddDays(1);
                        }
                    }
                }

                using (SampleContext context = new SampleContext()) // После завершения цикла нужно добавить значения листа в бд
                {
                    context.Timetables.AddRange(listtimetable);
                    context.SaveChanges();
                    if (listtimetable.Count == 0) // Может быть ситуация, при которой ни один объект не был добавлен в бд, пользователь будет осведомлен
                    {
                        return answer = "Ни один элемент расписания не был добавлен";
                    }
                }
            }
            return answer = "Добавление элемента(ов) расписания прошло успешно";
        }

        public string Del()
        {
            string o;
            using (SampleContext context = new SampleContext())
            {
                this.Deldate = DateTime.Now;
                context.Entry(this).State = EntityState.Modified;
                context.SaveChanges();
                o = "Удаление элемента расписания прошло успешно";
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
                    answer = "Редактирование элемента расписания прошло успешно";
                }
                return answer;
            }
            return answer;
        }


        public string addTeacher(Worker w)
        {
            TimetablesTeachers cw = new TimetablesTeachers();
            cw.TimetableID = this.ID;
            cw.TeacherID = w.ID;
            string answer = СheckTeac(cw);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    context.TimetablesTeachers.Add(cw);
                    context.SaveChanges();
                    answer = "Добавление преподавателя на это занятие прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public string delTeacher(Worker w)
        {
            TimetablesTeachers cw = new TimetablesTeachers();
            cw.TimetableID = this.ID;
            cw.TeacherID = w.ID;
            string answer = "";

            using (SampleContext context = new SampleContext())
            {
                TimetablesTeachers v = new TimetablesTeachers();
                v = context.TimetablesTeachers.Where(x => x.TeacherID == cw.TeacherID && x.TimetableID == cw.TimetableID).FirstOrDefault<TimetablesTeachers>();
                context.TimetablesTeachers.Remove(v);
                context.SaveChanges();

                answer = "Удаление преподавателя с этого занятия прошло успешно";
            }
            return answer;
        }

        public string Сheck(Timetable st)
        {
            if (st.CabinetID == 0)
            { return "Выберите кабинет. Это поле не может быть пустым"; }

            if (st.CourseID == 0)
            { return "Выберите курс. Это поле не может быть пустым"; }

            using (SampleContext context = new SampleContext())
            {
                Timetable v = context.Timetables.Where(x => x.Startlesson == st.Startlesson & x.CourseID == st.CourseID).FirstOrDefault<Timetable>();
                if (v != null)
                { return "Этот курс уже занят элементом расписания №" + v.ID + " в промежутке от " + v.Startlesson + " до " + v.Endlesson; }

                Timetable c = context.Timetables.Where(x => x.Startlesson == st.Startlesson & x.CabinetID == st.CabinetID).FirstOrDefault<Timetable>();
                if (c != null)
                { return "Этот кабинет уже занят элементом расписания №" + c.ID + " в промежутке от " + c.Startlesson + " до " + c.Endlesson; }

                List<Student> liststudents = Courses.CourseID(st.CourseID).GetStudents();
                foreach (Student s in liststudents)
                {
                    List<Course> listcourses = s.GetCourses();
                    foreach (Course co in listcourses)
                    {
                        Timetable ts = context.Timetables.Where(x => x.Startlesson == st.Startlesson & x.CourseID == co.ID).FirstOrDefault<Timetable>();
                        if (ts != null)
                        { return "Ученик №" + s.ID + " уже занят на курсе №" + co.ID + " элементом расписания №" + ts.ID + " в промежутке от " + ts.Startlesson + " до " + ts.Endlesson; }
                    }
                }
            }
            return "Данные корректны!";
        }


        public static string СheckTeac(TimetablesTeachers stpar)
        {
            using (SampleContext context = new SampleContext())
            {
                TimetablesTeachers v = new TimetablesTeachers();
                v = context.TimetablesTeachers.Where(x => x.TeacherID == stpar.TeacherID && x.TimetableID == stpar.TimetableID).FirstOrDefault<TimetablesTeachers>();
                if (v != null)
                { return "Этот преподаватель уже числится за этим занятием"; }

                Worker t = Workers.WorkerID(stpar.TeacherID);
                if (t.Type != 3)
                { return " Вам нужно было выбрать преподавателя (тип 3)"; }
            }
            return "Данные корректны!";
        }

        public List<Worker> GetFreeteachers(DateTime Endrepeat, string period)  // Поиск свободных на это время преподавателей
        {

            //        List<Worker> teachers = context.Workers.Where(x => x.Type == 3).ToList<Worker>(); // Отбор только преподавателей
            //        foreach (Worker t in teachers) // Цикл по каждому преподавателю, для поиска всех эл. расписания, которые еще ведет этот преподаватель
            //        {
            //            List<TimetablesTeachers> timetablesT = context.TimetablesTeachers.Where(x => x.TeacherID == t.ID).ToList<TimetablesTeachers>();
            //            if (timetablesT.Count != 0) // Если список не пуст, значит нужно проверить каждый эл. расписания из этого листа на временное перекрытие с заданным диапазоном
            //            {
            //                foreach (TimetablesTeachers tt in timetablesT) // Цикл для каждого элемента расписания одного преподавателя
            //                {
            //                    Timetable onetimetable = Timetables.TimetableID(tt.TimetableID); // эл. расписания, который есть у преподавателя (найденный по ID)
            //                    TimeRange timetablerange = new TimeRange(onetimetable.Startlesson, onetimetable.Endlesson); // Составление интервала для поиска временных перекрытий

            //                    bool overlap = Overlap(timetablerange, listtimerange); // Вызов метода, где проверяется конкретный промежуток расписания с заданным диапазоном!
            //                    if(overlap == true) // Если эл. расписания не перекрывается ни с одним промежутком из диапазона, то 
            //                    {

            //                    }


            //                }

            //            }
            //            else
            //            {
            //                freeteachers.Add(t);
            //            }
            //        }

            //    }

            //}

            //        Select* from Worker where
            //Type = [Учитель]
            //        and
            //ID not in
            //(
            //Select ID_Teacher from Timetable join TimetableTeachers
            //on TimetableTeachers.ID_Timetable = Timetable.ID

            //Start_date >= [1_дата_начало] and EndDate <= [1_дата_конец] — вот эти штуки нужно делать в цикле
            //or
            //Start_date >= [2_дата_начало] and EndDate <= [2_дата_конец]
            //or
            //...
            //or
            //Start_date >= [N_дата_начало] and EndDate <= [N_дата_конец]
            //)



                List<TimeRange> listtimerange = new List<TimeRange>();

                //Ежедневно
                //Еженедельно
                //Ежемесячно
                //Каждый год
                //Каждый будний день(пн - пт)

                DateTime newstart = this.Startlesson; // Присваиваем дате начала занятия пока что начальное значение переданное извне, далее эта переменная будет изменяться
                DateTime newend = this.Endlesson; // Присваиваем дате окончания занятия пока что начальное значение переданное извне, далее эта переменная будет изменяться


                while (newend <= Endrepeat) // Организуем цикл для перебора всех дат в заданном диапазоне, т.е. до Endrepeat
                {
                    using (SampleContext context = new SampleContext())
                    {
                        Timetable v = context.Timetables.Where(x => x.Startlesson == newstart).FirstOrDefault<Timetable>();

                        // В первом проходе добавляется или не добавляется начальная дата, а дальше уже происходит увеличение дат
                        if (v == null & (period != "Каждый будний день(пн - пт)")) // Добавление для всех вариантов, кроме будних дней, т.к. не нужно учитывать выходные!
                        {
                            TimeRange Range = new TimeRange(newstart, newend);
                            listtimerange.Add(Range);


                        }

                        if ((period == "Каждый будний день(пн - пт)") & v == null & (newstart.DayOfWeek != DayOfWeek.Saturday & newstart.DayOfWeek != DayOfWeek.Sunday)) // Добавление для варианта будних дней, т.к. нужно учитывать выходные и не добавлять такие дни в список!
                        {
                            TimeRange Range = new TimeRange(newstart, newend);
                            listtimerange.Add(Range);
                        }
                    }

                    if (period == "Ежедневно" || period == "Не повторять" || period == "Каждый будний день(пн - пт)") // Изменение дат исходя из условия
                    {
                        newstart = newstart.AddDays(1);

                        newend = newend.AddDays(1);
                    }

                    if (period == "Еженедельно") // Изменение дат исходя из условия
                    {
                        newstart = newstart.AddDays(7);

                        newend = newend.AddDays(7);
                    }

                    if (period == "Ежемесячно") // Изменение дат исходя из условия
                    {
                        newstart = newstart.AddMonths(1);

                        newend = newend.AddMonths(1);
                    }

                    if (period == "Каждый год") // Изменение дат исходя из условия
                    {
                        newstart = newstart.AddYears(1);

                        newend = newend.AddYears(1);
                    }

                    //if (period == "Каждый будний день(пн - пт)") // Изменение дат исходя из условия
                    //{
                    //    {
                    //        newstart = newstart.AddDays(1);

                    //        newend = newend.AddDays(1);
                    //    }
                    //}
                }

            List<Worker> freeteachers = new List<Worker>(); // Лист свободных преподавателей
            using (SampleContext context = new SampleContext())
            {
                

                StringBuilder s = new StringBuilder("Select Distinct Workers.* from Workers where Workers.Type = 3 and Workers.ID not in (Select Distinct Workers.ID from Workers join TimetablesTeachers on TimetablesTeachers.TeacherID = Workers.ID and Workers.Type = 3 join Timetables on TimetablesTeachers.TimetableID = Timetables.ID where ");

                List<string> sql = new List<string>();
                string format = "yyyy-MM-dd HH:mm:ss";

                foreach (TimeRange t in listtimerange)
                {

                    sql.Add(String.Format("(Startlesson >= '{0}' and Endlesson <= '{1}' and Startlesson <= '{1}')", t.Start.ToString(format), t.End.ToString(format))); // Внутри
                    sql.Add(String.Format("(Startlesson <= '{0}' and Endlesson >= '{1}' and Startlesson <= '{1}')", t.Start.ToString(format), t.End.ToString(format))); // Снаружи
                    sql.Add(String.Format("(Startlesson >= '{0}' and Endlesson >= '{1}' and Startlesson <= '{1}')", t.Start.ToString(format), t.End.ToString(format))); // верхняя граница
                    sql.Add(String.Format("(Startlesson <= '{0}' and Endlesson <= '{1}' and Endlesson >= '{0}')", t.Start.ToString(format), t.End.ToString(format)));// нижняя граница

                }

                s.Append(String.Join(" or ", sql));

                var query = context.Workers.SqlQuery(s.ToString() + " group by Workers.ID order by Workers.ID)");

                foreach (Worker t in query)
                {
                    freeteachers.Add(t);
                }
            }
            return freeteachers;
        }


    //    private bool Overlap(TimeRange timetablerange, List<TimeRange> listtimerange)
    //    {
    //        foreach (TimeRange date in listtimerange)
    //        {
    //            //// Теперь этот onetimetable нужно сравнить с каждой датой! - и если совпадений нет, то добавить этого преподавателя в лист.

    //            if (timetablerange.GetIntersection(date) != null)
    //            //    Console.WriteLine(timeRange5.GetIntersection(timeRange6).ToString());
    //            //if (timeRange5.GetIntersection(timeRange6).ToString() != "")
    //            {
    //                //" Наблюдается временное перекрытие -  timetablerange.GetIntersection(date): " +
    //                //              timetablerange.GetIntersection(date);
    //                break;
    //                return false;
    //            }
    //            //else
    //            //{
    //            //    Console.WriteLine("Временного перекрытия нет!");
    //            //}




    //            //if (onetimetable.Startlesson)
    //            //{

    //            //}

    //            //bool overlap = onetimetable.Startlesson < date.Endlesson && date.Startlesson < onetimetable.Endlesson;

    //            //TimeRange




    //            // Timetable time = context.Timetables.Where(x => x.Startlesson == timetable.Startlesson & x.CourseID == co.ID).FirstOrDefault<Timetable>(); //
    //            //if (ts != null)
    //            //{ return "Ученик №" + s.ID + " уже занят на курсе №" + co.ID + " элементом расписания №" + ts.ID + " в промежутке от " + ts.Startlesson + " до " + ts.Endlesson; }
    //        }
    //        return true;
    //    }

    }
    public static class Timetables
    {
        public static Timetable TimetableID(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                Timetable v = context.Timetables.Where(x => x.ID == id).FirstOrDefault<Timetable>();

                return v;
            }
        }

        //////////////////// ОДИН БОЛЬШОЙ ПОИСК !!! Если не введены никакие параметры, функция должна возвращать все элементы расписания //////////////////
        public static List<Timetable> FindAll(Boolean deldate, Branch branch, Cabinet cabinet, Worker teacher, Course course, Student student, DateTime date, String sort, String asсdesс, int page, int count, ref int countrecord) //deldate =false - все и удал и неудал!
        {
            List<Timetable> list = new List<Timetable>();
            using (SampleContext db = new SampleContext())
            {
                var query = from c in db.Cabinets
                            join t in db.Timetables on c.ID equals t.CabinetID
                            join tt in db.TimetablesTeachers on t.ID equals tt.TimetableID
                            join cour in db.Courses on t.CourseID equals cour.ID
                            join sc in db.StudentsCourses on t.CourseID equals sc.CourseID

                            select new { ID = t.ID, StudentID = sc.StudentID, CabinetID = t.CabinetID, CourseID = t.CourseID, Note = t.Note, Startlesson = t.Startlesson, Endlesson = t.Endlesson, Deldate = t.Deldate, Editdate = t.Editdate, BranchID = c.BranchID, TeacherID = tt.TeacherID };

                // Последовательно просеиваем наш список 

                if (deldate != false) // Убираем удаленных, если нужно
                {
                    query = query.Where(x => x.Deldate == null);
                }

                if (branch.ID != 0)
                {
                    query = query.Where(x => x.BranchID == branch.ID);
                }

                if (course.ID != 0)
                {
                    query = query.Where(x => x.CourseID == course.ID);
                }

                if (student.ID != 0)
                {
                    query = query.Where(x => x.StudentID == student.ID);
                }

                if (cabinet.ID != 0)
                {
                    query = query.Where(x => x.CabinetID == cabinet.ID);
                }

                if (teacher.ID != 0)
                {
                    query = query.Where(x => x.TeacherID == teacher.ID);
                }

    //            string format = "yyyy-MM-dd HH:mm:ss";
                // Подсчет недели, которую необходимо отобразить!
                DateTime firstdate = date.AddDays(-((date.DayOfWeek - System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 7) % 7)).Date;
                DateTime lastdate = firstdate.AddDays(+6);

                if (firstdate != DateTime.MinValue)
                {
                    query = query.Where(x => x.Startlesson >= firstdate);
                }

                if (lastdate != DateTime.MaxValue)
                {
                    query = query.Where(x => x.Endlesson <= lastdate);
                }

               //, StudentID = sc.StudentID, CabinetID = t.CabinetID, CourseID = t.CourseID, Note = t.Note, 
               // Startlesson = t.Startlesson, Endlesson = t.Endlesson, Deldate = t.Deldate, Editdate = t.Editdate, BranchID = c.BranchID, TeacherID = tt.TeacherID
                var query2 = query.GroupBy(t => new { t.ID, t.Startlesson, t.Endlesson, /*t.StudentID, */t.CabinetID, t.CourseID, t.Note, t.Deldate, t.Editdate }, (key, group) => new
                {
                    ID = key.ID,
                    CourseID = key.CourseID,
          //          StudentID = key.StudentID,
                    CabinetID = key.CabinetID,
                    Deldate = key.Deldate,
                    Editdate = key.Editdate, 
                    Note = key.Note, 
                    Startlesson = key.Startlesson,
                    Endlesson = key.Endlesson
                });




                //               query = query.Distinct();
                if (sort != null)  // Сортировка, если нужно
                {
                    query2 = Utilit.OrderByDynamic(query2, sort, asсdesс);
                }


                countrecord = query2.GroupBy(u => u.ID).Count();

                query2 = query2.Skip((page - 1) * count).Take(count);
                query2 = query2.Distinct();

                foreach (var p in query2)
                {
                    list.Add(new Timetable { ID = p.ID, CourseID = p.CourseID, CabinetID = p.CabinetID, Startlesson = p.Startlesson, Endlesson = p.Endlesson, Note = p.Note, Deldate = p.Deldate, Editdate = p.Editdate });
                }
                return list;
            }
        }
    }
}
