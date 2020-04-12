using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clicker
{
    public partial class Form1 : Form
    {
        int time;
        bool left = true;
        hook h;
        public Form1()
        {
            InitializeComponent();
            h = new hook(label6,label8,button1);
            h.SetHook();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                time = Convert.ToInt32(textBox1.Text);
                if(time >= 100 && time  <= 60000)
                {
                    h.NeedSetSetting = false;
                    h.createTimer(left, time);

                    label6.Text = "Готов к запуску";
                    label6.ForeColor = Color.Orange;
                } else
                {
                    MessageBox.Show("Неверные настройки частоты");
                }
            } catch
            {

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            left = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            left = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://vk.com/mrtokin");
            } catch
            {
                MessageBox.Show("Ошибка при открытии ссылки");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://t.me/mrtokin");
            }
            catch
            {
                MessageBox.Show("Ошибка при открытии ссылки");
            }
        }

   
    }
}
