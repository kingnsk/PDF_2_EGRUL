using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;


namespace PDF_2_EGRUL
{
    public partial class Form1 : Form
    {
        public string fileName;

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = " ";
            openFileDialog1.Filter = "PDF Files(*.pdf)|*.pdf|All files(*.*)|*.*";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string fileName = "1.pdf";
            string writePath1 = fileName+"_.txt";
            string writePath2 = fileName+"_.txt";
            string text_in = ExtractTextFromPdf(fileName); // весь текст из pdf
            string substring = "Код и наименование вида деятельности";
            string name = "", inn = "";

 /*           using (StreamWriter sw1 = new StreamWriter(writePath1, false, System.Text.Encoding.Default))
            {
                sw1.WriteLine(text_in);
            }
  */

            StreamWriter sw2 = new StreamWriter(writePath2, false, System.Text.Encoding.Default);

            name = text_in.Substring(text_in.IndexOf("Сокращенное наименование")+ "Сокращенное наименование".Length+1,100);
            string[] name_out = name.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            inn = text_in.Substring(text_in.IndexOf("ИНН") + "ИНН".Length + 1, 100);
            string[] inn_out = inn.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            sw2.WriteLine(name_out[0]+" (ИНН: "+inn_out[0]+")");
            

            var indices = new List<int>();

            int index = text_in.IndexOf(substring, 0);
            while (index > -1)
            {
                indices.Add(index);
                index = text_in.IndexOf(substring, index + substring.Length);
            }

            int i = 0,k=10,probel=0;
            string[] str_out;
            string str_out1 = "",str_tmp="",str_all="",str_all_out="";


            for(i=0;i<indices.Count-1;i++)
            {
                probel = 0;
                str_all_out = "";
                str_out1 = text_in.Substring(indices[i]+substring.Length+1, indices[i + 1] - indices[i]-substring.Length);
                str_out = str_out1.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                str_tmp = str_out[1] +" "+ str_out[2] +" "+ str_out[3]+" "+str_out[4]+"0";
                probel = str_out[0].IndexOf(" ");
                str_out[0] = str_out[0].Remove(probel, 1).Insert(probel, textBox1.Text) ;
                str_all = str_out[0] + " " + str_tmp;
                

                for(k=0;k<=9;k++) str_all_out = str_all_out + str_all[k];

                k = 10;
                while (!Char.IsDigit(str_all[k]))
                {
                    str_all_out = str_all_out + str_all[k];
                    k++;
                }

                str_all = str_all_out;
                
                Console.WriteLine(indices[i + 1] + " | " + indices[i] + " | " + str_all);
                sw2.WriteLine(str_all);
            }

            probel = 0;
            str_all_out = "";
            str_out1 = text_in.Substring(indices[i] + substring.Length+1, 200);
            str_out = str_out1.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            str_tmp = str_out[1] + " " + str_out[2] + " " + str_out[3]+" "+str_out[4];
            probel = str_out[0].IndexOf(" ");
            str_out[0] = str_out[0].Remove(probel, 1).Insert(probel, textBox1.Text);
            str_all = str_out[0] + " " + str_tmp;
            for (k = 0; k <= 9; k++) str_all_out = str_all_out + str_all[k];

            k = 10;
            while (!Char.IsDigit(str_all[k]))
            {
                str_all_out = str_all_out + str_all[k];
                k++;
            }

            str_all = str_all_out;
            Console.WriteLine(indices[i] + " | " + indices[i] + " | " + str_all);
            sw2.WriteLine(str_all);
            
            
            sw2.Close();
            label2.Text = "ВЫПОЛНЕНО! \r\n"+writePath2+"\r\nСОХРАНЕН!";
        }

        private static string ExtractTextFromPdf(string path)
        {
            PDDocument doc = null;
            try
            {
                doc = PDDocument.load(path);
              PDFTextStripper stripper = new PDFTextStripper();
               return stripper.getText(doc);
               
            }
            finally
            {
                if (doc != null)
                {
                    doc.close();
                }
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            fileName = openFileDialog1.FileName;
            // читаем файл в строку
            //string fileText = System.IO.File.ReadAllText(filename);
            //textBox1.Text = fileText;
            //MessageBox.Show("Файл открыт");
            label1.Text = fileName + " - ОТКРЫТ";
        }
    }
}
