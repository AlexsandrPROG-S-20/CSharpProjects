using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.AppModel
{
    //Класс - Исключение для класса User
    public class UserException : ArgumentException
    {
        public UserException(string message) : base(message)
        {
        }
    }
}
