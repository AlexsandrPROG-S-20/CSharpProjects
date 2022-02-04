using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.AppModel
{
    //Класс - Дело
    public class Task : ICloneable
    {
        public string Theme { get; set; } //тема
        public string Text { get; set; } //текст
        public DateTime Date { get; set; } //дата выполнения
        public string Name { get; set; } //имя пользователя
        public TaskStatus Status { get; set; } //статус выполнения

        //Строковое представление дела
        public override string ToString()
        {
            return Theme + " " + Date.ToString("HH:mm dd.MM.yyyy");
        }

        //Сравнение дел
        public override bool Equals(object obj)
        {
            if (obj is Task) return (obj as Task).Theme == Theme && (obj as Task).Name == Name;
            else return false;
        }

        //Клонирование дела
        public object Clone()
        {
            return new Task { Theme = Theme, Text = Text, Date = Date, Name = Name, Status = Status };
        }
    }
}
