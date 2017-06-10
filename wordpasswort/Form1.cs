using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Word = Microsoft.Office.Interop.Word;


namespace wordpasswort
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                        Object oMissing = System.Reflection.Missing.Value;

            bool Found = false;

            PasswordGenerator Generator = new PasswordGenerator(true, true, true, 1, 4);
            Thread.Sleep(1000);
            string pw = Generator.Dequeue();

            Word.Application WordApp = new Word.Application();
            Word.Document ExistingDocument;

            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();
            String Dateiname = file.FileName;

            while (pw != "" && !Found)
            {
                try
                {
                    ExistingDocument = WordApp.Documents.Open(Dateiname, oMissing, false, oMissing, pw);
                    Found = true;
                    WordApp.Quit();
                    MessageBox.Show("Das Passwort lautet: " + pw);
                }
                catch (System.Runtime.InteropServices.COMException ex)
                {
                }
                pw = Generator.Dequeue();
            }

            if (!Found)
                MessageBox.Show("Kein passendes Passwort gefunden.");
        
 
        }

    }
}
