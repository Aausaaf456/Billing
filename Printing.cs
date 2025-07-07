using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace SaraAppearrence
{
    public partial class Printing: Form
    {
        private string billText;
        private PrintDocument pd = new PrintDocument();
        public Printing(string bill1)
        {

            InitializeComponent();
            billText = bill1;
            richTextBox1.Text = billText;
            pd.PrintPage += new PrintPageEventHandler(PrintDoc_PrintPage);
        }

        private void Printing_Load(object sender, EventArgs e)
        {
            richTextBox1.ReadOnly = true;
        }
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                // Use system default UI font (no custom font name)
                using (System.Drawing.Font printFont = Control.DefaultFont)
                {
                    float x = e.MarginBounds.Left;
                    float y = e.MarginBounds.Top;
                    float lineHeight = printFont.GetHeight(e.Graphics);

                    string[] lines = billText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    foreach (string line in lines)
                    {
                        e.Graphics.DrawString(line, printFont, Brushes.Black, x, y);
                        y += lineHeight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error printing: " + ex.Message);
            }
        }

        // ✅ Save button: Save as .txt or .pdf
        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|PDF files (*.pdf)|*.pdf";
            save.DefaultExt = "txt";
            save.FileName = "Bill_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

            if (save.ShowDialog() == DialogResult.OK)
            {
                string path = save.FileName;
                if (Path.GetExtension(path).ToLower() == ".txt")
                {
                    File.WriteAllText(path, billText);
                }
                else if (Path.GetExtension(path).ToLower() == ".pdf")
                {
                    SaveAsPDF(path, billText);
                }

                MessageBox.Show("Bill saved to:\n" + path, "Saved", MessageBoxButtons.OK);
            }
        }

        // ✅ Save as PDF using iTextSharp
        private void SaveAsPDF(string path, string billText)
        {
            Document doc = new Document(PageSize.A4);
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();
            doc.Add(new Paragraph(billText));
            doc.Close();
        }

        // ✅ Print button
        private void button2_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }
    }
}
