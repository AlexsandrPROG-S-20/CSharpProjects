using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook
{
    public interface IAuth
    {
        //Аутентификация
        void Auth(string name, string password);

        //Добавление нового пользователя
        void AddUser(string name, string password);

        //Удаление пользователя
        void RemoveUser(string name);
    }
}
