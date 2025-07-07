using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SaraAppearrence
{
    public partial class Register: Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            SqlMy();
        }
       

        public void SqlMy()
        {
            string password = textBox3.Text;
            int number;
            LoginDataContext lg = new LoginDataContext();
            register1 r = new register1();
            if(Regex.IsMatch(Name2.Text, @"^[A-Za-z\s]{2,}$"))
            {
                r.Name1 = Name2.Text;
            }
            else
            {
                MessageBox.Show("Enter Valid Name");
            }
            if (int.TryParse(IdNumber.Text, out number))
            {
                r.id = Convert.ToInt32(IdNumber.Text);
            }
            else
            {
                MessageBox.Show("Please enter a valid number");
                IdNumber.Focus();
                return;
            }

            if (Regex.IsMatch(textBox2.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                r.Email = textBox2.Text;
            }
            else
            {
                MessageBox.Show("Enter valid email");
                return;
            }

            if (textBox3.Text == textBox4.Text)
            {
                if (password.Length < 8 ||
                password.Any(char.IsUpper) ||
                password.Any(char.IsLower) ||
                password.Any(char.IsDigit) ||
                password.Any(ch => "!@#$%^&*()_+-=[]{}|;':\",./<>?".Contains(ch)))
                {
                    r.password1 = textBox3.Text;
                    
                }

            }
            else
            {
                MessageBox.Show("password must be 8 letters!");
                return;
            }
                lg.register1s.InsertOnSubmit(r);
            lg.SubmitChanges();
            MessageBox.Show("its successfully submit! ");


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f = new Form1();
            f.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox4.UseSystemPasswordChar = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox4.UseSystemPasswordChar = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox3.UseSystemPasswordChar = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox3.UseSystemPasswordChar= true;
        }

        private void Register_Load(object sender, EventArgs e)
        {
            textBox3.UseSystemPasswordChar = true;
            textBox4.UseSystemPasswordChar = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IdNumber.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            Name2.Clear();
        }
    }
}
