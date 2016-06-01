using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Pack_Crate
{
    class CSQLExecute
    {
        public string strDataSource = "";
        public string strInitialCatalog = "";
        public string strUserID = "";
        public string strPwd = "";

        public string m_strConnString;
        public SqlConnection m_connConnection;

        public CSQLExecute(string strMyDataSource, string strMyInitialCatalog, string strMyUserID, string strMyPwd)
        {
            strDataSource = strMyDataSource;
            strInitialCatalog = strMyInitialCatalog;
            strUserID = strMyUserID;
            strPwd = strMyPwd;
            //strPwd = "palqms";
        }

        private void Open()
        {
            try
            {
                m_strConnString = "Data Source=" + strDataSource + ";Initial Catalog=" + strInitialCatalog + ";User ID=" + strUserID + ";Password=" + strPwd;
                m_connConnection = new SqlConnection(m_strConnString);
                m_connConnection.Open();    //---Open Database Connection
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public void Close()
        {
            try
            {
                //m_connConnection.Close();
                if (m_connConnection.State != ConnectionState.Closed)
                {
                    m_connConnection.Close();   //---Close Database Connection
                }
            }
            catch (SqlException e)
            {
                throw e;
            }

        }


        public SqlDataReader GetDataReader(string strCommandString)
        {
            try
            {
                Open();
                SqlCommand cmdCommand = new SqlCommand(strCommandString, m_connConnection);
                cmdCommand.CommandTimeout = 1200;
                SqlDataReader drDataReader = cmdCommand.ExecuteReader();
                //notice: drDataReaderÃ»ÓÐclose
                Close();
                return drDataReader;
            }
            catch (SqlException e)
            {
                throw e;
            }

        }
        public int RunSQL(string strCommandString)
        {
            try
            {
                Open();
                SqlCommand cmdCommand = new SqlCommand(strCommandString, m_connConnection);
                cmdCommand.CommandTimeout = 1200;
                int nAffected = cmdCommand.ExecuteNonQuery();
                Close();
                return nAffected;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }


        public DataSet GetDataSet(string strCommandString, string strTableName)
        {
            try
            {
                Open();
                SqlCommand cmdCommand = new SqlCommand(strCommandString, m_connConnection);
                cmdCommand.CommandTimeout = 1200;
                SqlDataAdapter adAdapter = new SqlDataAdapter();
                adAdapter.SelectCommand = new SqlCommand(strCommandString, m_connConnection);

                DataSet dsDataSet = new DataSet();

                adAdapter.Fill(dsDataSet, strTableName);
                Close();
                return dsDataSet;
            }
            catch (SqlException e)
            {
                throw e;
            }

        }

        public DataTable GetDataTable(string strCommandString)
        {
            try
            {
                Open();
                SqlCommand cmdCommand = new SqlCommand(strCommandString, m_connConnection);
                cmdCommand.CommandTimeout = 1200;
                SqlDataAdapter adAdapter = new SqlDataAdapter();
                adAdapter.SelectCommand = new SqlCommand(strCommandString, m_connConnection);
                adAdapter.SelectCommand.CommandTimeout = 1200;
                DataSet dsDataSet = new DataSet();

                adAdapter.Fill(dsDataSet);


                Close();
                return dsDataSet.Tables[0];
                //Close();
            }
            catch (SqlException e)
            {
                throw e;
            }

        }

        public DataView GetDataView(string strCommandString, string strTableName)
        {
            try
            {
                Open();
                DataSet dsDataSet;
                dsDataSet = this.GetDataSet(strCommandString, strTableName);
                Close();
                if (dsDataSet != null)
                {
                    return new DataView(dsDataSet.Tables[strTableName]);
                }
                else
                {
                    return null;
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
    }
}
