using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Data;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;


//+Contract() DONE
//+Contract(ID: Int) DONE
//+ add(): String DONE
//+ del(): String DONE
//+ edit(): String DONE
//+ Cancellation(): String DONE
//+ addPay(Payment: Double): String


namespace Test
{
    public class Contract
    {
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public double Cost { get; set; }
        public double PayofMonth { get; set; }
        public Nullable<System.DateTime> Deldate { get; set; }
        public Nullable<System.DateTime> Editdate { get; set; }
        public Nullable<System.DateTime> Canceldate { get; set; }



        public int StudentID { get; set; }
        public Student Student { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }

        public int ManagerID { get; set; }
        public Worker Manager { get; set; }

        public int BranchID { get; set; }
        public Branch Branch { get; set; }


        public Contract()
        {

        }
       
        public string Add()
        {
            string answer = Сheck(this);
            if (answer == "Данные корректны!")
            {
                using (SampleContext context = new SampleContext())
                {
                    StudentsCourses stpar = new StudentsCourses();
                    stpar.StudentID = this.StudentID;
                    stpar.CourseID = this.CourseID;
                    context.Contracts.Add(this);
                    context.StudentsCourses.Add(stpar);
                    context.SaveChanges();

                    answer = "Добавление договора прошло успешно";
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
                o = "Удаление договора прошло успешно";
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
                    answer = "Редактирование договора прошло успешно";
                }
                return answer;
            }
            return answer;
        }

        public string Cancellation()
        {
            string o;
            using (SampleContext context = new SampleContext())
            {
                this.Canceldate = DateTime.Now;
                context.Entry(this).State = EntityState.Modified;
                context.SaveChanges();
                o = " Расторжение договора прошло успешно";
            }
            return o;
        }
        public string Сheck(Contract st)
        {
            //if (st.FIO == "")
            //{ return "Введите ФИО ученика. Это поле не может быть пустым"; }
            //if (st.Phone == "")
            //{ return "Введите номер телефона ученика. Это поле не может быть пустым"; }
            //using (SampleContext context = new SampleContext())
            //{
            //    Worker v = new Worker();
            //    v = context.Workers.Where(x => x.FIO == st.FIO && x.Phone == st.Phone).FirstOrDefault<Worker>();
            //    if (v != null)
            //    { return "Такой ученик уже существует в базе под номером " + v.ID; }
            //}
            return "Данные корректны!";
        }

    }

    public static class Contracts
    {
        public static Contract ContractID(int id)
        {
            using (SampleContext context = new SampleContext())
            {
                Contract v = context.Contracts.Where(x => x.ID == id).FirstOrDefault<Contract>();
                return v;
            }
        }

        public static List<Contract> GetCo()
        {
            //      var context = new SampleContext();
            using (SampleContext db = new SampleContext())
            {
                var contracts = db.Contracts.ToList();
                return contracts;
            }
        }
    }
}
