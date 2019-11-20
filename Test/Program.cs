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
        { }

        //Отражение таблиц базы данных на свойства с типом DbSet
        public DbSet<Student> Students { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<StudentsParents> StudentsParents { get; set; }
        public DbSet<StudentsCourses> StudentsCourses { get; set; }
        public DbSet<TeachersCourses> TeachersCourses { get; set; }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
    class Program
    {
        private static SQLiteConnection m_dbConn;
        public static void Main(string[] args)
        {
            m_dbConn = new SQLiteConnection("Data Sourse=..\\..\\..\\TestDB.db");

            /////////////////////// ОБЪЯВЛЕНИЕ ОБЩИХ ПЕРЕМЕННЫХ - ДЛЯ УДОБСТВА ПРОВЕРКИ РАБОТЫ ПОИСКОВ ////////////////////////

            Boolean deldate = false; // true - неудален false - все!!!
            int count = 10;
            int page = 1;
            String sort = "";
            String askdesk = "ask";

        begin:;
            Console.WriteLine(" Что вы хотите сделать? Введите цифру от 1 - ученики , 2 - родители, 3 - филиалы, 4 -договоры, 5 - работники, 6 - кабинеты, 7 - тип курса, 8 - курсы");
            int ch = Convert.ToInt32(Console.ReadLine());

            if (ch == 1)                     ////////////////////////////////////////////////  УЧЕНИКИ ////////////////////////////////////////////////////////////////////////////
            {
                Console.WriteLine(" Что вы хотите сделать? Введите цифру от 1 до 6.");
                Console.WriteLine("1 - Вывод всех учеников на экран");
                Console.WriteLine("2 - Добавление нового ученика");
                Console.WriteLine("3 - Удаление ученика");
                Console.WriteLine("4 - Редактирование данных об ученике");
                Console.WriteLine("5 - Просмотр неудаленных учеников в виде отсортированного списка в порядке алфавита");
                Console.WriteLine("6 - Просмотр отсортированного списка учеников по алфавиту");
                Console.WriteLine("7 - Список отв. лиц этого ученика");
                Console.WriteLine("8 - Список договоров этого ученика");
                Console.WriteLine("9 - Добавление ученику ответственное лицо");
                Console.WriteLine("10 - Удаление у ученика ответственного лица");
                int choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 1)
                {
                    Parent parent = new Parent();
                    parent.FIO = null;
                    parent.ID = 0;
                    parent.Phone = null;
                    Student student = new Student();
                    //student.ID = 3 ;
                    //student.FIO = "Анохин Александр";
                    //student.Phone = "1111111111";
                    Contract contract = new Contract();

                    List<Student> stud = new List<Student>();
                    stud = Students.FindAll(deldate, parent, student, contract, sort, askdesk, page, count);

                    foreach (var s in stud)
                    {
                        Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);
                    }
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
                }

                if (choice == 3)
                {
                        Console.WriteLine("Удаление ученика:");
                        Console.WriteLine("Введите ID");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Student s = new Student();
                        s = Students.StudentID(id);
                        string Answ = s.Del();
                        Console.WriteLine(Answ);
                }

                if (choice == 4)
                {
                        Console.WriteLine("Редактирование ученика:");
                        Console.WriteLine("Введите ID");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Student v = new Student();
                        v = Students.StudentID(id);
                        Console.WriteLine("Введите ФИО");
                        string fio = Console.ReadLine();
                        Console.WriteLine("Введите номер телефона");
                        string nom = Console.ReadLine();
                        v.FIO = fio;
                        v.Phone = nom;
                        string Answer = v.Edit();
                        Console.WriteLine(Answer);
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
                if (choice == 7)
                {

                    /////////////////////// ВЫЗОВ ПОИСКА РОДИТЕЛЕЙ У УЧЕНИКА ////////////////////////

                    Console.WriteLine("Введите ID ученика");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Student st = Students.StudentID(id);
                    var listpar = Student.GetParents(st);
                    foreach (var s in listpar)
                    {
                        Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);
                    }
                }

                if (choice == 8)     //Запрос ищет договоры по студенту                     + GetContracts() in Student
                {   
                        Console.WriteLine("Введите ID ученика");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Student st = Students.StudentID(id);
                        var v = Student.GetContracts(st);
                        foreach (var s in v)
                        {
                            Console.WriteLine("ID: {0} \t Date: {1}  \t StudentID: {2} \t  Deldate: {3} \t Editdate: {4}", s.ID, s.Date, s.StudentID, s.Deldate, s.Editdate);
                        }
                }

                if (choice == 9)    //Добавление отв. лица
                {
                    Console.WriteLine("Добавление отв. лица");
                    Console.WriteLine("Введите ID ученика");
                    int ids = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите ID отв. лица");
                    int idp = Convert.ToInt32(Console.ReadLine());

                    Student s = new Student();
                    s = Students.StudentID(ids);

                    Parent p = new Parent();
                    p = Parents.ParentID(idp);

                    string Answ = Student.addParent(p, s);
                    Console.WriteLine(Answ);
                }

                if (choice == 10)    //Удаление отв. лица
                {
                    Console.WriteLine("Удаление отв. лица");
                    Console.WriteLine("Введите ID ученика");
                    int ids = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите ID отв. лица");
                    int idp = Convert.ToInt32(Console.ReadLine());

                    Student s = new Student();
                    s = Students.StudentID(ids);

                    Parent p = new Parent();
                    p = Parents.ParentID(idp);

                    string Answ = Student.delParent(p, s);
                    Console.WriteLine(Answ);
                }
            }


            if (ch == 2)                     ////////////////////////////////////////////////  РОДИТЕЛИ ////////////////////////////////////////////////////////////////////////////
            {
                Console.WriteLine(" Что вы хотите сделать? Введите цифру от 1 до 6.");
                Console.WriteLine("1 - Вывод всех отв. лица на экран");
                Console.WriteLine("2 - Добавление нового отв. лица");
                Console.WriteLine("3 - Удаление отв. лица");
                Console.WriteLine("4 - Редактирование данных об отв. лице");
                Console.WriteLine("5 - Список учеников этого отв. лица");
                Console.WriteLine("6 - Добавление  ответственного лица к ученику");
                Console.WriteLine("7 - Удаление у ответственного лица ученика");

                int choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 1)

                {
                    /////////////////////// ВЫЗОВ ПОИСКА РОДИТЕЛЕЙ ////////////////////////

                    Parent parent = new Parent();
                    parent.FIO = null;
                    parent.ID = 0;
                    parent.Phone = null;
                    Student student = new Student();
                    //student.ID = 3 ;
                    //student.FIO = "Анохин Александр";
                    //student.Phone = "1111111111";

                    List<Parent> parents = new List<Parent>();
                    parents = Parents.FindAll(deldate, parent, student, sort, askdesk, page, count);

                    foreach (var s in parents)
                    {
                        Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);
                    }

                }

                if (choice == 2)
                {
                    Console.WriteLine("Добавление отв. лица:");
                    Console.WriteLine("Введите ФИО");
                    string fio = Console.ReadLine();
                    Console.WriteLine("Введите номер телефона");
                    string nom = Console.ReadLine();

                    Parent st = new Parent();
                    st.FIO = fio;
                    st.Phone = nom;
                    string Answer = st.Add();
                    Console.WriteLine(Answer);
                }

                if (choice == 3)
                {
                    using (SampleContext context = new SampleContext())
                    {
                        Console.WriteLine("Удаление отв. лица:");
                        Console.WriteLine("Введите ID");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Parent s = new Parent();
                        s = Parents.ParentID(id);
                        string Answ = s.Del();
                        Console.WriteLine(Answ);
                    }
                }

                if (choice == 4)
                {
                    using (SampleContext context = new SampleContext())
                    {
                        Console.WriteLine("Редактирование отв. лица:");
                        Console.WriteLine("Введите ID");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Parent v = new Parent();
                        v = Parents.ParentID(id);
                        Console.WriteLine("Введите ФИО");
                        string fio = Console.ReadLine();
                        Console.WriteLine("Введите номер телефона");
                        string nom = Console.ReadLine();
                        v.FIO = fio;
                        v.Phone = nom;
                        string Answer = v.Edit();
                        Console.WriteLine(Answer);
                    }
                }

                if (choice == 5)
                {

                    /////////////////////// ВЫЗОВ ПОИСКА УЧЕНИКОВ У РОДИТЕЛЯ ////////////////////////

                    Console.WriteLine("Введите ID отв. лица");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Parent st = Parents.ParentID(id);
                    var v = Parent.GetStudents(st);
                    foreach (var s in v)
                    {
                        Console.WriteLine("ID: {0} \t FIO: {1}  \t Phone: {2} \t  Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);
                    }
                }

                if (choice == 6)    //Добавление ученика
                {
                    Console.WriteLine("Добавление ученика");

                    Console.WriteLine("Введите ID отв. лица");
                    int idp = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите ID ученика");
                    int ids = Convert.ToInt32(Console.ReadLine());

                    Student s = new Student();
                    s = Students.StudentID(ids);

                    Parent p = new Parent();
                    p = Parents.ParentID(idp);

                    string Answ = Student.addParent(p, s);
                    Console.WriteLine(Answ);
                }

                if (choice == 7)    //Удаление ученика
                {
                    Console.WriteLine("Удаление ученика");

                    Console.WriteLine("Введите ID отв. лица");
                    int idp = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите ID ученика");
                    int ids = Convert.ToInt32(Console.ReadLine());

                    Student s = new Student();
                    s = Students.StudentID(ids);

                    Parent p = new Parent();
                    p = Parents.ParentID(idp);

                    string Answ = Student.delParent(p, s);
                    Console.WriteLine(Answ);
                }
            }



            if (ch == 3)                     ////////////////////////////////////////////////  ФИЛИАЛЫ ////////////////////////////////////////////////////////////////////////////
            {
                Console.WriteLine(" Что вы хотите сделать? Введите цифру от 1 до 6.");
                Console.WriteLine("1 - Вывод всех филиалов на экран");
                Console.WriteLine("2 - Добавление нового филиалa");
                Console.WriteLine("3 - Удаление филиалa");
                Console.WriteLine("4 - Редактирование данных о филиалe");
                Console.WriteLine("5 - Список кабинетов в филиале");
                Console.WriteLine("6 - Список договоров в филиале");
                Console.WriteLine("7 - Список работников в филиале");
                Console.WriteLine("8 - Список курсов в филиале");
                Console.WriteLine("9 - Получить выручку филиала");
                Console.WriteLine("10 - Получить прибыль филиала");

                int choice3 = Convert.ToInt32(Console.ReadLine());

                if (choice3 == 1)    //////////////// здесь нужен лефт джоин скорее всего!

                {
                    /////////////////////// ВЫЗОВ ПОИСКА ФИЛИАЛОВ ////////////////////////

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

                }

                if (choice3 == 2)
                {
                    Console.WriteLine("Добавление филиала:");
                    Console.WriteLine("Введите название");
                    string name = Console.ReadLine();
                    Console.WriteLine("Введите адрес");
                    string address = Console.ReadLine();
                    Console.WriteLine("Введите ID директора");
                    int dir = Convert.ToInt32(Console.ReadLine());

                    Branch st = new Branch();
                    st.Name = name;
                    st.Address = address;
                    st.DirectorBranch = dir;
                    string Answer = st.Add();
                    Console.WriteLine(Answer);
                }

                if (choice3 == 3)
                {
                    using (SampleContext context = new SampleContext())
                    {
                        Console.WriteLine("Удаление филиала");
                        Console.WriteLine("Введите ID");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Branch s = new Branch();
                        s = Branches.BranchID(id);
                        string Answ = s.Del();
                        Console.WriteLine(Answ);
                    }
                }

                if (choice3 == 4)
                {
                    using (SampleContext context = new SampleContext())
                    {
                        Console.WriteLine("Редактирование филиала");
                        Console.WriteLine("Введите ID");
                        int id = Convert.ToInt32(Console.ReadLine());

                        Branch v = new Branch();
                        v = Branches.BranchID(id);
                        Console.WriteLine("Добавление филиала:");
                        Console.WriteLine("Введите название");
                        string name = Console.ReadLine();
                        Console.WriteLine("Введите адрес");
                        string address = Console.ReadLine();
                        Console.WriteLine("Введите ID директора");
                        int dir = Convert.ToInt32(Console.ReadLine());
                        string Answer = v.Edit();
                        Console.WriteLine(Answer);
                    }
                }

                if (choice3 == 5)
                {
                    Console.WriteLine("Введите ID филиала");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Branch st = Branches.BranchID(id);
                    var cabinets = Branch.GetCabinets(st);
                    foreach (var s in cabinets)
                    {
                        Console.WriteLine("ID: {0} \t Number: {1}  \t Capacity: {2} \t  Deldate: {3} \t Editdate: {4} \t BranchID: {5}", s.ID, s.Number, s.Capacity, s.Deldate, s.Editdate, s.BranchID);
                    }
                }

                if (choice3 == 6)
                {
                    Console.WriteLine("Введите ID филиала");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Branch st = Branches.BranchID(id);
                    var contracts = Branch.GetContracts(st);
                    foreach (var s in contracts)
                    {
                        Console.WriteLine("ID: {0} \t Date: {1}  \t StudentID: {2} \t  Deldate: {3} \t Editdate: {4}", s.ID, s.Date, s.StudentID, s.Deldate, s.Editdate);
                    }

                }

                if (choice3 == 7)
                {
                    Console.WriteLine("Введите ID филиала");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Branch st = Branches.BranchID(id);
                    var workers = Branch.GetWorkers(st);
                    foreach (var s in workers)
                    {
                        Console.WriteLine("ID: {0} \t FIO: {1} \t Type: {2} \t Position:{3} \t BranchID: {4} \t Editdate: {5}  \t Deldate:  {6}", s.ID, s.FIO, s.Type, s.Position, s.BranchID, s.Editdate, s.Deldate);
                    }

                }
            }



            if (ch == 4)                     ////////////////////////////////////////////////  ДОГОВОРЫ ////////////////////////////////////////////////////////////////////////////
            {
                Console.WriteLine(" Что вы хотите сделать? Введите цифру от 1 до 6.");
                Console.WriteLine("1 - Вывод всех договоров на экран");
                Console.WriteLine("2 - Добавление нового договора");
                Console.WriteLine("3 - Удаление договора");
                Console.WriteLine("4 - Редактирование данных о договоре");
                Console.WriteLine("5 - Расторжение договора");

                //Console.WriteLine("6 - Просмотр неудаленных договоров в виде отсортированного списка в порядке алфавита");
                //Console.WriteLine("7 - Просмотр отсортированного списка договоров по алфавиту");
                int choice4 = Convert.ToInt32(Console.ReadLine());

                if (choice4 == 1)
                {
                    List<Contract> contracts = Contracts.GetCo();

                    foreach (var s in contracts)
                    {
                        Console.WriteLine("ID: {0} \t Date: {1}  \t StudentID: {2} \t  Deldate: {3} \t Editdate: {4} \t ManagerID: {5} \t CourseID: {6}", s.ID, s.Date, s.StudentID, s.Deldate, s.Editdate, s.ManagerID, s.CourseID);
                    }
                }

                if (choice4 == 2)
                {
                    Console.WriteLine("Добавление договора:");
                    Console.WriteLine("Введите ID Ученика");
                    int id = Convert.ToInt32(Console.ReadLine());

                    Student stud = new Student();
                    stud = Students.StudentID(id); //Когда будут совпадать ФИО - вылезет ошибка!
                                                   //В формах будет проще - там будет выбираться сразу вся строчка ученика, в которой будет ID
                                                   // Если ввести ФИО, которого нет в бд - будет ошибка! Я не делаю проверку, потому что у меня в проекте
                                                   // Ученик будет выбираться из предложенных

                    Console.WriteLine("Введите ID курса");
                    int idс = Convert.ToInt32(Console.ReadLine());
                    Course cour = new Course();
                    cour = Courses.CourseID(idс); 

                    Console.WriteLine("Введите ID менеджера");
                    int idm = Convert.ToInt32(Console.ReadLine());
                    Worker man = new Worker();
                    man = Workers.WorkerID(idm); //Когда будут совпадать ФИО - вылезет ошибка!


                    Console.WriteLine("Введите ID филиала");
                    int idb = Convert.ToInt32(Console.ReadLine());
                    Branch bra = new Branch();
                    bra = Branches.BranchID(idb); //Когда будут совпадать ФИО - вылезет ошибка!

                    Console.WriteLine("Введите стоимость обучения");
                    double cost = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Введите размер ежемесячной платы");
                    double pay = Convert.ToDouble(Console.ReadLine());

                    Contract con = new Contract();
                    con.Date = DateTime.Now;
                    //           con.Student = stud;
                    con.StudentID = stud.ID;
                    con.CourseID = cour.ID;
                    con.ManagerID = man.ID;
                    con.BranchID = bra.ID;
                    con.Cost = cost;
                    con.PayofMonth = pay;
                    string answer1 = con.Add();
                    Console.WriteLine(answer1);
                }

                if (choice4 == 3)
                {
                    Console.WriteLine("Удаление договора:");
                    Console.WriteLine("Введите ID");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Contract v = Contracts.ContractID(id);
                    string a = v.Del();
                    Console.WriteLine(a);
                }

                if (choice4 == 4)
                {
                    Console.WriteLine("Редактирование договора:");
                    Console.WriteLine("Введите ID");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Contract v = Contracts.ContractID(id);

                    Console.WriteLine("Введите ID ученика");
                    int ids = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите ID курса");
                    int idcour = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите стоимость обучения");
                    double cos = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Введите ежемесячную плату");
                    double pay1 = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Введите ID филиала");
                    int idbr = Convert.ToInt32(Console.ReadLine());

                    v.StudentID = ids;
                    v.BranchID = idbr;
                    v.CourseID = idcour;
                    v.Cost = cos;
                    v.PayofMonth = pay1;

                    string a = v.Edit();
                    Console.WriteLine(a);
                }

                if (choice4 == 5)
                {
                    Console.WriteLine("Расторжение договора:");
                    Console.WriteLine("Введите ID");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Contract v = Contracts.ContractID(id);
                    string a = v.Cancellation();
                    Console.WriteLine(a);
                }



                //if (choice4 == 6)
                //{
                //    var v = context.Students.Where(x => x.Deldate == null).ToList<Student>().OrderBy(u => u.FIO);
                //    foreach (var s in v)
                //    {
                //        Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);

                //    }

                //}

                //if (choice4 == 7)
                //{
                //    var v = context.Students.OrderBy(u => u.FIO);
                //    foreach (var s in v)
                //    {
                //        Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);

                //    }

                //}

                //if (choice4 == 8)  //Запрос ищет договоры по студенту и менеджеру
                //{
                //    Console.WriteLine("Введите ID ученика");
                //    int id = Convert.ToInt32(Console.ReadLine());
                //    Console.WriteLine("Введите ID менеджера");
                //    int idm = Convert.ToInt32(Console.ReadLine());
                //    var v = context.Database.SqlQuery<Contract>("select * from Contracts Where Contracts.StudentID =" + "'" + id + "'" + "and Contracts.ManagerID =" + "'" + idm + "'").ToList();
                //    foreach (var s in v)
                //    {
                //        Console.WriteLine("ID: {0} \t Date: {1}  \t StudentID: {2} \t  Deldate: {3} \t Editdate: {4}  \t ManagerID: {5}", s.ID, s.Date, s.StudentID, s.Deldate, s.Editdate, s.ManagerID);
                //    }
                //}
            }

            if (ch == 5)                     ////////////////////////////////////////////////  РАБОТНИКИ ////////////////////////////////////////////////////////////////////////////
            {
                Console.WriteLine(" Что вы хотите сделать? Введите цифру от 1 до 6.");
                Console.WriteLine("1 - Вывод всех работников на экран");
                Console.WriteLine("2 - Добавление нового работника");
                Console.WriteLine("3 - Удаление работника");
                Console.WriteLine("4 - Редактирование данных о работнике");
                Console.WriteLine("5 - Просмотр договоров, заключенных этим менеджером");
                Console.WriteLine("6 - Просмотр отсортированного списка учеников по алфавиту");
                int choice5 = Convert.ToInt32(Console.ReadLine());

                if (choice5 == 1)
                {

                    /////////////////////// ВЫЗОВ ПОИСКА РАБОТНИКОВ ////////////////////////
                    Branch branch1 = new Branch();
                    branch1.ID = 0;
                    Worker wor = new Worker();
                    wor.Type = 0;                             // по идее работает правильно, но лучше перепроверять! 
                    List<Worker> workers = new List<Worker>();
                    workers = Workers.FindAll(deldate, wor, branch1, sort, askdesk, page, count);

                    foreach (var s in workers)
                    {
                        Console.WriteLine("ID: {0} \t FIO: {1} \t Type: {2} \t Position:{3} \t BranchID: {4} \t Editdate: {5}  \t Deldate:  {6}", s.ID, s.FIO, s.Type, s.Position, s.BranchID, s.Editdate, s.Deldate);
                    }
                }

                if (choice5 == 2)
                {
                    Console.WriteLine("Добавление работника:");
                    Worker st = new Worker();

                    Console.WriteLine("Введите ФИО");
                    string fio = Console.ReadLine();

                    Console.WriteLine("Введите номер телефона");
                    string nom = Console.ReadLine();

                    Console.WriteLine("Введите должность");
                    string pos = Console.ReadLine();

                    Console.WriteLine("Введите тип работника (1, 2 или 3)");
                    int type = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите ставку");
                    string p1 = Console.ReadLine();
                    if(p1 != "")         
                    {
                        double rate = Convert.ToDouble(p1);
                        st.Rate = rate;
                    }

                    Console.WriteLine("Введите ID филиала");
                    string p2 = Console.ReadLine();
                    if (p2 != "")
                    {
                        int br = Convert.ToInt32(p2);
                        st.BranchID = br;
                    }

                    Console.WriteLine("Введите пароль");
                    string p3 = Console.ReadLine();
                    if (p3 != "")
                    {
                        string pass = p3;
                        st.Password = pass;
                    }

                    st.FIO = fio;
                    st.Phone = nom;
                    st.Position = pos;
                    st.Type = type;
                    //st.Rate = rate;

                    
                    string Answer = st.Add();
                    Console.WriteLine(Answer);
                }

                if (choice5 == 3)
                {
                    using (SampleContext context = new SampleContext())
                    {
                        Console.WriteLine("Удаление работника:");
                        Console.WriteLine("Введите ID");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Worker s = new Worker();
                        s = Workers.WorkerID(id);
                        string Answ = s.Del();
                        Console.WriteLine(Answ);
                    }
                }

                if (choice5 == 4)
                {
                    using (SampleContext context = new SampleContext())
                    {
                        Console.WriteLine("Редактирование работника:");
                        Console.WriteLine("Введите ID");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Worker st = new Worker();
                        st = Workers.WorkerID(id);

                        Console.WriteLine("Введите ФИО");
                        string fio = Console.ReadLine();

                        Console.WriteLine("Введите номер телефона");
                        string nom = Console.ReadLine();

                        Console.WriteLine("Введите должность");
                        string pos = Console.ReadLine();

                        Console.WriteLine("Введите тип работника (1, 2 или 3)");
                        int type = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine("Введите ставку");
                        string p1 = Console.ReadLine();
                        if (p1 != "")
                        {
                            double rate = Convert.ToDouble(p1);
                            st.Rate = rate;
                        }

                        Console.WriteLine("Введите ID филиала");
                        string p2 = Console.ReadLine();
                        if (p2 != "")
                        {
                            int br = Convert.ToInt32(p2);
                            st.BranchID = br;
                        }

                        Console.WriteLine("Введите пароль");
                        string p3 = Console.ReadLine();
                        if (p3 != "")
                        {
                            string pass = p3;
                            st.Password = pass;
                        }

                        st.FIO = fio;
                        st.Phone = nom;
                        st.Position = pos;
                        st.Type = type;
                        string Answer = st.Edit();
                        Console.WriteLine(Answer);
                    }
                }

                if (choice5 == 5)            //       + GetContracts()
                {
                    Console.WriteLine("Введите ID менеджера");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Worker st = Workers.WorkerID(id);
                    var v = Worker.GetContracts(st);
                    foreach (var s in v)
                    {
                        Console.WriteLine("ID: {0} \t Date: {1}  \t StudentID: {2} \t  Deldate: {3} \t Editdate: {4}", s.ID, s.Date, s.StudentID, s.Deldate, s.Editdate);
                    }
                }
            }

            if (ch == 6)                     ////////////////////////////////////////////////  КАБИНЕТЫ ////////////////////////////////////////////////////////////////////////////
            {
                Console.WriteLine(" Что вы хотите сделать? Введите цифру от 1 до 6.");
                Console.WriteLine("1 - Вывод всех кабинетов на экран");
                Console.WriteLine("2 - Добавление нового кабинета");
                Console.WriteLine("3 - Удаление кабинета");
                Console.WriteLine("4 - Редактирование данных о кабинете");
                int choice6 = Convert.ToInt32(Console.ReadLine());

                if (choice6 == 1)
                {
                    /////////////////////// ВЫЗОВ ПОИСКА КАБИНЕТОВ   ////////////////////////
                    Branch branch1 = new Branch();
         //           branch1.ID = 0;
                    Cabinet cab = new Cabinet();
          //          cab.BranchID = 0;
          //          cab.Number = "Кабинет математики №1";
                    int min =0;
                    int max =0;

                    List<Cabinet> cabinets = new List<Cabinet>();
                    cabinets = Cabinets.FindAll(deldate, cab, branch1, min, max, sort, askdesk, page, count);

                    foreach (var s in cabinets)
                    {
                        Console.WriteLine("ID: {0} \t Number: {1}  \t Capacity: {2} \t  Deldate: {3} \t Editdate: {4} \t BranchID: {5}", s.ID, s.Number, s.Capacity, s.Deldate, s.Editdate, s.BranchID);
                    }
                }

                if (choice6 == 2)
                {
                    Console.WriteLine("Добавление кабинета:");

                    Console.WriteLine("Введите номер кабинета");
                    string num = Console.ReadLine();

                    Console.WriteLine("Введите вместимость кабинета");
                    int cap = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите ID филиала");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Branch br = new Branch();
                    br = Branches.BranchID(id); 


                    Cabinet cab = new Cabinet();
                    cab.Number = num;
                    cab.Capacity = cap;
                    cab.BranchID = br.ID;
                    string answer = cab.Add();
                    Console.WriteLine(answer);
                }

                if (choice6 == 3)
                {
                    Console.WriteLine("Удаление кабинета:");
                    Console.WriteLine("Введите ID");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Cabinet v = Cabinets.CabinetID(id);
                    string a = v.Del();
                    Console.WriteLine(a);
                }

                if (choice6 == 4)
                {
                    Console.WriteLine("Редактирование кабинета:");
                    Console.WriteLine("Введите ID");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Cabinet v = Cabinets.CabinetID(id);

                    Console.WriteLine("Введите номер кабинета");
                    string num = Console.ReadLine();

                    Console.WriteLine("Введите вместимость кабинета");
                    int cap = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите ID филиала");
                    int idbr = Convert.ToInt32(Console.ReadLine());

                    v.Number = num;
                    v.BranchID = idbr;
                    v.Capacity = cap;

                    string a = v.Edit();
                    Console.WriteLine(a);
                }
            }

            if (ch == 7)                     ////////////////////////////////////////////////  ТИП КУРСА ////////////////////////////////////////////////////////////////////////////
            {
                Console.WriteLine(" Что вы хотите сделать? Введите цифру от 1 до 6.");
                Console.WriteLine("1 - Вывод всех типов курсов на экран");
                Console.WriteLine("2 - Добавление нового типа курса");
                Console.WriteLine("3 - Удаление типа курса");
                Console.WriteLine("4 - Редактирование данных о типе курса");

                int choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 1)
                {
                  
                
                    Type type = new Type();
                    int minLes = 0;
                    int maxLes = 0;
                    double minCost = 0;
                    double maxCost = 0;
                    int minMonth = 0;
                    int maxMonth = 0;


                    List<Type> stud = new List<Type>();
                    stud = Types.FindAll(deldate, type, minLes, maxLes, minCost, maxCost, minMonth, maxMonth, sort, askdesk, page, count);

                    foreach (var s in stud)
                    {
                        Console.WriteLine("ID: {0} \t Name: {1} \t Cost: {2} \t Lessons: {3} \t Month: {4} \t Deldate: {5} \t Editdate: {6} ", s.ID, s.Name, s.Cost, s.Lessons, s.Month, s.Deldate, s.Editdate);
                    }
                }

                if (choice == 2)
                {
                    Console.WriteLine("Добавление типа курса:");
                    Type st = new Type();

                    Console.WriteLine("Введите наименование типа курса");
                    string name = Console.ReadLine();
                    st.Name = name;

                    Console.WriteLine("Введите стоимость обучения по данному типу курса");
                    string p1 = Console.ReadLine();
                    if (p1 != "")
                    {
                        double cost = Convert.ToDouble(p1);
                        st.Cost = cost;
                    }

                    Console.WriteLine("Введите количество занятий");
                    string p2 = Console.ReadLine();
                    if (p2 != "")
                    {
                        int les = Convert.ToInt32(p2);
                        st.Lessons = les;
                    }

                    Console.WriteLine("Введите количество месяцев обучения");
                    string p3 = Console.ReadLine();
                    if (p3 != "")
                    {
                        int mon = Convert.ToInt32(p3);
                        st.Month = mon;
                    }

                    Console.WriteLine("Введите описание типа курса");
                    string p4 = Console.ReadLine();
                    if (p4 != "")
                    {
                        string note = p4;
                        st.Note = note;
                    }

                    string Answer = st.Add();
                    Console.WriteLine(Answer);
                }

                if (choice == 3)
                {
                    using (SampleContext context = new SampleContext())
                    {
                        Console.WriteLine("Удаление типа курса:");
                        Console.WriteLine("Введите ID");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Type s = new Type();
                        s = Types.TypeID(id);
                        string Answ = s.Del();
                        Console.WriteLine(Answ);
                    }
                }

                if (choice == 4)
                {
                    using (SampleContext context = new SampleContext())
                    {
                        Console.WriteLine("Редактирование типа курса:");
                        Console.WriteLine("Введите ID");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Type st = new Type();
                        st = Types.TypeID(id);
                        Console.WriteLine("Введите наименование типа курса");
                        string name = Console.ReadLine();
                        st.Name = name;

                        Console.WriteLine("Введите стоимость обучения по данному типу курса");
                        string p1 = Console.ReadLine();
                        if (p1 != "")
                        {
                            double cost = Convert.ToDouble(p1);
                            st.Cost = cost;
                        }

                        Console.WriteLine("Введите количество занятий");
                        string p2 = Console.ReadLine();
                        if (p2 != "")
                        {
                            int les = Convert.ToInt32(p2);
                            st.Lessons = les;
                        }

                        Console.WriteLine("Введите количество месяцев обучения");
                        string p3 = Console.ReadLine();
                        if (p3 != "")
                        {
                            int mon = Convert.ToInt32(p3);
                            st.Month = mon;
                        }

                        Console.WriteLine("Введите описание типа курса");
                        string p4 = Console.ReadLine();
                        if (p4 != "")
                        {
                            string note = p4;
                            st.Note = note;
                        }
                        string Answer = st.Edit();
                        Console.WriteLine(Answer);
                    }
                }
            }

            if (ch == 8)                     ////////////////////////////////////////////////  КУРСЫ ////////////////////////////////////////////////////////////////////////////
            {
                Console.WriteLine(" Что вы хотите сделать? Введите цифру от 1 до 6.");
                Console.WriteLine("1 - Вывод всех курсов на экран");
                Console.WriteLine("2 - Добавление нового курса");
                Console.WriteLine("3 - Удаление курса");
                Console.WriteLine("4 - Редактирование данных о курсе");
                Console.WriteLine("5 - Список учеников этого курса");
                Console.WriteLine("6 - Список преподавателей этого курса");

                int choice4 = Convert.ToInt32(Console.ReadLine());

                if (choice4 == 1)
                {
                    List<Contract> contracts = Contracts.GetCo();

                    foreach (var s in contracts)
                    {
                        Console.WriteLine("ID: {0} \t Date: {1}  \t StudentID: {2} \t  Deldate: {3} \t Editdate: {4} \t ManagerID: {5} \t CourseID: {6}", s.ID, s.Date, s.StudentID, s.Deldate, s.Editdate, s.ManagerID, s.CourseID);
                    }
                }

                if (choice4 == 2)
                {
                    Console.WriteLine("Добавление договора:");
                    Console.WriteLine("Введите ID Ученика");
                    int id = Convert.ToInt32(Console.ReadLine());

                    Student stud = new Student();
                    stud = Students.StudentID(id); //Когда будут совпадать ФИО - вылезет ошибка!
                                                   //В формах будет проще - там будет выбираться сразу вся строчка ученика, в которой будет ID
                                                   // Если ввести ФИО, которого нет в бд - будет ошибка! Я не делаю проверку, потому что у меня в проекте
                                                   // Ученик будет выбираться из предложенных

                    Console.WriteLine("Введите ID курса");
                    int idс = Convert.ToInt32(Console.ReadLine());
                    Course cour = new Course();
                    cour = Courses.CourseID(idс);

                    Console.WriteLine("Введите ID менеджера");
                    int idm = Convert.ToInt32(Console.ReadLine());
                    Worker man = new Worker();
                    man = Workers.WorkerID(idm); //Когда будут совпадать ФИО - вылезет ошибка!


                    Console.WriteLine("Введите ID филиала");
                    int idb = Convert.ToInt32(Console.ReadLine());
                    Branch bra = new Branch();
                    bra = Branches.BranchID(idb); //Когда будут совпадать ФИО - вылезет ошибка!

                    Console.WriteLine("Введите стоимость обучения");
                    double cost = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Введите размер ежемесячной платы");
                    double pay = Convert.ToDouble(Console.ReadLine());

                    Contract con = new Contract();
                    con.Date = DateTime.Now;
                    //           con.Student = stud;
                    con.StudentID = stud.ID;
                    con.CourseID = cour.ID;
                    con.ManagerID = man.ID;
                    con.BranchID = bra.ID;
                    con.Cost = cost;
                    con.PayofMonth = pay;
                    string answer1 = con.Add();
                    Console.WriteLine(answer1);
                }

                if (choice4 == 3)
                {
                    Console.WriteLine("Удаление договора:");
                    Console.WriteLine("Введите ID");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Contract v = Contracts.ContractID(id);
                    string a = v.Del();
                    Console.WriteLine(a);
                }

                if (choice4 == 4)
                {
                    Console.WriteLine("Редактирование договора:");
                    Console.WriteLine("Введите ID");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Contract v = Contracts.ContractID(id);

                    Console.WriteLine("Введите ID ученика");
                    int ids = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите ID курса");
                    int idcour = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите стоимость обучения");
                    double cos = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Введите ежемесячную плату");
                    double pay1 = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Введите ID филиала");
                    int idbr = Convert.ToInt32(Console.ReadLine());

                    v.StudentID = ids;
                    v.BranchID = idbr;
                    v.CourseID = idcour;
                    v.Cost = cos;
                    v.PayofMonth = pay1;

                    string a = v.Edit();
                    Console.WriteLine(a);
                }

                if (choice4 == 5)
                {
                    Console.WriteLine("Введите ID курса");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Course st = Courses.CourseID(id);
                    var v = Course.GetStudents(st);
                    foreach (var s in v)
                    {
                        Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);
                    }
                }

                if (choice4 == 6)
                {
                    Console.WriteLine("Введите ID курса");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Course st = Courses.CourseID(id);
                    var v = Course.GetTeachers(st);
                    foreach (var s in v)
                    {
                        Console.WriteLine("ID: {0} \t FIO: {1} \t Phone: {2} \t Deldate: {3} \t Editdate: {4}", s.ID, s.FIO, s.Phone, s.Deldate, s.Editdate);
                    }
                }
                if (choice4 == 7)    //Добавление преподавателя
                {
                    Console.WriteLine("Добавление преподавателя");
                    Console.WriteLine("Введите ID курса");
                    int idc = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите ID преподавателя");
                    int idp = Convert.ToInt32(Console.ReadLine());

                    Course c = new Course();
                    c = Courses.CourseID(idc);

                    Worker p = new Worker();
                    p = Workers.WorkerID(idp);

                    string Answ = Course.addTeacher(c, p);
                    Console.WriteLine(Answ);
                }

                if (choice4 == 8)    //Удаление преподавателя
                {
                    Console.WriteLine("Удаление преподавателя");
                    Console.WriteLine("Введите ID курса");
                    int idc = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите ID преподавателя");
                    int idp = Convert.ToInt32(Console.ReadLine());

                    Course c = new Course();
                    c = Courses.CourseID(idc);

                    Worker p = new Worker();
                    p = Workers.WorkerID(idp);

                    string Answ = Course.delTeacher(c, p);
                    Console.WriteLine(Answ);
                }
            }


            ////////////using (SampleContext db = new SampleContext())
            ////////////{

            ////////////    // вывод 
            ////////////    foreach (Contract pl in db.Contracts.Include(p => p.Student))
            ////////////        Console.WriteLine("{0} - {1}", pl.ID, pl.Student != null ? pl.Student.FIO : "");
            ////////////    Console.WriteLine();
            ////////////    foreach (Student t in db.Students.Include(t => t.Contracts))
            ////////////    {
            ////////////        Console.WriteLine("Ученик: {0} - {1}", t.ID, t.FIO);
            ////////////        foreach (Contract pl in t.Contracts)
            ////////////        {
            ////////////            Console.WriteLine("{0} - {1}", pl.ID, pl.ManagerID);
            ////////////        }
            ////////////        Console.WriteLine();
            ////////////    }
            ////////////}



            Console.WriteLine("Повторить? Введдите q.");
            string f = Console.ReadLine();
            if (f == "q")
            {
                goto begin;
            }
            Console.ReadKey();
        }
    }
}
