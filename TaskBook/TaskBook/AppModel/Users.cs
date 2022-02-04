using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;


namespace TaskBook.AppModel
{
    //Класс - Пользователи - база данных пользователей
    public class Users : IDisposable
    {
        //Проверка пользователя на наличие в базе данных
        public bool Check(string name, string password)
        {
            User user = FByName(name);
            if (user != null && user.HashPassword == password.GetHashCode()) return true;
            else return false;
        }

        //Добавляет нового пользователя
        public void Add(string name, string password)
        {
            if (FByName(name) != null) throw new UserException("Пользователь уже есть в приложении.");
            users.Add(new User { Name = name, HashPassword = password.GetHashCode() });
        }

        //Удаляет пользователя
        public void Remove(string name)
        {
            User user = FByName(name);
            if (user == null) throw new UserException("Пользователя нет в приложении.");
            users.Remove(user);
        }

        //Сохраняет базу данных
        public void Dispose()
        {
            Save();
        }

        //Конструктор
        public Users()
        {
            try
            {
                Load();
            }
            catch (Exception)
            {
                users = new List<User>();
            }
        }

        List<User> users; //спиисок пользователей

        //Возвращает пользователя по его имени
        User FByName(string name)
        {
            return users.FirstOrDefault(u => u.Name == name);
        }

        //Загружает базу данных
        void Load()
        {
            using (StreamReader sr = new StreamReader("Users.txt"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));
                users = (List<User>)xmlSerializer.Deserialize(sr);
            }
        }

        //Сохраняет базу данных
        void Save()
        {
            using (StreamWriter sw = new StreamWriter("Users.txt"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));
                xmlSerializer.Serialize(sw, users);
            }
        }
    }
}
