using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.AppModel
{
    //Класс - Пользователь
    public class User
    {
        public string Name { get; set; } //имя
        public int HashPassword { get; set; } //хэш пароля

        //Сравнивает пользователей
        public override bool Equals(object obj)
        {
            if (!(obj is User)) return false;
            User user = obj as User;
            return Name == user.Name && HashPassword == user.HashPassword;
        }
    }
}
