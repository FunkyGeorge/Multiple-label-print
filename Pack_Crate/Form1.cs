using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Configuration;
//using System.Runtime.InteropServices;
//using Microsoft.Win32.SafeHandles;

namespace Pack_Crate
{
    public partial class Form1 : Form
    {
 

        CSQLExecute CSET = new CSQLExecute(Para.pstrDBIP, Para.pstrDBName, Para.pstrDBUID, Para.pstrDBPWD);
    
      
        public Form1()
        {
            InitializeComponent();
            GetSerialPort();
        }



        private void Printbtn_Click(object sender, EventArgs e)
        {
            if (rdoCrate.Checked)
            {
                
               
                String fileToPrint = Application.StartupPath + "\\Assets\\Crate Label.txt";

                string sqlPrint = @"EXEC QCA_FABPOLabel '" + txtSN.Text.Trim() + "'";
                DataTable dtPrint = CSET.GetDataTable(sqlPrint);

                if (dtPrint.Rows.Count < 1)
                {
                    MessageBox.Show("No Data in MIS_PO_Info or QCA_FABmapping","ERROR!");
                    Printbtn.Enabled = true;
                    return;
                }
                
                printCrate(dtPrint, fileToPrint);
                Printbtn.Enabled = true;
                
                
                
            }
            else if (rdoPO.Checked)
            {
                printPO();
            }
        }



        public void printCrate(DataTable dt, string LblPath)
        {
            if (File.Exists(LblPath))
            {
                string Origin;
                string comport = cboPort.Text.Trim();
                using (StreamReader myFile = new StreamReader(LblPath))
                {
                    Origin = myFile.ReadToEnd();
                }


                if (Origin.Length == 0)
                {
                    return;
                }
                else
                {
                    string strContent;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strContent = Origin;
                        char[] BigSpaceChars = { Convert.ToChar(0x09) };
                        strContent = strContent.Replace(new string(BigSpaceChars), "   ");
                        strContent = strContent.Replace("<CPN>", dt.Rows[i]["CPN"].ToString().ToUpper());
                        strContent = strContent.Replace("<RACKASSET>", dt.Rows[i]["RACKASSET"].ToString().ToUpper());
                        strContent = strContent.Replace("<LOC>", dt.Rows[i]["LOC"].ToString().ToUpper());
                        strContent = strContent.Replace("<RACKTYPE>", dt.Rows[i]["RACKTYPE"].ToString().ToUpper());
                        strContent = strContent.Replace("<RACKDESC>", dt.Rows[i]["RackDesc"].ToString().ToUpper());

                        try
                        {
                            SerialPort port = new SerialPort(comport, 9600, Parity.None, 8, StopBits.One);
                            port.Open();
                            port.Write(strContent);
                            port.Write(new byte[] { 0x0A, 0xE2, 0xFF }, 0, 3);
                            port.Close();
                        }

                        catch (Exception com)
                        {
                            MessageBox.Show(com.ToString());
                        }
                    }


               
                    return;
                }
            }
            else
            {
                return;
            }
        }


        private void printPO()
        {
            Printbtn.Enabled = false;


            String fileToPrint = Application.StartupPath + "\\Assets\\RackPrint.txt";

            string sqlPrint = @"EXEC QCA_FABPOLabel '" + txtSN.Text.Trim() + "'";
            DataTable dtPrint = CSET.GetDataTable(sqlPrint);

            if (dtPrint.Rows.Count < 1)
            {
                MessageBox.Show("No Data in MIS_PO_Info or QCA_FABmapping","ERROR!");
                Printbtn.Enabled = true;
                return;
            }

            if (File.Exists(fileToPrint))
            {
                string Original = "";
                string comport = cboPort.Text.Trim();
                using (StreamReader myFile = new StreamReader(fileToPrint))
                {
                    Original = myFile.ReadToEnd();
                }


                if (Original.Length == 0)
                {
                    //return false;
                }
                else
                {
                    string strContent;
                    for (int i = 0; i < dtPrint.Rows.Count; i++)
                    {
                        strContent = Original;
                        String textlength = dtPrint.Rows[i]["MODEL"].ToString();
                        int length = textlength.Length;
                        int number = 35;
                        number = number - length;

                        char[] BigSpaceChars = { Convert.ToChar(0x09) };
                        strContent = strContent.Replace(new string(BigSpaceChars), "   ");
                        strContent = strContent.Replace("<CPN>", dtPrint.Rows[i]["CPN"].ToString().ToUpper());
                        strContent = strContent.Replace("<MODEL>", dtPrint.Rows[i]["MODEL"].ToString().ToUpper());
                        strContent = strContent.Replace("<LOC>", dtPrint.Rows[i]["LOC"].ToString().ToUpper());
                        strContent = strContent.Replace("<PO>", dtPrint.Rows[i]["PO"].ToString().ToUpper());
                        if (length <= 10)
                        { strContent = strContent.Replace("<Size>", "30"); }
                        else if (length > 23)
                        { strContent = strContent.Replace("<Size>", (number + 3).ToString()); }
                        else
                        { strContent = strContent.Replace("<Size>", number.ToString()); }

                        try
                        {
                            SerialPort port = new SerialPort(comport, 9600, Parity.None, 8, StopBits.One);
                            port.Open();
                            port.Write(strContent);
                            port.Write(new byte[] { 0x0A, 0xE2, 0xFF }, 0, 3);
                            port.Close();
                        }

                        catch (Exception com)
                        {
                            MessageBox.Show(com.ToString());
                        }
                    }


                
                    Printbtn.Enabled = true;
                }
            }
            else
            {
                Printbtn.Enabled = true;
            }
        }

        private void txtSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13 || txtSN.Text.Length == 0)
            {
                return;
            }
            else
            {
                this.ProcessTabKey(true);
            }
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

        private void groupLabelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGroup frmgroup = new frmGroup(this);
            frmgroup.Show(this);
            this.Enabled = false;
           
        }

        private void shippingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShipping frmshipping = new frmShipping(this);
            frmshipping.Show(this);
            this.Enabled = false;
            
        }
    }
}
