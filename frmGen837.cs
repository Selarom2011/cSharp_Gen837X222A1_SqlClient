using Edidev.FrameworkEDI;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Gen837X222A1
{
    ////The tables in the 837X222A1 database were created using the scripts in the folder "CREATE_SQL_TABLES"
    public partial class frmGen837 : Form
    {
        public frmGen837()
        {
            InitializeComponent();
        }

        private string sConnection = ConfigurationManager.ConnectionStrings["Development"].ConnectionString;

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            ediDocument oEdiDoc = null;
            ediSchemas oSchemas = null;
            ediInterchange oInterchange = null;
            ediGroup oGroup = null;
            ediTransactionSet oTransactionSet = null;
            ediDataSegment oSegment = null;

            bool bDependent = false;
            int nHlCounter = 0;
            int nHlSubscriberParent = 0;
            int nHlDependentParent = 0;
            int nElemPos = 0;

            string sSql = "";
            string sAppPath = AppDomain.CurrentDomain.BaseDirectory;
            
            SqlConnection oConnection = new SqlConnection(sConnection);

            string sEdiFile = "837_5010X222A1.X12";
            string sSefFile = "837_005010X222A1.SemRef.SEF"; //EVALUATION SEF FILE



            //CREATES OEDIDOC OBJECT
            oEdiDoc = new ediDocument();

            //THIS MAKES CERTAIN THAT FREDI ONLY USES THE SEF FILE PROVIDED, AND THAT IT DOES 
            //NOT USE ITS BUILT-IN STANDARD REFERENCE TABLE TO GENERATE THE EDI FILE.
            oSchemas = (ediSchemas)oEdiDoc.GetSchemas();
            oSchemas.EnableStandardReference = false;

            //ENABLES FORWARD WRITE, AND INCREASES BUFFER I/O TO IMPROVE PERFORMANCE
            oEdiDoc.CursorType = DocumentCursorTypeConstants.Cursor_ForwardWrite;
            oEdiDoc.set_Property(DocumentPropertyIDConstants.Property_DocumentBufferIO, 200);

            //SET TERMINATORS
            oEdiDoc.SegmentTerminator = "~{13:10}";
            oEdiDoc.ElementTerminator = "*";
            oEdiDoc.CompositeTerminator = ":";

            //SET RELEASE INDICATOR
            oEdiDoc.ReleaseIndicator = "?";

            //LOADS THE SEF FILE
            oEdiDoc.LoadSchema(sAppPath + sSefFile, 0);

            sSql = "select * from [Interchange]";
            SqlDataAdapter oInterchangeAdapter = new SqlDataAdapter(sSql, oConnection);
            DataSet oInterchangeDs = new DataSet("dsInterchange");
            oInterchangeAdapter.Fill(oInterchangeDs, "dsInterchange");

            foreach (DataRow oInterchangeRow in oInterchangeDs.Tables["dsInterchange"].Rows)
            {
                //CREATES THE ISA SEGMENT
                ediInterchange.Set(ref oInterchange, oEdiDoc.CreateInterchange("X", "005010"));
                ediDataSegment.Set(ref oSegment, oInterchange.GetDataSegmentHeader());
                oSegment.set_DataElementValue(1, 0, oInterchangeRow["ISA01_AuthorizationInfoQlfr"].ToString());     //Authorization Information Qualifier
                oSegment.set_DataElementValue(2, 0, oInterchangeRow["ISA02_AuthorizationInfo"].ToString());     //Authorization Information
                oSegment.set_DataElementValue(3, 0, oInterchangeRow["ISA03_SecurityInfoQlfr"].ToString());     //Security Information Qualifier
                oSegment.set_DataElementValue(4, 0, oInterchangeRow["ISA04_SecurityInfo"].ToString());     //Security Information
                oSegment.set_DataElementValue(5, 0, oInterchangeRow["ISA05_SenderIdQlfr"].ToString());     //Interchange ID Qualifier
                oSegment.set_DataElementValue(6, 0, oInterchangeRow["ISA06_SenderId"].ToString());     //Interchange Sender ID
                oSegment.set_DataElementValue(7, 0, oInterchangeRow["ISA07_ReceiverIdQlfr"].ToString());     //Interchange ID Qualifier
                oSegment.set_DataElementValue(8, 0, oInterchangeRow["ISA08_ReceiverId"].ToString());     //Interchange Receiver ID
                oSegment.set_DataElementValue(9, 0, oInterchangeRow["ISA09_Date"].ToString());     //Interchange Date
                oSegment.set_DataElementValue(10, 0, oInterchangeRow["ISA10_Time"].ToString());     //Interchange Time
                oSegment.set_DataElementValue(11, 0, oInterchangeRow["ISA11_RepetitionSeparator"].ToString());     //Repetition Separator
                oSegment.set_DataElementValue(12, 0, oInterchangeRow["ISA12_ControlVersionNumber"].ToString());     //Interchange Control Version Number
                oSegment.set_DataElementValue(13, 0, oInterchangeRow["ISA13_ControlNumber"].ToString());     //Interchange Control Number
                oSegment.set_DataElementValue(14, 0, oInterchangeRow["ISA14_AcknowledgmentRequested"].ToString());     //Acknowledgment Requested
                oSegment.set_DataElementValue(15, 0, oInterchangeRow["ISA15_UsageIndicator"].ToString());     //Usage Indicator
                oSegment.set_DataElementValue(16, 0, oInterchangeRow["ISA16_ComponentElementSeparator"].ToString());     //Component Element Separator

                sSql = "select * from [FuncGroup] where Interkey = " + oInterchangeRow["Interkey"].ToString();
                SqlDataAdapter oGroupAdapter = new SqlDataAdapter(sSql, oConnection);
                DataSet oGroupDs = new DataSet("dsGroup");
                oGroupAdapter.Fill(oGroupDs, "dsGroup");

                foreach (DataRow oGroupRow in oGroupDs.Tables["dsGroup"].Rows)
                {
                    //CREATES THE GS SEGMENT
                    ediGroup.Set(ref oGroup, oInterchange.CreateGroup("005010X222A1"));
                    ediDataSegment.Set(ref oSegment, oGroup.GetDataSegmentHeader());
                    oSegment.set_DataElementValue(1, 0, oGroupRow["GS01_FunctionalIdfrCode"].ToString());     //Functional Identifier Code
                    oSegment.set_DataElementValue(2, 0, oGroupRow["GS02_SendersCode"].ToString());     //Application Sender//s Code
                    oSegment.set_DataElementValue(3, 0, oGroupRow["GS03_ReceiversCode"].ToString());     //Application Receiver//s Code
                    oSegment.set_DataElementValue(4, 0, oGroupRow["GS04_Date"].ToString());     //Date
                    oSegment.set_DataElementValue(5, 0, oGroupRow["GS05_Time"].ToString());     //Time
                    oSegment.set_DataElementValue(6, 0, oGroupRow["GS06_GroupControlNumber"].ToString());     //Group Control Number
                    oSegment.set_DataElementValue(7, 0, oGroupRow["GS07_ResponsibleAgencyCode"].ToString());     //Responsible Agency Code
                    oSegment.set_DataElementValue(8, 0, oGroupRow["GS08_VersionReleaseCode"].ToString());     //Version / Release / Industry Identifier Code

                    sSql = "select * from [837X222_Header] where Groupkey = " + oGroupRow["Groupkey"].ToString();
                    SqlDataAdapter oHeaderAdapter = new SqlDataAdapter(sSql, oConnection);
                    DataSet oHeaderDs = new DataSet("dsHeader");
                    oHeaderAdapter.Fill(oHeaderDs, "dsHeader");

                    foreach (DataRow oHeaderRow in oHeaderDs.Tables["dsHeader"].Rows)
                    {
                        //CREATES THE ST SEGMENT
                        ediTransactionSet.Set(ref oTransactionSet, oGroup.CreateTransactionSet("837"));
                        ediDataSegment.Set(ref oSegment, oTransactionSet.GetDataSegmentHeader());
                        oSegment.set_DataElementValue(1, 0, oHeaderRow["ST01_TranSetIdfrCode"].ToString());     //Transaction Set Identifier Code
                        oSegment.set_DataElementValue(2, 0, oHeaderRow["ST02_TranSetControlNo"].ToString());     //Transaction Set Control Number
                        oSegment.set_DataElementValue(3, 0, oHeaderRow["ST03_ImplementConventionRef"].ToString());     //Implementation Convention Reference

                        //BHT - BEGINNING OF HIERARCHICAL TRANSACTION
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("BHT"));
                        oSegment.set_DataElementValue(1, 0, oHeaderRow["BHT01_HierarchStructCode"].ToString());     //Hierarchical Structure Code
                        oSegment.set_DataElementValue(2, 0, oHeaderRow["BHT02_TranSetPurposeCode"].ToString());     //Transaction Set Purpose Code
                        oSegment.set_DataElementValue(3, 0, oHeaderRow["BHT03_RefId"].ToString());     //Reference Identification
                        oSegment.set_DataElementValue(4, 0, oHeaderRow["BHT04_Date"].ToString());     //Date
                        oSegment.set_DataElementValue(5, 0, oHeaderRow["BHT05_Time"].ToString());     //Time
                        oSegment.set_DataElementValue(6, 0, oHeaderRow["BHT06_TranTypeCode"].ToString());     //Transaction Type Code

                        //1000A SUBMITTER
                        if (true)
                        {
                            //NM1 - SUBMITTER NAME
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("NM1\\NM1"));
                            oSegment.set_DataElementValue(1, 0, "41");     //Entity Identifier Code
                            oSegment.set_DataElementValue(2, 0, oHeaderRow["NM102_SubmitterTypeQlfr"].ToString());     //Entity Type Qualifier
                            oSegment.set_DataElementValue(3, 0, oHeaderRow["NM103_SubmitterLastOrOrganizationName"].ToString());     //Name Last or Organization Name
                            oSegment.set_DataElementValue(4, 0, oHeaderRow["NM104_SubmitterFirst"].ToString()); 		// Name First (1036) 
                            oSegment.set_DataElementValue(5, 0, oHeaderRow["NM105_SubmitterMiddle"].ToString()); 		// Name Middle (1037) 
                            oSegment.set_DataElementValue(8, 0, oHeaderRow["NM108_SubmitterIdCodeQlfr"].ToString());     //Identification Code Qualifier
                            oSegment.set_DataElementValue(9, 0, oHeaderRow["NM109_SubmitterIdCode"].ToString());     //Identification Code

                            //PER - SUBMITTER EDI CONTACT INFO
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("NM1\\PER"));
                            nElemPos = 3;
                            if (oHeaderRow["PER0X_SubmitterPhoneNo"].ToString().Trim() != "")
                            {
                                oSegment.set_DataElementValue(nElemPos, 0, "TE");     //Communication Number Qualifier
                                oSegment.set_DataElementValue(nElemPos + 1, 0, oHeaderRow["PER0X_SubmitterPhoneNo"].ToString());     //Communication Number
                                nElemPos = nElemPos + 2;
                            }

                            if (oHeaderRow["PER0X_SubmitterPhoneExtNo"].ToString().Trim() != "")
                            {
                                oSegment.set_DataElementValue(nElemPos, 0, "EX");     //Communication Number Qualifier
                                oSegment.set_DataElementValue(nElemPos + 1, 0, oHeaderRow["PER0X_SubmitterPhoneExtNo"].ToString());     //Communication Number
                                nElemPos = nElemPos + 2;
                            }

                            if (oHeaderRow["PER0X_SubmitterFaxNo"].ToString().Trim() != "")
                            {
                                oSegment.set_DataElementValue(nElemPos, 0, "FX");     //Communication Number Qualifier
                                oSegment.set_DataElementValue(nElemPos + 1, 0, oHeaderRow["PER0X_SubmitterFaxNo"].ToString());     //Communication Number
                                nElemPos = nElemPos + 2;
                            }

                            if (oHeaderRow["PER0X_SubmitterEmail"].ToString().Trim() != "")
                            {
                                oSegment.set_DataElementValue(nElemPos, 0, "EM");     //Communication Number Qualifier
                                oSegment.set_DataElementValue(nElemPos + 1, 0, oHeaderRow["PER0X_SubmitterEmail"].ToString());     //Communication Number
                                nElemPos = nElemPos + 2;
                            }
                            if (nElemPos > 3)
                            {
                                oSegment.set_DataElementValue(1, 0, "IC");     //Contact Function Code
                                oSegment.set_DataElementValue(2, 0, oHeaderRow["PER02_SubmitterContactName"].ToString());     //Name
                            }

                        }//1000A SUBMITTER
                        
                        
                        //1000B RECEIVER
                        if (true)
                        {
                            //NM1 - RECEIVER
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("NM1\\NM1"));
                            oSegment.set_DataElementValue(1, 0, "40");     //Entity Identifier Code
                            oSegment.set_DataElementValue(2, 0, oHeaderRow["NM102_ReceiverTypeQlfr"].ToString());     //Entity Type Qualifier
                            oSegment.set_DataElementValue(3, 0, oHeaderRow["NM103_ReceiverLastOrOrganizationName"].ToString());     //Name Last or Organization Name
                            oSegment.set_DataElementValue(8, 0, oHeaderRow["NM108_ReceiverIdCodeQlfr"].ToString());     //Identification Code Qualifier
                            oSegment.set_DataElementValue(9, 0, oHeaderRow["NM109_ReceiverIdCode"].ToString());     //Identification Code

                        }//1000B RECEIVER
                        

                        nHlCounter = 0;
                        sSql = "select * from [837X222_InfoSource] where Headerkey = " + oHeaderRow["Headerkey"].ToString();
                        SqlDataAdapter oInfoSourceAdapter = new SqlDataAdapter(sSql, oConnection);
                        DataSet oInfoSourceDs = new DataSet("dsInfoSource");
                        oInfoSourceAdapter.Fill(oInfoSourceDs, "dsInfoSource");

                        //2000A INFORMATION SOURCE
                        foreach (DataRow oInfoSourceRow in oInfoSourceDs.Tables["dsInfoSource"].Rows)
                        {
                            nHlCounter = nHlCounter + 1;         //increment HL loop 
                            nHlSubscriberParent = nHlCounter;    //The value of this HL counter is the HL parent for the HL subscriber loop

                            //HL - BILLING PROVIDER
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\HL"));
                            oSegment.set_DataElementValue(1, 0, nHlCounter.ToString());     //Hierarchical ID Number
                            oSegment.set_DataElementValue(3, 0, "20");     //Hierarchical Level Code
                            oSegment.set_DataElementValue(4, 0, "1");     //Hierarchical Child Code

                            //PRV - BILLING PROVIDER SPECIALTY INFORMATION
                            if (oInfoSourceRow["PRV03_BillingProviderIdCode"].ToString().Trim() != "")
                            {
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\PRV"));
                                oSegment.set_DataElementValue(1, 0, "BI");     //Provider Code
                                oSegment.set_DataElementValue(2, 0, "PXC");     //Reference Identification Qualifier
                                oSegment.set_DataElementValue(3, 0, oInfoSourceRow["PRV03_BillingProviderIdCode"].ToString());     //Reference Identification
                            }
                            
                            // Currency (CUR) 
                            if (oInfoSourceRow["CUR02_CurrencyCode"].ToString().Trim() != "")
                            {
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CUR"));
                                oSegment.set_DataElementValue(1, 0, "85"); 		// Entity Identifier Code (98) 
                                oSegment.set_DataElementValue(2, 0, oInfoSourceRow["CUR02_CurrencyCode"].ToString()); 		// Currency Code (100) 
                            }
                            

                            //2010AA BILLING PROVIDER
                            if (true)
                            {
                                //NM1 - BILLING PROVIDER NAME
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\NM1"));
                                oSegment.set_DataElementValue(1, 0, "85");     //Entity Identifier Code
                                oSegment.set_DataElementValue(2, 0, oInfoSourceRow["NM102_BillingProviderTypeQualifier"].ToString());     //Entity Type Qualifier
                                oSegment.set_DataElementValue(3, 0, oInfoSourceRow["NM103_BillingProviderLastOrOrganizationName"].ToString());     //Name Last or Organization Name
                                oSegment.set_DataElementValue(4, 0, oInfoSourceRow["NM104_BillingProviderFirst"].ToString()); 		// Name First (1036) 
                                oSegment.set_DataElementValue(5, 0, oInfoSourceRow["NM105_BillingProviderMiddle"].ToString()); 		// Name Middle (1037) 
                                oSegment.set_DataElementValue(8, 0, "XX");     //Identification Code Qualifier
                                oSegment.set_DataElementValue(9, 0, oInfoSourceRow["NM109_BillingProviderIdCode"].ToString());     //Identification Code

                                //N3 - BILLING PROVIDER ADDRESS INFORMATION
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\N3"));
                                oSegment.set_DataElementValue(1, 0, oInfoSourceRow["N301_BillingProviderAddr1"].ToString());     //Address Information
                                oSegment.set_DataElementValue(2, 0, oInfoSourceRow["N302_BillingProviderAddr2"].ToString());     //Address Information

                                //N4 - BILLING PROVIDER LOCATION
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\N4"));
                                oSegment.set_DataElementValue(1, 0, oInfoSourceRow["N401_BillingProviderCity"].ToString());     //City Name
                                oSegment.set_DataElementValue(2, 0, oInfoSourceRow["N402_BillingProviderState"].ToString());     //State or Province Code
                                oSegment.set_DataElementValue(3, 0, oInfoSourceRow["N403_BillingProviderZip"].ToString());     //Postal Code
                                oSegment.set_DataElementValue(4, 0, oInfoSourceRow["N404_BillingProviderCountry"].ToString()); 		// Country Code (26) 
                                oSegment.set_DataElementValue(7, 0, oInfoSourceRow["N407_BillingProviderCountrySubdivision"].ToString()); 		// Country Subdivision Code (1715) 

                                //REF - EMPLOYER'S IDENTIFICATION NUMBER
                                if (oInfoSourceRow["REF02_BillingProviderEmployerId"].ToString().Trim() != "")
                                {
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                    oSegment.set_DataElementValue(1, 0, "EI");     //Reference Identification Qualifier
                                    oSegment.set_DataElementValue(2, 0, oInfoSourceRow["REF02_BillingProviderEmployerId"].ToString());     //Reference Identification
                                }

                                //REF - SOCIAL SECURITY NUMBER
                                if (oInfoSourceRow["REF02_BillingProviderSocialSecurityNo"].ToString().Trim() != "")
                                {
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                    oSegment.set_DataElementValue(1, 0, "SY");     //Reference Identification Qualifier
                                    oSegment.set_DataElementValue(2, 0, oInfoSourceRow["REF02_BillingProviderSocialSecurityNo"].ToString());     //Reference Identification
                                }

                                //REF - STATE LICENSE NUMBER
                                if (oInfoSourceRow["REF02_BillingProviderStateLicenseNo"].ToString().Trim() != "")
                                {
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                    oSegment.set_DataElementValue(1, 0, "0B");     //Reference Identification Qualifier
                                    oSegment.set_DataElementValue(2, 0, oInfoSourceRow["REF02_BillingProviderStateLicenseNo"].ToString());     //Reference Identification
                                }

                                //REF - PROVIDER UPIN NUMBER
                                if (oInfoSourceRow["REF02_BillingProviderProviderUPIN"].ToString().Trim() != "")
                                {
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                    oSegment.set_DataElementValue(1, 0, "1G");     //Reference Identification Qualifier
                                    oSegment.set_DataElementValue(2, 0, oInfoSourceRow["REF02_BillingProviderProviderUPIN"].ToString());     //Reference Identification
                                }

                                //PER - Billing Provider Contact Information
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\PER"));
                                nElemPos = 3;
                                if (oInfoSourceRow["PER0X_BillingProviderPhoneNo"].ToString().Trim() != "")
                                {
                                    oSegment.set_DataElementValue(nElemPos, 0, "TE");     //Communication Number Qualifier
                                    oSegment.set_DataElementValue(nElemPos + 1, 0, oInfoSourceRow["PER0X_BillingProviderPhoneNo"].ToString());     //Communication Number
                                    nElemPos = nElemPos + 2;
                                }

                                if (oInfoSourceRow["PER0X_BillingProviderPhoneExtNo"].ToString().Trim() != "")
                                {
                                    oSegment.set_DataElementValue(nElemPos, 0, "EX");     //Communication Number Qualifier
                                    oSegment.set_DataElementValue(nElemPos + 1, 0, oInfoSourceRow["PER0X_BillingProviderPhoneExtNo"].ToString());     //Communication Number
                                    nElemPos = nElemPos + 2;
                                }

                                if (oInfoSourceRow["PER0X_BillingProviderFaxNo"].ToString().Trim() != "")
                                {
                                    oSegment.set_DataElementValue(nElemPos, 0, "FX");     //Communication Number Qualifier
                                    oSegment.set_DataElementValue(nElemPos + 1, 0, oInfoSourceRow["PER0X_BillingProviderFaxNo"].ToString());     //Communication Number
                                    nElemPos = nElemPos + 2;
                                }

                                if (oInfoSourceRow["PER0X_BillingProviderEmail"].ToString().Trim() != "")
                                {
                                    oSegment.set_DataElementValue(nElemPos, 0, "EM");     //Communication Number Qualifier
                                    oSegment.set_DataElementValue(nElemPos + 1, 0, oInfoSourceRow["PER0X_BillingProviderEmail"].ToString());     //Communication Number
                                    nElemPos = nElemPos + 2;
                                }
                                if (nElemPos > 3)
                                {
                                    oSegment.set_DataElementValue(1, 0, "IC");     //Contact Function Code
                                    oSegment.set_DataElementValue(2, 0, oInfoSourceRow["PER02_BillingProviderContactName"].ToString());     //Name
                                }

                            }//2010AA BILLING PROVIDER
                            

                            //2010AB PAY-TO PROVIDER
                            if (oInfoSourceRow["N301_PayToProviderAddr1"].ToString().Trim() != "")
                            {
                                //NM1 - PAY-TO PROVIDER NAME
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\NM1"));
                                oSegment.set_DataElementValue(1, 0, "87");     //Entity Identifier Code
                                oSegment.set_DataElementValue(2, 0, oInfoSourceRow["NM102_PayToProviderTypeQlfr"].ToString());     //Entity Type Qualifier
                                //oSegment.set_DataElementValue(3, 0, oInfoSourceRow["NM103_PayToProviderLastOrOrganizatioName"].ToString());     //Name Last or Organization Name

                                //N3 - PAY-TO PROVIDER ADDRESS INFORMATION
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\N3"));
                                oSegment.set_DataElementValue(1, 0, oInfoSourceRow["N301_PayToProviderAddr1"].ToString());     //Address Information
                                oSegment.set_DataElementValue(2, 0, oInfoSourceRow["N302_PayToProviderAddr2"].ToString());     //Address Information

                                //N4 - PAY-TO PROVIDER LOCATION
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\N4"));
                                oSegment.set_DataElementValue(1, 0, oInfoSourceRow["N401_PayToProviderCity"].ToString());     //City Name
                                oSegment.set_DataElementValue(2, 0, oInfoSourceRow["N402_PayToProviderState"].ToString());     //State or Province Code
                                oSegment.set_DataElementValue(3, 0, oInfoSourceRow["N403_PayToProviderZip"].ToString());     //Postal Code
                                oSegment.set_DataElementValue(4, 0, oInfoSourceRow["N404_PayToProviderCountry"].ToString()); 	// Country Code (26) 
                                oSegment.set_DataElementValue(7, 0, oInfoSourceRow["N407_PayToProviderCountrySubdivision"].ToString()); 	// Country Subdivision Code (1715) 

                            }//2010AB PAY-TO PROVIDER


                            //2010AC PAY-TO PLAN NAME
                            if (oInfoSourceRow["NM103_PayeeLastOrOrganizationName"].ToString().Trim() != "")
                            {
                                //NM1 - Pay-To Plan Name
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\NM1"));
                                oSegment.set_DataElementValue(1, 0, "PE");     //Entity Identifier Code
                                oSegment.set_DataElementValue(2, 0, "2");     //Entity Type Qualifier
                                oSegment.set_DataElementValue(3, 0, oInfoSourceRow["NM103_PayeeLastOrOrganizationName"].ToString());     //Name Last or Organization Name
                                oSegment.set_DataElementValue(8, 0, oInfoSourceRow["NM108_PayeeIdCodeQlfr"].ToString());     //Identification Code Qualifier
                                oSegment.set_DataElementValue(9, 0, oInfoSourceRow["NM109_PayeeIdCode"].ToString());     //Identification Code

                                //N3 - Pay-to Plan Address
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\N3"));
                                oSegment.set_DataElementValue(1, 0, oInfoSourceRow["N301_PayeeAddr1"].ToString());     //Address Information
                                oSegment.set_DataElementValue(2, 0, oInfoSourceRow["N302_PayeeAddr2"].ToString());     //Address Information

                                //N4 - Pay-To Plan City, State, ZIP Code
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\N4"));
                                oSegment.set_DataElementValue(1, 0, oInfoSourceRow["N401_PayeeCity"].ToString());     //City Name
                                oSegment.set_DataElementValue(2, 0, oInfoSourceRow["N402_PayeeState"].ToString());     //State or Province Code
                                oSegment.set_DataElementValue(3, 0, oInfoSourceRow["N403_PayeeZip"].ToString());     //Postal Code
                                oSegment.set_DataElementValue(4, 0, oInfoSourceRow["N404_PayeeCountry"].ToString()); 	// Country Code (26) 
                                oSegment.set_DataElementValue(7, 0, oInfoSourceRow["N407_PayeeCountrySubdivision"].ToString()); 	// Country Subdivision Code (1715) 

                                //REF - Pay-to Plan Secondary Identification - PAYER IDENTIFICATION NUMBER"
                                if (oInfoSourceRow["REF02_PayeePayerId"].ToString().Trim() != "")
                                {
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                    oSegment.set_DataElementValue(1, 0, "2U");     //Reference Identification Qualifier
                                    oSegment.set_DataElementValue(2, 0, oInfoSourceRow["REF02_PayeePayerId"].ToString());     //Reference Identification
                                }

                                //REF - Pay-to Plan Secondary Identification - CLAIM OFFICE NUMBER
                                if (oInfoSourceRow["REF02_PayeeClaimOfficeNo"].ToString().Trim() != "")
                                {
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                    oSegment.set_DataElementValue(1, 0, "FY");     //Reference Identification Qualifier
                                    oSegment.set_DataElementValue(2, 0, oInfoSourceRow["REF02_PayeeClaimOfficeNo"].ToString());     //Reference Identification                              
                                }

                                //REF - Pay-to Plan Secondary Identification - NATIONAL ASSOCIATION OF INSURANCE COMMISSIONERS (NAIC) CODE
                                if (oInfoSourceRow["REF02_PayeeNAIC_Code"].ToString().Trim() != "")
                                {
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                    oSegment.set_DataElementValue(1, 0, "NF");     //Reference Identification Qualifier
                                    oSegment.set_DataElementValue(2, 0, oInfoSourceRow["REF02_PayeeNAIC_Code"].ToString());     //Reference Identification
                                }

                                //REF - Pay-To Plan Tax Identification Number - NATIONAL ASSOCIATION OF INSURANCE COMMISSIONERS (NAIC) CODE
                                if (oInfoSourceRow["REF02_PayeeEmployerId"].ToString().Trim() != "")
                                {
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                    oSegment.set_DataElementValue(1, 0, "EI");     //Reference Identification Qualifier
                                    oSegment.set_DataElementValue(2, 0, oInfoSourceRow["REF02_PayeeEmployerId"].ToString());     //Reference Identification
                                }

                            }//2010AC PAY-TO PLAN NAME

                            
                            //2000B SUBSCRIBER HL LOOP
                            sSql = "select * from [837X222_Subscriber] where InfoSourcekey = " + oInfoSourceRow["InfoSourcekey"].ToString();
                            SqlDataAdapter oSubscriberAdapter = new SqlDataAdapter(sSql, oConnection);
                            DataSet oSubscriberDs = new DataSet("dsSubscriber");
                            oSubscriberAdapter.Fill(oSubscriberDs, "dsSubscriber");

                            foreach (DataRow oSubscriberRow in oSubscriberDs.Tables["dsSubscriber"].Rows)
                            {
                                nHlCounter = nHlCounter + 1;
                                nHlDependentParent = nHlCounter;

                                if (oSubscriberRow["SBR02_IndividualRelationshipCode"].ToString() == "18")
                                    bDependent = false;
                                else
                                    bDependent = true;

                                //HL - SUBSCRIBER LEVEL
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\HL"));
                                oSegment.set_DataElementValue(1, 0, nHlCounter.ToString());     //Hierarchical ID Number
                                oSegment.set_DataElementValue(2, 0, nHlSubscriberParent.ToString());     //Hierarchical Parent ID Number
                                oSegment.set_DataElementValue(3, 0, "22");     //Hierarchical Level Code

                                if (bDependent) 
                                {
                                    oSegment.set_DataElementValue(4, 0, "1");  //Hierarchical Child Code
                                }
                                else //self
                                {
                                    oSegment.set_DataElementValue(4, 0, "0");  //Hierarchical Child Code
                                }

                                //SBR - SUBSCRIBER INFORMATION
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\SBR"));
                                oSegment.set_DataElementValue(1, 0, oSubscriberRow["SBR01_PayerResponsibilitySequenceNumberCode"].ToString()); 		// Payer Responsibility Sequence Number Code (1138) 
                                oSegment.set_DataElementValue(2, 0, oSubscriberRow["SBR02_IndividualRelationshipCode"].ToString()); 		// Individual Relationship Code (1069) 
                                oSegment.set_DataElementValue(3, 0, oSubscriberRow["SBR03_SubscriberGroup_PolicyNo"].ToString()); 		// Reference Identification (127) 
                                oSegment.set_DataElementValue(4, 0, oSubscriberRow["SBR04_SubscriberGroupName"].ToString()); 		// Name (93) 
                                oSegment.set_DataElementValue(5, 0, oSubscriberRow["SBR05_InsuranceTypeCode"].ToString()); 		// Insurance Type Code (1336) 
                                oSegment.set_DataElementValue(9, 0, oSubscriberRow["SBR09_ClaimFilingIndicatorCode"].ToString()); 		// Claim Filing Indicator Code (1032) 

                                //PAT - Patient Information
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\PAT"));
                                if (oSubscriberRow["PAT06_PatientDeathDate"].ToString().Trim() != "")
                                {
                                    oSegment.set_DataElementValue(5, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                                    oSegment.set_DataElementValue(6, 0, oSubscriberRow["PAT06_PatientDeathDate"].ToString()); 		// Date Time Period (1251) 
                                }
                                if (oSubscriberRow["PAT08_PatientWeightPounds"].ToString().Trim() != "")
                                {
                                    oSegment.set_DataElementValue(7, 0, "01"); 		// Unit or Basis for Measurement Code (355) 
                                    oSegment.set_DataElementValue(8, 0, oSubscriberRow["PAT08_PatientWeightPounds"].ToString()); 		// Weight (81) 
                                }
                                oSegment.set_DataElementValue(9, 0, oSubscriberRow["PAT09_Pregnant"].ToString()); 		// Yes/No Condition or Response Code (1073) 

                                //2010BA SUBSCRIBER
                                if (oSubscriberRow["NM103_SubscriberLastOrOrganizationName"].ToString().Trim() != "")
                                {
                                    //NM1 - SUBSCRIBER NAME
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\NM1"));
                                    oSegment.set_DataElementValue(1, 0, "IL");     //Entity Identifier Code
                                    oSegment.set_DataElementValue(2, 0, oSubscriberRow["NM102_SubscriberTypeQualifier"].ToString());     //Entity Type Qualifier
                                    oSegment.set_DataElementValue(3, 0, oSubscriberRow["NM103_SubscriberLastOrOrganizationName"].ToString());     //Name Last or Organization Name
                                    oSegment.set_DataElementValue(4, 0, oSubscriberRow["NM104_SubscriberFirst"].ToString());     //Name First
                                    oSegment.set_DataElementValue(5, 0, oSubscriberRow["NM105_SubscriberMiddle"].ToString()); 		// Name Middle (1037) 
                                    oSegment.set_DataElementValue(7, 0, oSubscriberRow["NM107_SubscriberSuffix"].ToString()); 		// Name Suffix (1039) 
                                    oSegment.set_DataElementValue(8, 0, oSubscriberRow["NM108_SubscriberIdCodeQlfr"].ToString());     //Identification Code Qualifier
                                    oSegment.set_DataElementValue(9, 0, oSubscriberRow["NM109_SubscriberIdCode"].ToString());     //Identification Code

                                    //N3 - Subscriber Address
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\N3"));
                                    oSegment.set_DataElementValue(1, 0, oSubscriberRow["N301_SubscriberAddr1"].ToString()); 		// Address Information (166) 
                                    oSegment.set_DataElementValue(2, 0, oSubscriberRow["N302_SubscriberAddr2"].ToString()); 		// Address Information (166) 

                                    //N4 - Subscriber City, State, ZIP Code  
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\N4"));
                                    oSegment.set_DataElementValue(1, 0, oSubscriberRow["N401_SubscriberCity"].ToString()); 		// City Name (19) 
                                    oSegment.set_DataElementValue(2, 0, oSubscriberRow["N402_SubscriberState"].ToString()); 		// State or Province Code (156) 
                                    oSegment.set_DataElementValue(3, 0, oSubscriberRow["N403_SubscriberZip"].ToString()); 		// Postal Code (116) 
                                    oSegment.set_DataElementValue(4, 0, oSubscriberRow["N404_SubscriberCountry"].ToString()); 		// Country Code (26)
                                    oSegment.set_DataElementValue(7, 0, oSubscriberRow["N407_SubscriberCountrySubdivision"].ToString()); 		// Country Subdivision Code (1715) 

                                    //DMG - SUBSCRIBER DEMOGRAPHIC INFORMATION
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\DMG"));
                                    if (oSubscriberRow["DMG02_SubscriberBirthDate"].ToString().Trim() != "")
                                    {
                                        oSegment.set_DataElementValue(1, 0, "D8");     //Date Time Period Format Qualifier
                                        oSegment.set_DataElementValue(2, 0, oSubscriberRow["DMG02_SubscriberBirthDate"].ToString());     //Date Time Period
                                    }
                                    oSegment.set_DataElementValue(3, 0, oSubscriberRow["DMG03_SubscriberGenderCode"].ToString());     //Gender Code


                                    //REF - Subscriber Secondary Identification - SOCIAL SECURITY NUMBER
                                    if (oSubscriberRow["REF02_SubscriberSocialSecurityNo"].ToString().Trim() != "")
                                    {
                                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                        oSegment.set_DataElementValue(1, 0, "SY"); 		// Reference Identification Qualifier (128) 
                                        oSegment.set_DataElementValue(2, 0, oSubscriberRow["REF02_SubscriberSocialSecurityNo"].ToString()); 		// Reference Identification (127
                                        
                                    }

                                    //REF - Property and Casualty Claim Number - AGENCY CLAIM NUMBER
                                    if (oSubscriberRow["REF02_PropertyCasualtyClaimNo"].ToString().Trim() != "")
                                    {
                                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                        oSegment.set_DataElementValue(1, 0, "Y4"); 		// Reference Identification Qualifier (128) 
                                        oSegment.set_DataElementValue(2, 0, oSubscriberRow["REF02_PropertyCasualtyClaimNo"].ToString()); 		// Reference Identification (127
                                        
                                    }

                                    //PER - Property and Casualty Subscriber Contact Information 
                                    if (oSubscriberRow["PER04_SubscriberPhoneNo"].ToString().Trim() != "")
                                    {
                                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\PER"));
                                        oSegment.set_DataElementValue(1, 0, "IC"); 		// Contact Function Code (366) 
                                        oSegment.set_DataElementValue(2, 0, oSubscriberRow["PER02_SubscriberContactName"].ToString()); 		// Name (93) 
                                        oSegment.set_DataElementValue(3, 0, "TE"); 		// Communication Number Qualifier (365) 
                                        oSegment.set_DataElementValue(4, 0, oSubscriberRow["PER04_SubscriberPhoneNo"].ToString()); 		// Communication Number (364) 
                                        if (oSubscriberRow["PER06_SubscriberPhoneExtNo"].ToString().Trim() != "")
                                        {
                                            oSegment.set_DataElementValue(5, 0, "EX"); 		// Communication Number Qualifier (365) 
                                            oSegment.set_DataElementValue(6, 0, oSubscriberRow["PER06_SubscriberPhoneExtNo"].ToString()); 		// Communication Number (364) 
                                        
                                        }//if

                                    }//PER

                                }//2010BA SUBSCRIBER
                                
                                
                                //2010BB PAYER
                                if (oSubscriberRow["NM103_PayerLastOrOrganizatioName"].ToString().Trim() != "")
                                {
                                    //NM1 - PAYER NAME
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\NM1"));
                                    oSegment.set_DataElementValue(1, 0, "PR");     //Entity Identifier Code
                                    oSegment.set_DataElementValue(2, 0, oSubscriberRow["NM102_PayerTypeQlfr"].ToString());     //Entity Type Qualifier
                                    oSegment.set_DataElementValue(3, 0, oSubscriberRow["NM103_PayerLastOrOrganizatioName"].ToString());     //Name Last or Organization Name
                                    oSegment.set_DataElementValue(8, 0, "PI");     //Identification Code Qualifier
                                    oSegment.set_DataElementValue(9, 0, oSubscriberRow["NM109_PayerIdCode"].ToString());     //Identification Code

                                    //N3 - Payer Address
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\N3"));
                                    oSegment.set_DataElementValue(1, 0, oSubscriberRow["N301_PayerAddr1"].ToString()); 		// Address Information (166) 
                                    oSegment.set_DataElementValue(2, 0, oSubscriberRow["N302_PayerAddr2"].ToString()); 		// Address Information (166) 

                                    //N4 - Payer City, State, ZIP Code  
                                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\N4"));
                                    oSegment.set_DataElementValue(1, 0, oSubscriberRow["N401_PayerCity"].ToString()); 		// City Name (19) 
                                    oSegment.set_DataElementValue(2, 0, oSubscriberRow["N402_PayerState"].ToString()); 		// State or Province Code (156) 
                                    oSegment.set_DataElementValue(3, 0, oSubscriberRow["N403_PayerZip"].ToString()); 		// Postal Code (116) 
                                    oSegment.set_DataElementValue(4, 0, oSubscriberRow["N404_PayerCountry"].ToString()); 		// Country Code (26)
                                    oSegment.set_DataElementValue(7, 0, oSubscriberRow["N407_PayerCountrySubdivision"].ToString()); 		// Country Subdivision Code (1715) 


                                    //REF - Subscriber Secondary Identification - PAYER IDENTIFICATION NUMBER
                                    if (oSubscriberRow["REF02_PayerId"].ToString().Trim() != "")
                                    {
                                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                        oSegment.set_DataElementValue(1, 0, "2U");     //Reference Identification Qualifier
                                        oSegment.set_DataElementValue(2, 0, oSubscriberRow["REF02_PayerId"].ToString());     //Reference Identification
                                        
                                    }

                                    //REF - Subscriber Secondary Identification  - EMPLOYER'S IDENTIFICATION NUMBER
                                    if (oSubscriberRow["REF02_EmployerId"].ToString().Trim() != "")
                                    {
                                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                        oSegment.set_DataElementValue(1, 0, "EI");     //Reference Identification Qualifier
                                        oSegment.set_DataElementValue(2, 0, oSubscriberRow["REF02_EmployerId"].ToString());     //Reference Identification
                                        
                                    }

                                    //REF - Subscriber Secondary Identification  - CLAIM OFFICE NUMBER
                                    if (oSubscriberRow["REF02_ClaimOfficeNo"].ToString().Trim() != "")
                                    {
                                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                        oSegment.set_DataElementValue(1, 0, "FY");     //Reference Identification Qualifier
                                        oSegment.set_DataElementValue(2, 0, oSubscriberRow["REF02_ClaimOfficeNo"].ToString());     //Reference Identification
                                        
                                    }

                                    //REF - Subscriber Secondary Identification  - NATIONAL ASSOCIATION OF INSURANCE COMMISSIONERS (NAIC) CODE
                                    if (oSubscriberRow["REF02_NAIC_Code"].ToString().Trim() != "")
                                    {
                                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                        oSegment.set_DataElementValue(1, 0, "NF");     //Reference Identification Qualifier
                                        oSegment.set_DataElementValue(2, 0, oSubscriberRow["REF02_NAIC_Code"].ToString());     //Reference Identification
                                        
                                    }

                                    //REF - BILLING PROVIDER SECONDARY IDENTIFICATION - PROVIDER COMMERCIAL NUMBER
                                    if (oSubscriberRow["REF02_ProviderCommercialNo"].ToString().Trim() != "")
                                    {
                                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                        oSegment.set_DataElementValue(1, 0, "G2");     //Reference Identification Qualifier
                                        oSegment.set_DataElementValue(2, 0, oSubscriberRow["REF02_ProviderCommercialNo"].ToString());     //Reference Identification
                                        
                                    }

                                    //REF - BILLING PROVIDER SECONDARY IDENTIFICATION - LOCATION NUMBER
                                    if (oSubscriberRow["REF02_LocationNo"].ToString().Trim() != "")
                                    {
                                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                        oSegment.set_DataElementValue(1, 0, "LU");     //Reference Identification Qualifier
                                        oSegment.set_DataElementValue(2, 0, oSubscriberRow["REF02_LocationNo"].ToString());     //Reference Identification

                                    }//REF

                                }//2010BB PAYER
                                
                                
                                //The claims loop can occur in both the HL subscriber loop and HL patient (dependent) loop
                                if (!bDependent)    //self
                                {
                                    //Subscriber //2300 CLAIM INFORMATION
                                    Proc_2300_Claim(ref oTransactionSet, ref oConnection, "select * from [837X222_Claims] where Subscriberkey = " + oSubscriberRow["Subscriberkey"].ToString());
                                
                                }
                                else
                                {
                                    //2000C PATIENT HL LOOP
                                    sSql = "select * from [837X222_Dependent] where Subscriberkey = " + oSubscriberRow["Subscriberkey"].ToString();
                                    SqlDataAdapter oDependentAdapter = new SqlDataAdapter(sSql, oConnection);
                                    DataSet oDependentDs = new DataSet("dsDependent");
                                    oDependentAdapter.Fill(oDependentDs, "dsDependent");

                                    foreach (DataRow oDependentRow in oDependentDs.Tables["dsDependent"].Rows)
                                    {
                                        nHlCounter = nHlCounter + 1;

                                        //HL - PATIENT LEVEL
                                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\HL"));
                                        oSegment.set_DataElementValue(1, 0, nHlCounter.ToString());     //Hierarchical ID Number
                                        oSegment.set_DataElementValue(2, 0, nHlDependentParent.ToString());     //Hierarchical Parent ID Number
                                        oSegment.set_DataElementValue(3, 0, "23");     //Hierarchical Level Code
                                        oSegment.set_DataElementValue(4, 0, "0");     //Hierarchical Child Code

                                        //PAT - PATIENT INFORMATION
                                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\PAT"));
                                        oSegment.set_DataElementValue(1, 0, oDependentRow["PAT01_IndividualRelationshipCode"].ToString()); 		// Individual Relationship Code (1069) 
                                        if (oDependentRow["PAT06_PatientDeathDate"].ToString().Trim() != "")
                                        {
                                            oSegment.set_DataElementValue(5, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                                            oSegment.set_DataElementValue(6, 0, oDependentRow["PAT06_PatientDeathDate"].ToString()); 		// Date Time Period (1251) 
                                        }
                                        if (oDependentRow["PAT08_PatientWeightPounds"].ToString().Trim() != "")
                                        {
                                            oSegment.set_DataElementValue(7, 0, "01"); 		// Unit or Basis for Measurement Code (355) 
                                            oSegment.set_DataElementValue(8, 0, oDependentRow["PAT08_PatientWeightPounds"].ToString()); 		// Weight (81) 
                                        }
                                        oSegment.set_DataElementValue(9, 0, oDependentRow["PAT09_Pregnant"].ToString()); 		// Yes/No Condition or Response Code (1073) 

                                        //2010CA PATIENT
                                        if (oDependentRow["NM103_PatientLastOrOrganizationName"].ToString().Trim() != "")
                                        {
                                            //NM1 - PATIENT NAME
                                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\NM1"));
                                            oSegment.set_DataElementValue(1, 0, "QC");     //Entity Identifier Code
                                            oSegment.set_DataElementValue(2, 0, oDependentRow["NM102_PatientTypeQualifier"].ToString());     //Entity Type Qualifier
                                            oSegment.set_DataElementValue(3, 0, oDependentRow["NM103_PatientLastOrOrganizationName"].ToString());     //Name Last or Organization Name
                                            oSegment.set_DataElementValue(4, 0, oDependentRow["NM104_PatientFirst"].ToString());     //Name First
                                            oSegment.set_DataElementValue(5, 0, oDependentRow["NM105_PatientMiddle"].ToString()); 		// Name Middle (1037) 
                                            oSegment.set_DataElementValue(7, 0, oDependentRow["NM107_PatientSuffix"].ToString()); 		// Name Suffix (1039) 

                                            //N3 - PATIENT ADDRESS INFORMATION
                                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\N3"));
                                            oSegment.set_DataElementValue(1, 0, oDependentRow["N301_PatientAddr1"].ToString());     //Address Information
                                            oSegment.set_DataElementValue(2, 0, oDependentRow["N302_PatientAddr2"].ToString()); 		// Address Information (166) 

                                            //N4 - PATIENT LOCATION
                                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\N4"));
                                            oSegment.set_DataElementValue(1, 0, oDependentRow["N401_PatientCity"].ToString());     //City Name
                                            oSegment.set_DataElementValue(2, 0, oDependentRow["N402_PatientState"].ToString());     //State or Province Code
                                            oSegment.set_DataElementValue(3, 0, oDependentRow["N403_PatientZip"].ToString());     //Postal Code
                                            oSegment.set_DataElementValue(4, 0, oDependentRow["N404_PatientCountry"].ToString()); 		// Country Code (26) 
                                            oSegment.set_DataElementValue(7, 0, oDependentRow["N407_PatientCountrySubdivision"].ToString()); 		// Country Subdivision Code (1715) 

                                            //DMG - PATIENT DEMOGRAPHIC INFORMATION
                                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\DMG"));
                                            if (oDependentRow["DMG02_PatientBirthDate"].ToString().Trim() != "")
                                            {
                                                oSegment.set_DataElementValue(1, 0, "D8");     //Date Time Period Format Qualifier
                                                oSegment.set_DataElementValue(2, 0, oDependentRow["DMG02_PatientBirthDate"].ToString());     //Date Time Period
                                            }
                                            oSegment.set_DataElementValue(3, 0, oDependentRow["DMG03_PatientGenderCode"].ToString());     //Gender Code

                                            //REF - Property and Casualty Claim Number - "AGENCY CLAIM NUMBER"
                                            if (oDependentRow["REF02_PropertyCasualtyClaimNo"].ToString().Trim() != "")
                                            {
                                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                                oSegment.set_DataElementValue(1, 0, "Y4"); 		// Reference Identification Qualifier (128) 
                                                oSegment.set_DataElementValue(2, 0, oDependentRow["REF02_PropertyCasualtyClaimNo"].ToString()); 		// Reference Identification (127) 
                                            }

                                            //REF - Property and Casualty Patient Identifier - SOCIAL SECURITY NUMBER
                                            if (oDependentRow["REF02_PatientSocialSecurityNo"].ToString().Trim() != "")
                                            {
                                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                                oSegment.set_DataElementValue(1, 0, "SY"); 		// Reference Identification Qualifier (128) 
                                                oSegment.set_DataElementValue(2, 0, oDependentRow["REF02_PatientSocialSecurityNo"].ToString()); 		// Reference Identification (127) 
                                            }

                                            //REF - Property and Casualty Patient Identifier - MEMBER IDENTIFICATION NUMBER
                                            if (oDependentRow["REF02_MemberIdNo"].ToString().Trim() != "")
                                            {
                                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\REF"));
                                                oSegment.set_DataElementValue(1, 0, "1W"); 		// Reference Identification Qualifier (128) 
                                                oSegment.set_DataElementValue(2, 0, oDependentRow["REF02_MemberIdNo"].ToString()); 		// Reference Identification (127) 
                                            }

                                            //PER - Property and Casualty Patient Contact Information  
                                            if (oDependentRow["PER04_PatientPhoneNo"].ToString().Trim() != "")
                                            {
                                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\NM1\\PER"));
                                                oSegment.set_DataElementValue(1, 0, "IC"); 		// Contact Function Code (366) 
                                                oSegment.set_DataElementValue(2, 0, oDependentRow["PER02_PatientContactName"].ToString()); 		// Name (93) 
                                                oSegment.set_DataElementValue(3, 0, "TE"); 		// Communication Number Qualifier (365) 
                                                oSegment.set_DataElementValue(4, 0, oDependentRow["PER04_PatientPhoneNo"].ToString()); 		// Communication Number (364) 
                                                if (oDependentRow["PER06_PatientPhoneExtNo"].ToString().Trim() != "")
                                                {
                                                    oSegment.set_DataElementValue(5, 0, "EX"); 		// Communication Number Qualifier (365) 
                                                    oSegment.set_DataElementValue(6, 0, oDependentRow["PER06_PatientPhoneExtNo"].ToString()); 		// Communication Number (364) 
                                                
                                                }

                                            }//PER

                                        }//2010CA PATIENT
                                        
                                        //Patient//s claims
                                        Proc_2300_Claim(ref oTransactionSet, ref oConnection, "select * from [837X222_Claims] where Dependentkey = " + oDependentRow["Dependentkey"].ToString());

                                    }//foreach oDependentRow

                                }//if (!bDependent) 

                            }//foreach oSubscriberRow

                        }//foreach oInfoSourceRow
                        
                    }//foreach oHeaderRow

                }//foreach oGroupRow 
                
            }//foreach oInterchangeRow

           
            //TRAILING SEGMENTS ARE AUTOMATICALLY CREATED WHEN FREDI COMMITS (SAVES)
            //THE EDIDOC OBJECT INTO AN EDI FILE.
            oEdiDoc.Save(sAppPath + sEdiFile);

            MessageBox.Show(oEdiDoc.GetEdiString());

            MessageBox.Show("Done");

            //DESTROYS OBJECTS
            //oTransactionSet.Dispose();
            oGroup.Dispose();
            oInterchange.Dispose();
            oSchemas.Dispose();
            oEdiDoc.Dispose();
 
        } // btngenerate





        private void Proc_2300_Claim(ref ediTransactionSet oTransactionSet, ref SqlConnection oConnection, string sSql)
        {
            ediDataSegment oSegment=null;
            int nElemPos = 0;

            //2300 CLAIM
            SqlDataAdapter oClaimsAdapter = new SqlDataAdapter(sSql, oConnection);
            DataSet oClaimsDs = new DataSet("dsClaims");
            oClaimsAdapter.Fill(oClaimsDs, "dsClaims");

            foreach (DataRow oClaimsRow in oClaimsDs.Tables["dsClaims"].Rows)
            {
                //CLM - HEALTH CLAIM
                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\CLM"));
                oSegment.set_DataElementValue(1, 0, oClaimsRow["CLM01_PatientControlNo"].ToString());      //Claim Submitter//s Identifier
                oSegment.set_DataElementValue(2, 0, oClaimsRow["CLM02_TotalClaimChargeAmount"].ToString());      //Monetary Amount
                oSegment.set_DataElementValue(5, 1, 0, oClaimsRow["CLM05_01_PlaceOfServiceCode"].ToString());     //Facility Code Value
                oSegment.set_DataElementValue(5, 2, 0, "B");     //Facility Code Qualifier
                oSegment.set_DataElementValue(5, 3, 0, oClaimsRow["CLM05_03_ClaimFrequencyCode"].ToString());     //Claim Frequency Type Code
                oSegment.set_DataElementValue(6, 0, oClaimsRow["CLM06_SupplierSignatureIndicator"].ToString());      //Yes/No Condition or Response Code
                oSegment.set_DataElementValue(7, 0, oClaimsRow["CLM07_PlanParticipationCode"].ToString());      //Provider Accept Assignment Code
                oSegment.set_DataElementValue(8, 0, oClaimsRow["CLM08_BenefitsAssignmentCertIndicator"].ToString());      //Yes/No Condition or Response Code
                oSegment.set_DataElementValue(9, 0, oClaimsRow["CLM09_ReleaseOfInformationCode"].ToString());      //Release of Information Code
                oSegment.set_DataElementValue(10, 0, oClaimsRow["CLM10_PatientSignatureSourceCode"].ToString()); 		// Patient Signature Source Code (1351) 
                oSegment.set_DataElementValue(11, 1, oClaimsRow["CLM11_01_RelatedCausesCode"].ToString()); 		// Related-Causes Code (1362) 
                oSegment.set_DataElementValue(11, 2, oClaimsRow["CLM11_02_RelatedCausesCode"].ToString()); 		// Related-Causes Code (1362) 
                oSegment.set_DataElementValue(11, 4, oClaimsRow["CLM11_04_AutoAccidentStateCode"].ToString()); 		// State or Province Code (156) 
                oSegment.set_DataElementValue(11, 5, oClaimsRow["CLM11_05_CountryCode"].ToString()); 		// Country Code (26) 
                oSegment.set_DataElementValue(12, 0, oClaimsRow["CLM112_SpecialProgramCode"].ToString()); 		// Special Program Code (1366) 
                oSegment.set_DataElementValue(20, 0, oClaimsRow["CLM120_DelayReasonCode"].ToString()); 		// Delay Reason Code (1514) 

                //DTP - Date - Onset of Current Illness or Symptom
                if (oClaimsRow["DTP03_OnsetofCurrentIllnessInjuryDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "431"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_OnsetofCurrentIllnessInjuryDate"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP -  Date -Initial Treatment Date 
                if (oClaimsRow["DTP03_InitialTreatmentDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "454"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_InitialTreatmentDate"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Last Seen Date 
                if (oClaimsRow["DTP03_LastSeenDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "304"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_LastSeenDate"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Acute Manifestation 
                if (oClaimsRow["DTP03_AcuteManifestationDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "453"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_AcuteManifestationDate"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Accident 
                if (oClaimsRow["DTP03_AccidentDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "439"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_AccidentDate"].ToString()); 		// Date Time Period (1251) 
                    
                }

                //DTP - Date - Last Menstrual Period 
                if (oClaimsRow["DTP03_LastMenstrualPeriodDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "484"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_LastMenstrualPeriodDate"].ToString()); 		// Date Time Period (1251)
                }

                //DTP - Date - Last X-ray Date 
                if (oClaimsRow["DTP03_LastXrayDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "455"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_LastXrayDate"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Hearing and Vision Prescription Date 
                if (oClaimsRow["DTP03_HearVisionPrescriptDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "471"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_HearVisionPrescriptDate"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Disability Dates
                if (oClaimsRow["DTP03_Disability"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "314"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "RD8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_Disability"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Disability Dates - INITIAL DISABILITY PERIOD START
                if (oClaimsRow["DTP03_InitialDisabilityPeriodStart"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "360"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_InitialDisabilityPeriodStart"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Disability Dates - INITIAL DISABILITY PERIOD END
                if (oClaimsRow["DTP03_InitialDisabilityPeriodEnd"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "361"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_InitialDisabilityPeriodEnd"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Last Worked
                if (oClaimsRow["DTP03_LastWorkedDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "297"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_LastWorkedDate"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Authorized Return to Work
                if (oClaimsRow["DTP03_WorkReturnDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "296"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_WorkReturnDate"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Admission 
                if (oClaimsRow["DTP03_HospitalizationAdmissionDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "435"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_HospitalizationAdmissionDate"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Discharge 
                if (oClaimsRow["DTP03_HospitalizationDischargeDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "096"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_HospitalizationDischargeDate"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Assumed and Relinquished Care Dates - REPORT START
                if (oClaimsRow["DTP03_AssumedRelinquishedCareStartDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "090"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_AssumedRelinquishedCareStartDate"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Assumed and Relinquished Care Dates - REPORT END
                if (oClaimsRow["DTP03_AssumedRelinquishedCareEndDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "091"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_AssumedRelinquishedCareEndDate"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Property and Casualty Date of First Contact 
                if (oClaimsRow["DTP03_PropertyCasualtyFirstContactDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "444"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_PropertyCasualtyFirstContactDate"].ToString()); 		// Date Time Period (1251) 
                }

                //DTP - Date - Repricer Received Date
                if (oClaimsRow["DTP03_RepricerReceivedDate"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\DTP"));
                    oSegment.set_DataElementValue(1, 0, "050"); 		// Date/Time Qualifier (374) 
                    oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["DTP03_RepricerReceivedDate"].ToString()); 		// Date Time Period (1251) 
                }

                //PWK - Claim Supplemental Information  
                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\PWK"));
                oSegment.set_DataElementValue(1, 0, oClaimsRow["PWK01_AttachmentReportTypeCode"].ToString()); 		// Report Type Code (755) 
                oSegment.set_DataElementValue(2, 0, oClaimsRow["PWK02_AttachmentTransmissionCode"].ToString()); 		// Report Transmission Code (756) 
                if (oClaimsRow["PWK06_AttachmentControlNo"].ToString().Trim() != "")
                {
                    oSegment.set_DataElementValue(5, 0, "AC"); 		// Identification Code Qualifier (66) 
                    oSegment.set_DataElementValue(6, 0, oClaimsRow["PWK06_AttachmentControlNo"].ToString()); 		// Identification Code (67) 
                }

                //CN1 - Contract Information (CN1)
                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\CN1"));
                oSegment.set_DataElementValue(1, 0, oClaimsRow["CN101_ContractTypeCode"].ToString()); 		// Contract Type Code (1166) 
                oSegment.set_DataElementValue(2, 0, oClaimsRow["CN102_ContractAmount"].ToString()); 		// Monetary Amount (782) 
                oSegment.set_DataElementValue(3, 0, oClaimsRow["CN103_ContractPercentage"].ToString()); 		// Percent (332) 
                oSegment.set_DataElementValue(4, 0, oClaimsRow["CN104_ContractCode"].ToString()); 		// Reference Identification (127) 
                oSegment.set_DataElementValue(5, 0, oClaimsRow["CN105_TermsDiscountPercent"].ToString()); 		// Terms Discount Percent (338) 
                oSegment.set_DataElementValue(6, 0, oClaimsRow["CN106_ContractVersionIdentifier"].ToString()); 		// Version Identifier (799) 

                //AMT - Patient Amount Paid
                if (oClaimsRow["AMT02_PatientAmountPaid"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\AMT"));
                    oSegment.set_DataElementValue(1, 0, "F5"); 		// Amount Qualifier Code (522) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["AMT02_PatientAmountPaid"].ToString()); 		// Monetary Amount (782)
                }


                //REF - SERVICE AUTHORIZATION EXCEPTION CODE
                if (oClaimsRow["REF02_SpecialPaymentReferenceNumber"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "4N"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_SpecialPaymentReferenceNumber"].ToString()); 		// Reference Identification (127) 
                }

                //REF - Mandatory Medicare (Section 4081) Crossover Indicator
                if (oClaimsRow["REF02_MedicareVersionCode"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "F5"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_MedicareVersionCode"].ToString()); 		// Reference Identification (127) 
                }

                //REF - Mammography Certification Number
                if (oClaimsRow["REF02_MammographyCertificationNumber"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "EW"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_MammographyCertificationNumber"].ToString()); 		// Reference Identification (127) 
                }

                //REF - Referral Number (REF) 
                if (oClaimsRow["REF02_ReferralNumber"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "9F"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_ReferralNumber"].ToString()); 		// Reference Identification (127) 
                }

                //REF - Prior Authorization (REF) 
                if (oClaimsRow["REF02_PriorAuthorizationNumber"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "G1"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_PriorAuthorizationNumber"].ToString()); 		// Reference Identification (127) 
                }

                //REF - Payer Claim Control Number (REF) 
                if (oClaimsRow["REF02_PayerClaimControlNumber"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "F8"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_PayerClaimControlNumber"].ToString()); 		// Reference Identification (127) 
                }

                //REF - Clinical Laboratory Improvement Amendment (CLIA) Number
                if (oClaimsRow["REF02_ClinicalLabAmendmentNumber"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "X4"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_ClinicalLabAmendmentNumber"].ToString()); 		// Reference Identification (127) 
                }

                //REF - Repriced Claim Number
                if (oClaimsRow["REF02_RepricedClaimReferenceNumber"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "9A"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_RepricedClaimReferenceNumber"].ToString()); 		// Reference Identification (127) 
                }

                //REF - Adjusted Repriced Claim Number
                if (oClaimsRow["REF02_AdjRepricedClaimReferenceNo"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "9C"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_AdjRepricedClaimReferenceNo"].ToString()); 		// Reference Identification (127) 
                }

                //REF - Investigational Device Exemption Number 
                if (oClaimsRow["REF02_InvestigatDeviceExemptIdfr"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "LX"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_InvestigatDeviceExemptIdfr"].ToString()); 		// Reference Identification (127) 
                }

                //REF - Claim Identifier For Transmission Intermediaries
                if (oClaimsRow["REF02_ValueAddedNetworkTraceNumber"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "D9"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_ValueAddedNetworkTraceNumber"].ToString()); 		// Reference Identification (127) 
                }

                //REF - Medical Record Number
                if (oClaimsRow["REF02_MedicalRecordNumber"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "EA"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_MedicalRecordNumber"].ToString()); 		// Reference Identification (127) 
                }

                ///REF - Demonstration Project Identifier
                if (oClaimsRow["REF02_DemonstrationProjectIdentifier"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "P4"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_DemonstrationProjectIdentifier"].ToString()); 		// Reference Identification (127) 
                }

                //REF - Care Plan Oversight 
                if (oClaimsRow["REF02_CarePlanOversightNumber"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\REF"));
                    oSegment.set_DataElementValue(1, 0, "1J"); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_CarePlanOversightNumber"].ToString()); 		// Reference Identification (127) 
                }

                //K3 - File Information
                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\K3"));
                oSegment.set_DataElementValue(1, 0, oClaimsRow["K301_FileInformation"].ToString()); 		// Fixed Format Information (449) 

                //NTE - Claim Note
                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NTE"));
                oSegment.set_DataElementValue(1, 0, oClaimsRow["NTE01_NoteReferenceCode"].ToString()); 		// Note Reference Code (363) 
                oSegment.set_DataElementValue(2, 0, oClaimsRow["NTE02_ClaimNoteText"].ToString()); 		// Description (352) 

                //CR1 - Ambulance Transport Information
                if (oClaimsRow["CR106_TransportDistanceMiles"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\CR1"));
                    if (oClaimsRow["CR102_PatientWeightPounds"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(1, 0, "LB"); 		// Unit or Basis for Measurement Code (355) 
                        oSegment.set_DataElementValue(2, 0, oClaimsRow["CR102_PatientWeightPounds"].ToString()); 		// Weight (81) 
                    }
                    oSegment.set_DataElementValue(4, 0, oClaimsRow["CR104_Ambulance_Transport_Reason_Code"].ToString()); 		// Ambulance Transport Reason Code (1317) 
                    oSegment.set_DataElementValue(5, 0, "DH"); 		// Unit or Basis for Measurement Code (355) 
                    oSegment.set_DataElementValue(6, 0, oClaimsRow["CR106_TransportDistanceMiles"].ToString()); 		// Quantity (380) 
                    oSegment.set_DataElementValue(9, 0, oClaimsRow["CR109_RoundTripPurposeDescription"].ToString()); 		// Description (352) 
                    oSegment.set_DataElementValue(10, 0, oClaimsRow["CR110_StretcherPurposeDescription"].ToString()); 		// Description (352) 
                }

                //CR2 - Spinal Manipulation Service Information 
                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\CR2"));
                oSegment.set_DataElementValue(8, 0, oClaimsRow["CR208_NatureOfConditionCode"].ToString()); 		// Nature of Condition Code (1342) 
                oSegment.set_DataElementValue(10, 0, oClaimsRow["CR210_PatientConditionDescription"].ToString()); 		// Description (352) 
                oSegment.set_DataElementValue(11, 0, oClaimsRow["CR211_PatientConditionDescription2"].ToString()); 		// Description (352) 

                //CRC - Ambulance Certification 
                if (oClaimsRow["CRC02_AmbulanceConditionIndicator"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\CRC"));
                    oSegment.set_DataElementValue(1, 0, "07"); 		// Code Category (1136) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["CRC02_AmbulanceConditionIndicator"].ToString()); 		// Yes/No Condition or Response Code (1073) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["CRC03_AmbulanceConditionCode"].ToString()); 		// Condition Indicator (1321) 
                    oSegment.set_DataElementValue(4, 0, oClaimsRow["CRC04_AmbulanceConditionCode2"].ToString()); 		// Condition Indicator (1321) 
                    oSegment.set_DataElementValue(5, 0, oClaimsRow["CRC05_AmbulanceConditionCode3"].ToString()); 		// Condition Indicator (1321) 
                    oSegment.set_DataElementValue(6, 0, oClaimsRow["CRC06_AmbulanceConditionCode4"].ToString()); 		// Condition Indicator (1321) 
                    oSegment.set_DataElementValue(7, 0, oClaimsRow["CRC07_AmbulanceConditionCode5"].ToString()); 		// Condition Indicator (1321) 
                }

                //CRC - Patient Condition Information: Vision 
                if (oClaimsRow["CRC01_PatientVisionCodeCategory"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\CRC"));
                    oSegment.set_DataElementValue(1, 0, oClaimsRow["CRC01_PatientVisionCodeCategory"].ToString()); 		// Code Category (1136) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["CRC02_PatientVisionConditionIndicator"].ToString()); 		// Yes/No Condition or Response Code (1073) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["CRC03_PatientVisionConditionCode"].ToString()); 		// Condition Indicator (1321) 
                    oSegment.set_DataElementValue(4, 0, oClaimsRow["CRC04_PatientVisionConditionCode2"].ToString()); 		// Condition Indicator (1321) 
                    oSegment.set_DataElementValue(5, 0, oClaimsRow["CRC05_PatientVisionConditionCode3"].ToString()); 		// Condition Indicator (1321) 
                    oSegment.set_DataElementValue(6, 0, oClaimsRow["CRC06_PatientVisionConditionCode4"].ToString()); 		// Condition Indicator (1321) 
                    oSegment.set_DataElementValue(7, 0, oClaimsRow["CRC07_PatientVisionConditionCode5"].ToString()); 		// Condition Indicator (1321) 
                }

                //CRC - Homebound Indicator 
                if (oClaimsRow["CRC02_HomeboundConditionCode"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\CRC"));
                    oSegment.set_DataElementValue(1, 0, "75"); 		// Code Category (1136) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["CRC02_HomeboundConditionCode"].ToString()); 		// Yes/No Condition or Response Code (1073) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["CRC03_HomeboundIndicator"].ToString()); 		// Condition Indicator (1321) 
                }

                //CRC - EPSDT Referral
                if (oClaimsRow["CRC02_EPSDT_ConditionCodeAppliesIndicator"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\CRC"));
                    oSegment.set_DataElementValue(1, 0, "ZZ"); 		// Code Category (1136) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["CRC02_EPSDT_ConditionCodeAppliesIndicator"].ToString()); 		// Yes/No Condition or Response Code (1073) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["CRC03_EPSDT_ConditionIndicator"].ToString()); 		// Condition Indicator (1321) 
                    oSegment.set_DataElementValue(4, 0, oClaimsRow["CRC04_EPSDT_ConditionIndicator2"].ToString()); 		// Condition Indicator (1321) 
                    oSegment.set_DataElementValue(5, 0, oClaimsRow["CRC05_EPSDT_ConditionIndicator3"].ToString()); 		// Condition Indicator (1321) 
                }

                //HI - HEALTH CARE INFORMATION DIAGNOSIS CODES
                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\HI"));
                oSegment.set_DataElementValue(1, 1, oClaimsRow["HI01_01_DiagnosisTypeCode"].ToString()); 		// Code List Qualifier Code (1270) 
                oSegment.set_DataElementValue(1, 2, oClaimsRow["HI01_02_DiagnosisCode"].ToString()); 		// Industry Code (1271) 
                oSegment.set_DataElementValue(2, 1, oClaimsRow["HI02_01_DiagnosisTypeCode2"].ToString()); 		// Code List Qualifier Code (1270) 
                oSegment.set_DataElementValue(2, 2, oClaimsRow["HI02_02_DiagnosisCode2"].ToString()); 		// Industry Code (1271) 
                oSegment.set_DataElementValue(3, 1, oClaimsRow["HI03_01_DiagnosisTypeCode3"].ToString()); 		// Code List Qualifier Code (1270) 
                oSegment.set_DataElementValue(3, 2, oClaimsRow["HI03_02_DiagnosisCode3"].ToString()); 		// Industry Code (1271) 
                oSegment.set_DataElementValue(4, 1, oClaimsRow["HI04_01_DiagnosisTypeCode4"].ToString()); 		// Code List Qualifier Code (1270) 
                oSegment.set_DataElementValue(4, 2, oClaimsRow["HI04_02_DiagnosisCode4"].ToString()); 		// Industry Code (1271) 
                oSegment.set_DataElementValue(5, 1, oClaimsRow["HI05_01_DiagnosisTypeCode5"].ToString()); 		// Code List Qualifier Code (1270) 
                oSegment.set_DataElementValue(5, 2, oClaimsRow["HI05_02_DiagnosisCode5"].ToString()); 		// Industry Code (1271) 
                oSegment.set_DataElementValue(6, 1, oClaimsRow["HI06_01_DiagnosisTypeCode6"].ToString()); 		// Code List Qualifier Code (1270) 
                oSegment.set_DataElementValue(6, 2, oClaimsRow["HI06_02_DiagnosisCode6"].ToString()); 		// Industry Code (1271) 
                oSegment.set_DataElementValue(7, 1, oClaimsRow["HI07_01_DiagnosisTypeCode7"].ToString()); 		// Code List Qualifier Code (1270) 
                oSegment.set_DataElementValue(7, 2, oClaimsRow["HI07_02_DiagnosisCode7"].ToString()); 		// Industry Code (1271) 
                oSegment.set_DataElementValue(8, 1, oClaimsRow["HI08_01_DiagnosisTypeCode8"].ToString()); 		// Code List Qualifier Code (1270) 
                oSegment.set_DataElementValue(8, 2, oClaimsRow["HI08_02_DiagnosisCode8"].ToString()); 		// Industry Code (1271) 
                oSegment.set_DataElementValue(9, 1, oClaimsRow["HI09_01_DiagnosisTypeCode9"].ToString()); 		// Code List Qualifier Code (1270) 
                oSegment.set_DataElementValue(9, 2, oClaimsRow["HI09_02_DiagnosisCode9"].ToString()); 		// Industry Code (1271) 
                oSegment.set_DataElementValue(10, 1, oClaimsRow["HI10_01_DiagnosisTypeCode10"].ToString()); 		// Code List Qualifier Code (1270) 
                oSegment.set_DataElementValue(10, 2, oClaimsRow["HI10_02_DiagnosisCode10"].ToString()); 		// Industry Code (1271) 
                oSegment.set_DataElementValue(11, 1, oClaimsRow["HI11_01_DiagnosisTypeCode11"].ToString()); 		// Code List Qualifier Code (1270) 
                oSegment.set_DataElementValue(11, 2, oClaimsRow["HI11_02_DiagnosisCode11"].ToString()); 		// Industry Code (1271) 
                oSegment.set_DataElementValue(12, 1, oClaimsRow["HI12_01_DiagnosisTypeCode12"].ToString()); 		// Code List Qualifier Code (1270) 
                oSegment.set_DataElementValue(12, 2, oClaimsRow["HI12_02_DiagnosisCode12"].ToString()); 		// Industry Code (1271) 


                //HI - Anesthesia Related Procedure
                if (oClaimsRow["HI01_02_AnesthesiaSurgicalPrincipleProcedure"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\HI"));
                    oSegment.set_DataElementValue(1, 1, "BP"); 		// Code List Qualifier Code (1270) 
                    oSegment.set_DataElementValue(1, 2, oClaimsRow["HI01_02_AnesthesiaSurgicalPrincipleProcedure"].ToString()); 		// Industry Code (1271) 
                    if (oClaimsRow["HI02_02_AnesthesiaSurgicalProcedure"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(2, 1, "BO"); 		// Code List Qualifier Code (1270) 
                        oSegment.set_DataElementValue(2, 2, oClaimsRow["HI02_02_AnesthesiaSurgicalProcedure"].ToString()); 		// Industry Code (1271) 
                    }
                }

                //HI - Condition Information
                if (oClaimsRow["HI01_02_ConditionCode1"].ToString().Trim() != "")
                {
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\HI"));
                    oSegment.set_DataElementValue(1, 1, "BG"); 		// Code List Qualifier Code (1270) 
                    oSegment.set_DataElementValue(1, 2, oClaimsRow["HI01_02_ConditionCode1"].ToString()); 		// Industry Code (1271) 
                    if (oClaimsRow["HI02_02_ConditionCode2"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(2, 1, "BG"); 		// Code List Qualifier Code (1270) 
                        oSegment.set_DataElementValue(2, 2, oClaimsRow["HI02_02_ConditionCode2"].ToString()); 		// Industry Code (1271) 
                    }
                    if (oClaimsRow["HI03_02_ConditionCode3"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(3, 1, "BG"); 		// Code List Qualifier Code (1270) 
                        oSegment.set_DataElementValue(3, 2, oClaimsRow["HI03_02_ConditionCode3"].ToString()); 		// Industry Code (1271) 
                    }
                    if (oClaimsRow["HI04_02_ConditionCode4"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(4, 1, "BG"); 		// Code List Qualifier Code (1270) 
                        oSegment.set_DataElementValue(4, 2, oClaimsRow["HI04_02_ConditionCode4"].ToString()); 		// Industry Code (1271) 
                    }
                    if (oClaimsRow["HI05_02_ConditionCode5"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(5, 1, "BG"); 		// Code List Qualifier Code (1270) 
                        oSegment.set_DataElementValue(5, 2, oClaimsRow["HI05_02_ConditionCode5"].ToString()); 		// Industry Code (1271) 
                    }
                    if (oClaimsRow["HI06_02_ConditionCode6"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(6, 1, "BG"); 		// Code List Qualifier Code (1270) 
                        oSegment.set_DataElementValue(6, 2, oClaimsRow["HI06_02_ConditionCode6"].ToString()); 		// Industry Code (1271) 
                    }
                    if (oClaimsRow["HI07_02_ConditionCode7"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(7, 1, "BG"); 		// Code List Qualifier Code (1270) 
                        oSegment.set_DataElementValue(7, 2, oClaimsRow["HI07_02_ConditionCode7"].ToString()); 		// Industry Code (1271) 
                    }
                    if (oClaimsRow["HI08_02_ConditionCode8"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(8, 1, "BG"); 		// Code List Qualifier Code (1270) 
                        oSegment.set_DataElementValue(8, 2, oClaimsRow["HI08_02_ConditionCode8"].ToString()); 		// Industry Code (1271) 
                    
                    }
                    if (oClaimsRow["HI09_02_ConditionCode9"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(9, 1, "BG"); 		// Code List Qualifier Code (1270) 
                        oSegment.set_DataElementValue(9, 2, oClaimsRow["HI09_02_ConditionCode9"].ToString()); 		// Industry Code (1271) 
                    }
                    if (oClaimsRow["HI10_02_ConditionCode10"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(10, 1, "BG"); 		// Code List Qualifier Code (1270) 
                        oSegment.set_DataElementValue(10, 2, oClaimsRow["HI10_02_ConditionCode10"].ToString()); 		// Industry Code (1271) 
                    }
                    if (oClaimsRow["HI11_02_ConditionCode11"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(11, 1, "BG"); 		// Code List Qualifier Code (1270) 
                        oSegment.set_DataElementValue(11, 2, oClaimsRow["HI11_02_ConditionCode11"].ToString()); 		// Industry Code (1271) 
                    }
                    if (oClaimsRow["HI12_02_ConditionCode12"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(12, 1, "BG"); 		// Code List Qualifier Code (1270) 
                        oSegment.set_DataElementValue(12, 2, oClaimsRow["HI12_02_ConditionCode12"].ToString()); 		// Industry Code (1271) 
                    }

                }//HI - Condition Information

                //HCP - Claim Pricing/Repricing Information (HCP) 
                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\HCP"));
                oSegment.set_DataElementValue(1, 0, oClaimsRow["HCP01_PricingMethodology"].ToString()); 		// Pricing Methodology (1473) 
                oSegment.set_DataElementValue(2, 0, oClaimsRow["HCP02_RepricedAllowedAmount"].ToString()); 		// Monetary Amount (782) 
                oSegment.set_DataElementValue(3, 0, oClaimsRow["HCP03_RepricedSavingAmount"].ToString()); 		// Monetary Amount (782) 
                oSegment.set_DataElementValue(4, 0, oClaimsRow["HCP04_RepricingOrganizationIdentifier"].ToString()); 		// Reference Identification (127) 
                oSegment.set_DataElementValue(5, 0, oClaimsRow["HCP05_RepricingPerDiemFlatRateAmount"].ToString()); 		// Rate (118) 
                oSegment.set_DataElementValue(6, 0, oClaimsRow["HCP06_RepricedApprovAmbPatientGroupCode"].ToString()); 		// Reference Identification (127) 
                oSegment.set_DataElementValue(7, 0, oClaimsRow["HCP07_RepricedApprovAmbPatientGroupAmount"].ToString()); 		// Monetary Amount (782) 
                oSegment.set_DataElementValue(13, 0, oClaimsRow["HCP13_RejectReasonCode"].ToString()); 		// Reject Reason Code (901) 
                oSegment.set_DataElementValue(14, 0, oClaimsRow["HCP14_PolicyComplianceCode"].ToString()); 		// Policy Compliance Code (1526) 
                oSegment.set_DataElementValue(15, 0, oClaimsRow["HCP15_ExceptionCode"].ToString()); 		// Exception Code (1527) 


                //2310A REFERRING PROVIDER NAME
                if (oClaimsRow["NM103_ReferringProviderLastName"].ToString().Trim() != "")
                {
                    //NM1 - Referring Provider Name
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\NM1"));
                    oSegment.set_DataElementValue(1, 0, "DN"); 		// Entity Identifier Code (98) 
                    oSegment.set_DataElementValue(2, 0, "1"); 		// Entity Type Qualifier (1065) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["NM103_ReferringProviderLastName"].ToString()); 		// Name Last or Organization Name (1035) 
                    oSegment.set_DataElementValue(4, 0, oClaimsRow["NM104_ReferringProviderLastFirst"].ToString()); 		// Name First (1036) 
                    oSegment.set_DataElementValue(5, 0, oClaimsRow["NM105_ReferringProviderLastMiddle"].ToString()); 		// Name Middle (1037) 
                    oSegment.set_DataElementValue(7, 0, oClaimsRow["NM107_ReferringProviderLastSuffix"].ToString()); 		// Name Suffix (1039) 
                    oSegment.set_DataElementValue(8, 0, "XX"); 		// Identification Code Qualifier (66) 
                    oSegment.set_DataElementValue(9, 0, oClaimsRow["NM109_ReferringProviderIdentifier"].ToString()); 		// Identification Code (67) 

                    //REF - Referring Provider Secondary Identification
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\REF"));
                    oSegment.set_DataElementValue(1, 0, oClaimsRow["REF01_ReferringProviderSecondaryIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_ReferringProviderSecondaryId"].ToString()); 		// Reference Identification (127) 

                }//2310A REFERRING PROVIDER NAME


                //2310A REFERRING PROVIDER NAME - Primary Care Provider
                if (oClaimsRow["NM103_PrimaryCareProviderLastName"].ToString().Trim() != "")
                {
                    //NM1 - Referring Provider Name - Primary Care Provider
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\NM1"));
                    oSegment.set_DataElementValue(1, 0, "P3"); 		// Entity Identifier Code (98) 
                    oSegment.set_DataElementValue(2, 0, "1"); 		// Entity Type Qualifier (1065) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["NM103_PrimaryCareProviderLastName"].ToString()); 		// Name Last or Organization Name (1035) 
                    oSegment.set_DataElementValue(4, 0, oClaimsRow["NM104_PrimaryCareProviderLastFirst"].ToString()); 		// Name First (1036) 
                    oSegment.set_DataElementValue(5, 0, oClaimsRow["NM105_PrimaryCareProviderLastMiddle"].ToString()); 		// Name Middle (1037) 
                    oSegment.set_DataElementValue(7, 0, oClaimsRow["NM107_PrimaryCareProviderLastSuffix"].ToString()); 		// Name Suffix (1039) 
                    if (oClaimsRow["NM109_PrimaryCareProviderIdentifier"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(8, 0, "XX"); 		// Identification Code Qualifier (66) 
                        oSegment.set_DataElementValue(9, 0, oClaimsRow["NM109_PrimaryCareProviderIdentifier"].ToString()); 		// Identification Code (67) 
                    }

                    //REF - Referring Provider Secondary Identification - Primary Care Provider
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\REF"));
                    oSegment.set_DataElementValue(1, 0, oClaimsRow["REF01_ReferringProviderSecondaryIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_ReferringProviderSecondaryId"].ToString()); 		// Reference Identification (127) 

                }//2310A REFERRING PROVIDER NAME - Primary Care Provider


                //2310B RENDERING PROVIDER 
                if (oClaimsRow["NM103_RenderingProviderLastOrOrganizationName"].ToString().Trim() != "")
                {
                    //NM1 - Rendering Provider Name
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\NM1"));
                    oSegment.set_DataElementValue(1, 0, "82"); 		// Entity Identifier Code (98) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["NM102_RenderingProviderTypeQualifier"].ToString()); 		// Entity Type Qualifier (1065) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["NM103_RenderingProviderLastOrOrganizationName"].ToString()); 		// Name Last or Organization Name (1035) 
                    oSegment.set_DataElementValue(4, 0, oClaimsRow["NM104_RenderingProviderFirst"].ToString()); 		// Name First (1036) 
                    oSegment.set_DataElementValue(5, 0, oClaimsRow["NM105_RenderingProviderMiddle"].ToString()); 		// Name Middle (1037) 
                    oSegment.set_DataElementValue(7, 0, oClaimsRow["NM107_RenderingProviderSuffix"].ToString()); 		// Name Suffix (1039) 
                    if (oClaimsRow["NM109_RenderingProviderIdentifier"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(8, 0, "XX"); 		// Identification Code Qualifier (66) 
                        oSegment.set_DataElementValue(9, 0, oClaimsRow["NM109_RenderingProviderIdentifier"].ToString()); 		// Identification Code (67) 
			        }

                    //PRV - Rendering Provider Specialty Information
                    if (oClaimsRow["PRV03_ProviderTaxonomyCode"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\PRV"));
                        oSegment.set_DataElementValue(1, 0, "PE"); 		// Provider Code (1221) 
                        oSegment.set_DataElementValue(2, 0, "PXC"); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(3, 0, oClaimsRow["PRV03_ProviderTaxonomyCode"].ToString()); 		// Reference Identification (127) 
                    }

                    //REF - Rendering Provider Secondary Identification
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\REF"));
                    oSegment.set_DataElementValue(1, 0, oClaimsRow["REF01_RenderingProviderSecondaryIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_RenderingProviderSecondaryId"].ToString()); 		// Reference Identification (127) 

                }//2310B RENDERING PROVIDER


                //2310C SERVICE FACILITY LOCATION
                if (oClaimsRow["NM103_LabFacilityName"].ToString().Trim() != "")
                {
                    //NM1 - Service Facility Location Name
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\NM1"));
                    oSegment.set_DataElementValue(1, 0, "77"); 		// Entity Identifier Code (98) 
                    oSegment.set_DataElementValue(2, 0, "2"); 		// Entity Type Qualifier (1065) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["NM103_LabFacilityName"].ToString()); 		// Name Last or Organization Name (1035) 
                    if (oClaimsRow["NM109_LabFacilityIdentifier"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(8, 0, "XX"); 		// Identification Code Qualifier (66) 
                        oSegment.set_DataElementValue(9, 0, oClaimsRow["NM109_LabFacilityIdentifier"].ToString()); 		// Identification Code (67) 
                    }

                    //N3 - Service Facility Location Address
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\N3"));
                    oSegment.set_DataElementValue(1, 0, oClaimsRow["N301_LabFacilityAddress1"].ToString()); 		// Address Information (166) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["N302_LabFacilityAddress2"].ToString()); 		// Address Information (166) 

                    //N4 - Service Facility Location City, State, ZIP Code
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\N4"));
                    oSegment.set_DataElementValue(1, 0, oClaimsRow["N401_LabFacilityCity"].ToString()); 		// City Name (19) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["N402_LabFacilityState"].ToString()); 		// State or Province Code (156) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["N403_LabFacilityZip"].ToString()); 		// Postal Code (116) 
                    oSegment.set_DataElementValue(4, 0, oClaimsRow["N404_LabFacilityCountryCode"].ToString()); 		// Country Code (26) 
                    oSegment.set_DataElementValue(7, 0, oClaimsRow["N407_LabFacilityCountrySubdivisionCode"].ToString()); 		// Country Subdivision Code (1715) 

                    //REF - Service Facility Location Secondary Identification
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\REF"));
                    oSegment.set_DataElementValue(1, 0, oClaimsRow["REF01_LabFacilityyIdQualifier"].ToString()); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_LabFacilityIdentification"].ToString()); 		// Reference Identification (127) 

                    //PER - Service Facility Contact Information 
                    if (oClaimsRow["PER04_LabFacilityTelephoneNumber"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\PER"));
                        oSegment.set_DataElementValue(1, 0, "IC"); 		// Contact Function Code (366) 
                        oSegment.set_DataElementValue(2, 0, oClaimsRow["PER02_LabFacilityContactName"].ToString()); 		// Name (93) 
                        oSegment.set_DataElementValue(3, 0, "TE"); 		// Communication Number Qualifier (365) 
                        oSegment.set_DataElementValue(4, 0, oClaimsRow["PER04_LabFacilityTelephoneNumber"].ToString()); 		// Communication Number (364) 
                        if (oClaimsRow["PER06_LabFacilityExtensionNumber"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(5, 0, "EX"); 		// Communication Number Qualifier (365) 
                            oSegment.set_DataElementValue(6, 0, oClaimsRow["PER06_LabFacilityExtensionNumber"].ToString()); 		// Communication Number (364) 

                        }
                    }//PER
                    
                }//2310C SERVICE FACILITY LOCATION

                
                //2310D SUPERVISING PROVIDER
                if (oClaimsRow["NM103_SupervisingPhysicianLastName"].ToString().Trim() != "")
                {
                    //NM1 - Supervising Provider Name
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\NM1"));
                    oSegment.set_DataElementValue(1, 0, "DQ"); 		// Entity Identifier Code (98) 
                    oSegment.set_DataElementValue(2, 0, "1"); 		// Entity Type Qualifier (1065) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["NM103_SupervisingPhysicianLastName"].ToString()); 		// Name Last or Organization Name (1035) 
                    oSegment.set_DataElementValue(4, 0, oClaimsRow["NM104_SupervisingPhysicianFirst"].ToString()); 		// Name First (1036) 
                    oSegment.set_DataElementValue(5, 0, oClaimsRow["NM105_SupervisingPhysicianMiddle"].ToString()); 		// Name Middle (1037) 
                    oSegment.set_DataElementValue(7, 0, oClaimsRow["NM107_SupervisingPhysicianSuffix"].ToString()); 		// Name Suffix (1039) 
                    if (oClaimsRow["NM109_SupervisingPhysicianIdentifier"].ToString().Trim() != "")
                    {
                        oSegment.set_DataElementValue(8, 0, "XX"); 		// Identification Code Qualifier (66) 
                        oSegment.set_DataElementValue(9, 0, oClaimsRow["NM109_SupervisingPhysicianIdentifier"].ToString()); 		// Identification Code (67) 
                    }

                    //REF - Supervising Provider Secondary Identification
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\REF"));
                    oSegment.set_DataElementValue(1, 0, oClaimsRow["REF01_SupervisingPhysicianSecondaryIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["REF02_SupervisingPhysicianIdSecondaryId"].ToString()); 		// Reference Identification (127) 

                }//2310D SUPERVISING PROVIDER


                //2310E AMBULANCE PICK-UP LOCATION
                if (oClaimsRow["N301_AmbulancePickupAddress1"].ToString().Trim() != "")
                {
                    //NM1 - Ambulance Pick-up Location
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\NM1"));
                    oSegment.set_DataElementValue(1, 0, "PW"); 		// Entity Identifier Code (98) 
                    oSegment.set_DataElementValue(2, 0, "2"); 		// Entity Type Qualifier (1065) 

                    //N3 - Ambulance Pick-up Location Address 
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\N3"));
                    oSegment.set_DataElementValue(1, 0, oClaimsRow["N301_AmbulancePickupAddress1"].ToString()); 		// Address Information (166) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["N302_AmbulancePickupAddress2"].ToString()); 		// Address Information (166) 

                    //N4 - Ambulance Pick-up Location City, State, ZIP Code
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\N4"));
                    oSegment.set_DataElementValue(1, 0, oClaimsRow["N401_AmbulancePickupCity"].ToString()); 		// City Name (19) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["N402_AmbulancePickupState"].ToString()); 		// State or Province Code (156) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["N403_AmbulancePickupZip"].ToString()); 		// Postal Code (116) 
                    oSegment.set_DataElementValue(4, 0, oClaimsRow["N404_AmbulancePickupCountryCode"].ToString()); 		// Country Code (26) 
                    oSegment.set_DataElementValue(7, 0, oClaimsRow["N407_AmbulancePickupCountrySubdivisionCode"].ToString()); 		// Country Subdivision Code (1715) 

                }//2310E AMBULANCE PICK-UP LOCATION


                //2310F AMBULANCE DROP-OFF LOCATION
                if (oClaimsRow["N301_AmbulanceDropOffAddress1"].ToString().Trim() != "")
                {
                    //NM1 - Ambulance Drop-off Location
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\NM1"));
                    oSegment.set_DataElementValue(1, 0, "45"); 		// Entity Identifier Code (98) 
                    oSegment.set_DataElementValue(2, 0, "2"); 		// Entity Type Qualifier (1065) 

                    //N3 - Ambulance Drop-off Location Address 
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\N3"));
                    oSegment.set_DataElementValue(1, 0, oClaimsRow["N301_AmbulanceDropOffAddress1"].ToString()); 		// Address Information (166) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["N302_AmbulanceDropOffAddress2"].ToString()); 		// Address Information (166) 

                    //N4 - Ambulance Drop-off Location City, State, ZIP Code
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\NM1\\N4"));
                    oSegment.set_DataElementValue(1, 0, oClaimsRow["N401_AmbulanceDropOffCity"].ToString()); 		// City Name (19) 
                    oSegment.set_DataElementValue(2, 0, oClaimsRow["N402_AmbulanceDropOffState"].ToString()); 		// State or Province Code (156) 
                    oSegment.set_DataElementValue(3, 0, oClaimsRow["N403_AmbulanceDropOffZip"].ToString()); 		// Postal Code (116) 
                    oSegment.set_DataElementValue(4, 0, oClaimsRow["N404_AmbulanceDropOffCountryCode"].ToString()); 		// Country Code (26) 
                    oSegment.set_DataElementValue(7, 0, oClaimsRow["N407_AmbulanceDropOffCountrySubdivisionCode"].ToString()); 		// Country Subdivision Code (1715) 

                }//2310F AMBULANCE DROP-OFF LOCATION
                
                
                //2320 OTHER SUBSCRIBER INFORMATION
                sSql = "select * from [837X222_OtherSubscriberInfo] where Claimskey = " + oClaimsRow["Claimskey"].ToString();
                SqlDataAdapter oOtherSubscriberInfoAdapter = new SqlDataAdapter(sSql, oConnection);
                DataSet oOtherSubscriberInfoDs = new DataSet("dsOtherSubscriberInfo");
                oOtherSubscriberInfoAdapter.Fill(oOtherSubscriberInfoDs, "dsOtherSubscriberInfo");

                foreach (DataRow oOtherSubscriberInfoRow in oOtherSubscriberInfoDs.Tables["dsOtherSubscriberInfo"].Rows)
                {
                    if (oOtherSubscriberInfoRow["SBR01_PayerResponsibSeqNoCode"].ToString().Trim() != "")
                    {
                        //SBR - Other Subscriber Information
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\SBR"));
                        oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["SBR01_PayerResponsibSeqNoCode"].ToString()); 		// Payer Responsibility Sequence Number Code (1138) 
                        oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["SBR02_IndividualRelationshipCode"].ToString()); 		// Individual Relationship Code (1069) 
                        oSegment.set_DataElementValue(3, 0, oOtherSubscriberInfoRow["SBR03_ReferenceIdentification"].ToString()); 		// Reference Identification (127) 
                        oSegment.set_DataElementValue(4, 0, oOtherSubscriberInfoRow["SBR04_OtherInsuredGroupName"].ToString()); 		// Name (93) 
                        oSegment.set_DataElementValue(5, 0, oOtherSubscriberInfoRow["SBR05_InsuranceTypeCode"].ToString()); 		// Insurance Type Code (1336) 
                        oSegment.set_DataElementValue(9, 0, oOtherSubscriberInfoRow["SBR09_ClaimFilingIndicatorCode"].ToString()); 		// Claim Filing Indicator Code (1032) 

                        //CAS - Claim Level Adjustments 
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\CAS"));
                        oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["CAS01_ClaimAdjustmentGroupCode"].ToString()); 		// Claim Adjustment Group Code (1033) 
                        oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["CAS02_AdjustmentReasonCode"].ToString()); 		// Claim Adjustment Reason Code (1034) 
                        oSegment.set_DataElementValue(3, 0, oOtherSubscriberInfoRow["CAS03_AdjustmentAmount"].ToString()); 		// Monetary Amount (782) 
                        oSegment.set_DataElementValue(4, 0, oOtherSubscriberInfoRow["CAS04_AdjustmentQuantity"].ToString()); 		// Quantity (380) 
                        oSegment.set_DataElementValue(5, 0, oOtherSubscriberInfoRow["CAS05_AdjustmentReasonCode2"].ToString()); 		// Claim Adjustment Reason Code (1034) 
                        oSegment.set_DataElementValue(6, 0, oOtherSubscriberInfoRow["CAS06_AdjustmentAmount2"].ToString()); 		// Monetary Amount (782) 
                        oSegment.set_DataElementValue(7, 0, oOtherSubscriberInfoRow["CAS07_AdjustmentQuantity2"].ToString()); 		// Quantity (380) 
                        oSegment.set_DataElementValue(8, 0, oOtherSubscriberInfoRow["CAS08_AdjustmentReasonCode3"].ToString()); 		// Claim Adjustment Reason Code (1034) 
                        oSegment.set_DataElementValue(9, 0, oOtherSubscriberInfoRow["CAS09_AdjustmentAmount3"].ToString()); 		// Monetary Amount (782) 
                        oSegment.set_DataElementValue(10, 0, oOtherSubscriberInfoRow["CAS10_AdjustmentQuantity3"].ToString()); 		// Quantity (380) 
                        oSegment.set_DataElementValue(11, 0, oOtherSubscriberInfoRow["CAS11_AdjustmentReasonCode4"].ToString()); 		// Claim Adjustment Reason Code (1034) 
                        oSegment.set_DataElementValue(12, 0, oOtherSubscriberInfoRow["CAS12_AdjustmentAmount4"].ToString()); 		// Monetary Amount (782) 
                        oSegment.set_DataElementValue(13, 0, oOtherSubscriberInfoRow["CAS13_AdjustmentQuantity4"].ToString()); 		// Quantity (380) 
                        oSegment.set_DataElementValue(14, 0, oOtherSubscriberInfoRow["CAS14_AdjustmentReasonCode5"].ToString()); 		// Claim Adjustment Reason Code (1034) 
                        oSegment.set_DataElementValue(15, 0, oOtherSubscriberInfoRow["CAS15_AdjustmentAmount5"].ToString()); 		// Monetary Amount (782) 
                        oSegment.set_DataElementValue(16, 0, oOtherSubscriberInfoRow["CAS16_AdjustmentQuantity5"].ToString()); 		// Quantity (380) 
                        oSegment.set_DataElementValue(17, 0, oOtherSubscriberInfoRow["CAS17_AdjustmentReasonCode6"].ToString()); 		// Claim Adjustment Reason Code (1034) 
                        oSegment.set_DataElementValue(18, 0, oOtherSubscriberInfoRow["CAS18_AdjustmentAmount6"].ToString()); 		// Monetary Amount (782) 
                        oSegment.set_DataElementValue(19, 0, oOtherSubscriberInfoRow["CAS19_AdjustmentQuantity6"].ToString()); 		// Quantity (380) 


                        //AMT - Coordination of Benefits (COB) Payer Paid Amount
                        if (oOtherSubscriberInfoRow["AMT02_PayorAmountPaid"].ToString().Trim() != "")
                        {
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\AMT"));
                            oSegment.set_DataElementValue(1, 0, "D"); 		// Amount Qualifier Code (522) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["AMT02_PayorAmountPaid"].ToString()); 		// Monetary Amount (782) 
                        }

                        //AMT - Coordination of Benefits (COB) Total Non-Covered Amount
                        if (oOtherSubscriberInfoRow["AMT02_NonCoveredChargedAmount"].ToString().Trim() != "")
                        {
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\AMT"));
                            oSegment.set_DataElementValue(1, 0, "A8"); 		// Amount Qualifier Code (522) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["AMT02_NonCoveredChargedAmount"].ToString()); 		// Monetary Amount (782) 
                        }

                        //AMT - Remaining Patient Liability 
                        if (oOtherSubscriberInfoRow["AMT02_RemainingPatientLiability"].ToString().Trim() != "")
                        {
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\AMT"));
                            oSegment.set_DataElementValue(1, 0, "EAF"); 		// Amount Qualifier Code (522) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["AMT02_RemainingPatientLiability"].ToString()); 		// Monetary Amount (782) 
                        }

                        //OI - Other Insurance Coverage Information
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\OI"));
                        oSegment.set_DataElementValue(3, 0, oOtherSubscriberInfoRow["OI03_BenefitsAssignmentCertIndicator"].ToString()); 		// Yes/No Condition or Response Code (1073) 
                        oSegment.set_DataElementValue(4, 0, oOtherSubscriberInfoRow["OI04_PatientSignatureSourceCode"].ToString()); 		// Patient Signature Source Code (1351) 
                        oSegment.set_DataElementValue(6, 0, oOtherSubscriberInfoRow["OI06_ReleaseOfInformationCode"].ToString()); 		// Release of Information Code (1363) 

                        //MOA - Outpatient Adjudication Information
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\MOA"));
                        oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["MOA01_ReimbursementRate"].ToString()); 		// Percent (954) 
                        oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["MOA02_HCPCS_PayableAmount"].ToString()); 		// Monetary Amount (782) 
                        oSegment.set_DataElementValue(3, 0, oOtherSubscriberInfoRow["MOA03_ClaimPaymentRemarkCode"].ToString()); 		// Reference Identification (127) 
                        oSegment.set_DataElementValue(4, 0, oOtherSubscriberInfoRow["MOA04_ClaimPaymentRemarkCode2"].ToString()); 		// Reference Identification (127) 
                        oSegment.set_DataElementValue(5, 0, oOtherSubscriberInfoRow["MOA05_ClaimPaymentRemarkCode3"].ToString()); 		// Reference Identification (127) 
                        oSegment.set_DataElementValue(6, 0, oOtherSubscriberInfoRow["MOA06_ClaimPaymentRemarkCode4"].ToString()); 		// Reference Identification (127) 
                        oSegment.set_DataElementValue(7, 0, oOtherSubscriberInfoRow["MOA07_ClaimPaymentRemarkCode5"].ToString()); 		// Reference Identification (127) 
                        oSegment.set_DataElementValue(8, 0, oOtherSubscriberInfoRow["MOA08_EndStageRenalDiseasePaymntAmnt"].ToString()); 		// Monetary Amount (782) 
                        oSegment.set_DataElementValue(9, 0, oOtherSubscriberInfoRow["MOA09_NonPayableProfessionComponentBill"].ToString()); 		// Monetary Amount (782) 

                        //2330A OTHER SUBSCRIBER OR INSURED
                        if (oOtherSubscriberInfoRow["NM103_OtherInsuredLastName"].ToString().Trim() != "")
                        {
                            //NM1 - Other Subscriber Name
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\NM1"));
                            oSegment.set_DataElementValue(1, 0, "IL"); 		// Entity Identifier Code (98) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["NM102_OtherInsuredEntityTypeQlfr"].ToString()); 		// Entity Type Qualifier (1065) 
                            oSegment.set_DataElementValue(3, 0, oOtherSubscriberInfoRow["NM103_OtherInsuredLastName"].ToString()); 		// Name Last or Organization Name (1035) 
                            oSegment.set_DataElementValue(4, 0, oOtherSubscriberInfoRow["NM104_OtherInsuredFirst"].ToString()); 		// Name First (1036) 
                            oSegment.set_DataElementValue(5, 0, oOtherSubscriberInfoRow["NM105_OtherInsuredMiddle"].ToString()); 		// Name Middle (1037) 
                            oSegment.set_DataElementValue(7, 0, oOtherSubscriberInfoRow["NM107_OtherInsuredSuffix"].ToString()); 		// Name Suffix (1039) 
                            oSegment.set_DataElementValue(8, 0, oOtherSubscriberInfoRow["NM108_OtherInsuredIdQlfr"].ToString()); 		// Identification Code Qualifier (66) 
                            oSegment.set_DataElementValue(9, 0, oOtherSubscriberInfoRow["NM109_OtherInsuredID"].ToString()); 		// Identification Code (67) 

                            //N3 - Other Subscriber Address
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\N3"));
                            oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["N301_OtherInsuredAddress"].ToString()); 		// Address Information (166) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["N302_OtherInsuredAddress2"].ToString()); 		// Address Information (166) 

                            //N4 - Other Subscriber City, State, ZIP Code
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\N4"));
                            oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["N401_OtherInsuredCity"].ToString()); 		// City Name (19) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["N402_OtherInsuredState"].ToString()); 		// State or Province Code (156) 
                            oSegment.set_DataElementValue(3, 0, oOtherSubscriberInfoRow["N403_OtherInsuredZip"].ToString()); 		// Postal Code (116) 
                            oSegment.set_DataElementValue(4, 0, oOtherSubscriberInfoRow["N404_OtherInsuredCountryCode"].ToString()); 		// Country Code (26) 
                            oSegment.set_DataElementValue(7, 0, oOtherSubscriberInfoRow["N407_OtherInsuredCountrySubdivision"].ToString()); 		// Country Subdivision Code (1715) 

                            //REF - Other Subscriber Secondary Identification
                            if (oOtherSubscriberInfoRow["REF02_OtherInsuredSecondaryID"].ToString().Trim() != "")
                            {
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\REF"));
                                oSegment.set_DataElementValue(1, 0, "SY"); 		// Reference Identification Qualifier (128) 
                                oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["REF02_OtherInsuredSecondaryID"].ToString()); 		// Reference Identification (127) 
                            }

                        }//2330A OTHER SUBSCRIBER OR INSURED


                        //2330B OTHER PAYER
                        if (oOtherSubscriberInfoRow["NM103_OtherPayerOrganizationName"].ToString().Trim() != "")
                        {
                            //NM1 - Other Payer Name
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\NM1"));
                            oSegment.set_DataElementValue(1, 0, "PR"); 		// Entity Identifier Code (98) 
                            oSegment.set_DataElementValue(2, 0, "2"); 		// Entity Type Qualifier (1065) 
                            oSegment.set_DataElementValue(3, 0, oOtherSubscriberInfoRow["NM103_OtherPayerOrganizationName"].ToString()); 		// Name Last or Organization Name (1035) 
                            oSegment.set_DataElementValue(8, 0, oOtherSubscriberInfoRow["NM108_OtherPayerCodeQlfr"].ToString()); 		// Identification Code Qualifier (66) 
                            oSegment.set_DataElementValue(9, 0, oOtherSubscriberInfoRow["NM109_OtherPayerPrimaryID"].ToString()); 		// Identification Code (67) 

                            //N3 - Other Payer Address
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\N3"));
                            oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["N301_OtherPayerAddress1"].ToString()); 		// Address Information (166) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["N302_OtherPayerAddress2"].ToString()); 		// Address Information (166) 

                            //N4 - Other Payer City, State, ZIP Code
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\N4"));
                            oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["N401_OtherPayerCity"].ToString()); 		// City Name (19) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["N402_OtherPayerState"].ToString()); 		// State or Province Code (156) 
                            oSegment.set_DataElementValue(3, 0, oOtherSubscriberInfoRow["N403_OtherPayerZip"].ToString()); 		// Postal Code (116) 
                            oSegment.set_DataElementValue(4, 0, oOtherSubscriberInfoRow["N404_OtherPayerCountryCode"].ToString()); 		// Country Code (26) 
                            oSegment.set_DataElementValue(7, 0, oOtherSubscriberInfoRow["N407_OtherPayerCountrySubdivision"].ToString()); 		// Country Subdivision Code (1715) 

                            //DTP - Claim Check or Remittance Date
                            if (oOtherSubscriberInfoRow["DTP03_OtherPayerPaymentDate"].ToString().Trim() != "")
                            {
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\DTP"));
                                oSegment.set_DataElementValue(1, 0, "573"); 		// Date/Time Qualifier (374) 
                                oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                                oSegment.set_DataElementValue(3, 0, oOtherSubscriberInfoRow["DTP03_OtherPayerPaymentDate"].ToString()); 		// Date Time Period (1251) 
                            }

                            //REF - Other Payer Secondary Identification
                            if (oOtherSubscriberInfoRow["REF02_OtherPayerSecondaryID"].ToString().Trim() != "")
                            {
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\REF"));
                                oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["REF01_OtherPayerSecondaryIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                                oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["REF02_OtherPayerSecondaryID"].ToString()); 		// Reference Identification (127) 
                            }

                            //REF - Other Payer Prior Authorization Number
                            if (oOtherSubscriberInfoRow["REF02_OtherPayerPriorAuthorizationNo"].ToString().Trim() != "")
                            {
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\REF"));
                                oSegment.set_DataElementValue(1, 0, "G1"); 		// Reference Identification Qualifier (128) 
                                oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["REF02_OtherPayerPriorAuthorizationNo"].ToString()); 		// Reference Identification (127) 
                            }

                            //REF - Other Payer Referral Number
                            if (oOtherSubscriberInfoRow["REF02_OtherPayerReferralNo"].ToString().Trim() != "")
                            {
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\REF"));
                                oSegment.set_DataElementValue(1, 0, "9F"); 		// Reference Identification Qualifier (128) 
                                oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["REF02_OtherPayerReferralNo"].ToString()); 		// Reference Identification (127) 
                            }

                            //REF - Other Payer Claim Adjustment Indicator
                            if (oOtherSubscriberInfoRow["REF02_OtherPayerClaimAdjustmentIndicator"].ToString().Trim() != "")
                            {
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\REF"));
                                oSegment.set_DataElementValue(1, 0, "T4"); 		// Reference Identification Qualifier (128) 
                                oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["REF02_OtherPayerClaimAdjustmentIndicator"].ToString()); 		// Reference Identification (127) 
                            }

                            //REF - Other Payer Claim Control Number
                            if (oOtherSubscriberInfoRow["REF02_OtherPayerClaimControlNo"].ToString().Trim() != "")
                            {
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\REF"));
                                oSegment.set_DataElementValue(1, 0, "F8"); 		// Reference Identification Qualifier (128) 
                                oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["REF02_OtherPayerClaimControlNo"].ToString()); 		// Reference Identification (127) 
                            }

                        }//2330B OTHER PAYER


                        //2330C OTHER PAYER REFERRING PROVIDER 
                        if (oOtherSubscriberInfoRow["REF02_OtherReferringProviderID"].ToString().Trim() != "")
                        {
                            //NM1 - Other Payer Referring Provider
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\NM1"));
                            oSegment.set_DataElementValue(1, 0, "DN"); 		// Entity Identifier Code (98) 
                            oSegment.set_DataElementValue(2, 0, "1"); 		// Entity Type Qualifier (1065) 

                            ///REF - Other Payer Referring Provider Secondary Identification
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\REF"));
                            oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["REF01_OtherReferringProviderIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["REF02_OtherReferringProviderID"].ToString()); 		// Reference Identification (127) 

                        }//2330C OTHER PAYER REFERRING PROVIDER


                        //2330C OTHER PAYER REFERRING PROVIDER - Primary Care Provider
                        if (oOtherSubscriberInfoRow["REF02_OtherPrimaryCareProviderID"].ToString().Trim() != "")
                        {
                            //NM1 - Other Payer Referring Provider - Primary Care Provider
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\NM1"));
                            oSegment.set_DataElementValue(1, 0, "P3"); 		// Entity Identifier Code (98) 
                            oSegment.set_DataElementValue(2, 0, "1"); 		// Entity Type Qualifier (1065) 

                            //REF - Other Payer Referring Provider Secondary Identification - Primary Care Provider
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\REF"));
                            oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["REF01_OtherPrimaryCareProviderIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["REF02_OtherPrimaryCareProviderID"].ToString()); 		// Reference Identification (127) 

                        }//2330C OTHER PAYER REFERRING PROVIDER - Primary Care Provider


                        //2330D OTHER PAYER RENDERING PROVIDER
                        if (oOtherSubscriberInfoRow["REF02_OtherRenderingProviderID"].ToString().Trim() != "")
                        {
                            //NM1 - Other Payer Rendering Provider
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\NM1"));
                            oSegment.set_DataElementValue(1, 0, "82"); 		// Entity Identifier Code (98) 
                            oSegment.set_DataElementValue(2, 0, "1"); 		// Entity Type Qualifier (1065) 

                            //REF - Other Payer Rendering Provider Secondary Identification
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\REF"));
                            oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["REF01_OtherRenderingProviderIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["REF02_OtherRenderingProviderID"].ToString()); 		// Reference Identification (127) 

                        }//2330D OTHER PAYER RENDERING PROVIDER


                        //2330E OTHER PAYER SERVICE FACILITY LOCATION
                        if (oOtherSubscriberInfoRow["REF02_OtherServiceLocationID"].ToString().Trim() != "")
                        {
                            //NM1 - Other Payer Service Facility Location
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\NM1"));
                            oSegment.set_DataElementValue(1, 0, "77"); 		// Entity Identifier Code (98) 
                            oSegment.set_DataElementValue(2, 0, "2"); 		// Entity Type Qualifier (1065) 

                            //REF - Other Payer Service Facility Location Secondary Identification
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\REF"));
                            oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["REF01_OtherServiceLocationIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["REF02_OtherServiceLocationID"].ToString()); 		// Reference Identification (127) 

                        }//2330E OTHER PAYER SERVICE FACILITY LOCATION


                        //2330F OTHER PAYER SUPERVISING PROVIDER
                        if (oOtherSubscriberInfoRow["REF02_OtherSupervisingPhysicianID"].ToString().Trim() != "")
                        {
                            //NM1 - Other Payer Supervising Provider
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\NM1"));
                            oSegment.set_DataElementValue(1, 0, "DQ"); 		// Entity Identifier Code (98) 
                            oSegment.set_DataElementValue(2, 0, "1"); 		// Entity Type Qualifier (1065) 

                            //REF - Other Payer Supervising Provider Secondary Identification
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\REF"));
                            oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["REF01_OtherSupervisingPhysicianIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["REF02_OtherSupervisingPhysicianID"].ToString()); 		// Reference Identification (127) 

                        }//2330F OTHER PAYER SUPERVISING PROVIDER


                        //2330G OTHER PAYER BILLING PROVIDER
                        if (oOtherSubscriberInfoRow["REF02_OtherBillingProviderID"].ToString().Trim() != "")
                        {
                            //NM1 - Other Payer Billing Provider
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\NM1"));
                            oSegment.set_DataElementValue(1, 0, "85"); 		// Entity Identifier Code (98) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["NM102_OtherBillingProvideEntityTypeQlfr"].ToString()); 		// Entity Type Qualifier (1065) 

                            //REF - Other Payer Billing Provider Secondary Identification
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\SBR\\NM1\\REF"));
                            oSegment.set_DataElementValue(1, 0, oOtherSubscriberInfoRow["REF01_OtherBillingProviderIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                            oSegment.set_DataElementValue(2, 0, oOtherSubscriberInfoRow["REF02_OtherBillingProviderID"].ToString()); 		// Reference Identification (127) 
                        
                        }//2330G OTHER PAYER BILLING PROVIDER


                    }//if (oOtherSubscriberInfoRow["SBR01_PayerResponsibSeqNoCode"].ToString().Trim() != "")

                }//2320 OTHER SUBSCRIBER INFORMATION foreach oOtherSubscriberInfoRow 


                //2400 SERVICE LINE 
                sSql = "select * from [837X222_ServiceLine] where Claimskey = " + oClaimsRow["Claimskey"].ToString();
                SqlDataAdapter oServiceLineAdapter = new SqlDataAdapter(sSql, oConnection);
                DataSet oServiceLineDs = new DataSet("dsServiceLine");
                oServiceLineAdapter.Fill(oServiceLineDs, "dsServiceLine");

                foreach (DataRow oServiceLineRow in oServiceLineDs.Tables["dsServiceLine"].Rows)
                {
                
                    //LX - SERVICE LINE COUNTER
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\LX"));
                    oSegment.set_DataElementValue(1, 0, oServiceLineRow["LX01_AssignedNumber"].ToString());      //Assigned Number

                    //SV1 - PROFESSIONAL SERVICE
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\SV1"));
                    oSegment.set_DataElementValue(1, 1, oServiceLineRow["SV101_01_ProductServiceIdQualifier"].ToString()); 		// Product/Service ID Qualifier (235) 
                    oSegment.set_DataElementValue(1, 2, oServiceLineRow["SV101_02_ProcedureCode"].ToString()); 		// Product/Service ID (234) 
                    oSegment.set_DataElementValue(1, 3, oServiceLineRow["SV101_03_ProcedureModifier1"].ToString()); 		// Procedure Modifier (1339) 
                    oSegment.set_DataElementValue(1, 4, oServiceLineRow["SV101_04_ProcedureModifier2"].ToString()); 		// Procedure Modifier (1339) 
                    oSegment.set_DataElementValue(1, 5, oServiceLineRow["SV101_05_ProcedureModifier3"].ToString()); 		// Procedure Modifier (1339) 
                    oSegment.set_DataElementValue(1, 6, oServiceLineRow["SV101_06_ProcedureModifier4"].ToString()); 		// Procedure Modifier (1339) 
                    oSegment.set_DataElementValue(1, 7, oServiceLineRow["SV101_07_ServiceDescription"].ToString()); 		// Description (352) 
                    oSegment.set_DataElementValue(2, 0, oServiceLineRow["SV102_LineItemChargeAmount"].ToString()); 		// Monetary Amount (782) 
                    oSegment.set_DataElementValue(3, 0, oServiceLineRow["SV103_UnitForMeasurement_Code"].ToString()); 		// Unit or Basis for Measurement Code (355) 
                    oSegment.set_DataElementValue(4, 0, oServiceLineRow["SV104_ServiceUnitCount"].ToString()); 		// Quantity (380) 
                    oSegment.set_DataElementValue(5, 0, oServiceLineRow["SV105_PlaceOfServiceCode"].ToString()); 		// Facility Code Value (1331) 
                    oSegment.set_DataElementValue(7, 1, oServiceLineRow["SV107_01_DiagnosisCodePointer1"].ToString()); 		// Diagnosis Code Pointer (1328) 
                    oSegment.set_DataElementValue(7, 2, oServiceLineRow["SV107_02_DiagnosisCodePointer2"].ToString()); 		// Diagnosis Code Pointer (1328) 
                    oSegment.set_DataElementValue(7, 3, oServiceLineRow["SV107_03_DiagnosisCodePointer3"].ToString()); 		// Diagnosis Code Pointer (1328) 
                    oSegment.set_DataElementValue(7, 4, oServiceLineRow["SV107_04_DiagnosisCodePointer4"].ToString()); 		// Diagnosis Code Pointer (1328) 
                    oSegment.set_DataElementValue(9, 0, oServiceLineRow["SV109_EmergencyIndicator"].ToString()); 		// Yes/No Condition or Response Code (1073) 
                    oSegment.set_DataElementValue(11, 0, oServiceLineRow["SV111_EPSDT_Indicator"].ToString()); 		// Yes/No Condition or Response Code (1073) 
                    oSegment.set_DataElementValue(12, 0, oServiceLineRow["SV112_FamilyPlanningIndicator"].ToString()); 		// Yes/No Condition or Response Code (1073) 
                    oSegment.set_DataElementValue(15, 0, oServiceLineRow["SV115_CopayStatusCode"].ToString()); 		// Copay Status Code (1327) 

                    //SV5 - Durable Medical Equipment Service DURABLE MEDICAL EQUIPMENT SERVICE
                    if (oServiceLineRow["SV506_RentalUnitPriceInidcator"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\SV5"));
                        oSegment.set_DataElementValue(1, 1, oServiceLineRow["SV501_01_ProcedureIdentifier"].ToString()); 		// Product/Service ID Qualifier (235) 
                        oSegment.set_DataElementValue(1, 2, oServiceLineRow["SV501_02_ProcedureCode"].ToString()); 		// Product/Service ID (234) 
                        oSegment.set_DataElementValue(2, 0, "DA"); 		// Unit or Basis for Measurement Code (355) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["SV503_DaysLengthOfMedicalNecissity"].ToString()); 		// Quantity (380) 
                        oSegment.set_DataElementValue(4, 0, oServiceLineRow["SV504_DME_RentalPrice"].ToString()); 		// Monetary Amount (782) 
                        oSegment.set_DataElementValue(5, 0, oServiceLineRow["SV505_DME_PurchasePrice"].ToString()); 		// Monetary Amount (782) 
                        oSegment.set_DataElementValue(6, 0, oServiceLineRow["SV506_RentalUnitPriceInidcator"].ToString()); 		// Frequency Code (594) 
                    }


                    //PWK - Line Supplemental Information 
                    if (oServiceLineRow["PWK02_LineSupplementAttachTransmissnCode"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\PWK"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["PWK01_LineSupplementAttachReportTypeCode"].ToString()); 		// Report Type Code (755) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["PWK02_LineSupplementAttachTransmissnCode"].ToString()); 		// Report Transmission Code (756) 
                        oSegment.set_DataElementValue(5, 0, "AC"); 		// Identification Code Qualifier (66) 
                        oSegment.set_DataElementValue(6, 0, oServiceLineRow["PWK06_LineSupplementAttachControlNo"].ToString()); 		// Identification Code (67) 
                    }

                    //PWK - Durable Medical Equipment Certificate of Medical 
                    if (oServiceLineRow["PWK02_DMERC_CMN_AttachTransmissnCode"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\PWK"));
                        oSegment.set_DataElementValue(1, 0, "CT"); 		// Report Type Code (755) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["PWK02_DMERC_CMN_AttachTransmissnCode"].ToString()); 		// Report Transmission Code (756) 
                    }

                    //CR1 - Ambulance Transport Information 
                    if (oServiceLineRow["CR102_PatientWeightLbs"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\CR1"));
                        oSegment.set_DataElementValue(1, 0, "LB"); 		// Unit or Basis for Measurement Code (355) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["CR102_PatientWeightLbs"].ToString()); 		// Weight (81) 
                        oSegment.set_DataElementValue(4, 0, oServiceLineRow["CR104_AmbulanceTransportReasonCode"].ToString()); 		// Ambulance Transport Reason Code (1317) 
                        oSegment.set_DataElementValue(5, 0, "DH"); 		// Unit or Basis for Measurement Code (355) 
                        oSegment.set_DataElementValue(6, 0, oServiceLineRow["CR106_TransportDistanceMiles"].ToString()); 		// Quantity (380) 
                        oSegment.set_DataElementValue(9, 0, oServiceLineRow["CR109_RoundTripPurposeDescription"].ToString()); 		// Description (352) 
                        oSegment.set_DataElementValue(10, 0, oServiceLineRow["CR110_StretcherPurposeDescription"].ToString()); 		// Description (352) 
                    }

                    //CR3 - Durable Medical Equipment Certification
                    if (oServiceLineRow["CR303_DME_DurationMonths"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\CR3"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["CR301_DMERC_CertificationTypeCode"].ToString()); 		// Certification Type Code (1322) 
                        oSegment.set_DataElementValue(2, 0, "MO"); 		// Unit or Basis for Measurement Code (355) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["CR303_DME_DurationMonths"].ToString()); 		// Quantity (380) 
                    }


                    //CRC - Ambulance Certification
                    if (oServiceLineRow["CRC02_AmbulanceCertConditionIndicator"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\CRC"));
                        oSegment.set_DataElementValue(1, 0, "07"); 		// Code Category (1136) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["CRC02_AmbulanceCertConditionIndicator"].ToString()); 		// Yes/No Condition or Response Code (1073) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["CRC03_AmbulanceCertConditionCode1"].ToString()); 		// Condition Indicator (1321) 
                        oSegment.set_DataElementValue(4, 0, oServiceLineRow["CRC04_AmbulanceCertConditionCode2"].ToString()); 		// Condition Indicator (1321) 
                        oSegment.set_DataElementValue(5, 0, oServiceLineRow["CRC05_AmbulanceCertConditionCode3"].ToString()); 		// Condition Indicator (1321) 
                        oSegment.set_DataElementValue(6, 0, oServiceLineRow["CRC06_AmbulanceCertConditionCode4"].ToString()); 		// Condition Indicator (1321) 
                        oSegment.set_DataElementValue(7, 0, oServiceLineRow["CRC07_AmbulanceCertConditionCode5"].ToString()); 		// Condition Indicator (1321) 
                    }

                    //CRC - Hospice Employee Indicator 
                    if (oServiceLineRow["CRC02_HospiceEmployedProviderIndicator"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\CRC"));
                        oSegment.set_DataElementValue(1, 0, "70"); 		// Code Category (1136) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["CRC02_HospiceEmployedProviderIndicator"].ToString()); 		// Yes/No Condition or Response Code (1073) 
                        oSegment.set_DataElementValue(3, 0, "65"); 		// Condition Indicator (1321) 
                    }

                    //CRC - Condition Indicator/Durable Medical Equipment
                    if (oServiceLineRow["CRC02_DMERC_ConditionIndicator"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\CRC"));
                        oSegment.set_DataElementValue(1, 0, "09"); 		// Code Category (1136) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["CRC02_DMERC_ConditionIndicator"].ToString()); 		// Yes/No Condition or Response Code (1073) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["CRC03_DMERC_ConditionCode1"].ToString()); 		// Condition Indicator (1321) 
                        oSegment.set_DataElementValue(4, 0, oServiceLineRow["CRC04_DMERC_ConditionCode2"].ToString()); 		// Condition Indicator (1321) 
                    }

                    //DTP - SERVICE DATE 
                    if (oServiceLineRow["DTP03_ServiceDate"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\DTP"));
                        oSegment.set_DataElementValue(1, 0, "472");         // Date/Time Qualifier (374) 

                        if (oServiceLineRow["DTP03_ServiceDate"].ToString().Contains("-"))
                        {
                            oSegment.set_DataElementValue(2, 0, "RD8"); 		// Date Time Period Format Qualifier (1250) 
                        }
                        else
                        {
                            oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                        }
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["DTP03_ServiceDate"].ToString()); 		// Date Time Period (1251) 
                    }

                    //DTP - PRESCRIPTION DATE
                    if (oServiceLineRow["DTP03_PrescriptionDate"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\DTP"));
                        oSegment.set_DataElementValue(1, 0, "471"); 		// Date/Time Qualifier (374) 
                        oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["DTP03_PrescriptionDate"].ToString()); 		// Date Time Period (1251) 
                    }

                    //DTP - Date - Certification Revision/Recertification Date
                    if (oServiceLineRow["DTP03_CertificationRevisionDate"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\DTP"));
                        oSegment.set_DataElementValue(1, 0, "607"); 		// Date/Time Qualifier (374) 
                        oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["DTP03_CertificationRevisionDate"].ToString()); 		// Date Time Period (1251) 
                    }

                    ///DTP - Date - Begin Therapy Date
                    if (oServiceLineRow["DTP03_BeginTherapyDate"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\DTP"));
                        oSegment.set_DataElementValue(1, 0, "463"); 		// Date/Time Qualifier (374) 
                        oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["DTP03_BeginTherapyDate"].ToString()); 		// Date Time Period (1251) 
                    }

                    //DTP - Date - Last Certification Date
                    if (oServiceLineRow["DTP03_LastCertificationDate"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\DTP"));
                        oSegment.set_DataElementValue(1, 0, "461"); 		// Date/Time Qualifier (374) 
                        oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["DTP03_LastCertificationDate"].ToString()); 		// Date Time Period (1251) 
                    }

                    //DTP - Date - Last Seen Date
                    if (oServiceLineRow["DTP03_LastSeenDate"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\DTP"));
                        oSegment.set_DataElementValue(1, 0, "304"); 		// Date/Time Qualifier (374) 
                        oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["DTP03_LastSeenDate"].ToString()); 		// Date Time Period (1251) 
                     }

                    //DTP - Date - Test Date
                    if (oServiceLineRow["DTP03_LastTestDate"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\DTP"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["DTP01_LastTestQualifier"].ToString()); 		// Date/Time Qualifier (374) 
                        oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["DTP03_LastTestDate"].ToString()); 		// Date Time Period (1251) 
                    }

                    //DTP - Date - Shipped Date
                    if (oServiceLineRow["DTP03_ShippedDate"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\DTP"));
                        oSegment.set_DataElementValue(1, 0, "011"); 		// Date/Time Qualifier (374) 
                        oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["DTP03_ShippedDate"].ToString()); 		// Date Time Period (1251) 
                      }

                    //DTP - Date - Last X-ray Date
                    if (oServiceLineRow["DTP03_LastXrayDate"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\DTP"));
                        oSegment.set_DataElementValue(1, 0, "455"); 		// Date/Time Qualifier (374) 
                        oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["DTP03_LastXrayDate"].ToString()); 		// Date Time Period (1251) 
                    }

                    //DTP - Date - Initial Treatment Date
                    if (oServiceLineRow["DTP03_InitialTreatmentDate"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\DTP"));
                        oSegment.set_DataElementValue(1, 0, "454"); 		// Date/Time Qualifier (374) 
                        oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["DTP03_InitialTreatmentDate"].ToString()); 		// Date Time Period (1251) 
                    }

                    //QTY - Ambulance Patient Count  
                    if (oServiceLineRow["QTY02_AmbulancePatientCount"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\QTY"));
                        oSegment.set_DataElementValue(1, 0, "PT"); 		// Quantity Qualifier (673) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["QTY02_AmbulancePatientCount"].ToString()); 		// Quantity (380) 
                    }

                    //QTY - Obstetric Anesthesia Additional Units  
                    if (oServiceLineRow["QTY02_ObstetricAdditionalUnits"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\QTY"));
                        oSegment.set_DataElementValue(1, 0, "FL"); 		// Quantity Qualifier (673) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["QTY02_ObstetricAdditionalUnits"].ToString()); 		// Quantity (380) 
                    }

                    //MEA - Test Result 
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\MEA"));
                    oSegment.set_DataElementValue(1, 0, oServiceLineRow["MEA01_TestResultReferenceIdCode"].ToString()); 		// Measurement Reference ID Code (737) 
                    oSegment.set_DataElementValue(2, 0, oServiceLineRow["MEA02_TestResultQualifier"].ToString()); 		// Measurement Qualifier (738) 
                    oSegment.set_DataElementValue(3, 0, oServiceLineRow["MEA03_TestResultValue"].ToString()); 		// Measurement Value (739) 

                    //CN1 - Contract Information 
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\CN1"));
                    oSegment.set_DataElementValue(1, 0, oServiceLineRow["CN101_ContractTypeCode"].ToString()); 		// Contract Type Code (1166) 
                    oSegment.set_DataElementValue(2, 0, oServiceLineRow["CN102_ContractAmount"].ToString()); 		// Monetary Amount (782) 
                    oSegment.set_DataElementValue(3, 0, oServiceLineRow["CN103_ContractPercentage"].ToString()); 		// Percent (332) 
                    oSegment.set_DataElementValue(4, 0, oServiceLineRow["CN104_ContractCode"].ToString()); 		// Reference Identification (127) 
                    oSegment.set_DataElementValue(5, 0, oServiceLineRow["CN105_ContractTermsDiscPercent"].ToString()); 		// Terms Discount Percent (338) 
                    oSegment.set_DataElementValue(6, 0, oServiceLineRow["CN106_ContractVersionIdentifier"].ToString()); 		// Version Identifier (799) 


                    //REF - Repriced Line Item Reference Number 
                    if (oServiceLineRow["REF02_RepricedLineItemRefNo"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\REF"));
                        oSegment.set_DataElementValue(1, 0, "9B"); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_RepricedLineItemRefNo"].ToString()); 		// Reference Identification (127) 
                    }

                    //REF - Adjusted Repriced Line Item Reference Number
                    if (oServiceLineRow["REF02_AdjustedRepricedLineItemRefNo"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\REF"));
                        oSegment.set_DataElementValue(1, 0, "9D"); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_AdjustedRepricedLineItemRefNo"].ToString()); 		// Reference Identification (127) 
                      }

                    //REF - Prior Authorization
                    if (oServiceLineRow["REF02_PriorAuthorizationNumber"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\REF"));
                        oSegment.set_DataElementValue(1, 0, "G1"); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_PriorAuthorizationNumber"].ToString()); 		// Reference Identification (127) 
                        oSegment.set_DataElementValue(4, 1, "2U"); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(4, 2, oServiceLineRow["REF04_02_PriorAuthorizOtherPayrPrimaryID"].ToString()); 		// Reference Identification (127) 
                    }

                    //REF - Line Item Control Number
                    if (oServiceLineRow["REF02_LineItemControlNumber"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\REF"));
                        oSegment.set_DataElementValue(1, 0, "6R"); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_LineItemControlNumber"].ToString()); 		// Reference Identification (127) 
                    }
                    //REF - Mammography Certification Number
                    if (oServiceLineRow["REF02_MammographyCertificationNumber"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\REF"));
                        oSegment.set_DataElementValue(1, 0, "EW"); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_MammographyCertificationNumber"].ToString()); 		// Reference Identification (127) 
                    }

                    //REF - Clinical Laboratory Improvement Amendment (CLIA)
                    if (oServiceLineRow["REF02_ClinicalLabImproveAmendment"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\REF"));
                        oSegment.set_DataElementValue(1, 0, "X4"); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_ClinicalLabImproveAmendment"].ToString()); 		// Reference Identification (127) 
                      }

                    //REF - Referring Clinical Laboratory Improvement Amendment
                    if (oServiceLineRow["REF02_ReferringCLIA_Number"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\REF"));
                        oSegment.set_DataElementValue(1, 0, "F4"); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_ReferringCLIA_Number"].ToString()); 		// Reference Identification (127) 
                    }

                    //REF - Immunization Batch Number
                    if (oServiceLineRow["REF02_ImmunizationBatchNumber"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\REF"));
                        oSegment.set_DataElementValue(1, 0, "BT"); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_ImmunizationBatchNumber"].ToString()); 		// Reference Identification (127) 
                    }

                    //REF - Referral Number
                    if (oServiceLineRow["REF02_ReferralNumber"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\REF"));
                        oSegment.set_DataElementValue(1, 0, "9F"); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_ReferralNumber"].ToString()); 		// Reference Identification (127) 
                        oSegment.set_DataElementValue(4, 1, "2U"); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(4, 2, oServiceLineRow["REF04_02_ReferralOtherPayerPrimaryID"].ToString()); 		// Reference Identification (127) 
                    }

                    //AMT - Sales Tax Amount
                    if (oServiceLineRow["AMT02_SalesTaxAmount"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\AMT"));
                        oSegment.set_DataElementValue(1, 0, "T"); 		// Amount Qualifier Code (522) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["AMT02_SalesTaxAmount"].ToString()); 		// Monetary Amount (782) 
                    }

                    //AMT - Postage Claimed Amount
                    if (oServiceLineRow["AMT02_PostageClaimedAmount"].ToString().Trim() != "")
                    {
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\AMT"));
                        oSegment.set_DataElementValue(1, 0, "F4"); 		// Amount Qualifier Code (522) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["AMT02_PostageClaimedAmount"].ToString()); 		// Monetary Amount (782) 
                    }

                    //K3 - File Information 
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\K3"));
                    oSegment.set_DataElementValue(1, 0, oServiceLineRow["K301_FileInformation"].ToString()); 		// Fixed Format Information (449) 

                    //NTE - Line Note
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NTE(1)"));
                    oSegment.set_DataElementValue(1, 0, oServiceLineRow["NTE01_LineNoteReferenceCode"].ToString()); 		// Note Reference Code (363) 
                    oSegment.set_DataElementValue(2, 0, oServiceLineRow["NTE02_LineNoteText"].ToString()); 		// Description (352) 

                    //NTE - Third Party Organization Notes 
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NTE(2)"));
                    oSegment.set_DataElementValue(1, 0, oServiceLineRow["NTE01_ThirdPartyNoteCode"].ToString()); 		// Note Reference Code (363) 
                    oSegment.set_DataElementValue(2, 0, oServiceLineRow["NTE02_ThirdPartyText"].ToString()); 		// Description (352) 

                    //PS1 - Purchased Service Information 
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\PS1"));
                    oSegment.set_DataElementValue(1, 0, oServiceLineRow["PS101_PurchasedServiceProviderIdfr"].ToString()); 		// Reference Identification (127) 
                    oSegment.set_DataElementValue(2, 0, oServiceLineRow["PS102_PurchasedServiceChargeAmnt"].ToString()); 		// Monetary Amount (782) 
                    // oSegment.set_DataElementValue(3, 0, "CA"); 		// State or Province Code (156)  <<<User Requirement: NOT USED>>> 

                    //HCP - Line Pricing/Repricing Information 
                    ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\HCP"));
                    oSegment.set_DataElementValue(1, 0, oServiceLineRow["HCP01_LineRepriceCode"].ToString()); 		// Pricing Methodology (1473) 
                    oSegment.set_DataElementValue(2, 0, oServiceLineRow["HCP02_RepricedAllowedAmount"].ToString()); 		// Monetary Amount (782) 
                    oSegment.set_DataElementValue(3, 0, oServiceLineRow["HCP03_RepricedSavingAmount"].ToString()); 		// Monetary Amount (782) 
                    oSegment.set_DataElementValue(4, 0, oServiceLineRow["HCP04_RepricingOrganizationID"].ToString()); 		// Reference Identification (127) 
                    oSegment.set_DataElementValue(5, 0, oServiceLineRow["HCP05_RepricingPerDiemFlatRateAmount"].ToString()); 		// Rate (118) 
                    oSegment.set_DataElementValue(6, 0, oServiceLineRow["HCP06_RepricedApprovedAmbPatientGrpCode"].ToString()); 		// Reference Identification (127) 
                    oSegment.set_DataElementValue(7, 0, oServiceLineRow["HCP07_RepricedApprovedAmbPatientGroupAmnt"].ToString()); 		// Monetary Amount (782) 
                    oSegment.set_DataElementValue(9, 0, oServiceLineRow["HCP09_RepricedServiceIdQualifier"].ToString()); 		// Product/Service ID Qualifier (235) 
                    oSegment.set_DataElementValue(10, 0, oServiceLineRow["HCP10_RepricedApprovedHCPCS_Code"].ToString()); 		// Product/Service ID (234) 
                    oSegment.set_DataElementValue(11, 0, oServiceLineRow["HCP11_RepricedUnitMeasurementCode"].ToString()); 		// Unit or Basis for Measurement Code (355) 
                    oSegment.set_DataElementValue(12, 0, oServiceLineRow["HCP12_RepricedApprovedServiceUnitCount"].ToString()); 		// Quantity (380) 
                    oSegment.set_DataElementValue(13, 0, oServiceLineRow["HCP13_RepricedRejectReasonCode"].ToString()); 		// Reject Reason Code (901) 
                    oSegment.set_DataElementValue(14, 0, oServiceLineRow["HCP14_RepricedPolicyComplianceCode"].ToString()); 		// Policy Compliance Code (1526) 
                    oSegment.set_DataElementValue(15, 0, oServiceLineRow["HCP15_RepricedExceptionCode"].ToString()); 		// Exception Code (1527) 

                    //2410 DRUG IDENTIFICATION
                    if (oServiceLineRow["LIN03_NationalDrugCode"].ToString().Trim() != "")
                    {
                        //LIN - Item Identification
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\LIN\\LIN"));
                        oSegment.set_DataElementValue(2, 0, "N4"); 		// Product/Service ID Qualifier (235) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["LIN03_NationalDrugCode"].ToString()); 		// Product/Service ID (234) 

                        //CTP - Drug Quantity  
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\LIN\\CTP"));
                        oSegment.set_DataElementValue(4, 0, oServiceLineRow["CTP04_NationalDrugUnitCount"].ToString()); 		// Quantity (380) 
                        oSegment.set_DataElementValue(5, 1, oServiceLineRow["CTP05_01_UnitMeasurementCode"].ToString()); 		// Unit or Basis for Measurement Code (355) 

                        //REF - Prescription or Compound Drug Association Number
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\LIN\\REF"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["REF01_PrescriptionQualifier"].ToString()); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_PrescriptionNumber"].ToString()); 		// Reference Identification (127) 

                    }//2410 DRUG IDENTIFICATION

                    
                    //2420A RENDERING PROVIDER
                    if (oServiceLineRow["NM103_RenderingProviderNameLastOrg"].ToString().Trim() != "")
                    {
                        //NM1 - Rendering Provider Name
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\NM1"));
                        oSegment.set_DataElementValue(1, 0, "82"); 		// Entity Identifier Code (98) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["NM102_RenderingProviderEntityTypeQlfr"].ToString()); 		// Entity Type Qualifier (1065) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["NM103_RenderingProviderNameLastOrg"].ToString()); 		// Name Last or Organization Name (1035) 
                        oSegment.set_DataElementValue(4, 0, oServiceLineRow["NM104_RenderingProviderFirst"].ToString()); 		// Name First (1036) 
                        oSegment.set_DataElementValue(5, 0, oServiceLineRow["NM105_RenderingProviderMiddle"].ToString()); 		// Name Middle (1037) 
                        oSegment.set_DataElementValue(7, 0, oServiceLineRow["NM107_RenderingProviderSuffix"].ToString()); 		// Name Suffix (1039) 
                        if (oServiceLineRow["NM109_RenderingProviderID"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(8, 0, "XX"); 		// Identification Code Qualifier (66) 
                            oSegment.set_DataElementValue(9, 0, oServiceLineRow["NM109_RenderingProviderID"].ToString()); 		// Identification Code (67) 
                        }

                        //PRV - Rendering Provider Specialty Information
                        if (oServiceLineRow["PRV03_RenderingProviderTaxonomyCode"].ToString().Trim() != "")
                        {
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\PRV"));
                            oSegment.set_DataElementValue(1, 0, "PE"); 		// Provider Code (1221) 
                            oSegment.set_DataElementValue(2, 0, "PXC"); 		// Reference Identification Qualifier (128) 
                            oSegment.set_DataElementValue(3, 0, oServiceLineRow["PRV03_RenderingProviderTaxonomyCode"].ToString()); 		// Reference Identification (127) 
                        }

                        //REF - Rendering Provider Secondary Identification
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\REF"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["REF01_RenderingProviderSecondaryQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_RenderingProviderSecondaryID"].ToString()); 		// Reference Identification (127) 
                        oSegment.set_DataElementValue(4, 1, oServiceLineRow["REF04_01_ReferenceIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(4, 2, oServiceLineRow["REF04_02_RenderingProviderSecondaryPayerID"].ToString()); 		// Reference Identification (127) 

                    }//2420A RENDERING PROVIDER
                    

                    //2420B PURCHASED SERVICE PROVIDER
                    if (oServiceLineRow["NM109_PurchasedServiceProviderID"].ToString().Trim() != "")
                    {
                        //NM1 - Purchased Service Provider Name
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\NM1"));
                        oSegment.set_DataElementValue(1, 0, "QB"); 		// Entity Identifier Code (98) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["NM102_PurchasedServiceProviderEntityType"].ToString()); 		// Entity Type Qualifier (1065) 
                        if (oServiceLineRow["NM109_PurchasedServiceProviderID"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(8, 0, "XX"); 		// Identification Code Qualifier (66) 
                            oSegment.set_DataElementValue(9, 0, oServiceLineRow["NM109_PurchasedServiceProviderID"].ToString()); 		// Identification Code (67) 
                        }

                        //REF - Purchased Service Provider Secondary Identification
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\REF"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["REF01_PurchasedServicProvidrSecondryIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_PurchasedServiceProviderSecondaryID"].ToString()); 		// Reference Identification (127) 
                        if (oServiceLineRow["REF04_02_PurchsdServcProvdrSecndryPayrIdNo"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(4, 1, "2U"); 		// Reference Identification Qualifier (128) 
                            oSegment.set_DataElementValue(4, 2, oServiceLineRow["REF04_02_PurchsdServcProvdrSecndryPayrIdNo"].ToString()); 		// Reference Identification (127) 
                        }
                        
                    }//2420B PURCHASED SERVICE PROVIDER

                                        
                    //2420C SERVICE FACILITY
                    if (oServiceLineRow["NM103_ServiceFacilityName"].ToString().Trim() != "")
                    {
                        //NM1 - Service Facility Location Name
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\NM1"));
                        oSegment.set_DataElementValue(1, 0, "77"); 		// Entity Identifier Code (98) 
                        oSegment.set_DataElementValue(2, 0, "2"); 		// Entity Type Qualifier (1065) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["NM103_ServiceFacilityName"].ToString()); 		// Name Last or Organization Name (1035) 
                        if (oServiceLineRow["NM109_ServiceFacilityID"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(8, 0, "XX"); 		// Identification Code Qualifier (66) 
                            oSegment.set_DataElementValue(9, 0, oServiceLineRow["NM109_ServiceFacilityID"].ToString()); 		// Identification Code (67) 
                        }

                        //N3 - Service Facility Location Address  
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\N3"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["N301_ServiceFacilityAddress1"].ToString()); 		// Address Information (166) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["N302_ServiceFacilityAddress2"].ToString()); 		// Address Information (166) 

                        //N4 - Service Facility Location City, State, ZIP Code 
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\N4"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["N401_ServiceFacilityCity"].ToString()); 		// City Name (19) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["N402_ServiceFacilityState"].ToString()); 		// State or Province Code (156) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["N403_ServiceFacilityZip"].ToString()); 		// Postal Code (116) 
                        oSegment.set_DataElementValue(4, 0, oServiceLineRow["N404_ServiceFacilityCountryCode"].ToString()); 		// Country Code (26) 
                        oSegment.set_DataElementValue(7, 0, oServiceLineRow["N407_ServiceFacilityCountrySubdivision"].ToString()); 		// Country Subdivision Code (1715) 

                        //REF - Service Facility Location Secondary Identification
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\REF"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["REF01_ServiceFacilitySecondaryIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_ServiceFacilitySecondaryID"].ToString()); 		// Reference Identification (127) 
                        if (oServiceLineRow["REF04_02_ServiceFaciltySecondryPayrIdNo"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(4, 1, "2U"); 		// Reference Identification Qualifier (128) 
                            oSegment.set_DataElementValue(4, 2, oServiceLineRow["REF04_02_ServiceFaciltySecondryPayrIdNo"].ToString()); 		// Reference Identification (127) 
                        }
                        
                    }//2420C SERVICE FACILITY

                    
                    //2420D SUPERVISING PROVIDER
                    if (oServiceLineRow["NM103_SupervisingProviderLastName"].ToString().Trim() != "")
                    {
                        //NM1 - Supervising Provider Name
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\NM1"));
                        oSegment.set_DataElementValue(1, 0, "DQ"); 		// Entity Identifier Code (98) 
                        oSegment.set_DataElementValue(2, 0, "1"); 		// Entity Type Qualifier (1065) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["NM103_SupervisingProviderLastName"].ToString()); 		// Name Last or Organization Name (1035) 
                        oSegment.set_DataElementValue(4, 0, oServiceLineRow["NM104_SupervisingProviderFirst"].ToString()); 		// Name First (1036) 
                        oSegment.set_DataElementValue(5, 0, oServiceLineRow["NM105_SupervisingProviderMiddle"].ToString()); 		// Name Middle (1037) 
                        oSegment.set_DataElementValue(7, 0, oServiceLineRow["NM107_SupervisingProviderSuffix"].ToString()); 		// Name Suffix (1039) 
                        if (oServiceLineRow["NM109_SupervisingProviderID"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(8, 0, "XX"); 		// Identification Code Qualifier (66) 
                            oSegment.set_DataElementValue(9, 0, oServiceLineRow["NM109_SupervisingProviderID"].ToString()); 		// Identification Code (67) 
                        }

                        //REF - Supervising Provider Secondary Identification
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\REF"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["REF01_SupervisingProvidrSecondryIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_SupervisingProviderSecondaryID"].ToString()); 		// Reference Identification (127) 
                        if (oServiceLineRow["REF04_02_SupervisngProvdrSecndryPayrIdNo"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(4, 1, "2U"); 		// Reference Identification Qualifier (128) 
                            oSegment.set_DataElementValue(4, 2, oServiceLineRow["REF04_02_SupervisngProvdrSecndryPayrIdNo"].ToString()); 		// Reference Identification (127) 
                        }
                        
                    }//2420D SUPERVISING PROVIDER

                    
                    //2420E ORDERING PROVIDER
                    if (oServiceLineRow["NM103_OrderingProviderLastName"].ToString().Trim() != "")
                    {
                        //NM1 - Ordering Provider Location Name
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\NM1"));
                        oSegment.set_DataElementValue(1, 0, "DK"); 		// Entity Identifier Code (98) 
                        oSegment.set_DataElementValue(2, 0, "1"); 		// Entity Type Qualifier (1065) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["NM103_OrderingProviderLastName"].ToString()); 		// Name Last or Organization Name (1035) 
                        oSegment.set_DataElementValue(4, 0, oServiceLineRow["NM104_OrderingProviderFirst"].ToString()); 		// Name First (1036) 
                        oSegment.set_DataElementValue(5, 0, oServiceLineRow["NM105_OrderingProviderMiddle"].ToString()); 		// Name Middle (1037) 
                        oSegment.set_DataElementValue(7, 0, oServiceLineRow["NM107_OrderingProviderSuffix"].ToString()); 		// Name Suffix (1039) 
                        if (oServiceLineRow["NM109_OrderingProviderID"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(8, 0, "XX"); 		// Identification Code Qualifier (66) 
                            oSegment.set_DataElementValue(9, 0, oServiceLineRow["NM109_OrderingProviderID"].ToString()); 		// Identification Code (67) 
                        }

                        //N3 - Ordering Provider Location Address  
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\N3"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["N301_OrderingProviderAddress1"].ToString()); 		// Address Information (166) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["N302_OrderingProviderAddress2"].ToString()); 		// Address Information (166) 

                        //N4 - Ordering Provider Location City, State, ZIP Code 
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\N4"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["N401_OrderingProviderCity"].ToString()); 		// City Name (19) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["N402_OrderingProviderState"].ToString()); 		// State or Province Code (156) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["N403_OrderingProviderZip"].ToString()); 		// Postal Code (116) 
                        oSegment.set_DataElementValue(4, 0, oServiceLineRow["N404_OrderingProviderCountryCode"].ToString()); 		// Country Code (26) 
                        oSegment.set_DataElementValue(7, 0, oServiceLineRow["N407_OrderingProviderCountrySubdivision"].ToString()); 		// Country Subdivision Code (1715) 

                        //REF - Ordering Provider Location Secondary Identification
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\REF"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["REF01_OrderingProviderSecondaryIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_OrderingProviderSecondaryID"].ToString()); 		// Reference Identification (127) 
                        if (oServiceLineRow["REF04_02_OrderingProviderSecondaryIdNo"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(4, 1, "2U"); 		// Reference Identification Qualifier (128) 
                            oSegment.set_DataElementValue(4, 2, oServiceLineRow["REF04_02_OrderingProviderSecondaryIdNo"].ToString()); 		// Reference Identification (127) 
                        }

                        //PER - Ordering Provider Contact Information  
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\PER"));
                        nElemPos = 3;
                        if (oServiceLineRow["PER0X_OrderingProviderContactTelephone"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(nElemPos, 0, "TE");     //Communication Number Qualifier
                            oSegment.set_DataElementValue(nElemPos + 1, 0, oServiceLineRow["PER0X_OrderingProviderContactTelephone"].ToString());     //Communication Number
                            nElemPos = nElemPos + 2;
                        }

                        if (oServiceLineRow["PER0X_OrderingProviderContactExtension"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(nElemPos, 0, "EX");     //Communication Number Qualifier
                            oSegment.set_DataElementValue(nElemPos + 1, 0, oServiceLineRow["PER0X_OrderingProviderContactExtension"].ToString());     //Communication Number
                            nElemPos = nElemPos + 2;
                        }

                        if (oServiceLineRow["PER0X_OrderingProviderContactFax"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(nElemPos, 0, "FX");     //Communication Number Qualifier
                            oSegment.set_DataElementValue(nElemPos + 1, 0, oServiceLineRow["PER0X_OrderingProviderContactFax"].ToString());     //Communication Number
                            nElemPos = nElemPos + 2;
                        }

                        if (oServiceLineRow["PER0X_OrderingProviderContactEmail"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(nElemPos, 0, "EM");     //Communication Number Qualifier
                            oSegment.set_DataElementValue(nElemPos + 1, 0, oServiceLineRow["PER0X_OrderingProviderContactEmail"].ToString());     //Communication Number
                            nElemPos = nElemPos + 2;
                        }
                        if (nElemPos > 3)
                        {
                            oSegment.set_DataElementValue(1, 0, "IC");     //Contact Function Code
                            oSegment.set_DataElementValue(2, 0, oServiceLineRow["PER02_OrderingProviderContactName"].ToString());     //Name
                        }
                        
                    }//2420E ORDERING PROVIDER

                    
                    //2420F REFERRING PROVIDER
                    if (oServiceLineRow["NM103_ReferringProviderLastName"].ToString().Trim() != "")
                    {
                        //NM1 - Referring Provider Name
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\NM1"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["NM101_ReferringProviderEntityIdfr"].ToString()); 		// Entity Identifier Code (98) 
                        oSegment.set_DataElementValue(2, 0, "1"); 		// Entity Type Qualifier (1065) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["NM103_ReferringProviderLastName"].ToString()); 		// Name Last or Organization Name (1035) 
                        oSegment.set_DataElementValue(4, 0, oServiceLineRow["NM104_ReferringProviderFirst"].ToString()); 		// Name First (1036) 
                        oSegment.set_DataElementValue(5, 0, oServiceLineRow["NM105_ReferringProviderMiddle"].ToString()); 		// Name Middle (1037) 
                        oSegment.set_DataElementValue(7, 0, oServiceLineRow["NM107_ReferringProviderSuffix"].ToString()); 		// Name Suffix (1039) 
                        if (oServiceLineRow["NM109_ReferringProviderID"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(8, 0, "XX"); 		// Identification Code Qualifier (66) 
                            oSegment.set_DataElementValue(9, 0, oServiceLineRow["NM109_ReferringProviderID"].ToString()); 		// Identification Code (67) 
                        }

                        //REF - Referring Provider Secondary Identification
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\REF"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["REF01_ReferringProviderSecondaryIdQlfr"].ToString()); 		// Reference Identification Qualifier (128) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["REF02_ReferringProviderSecondaryID"].ToString()); 		// Reference Identification (127) 
                        if (oServiceLineRow["REF04_02_ReferngProvdrSecndryPayrIdNo"].ToString().Trim() != "")
                        {
                            oSegment.set_DataElementValue(4, 1, "2U"); 		// Reference Identification Qualifier (128) 
                            oSegment.set_DataElementValue(4, 2, oServiceLineRow["REF04_02_ReferngProvdrSecndryPayrIdNo"].ToString()); 		// Reference Identification (127) 
                        }
                        
                    }//2420F REFERRING PROVIDER

                    
                    //2420G AMBULANCE PICK-UP LOCATION
                    if (oServiceLineRow["N301_AmbulancePickupAddress1"].ToString().Trim() != "")
                    {
                        //NM1 - Ambulance Pick-up Location
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\NM1"));
                        oSegment.set_DataElementValue(1, 0, "PW"); 		// Entity Identifier Code (98) 
                        oSegment.set_DataElementValue(2, 0, "2"); 		// Entity Type Qualifier (1065) 

                        //N3 - Ambulance Pick-up Location Address
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\N3"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["N301_AmbulancePickupAddress1"].ToString()); 		// Address Information (166) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["N302_AmbulancePickupAddress2"].ToString()); 		// Address Information (166) 

                        //N4 - Ambulance Pick-up Location City, State, ZIP Code
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\N4"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["N401_AmbulancePickupCity"].ToString()); 		// City Name (19) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["N402_AmbulancePickupState"].ToString()); 		// State or Province Code (156) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["N403_AmbulancePickupZip"].ToString()); 		// Postal Code (116) 
                        oSegment.set_DataElementValue(4, 0, oServiceLineRow["N404_AmbulancePickupCountryCode"].ToString()); 		// Country Code (26) 
                        oSegment.set_DataElementValue(7, 0, oServiceLineRow["N407_AmbulncePickupCntrySubdivisn"].ToString()); 		// Country Subdivision Code (1715) 

                    }//2420G AMBULANCE PICK-UP LOCATION

                    
                    //2420H AMBULANCE DROP-OFF LOCATION
                    if (oServiceLineRow["N301_AmbulanceDropOffAddress1"].ToString().Trim() != "")
                    {
                        //NM1 - Ambulance Drop-off Location
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\NM1"));
                        oSegment.set_DataElementValue(1, 0, "45"); 		// Entity Identifier Code (98) 
                        oSegment.set_DataElementValue(2, 0, "2"); 		// Entity Type Qualifier (1065) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["NM103_AmbulanceDropOffName"].ToString()); 		// Name Last or Organization Name (1035) 

                        //N3 - Ambulance Drop-off Location Address 
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\N3"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["N301_AmbulanceDropOffAddress1"].ToString()); 		// Address Information (166) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["N302_AmbulanceDropOffAddress2"].ToString()); 		// Address Information (166) 

                        //N4 - Ambulance Drop-off Location City, State, ZIP Code
                        ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\NM1\\N4"));
                        oSegment.set_DataElementValue(1, 0, oServiceLineRow["N401_AmbulanceDropOffCity"].ToString()); 		// City Name (19) 
                        oSegment.set_DataElementValue(2, 0, oServiceLineRow["N402_AmbulanceDropOffState"].ToString()); 		// State or Province Code (156) 
                        oSegment.set_DataElementValue(3, 0, oServiceLineRow["N403_AmbulanceDropOffZip"].ToString()); 		// Postal Code (116) 
                        oSegment.set_DataElementValue(4, 0, oServiceLineRow["N404_AmbulanceDropOffCountryCode"].ToString()); 		// Country Code (26) 
                        oSegment.set_DataElementValue(7, 0, oServiceLineRow["N407_AmbulnceDropOffCntrySubdivisn"].ToString()); 		// Country Subdivision Code (1715) 

                    }//2420H AMBULANCE DROP-OFF LOCATION


                    //2430 LINE ADJUDICATION INFORMATION
                    sSql = "select * from [837X222_ServiceLineAdj] where ServiceLinekey = " + oServiceLineRow["ServiceLinekey"].ToString();
                    SqlDataAdapter oServiceLineAdjAdapter = new SqlDataAdapter(sSql, oConnection);
                    DataSet oServiceLineAdjDs = new DataSet("dsServiceLineAdj");
                    oServiceLineAdjAdapter.Fill(oServiceLineAdjDs, "dsServiceLineAdj");

                    foreach (DataRow oServiceLineAdjRow in oServiceLineAdjDs.Tables["dsServiceLineAdj"].Rows)
                    {
                        if (oServiceLineAdjRow["SVD01_OtherPayerPrimaryIdfr"].ToString().Trim() != "")
                        {
                            //SVD - Line Adjudication Information
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\SVD\\SVD"));
                            oSegment.set_DataElementValue(1, 0, oServiceLineAdjRow["SVD01_OtherPayerPrimaryIdfr"].ToString()); 		// Identification Code (67) 
                            oSegment.set_DataElementValue(2, 0, oServiceLineAdjRow["SVD02_ServiceLinePaidAmount"].ToString()); 		// Monetary Amount (782) 
                            oSegment.set_DataElementValue(3, 1, oServiceLineAdjRow["SVD03_01_ProductServiceIdQlfr"].ToString()); 		// Product/Service ID Qualifier (235) 
                            oSegment.set_DataElementValue(3, 2, oServiceLineAdjRow["SVD03_02_ProcedureCode"].ToString()); 		// Product/Service ID (234) 
                            oSegment.set_DataElementValue(3, 3, oServiceLineAdjRow["SVD03_03_ProcedureModifier1"].ToString()); 		// Procedure Modifier (1339) 
                            oSegment.set_DataElementValue(3, 4, oServiceLineAdjRow["SVD03_04_ProcedurModifier2"].ToString()); 		// Procedure Modifier (1339) 
                            oSegment.set_DataElementValue(3, 5, oServiceLineAdjRow["SVD03_05_ProcedureModifier3"].ToString()); 		// Procedure Modifier (1339) 
                            oSegment.set_DataElementValue(3, 6, oServiceLineAdjRow["SVD03_06_ProcedureModifier4"].ToString()); 		// Procedure Modifier (1339) 
                            oSegment.set_DataElementValue(3, 7, oServiceLineAdjRow["SVD03_07_ProcedureCodeDesc"].ToString()); 		// Description (352) 
                            oSegment.set_DataElementValue(5, 0, oServiceLineAdjRow["SVD05_PaidServiceUnitCount"].ToString()); 		// Quantity (380) 
                            oSegment.set_DataElementValue(6, 0, oServiceLineAdjRow["SVD06_LineNumber"].ToString()); 		// Assigned Number (554) 

                            //CAS - Line Adjustment 
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\SVD\\CAS"));
                            oSegment.set_DataElementValue(1, 0, oServiceLineAdjRow["CAS01_AdjustmentReasonCode"].ToString()); 		// Claim Adjustment Group Code (1033) 
                            oSegment.set_DataElementValue(2, 0, oServiceLineAdjRow["CAS02_AdjustmentAmount"].ToString()); 		// Claim Adjustment Reason Code (1034) 
                            oSegment.set_DataElementValue(3, 0, oServiceLineAdjRow["CAS03_AdjustmentQuantity"].ToString()); 		// Monetary Amount (782) 
                            oSegment.set_DataElementValue(4, 0, oServiceLineAdjRow["CAS04_Quantity"].ToString()); 		// Quantity (380) 
                            oSegment.set_DataElementValue(5, 0, oServiceLineAdjRow["CAS05_AdjustmentReasonCode2"].ToString()); 		// Claim Adjustment Reason Code (1034) 
                            oSegment.set_DataElementValue(6, 0, oServiceLineAdjRow["CAS06_AdjustmentAmount2"].ToString()); 		// Monetary Amount (782) 
                            oSegment.set_DataElementValue(7, 0, oServiceLineAdjRow["CAS07_AdjustmentQuantity2"].ToString()); 		// Quantity (380) 
                            oSegment.set_DataElementValue(8, 0, oServiceLineAdjRow["CAS08_AdjustmentReasonCode3"].ToString()); 		// Claim Adjustment Reason Code (1034) 
                            oSegment.set_DataElementValue(9, 0, oServiceLineAdjRow["CAS09_AdjustmentAmount3"].ToString()); 		// Monetary Amount (782) 
                            oSegment.set_DataElementValue(10, 0, oServiceLineAdjRow["CAS10_AdjustmentQuantity3"].ToString()); 		// Quantity (380) 
                            oSegment.set_DataElementValue(11, 0, oServiceLineAdjRow["CAS11_AdjustmentReasonCode4"].ToString()); 		// Claim Adjustment Reason Code (1034) 
                            oSegment.set_DataElementValue(12, 0, oServiceLineAdjRow["CAS12_AdjustmentAmount4"].ToString()); 		// Monetary Amount (782) 
                            oSegment.set_DataElementValue(13, 0, oServiceLineAdjRow["CAS13_AdjustmentQuantity4"].ToString()); 		// Quantity (380) 
                            oSegment.set_DataElementValue(14, 0, oServiceLineAdjRow["CAS14_AdjustmentReasonCode5"].ToString()); 		// Claim Adjustment Reason Code (1034) 
                            oSegment.set_DataElementValue(15, 0, oServiceLineAdjRow["CAS15_AdjustmentAmount5"].ToString()); 		// Monetary Amount (782) 
                            oSegment.set_DataElementValue(16, 0, oServiceLineAdjRow["CAS16_AdjustmentQuantity5"].ToString()); 		// Quantity (380) 
                            oSegment.set_DataElementValue(17, 0, oServiceLineAdjRow["CAS17_AdjustmentReasonCode6"].ToString()); 		// Claim Adjustment Reason Code (1034) 
                            oSegment.set_DataElementValue(18, 0, oServiceLineAdjRow["CAS18_AdjustmentAmount6"].ToString()); 		// Monetary Amount (782) 
                            oSegment.set_DataElementValue(19, 0, oServiceLineAdjRow["CAS19_AdjustmentQuantity6"].ToString()); 		// Quantity (380) 

                            //DTP - Line Check or Remittance Date
                            if (oServiceLineAdjRow["DTP03_AdjudicationPaymentDate"].ToString().Trim() != "")
                            {
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\SVD\\DTP"));
                                oSegment.set_DataElementValue(1, 0, "573"); 		// Date/Time Qualifier (374) 
                                oSegment.set_DataElementValue(2, 0, "D8"); 		// Date Time Period Format Qualifier (1250) 
                                oSegment.set_DataElementValue(3, 0, oServiceLineAdjRow["DTP03_AdjudicationPaymentDate"].ToString()); 		// Date Time Period (1251) 
                            }

                            //AMT - Remaining Patient Liability 
                            if (oServiceLineAdjRow["AMT02_RemainingPatientLiability"].ToString().Trim() != "")
                            {
                                ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\SVD\\AMT"));
                                oSegment.set_DataElementValue(1, 0, "EAF"); 		// Amount Qualifier Code (522) 
                                oSegment.set_DataElementValue(2, 0, oServiceLineAdjRow["AMT02_RemainingPatientLiability"].ToString()); 		// Monetary Amount (782) 
                            }
                        
                        }//if (oServiceLineAdjRow["SVD01_OtherPayerPrimaryIdfr"].ToString().Trim() != "")

                    }//2430 LINE ADJUDICATION INFORMATION  foreach oServiceLineAdjRow 


                    //2440 FORM IDENTIFICATION CODE
                    sSql = "select * from [837X222_ServiceDocument] where ServiceLinekey = " + oServiceLineRow["ServiceLinekey"].ToString();
                    SqlDataAdapter oServiceDocumentAdapter = new SqlDataAdapter(sSql, oConnection);
                    DataSet oServiceDocumentDs = new DataSet("dsServiceDocument");
                    oServiceDocumentAdapter.Fill(oServiceDocumentDs, "dsServiceDocument");

                    foreach (DataRow oServiceDocumentRow in oServiceDocumentDs.Tables["dsServiceDocument"].Rows)
                    {
                        if (oServiceDocumentRow["LQ01_CodeListQualifierCode"].ToString().Trim() != "")
                        {
                            //LQ - Form Identification Code 
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\LQ\\LQ"));
                            oSegment.set_DataElementValue(1, 0, oServiceDocumentRow["LQ01_CodeListQualifierCode"].ToString()); 		// Code List Qualifier Code (1270) 
                            oSegment.set_DataElementValue(2, 0, oServiceDocumentRow["LQ02_FormIdentifier"].ToString()); 		// Industry Code (1271) 

                            //FRM - Supporting Documentation  
                            ediDataSegment.Set(ref oSegment, oTransactionSet.CreateDataSegment("HL\\CLM\\LX\\LQ\\FRM"));
                            oSegment.set_DataElementValue(1, 0, oServiceDocumentRow["FRM01_QuestionNumberLetter"].ToString()); 		// Assigned Identification (350) 
                            oSegment.set_DataElementValue(2, 0, oServiceDocumentRow["FRM02_QuestionResponseCode"].ToString()); 		// Yes/No Condition or Response Code (1073) 
                            oSegment.set_DataElementValue(3, 0, oServiceDocumentRow["FRM03_QuestionResponse"].ToString()); 		// Reference Identification (127) 
                            oSegment.set_DataElementValue(4, 0, oServiceDocumentRow["FRM04_QuestionResponseDate"].ToString()); 		// Date (373) 
                            oSegment.set_DataElementValue(5, 0, oServiceDocumentRow["FRM05_QuestionResponsePercent"].ToString()); 		// Percent (332) 

                        }// if (oServiceDocumentRow["LQ01_CodeListQualifierCode"].ToString().Trim() != "")
                        
                    }//foreach oServiceDocumentRow //2440 FORM IDENTIFICATION CODE

                }//foreach service line  //2400 SERVICE LINE 

            }//foreach claim //2300
        }

        private void btnViewTables_Click(object sender, EventArgs e)
        {
            frmViewTables ofrmViewTables = new frmViewTables();

            ofrmViewTables.Show();

        }

        private void btnAddDelRec_Click(object sender, EventArgs e)
        {
            frmAddRec oAddRec;

            oAddRec = new frmAddRec();
            oAddRec.Show();

        }   //private void Proc_2300_Claim
    }   //public partial class form
}
