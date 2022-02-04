using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.AppModel
{
    //Класс - Исключение для класса Task
    public class TaskException : ArgumentException
    {
        public TaskException(string message) : base(message) { }
    }
}
