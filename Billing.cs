using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SaraAppearrence
{
    public partial class SaraAppearrence: Form
    {
        DataTable dt = new DataTable();
        
        public SaraAppearrence()
        {
            InitializeComponent();
            CreateBillTable();
        }
        public void CreateBillTable()
        {
            dt.Columns.Add("Barcode");
            dt.Columns.Add("Description");
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("rate", typeof(decimal));
            dt.Columns.Add("MRP", typeof(decimal));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("discount %", typeof(decimal));
            dt.Columns.Add("Amount with disc", typeof(decimal));
            dataGridView1.DataSource = dt;
        }
        public void AddBarcode(string barcode,string description,int quantity, decimal rate,decimal discount,decimal netAmount)
        {
            bool found = false;

            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Barcode"].Value!=null && row.Cells["Barcode"].Value.ToString()==barcode)
                {
                    int current = Convert.ToInt32(row.Cells["Quantity"].Value);
                    current++;
                    row.Cells["Quantity"].Value = current;
                    row.Cells["Amount with disc"].Value = (rate * current) - discount * current;
                    row.Cells["discount %"].Value = discount * current;

                    found = true;
                    break;
                }
            }
            if(!found)
            {
                int quantity1 = 1;
                decimal discountValue = (rate * discount) / 100;
                decimal netAmount1 = (rate * quantity) - discountValue;
                dataGridView1.Rows.Add(barcode, description, quantity1, rate, discount, netAmount1);
            }
        }

        private void SaraAppearrence_Load(object sender, EventArgs e)
        {

        }
        public  void CalculateAmount()
        {
            decimal total = 0;
            foreach(DataRow row in dt.Rows)
            {
                total += Convert.ToDecimal(row["Amount with disc"]);
            }
            textNet.Text = total.ToString();
        }

        private void Barcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barcode = Barcode.Text;
                decimal rate = 999;
                decimal discount = 25;
                decimal discountValue = (rate * discount) / 100;
                decimal finalAmt = rate - discountValue;
                dt.Rows.Add(barcode, "regular", 1, rate, rate, rate, discount,finalAmt);
                CalculateAmount();
                Barcode.Clear();

            }
        }
            
      

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f = new Form1();
            f.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string payMode = Combo.Text;
            string Email1 = cash.Text;
            string refId = textBox2.Text;
            string bill = "";
            bill += "********* BILL RECEIPT *********\n";
            bill += "Date: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm") + "\n";
            bill += "Customer: " + name.Text + ", Mobile: " + mobile.Text + "\n"+"Email: "+Email1+"\n";
            bill += "------------------------------------------\n";
            bill += "Barcode\t\tDesc\t\tQty\t\tRate\t\tDisc\t\tNetAmt\n";

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    bill += row.Cells["Barcode"].Value + "\t" + "\t" +"|"+
                            row.Cells["Description"].Value + "\t" + "\t" + "|"+
                            row.Cells["Quantity"].Value + "\t" + "|" +
                            row.Cells["rate"].Value +"|"  + "\t" +
                            row.Cells["discount %"].Value + "\t" + "|" +
                            row.Cells["Amount with disc"].Value + "\t";
                }
            }

            bill += "------------------------------------------\n";
            bill += "Total: " + textNet.Text + "\n";
            if (payMode != "Cash" && !string.IsNullOrEmpty(refId))
            {
                bill+=payMode == "UpI" ? "UPI Ref No"+refId : "Transaction ID:" ;
            }
            decimal paid1 = Convert.ToDecimal(paid.Text);
            bill += "\n"+ "Paid: " + paid1 + "\n";
            
           
            bill += "Return: " + textBox7.Text + "\n";
            bill += "********* THANK YOU *********\n";

            // 2. Show second form with bill content
            Printing printForm = new Printing(bill);
            printForm.Show();


        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void paid_KeyDown(object sender, KeyEventArgs e)
        {
           
            decimal total = 0;
               if(e.KeyCode==Keys.Tab||e.KeyCode==Keys.Enter||e.KeyCode==Keys.PageDown)
            {
                foreach(DataRow r in dt.Rows)
                {
                    total += Convert.ToDecimal(r["Amount with disc"]);

                }
                
                decimal return1 = Convert.ToDecimal(paid.Text) - total;
                textBox7.Text = return1.ToString("0.0");
            }
        }

        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
