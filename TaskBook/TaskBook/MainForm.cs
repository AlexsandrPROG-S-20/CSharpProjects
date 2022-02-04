using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using TaskBook.AppModel;

namespace TaskBook
{
    //Главный класс приложения
    public partial class MainForm : Form
    {
        AuthForm authForm; //форма аутентификации
        AppTasks appTasks; //дела

        //Конструктор
        public MainForm()
        {
            InitializeComponent();
            try
            {
                appTasks = new AppTasks();
                authForm = new AuthForm(appTasks);
            }
            catch (Exception)
            {
                Close();
            }
            comboBox1.DataSource = new[] {
                new {Name="Сделать", Value= TaskStatus.DO },
                new { Name = "В процессе", Value = TaskStatus.INPROGRESS },
                new {Name="Готово", Value=TaskStatus.DONE}
                };
            comboBox1.ValueMember = "Value";
            comboBox1.DisplayMember = "Name";
            dateTimePicker1.CustomFormat = "HH:mm dd.MM.yyyy";
        }

        //Возвращает ListBox отображающий дела с указанным статусом
        ListBox ListBoxByStatus(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.DO:
                    return listBox1;
                case TaskStatus.INPROGRESS:
                    return listBox2;
                case TaskStatus.DONE:
                    return listBox3;
                default:
                    return listBox1;
            }
        }

        //Очищает выделение указанных ListBox и, при необходимости, очищает их
        private void ClearSelectedListBoxes(int[] idx, bool isClear = false)
        {
            var lb = new[] { listBox1, listBox2, listBox3 };
            for (int i = 0; i < idx.Length; i++)
            {
                lb[idx[i] - 1].ClearSelected();
                if (isClear) lb[idx[i] - 1].Items.Clear();
            }
        }

        //Перезаписывает содержимое указанного ListBox
        private void RefreshListBox(TaskStatus status, Task task = null)
        {
            ListBox lb = ListBoxByStatus(status);
            lb.Items.Clear();
            foreach (Task t in appTasks.FindTasksByStatus(status))
            {
                lb.Items.Add(t);
                if (task != null && task.Equals(t))
                {
                    ClearSelectedListBoxes(new[] { 1, 2, 3 });
                    lb.SelectedIndex = lb.Items.Count - 1;
                }
            }
        }

        //Заполняет всю форму данными
        private void StartFill()
        {
            label5.Text = "Исполнитель " + appTasks.User.Name;
            RefreshListBox(TaskStatus.DO);
            RefreshListBox(TaskStatus.INPROGRESS);
            RefreshListBox(TaskStatus.DONE);
        }

        //Заполняет форму данными дела
        private void FillTaskInfo(Task task)
        {
            textBox1.Text = task.Text;
            textBox2.Text = task.Theme;
            comboBox1.SelectedValue = task.Status;
            dateTimePicker1.Value = task.Date;
        }

        //Удаляет из формы данные дела
        private void ClearTaskInfo()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            comboBox1.SelectedValue = TaskStatus.DO;
            dateTimePicker1.Value = DateTime.Now;
        }

        //Аутентифицирует пользователя
        private void Auth()
        {
            label5.Text = "Исполнитель";
            ClearSelectedListBoxes(new[] { 1, 2, 3 }, true);
            ClearTaskInfo();
            appTasks.Exit();
            if (authForm.ShowDialog() == DialogResult.OK)
                StartFill();
        }

        //Обрабатывает событие открытия формы
        private void MainForm_Shown(object sender, EventArgs e)
        {
            Auth();
        }

        //Обрабатывает событие закрытия формы
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            appTasks.Dispose();
        }

        //Обрабатывает событие нажатия кнопки "Сохранить"
        private void SaveTaskButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text.Trim() == string.Empty) throw new TaskException("Неверно указана тема дела.");
                Task ft = appTasks.FindTaskByTheme(textBox2.Text.Trim());
                TaskStatus ts = (TaskStatus)comboBox1.SelectedValue;
                if (ft == null)
                    if (ts != TaskStatus.DO)
                        throw new TaskException("Новое дело можно создавать только со статусом \"Сделать\"");
                    else
                        ft = appTasks.AddDo(textBox2.Text.Trim(), textBox1.Text, dateTimePicker1.Value);
                else
                {
                    if (ft.Text != textBox1.Text || ft.Date != dateTimePicker1.Value)
                        throw new TaskException("Для созданного дела можно изменять только его статус.");
                    TaskStatus oldTs = appTasks.SetStatus(textBox2.Text.Trim(), ts);
                    RefreshListBox(oldTs);
                }
                RefreshListBox(ts, ft);
                statusStrip1.Items[0].Text = string.Empty;
            }
            catch (Exception excep)
            {
                statusStrip1.Items[0].Text = excep.Message;
            }
        }

        //Обрабатывает событие нажатия кнопки "Удалить"
        private void DeleteTaskButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (listBox3.SelectedIndex != -1)
                {
                    appTasks.Remove((listBox3.SelectedItem as Task).Theme);
                    listBox3.Items.RemoveAt(listBox3.SelectedIndex);
                    ClearTaskInfo();
                    statusStrip1.Items[0].Text = string.Empty;
                }
                else throw new TaskException("Удалять можно только дела из списка готово.");
            }
            catch (Exception excep)
            {
                StartFill();
                statusStrip1.Items[0].Text = excep.Message;
            }
        }

        //Обрабатывает событие выбора элемента списка 1
        private void DOlistBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ClearSelectedListBoxes(new[] { 2, 3 });
                FillTaskInfo(appTasks.FindTaskByTheme((listBox1.SelectedItem as Task).Theme));
            }
        }

        //Обрабатывает событие выбора элемента списка 2
        private void INPROGRESSlistBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                ClearSelectedListBoxes(new[] { 1, 3 });
                FillTaskInfo(appTasks.FindTaskByTheme((listBox2.SelectedItem as Task).Theme));
            }
        }

        //Обрабатывает событие выбора элемента списка 3
        private void DONElistBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                ClearSelectedListBoxes(new[] { 1, 2 });
                FillTaskInfo(appTasks.FindTaskByTheme((listBox3.SelectedItem as Task).Theme));
            }
        }

        //Обрабатывает событие нажатия кнопки "Выйти"
        private void ExitButtonClick(object sender, EventArgs e)
        {
            Auth();
        }
    }
}
