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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SaraAppearrence
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar =true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Validation();
        }

      
            public void Validation()
        {
            string email = textBox1.Text;
            string password = textBox2.Text;

            // Step 1: Check email format
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Please enter a valid email address.");
                return;
            }

            // Step 2: Check password strength
            bool strongPassword = password.Length >= 8 &&
                                  password.Any(char.IsUpper) &&
                                  password.Any(char.IsLower) &&
                                  password.Any(char.IsDigit) &&
                                  password.Any(ch => "!@#$%^&*()_+-=[]{}|;':\",./<>?".Contains(ch));

            if (!strongPassword)
            {
                MessageBox.Show("Password must be at least 8 characters and include uppercase, lowercase, digit, and special character.");
                return;
            }

            // Step 3: Check email + password in database
            using (LoginDataContext lg = new LoginDataContext())
            {
                var user = lg.register1s.FirstOrDefault(u => u.Email == email && u.password1 == password);

                if (user != null)
                {
                   
                    MessageBox.Show("Login successful!");
                    SaraAppearrence m = new SaraAppearrence();
                    m.label15.Text = user.Name1;
                    m.label17.Text = user.Email;
                    m.Show();
                }
                else
                {
                    MessageBox.Show("Email or password is incorrect.");
                }
            }

            }

       

        private void button2_Click(object sender, EventArgs e)
        {
          
            Register r = new Register();
            r.Show();
        }
    }
}
