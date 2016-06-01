using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace Pack_Crate
{
    public partial class frmShipping : Form
    {
        Form1 origin;

        public frmShipping(Form1 callForm)
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            int qty = 0;
            try
            {
                qty = int.Parse(txtQTY.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            for (int i = 0; i < qty; i++)
            {
                printShipping();
            }
        }

        private void printShipping()
        { 
            string strContent = "";
            string comport = cboPort.Text.Trim();
            string LblPath = Application.StartupPath + "\\Assets\\FABshipping.txt";


            using (StreamReader myFile = new StreamReader(LblPath))
            {
                strContent = myFile.ReadToEnd();
            }

            SerialPort port = new SerialPort(comport, 9600, Parity.None, 8, StopBits.One);
            port.Open();
            port.Write(strContent);
            port.Write(new byte[] { 0x0A, 0xE2, 0xFF }, 0, 3);
            port.Close();
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

        private void frmShipping_FormClosed(object sender, FormClosedEventArgs e)
        {
            origin.Enabled = true;
        }

     
    }
}
