using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TaskBook.AppModel
{
    //Класс - Приложение - Дела
    public class AppTasks : IAuth, IDisposable
    {
        //Аутентифицирует пользователя
        public void Auth(string name, string password)
        {
            if (!users.Check(name, password))
            {
                cUser = null;
                throw new TaskException("Ошибка. Неверная аутентификация. Неверно указано имя пользователя или пароль.");
            }
            cUser = new User { Name = name, HashPassword = password.GetHashCode() };
        }

        //Сбрасывает аутентификацию
        public void Exit()
        {
            cUser = null;
        }

        public User User => cUser; //Возвращает пользователя

        //Добавляет нового пользователя
        public void AddUser(string name, string password)
        {
            users.Add(name, password);
        }

        //Удаляет пользователя
        public void RemoveUser(string name)
        {
            users.Remove(name);
        }

        //Возвращает все дела для зарегистрированного пользователя
        public IEnumerable<Task> FindTasksByUser()
        {
            Check();
            return from _ in tasks
                   where _.Name == cUser.Name
                   select _;
        }

        //Возвращает дело указанной темы для зарегистрированнго пользователя
        public Task FindTaskByTheme(string theme)
        {
            var t = FindTasksByUser();
            if (t != null) return t.FirstOrDefault(tk => tk.Theme == theme);
            else return null;
        }

        //Возвращает все дела для указанного статуса для зарегистрированного пользователя
        public IEnumerable<Task> FindTasksByStatus(TaskStatus status)
        {
            var t = FindTasksByUser();
            if (t != null) return t.Where(tk => tk.Status == status);
            else return null;
        }

        //Добавляет новое дело в список дел
        public Task AddDo(string theme, string text, DateTime date)
        {
            Check();
            return tasks.AddDo(cUser.Name, theme, text, date);
        }

        //Изменяет статус указанного дела
        public TaskStatus SetStatus(string theme, TaskStatus status)
        {
            Check();
            if (tasks.FindByTheme(cUser.Name, theme).Name != cUser.Name) throw new TasksAppException("Ошибка. Попытка изменения статуса дела не его автором.");
            return tasks.SetStatus(cUser.Name, theme, status);
        }

        //Удаляет дело из списка дел
        public void Remove(string theme)
        {
            Check();
            Task t = tasks.FindByTheme(cUser.Name, theme);
            if (t.Name != cUser.Name) throw new TasksAppException("Ошибка. Попытка удаления дела не его автором.");
            if (t.Status != TaskStatus.DONE) throw new TasksAppException("Ошибка. Попытка удаления не оконченного дела.");
            tasks.Remove(cUser.Name, t.Theme);
        }

        //Сохраняет базы данных (пользователей и дел)
        public void Dispose()
        {
            tasks.Dispose();
            users.Dispose();
        }

        //Конструктор
        public AppTasks()
        {
            users = new Users();
            tasks = new Tasks();
        }

        Users users; //база данных - пользователи
        Tasks tasks; //база данных - дела
        User cUser; //зарегистрированный пользователь

        //Проверяет регистрацию пользователя
        void Check()
        {
            if (cUser == null) throw new TaskException("Ошибка. Пользователь не аутентифицирован.");
        }
    }
}
