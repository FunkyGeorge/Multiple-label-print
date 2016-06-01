using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace Pack_Crate
{
    public partial class frmGroup : Form
    {
        Form1 origin;

        public frmGroup(Form1 callForm)
        {
            InitializeComponent();
            GetSerialPort();
            origin = callForm;
        }

     

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            origin.Enabled = true;
        }

        private void GetSerialPort()
        {
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length != 0)
            {
                foreach (string StrPort in ports)
                {
                    cboPort.Items.Add(StrPort);
                }
                cboPort.SelectedIndex = 0;
            }
        }

        private void frmGroup_FormClosed(object sender, FormClosedEventArgs e)
        {
            origin.Enabled = true;
        }

        private void txtPrint_Click(object sender, EventArgs e)
        {
            

            string strContent = "";
            
            string LblPath = Application.StartupPath + "\\Assets\\FABgroup.txt";


            using (StreamReader myFile = new StreamReader(LblPath))
            {
                strContent = myFile.ReadToEnd();
            }

            for (int inc = 0; inc < 3; inc++)
            {
                printGroup(strContent,inc);
            }
            txtGroupNum.Focus();
        }   

        private void printGroup(string strContent,int i)
        {
            string[] Alpha = { "A", "B", "C" };
            string comport = cboPort.Text.Trim();
            string size = "";
            if (int.Parse(txtGroupNum.Text) < 10)
            {
                size = "120";
            }
            else if (int.Parse(txtGroupNum.Text) <= 20)
            {
                size = "110";
            }
            else
            {
                MessageBox.Show("Must pick a number 1-20");
                return;
            }

            strContent = strContent.Replace("<Size>",size);
            strContent = strContent.Replace("<Num>", txtGroupNum.Text.Trim());
            strContent = strContent.Replace("<Alpha>", Alpha[i]);

            SerialPort port = new SerialPort(comport, 9600, Parity.None, 8, StopBits.One);
            port.Open();
            port.Write(strContent);
            port.Write(new byte[] { 0x0A, 0xE2, 0xFF }, 0, 3);
            port.Close();
        }

      
        private void txtGroupNum_KeyPress(object sender, KeyPressEventArgs e)
        {
         if (e.KeyChar != 13 || txtGroupNum.Text.Length == 0)
            {
                return;
            }
            else
            {
                this.ProcessTabKey(true);
            }
        }
        
 
    }
}
