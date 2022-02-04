using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.AppModel
{
    //Класс - Статус дела
    public enum TaskStatus
    {
        DO, //ожидает выполнения
        INPROGRESS, //выполняется
        DONE //готово
    }
}
