using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

//+ edit(): String
//+ add(): String
//+ del(): String
//+ addTeachers(): String DONE
//+ delTeachers(): String DONE
//+ GetFreeteachers(DateTime date) : List<Worker> 
//+ findAll(Start: Datetime, End: Datetime, editDate:	DateTime, delDate:	DateTime, Course: Course, Cabinet: Cabinet, Teacher: Worker): List<Timetable> 

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


        public static string addTeacher(Timetable c, Worker w)
        {
            TimetablesTeachers cw = new TimetablesTeachers();
            cw.TimetableID = c.ID;
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

        public static string delTeacher(Timetable c, Worker w)
        {
            TimetablesTeachers cw = new TimetablesTeachers();
            cw.TimetableID = c.ID;
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

                List<Student> liststudents = Course.GetStudents(Courses.CourseID(st.CourseID));
                foreach (Student s in liststudents)
                {
                    List<Course> listcourses = Student.GetCourses(s);
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

        public List<Worker> GetFreeteachers(List<Timetable> perioddates)
        {
            List<Worker> freeteachers = new List<Worker>();
            using (SampleContext context = new SampleContext())
            {
                List<Worker> teachers = context.Workers.Where(x => x.Type == 3).ToList<Worker>();
                foreach (Worker t in teachers)
                {
                    List<TimetablesTeachers> timetablesT = context.TimetablesTeachers.Where(x => x.TeacherID == t.ID).ToList<TimetablesTeachers>();
                    if (timetablesT.Count != 0)
                    {
                        foreach (TimetablesTeachers tt in timetablesT)
                        {
                            Timetable onetimetable = Timetables.TimetableID(tt.TimetableID); // эл. расписания, который есть у преподавателя

                            foreach (Timetable date in perioddates)
                            {
                                // Теперь этот onetimetable нужно сравнить с каждой датой! - и если совпадений нет, то добавить этого преподавателя в лист.
                                if(onetimetable.Startlesson)
                                {

                                }

                                bool overlap = onetimetable.Startlesson < date.Endlesson && date.Startlesson < onetimetable.Endlesson;
                                onetimetable.Startlesson.IsSamePeriod()




                                Timetable time = context.Timetables.Where(x => x.Startlesson == timetable.Startlesson & x.CourseID == co.ID).FirstOrDefault<Timetable>(); //
                                if (ts != null)
                                { return "Ученик №" + s.ID + " уже занят на курсе №" + co.ID + " элементом расписания №" + ts.ID + " в промежутке от " + ts.Startlesson + " до " + ts.Endlesson; }
                            }
                            
                        }

                    }
                    else
                    {
                        freeteachers.Add(t);
                    }
                }

            }

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
    }
}
