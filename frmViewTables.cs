using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Gen837X222A1
{
    public partial class frmViewTables : Form
    {
        public frmViewTables()
        {
            InitializeComponent();
        }

        private string sConnection = ConfigurationManager.ConnectionStrings["Development"].ConnectionString;

        private void frmViewTables_Load(object sender, EventArgs e)
        {
            string sSql;
            string sAppPath = AppDomain.CurrentDomain.BaseDirectory;
            
            SqlConnection oConnection = new SqlConnection(sConnection);

            oConnection.Open();

            sSql = "select * from [Interchange]";
            SqlDataAdapter oInterchangeAdapter = new SqlDataAdapter(sSql, oConnection);
            DataSet oInterchangeDs = new DataSet("dsInterchange");
            oInterchangeAdapter.Fill(oInterchangeDs, "dsInterchange");
            dgvInterchange.DataSource = oInterchangeDs.Tables["dsInterchange"];

            sSql = "select * from [FuncGroup]";
            SqlDataAdapter oGroupAdapter = new SqlDataAdapter(sSql, oConnection);
            DataSet oGroupDs = new DataSet("dsGroup");
            oGroupAdapter.Fill(oGroupDs, "dsGroup");
            dgvGroup.DataSource = oGroupDs.Tables["dsGroup"];

            sSql = "select * from [837X222_Header]";
            SqlDataAdapter oHeaderAdapter = new SqlDataAdapter(sSql, oConnection);
            DataSet oHeaderDs = new DataSet("dsHeader");
            oHeaderAdapter.Fill(oHeaderDs, "dsHeader");
            dgvHeader.DataSource = oHeaderDs.Tables["dsHeader"];

            sSql = "select * from [837X222_InfoSource]";
            SqlDataAdapter oInfoSourceAdapter = new SqlDataAdapter(sSql, oConnection);
            DataSet oInfoSourceDs = new DataSet("dsInfoSource");
            oInfoSourceAdapter.Fill(oInfoSourceDs, "dsInfoSource");
            dgvInfoSource.DataSource = oInfoSourceDs.Tables["dsInfoSource"];

            sSql = "select * from [837X222_Subscriber]";
            SqlDataAdapter oSubscriberAdapter = new SqlDataAdapter(sSql, oConnection);
            DataSet oSubscriberDs = new DataSet("dsSubscriber");
            oSubscriberAdapter.Fill(oSubscriberDs, "dsSubscriber");
            dgvSubscriber.DataSource = oSubscriberDs.Tables["dsSubscriber"];

            sSql = "select * from [837X222_Dependent]";
            SqlDataAdapter oDependentAdapter = new SqlDataAdapter(sSql, oConnection);
            DataSet oDependentDs = new DataSet("dsDependent");
            oDependentAdapter.Fill(oDependentDs, "dsDependent");
            dgvDependent.DataSource = oDependentDs.Tables["dsDependent"];

            sSql = "select * from [837X222_Claims]";
            SqlDataAdapter oClaimsAdapter = new SqlDataAdapter(sSql, oConnection);
            DataSet oClaimsDs = new DataSet("dsClaims");
            oClaimsAdapter.Fill(oClaimsDs, "dsClaims");
            dgvClaims.DataSource = oClaimsDs.Tables["dsClaims"];

            sSql = "select * from [837X222_ServiceLine]";
            SqlDataAdapter oServiceLineAdapter = new SqlDataAdapter(sSql, oConnection);
            DataSet oServiceLineDs = new DataSet("dsServiceLine");
            oServiceLineAdapter.Fill(oServiceLineDs, "dsServiceLine");
            dgvServiceLine.DataSource = oServiceLineDs.Tables["dsServiceLine"];

            sSql = "select * from [837X222_ServiceLineAdj]";
            SqlDataAdapter oServiceLineAdjAdapter = new SqlDataAdapter(sSql, oConnection);
            DataSet oServiceLineAdjDs = new DataSet("dsServiceLineAdj");
            oServiceLineAdjAdapter.Fill(oServiceLineAdjDs, "dsServiceLineAdj");
            dgvServiceLineAdj.DataSource = oServiceLineAdjDs.Tables["dsServiceLineAdj"];

            sSql = "select * from [837X222_ServiceDocument]";
            SqlDataAdapter oServiceDocumentAdapter = new SqlDataAdapter(sSql, oConnection);
            DataSet oServiceDocumentDs = new DataSet("dsServiceDocument");
            oServiceDocumentAdapter.Fill(oServiceDocumentDs, "dsServiceDocument");
            dgvServiceDocument.DataSource = oServiceDocumentDs.Tables["dsServiceDocument"];

            oConnection.Close();

        }
    }
}
