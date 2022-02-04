using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskBook.AppModel;

namespace TaskBook
{
    public partial class AuthForm : Form
    { 
        IAuth userUtil; //пользователи

        public AuthForm(IAuth userUtil)
        {
            InitializeComponent();
            this.userUtil = userUtil;
        }

        //Обрабатывает событие нажатия кнопки "Войти"
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                userUtil.Auth(textBox1.Text.Trim(), textBox2.Text.Trim());
                label3.Text = string.Empty;
                DialogResult = DialogResult.OK;
                Hide();
            }
            catch (Exception excep)
            {
                label3.Text = excep.Message;
            }
        }

        //Обрабатывает событие нажатия кнопки "Зарегистрироваться"
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Trim() == string.Empty || char.IsDigit(textBox1.Text[0])) throw new ArgumentException("Неверный формат логина.");
                if (textBox2.Text.Trim() == string.Empty) throw new ArgumentException("Неверный формат пароля.");
                userUtil.AddUser(textBox1.Text.Trim(), textBox2.Text.Trim());
                label3.Text = "Регистрация прошла успешно.";
            }
            catch (Exception excep)
            {
                label3.Text = excep.Message;
            }
        }

        //Обрабатывает событие нажатия кнопки "Удалить регистрацию"
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Trim() == string.Empty || char.IsDigit(textBox1.Text[0])) throw new ArgumentException("Неверный формат логина.");
                userUtil.RemoveUser(textBox1.Text.Trim());
                label3.Text = "Пользователь успешно удален.";
            }
            catch (Exception excep)
            {
                label3.Text = excep.Message;
            }
        }

        //Обрабатывает событие перед закрытием формы
        private void AuthForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK) Application.Exit();
        }
    }
}
