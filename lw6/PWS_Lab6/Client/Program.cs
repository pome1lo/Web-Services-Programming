using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //Проверьте следующие запросы:
            //Выбор списка студентов: http://localhost:port/StudentDataService.svc/Student
            //Выбор по первичному ключу: http://localhost:port/StudentDataService.svc/Student(7)
            //Сортировка(orderby):http://localhost:51565/Service1.svc/student?$orderby=name%20desc || http://localhost:51565/Service1.svc/student?$orderby=name%20asc
            //Выборка(select): http://localhost:51565/Service1.svc/student(6)/name
            //Фильтрация(filter): http://localhost:port/StudentDataService.svc/Students?$filter=name eq 'Иван Иванов'
            //http://localhost:51565/Service1.svc/student(7)/?$format=json
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
