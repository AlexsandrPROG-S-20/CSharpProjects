using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.AppModel
{
    //Класс - Исключение для класса TasksApp
    public class TasksAppException : ArgumentException
    {
        public TasksAppException(string message) : base(message) { }
    }
}
