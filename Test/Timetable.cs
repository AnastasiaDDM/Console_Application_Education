using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

//+ edit(): String
//+ add(): String
//+ del(): String
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
                return answer;
            }
            return answer;
        }

        public string Add(DateTime Endrepeat, string period, Timetable timetable)
        {
  //          Timetable timetable = new Timetable();
  //          timetable = this;
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                List<Timetable> listtimetable = new List<Timetable>();
 //               listtimetable.Add(timetable);

                //Ежедневно
                //Еженедельно
                //Ежемесячно
                //Каждый год
                //Каждый будний день(пн - пт)

                DateTime newstart = timetable.Startlesson;
                DateTime newend = timetable.Startlesson;

                while (newend <= Endrepeat)
                {

                    using (SampleContext context = new SampleContext())
                    {
                        Timetable v = context.Timetables.Where(x => x.Startlesson == newstart).FirstOrDefault<Timetable>();
                        if (v == null)
                        {
                            Timetable newtimetable = new Timetable();
                            newtimetable.CabinetID = timetable.CabinetID;
                            newtimetable.CourseID = timetable.CourseID;
                            newtimetable.Note = timetable.Note;
                            newtimetable.Startlesson = newstart;
                            newtimetable.Endlesson = newend;
                            listtimetable.Add(newtimetable);
                        }
                    }

                    if (period == "Ежедневно")
                    {
  //                      newstart = timetable.Startlesson.AddDays(1);
                        newstart = newstart.AddDays(1);
                        //                      timetable.Startlesson = newtime;

                        ////newend = timetable.Endlesson.AddDays(1);
                        newend = newend.AddDays(1);
                        //                      timetable.Endlesson = newtime;
                    }

                    if (period == "Еженедельно")
                    {
                        newstart = newstart.AddDays(7);
 //                       timetable.Startlesson = newtime;

                        newend = newend.AddDays(7);
 //                       timetable.Endlesson = newtime;
                    }

                    if (period == "Ежемесячно")
                    {
                        newstart = newstart.AddMonths(1);
 //                       timetable.Startlesson = newtime;

                        newend = newend.AddMonths(1);
//                        timetable.Endlesson = newtime;
                    }

                    if (period == "Каждый год")
                    {
                        newstart = newstart.AddYears(1);
  //                      timetable.Startlesson = newtime;

                        newend = newend.AddYears(1);
    //                    timetable.Endlesson = newtime;
                    }

                    if (period == "Каждый будний день(пн - пт)")
                    {
                        if (timetable.Startlesson.DayOfWeek != DayOfWeek.Saturday || timetable.Startlesson.DayOfWeek != DayOfWeek.Sunday)
                        {
                            newstart = newstart.AddDays(1);
  //                          timetable.Startlesson = newtime;

                            newend = newend.AddDays(1);
   //                         timetable.Endlesson = newtime;
                        }
                    }
                     
                }

                using (SampleContext context = new SampleContext())
                {
                    context.Timetables.AddRange(listtimetable);
                    context.SaveChanges();
                    answer = "Добавление элемента(ов) расписания прошло успешно";
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

        public string Сheck(Timetable st)
        {
            //if (st.Name == "")
            //{ return "Введите название типа курса. Это поле не может быть пустым"; }
            //if (st.Cost == 0)
            //{ return "Введите стоимость обучения по данному типу курса. Это поле не может быть пустым"; }
            ////using (SampleContext context = new SampleContext())
            ////{
            ////    Student v = new Student();
            ////    v = context.Students.Where(x => x.FIO == st.FIO && x.Phone == st.Phone).FirstOrDefault<Student>();
            ////    if (v != null)
            ////    { return "Такой ученик уже существует в базе под номером " + v.ID; }
            ////}
            return "Данные корректны!";
        }
    }
}
