using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Entity;

namespace Test
{
    public class SampleContext : DbContext
    {
        // Имя будущей базы данных можно указать через
        // вызов конструктора базового класса
        public SampleContext() : base("TestDB")
        {
        }

        //Отражение таблиц базы данных на свойства с типом DbSet
        public DbSet<Student> Students { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<StudentsParents> StudentsParents { get; set; }
        public DbSet<Branch> Branches { get; set; }
    }
    class Program
    {
        private static SQLiteConnection m_dbConn;  
        public static void Main(string[] args)
        {
            m_dbConn = new SQLiteConnection("Data Sourse=..\\..\\..\\TestDB.db");
            //using (SampleContext context = new SampleContext())
            //{
            //    context.Students.Load();
            //}
            ///////////            var context = new SampleContext();
            //  context.Students.Load();
            //       context.Parents.Load();

            Boolean deldate = true; // true - неудален false - все!!!
            Parent parent = new Parent();
            parent.FIO = null;
            parent.ID = 0;
            parent.Phone = null;
            String sort = "";
            String askdesk = "desk";
            Student student=new Student();
            //student.ID = 3 ;
            //student.FIO = "Анохин Александр";
            //student.Phone = "1111111111";
            int count = 10;
            int page = 1;


            Branch branch = new Branch();
     //       branch.Address = "ул. Ленина, 6";
            Worker director = new Worker();
  //          director.ID =1;




            List<Branch> branches = new List<Branch>();
            branches = Branches.FindAll(deldate, branch, director, sort, askdesk, page, count);


            foreach (var s in branches)
            {
                Console.WriteLine("ID: {0} \t Name: {1} \t Address: {2} \t Deldate: {3} \t Editdate: {4}  \t Director: {5}", s.ID, s.Name, s.Address, s.Deldate, s.Editdate, s.DirectorBranch);
            }



            List<Parent> parents = new List<Parent>();
            parents = Parents.FindAll(deldate, parent, student, sort, askdesk, page, count);

  //          parents = Parentsold.FindAll(deldate, parent.FIO, parent.Phone, student, sort, askdesk);

       
            foreach (var s in parents)
            {
                Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);
            }



        //List<Student> listst = new List<Student>();
        //listst = Parent.GetStudents(2); // Сюда передается айди родителя!!!!!
        //foreach (var s in listst)
        //{
        //    Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);
        //}


        //List<Parent> listpar = new List<Parent>();
        //listpar = Student.GetParents(7); // Сюда передается айди ученика!!!!
        //foreach (var s in listpar)
        //{
        //    Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);
        //}

        begin:;
            Console.WriteLine(" Что вы хотите сделать? Введите цифру от 1 - ученики, 2 -договоры");
            int ch = Convert.ToInt32(Console.ReadLine());
            if (ch == 1)
            {
                Console.WriteLine(" Что вы хотите сделать? Введите цифру от 1 до 6.");
                Console.WriteLine("1 - Вывод всех учеников на экран");
                Console.WriteLine("2 - Добавление нового ученика");
                Console.WriteLine("3 - Удаление ученика");
                Console.WriteLine("4 - Редактирование данных об ученике");
                Console.WriteLine("5 - Просмотр неудаленных учеников в виде отсортированного списка в порядке алфавита");
                Console.WriteLine("6 - Просмотр отсортированного списка учеников по алфавиту");
                int choice = Convert.ToInt32(Console.ReadLine());

                //    context.DataContext = context.Students.Local.ToBindingList();

                if (choice == 1)
                {
                    //using (SampleContext context = new SampleContext())
                    //{
                    //    var students = context.Students.ToList();
                    //    //                       return students;

                        var students = Students.GetSt();

                        foreach (var s in students)
                        {
                            Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);
                        }
                    //}
                }

                if (choice == 2)
                {
                    Console.WriteLine("Добавление ученика:");
                    Console.WriteLine("Введите ФИО");
                    string fio = Console.ReadLine();
                    Console.WriteLine("Введите номер телефона");
                    string nom = Console.ReadLine();

                    Student st = new Student();
                    st.FIO = fio;
                    st.Phone = nom;
                    string Answer = st.Add();
                    Console.WriteLine(Answer);

                    //using (SampleContext context = new SampleContext())
                    //{
                    //    context.Students.Add(st);
                    //    context.SaveChanges();
                    //    Console.WriteLine("Добавление ученика прошло успешно");
                    //}
                }

                if (choice == 3) // Исправить нужно !
                {
                    using (SampleContext context = new SampleContext())
                    {
                        Console.WriteLine("Удаление ученика:");
                        Console.WriteLine("Введите ID");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Student s = new Student();
                        s = s.StudentID(id);
                        string Answ = s.Del();
                        //Student v = context.Students.Where(x => x.ID == id).FirstOrDefault<Student>();
                        //v.Deldate = DateTime.Now;
                        //context.SaveChanges();
                        Console.WriteLine(Answ);
                    }
                }

                if (choice == 4)   // Исправить нужно !
                {
                    using (SampleContext context = new SampleContext())
                    {
                        Console.WriteLine("Редактирование ученика:");
                        Console.WriteLine("Введите ID");
                        int id = Convert.ToInt32(Console.ReadLine());
        //                Student v = context.Students.Where(x => x.ID == id).FirstOrDefault<Student>();
                        Student v = new Student();
                        v = v.StudentID(id);
                        Console.WriteLine("Введите ФИО");
                        string fio = Console.ReadLine();
                        Console.WriteLine("Введите номер телефона");
                        string nom = Console.ReadLine();
                        v.FIO = fio;
                        v.Phone = nom;
                    //    string Answ = v.Edit(v);
                        v.Editdate = DateTime.Now;
                        context.SaveChanges();
                        //                  Console.WriteLine(Answ);




                        //    //    Console.WriteLine("Введите ID");
                        //    //    int id = Convert.ToInt32(Console.ReadLine());
                        //    //    Contract v = context.Contracts.Where(x => x.ID == id).FirstOrDefault<Contract>();
                        //    //    Console.WriteLine("Введите ФИО");
                        //    //    string fio = Console.ReadLine();
                        //    //    Console.WriteLine("Введите номер телефона");
                        //    //    string nom = Console.ReadLine();
                        //    //    v.FIO = fio;
                        //    //    v.Phone = nom;

                        //    //    v.Editdate = DateTime.Now;
                        //    //    context.SaveChanges();
                        //    //    Console.WriteLine("Редактирование ученика прошло успешно");



                    }
                }

                if (choice == 5)
                {
                    using (SampleContext context = new SampleContext())
                    {
                        var v = context.Students.Where(x => x.Deldate == null).ToList<Student>().OrderBy(u => u.FIO);
                        foreach (var s in v)
                        {
                            Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);
                        }
                    }
                }

                if (choice == 6)
                {
                    using (SampleContext context = new SampleContext())
                    {
                        var v = context.Students.OrderBy(u => u.FIO);
                        foreach (var s in v)
                        {
                            Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);

                        }
                    }
                }
                //if (ch == 2)
                //{

                //    Console.WriteLine(" Что вы хотите сделать? Введите цифру от 1 до 6.");
                //    Console.WriteLine("1 - Вывод всех договоров на экран");
                //    Console.WriteLine("2 - Добавление нового договора");
                //    Console.WriteLine("3 - Удаление договора");
                //    Console.WriteLine("4 - Редактирование данных о договоре");
                //    Console.WriteLine("5 - Просмотр неудаленных договоров в виде отсортированного списка в порядке алфавита");
                //    Console.WriteLine("6 - Просмотр отсортированного списка договоров по алфавиту");
                //    Console.WriteLine("7 - Поиск договоров по студенту");
                //    int choice = Convert.ToInt32(Console.ReadLine());

                //    //conte.DataContext = context.Students.Local.ToBindingList();

                //    if (choice == 1)
                //    {
                //        List<Contract> contracts = Contracts.GetCo(context);

                //        foreach (var s in contracts)
                //        {
                //            Console.WriteLine("ID: {0} \t Date: {1}  \t StudentID: {2} \t  Deldate: {3} \t Editdate: {4}", s.ID, s.Date, s.StudentID, s.Deldate, s.Editdate);
                //        }
                //    }

                //    if (choice == 2)
                //    {
                //        Console.WriteLine("Добавление договора:");
                //        Console.WriteLine("Введите ФИО Ученика");
                //        string fio = Console.ReadLine();
                //        Student stud = context.Students.Where(x => x.FIO == fio).FirstOrDefault(); //Когда будут совпадать ФИО - вылезет ошибка!
                //                                                                                   //В формах будет проще - там будет выбираться сразу вся строчка ученика, в которой будет ID
                //                                                                                   // Если ввести ФИО, которого нет в бд - будет ошибка! Я не делаю проверку, потому что у меня в проекте
                //                                                                                   // Ученик будет выбираться из предложенных
                //        Console.WriteLine("Введите ID менеджера");
                //        int idm = Convert.ToInt32(Console.ReadLine());

                //        Contract con = new Contract();
                //        con.Date = DateTime.Now;
                //        con.StudentID = stud.ID;
                //        con.ManagerID = idm;
                //        context.Contracts.Add(con);
                //        context.SaveChanges();
                //        Console.WriteLine("Добавление договора прошло успешно");
                //    }

                //    if (choice == 3)
                //    {
                //        Console.WriteLine("Удаление договора:");
                //        Console.WriteLine("Введите ID");
                //        int id = Convert.ToInt32(Console.ReadLine());
                //        Contract v = context.Contracts.Where(x => x.ID == id).FirstOrDefault<Contract>();
                //        v.Deldate = DateTime.Now;
                //        context.SaveChanges();
                //        Console.WriteLine("Удаление договора прошло успешно");
                //    }

                //    //if (choice == 4)
                //    //{
                //    //    Console.WriteLine("Редактирование договора:");
                //    //    Console.WriteLine("Введите ID");
                //    //    int id = Convert.ToInt32(Console.ReadLine());
                //    //    Contract v = context.Contracts.Where(x => x.ID == id).FirstOrDefault<Contract>();
                //    //    Console.WriteLine("Введите ФИО");
                //    //    string fio = Console.ReadLine();
                //    //    Console.WriteLine("Введите номер телефона");
                //    //    string nom = Console.ReadLine();
                //    //    v.FIO = fio;
                //    //    v.Phone = nom;

                //    //    v.Editdate = DateTime.Now;
                //    //    context.SaveChanges();
                //    //    Console.WriteLine("Редактирование ученика прошло успешно");

                //    //}

                //    //if (choice == 5)
                //    //{
                //    //    var v = context.Students.Where(x => x.Deldate == null).ToList<Student>().OrderBy(u => u.FIO);
                //    //    foreach (var s in v)
                //    //    {
                //    //        Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);

                //    //    }

                //    //}

                //    //if (choice == 6)
                //    //{
                //    //    var v = context.Students.OrderBy(u => u.FIO);
                //    //    foreach (var s in v)
                //    //    {
                //    //        Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);

                //    //    }

                //    //}

                //    if (choice == 7)   //Запрос ищет договоры по студенту
                //    {
                //        Console.WriteLine("Введите ID ученика");
                //        int id = Convert.ToInt32(Console.ReadLine());
                //        var v = context.Contracts.Where(x => x.StudentID == id).ToList<Contract>().OrderBy(u => u.ID);
                //        foreach (var s in v)
                //        {
                //            Console.WriteLine("ID: {0} \t Date: {1}  \t StudentID: {2} \t  Deldate: {3} \t Editdate: {4}", s.ID, s.Date, s.StudentID, s.Deldate, s.Editdate);
                //        }
                //    }

                //    if (choice == 8)  //Запрос ищет договоры по студенту и менеджеру
                //    {
                //        Console.WriteLine("Введите ID ученика");
                //        int id = Convert.ToInt32(Console.ReadLine());
                //        Console.WriteLine("Введите ID менеджера");
                //        int idm = Convert.ToInt32(Console.ReadLine());
                //        var v = context.Database.SqlQuery<Contract>("select * from Contracts Where Contracts.StudentID =" + "'" + id + "'" + "and Contracts.ManagerID =" + "'" + idm + "'").ToList();
                //        foreach (var s in v)
                //        {
                //            Console.WriteLine("ID: {0} \t Date: {1}  \t StudentID: {2} \t  Deldate: {3} \t Editdate: {4}  \t ManagerID: {5}", s.ID, s.Date, s.StudentID, s.Deldate, s.Editdate, s.ManagerID);
                //        }
                //    }                
                //}

                Console.WriteLine("Повторить? Введдите q.");
                string f = Console.ReadLine();
                if (f == "q")
                {
                    goto begin;
                }
                //         Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }


        }
    }
}
