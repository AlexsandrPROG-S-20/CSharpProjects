using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace TaskBook.AppModel
{
    //Класс - Дела - база данных дел
    public class Tasks : IEnumerable<Task>, IDisposable
    {
        //Добавлет новое дело
        public Task AddDo(string name, string theme, string text, DateTime date)
        {
            Task t = new Task { Theme = theme, Name = name, Date = date, Status = TaskStatus.DO, Text = text };
            tasks.Add(t);
            return (Task)t.Clone();
        }

        //Изменяет статус дела по исполнителю и теме
        public TaskStatus SetStatus(string name, string theme, TaskStatus status)
        {
            Task t = FindByTheme(name, theme);
            TaskStatus ts = t.Status;
            t.Status = status;
            return ts;
        }

        //Удаляет дело по исполнителю и теме
        public void Remove(string name, string theme)
        {
            tasks.Remove(FindByTheme(name, theme));
        }

        //Возвращает дело по исполнителю и теме
        public Task FindByTheme(string name, string theme)
        {
            var r = tasks.FirstOrDefault(c => c.Name == name && c.Theme == theme);
            if (r == null) throw new TaskException("Дела " + theme + " нет в списке.");
            else
                return r;
        }

        //Возвращает перечислитель всех дел
        public IEnumerator<Task> GetEnumerator()
        {
            foreach (Task c in tasks)
                yield return (Task)c.Clone();
        }

        //Возвращает нетипизированный перечислитель для всех дел
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //Сохрает базу данных
        public void Dispose()
        {
            Save();
        }

        //Конструктор
        public Tasks()
        {
            try
            {
                Load();
            }
            catch (Exception)
            {
                tasks = new List<Task>();
            }
        }


        List<Task> tasks; //список дел

        //Загружает базу данных
        void Load()
        {
            using (StreamReader sr = new StreamReader("Tasks.txt"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Task>));
                tasks = (List<Task>)xmlSerializer.Deserialize(sr);
            }
        }

        //Сохраняет базу данных
        void Save()
        {
            using (StreamWriter sw = new StreamWriter("Tasks.txt"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Task>));
                xmlSerializer.Serialize(sw, tasks);
            }
        }
    }
}
