using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gen837X222A1
{
    public partial class frmAddRec : Form
    {
        public frmAddRec()
        {
            InitializeComponent();
        }

        //the script files to create the tables can be found in the folder "CREATE_SQL_TABLES"

        private string sConnection = ConfigurationManager.ConnectionStrings["Development"].ConnectionString;

        private SqlConnection oConnection;
        private SqlDataAdapter oDaInterchange = new SqlDataAdapter();
        private SqlDataAdapter oDaFuncGroup = new SqlDataAdapter();
        private SqlDataAdapter oDaHeader = new SqlDataAdapter();
        private SqlDataAdapter oDaInfoSource = new SqlDataAdapter();
        private SqlDataAdapter oDaSubscriber = new SqlDataAdapter();
        private SqlDataAdapter oDaOtherSubscriberInfo = new SqlDataAdapter();
        private SqlDataAdapter oDaDependent = new SqlDataAdapter();
        private SqlDataAdapter oDaClaims = new SqlDataAdapter();
        private SqlDataAdapter oDaServiceLine = new SqlDataAdapter();
        private SqlDataAdapter oDaServiceDocument = new SqlDataAdapter();
        private SqlDataAdapter oDaServiceLineAdj = new SqlDataAdapter();

        private Int32 nInterkey;
        private Int32 nGroupkey;
        private Int32 nHeaderkey;
        private Int32 nInfoSourcekey;
        private Int32 nSubscriberkey;
        private Int32 nOtherSubscriberInfokey;
        private Int32 nDependentkey;
        private Int32 nClaimskey;
        private Int32 nServiceLinekey;
        private Int32 nServiceDocumentkey;
        private Int32 nServiceLineAdjkey;

        private void btnAddRec_Click(object sender, EventArgs e)
        {

            oConnection = new SqlConnection(sConnection);
            oConnection.Open();

            string sSql = "";

            //INSERT A RECORD INTO INTERCHANGE TABLE
            sSql = @"INSERT INTO [Interchange] (ISA01_AuthorizationInfoQlfr, ISA02_AuthorizationInfo, ISA03_SecurityInfoQlfr,
                        ISA04_SecurityInfo, ISA05_SenderIdQlfr, ISA06_SenderId, ISA07_ReceiverIdQlfr, ISA08_ReceiverId, ISA09_Date, 
                        ISA10_Time, ISA11_RepetitionSeparator, ISA12_ControlVersionNumber, ISA13_ControlNumber, 
                        ISA14_AcknowledgmentRequested, ISA15_UsageIndicator, ISA16_ComponentElementSeparator) 
                        values 
                        (@ISA01_AuthorizationInfoQlfr, @ISA02_AuthorizationInfo, @ISA03_SecurityInfoQlfr,
                        @ISA04_SecurityInfo, @ISA05_SenderIdQlfr, @ISA06_SenderId, @ISA07_ReceiverIdQlfr, @ISA08_ReceiverId, @ISA09_Date, 
                        @ISA10_Time, @ISA11_RepetitionSeparator, @ISA12_ControlVersionNumber, @ISA13_ControlNumber, 
                        @ISA14_AcknowledgmentRequested, @ISA15_UsageIndicator, @ISA16_ComponentElementSeparator);
                        SELECT scope_identity()";

            oDaInterchange.InsertCommand = new SqlCommand(sSql, oConnection);
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA01_AuthorizationInfoQlfr", "00");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA02_AuthorizationInfo", "");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA03_SecurityInfoQlfr", "00");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA04_SecurityInfo", "");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA05_SenderIdQlfr", "ZZ");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA06_SenderId", "SENDER_ID");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA07_ReceiverIdQlfr", "ZZ");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA08_ReceiverId", "REVEIVER_ID");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA09_Date", "020617");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA10_Time", "1816");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA11_RepetitionSeparator", "^");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA12_ControlVersionNumber", "00501");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA13_ControlNumber", "000000238");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA14_AcknowledgmentRequested", "0");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA15_UsageIndicator", "T");
            oDaInterchange.InsertCommand.Parameters.AddWithValue("@ISA16_ComponentElementSeparator", ":");
            nInterkey = (Int32)(decimal)oDaInterchange.InsertCommand.ExecuteScalar();


            //INSERT A RECORD INTO FUNCGROUP TABLE
            sSql = @"INSERT INTO [FuncGroup] ( Interkey, GS01_FunctionalIdfrCode, GS02_SendersCode, GS03_ReceiversCode, 
                        GS04_Date, GS05_Time, GS06_GroupControlNumber, GS07_ResponsibleAgencyCode, GS08_VersionReleaseCode)
                        values 
                        (@Interkey, @GS01_FunctionalIdfrCode, @GS02_SendersCode, @GS03_ReceiversCode, 
                        @GS04_Date, @GS05_Time, @GS06_GroupControlNumber, @GS07_ResponsibleAgencyCode, @GS08_VersionReleaseCode);
                        SELECT scope_identity()";

            oDaFuncGroup.InsertCommand = new SqlCommand(sSql, oConnection);
            oDaFuncGroup.InsertCommand.Parameters.AddWithValue("@Interkey", nInterkey);
            oDaFuncGroup.InsertCommand.Parameters.AddWithValue("@GS01_FunctionalIdfrCode", "HC");     //Functional Identifier Code
            oDaFuncGroup.InsertCommand.Parameters.AddWithValue("@GS02_SendersCode", "SENDER_ID");     //Application Sender's Code
            oDaFuncGroup.InsertCommand.Parameters.AddWithValue("@GS03_ReceiversCode", "RECEIVER_ID");     //Application Receiver's Code
            oDaFuncGroup.InsertCommand.Parameters.AddWithValue("@GS04_Date", "20020617");     //Date
            oDaFuncGroup.InsertCommand.Parameters.AddWithValue("@GS05_Time", "1816");     //Time
            oDaFuncGroup.InsertCommand.Parameters.AddWithValue("@GS06_GroupControlNumber", "206");     //Group Control Number
            oDaFuncGroup.InsertCommand.Parameters.AddWithValue("@GS07_ResponsibleAgencyCode", "X");     //Responsible Agency Code
            oDaFuncGroup.InsertCommand.Parameters.AddWithValue("@GS08_VersionReleaseCode", "005010X222A1");     //Version / Release / Industry Identifier Code
            nGroupkey = (Int32)(decimal)oDaFuncGroup.InsertCommand.ExecuteScalar();


            //INSERT A RECORD INTO HEADER TABLE
            sSql = @"INSERT INTO [837X222_Header] ( Groupkey, ST01_TranSetIdfrCode, ST02_TranSetControlNo, ST03_ImplementConventionRef, 
                        BHT01_HierarchStructCode, BHT02_TranSetPurposeCode, BHT03_RefId, BHT04_Date, BHT05_Time, BHT06_TranTypeCode, 
                        NM102_SubmitterTypeQlfr, NM103_SubmitterLastOrOrganizationName, NM104_SubmitterFirst, NM105_SubmitterMiddle, 
                        NM108_SubmitterIdCodeQlfr, NM109_SubmitterIdCode, PER02_SubmitterContactName, PER0X_SubmitterPhoneNo, 
                        PER0X_SubmitterPhoneExtNo, PER0X_SubmitterFaxNo, PER0X_SubmitterEmail, NM102_ReceiverTypeQlfr, 
                        NM103_ReceiverLastOrOrganizationName, NM108_ReceiverIdCodeQlfr, NM109_ReceiverIdCode ) 
                        values 
                        (@Groupkey, @ST01_TranSetIdfrCode, @ST02_TranSetControlNo, @ST03_ImplementConventionRef, 
                        @BHT01_HierarchStructCode, @BHT02_TranSetPurposeCode, @BHT03_RefId, @BHT04_Date, @BHT05_Time, @BHT06_TranTypeCode, 
                        @NM102_SubmitterTypeQlfr, @NM103_SubmitterLastOrOrganizationName, @NM104_SubmitterFirst, @NM105_SubmitterMiddle, 
                        @NM108_SubmitterIdCodeQlfr, @NM109_SubmitterIdCode, @PER02_SubmitterContactName, @PER0X_SubmitterPhoneNo, 
                        @PER0X_SubmitterPhoneExtNo, @PER0X_SubmitterFaxNo, @PER0X_SubmitterEmail, @NM102_ReceiverTypeQlfr, 
                        @NM103_ReceiverLastOrOrganizationName, @NM108_ReceiverIdCodeQlfr, @NM109_ReceiverIdCode);
                        SELECT scope_identity()";

            oDaHeader.InsertCommand = new SqlCommand(sSql, oConnection);
            oDaHeader.InsertCommand.Parameters.AddWithValue("@Groupkey", nGroupkey);
            oDaHeader.InsertCommand.Parameters.AddWithValue("@ST01_TranSetIdfrCode", "837");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@ST02_TranSetControlNo", "0021");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@ST03_ImplementConventionRef", "005010X222A1");
            //BHT - Beginning of Hierarchical Transaction
            oDaHeader.InsertCommand.Parameters.AddWithValue("@BHT01_HierarchStructCode", "0019");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@BHT02_TranSetPurposeCode", "00");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@BHT03_RefId", "244579");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@BHT04_Date", "20061015");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@BHT05_Time", "1023");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@BHT06_TranTypeCode", "CH");
            //1000A SUBMITTER
            //NM1 - Submitter Name
            oDaHeader.InsertCommand.Parameters.AddWithValue("@NM102_SubmitterTypeQlfr", "2");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@NM103_SubmitterLastOrOrganizationName", "PREMIER BILLING SERVICE");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@NM104_SubmitterFirst", "");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@NM105_SubmitterMiddle", "");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@NM108_SubmitterIdCodeQlfr", "46");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@NM109_SubmitterIdCode", "TG123");
            //PER - Submitter EDI Contact Information
            oDaHeader.InsertCommand.Parameters.AddWithValue("@PER02_SubmitterContactName", "JERRY");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@PER0X_SubmitterPhoneNo", "3055552222");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@PER0X_SubmitterPhoneExtNo", "231");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@PER0X_SubmitterFaxNo", "");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@PER0X_SubmitterEmail", "");
            //1000B RECEIVER
            //NM1 - Receiver Name
            oDaHeader.InsertCommand.Parameters.AddWithValue("@NM102_ReceiverTypeQlfr", "2");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@NM103_ReceiverLastOrOrganizationName", "KEY INSURANCE COMPANY");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@NM108_ReceiverIdCodeQlfr", "46");
            oDaHeader.InsertCommand.Parameters.AddWithValue("@NM109_ReceiverIdCode", "66783JJT");
            nHeaderkey = (Int32)(decimal)oDaHeader.InsertCommand.ExecuteScalar();


            //INSERT A RECORD INTO INFOSOURCE TABLE
            sSql = @"INSERT INTO [837X222_InfoSource] ( Headerkey, PRV03_BillingProviderIdCode, CUR02_CurrencyCode, 
                        NM102_BillingProviderTypeQualifier, NM103_BillingProviderLastOrOrganizationName, NM104_BillingProviderFirst, 
                        NM105_BillingProviderMiddle, NM109_BillingProviderIdCode, N301_BillingProviderAddr1, N302_BillingProviderAddr2, 
                        N401_BillingProviderCity, N402_BillingProviderState, N403_BillingProviderZip, N404_BillingProviderCountry, 
                        N407_BillingProviderCountrySubdivision, REF02_BillingProviderEmployerId, REF02_BillingProviderSocialSecurityNo, 
                        REF02_BillingProviderStateLicenseNo, REF02_BillingProviderProviderUPIN, PER02_BillingProviderContactName, 
                        PER0X_BillingProviderPhoneNo, PER0X_BillingProviderPhoneExtNo, PER0X_BillingProviderFaxNo, 
                        PER0X_BillingProviderEmail, NM102_PayToProviderTypeQlfr, NM103_PayToProviderLastOrOrganizatioName, 
                        N301_PayToProviderAddr1, N302_PayToProviderAddr2, N401_PayToProviderCity, N402_PayToProviderState, 
                        N403_PayToProviderZip, N404_PayToProviderCountry, N407_PayToProviderCountrySubdivision,  
                        NM103_PayeeLastOrOrganizationName, NM108_PayeeIdCodeQlfr, NM109_PayeeIdCode, N301_PayeeAddr1, N302_PayeeAddr2, N401_PayeeCity, 
                        N402_PayeeState, N403_PayeeZip, N404_PayeeCountry, N407_PayeeCountrySubdivision, REF02_PayeePayerId, 
                        REF02_PayeeClaimOfficeNo, REF02_PayeeNAIC_Code, REF02_PayeeEmployerId ) 
                        values 
                        (@Headerkey, @PRV03_BillingProviderIdCode, @CUR02_CurrencyCode, 
                        @NM102_BillingProviderTypeQualifier, @NM103_BillingProviderLastOrOrganizationName, @NM104_BillingProviderFirst, 
                        @NM105_BillingProviderMiddle, @NM109_BillingProviderIdCode, @N301_BillingProviderAddr1, @N302_BillingProviderAddr2, 
                        @N401_BillingProviderCity, @N402_BillingProviderState, @N403_BillingProviderZip, @N404_BillingProviderCountry, 
                        @N407_BillingProviderCountrySubdivision, @REF02_BillingProviderEmployerId, @REF02_BillingProviderSocialSecurityNo, 
                        @REF02_BillingProviderStateLicenseNo, @REF02_BillingProviderProviderUPIN, @PER02_BillingProviderContactName, 
                        @PER0X_BillingProviderPhoneNo, @PER0X_BillingProviderPhoneExtNo, @PER0X_BillingProviderFaxNo, 
                        @PER0X_BillingProviderEmail, @NM102_PayToProviderTypeQlfr, @NM103_PayToProviderLastOrOrganizatioName, 
                        @N301_PayToProviderAddr1, @N302_PayToProviderAddr2, @N401_PayToProviderCity, @N402_PayToProviderState, 
                        @N403_PayToProviderZip, @N404_PayToProviderCountry, @N407_PayToProviderCountrySubdivision,  
                        @NM103_PayeeLastOrOrganizationName, @NM108_PayeeIdCodeQlfr, @NM109_PayeeIdCode, @N301_PayeeAddr1, @N302_PayeeAddr2, @N401_PayeeCity, 
                        @N402_PayeeState, @N403_PayeeZip, @N404_PayeeCountry, @N407_PayeeCountrySubdivision, @REF02_PayeePayerId, 
                        @REF02_PayeeClaimOfficeNo, @REF02_PayeeNAIC_Code, @REF02_PayeeEmployerId);
                        SELECT scope_identity()";

            oDaInfoSource.InsertCommand = new SqlCommand(sSql, oConnection);
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@Headerkey", nHeaderkey);
            //2000A BILLING PROVIDER HIERARCHICAL LEVEL
            //PRV - Billing Provider Specialty Information
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@PRV03_BillingProviderIdCode", "203BF0100Y");
            //CUR - Foreign Currency Information
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@CUR02_CurrencyCode", "CAD");
            //2010AA BILLING PROVIDER
            //NM1 - Billing Provider Name
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@NM102_BillingProviderTypeQualifier", "2");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@NM103_BillingProviderLastOrOrganizationName", "BEN KILDARE SERVICE");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@NM104_BillingProviderFirst", "");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@NM105_BillingProviderMiddle", "");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@NM109_BillingProviderIdCode", "9876543210");
            //N3 - BBilling Provider Address
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N301_BillingProviderAddr1", "234 SEAWAY ST");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N302_BillingProviderAddr2", "");
            //N4 - Billing Provider City, State, Zip Code
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N401_BillingProviderCity", "MIAMI");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N402_BillingProviderState", "FL");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N403_BillingProviderZip", "33111");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N404_BillingProviderCountry", "");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N407_BillingProviderCountrySubdivision", "");
            //REF - Billing Provider Tax Identification
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@REF02_BillingProviderEmployerId", "587654321");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@REF02_BillingProviderSocialSecurityNo", "");
            //REF - Billing Provider UPIN/License Information
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@REF02_BillingProviderStateLicenseNo", "");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@REF02_BillingProviderProviderUPIN", "6543211");
            //PER - Billing Provider Contact Information
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@PER02_BillingProviderContactName", "");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@PER0X_BillingProviderPhoneNo", "1235551234");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@PER0X_BillingProviderPhoneExtNo", "234");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@PER0X_BillingProviderFaxNo", "");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@PER0X_BillingProviderEmail", "JSMITH@DOMAIN.COM");
            //2010AB PAY-TO PROVIDER
            //NM1 - Pay-to Address Name
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@NM102_PayToProviderTypeQlfr", "2");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@NM103_PayToProviderLastOrOrganizatioName", "");
            //N3 - Pay-to Address 
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N301_PayToProviderAddr1", "2345 OCEAN BLVD");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N302_PayToProviderAddr2", "");
            //N4 - Pay-To Address City, State, Zip Code
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N401_PayToProviderCity", "MIAMI");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N402_PayToProviderState", "FL");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N403_PayToProviderZip", "33111");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N404_PayToProviderCountry", "");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N407_PayToProviderCountrySubdivision", "");
            //2010AC PAY-TO PLAN NAME
            //NM1 - Pay-To Plan Name
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@NM103_PayeeLastOrOrganizationName", "ANY STATE MEDICAID");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@NM108_PayeeIdCodeQlfr", "PI");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@NM109_PayeeIdCode", "12345");
            //N3 - Pay-to Plan Address
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N301_PayeeAddr1", "123 HIGHWAY RD");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N302_PayeeAddr2", "");
            //N4 - Pay-To Plan City, State, ZIP Code
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N401_PayeeCity", "KANSAS");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N402_PayeeState", "MO");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N403_PayeeZip", "64108");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N404_PayeeCountry", "");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@N407_PayeeCountrySubdivision", "");
            //REF - Pay-to Plan Secondary Identification
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@REF02_PayeePayerId", "98765");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@REF02_PayeeClaimOfficeNo", "");
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@REF02_PayeeNAIC_Code", "");
            //REF - Pay-To Plan Tax Identification Number
            oDaInfoSource.InsertCommand.Parameters.AddWithValue("@REF02_PayeeEmployerId", "123456789");
            nInfoSourcekey = (Int32)(decimal)oDaInfoSource.InsertCommand.ExecuteScalar();


            //INSERT A RECORD INTO SUBSCRIBER TABLE
            sSql = @"INSERT INTO [837X222_Subscriber] (InfoSourcekey, SBR01_PayerResponsibilitySequenceNumberCode, 
                        SBR02_IndividualRelationshipCode, SBR03_SubscriberGroup_PolicyNo, SBR04_SubscriberGroupName, 
                        SBR05_InsuranceTypeCode, SBR09_ClaimFilingIndicatorCode, PAT06_PatientDeathDate, PAT08_PatientWeightPounds, 
                        PAT09_Pregnant, NM102_SubscriberTypeQualifier, NM103_SubscriberLastOrOrganizationName, NM104_SubscriberFirst, 
                        NM105_SubscriberMiddle, NM107_SubscriberSuffix, NM108_SubscriberIdCodeQlfr, NM109_SubscriberIdCode, 
                        N301_SubscriberAddr1, N302_SubscriberAddr2, N401_SubscriberCity, N402_SubscriberState, N403_SubscriberZip, 
                        N404_SubscriberCountry, N407_SubscriberCountrySubdivision, DMG02_SubscriberBirthDate, DMG03_SubscriberGenderCode, 
                        REF02_SubscriberSocialSecurityNo, REF02_PropertyCasualtyClaimNo, PER02_SubscriberContactName, 
                        PER04_SubscriberPhoneNo, PER06_SubscriberPhoneExtNo, NM102_PayerTypeQlfr, NM103_PayerLastOrOrganizatioName, NM109_PayerIdCode,
                        N301_PayerAddr1, N302_PayerAddr2, N401_PayerCity, N402_PayerState, N403_PayerZip, N404_PayerCountry, 
                        N407_PayerCountrySubdivision, REF02_PayerId, REF02_EmployerId, REF02_ClaimOfficeNo, REF02_NAIC_Code, 
                        REF02_ProviderCommercialNo, REF02_LocationNo ) 
                        values 
                        (@InfoSourcekey, @SBR01_PayerResponsibilitySequenceNumberCode, 
                        @SBR02_IndividualRelationshipCode, @SBR03_SubscriberGroup_PolicyNo, @SBR04_SubscriberGroupName, 
                        @SBR05_InsuranceTypeCode, @SBR09_ClaimFilingIndicatorCode, @PAT06_PatientDeathDate, @PAT08_PatientWeightPounds, 
                        @PAT09_Pregnant, @NM102_SubscriberTypeQualifier, @NM103_SubscriberLastOrOrganizationName, @NM104_SubscriberFirst, 
                        @NM105_SubscriberMiddle, @NM107_SubscriberSuffix, @NM108_SubscriberIdCodeQlfr, @NM109_SubscriberIdCode, 
                        @N301_SubscriberAddr1, @N302_SubscriberAddr2, @N401_SubscriberCity, @N402_SubscriberState, @N403_SubscriberZip, 
                        @N404_SubscriberCountry, @N407_SubscriberCountrySubdivision, @DMG02_SubscriberBirthDate, @DMG03_SubscriberGenderCode, 
                        @REF02_SubscriberSocialSecurityNo, @REF02_PropertyCasualtyClaimNo, @PER02_SubscriberContactName, 
                        @PER04_SubscriberPhoneNo, @PER06_SubscriberPhoneExtNo, @NM102_PayerTypeQlfr, @NM103_PayerLastOrOrganizatioName, @NM109_PayerIdCode,
                        @N301_PayerAddr1, @N302_PayerAddr2, @N401_PayerCity, @N402_PayerState, @N403_PayerZip, @N404_PayerCountry, 
                        @N407_PayerCountrySubdivision, @REF02_PayerId, @REF02_EmployerId, @REF02_ClaimOfficeNo, @REF02_NAIC_Code, 
                        @REF02_ProviderCommercialNo, @REF02_LocationNo);
                        SELECT scope_identity()";

            oDaSubscriber.InsertCommand = new SqlCommand(sSql, oConnection);
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@InfoSourcekey", nInfoSourcekey);
            //SBR - SUBSCRIBER INFORMATION
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@SBR01_PayerResponsibilitySequenceNumberCode", "P");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@SBR02_IndividualRelationshipCode", "");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@SBR03_SubscriberGroup_PolicyNo", "2222-SJ");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@SBR04_SubscriberGroupName", "");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@SBR05_InsuranceTypeCode", "");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@SBR09_ClaimFilingIndicatorCode", "CI");
            //PAT - Patient Information
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@PAT06_PatientDeathDate", "19970314");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@PAT08_PatientWeightPounds", "");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@PAT09_Pregnant", "");
            //2010BA SUBSCRIBER  
            //NM1 - Subscriber Name
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@NM102_SubscriberTypeQualifier", "1");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@NM103_SubscriberLastOrOrganizationName", "SMITH");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@NM104_SubscriberFirst", "JANE");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@NM105_SubscriberMiddle", "");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@NM107_SubscriberSuffix", "");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@NM108_SubscriberIdCodeQlfr", "MI");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@NM109_SubscriberIdCode", "JS00111223333");
            //N3 - Subscriber Address
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N301_SubscriberAddr1", "123 MAIN AVENUE");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N302_SubscriberAddr2", "");
            //N4 - Subscriber City, State, ZIP Code
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N401_SubscriberCity", "PARIS");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N402_SubscriberState", "CA");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N403_SubscriberZip", "91506");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N404_SubscriberCountry", "");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N407_SubscriberCountrySubdivision", "");
            //DMG - Subscriber Demographic Information
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@DMG02_SubscriberBirthDate", "19430501");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@DMG03_SubscriberGenderCode", "F");
            //REF - Subscriber Secondary Identification 
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@REF02_SubscriberSocialSecurityNo", "123456789");
            //REF - Property and Casualty Claim Number
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@REF02_PropertyCasualtyClaimNo", "4445555");
            //PER - Property and Casualty Subscriber Contact Information
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@PER02_SubscriberContactName", "JOHN SMITH");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@PER04_SubscriberPhoneNo", "5555551234");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@PER06_SubscriberPhoneExtNo", "123");
            //2010BB PAYER
            //NM1 - Payer Name
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@NM102_PayerTypeQlfr", "2");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@NM103_PayerLastOrOrganizatioName", "KEY INSURANCE COMPANY");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@NM109_PayerIdCode", "11122333");
            //N3 - Payer Address
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N301_PayerAddr1", "321 HIGHWAY AVENUE");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N302_PayerAddr2", "");
            //N4 - Payer City, State, ZIP Code  
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N401_PayerCity", "TORRANCE");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N402_PayerState", "CA");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N403_PayerZip", "71506");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N404_PayerCountry", "");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@N407_PayerCountrySubdivision", "");
            //REF - Payer Secondary Identification
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@REF02_PayerId", "435261708");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@REF02_EmployerId", "");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@REF02_ClaimOfficeNo", "");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@REF02_NAIC_Code", "");
            //REF - Billing Provider Secondary Identification
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@REF02_ProviderCommercialNo", "KA6663");
            oDaSubscriber.InsertCommand.Parameters.AddWithValue("@REF02_LocationNo", "");
            nSubscriberkey = (Int32)(decimal)oDaSubscriber.InsertCommand.ExecuteScalar();


           
            //INSERT A RECORD INTO DEPEDENT TABLE
            sSql = @"INSERT INTO [837X222_Dependent] (Subscriberkey, PAT01_IndividualRelationshipCode, PAT06_PatientDeathDate, 
                        PAT08_PatientWeightPounds, PAT09_Pregnant, NM102_PatientTypeQualifier, NM103_PatientLastOrOrganizationName, 
                        NM104_PatientFirst, NM105_PatientMiddle, NM107_PatientSuffix, 
                        N301_PatientAddr1, N302_PatientAddr2, N401_PatientCity, N402_PatientState, N403_PatientZip, 
                        N404_PatientCountry, N407_PatientCountrySubdivision, DMG02_PatientBirthDate, DMG03_PatientGenderCode, 
                        REF02_PropertyCasualtyClaimNo, REF02_PatientSocialSecurityNo, REF02_MemberIdNo, 
                        PER02_PatientContactName, PER04_PatientPhoneNo, PER06_PatientPhoneExtNo ) 
                        values 
                        (@Subscriberkey, @PAT01_IndividualRelationshipCode, @PAT06_PatientDeathDate, 
                        @PAT08_PatientWeightPounds, @PAT09_Pregnant, @NM102_PatientTypeQualifier, @NM103_PatientLastOrOrganizationName, 
                        @NM104_PatientFirst, @NM105_PatientMiddle, @NM107_PatientSuffix, 
                        @N301_PatientAddr1, @N302_PatientAddr2, @N401_PatientCity, @N402_PatientState, @N403_PatientZip, 
                        @N404_PatientCountry, @N407_PatientCountrySubdivision, @DMG02_PatientBirthDate, @DMG03_PatientGenderCode, 
                        @REF02_PropertyCasualtyClaimNo, @REF02_PatientSocialSecurityNo, @REF02_MemberIdNo, 
                        @PER02_PatientContactName, @PER04_PatientPhoneNo, @PER06_PatientPhoneExtNo);
                        SELECT scope_identity()";

            oDaDependent.InsertCommand = new SqlCommand(sSql, oConnection);
            oDaDependent.InsertCommand.Parameters.AddWithValue("@Subscriberkey", nSubscriberkey);
            //2000C PATIENT HIERARCHICAL LEVEL
            //PAT - Patient Information
            oDaDependent.InsertCommand.Parameters.AddWithValue("@PAT01_IndividualRelationshipCode", "19");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@PAT06_PatientDeathDate", "");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@PAT08_PatientWeightPounds", "");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@PAT09_Pregnant", "");
            //2010CA PATIENT
            //NM1 - Patient Name
            oDaDependent.InsertCommand.Parameters.AddWithValue("@NM102_PatientTypeQualifier", "1");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@NM103_PatientLastOrOrganizationName", "SMITH");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@NM104_PatientFirst", "TED");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@NM105_PatientMiddle", "");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@NM107_PatientSuffix", "");
            //N3 - Patient Address
            oDaDependent.InsertCommand.Parameters.AddWithValue("@N301_PatientAddr1", "236 N MAIN ST");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@N302_PatientAddr2", "");
            //N4 - Patient City, State, Zip Code
            oDaDependent.InsertCommand.Parameters.AddWithValue("@N401_PatientCity", "MIAMI");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@N402_PatientState", "FL");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@N403_PatientZip", "33413");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@N404_PatientCountry", "");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@N407_PatientCountrySubdivision", "");
            //DMG - Patient Demographic Information
            oDaDependent.InsertCommand.Parameters.AddWithValue("@DMG02_PatientBirthDate", "19730501");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@DMG03_PatientGenderCode", "M");
            //REF - Property and Casualty Claim Number
            oDaDependent.InsertCommand.Parameters.AddWithValue("@REF02_PropertyCasualtyClaimNo", "KA6663");
            //REF - Property and Casualty Patient Identifier
            oDaDependent.InsertCommand.Parameters.AddWithValue("@REF02_PatientSocialSecurityNo", "");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@REF02_MemberIdNo", "KA6663");
            //PER - Property and Casualty Patient Contact Information
            oDaDependent.InsertCommand.Parameters.AddWithValue("@PER02_PatientContactName", "JOHN SMITH");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@PER04_PatientPhoneNo", "5555551234");
            oDaDependent.InsertCommand.Parameters.AddWithValue("@PER06_PatientPhoneExtNo", "123");
            nDependentkey = (Int32)(decimal)oDaDependent.InsertCommand.ExecuteScalar();


            //INSERT A RECORD INTO CLAIMS TABLE
            sSql = @"INSERT INTO [837X222_Claims] (Subscriberkey, Dependentkey, CLM01_PatientControlNo, CLM02_TotalClaimChargeAmount, 
                        CLM05_01_PlaceOfServiceCode, CLM05_03_ClaimFrequencyCode, CLM06_SupplierSignatureIndicator, CLM07_PlanParticipationCode, 
                        CLM08_BenefitsAssignmentCertIndicator, CLM09_ReleaseOfInformationCode, CLM10_PatientSignatureSourceCode, 
                        CLM11_01_RelatedCausesCode, CLM11_02_RelatedCausesCode, CLM11_04_AutoAccidentStateCode, CLM11_05_CountryCode, 
                        CLM112_SpecialProgramCode, CLM120_DelayReasonCode, DTP03_OnsetofCurrentIllnessInjuryDate, DTP03_InitialTreatmentDate, 
                        DTP03_LastSeenDate, DTP03_AcuteManifestationDate, DTP03_AccidentDate, DTP03_LastMenstrualPeriodDate, DTP03_LastXrayDate, 
                        DTP03_Disability, DTP03_InitialDisabilityPeriodStart, DTP03_InitialDisabilityPeriodEnd, DTP03_LastWorkedDate, 
                        DTP03_WorkReturnDate, DTP03_HospitalizationAdmissionDate, DTP03_HospitalizationDischargeDate, DTP03_AssumedRelinquishedCareStartDate,
                        DTP03_AssumedRelinquishedCareEndDate, DTP03_PropertyCasualtyFirstContactDate, DTP03_RepricerReceivedDate, 
                        PWK01_AttachmentReportTypeCode, PWK02_AttachmentTransmissionCode, PWK06_AttachmentControlNo, CN101_ContractTypeCode, 
                        CN102_ContractAmount, CN103_ContractPercentage, CN104_ContractCode, CN105_TermsDiscountPercent, 
                        CN106_ContractVersionIdentifier, AMT02_PatientAmountPaid, REF02_SpecialPaymentReferenceNumber, REF02_MedicareVersionCode, 
                        REF02_MammographyCertificationNumber, REF02_ReferralNumber, REF02_PriorAuthorizationNumber, REF02_PayerClaimControlNumber, 
                        REF02_ClinicalLabAmendmentNumber, REF02_RepricedClaimReferenceNumber, 
                        REF02_AdjRepricedClaimReferenceNo, REF02_InvestigatDeviceExemptIdfr, 
                        REF02_ValueAddedNetworkTraceNumber, REF02_MedicalRecordNumber, REF02_DemonstrationProjectIdentifier, 
                        REF02_CarePlanOversightNumber, K301_FileInformation, NTE01_NoteReferenceCode, NTE02_ClaimNoteText, CR102_PatientWeightPounds, 
                        CR104_Ambulance_Transport_Reason_Code, CR106_TransportDistanceMiles, CR109_RoundTripPurposeDescription, 
                        CR110_StretcherPurposeDescription, CR208_NatureOfConditionCode, CR210_PatientConditionDescription, 
                        CR211_PatientConditionDescription2, CRC02_AmbulanceConditionIndicator, 
                        CRC03_AmbulanceConditionCode, CRC04_AmbulanceConditionCode2, CRC05_AmbulanceConditionCode3, CRC06_AmbulanceConditionCode4, 
                        CRC07_AmbulanceConditionCode5, CRC01_PatientVisionCodeCategory, CRC02_PatientVisionConditionIndicator, 
                        CRC03_PatientVisionConditionCode, CRC04_PatientVisionConditionCode2, CRC05_PatientVisionConditionCode3, 
                        CRC06_PatientVisionConditionCode4, CRC07_PatientVisionConditionCode5,  
                        CRC02_HomeboundConditionCode, CRC03_HomeboundIndicator, CRC02_EPSDT_ConditionCodeAppliesIndicator, 
                        CRC03_EPSDT_ConditionIndicator, CRC04_EPSDT_ConditionIndicator2, CRC05_EPSDT_ConditionIndicator3, HI01_01_DiagnosisTypeCode, 
                        HI01_02_DiagnosisCode, HI02_01_DiagnosisTypeCode2, HI02_02_DiagnosisCode2, HI03_01_DiagnosisTypeCode3, HI03_02_DiagnosisCode3, 
                        HI04_01_DiagnosisTypeCode4, HI04_02_DiagnosisCode4, HI05_01_DiagnosisTypeCode5, HI05_02_DiagnosisCode5, 
                        HI06_01_DiagnosisTypeCode6, HI06_02_DiagnosisCode6, HI07_01_DiagnosisTypeCode7, HI07_02_DiagnosisCode7, 
                        HI08_01_DiagnosisTypeCode8, HI08_02_DiagnosisCode8, HI09_01_DiagnosisTypeCode9, HI09_02_DiagnosisCode9, 
                        HI10_01_DiagnosisTypeCode10, HI10_02_DiagnosisCode10, HI11_01_DiagnosisTypeCode11, HI11_02_DiagnosisCode11, 
                        HI12_01_DiagnosisTypeCode12, HI12_02_DiagnosisCode12, HI01_02_AnesthesiaSurgicalPrincipleProcedure,
                        HI02_02_AnesthesiaSurgicalProcedure, HI01_02_ConditionCode1, 
                        HI02_02_ConditionCode2, HI03_02_ConditionCode3, 
                        HI04_02_ConditionCode4, HI05_02_ConditionCode5, 
                        HI06_02_ConditionCode6, HI07_02_ConditionCode7, 
                        HI08_02_ConditionCode8, HI09_02_ConditionCode9, 
                        HI10_02_ConditionCode10, HI11_02_ConditionCode11, 
                        HI12_02_ConditionCode12, HCP01_PricingMethodology, HCP02_RepricedAllowedAmount, 
                        HCP03_RepricedSavingAmount, HCP04_RepricingOrganizationIdentifier, HCP05_RepricingPerDiemFlatRateAmount, 
                        HCP06_RepricedApprovAmbPatientGroupCode, HCP07_RepricedApprovAmbPatientGroupAmount, 
                        HCP13_RejectReasonCode, HCP14_PolicyComplianceCode, HCP15_ExceptionCode, NM103_ReferringProviderLastName, 
                        NM104_ReferringProviderLastFirst, NM105_ReferringProviderLastMiddle, NM107_ReferringProviderLastSuffix, 
                        NM109_ReferringProviderIdentifier, REF01_ReferringProviderSecondaryIdQlfr, 
                        REF02_ReferringProviderSecondaryId, NM103_PrimaryCareProviderLastName, 
                        NM104_PrimaryCareProviderLastFirst, NM105_PrimaryCareProviderLastMiddle, NM107_PrimaryCareProviderLastSuffix, 
                        NM109_PrimaryCareProviderIdentifier, NM102_RenderingProviderTypeQualifier, 
                        NM103_RenderingProviderLastOrOrganizationName, NM104_RenderingProviderFirst, NM105_RenderingProviderMiddle, 
                        NM107_RenderingProviderSuffix, NM109_RenderingProviderIdentifier, PRV03_ProviderTaxonomyCode, 
                        REF01_RenderingProviderSecondaryIdQlfr, REF02_RenderingProviderSecondaryId, NM103_LabFacilityName, 
                        NM109_LabFacilityIdentifier, N301_LabFacilityAddress1, N302_LabFacilityAddress2, N401_LabFacilityCity, 
                        N402_LabFacilityState, N403_LabFacilityZip, N404_LabFacilityCountryCode, N407_LabFacilityCountrySubdivisionCode, 
                        REF01_LabFacilityyIdQualifier, REF02_LabFacilityIdentification, PER02_LabFacilityContactName, 
                        PER04_LabFacilityTelephoneNumber, PER06_LabFacilityExtensionNumber, NM103_SupervisingPhysicianLastName, 
                        NM104_SupervisingPhysicianFirst, NM105_SupervisingPhysicianMiddle, NM107_SupervisingPhysicianSuffix, 
                        NM109_SupervisingPhysicianIdentifier, REF01_SupervisingPhysicianSecondaryIdQlfr, REF02_SupervisingPhysicianIdSecondaryId, 
                        N301_AmbulancePickupAddress1, N302_AmbulancePickupAddress2, N401_AmbulancePickupCity, N402_AmbulancePickupState, 
                        N403_AmbulancePickupZip, N404_AmbulancePickupCountryCode, N407_AmbulancePickupCountrySubdivisionCode, 
                        N301_AmbulanceDropOffAddress1, N302_AmbulanceDropOffAddress2, N401_AmbulanceDropOffCity, N402_AmbulanceDropOffState, 
                        N403_AmbulanceDropOffZip, N404_AmbulanceDropOffCountryCode, N407_AmbulanceDropOffCountrySubdivisionCode) 
                        values 
                        (@Subscriberkey, @Dependentkey, @CLM01_PatientControlNo, @CLM02_TotalClaimChargeAmount, 
                        @CLM05_01_PlaceOfServiceCode, @CLM05_03_ClaimFrequencyCode, @CLM06_SupplierSignatureIndicator, @CLM07_PlanParticipationCode, 
                        @CLM08_BenefitsAssignmentCertIndicator, @CLM09_ReleaseOfInformationCode, @CLM10_PatientSignatureSourceCode, 
                        @CLM11_01_RelatedCausesCode, @CLM11_02_RelatedCausesCode, @CLM11_04_AutoAccidentStateCode, @CLM11_05_CountryCode, 
                        @CLM112_SpecialProgramCode, @CLM120_DelayReasonCode, @DTP03_OnsetofCurrentIllnessInjuryDate, @DTP03_InitialTreatmentDate, 
                        @DTP03_LastSeenDate, @DTP03_AcuteManifestationDate, @DTP03_AccidentDate, @DTP03_LastMenstrualPeriodDate, @DTP03_LastXrayDate, 
                        @DTP03_Disability, @DTP03_InitialDisabilityPeriodStart, @DTP03_InitialDisabilityPeriodEnd, @DTP03_LastWorkedDate, 
                        @DTP03_WorkReturnDate, @DTP03_HospitalizationAdmissionDate, @DTP03_HospitalizationDischargeDate, @DTP03_AssumedRelinquishedCareStartDate,
                        @DTP03_AssumedRelinquishedCareEndDate, @DTP03_PropertyCasualtyFirstContactDate, @DTP03_RepricerReceivedDate, 
                        @PWK01_AttachmentReportTypeCode, @PWK02_AttachmentTransmissionCode, @PWK06_AttachmentControlNo, @CN101_ContractTypeCode, 
                        @CN102_ContractAmount, @CN103_ContractPercentage, @CN104_ContractCode, @CN105_TermsDiscountPercent, 
                        @CN106_ContractVersionIdentifier, @AMT02_PatientAmountPaid, @REF02_SpecialPaymentReferenceNumber, @REF02_MedicareVersionCode, 
                        @REF02_MammographyCertificationNumber, @REF02_ReferralNumber, @REF02_PriorAuthorizationNumber, @REF02_PayerClaimControlNumber, 
                        @REF02_ClinicalLabAmendmentNumber, @REF02_RepricedClaimReferenceNumber, 
                        @REF02_AdjRepricedClaimReferenceNo, @REF02_InvestigatDeviceExemptIdfr, 
                        @REF02_ValueAddedNetworkTraceNumber, @REF02_MedicalRecordNumber, @REF02_DemonstrationProjectIdentifier, 
                        @REF02_CarePlanOversightNumber, @K301_FileInformation, @NTE01_NoteReferenceCode, @NTE02_ClaimNoteText, @CR102_PatientWeightPounds, 
                        @CR104_Ambulance_Transport_Reason_Code, @CR106_TransportDistanceMiles, @CR109_RoundTripPurposeDescription, 
                        @CR110_StretcherPurposeDescription, @CR208_NatureOfConditionCode, @CR210_PatientConditionDescription, 
                        @CR211_PatientConditionDescription2, @CRC02_AmbulanceConditionIndicator, 
                        @CRC03_AmbulanceConditionCode, @CRC04_AmbulanceConditionCode2, @CRC05_AmbulanceConditionCode3, @CRC06_AmbulanceConditionCode4, 
                        @CRC07_AmbulanceConditionCode5, @CRC01_PatientVisionCodeCategory, @CRC02_PatientVisionConditionIndicator, 
                        @CRC03_PatientVisionConditionCode, @CRC04_PatientVisionConditionCode2, @CRC05_PatientVisionConditionCode3, 
                        @CRC06_PatientVisionConditionCode4, @CRC07_PatientVisionConditionCode5, 
                        @CRC02_HomeboundConditionCode, @CRC03_HomeboundIndicator, @CRC02_EPSDT_ConditionCodeAppliesIndicator, 
                        @CRC03_EPSDT_ConditionIndicator, @CRC04_EPSDT_ConditionIndicator2, @CRC05_EPSDT_ConditionIndicator3, @HI01_01_DiagnosisTypeCode, 
                        @HI01_02_DiagnosisCode, @HI02_01_DiagnosisTypeCode2, @HI02_02_DiagnosisCode2, @HI03_01_DiagnosisTypeCode3, @HI03_02_DiagnosisCode3, 
                        @HI04_01_DiagnosisTypeCode4, @HI04_02_DiagnosisCode4, @HI05_01_DiagnosisTypeCode5, @HI05_02_DiagnosisCode5, 
                        @HI06_01_DiagnosisTypeCode6, @HI06_02_DiagnosisCode6, @HI07_01_DiagnosisTypeCode7, @HI07_02_DiagnosisCode7, 
                        @HI08_01_DiagnosisTypeCode8, @HI08_02_DiagnosisCode8, @HI09_01_DiagnosisTypeCode9, @HI09_02_DiagnosisCode9, 
                        @HI10_01_DiagnosisTypeCode10, @HI10_02_DiagnosisCode10, @HI11_01_DiagnosisTypeCode11, @HI11_02_DiagnosisCode11, 
                        @HI12_01_DiagnosisTypeCode12, @HI12_02_DiagnosisCode12, @HI01_02_AnesthesiaSurgicalPrincipleProcedure,
                        @HI02_02_AnesthesiaSurgicalProcedure, @HI01_02_ConditionCode1, 
                        @HI02_02_ConditionCode2, @HI03_02_ConditionCode3, 
                        @HI04_02_ConditionCode4, @HI05_02_ConditionCode5, 
                        @HI06_02_ConditionCode6, @HI07_02_ConditionCode7, 
                        @HI08_02_ConditionCode8, @HI09_02_ConditionCode9, 
                        @HI10_02_ConditionCode10, @HI11_02_ConditionCode11, 
                        @HI12_02_ConditionCode12, @HCP01_PricingMethodology, @HCP02_RepricedAllowedAmount, 
                        @HCP03_RepricedSavingAmount, @HCP04_RepricingOrganizationIdentifier, @HCP05_RepricingPerDiemFlatRateAmount, 
                        @HCP06_RepricedApprovAmbPatientGroupCode, @HCP07_RepricedApprovAmbPatientGroupAmount, 
                        @HCP13_RejectReasonCode, @HCP14_PolicyComplianceCode, @HCP15_ExceptionCode, @NM103_ReferringProviderLastName, 
                        @NM104_ReferringProviderLastFirst, @NM105_ReferringProviderLastMiddle, @NM107_ReferringProviderLastSuffix, 
                        @NM109_ReferringProviderIdentifier, @REF01_ReferringProviderSecondaryIdQlfr, 
                        @REF02_ReferringProviderSecondaryId, @NM103_PrimaryCareProviderLastName, 
                        @NM104_PrimaryCareProviderLastFirst, @NM105_PrimaryCareProviderLastMiddle, @NM107_PrimaryCareProviderLastSuffix, 
                        @NM109_PrimaryCareProviderIdentifier, @NM102_RenderingProviderTypeQualifier, 
                        @NM103_RenderingProviderLastOrOrganizationName, @NM104_RenderingProviderFirst, @NM105_RenderingProviderMiddle, 
                        @NM107_RenderingProviderSuffix, @NM109_RenderingProviderIdentifier, @PRV03_ProviderTaxonomyCode, 
                        @REF01_RenderingProviderSecondaryIdQlfr, @REF02_RenderingProviderSecondaryId, @NM103_LabFacilityName, 
                        @NM109_LabFacilityIdentifier, @N301_LabFacilityAddress1, @N302_LabFacilityAddress2, @N401_LabFacilityCity, 
                        @N402_LabFacilityState, @N403_LabFacilityZip, @N404_LabFacilityCountryCode, @N407_LabFacilityCountrySubdivisionCode, 
                        @REF01_LabFacilityyIdQualifier, @REF02_LabFacilityIdentification, @PER02_LabFacilityContactName, 
                        @PER04_LabFacilityTelephoneNumber, @PER06_LabFacilityExtensionNumber, @NM103_SupervisingPhysicianLastName, 
                        @NM104_SupervisingPhysicianFirst, @NM105_SupervisingPhysicianMiddle, @NM107_SupervisingPhysicianSuffix, 
                        @NM109_SupervisingPhysicianIdentifier, @REF01_SupervisingPhysicianSecondaryIdQlfr, @REF02_SupervisingPhysicianIdSecondaryId, 
                        @N301_AmbulancePickupAddress1, @N302_AmbulancePickupAddress2, @N401_AmbulancePickupCity, @N402_AmbulancePickupState, 
                        @N403_AmbulancePickupZip, @N404_AmbulancePickupCountryCode, @N407_AmbulancePickupCountrySubdivisionCode, 
                        @N301_AmbulanceDropOffAddress1, @N302_AmbulanceDropOffAddress2, @N401_AmbulanceDropOffCity, @N402_AmbulanceDropOffState, 
                        @N403_AmbulanceDropOffZip, @N404_AmbulanceDropOffCountryCode, @N407_AmbulanceDropOffCountrySubdivisionCode);
                        SELECT scope_identity()";

            oDaClaims.InsertCommand = new SqlCommand(sSql, oConnection);
            oDaClaims.InsertCommand.Parameters.AddWithValue("@Subscriberkey", nSubscriberkey);
            oDaClaims.InsertCommand.Parameters.AddWithValue("@Dependentkey", nDependentkey);
            //2300 CLAIM
            //CLM - Claim Information
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM01_PatientControlNo", "26463774");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM02_TotalClaimChargeAmount", "100");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM05_01_PlaceOfServiceCode", "11");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM05_03_ClaimFrequencyCode", "1");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM06_SupplierSignatureIndicator", "Y");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM07_PlanParticipationCode", "A");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM08_BenefitsAssignmentCertIndicator", "Y");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM09_ReleaseOfInformationCode", "I");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM10_PatientSignatureSourceCode", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM11_01_RelatedCausesCode", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM11_02_RelatedCausesCode", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM11_04_AutoAccidentStateCode", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM11_05_CountryCode", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM112_SpecialProgramCode", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CLM120_DelayReasonCode", "");
            //DTP
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_OnsetofCurrentIllnessInjuryDate", "20050108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_InitialTreatmentDate", "20050108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_LastSeenDate", "20050108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_AcuteManifestationDate", "20050108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_AccidentDate", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_LastMenstrualPeriodDate", "20050108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_LastXrayDate", "20050108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_Disability", "20050108-20050715");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_InitialDisabilityPeriodStart", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_InitialDisabilityPeriodEnd", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_LastWorkedDate", "20050108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_WorkReturnDate", "20050108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_HospitalizationAdmissionDate", "20050108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_HospitalizationDischargeDate", "20050108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_AssumedRelinquishedCareStartDate", "20050108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_AssumedRelinquishedCareEndDate", "20050108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_PropertyCasualtyFirstContactDate", "20050108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@DTP03_RepricerReceivedDate", "20050108");
            //PWK - Claim Supplemental Information 
            oDaClaims.InsertCommand.Parameters.AddWithValue("@PWK01_AttachmentReportTypeCode", "OZ");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@PWK02_AttachmentTransmissionCode", "BM");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@PWK06_AttachmentControlNo", "DMN0012");
            //CN1 - Contract Information
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CN101_ContractTypeCode", "02");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CN102_ContractAmount", "550");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CN103_ContractPercentage", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CN104_ContractCode", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CN105_TermsDiscountPercent", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CN106_ContractVersionIdentifier", "");
            //AMT - Patient Amount Paid
            oDaClaims.InsertCommand.Parameters.AddWithValue("@AMT02_PatientAmountPaid", "152.12");
            //REF - Service Authorization Exception Code
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_SpecialPaymentReferenceNumber", "1");
            //REF - Mandatory Medicare (Section 4081) Crossover Indicator
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_MedicareVersionCode", "N");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_MammographyCertificationNumber", "T554");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_ReferralNumber", "12345");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_PriorAuthorizationNumber", "13579");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_PayerClaimControlNumber", "R555588");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_ClinicalLabAmendmentNumber", "12D4567890");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_RepricedClaimReferenceNumber", "RJ55555");
            //REF - Adjusted Repriced Claim Number
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_AdjRepricedClaimReferenceNo", "RP44444444");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_InvestigatDeviceExemptIdfr", "432907");
            //REF - Claim Identifier For Transmission Intermediaries
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_ValueAddedNetworkTraceNumber", "17312345600006351");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_MedicalRecordNumber", "44444TH56");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_DemonstrationProjectIdentifier", "THJ1222");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_CarePlanOversightNumber", "12345678");
            //K3 - File Information
            oDaClaims.InsertCommand.Parameters.AddWithValue("@K301_FileInformation", "STATE DATA REQUIREMENT");
            //NTE - Claim Note
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NTE01_NoteReferenceCode", "ADD");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NTE02_ClaimNoteText", "SURGERY WAS UNUSUALLY LONG");
            //CR1 - Ambulance Transport Information
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CR102_PatientWeightPounds", "140");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CR104_Ambulance_Transport_Reason_Code", "A");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CR106_TransportDistanceMiles", "12");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CR109_RoundTripPurposeDescription", "UNCONSCIOUS");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CR110_StretcherPurposeDescription", "");
            //CR2 - Spinal Manipulation Service Information
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CR208_NatureOfConditionCode", "M");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CR210_PatientConditionDescription", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CR211_PatientConditionDescription2", "");
            //CRC - Ambulance Certification
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC02_AmbulanceConditionIndicator", "Y");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC03_AmbulanceConditionCode", "01");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC04_AmbulanceConditionCode2", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC05_AmbulanceConditionCode3", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC06_AmbulanceConditionCode4", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC07_AmbulanceConditionCode5", "");
            //CRC - Patient Condition Information: Vision
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC01_PatientVisionCodeCategory", "E1");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC02_PatientVisionConditionIndicator", "Y");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC03_PatientVisionConditionCode", "L1");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC04_PatientVisionConditionCode2", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC05_PatientVisionConditionCode3", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC06_PatientVisionConditionCode4", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC07_PatientVisionConditionCode5", "");
            //CRC - Homebound Indicator
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC02_HomeboundConditionCode", "Y");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC03_HomeboundIndicator", "IH");
            //CRC - EPSDT Referral
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC02_EPSDT_ConditionCodeAppliesIndicator", "Y");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC03_EPSDT_ConditionIndicator", "ST");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC04_EPSDT_ConditionIndicator2", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@CRC05_EPSDT_ConditionIndicator3", "");
            //HI - Health Care Diagnosis Code
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI01_01_DiagnosisTypeCode", "BK");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI01_02_DiagnosisCode", "0340");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI02_01_DiagnosisTypeCode2", "BF");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI02_02_DiagnosisCode2", "V7389");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI03_01_DiagnosisTypeCode3", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI03_02_DiagnosisCode3", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI04_01_DiagnosisTypeCode4", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI04_02_DiagnosisCode4", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI05_01_DiagnosisTypeCode5", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI05_02_DiagnosisCode5", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI06_01_DiagnosisTypeCode6", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI06_02_DiagnosisCode6", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI07_01_DiagnosisTypeCode7", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI07_02_DiagnosisCode7", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI08_01_DiagnosisTypeCode8", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI08_02_DiagnosisCode8", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI09_01_DiagnosisTypeCode9", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI09_02_DiagnosisCode9", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI10_01_DiagnosisTypeCode10", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI10_02_DiagnosisCode10", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI11_01_DiagnosisTypeCode11", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI11_02_DiagnosisCode11", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI12_01_DiagnosisTypeCode12", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI12_02_DiagnosisCode12", "");
            //HI -  Anesthesia Related Procedure 
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI01_02_AnesthesiaSurgicalPrincipleProcedure", "33414");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI02_02_AnesthesiaSurgicalProcedure", "");
            //HI -  Condition Information 
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI01_02_ConditionCode1", "17");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI02_02_ConditionCode2", "67");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI03_02_ConditionCode3", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI04_02_ConditionCode4", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI05_02_ConditionCode5", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI06_02_ConditionCode6", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI07_02_ConditionCode7", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI08_02_ConditionCode8", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI09_02_ConditionCode9", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI10_02_ConditionCode10", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI11_02_ConditionCode11", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HI12_02_ConditionCode12", "");
            //HCP - Claim Pricing/Repricing Information 
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HCP01_PricingMethodology", "03");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HCP02_RepricedAllowedAmount", "100");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HCP03_RepricedSavingAmount", "10");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HCP04_RepricingOrganizationIdentifier", "RPO12345");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HCP05_RepricingPerDiemFlatRateAmount", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HCP06_RepricedApprovAmbPatientGroupCode", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HCP07_RepricedApprovAmbPatientGroupAmount", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HCP13_RejectReasonCode", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HCP14_PolicyComplianceCode", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@HCP15_ExceptionCode", "");
            //2310A REFERRING PROVIDER NAME
            //NM1 - Referring Provider Name
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM103_ReferringProviderLastName", "SMITH");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM104_ReferringProviderLastFirst", "TED");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM105_ReferringProviderLastMiddle", "W");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM107_ReferringProviderLastSuffix", "JR");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM109_ReferringProviderIdentifier", "1234567891");
            //REF - Referring Provider Secondary Identification
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF01_ReferringProviderSecondaryIdQlfr", "G2");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_ReferringProviderSecondaryId", "12345");
            //2310A REFERRING PROVIDER NAME
            //NM1 - Primary Care Provider
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM103_PrimaryCareProviderLastName", "DOE");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM104_PrimaryCareProviderLastFirst", "JANE");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM105_PrimaryCareProviderLastMiddle", "C");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM107_PrimaryCareProviderLastSuffix", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM109_PrimaryCareProviderIdentifier", "1234567804");
            //2310B RENDERING PROVIDER
            //NM1 - Rendering Provider Name
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM102_RenderingProviderTypeQualifier", "1");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM103_RenderingProviderLastOrOrganizationName", "DOE");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM104_RenderingProviderFirst", "JANE");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM105_RenderingProviderMiddle", "C");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM107_RenderingProviderSuffix", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM109_RenderingProviderIdentifier", "1234567804");
            //PRV - Rendering Provider Specialty Information
            oDaClaims.InsertCommand.Parameters.AddWithValue("@PRV03_ProviderTaxonomyCode", "1223G0001X");
            //REF - Rendering Provider Secondary Identification
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF01_RenderingProviderSecondaryIdQlfr", "G2");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_RenderingProviderSecondaryId", "12345");
            //2310C SERVICE FACILITY LOCATION
            //NM1 - Service Facility Location Name
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM103_LabFacilityName", "ABC CLINIC");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM109_LabFacilityIdentifier", "1234567891");
            //N3 - Service Facility Location Address  
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N301_LabFacilityAddress1", "123 MAIN STREET");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N302_LabFacilityAddress2", "");
            //N4 - Service Facility Location City, State, ZIP Code 
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N401_LabFacilityCity", "KANSAS");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N402_LabFacilityState", "MO");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N403_LabFacilityZip", "64108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N404_LabFacilityCountryCode", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N407_LabFacilityCountrySubdivisionCode", "");
            //REF - Service Facility Location Secondary Identification
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF01_LabFacilityyIdQualifier", "G2");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_LabFacilityIdentification", "12345");
            //PER - Service Facility Contact Information  
            oDaClaims.InsertCommand.Parameters.AddWithValue("@PER02_LabFacilityContactName", "JOHN SMITH");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@PER04_LabFacilityTelephoneNumber", "5555551234");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@PER06_LabFacilityExtensionNumber", "123");
            //2310D SUPERVISING PROVIDER NAME
            //NM1 - Supervising Provider Name
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM103_SupervisingPhysicianLastName", "DOE");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM104_SupervisingPhysicianFirst", "JOHN");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM105_SupervisingPhysicianMiddle", "B");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM107_SupervisingPhysicianSuffix", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@NM109_SupervisingPhysicianIdentifier", "1234567891");
            //REF - Supervising Provider Secondary Identification
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF01_SupervisingPhysicianSecondaryIdQlfr", "G2");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@REF02_SupervisingPhysicianIdSecondaryId", "12345");
            //2310E AMBULANCE PICK-UP LOCATION
            //N3 - Ambulance Pick-up Location Address 
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N301_AmbulancePickupAddress1", "123 MAIN STREET");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N302_AmbulancePickupAddress2", "");
            //N4 - Ambulance Pick-up Location City, State, ZIP Code
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N401_AmbulancePickupCity", "KANSAS");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N402_AmbulancePickupState", "MO");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N403_AmbulancePickupZip", "64108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N404_AmbulancePickupCountryCode", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N407_AmbulancePickupCountrySubdivisionCode", "");
            //2310F AMBULANCE DROP-OFF LOCATION
            //N3 - Ambulance Drop-off Location Address
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N301_AmbulanceDropOffAddress1", "123 MAIN STREET");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N302_AmbulanceDropOffAddress2", "");
            //N4 - Ambulance Drop-off Location City, State, ZIP Code
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N401_AmbulanceDropOffCity", "KANSAS");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N402_AmbulanceDropOffState", "MO");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N403_AmbulanceDropOffZip", "64108");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N404_AmbulanceDropOffCountryCode", "");
            oDaClaims.InsertCommand.Parameters.AddWithValue("@N407_AmbulanceDropOffCountrySubdivisionCode", "");
            nClaimskey = (Int32)(decimal)oDaClaims.InsertCommand.ExecuteScalar();


            //INSERT A RECORD INTO OTHERSUBSCRIBERINFO TABLE
            sSql = @"INSERT INTO [837X222_OtherSubscriberInfo] (Claimskey, SBR01_PayerResponsibSeqNoCode, SBR02_IndividualRelationshipCode, 
                        SBR03_ReferenceIdentification, SBR04_OtherInsuredGroupName, SBR05_InsuranceTypeCode, SBR09_ClaimFilingIndicatorCode, 
                        CAS01_ClaimAdjustmentGroupCode, CAS02_AdjustmentReasonCode, CAS03_AdjustmentAmount, CAS04_AdjustmentQuantity, 
                        CAS05_AdjustmentReasonCode2, CAS06_AdjustmentAmount2, CAS07_AdjustmentQuantity2, CAS08_AdjustmentReasonCode3, 
                        CAS09_AdjustmentAmount3, CAS10_AdjustmentQuantity3, CAS11_AdjustmentReasonCode4, CAS12_AdjustmentAmount4, 
                        CAS13_AdjustmentQuantity4, CAS14_AdjustmentReasonCode5, CAS15_AdjustmentAmount5, CAS16_AdjustmentQuantity5, 
                        CAS17_AdjustmentReasonCode6, CAS18_AdjustmentAmount6, CAS19_AdjustmentQuantity6, AMT02_PayorAmountPaid, 
                        AMT02_NonCoveredChargedAmount, AMT02_RemainingPatientLiability, OI03_BenefitsAssignmentCertIndicator, 
                        OI04_PatientSignatureSourceCode, OI06_ReleaseOfInformationCode, MOA01_ReimbursementRate, MOA02_HCPCS_PayableAmount, 
                        MOA03_ClaimPaymentRemarkCode, MOA04_ClaimPaymentRemarkCode2, MOA05_ClaimPaymentRemarkCode3, MOA06_ClaimPaymentRemarkCode4, 
                        MOA07_ClaimPaymentRemarkCode5, MOA08_EndStageRenalDiseasePaymntAmnt, MOA09_NonPayableProfessionComponentBill, 
                        NM102_OtherInsuredEntityTypeQlfr, NM103_OtherInsuredLastName, NM104_OtherInsuredFirst, NM105_OtherInsuredMiddle, 
                        NM107_OtherInsuredSuffix, NM108_OtherInsuredIdQlfr, NM109_OtherInsuredID, N301_OtherInsuredAddress, 
                        N302_OtherInsuredAddress2, N401_OtherInsuredCity, N402_OtherInsuredState, N403_OtherInsuredZip, 
                        N404_OtherInsuredCountryCode, N407_OtherInsuredCountrySubdivision,
                        REF02_OtherInsuredSecondaryID, NM103_OtherPayerOrganizationName, NM108_OtherPayerCodeQlfr, NM109_OtherPayerPrimaryID, 
                        N301_OtherPayerAddress1, N302_OtherPayerAddress2, N401_OtherPayerCity, N402_OtherPayerState, N403_OtherPayerZip, 
                        N404_OtherPayerCountryCode, N407_OtherPayerCountrySubdivision, DTP03_OtherPayerPaymentDate, REF01_OtherPayerSecondaryIdQlfr, 
                        REF02_OtherPayerSecondaryID, REF02_OtherPayerPriorAuthorizationNo, REF02_OtherPayerReferralNo, 
                        REF02_OtherPayerClaimAdjustmentIndicator, REF02_OtherPayerClaimControlNo, REF01_OtherReferringProviderIdQlfr, 
                        REF02_OtherReferringProviderID, REF01_OtherPrimaryCareProviderIdQlfr, REF02_OtherPrimaryCareProviderID, 
                        REF01_OtherRenderingProviderIdQlfr, REF02_OtherRenderingProviderID, REF01_OtherServiceLocationIdQlfr, 
                        REF02_OtherServiceLocationID, REF01_OtherSupervisingPhysicianIdQlfr, REF02_OtherSupervisingPhysicianID, 
                        NM102_OtherBillingProvideEntityTypeQlfr, REF01_OtherBillingProviderIdQlfr, REF02_OtherBillingProviderID) 
                        values 
                        (@Claimskey, @SBR01_PayerResponsibSeqNoCode, @SBR02_IndividualRelationshipCode, 
                        @SBR03_ReferenceIdentification, @SBR04_OtherInsuredGroupName, @SBR05_InsuranceTypeCode, @SBR09_ClaimFilingIndicatorCode, 
                        @CAS01_ClaimAdjustmentGroupCode, @CAS02_AdjustmentReasonCode, @CAS03_AdjustmentAmount, @CAS04_AdjustmentQuantity, 
                        @CAS05_AdjustmentReasonCode2, @CAS06_AdjustmentAmount2, @CAS07_AdjustmentQuantity2, @CAS08_AdjustmentReasonCode3, 
                        @CAS09_AdjustmentAmount3, @CAS10_AdjustmentQuantity3, @CAS11_AdjustmentReasonCode4, @CAS12_AdjustmentAmount4, 
                        @CAS13_AdjustmentQuantity4, @CAS14_AdjustmentReasonCode5, @CAS15_AdjustmentAmount5, @CAS16_AdjustmentQuantity5, 
                        @CAS17_AdjustmentReasonCode6, @CAS18_AdjustmentAmount6, @CAS19_AdjustmentQuantity6, @AMT02_PayorAmountPaid, 
                        @AMT02_NonCoveredChargedAmount, @AMT02_RemainingPatientLiability, @OI03_BenefitsAssignmentCertIndicator, 
                        @OI04_PatientSignatureSourceCode, @OI06_ReleaseOfInformationCode, @MOA01_ReimbursementRate, @MOA02_HCPCS_PayableAmount, 
                        @MOA03_ClaimPaymentRemarkCode, @MOA04_ClaimPaymentRemarkCode2, @MOA05_ClaimPaymentRemarkCode3, @MOA06_ClaimPaymentRemarkCode4, 
                        @MOA07_ClaimPaymentRemarkCode5, @MOA08_EndStageRenalDiseasePaymntAmnt, @MOA09_NonPayableProfessionComponentBill, 
                        @NM102_OtherInsuredEntityTypeQlfr, @NM103_OtherInsuredLastName, @NM104_OtherInsuredFirst, @NM105_OtherInsuredMiddle, 
                        @NM107_OtherInsuredSuffix, @NM108_OtherInsuredIdQlfr, @NM109_OtherInsuredID, @N301_OtherInsuredAddress, 
                        @N302_OtherInsuredAddress2, @N401_OtherInsuredCity, @N402_OtherInsuredState, @N403_OtherInsuredZip, 
                        @N404_OtherInsuredCountryCode, @N407_OtherInsuredCountrySubdivision,  
                        @REF02_OtherInsuredSecondaryID, @NM103_OtherPayerOrganizationName, @NM108_OtherPayerCodeQlfr, @NM109_OtherPayerPrimaryID, 
                        @N301_OtherPayerAddress1, @N302_OtherPayerAddress2, @N401_OtherPayerCity, @N402_OtherPayerState, @N403_OtherPayerZip, 
                        @N404_OtherPayerCountryCode, @N407_OtherPayerCountrySubdivision, @DTP03_OtherPayerPaymentDate, @REF01_OtherPayerSecondaryIdQlfr, 
                        @REF02_OtherPayerSecondaryID, @REF02_OtherPayerPriorAuthorizationNo, @REF02_OtherPayerReferralNo,
                        @REF02_OtherPayerClaimAdjustmentIndicator, @REF02_OtherPayerClaimControlNo, @REF01_OtherReferringProviderIdQlfr, 
                        @REF02_OtherReferringProviderID, @REF01_OtherPrimaryCareProviderIdQlfr, @REF02_OtherPrimaryCareProviderID, 
                        @REF01_OtherRenderingProviderIdQlfr, @REF02_OtherRenderingProviderID, @REF01_OtherServiceLocationIdQlfr, 
                        @REF02_OtherServiceLocationID, @REF01_OtherSupervisingPhysicianIdQlfr, @REF02_OtherSupervisingPhysicianID, 
                        @NM102_OtherBillingProvideEntityTypeQlfr, @REF01_OtherBillingProviderIdQlfr, @REF02_OtherBillingProviderID);
                        SELECT scope_identity()";

            oDaOtherSubscriberInfo.InsertCommand = new SqlCommand(sSql, oConnection);
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@Claimskey", nClaimskey);
            //2320 OTHER SUBSCRIBER INFORMATION
            //SBR - Other Subscriber Information 
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@SBR01_PayerResponsibSeqNoCode", "S");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@SBR02_IndividualRelationshipCode", "01");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@SBR03_ReferenceIdentification", "GR00786");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@SBR04_OtherInsuredGroupName", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@SBR05_InsuranceTypeCode", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@SBR09_ClaimFilingIndicatorCode", "");
            //CAS - Claim Level Adjustments
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS01_ClaimAdjustmentGroupCode", "PR");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS02_AdjustmentReasonCode", "1");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS03_AdjustmentAmount", "7.93");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS04_AdjustmentQuantity", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS05_AdjustmentReasonCode2", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS06_AdjustmentAmount2", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS07_AdjustmentQuantity2", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS08_AdjustmentReasonCode3", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS09_AdjustmentAmount3", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS10_AdjustmentQuantity3", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS11_AdjustmentReasonCode4", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS12_AdjustmentAmount4", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS13_AdjustmentQuantity4", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS14_AdjustmentReasonCode5", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS15_AdjustmentAmount5", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS16_AdjustmentQuantity5", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS17_AdjustmentReasonCode6", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS18_AdjustmentAmount6", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@CAS19_AdjustmentQuantity6", "");
            //AMT - Coordination of Benefits (COB) Payer Paid Amount
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@AMT02_PayorAmountPaid", "411");
            //AMT - Coordination of Benefits (COB) Total Non-Covered Amount  
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@AMT02_NonCoveredChargedAmount", "273");
            //AMT -  Remaining Patient Liability
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@AMT02_RemainingPatientLiability", "75");
            //OI -  Other Insurance Coverage Information
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@OI03_BenefitsAssignmentCertIndicator", "Y");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@OI04_PatientSignatureSourceCode", "P");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@OI06_ReleaseOfInformationCode", "Y");
            //MOA -  Outpatient Adjudication Informatio
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@MOA01_ReimbursementRate", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@MOA02_HCPCS_PayableAmount", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@MOA03_ClaimPaymentRemarkCode", "A4");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@MOA04_ClaimPaymentRemarkCode2", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@MOA05_ClaimPaymentRemarkCode3", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@MOA06_ClaimPaymentRemarkCode4", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@MOA07_ClaimPaymentRemarkCode5", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@MOA08_EndStageRenalDiseasePaymntAmnt", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@MOA09_NonPayableProfessionComponentBill", "");
            //2330A OTHER SUBSCRIBER NAME
            //NM1 - Other Subscriber Name
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@NM102_OtherInsuredEntityTypeQlfr", "1");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@NM103_OtherInsuredLastName", "DOE");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@NM104_OtherInsuredFirst", "JOHN");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@NM105_OtherInsuredMiddle", "T");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@NM107_OtherInsuredSuffix", "JR");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@NM108_OtherInsuredIdQlfr", "MI");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@NM109_OtherInsuredID", "123456789");
            //N3 - Other Subscriber Address
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N301_OtherInsuredAddress", "123 MAIN STREET");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N302_OtherInsuredAddress2", "");
            //N4 - Other Subscriber City, State, ZIP Code
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N401_OtherInsuredCity", "KANSAS");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N402_OtherInsuredState", "MO");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N403_OtherInsuredZip", "64108");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N404_OtherInsuredCountryCode", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N407_OtherInsuredCountrySubdivision", "");
            //REF - Other Subscriber Secondary Identification
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF02_OtherInsuredSecondaryID", "123456789");
            //2330B OTHER PAYER NAME
            //NM1 - Other Payer Name
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@NM103_OtherPayerOrganizationName", "ABC INSURANCE CO");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@NM108_OtherPayerCodeQlfr", "PI");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@NM109_OtherPayerPrimaryID", "11122333");
            //N3 - Other Payer Address
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N301_OtherPayerAddress1", "23 MAIN STREET");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N302_OtherPayerAddress2", "");
            //N4 - Other Payer City, State, ZIP Code
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N401_OtherPayerCity", "KANSAS");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N402_OtherPayerState", "MO");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N403_OtherPayerZip", "64108");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N404_OtherPayerCountryCode", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@N407_OtherPayerCountrySubdivision", "");
            //DTP - Claim Check or Remittance Date
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@DTP03_OtherPayerPaymentDate", "20040203");
            //REF - Other Payer Secondary Identification
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF01_OtherPayerSecondaryIdQlfr", "2U");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF02_OtherPayerSecondaryID", "12345");
            //REF - Other Payer Prior Authorization Number
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF02_OtherPayerPriorAuthorizationNo", "AB333-Y5");
            //REF - Other Payer Referral Number
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF02_OtherPayerReferralNo", "12345");
            //REF - Other Payer Claim Adjustment Indicator
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF02_OtherPayerClaimAdjustmentIndicator", "Y");
            //REF - Other Payer Claim Control Number
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF02_OtherPayerClaimControlNo", "R555588");
            //2330C OTHER PAYER REFERRING PROVIDER
            //REF - Other Payer Referring Provider Secondary Identification
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF01_OtherReferringProviderIdQlfr", "G2");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF02_OtherReferringProviderID", "12345");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF01_OtherPrimaryCareProviderIdQlfr", "");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF02_OtherPrimaryCareProviderID", "");
            //2330D OTHER PAYER RENDERING PROVIDER
            //REF - Other Payer Rendering Provider Secondary Identification
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF01_OtherRenderingProviderIdQlfr", "G2");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF02_OtherRenderingProviderID", "12345");
            //2330E OTHER PAYER SERVICE FACILITY PROVIDER
            //REF - Other Payer Service Facility Location Secondary Identification
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF01_OtherServiceLocationIdQlfr", "G2");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF02_OtherServiceLocationID", "12345");
            //2330F OTHER PAYER SUPERVISING PROVIDER
            //REF - Other Payer Service Facility Location Secondary Identification
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF01_OtherSupervisingPhysicianIdQlfr", "G2");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF02_OtherSupervisingPhysicianID", "12345");
            //2330G OTHER PAYER BILLING PROVIDER
            //NM1 - Other Payer Billing Provider
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@NM102_OtherBillingProvideEntityTypeQlfr", "2");
            //REF - Other Payer Billing Provider Secondary Identification
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF01_OtherBillingProviderIdQlfr", "G2");
            oDaOtherSubscriberInfo.InsertCommand.Parameters.AddWithValue("@REF02_OtherBillingProviderID", "12345");
            nOtherSubscriberInfokey = (Int32)(decimal)oDaOtherSubscriberInfo.InsertCommand.ExecuteScalar();


            //INSERT A RECORD INTO SERVICELINE TABLE
            sSql = @"INSERT INTO [837X222_ServiceLine] (Claimskey, LX01_AssignedNumber, SV101_01_ProductServiceIdQualifier, SV101_02_ProcedureCode, 
                        SV101_03_ProcedureModifier1, SV101_04_ProcedureModifier2, SV101_05_ProcedureModifier3, SV101_06_ProcedureModifier4, 
                        SV101_07_ServiceDescription, SV102_LineItemChargeAmount, SV103_UnitForMeasurement_Code, SV104_ServiceUnitCount, 
                        SV105_PlaceOfServiceCode, SV107_01_DiagnosisCodePointer1, SV107_02_DiagnosisCodePointer2, SV107_03_DiagnosisCodePointer3, 
                        SV107_04_DiagnosisCodePointer4, SV109_EmergencyIndicator, SV111_EPSDT_Indicator, SV112_FamilyPlanningIndicator, 
                        SV115_CopayStatusCode, SV501_01_ProcedureIdentifier, SV501_02_ProcedureCode, SV503_DaysLengthOfMedicalNecissity, 
                        SV504_DME_RentalPrice, SV505_DME_PurchasePrice, SV506_RentalUnitPriceInidcator, PWK01_LineSupplementAttachReportTypeCode, 
                        PWK02_LineSupplementAttachTransmissnCode, PWK06_LineSupplementAttachControlNo, PWK02_DMERC_CMN_AttachTransmissnCode, 
                        CR102_PatientWeightLbs, CR104_AmbulanceTransportReasonCode, CR106_TransportDistanceMiles, CR109_RoundTripPurposeDescription, 
                        CR110_StretcherPurposeDescription, CR301_DMERC_CertificationTypeCode, CR303_DME_DurationMonths, 
                        CRC02_AmbulanceCertConditionIndicator, CRC03_AmbulanceCertConditionCode1, CRC04_AmbulanceCertConditionCode2, 
                        CRC05_AmbulanceCertConditionCode3, CRC06_AmbulanceCertConditionCode4, CRC07_AmbulanceCertConditionCode5, 
                        CRC02_HospiceEmployedProviderIndicator, CRC02_DMERC_ConditionIndicator, CRC03_DMERC_ConditionCode1, 
                        CRC04_DMERC_ConditionCode2, DTP03_ServiceDate, DTP03_PrescriptionDate, DTP03_CertificationRevisionDate, 
                        DTP03_BeginTherapyDate, DTP03_LastCertificationDate, DTP03_LastSeenDate, DTP01_LastTestQualifier, DTP03_LastTestDate, 
                        DTP03_ShippedDate, DTP03_LastXrayDate, DTP03_InitialTreatmentDate, QTY02_AmbulancePatientCount, 
                        QTY02_ObstetricAdditionalUnits, MEA01_TestResultReferenceIdCode, MEA02_TestResultQualifier, MEA03_TestResultValue, 
                        CN101_ContractTypeCode, CN102_ContractAmount, CN103_ContractPercentage, CN104_ContractCode, CN105_ContractTermsDiscPercent, 
                        CN106_ContractVersionIdentifier, REF02_RepricedLineItemRefNo, REF02_AdjustedRepricedLineItemRefNo, 
                        REF02_PriorAuthorizationNumber, REF04_02_PriorAuthorizOtherPayrPrimaryID, REF02_LineItemControlNumber, 
                        REF02_MammographyCertificationNumber, REF02_ClinicalLabImproveAmendment, REF02_ReferringCLIA_Number, 
                        REF02_ImmunizationBatchNumber, REF02_ReferralNumber, REF04_02_ReferralOtherPayerPrimaryID, AMT02_SalesTaxAmount, 
                        AMT02_PostageClaimedAmount, K301_FileInformation, NTE01_LineNoteReferenceCode, NTE02_LineNoteText, NTE01_ThirdPartyNoteCode, 
                        NTE02_ThirdPartyText, PS102_PurchasedServiceChargeAmnt, HCP01_LineRepriceCode, HCP02_RepricedAllowedAmount, 
                        HCP03_RepricedSavingAmount, HCP04_RepricingOrganizationID, HCP05_RepricingPerDiemFlatRateAmount, 
                        HCP06_RepricedApprovedAmbPatientGrpCode, HCP07_RepricedApprovedAmbPatientGroupAmnt, HCP09_RepricedServiceIdQualifier, 
                        HCP10_RepricedApprovedHCPCS_Code, HCP11_RepricedUnitMeasurementCode, HCP12_RepricedApprovedServiceUnitCount, 
                        HCP13_RepricedRejectReasonCode, HCP14_RepricedPolicyComplianceCode, HCP15_RepricedExceptionCode, LIN03_NationalDrugCode, 
                        CTP04_NationalDrugUnitCount, CTP05_01_UnitMeasurementCode, REF01_PrescriptionQualifier, REF02_PrescriptionNumber, 
                        NM102_RenderingProviderEntityTypeQlfr, NM103_RenderingProviderNameLastOrg, NM104_RenderingProviderFirst, 
                        NM105_RenderingProviderMiddle, NM107_RenderingProviderSuffix, NM109_RenderingProviderID, 
                        PRV03_RenderingProviderTaxonomyCode, REF01_RenderingProviderSecondaryQlfr, REF02_RenderingProviderSecondaryID, 
                        REF04_01_ReferenceIdQlfr, REF04_02_RenderingProviderSecondaryPayerID, NM102_PurchasedServiceProviderEntityType, 
                        NM109_PurchasedServiceProviderID, REF01_PurchasedServicProvidrSecondryIdQlfr, REF02_PurchasedServiceProviderSecondaryID, 
                        REF04_02_PurchsdServcProvdrSecndryPayrIdNo, NM103_ServiceFacilityName, NM109_ServiceFacilityID, N301_ServiceFacilityAddress1, 
                        N302_ServiceFacilityAddress2, N401_ServiceFacilityCity, N402_ServiceFacilityState, N403_ServiceFacilityZip, 
                        N404_ServiceFacilityCountryCode, N407_ServiceFacilityCountrySubdivision, REF01_ServiceFacilitySecondaryIdQlfr, 
                        REF02_ServiceFacilitySecondaryID, REF04_02_ServiceFaciltySecondryPayrIdNo, NM103_SupervisingProviderLastName, 
                        NM104_SupervisingProviderFirst, NM105_SupervisingProviderMiddle, NM107_SupervisingProviderSuffix, 
                        NM109_SupervisingProviderID, REF01_SupervisingProvidrSecondryIdQlfr, REF02_SupervisingProviderSecondaryID, 
                        REF04_02_SupervisngProvdrSecndryPayrIdNo, NM103_OrderingProviderLastName, NM104_OrderingProviderFirst, 
                        NM105_OrderingProviderMiddle, NM107_OrderingProviderSuffix, NM109_OrderingProviderID, N301_OrderingProviderAddress1, 
                        N302_OrderingProviderAddress2, N401_OrderingProviderCity, N402_OrderingProviderState, N403_OrderingProviderZip, 
                        N404_OrderingProviderCountryCode, N407_OrderingProviderCountrySubdivision, REF01_OrderingProviderSecondaryIdQlfr, 
                        REF02_OrderingProviderSecondaryID, REF04_02_OrderingProviderSecondaryIdNo, PER02_OrderingProviderContactName, 
                        PER0X_OrderingProviderContactTelephone, PER0X_OrderingProviderContactExtension, PER0X_OrderingProviderContactFax, 
                        PER0X_OrderingProviderContactEmail, NM101_ReferringProviderEntityIdfr, NM103_ReferringProviderLastName, 
                        NM104_ReferringProviderFirst, NM105_ReferringProviderMiddle, NM107_ReferringProviderSuffix, NM109_ReferringProviderID, 
                        REF01_ReferringProviderSecondaryIdQlfr, REF02_ReferringProviderSecondaryID, REF04_02_ReferngProvdrSecndryPayrIdNo, 
                        N301_AmbulancePickupAddress1, N302_AmbulancePickupAddress2, 
                        N401_AmbulancePickupCity, N402_AmbulancePickupState, N403_AmbulancePickupZip, N404_AmbulancePickupCountryCode, 
                        N407_AmbulncePickupCntrySubdivisn, NM103_AmbulanceDropOffName, N301_AmbulanceDropOffAddress1, 
                        N302_AmbulanceDropOffAddress2, N401_AmbulanceDropOffCity, N402_AmbulanceDropOffState, N403_AmbulanceDropOffZip, 
                        N404_AmbulanceDropOffCountryCode, N407_AmbulnceDropOffCntrySubdivisn) 
                        values 
                        (@Claimskey, @LX01_AssignedNumber, @SV101_01_ProductServiceIdQualifier, @SV101_02_ProcedureCode, 
                        @SV101_03_ProcedureModifier1, @SV101_04_ProcedureModifier2, @SV101_05_ProcedureModifier3, @SV101_06_ProcedureModifier4, 
                        @SV101_07_ServiceDescription, @SV102_LineItemChargeAmount, @SV103_UnitForMeasurement_Code, @SV104_ServiceUnitCount, 
                        @SV105_PlaceOfServiceCode, @SV107_01_DiagnosisCodePointer1, @SV107_02_DiagnosisCodePointer2, @SV107_03_DiagnosisCodePointer3, 
                        @SV107_04_DiagnosisCodePointer4, @SV109_EmergencyIndicator, @SV111_EPSDT_Indicator, @SV112_FamilyPlanningIndicator, 
                        @SV115_CopayStatusCode, @SV501_01_ProcedureIdentifier, @SV501_02_ProcedureCode, @SV503_DaysLengthOfMedicalNecissity, 
                        @SV504_DME_RentalPrice, @SV505_DME_PurchasePrice, @SV506_RentalUnitPriceInidcator, @PWK01_LineSupplementAttachReportTypeCode, 
                        @PWK02_LineSupplementAttachTransmissnCode, @PWK06_LineSupplementAttachControlNo, @PWK02_DMERC_CMN_AttachTransmissnCode, 
                        @CR102_PatientWeightLbs, @CR104_AmbulanceTransportReasonCode, @CR106_TransportDistanceMiles, @CR109_RoundTripPurposeDescription, 
                        @CR110_StretcherPurposeDescription, @CR301_DMERC_CertificationTypeCode, @CR303_DME_DurationMonths, 
                        @CRC02_AmbulanceCertConditionIndicator, @CRC03_AmbulanceCertConditionCode1, @CRC04_AmbulanceCertConditionCode2, 
                        @CRC05_AmbulanceCertConditionCode3, @CRC06_AmbulanceCertConditionCode4, @CRC07_AmbulanceCertConditionCode5, 
                        @CRC02_HospiceEmployedProviderIndicator, @CRC02_DMERC_ConditionIndicator, @CRC03_DMERC_ConditionCode1, 
                        @CRC04_DMERC_ConditionCode2, @DTP03_ServiceDate, @DTP03_PrescriptionDate, @DTP03_CertificationRevisionDate, 
                        @DTP03_BeginTherapyDate, @DTP03_LastCertificationDate, @DTP03_LastSeenDate, @DTP01_LastTestQualifier, @DTP03_LastTestDate, 
                        @DTP03_ShippedDate, @DTP03_LastXrayDate, @DTP03_InitialTreatmentDate, @QTY02_AmbulancePatientCount, 
                        @QTY02_ObstetricAdditionalUnits, @MEA01_TestResultReferenceIdCode, @MEA02_TestResultQualifier, @MEA03_TestResultValue, 
                        @CN101_ContractTypeCode, @CN102_ContractAmount, @CN103_ContractPercentage, @CN104_ContractCode, @CN105_ContractTermsDiscPercent, 
                        @CN106_ContractVersionIdentifier, @REF02_RepricedLineItemRefNo, @REF02_AdjustedRepricedLineItemRefNo,
                        @REF02_PriorAuthorizationNumber, @REF04_02_PriorAuthorizOtherPayrPrimaryID, @REF02_LineItemControlNumber, 
                        @REF02_MammographyCertificationNumber, @REF02_ClinicalLabImproveAmendment, @REF02_ReferringCLIA_Number, 
                        @REF02_ImmunizationBatchNumber, @REF02_ReferralNumber, @REF04_02_ReferralOtherPayerPrimaryID, @AMT02_SalesTaxAmount, 
                        @AMT02_PostageClaimedAmount, @K301_FileInformation, @NTE01_LineNoteReferenceCode, @NTE02_LineNoteText, @NTE01_ThirdPartyNoteCode, 
                        @NTE02_ThirdPartyText, @PS102_PurchasedServiceChargeAmnt, @HCP01_LineRepriceCode, @HCP02_RepricedAllowedAmount, 
                        @HCP03_RepricedSavingAmount, @HCP04_RepricingOrganizationID, @HCP05_RepricingPerDiemFlatRateAmount,
                        @HCP06_RepricedApprovedAmbPatientGrpCode, @HCP07_RepricedApprovedAmbPatientGroupAmnt, @HCP09_RepricedServiceIdQualifier, 
                        @HCP10_RepricedApprovedHCPCS_Code, @HCP11_RepricedUnitMeasurementCode, @HCP12_RepricedApprovedServiceUnitCount, 
                        @HCP13_RepricedRejectReasonCode, @HCP14_RepricedPolicyComplianceCode, @HCP15_RepricedExceptionCode, @LIN03_NationalDrugCode, 
                        @CTP04_NationalDrugUnitCount, @CTP05_01_UnitMeasurementCode, @REF01_PrescriptionQualifier, @REF02_PrescriptionNumber, 
                        @NM102_RenderingProviderEntityTypeQlfr, @NM103_RenderingProviderNameLastOrg, @NM104_RenderingProviderFirst, 
                        @NM105_RenderingProviderMiddle, @NM107_RenderingProviderSuffix, @NM109_RenderingProviderID, 
                        @PRV03_RenderingProviderTaxonomyCode, @REF01_RenderingProviderSecondaryQlfr, @REF02_RenderingProviderSecondaryID, 
                        @REF04_01_ReferenceIdQlfr, @REF04_02_RenderingProviderSecondaryPayerID, @NM102_PurchasedServiceProviderEntityType, 
                        @NM109_PurchasedServiceProviderID, @REF01_PurchasedServicProvidrSecondryIdQlfr, @REF02_PurchasedServiceProviderSecondaryID, 
                        @REF04_02_PurchsdServcProvdrSecndryPayrIdNo, @NM103_ServiceFacilityName, @NM109_ServiceFacilityID, @N301_ServiceFacilityAddress1, 
                        @N302_ServiceFacilityAddress2, @N401_ServiceFacilityCity, @N402_ServiceFacilityState, @N403_ServiceFacilityZip, 
                        @N404_ServiceFacilityCountryCode, @N407_ServiceFacilityCountrySubdivision, @REF01_ServiceFacilitySecondaryIdQlfr, 
                        @REF02_ServiceFacilitySecondaryID, @REF04_02_ServiceFaciltySecondryPayrIdNo, @NM103_SupervisingProviderLastName, 
                        @NM104_SupervisingProviderFirst, @NM105_SupervisingProviderMiddle, @NM107_SupervisingProviderSuffix, 
                        @NM109_SupervisingProviderID, @REF01_SupervisingProvidrSecondryIdQlfr, @REF02_SupervisingProviderSecondaryID, 
                        @REF04_02_SupervisngProvdrSecndryPayrIdNo, @NM103_OrderingProviderLastName, @NM104_OrderingProviderFirst, 
                        @NM105_OrderingProviderMiddle, @NM107_OrderingProviderSuffix, @NM109_OrderingProviderID, @N301_OrderingProviderAddress1, 
                        @N302_OrderingProviderAddress2, @N401_OrderingProviderCity, @N402_OrderingProviderState, @N403_OrderingProviderZip, 
                        @N404_OrderingProviderCountryCode, @N407_OrderingProviderCountrySubdivision, @REF01_OrderingProviderSecondaryIdQlfr,
                        @REF02_OrderingProviderSecondaryID, @REF04_02_OrderingProviderSecondaryIdNo, @PER02_OrderingProviderContactName, 
                        @PER0X_OrderingProviderContactTelephone, @PER0X_OrderingProviderContactExtension, @PER0X_OrderingProviderContactFax, 
                        @PER0X_OrderingProviderContactEmail, @NM101_ReferringProviderEntityIdfr, @NM103_ReferringProviderLastName, 
                        @NM104_ReferringProviderFirst, @NM105_ReferringProviderMiddle, @NM107_ReferringProviderSuffix, @NM109_ReferringProviderID, 
                        @REF01_ReferringProviderSecondaryIdQlfr, @REF02_ReferringProviderSecondaryID, @REF04_02_ReferngProvdrSecndryPayrIdNo, 
                        @N301_AmbulancePickupAddress1, @N302_AmbulancePickupAddress2,
                        @N401_AmbulancePickupCity, @N402_AmbulancePickupState, @N403_AmbulancePickupZip, @N404_AmbulancePickupCountryCode, 
                        @N407_AmbulncePickupCntrySubdivisn, @NM103_AmbulanceDropOffName, @N301_AmbulanceDropOffAddress1, 
                        @N302_AmbulanceDropOffAddress2, @N401_AmbulanceDropOffCity, @N402_AmbulanceDropOffState, @N403_AmbulanceDropOffZip, 
                        @N404_AmbulanceDropOffCountryCode, @N407_AmbulnceDropOffCntrySubdivisn);
                        SELECT scope_identity()";

            oDaServiceLine.InsertCommand = new SqlCommand(sSql, oConnection);
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@Claimskey", nClaimskey);
            //2400 SERVICE LINE
            //LX - Service Line Number
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@LX01_AssignedNumber", "");
            //SV1 - Professional Service
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV101_01_ProductServiceIdQualifier", "HC");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV101_02_ProcedureCode", "99213");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV101_03_ProcedureModifier1", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV101_04_ProcedureModifier2", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV101_05_ProcedureModifier3", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV101_06_ProcedureModifier4", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV101_07_ServiceDescription", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV102_LineItemChargeAmount", "40");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV103_UnitForMeasurement_Code", "UN");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV104_ServiceUnitCount", "1");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV105_PlaceOfServiceCode", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV107_01_DiagnosisCodePointer1", "1");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV107_02_DiagnosisCodePointer2", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV107_03_DiagnosisCodePointer3", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV107_04_DiagnosisCodePointer4", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV109_EmergencyIndicator", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV111_EPSDT_Indicator", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV112_FamilyPlanningIndicator", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV115_CopayStatusCode", "");
            //SV5 - Durable Medical Equipment Service
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV501_01_ProcedureIdentifier", "HC");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV501_02_ProcedureCode", "A4631");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV503_DaysLengthOfMedicalNecissity", "30");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV504_DME_RentalPrice", "50");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV505_DME_PurchasePrice", "5000");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@SV506_RentalUnitPriceInidcator", "4");
            //PWK - Line Supplemental Information 
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@PWK01_LineSupplementAttachReportTypeCode", "OZ");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@PWK02_LineSupplementAttachTransmissnCode", "BM");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@PWK06_LineSupplementAttachControlNo", "DMN0012");
            //PWK - Durable Medical Equipment Certificate of Medical 
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@PWK02_DMERC_CMN_AttachTransmissnCode", "AB");
            //CR1 - Ambulance Transport Information
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CR102_PatientWeightLbs", "140");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CR104_AmbulanceTransportReasonCode", "A");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CR106_TransportDistanceMiles", "12");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CR109_RoundTripPurposeDescription", "DESC");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CR110_StretcherPurposeDescription", "");
            //CR3 - Durable Medical Equipment Certification
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CR301_DMERC_CertificationTypeCode", "I");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CR303_DME_DurationMonths", "6");
            //CRC - Ambulance Certification
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CRC02_AmbulanceCertConditionIndicator", "Y");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CRC03_AmbulanceCertConditionCode1", "01");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CRC04_AmbulanceCertConditionCode2", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CRC05_AmbulanceCertConditionCode3", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CRC06_AmbulanceCertConditionCode4", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CRC07_AmbulanceCertConditionCode5", "");
            //CRC - Hospice Employee Indicator 
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CRC02_HospiceEmployedProviderIndicator", "Y");
            //CRC - Condition Indicator/Durable Medical Equipment
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CRC02_DMERC_ConditionIndicator", "Y");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CRC03_DMERC_ConditionCode1", "ZV");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CRC04_DMERC_ConditionCode2", "");
            //DTP - Service Date
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@DTP03_ServiceDate", "20061003");
            //DTP - Prescription Date
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@DTP03_PrescriptionDate", "20050108");
            //DTP - Certification Revision/Recertification Date
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@DTP03_CertificationRevisionDate", "20050108");
            //DTP - Begin Therapy Date
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@DTP03_BeginTherapyDate", "20050108");
            //DTP - Last Certification Date
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@DTP03_LastCertificationDate", "20050108");
            //DTP - Last Seen Date
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@DTP03_LastSeenDate", "20050108");
            //DTP - Test Date
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@DTP01_LastTestQualifier", "738");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@DTP03_LastTestDate", "20050108");
            //DTP - Shipped Date
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@DTP03_ShippedDate", "20050108");
            //DTP - Last X-ray Date
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@DTP03_LastXrayDate", "20050108");
            //DTP - Initial Treatment Date
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@DTP03_InitialTreatmentDate", "20050108");
            //QTY - Ambulance Patient Count
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@QTY02_AmbulancePatientCount", "2");
            //QTY - Obstetric Anesthesia Additional Units  
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@QTY02_ObstetricAdditionalUnits", "3");
            //MEA - Test Result
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@MEA01_TestResultReferenceIdCode", "TR");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@MEA02_TestResultQualifier", "R1");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@MEA03_TestResultValue", "113.4");
            //CN1 - Contract Information 
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CN101_ContractTypeCode", "02");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CN102_ContractAmount", "550");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CN103_ContractPercentage", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CN104_ContractCode", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CN105_ContractTermsDiscPercent", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CN106_ContractVersionIdentifier", "");
            //REF - Repriced Line Item Reference Number 
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_RepricedLineItemRefNo", "9B");
            //REF - Adjusted Repriced Line Item Reference Number
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_AdjustedRepricedLineItemRefNo", "44444");
            //REF - Prior Authorization
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_PriorAuthorizationNumber", "9D");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF04_02_PriorAuthorizOtherPayrPrimaryID", "4444");
            //REF - Line Item Control Number
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_LineItemControlNumber", "54321");
            //REF -  Mammography Certification Number
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_MammographyCertificationNumber", "T554");
            //REF -  Clinical Laboratory Improvement Amendment (CLIA)
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_ClinicalLabImproveAmendment", "34D1234567");
            //REF -  Referring Clinical Laboratory Improvement Amendment
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_ReferringCLIA_Number", "34D1234567");
            //REF -  Immunization Batch Number
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_ImmunizationBatchNumber", "DTP22333444");
            //REF -  Referral Number
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_ReferralNumber", "12345");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF04_02_ReferralOtherPayerPrimaryID", "65432");
            //AMT - Sales Tax Amount
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@AMT02_SalesTaxAmount", "45");
            //AMT - Postage Claimed Amount
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@AMT02_PostageClaimedAmount", "56.78");
            //K3 - File Information 
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@K301_FileInformation", "STATE DATA REQUIREMENT");
            //NTE - Line Note
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NTE01_LineNoteReferenceCode", "DCP");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NTE02_LineNoteText", "ATIENT GOAL TO BE OFF OXYGEN BY END OF MONTH");
            //NTE - Third Party Organization Notes
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NTE01_ThirdPartyNoteCode", "TPO");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NTE02_ThirdPartyText", "STATE REGULATION 123 WAS APPLIED DURING THE PRICING OF THIS CLAIM");
            //PS1 - Purchased Service Information 
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@PS102_PurchasedServiceChargeAmnt", "110");
            //HCP - Line Pricing/Repricing Information 
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP01_LineRepriceCode", "03");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP02_RepricedAllowedAmount", "100");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP03_RepricedSavingAmount", "10");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP04_RepricingOrganizationID", "RPO12345");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP05_RepricingPerDiemFlatRateAmount", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP06_RepricedApprovedAmbPatientGrpCode", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP07_RepricedApprovedAmbPatientGroupAmnt", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP09_RepricedServiceIdQualifier", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP10_RepricedApprovedHCPCS_Code", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP11_RepricedUnitMeasurementCode", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP12_RepricedApprovedServiceUnitCount", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP13_RepricedRejectReasonCode", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP14_RepricedPolicyComplianceCode", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@HCP15_RepricedExceptionCode", "");
            //2410 DRUG IDENTIFICATION
            //LIN - Item Identification
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@LIN03_NationalDrugCode", "01234567891");
            //CTP - Drug Quantity 
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CTP04_NationalDrugUnitCount", "2");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@CTP05_01_UnitMeasurementCode", "UN");
            //REF - Prescription or Compound Drug Association Number
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF01_PrescriptionQualifier", "XZ");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_PrescriptionNumber", "123456");
            //2420A RENDERING PROVIDER NAME
            //NM1 - Rendering Provider Name
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM102_RenderingProviderEntityTypeQlfr", "1");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM103_RenderingProviderNameLastOrg", "DOE");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM104_RenderingProviderFirst", "JANE");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM105_RenderingProviderMiddle", "C");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM107_RenderingProviderSuffix", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM109_RenderingProviderID", "1234567804");
            //PRV - Rendering Provider Specialty Information
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@PRV03_RenderingProviderTaxonomyCode", "1223G0001X");
            //REF - Rendering Provider Secondary Identification
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF01_RenderingProviderSecondaryQlfr", "G2");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_RenderingProviderSecondaryID", "12345");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF04_01_ReferenceIdQlfr", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF04_02_RenderingProviderSecondaryPayerID", "");
            //2420B PURCHASED SERVICE PROVIDER NAME
            //NM1 - Purchased Service Provider Name
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM102_PurchasedServiceProviderEntityType", "2");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM109_PurchasedServiceProviderID", "1234567891");
            //REF - Purchased Service Provider Secondary Identification
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF01_PurchasedServicProvidrSecondryIdQlfr", "G2");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_PurchasedServiceProviderSecondaryID", "12345");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF04_02_PurchsdServcProvdrSecndryPayrIdNo", "");
            //2420C SERVICE FACILITY LOCATION NAME
            //NM1 - Service Facility Location Name
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM103_ServiceFacilityName", "ABC CLINIC");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM109_ServiceFacilityID", "1234567891");
            //N3 - Service Facility Location Address
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N301_ServiceFacilityAddress1", "123 MAIN STREET");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N302_ServiceFacilityAddress2", "");
            //N4 - Service Facility Location City, State, ZIP Code
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N401_ServiceFacilityCity", "KANSAS");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N402_ServiceFacilityState", "MO");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N403_ServiceFacilityZip", "64108");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N404_ServiceFacilityCountryCode", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N407_ServiceFacilityCountrySubdivision", "");
            //REF - Service Facility Location Secondary Identification
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF01_ServiceFacilitySecondaryIdQlfr", "G2");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_ServiceFacilitySecondaryID", "12345");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF04_02_ServiceFaciltySecondryPayrIdNo", "6780");
            //2420D SUPERVISING PROVIDER NAME
            //NM1 - Supervising Provider Name
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM103_SupervisingProviderLastName", "DOE");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM104_SupervisingProviderFirst", "JOHN");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM105_SupervisingProviderMiddle", "B");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM107_SupervisingProviderSuffix", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM109_SupervisingProviderID", "1234567891");
            //REF - Supervising Provider Secondary Identification
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF01_SupervisingProvidrSecondryIdQlfr", "G2");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_SupervisingProviderSecondaryID", "12345");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF04_02_SupervisngProvdrSecndryPayrIdNo", "");
            //2420E ORDERING PROVIDER NAME
            //NM1 - Ordering Provider Location Name
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM103_OrderingProviderLastName", "RICHARDSON");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM104_OrderingProviderFirst", "TRENT");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM105_OrderingProviderMiddle", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM107_OrderingProviderSuffix", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM109_OrderingProviderID", "1234567891");
            //N3 - Ordering Provider Location Address 
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N301_OrderingProviderAddress1", "123 MAIN STREET");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N302_OrderingProviderAddress2", "");
            //N4 - Ordering Provider Location City, State, ZIP Code 
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N401_OrderingProviderCity", "KANSAS");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N402_OrderingProviderState", "MO");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N403_OrderingProviderZip", "64108");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N404_OrderingProviderCountryCode", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N407_OrderingProviderCountrySubdivision", "");
            //REF - Ordering Provider Location Secondary Identification
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF01_OrderingProviderSecondaryIdQlfr", "G2");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_OrderingProviderSecondaryID", "12345");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF04_02_OrderingProviderSecondaryIdNo", "");
            //PER - Ordering Provider Contact Information  
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@PER02_OrderingProviderContactName", "JOHN SMITH");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@PER0X_OrderingProviderContactTelephone", "5555551234");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@PER0X_OrderingProviderContactExtension", "123");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@PER0X_OrderingProviderContactFax", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@PER0X_OrderingProviderContactEmail", "");
            //2420F REFERRING PROVIDER NAME
            //NM1 - Referring Provider Name
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM101_ReferringProviderEntityIdfr", "DN");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM103_ReferringProviderLastName", "SMITH");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM104_ReferringProviderFirst", "TED");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM105_ReferringProviderMiddle", "W");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM107_ReferringProviderSuffix", "JR");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM109_ReferringProviderID", "1234567891");
            //REF - Referring Provider Secondary Identification
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF01_ReferringProviderSecondaryIdQlfr", "G2");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF02_ReferringProviderSecondaryID", "12345");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@REF04_02_ReferngProvdrSecndryPayrIdNo", "");
            //2420G AMBULANCE PICK-UP LOCATION
            //N3 - Ambulance Pick-up Location Address
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N301_AmbulancePickupAddress1", "123 MAIN STREET");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N302_AmbulancePickupAddress2", "");
            //N4 - Ambulance Pick-up Location City, State, ZIP Code
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N401_AmbulancePickupCity", "KANSAS");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N402_AmbulancePickupState", "MO");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N403_AmbulancePickupZip", "64108");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N404_AmbulancePickupCountryCode", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N407_AmbulncePickupCntrySubdivisn", "");
            //2420H AMBULANCE DROP-OFF LOCATION
            //NM1 - Ambulance Drop-off Location
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@NM103_AmbulanceDropOffName", "");
            //N3 - Ambulance Drop-off Location Address 
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N301_AmbulanceDropOffAddress1", "123 MAIN STREET");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N302_AmbulanceDropOffAddress2", "");
            //N4 - Ambulance Drop-off Location City, State, ZIP Code
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N401_AmbulanceDropOffCity", "KANSAS");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N402_AmbulanceDropOffState", "MO");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N403_AmbulanceDropOffZip", "64108");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N404_AmbulanceDropOffCountryCode", "");
            oDaServiceLine.InsertCommand.Parameters.AddWithValue("@N407_AmbulnceDropOffCntrySubdivisn", "");
            nServiceLinekey = (Int32)(decimal)oDaServiceLine.InsertCommand.ExecuteScalar();
         

            //INSERT A RECORD INTO SERVICELINEADJ TABLE
            sSql = @"INSERT INTO [837X222_ServiceLineAdj] (ServiceLinekey, SVD01_OtherPayerPrimaryIdfr, SVD02_ServiceLinePaidAmount, 
                        SVD03_01_ProductServiceIdQlfr, SVD03_02_ProcedureCode, SVD03_03_ProcedureModifier1, SVD03_04_ProcedurModifier2, 
                        SVD03_05_ProcedureModifier3, SVD03_06_ProcedureModifier4, SVD03_07_ProcedureCodeDesc, SVD05_PaidServiceUnitCount, 
                        SVD06_LineNumber, CAS01_AdjustmentReasonCode, CAS02_AdjustmentAmount, CAS03_AdjustmentQuantity, CAS04_Quantity, 
                        CAS05_AdjustmentReasonCode2, CAS06_AdjustmentAmount2, CAS07_AdjustmentQuantity2, CAS08_AdjustmentReasonCode3, 
                        CAS09_AdjustmentAmount3, CAS10_AdjustmentQuantity3, CAS11_AdjustmentReasonCode4, CAS12_AdjustmentAmount4, 
                        CAS13_AdjustmentQuantity4, CAS14_AdjustmentReasonCode5, CAS15_AdjustmentAmount5, CAS16_AdjustmentQuantity5, 
                        CAS17_AdjustmentReasonCode6, CAS18_AdjustmentAmount6, CAS19_AdjustmentQuantity6, DTP03_AdjudicationPaymentDate, 
                        AMT02_RemainingPatientLiability) 
                        values 
                        (@ServiceLinekey, @SVD01_OtherPayerPrimaryIdfr, @SVD02_ServiceLinePaidAmount, 
                        @SVD03_01_ProductServiceIdQlfr, @SVD03_02_ProcedureCode, @SVD03_03_ProcedureModifier1, @SVD03_04_ProcedurModifier2, 
                        @SVD03_05_ProcedureModifier3, @SVD03_06_ProcedureModifier4, @SVD03_07_ProcedureCodeDesc, @SVD05_PaidServiceUnitCount, 
                        @SVD06_LineNumber, @CAS01_AdjustmentReasonCode, @CAS02_AdjustmentAmount, @CAS03_AdjustmentQuantity, @CAS04_Quantity,
                        @CAS05_AdjustmentReasonCode2, @CAS06_AdjustmentAmount2, @CAS07_AdjustmentQuantity2, @CAS08_AdjustmentReasonCode3, 
                        @CAS09_AdjustmentAmount3, @CAS10_AdjustmentQuantity3, @CAS11_AdjustmentReasonCode4, @CAS12_AdjustmentAmount4, 
                        @CAS13_AdjustmentQuantity4, @CAS14_AdjustmentReasonCode5, @CAS15_AdjustmentAmount5, @CAS16_AdjustmentQuantity5, 
                        @CAS17_AdjustmentReasonCode6, @CAS18_AdjustmentAmount6, @CAS19_AdjustmentQuantity6, @DTP03_AdjudicationPaymentDate, 
                        @AMT02_RemainingPatientLiability);
                        SELECT scope_identity()";

            oDaServiceLineAdj.InsertCommand = new SqlCommand(sSql, oConnection);
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@ServiceLinekey", nServiceLinekey);
            //2430 LINE ADJUDICATION INFORMATION
            //SVD - Line Adjudication Information
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@SVD01_OtherPayerPrimaryIdfr", "43");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@SVD02_ServiceLinePaidAmount", "55");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@SVD03_01_ProductServiceIdQlfr", "HC");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@SVD03_02_ProcedureCode", "84550");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@SVD03_03_ProcedureModifier1", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@SVD03_04_ProcedurModifier2", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@SVD03_05_ProcedureModifier3", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@SVD03_06_ProcedureModifier4", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@SVD03_07_ProcedureCodeDesc", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@SVD05_PaidServiceUnitCount", "3");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@SVD06_LineNumber", "");
            //CAS - Line Adjustment 
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS01_AdjustmentReasonCode", "PR");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS02_AdjustmentAmount", "1");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS03_AdjustmentQuantity", "7.93");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS04_Quantity", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS05_AdjustmentReasonCode2", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS06_AdjustmentAmount2", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS07_AdjustmentQuantity2", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS08_AdjustmentReasonCode3", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS09_AdjustmentAmount3", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS10_AdjustmentQuantity3", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS11_AdjustmentReasonCode4", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS12_AdjustmentAmount4", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS13_AdjustmentQuantity4", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS14_AdjustmentReasonCode5", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS15_AdjustmentAmount5", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS16_AdjustmentQuantity5", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS17_AdjustmentReasonCode6", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS18_AdjustmentAmount6", "");
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@CAS19_AdjustmentQuantity6", "");
            //DTP - Date or Time or Period 
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@DTP03_AdjudicationPaymentDate", "20040203");
            //AMT - Remaining Patient Liability 
            oDaServiceLineAdj.InsertCommand.Parameters.AddWithValue("@AMT02_RemainingPatientLiability", "75");
            nServiceLineAdjkey = (Int32)(decimal)oDaServiceLineAdj.InsertCommand.ExecuteScalar();


            //INSERT A RECORD INTO SERVICEDOCUMENT TABLE
            sSql = @"INSERT INTO [837X222_ServiceDocument] (ServiceLinekey, LQ01_CodeListQualifierCode, LQ02_FormIdentifier, 
                        FRM01_QuestionNumberLetter, FRM02_QuestionResponseCode, FRM03_QuestionResponse, FRM04_QuestionResponseDate, 
                        FRM05_QuestionResponsePercent) 
                        values 
                        (@ServiceLinekey, @LQ01_CodeListQualifierCode, @LQ02_FormIdentifier, 
                        @FRM01_QuestionNumberLetter, @FRM02_QuestionResponseCode, @FRM03_QuestionResponse, @FRM04_QuestionResponseDate, 
                        @FRM05_QuestionResponsePercent);
                        SELECT scope_identity()";

            oDaServiceDocument.InsertCommand = new SqlCommand(sSql, oConnection);
            oDaServiceDocument.InsertCommand.Parameters.AddWithValue("@ServiceLinekey", nServiceLinekey);
            //2440 FORM IDENTIFICATION CODE
            //LQ - Form Identification Code
            oDaServiceDocument.InsertCommand.Parameters.AddWithValue("@LQ01_CodeListQualifierCode", "UT");
            oDaServiceDocument.InsertCommand.Parameters.AddWithValue("@LQ02_FormIdentifier", "01.02");
            //FRM - Supporting Documentation  
            oDaServiceDocument.InsertCommand.Parameters.AddWithValue("@FRM01_QuestionNumberLetter", "1A");
            oDaServiceDocument.InsertCommand.Parameters.AddWithValue("@FRM02_QuestionResponseCode", "");
            oDaServiceDocument.InsertCommand.Parameters.AddWithValue("@FRM03_QuestionResponse", "J0234");
            oDaServiceDocument.InsertCommand.Parameters.AddWithValue("@FRM04_QuestionResponseDate", "");
            oDaServiceDocument.InsertCommand.Parameters.AddWithValue("@FRM05_QuestionResponsePercent", "");
            nServiceDocumentkey = (Int32)(decimal)oDaServiceDocument.InsertCommand.ExecuteScalar();

            oDaServiceLineAdj.Dispose();
            oDaServiceDocument.Dispose();
            oDaServiceLine.Dispose();
            oDaClaims.Dispose();
            oDaDependent.Dispose();
            oDaOtherSubscriberInfo.Dispose();
            oDaSubscriber.Dispose();
            oDaInfoSource.Dispose();
            oDaHeader.Dispose();
            oDaFuncGroup.Dispose();
            oDaInterchange.Dispose();


            oConnection.Close();

            MessageBox.Show("Record added");
        }

        private void btnDelRec_Click(object sender, EventArgs e)
        {

            SqlDataAdapter oDa = new SqlDataAdapter();
            oConnection = new SqlConnection(sConnection);
            oConnection.Open();

            string sSql;

            sSql = "delete [837X222_ServiceDocument]";
            oDa.DeleteCommand = new SqlCommand(sSql, oConnection);
            oDa.DeleteCommand.ExecuteNonQuery();


            sSql = "delete [837X222_ServiceLineAdj]";
            oDa.DeleteCommand = new SqlCommand(sSql, oConnection);
            oDa.DeleteCommand.ExecuteNonQuery();


            sSql = "delete [837X222_ServiceLine]";
            oDa.DeleteCommand = new SqlCommand(sSql, oConnection);
            oDa.DeleteCommand.ExecuteNonQuery();


            sSql = "delete [837X222_OtherSubscriberInfo]";
            oDa.DeleteCommand = new SqlCommand(sSql, oConnection);
            oDa.DeleteCommand.ExecuteNonQuery();


            sSql = "delete [837X222_Claims]";
            oDa.DeleteCommand = new SqlCommand(sSql, oConnection);
            oDa.DeleteCommand.ExecuteNonQuery();


            sSql = "delete [837X222_Dependent]";
            oDa.DeleteCommand = new SqlCommand(sSql, oConnection);
            oDa.DeleteCommand.ExecuteNonQuery();


            sSql = "delete [837X222_Subscriber]";
            oDa.DeleteCommand = new SqlCommand(sSql, oConnection);
            oDa.DeleteCommand.ExecuteNonQuery();


            sSql = "delete [837X222_InfoSource]";
            oDa.DeleteCommand = new SqlCommand(sSql, oConnection);
            oDa.DeleteCommand.ExecuteNonQuery();


            sSql = "delete [837X222_Header]";
            oDa.DeleteCommand = new SqlCommand(sSql, oConnection);
            oDa.DeleteCommand.ExecuteNonQuery();


            sSql = "delete[FuncGroup]";
            oDa.DeleteCommand = new SqlCommand(sSql, oConnection);
            oDa.DeleteCommand.ExecuteNonQuery();

            sSql = "delete [Interchange]";
            oDa.DeleteCommand = new SqlCommand(sSql, oConnection);
            oDa.DeleteCommand.ExecuteNonQuery();

            oDa.Dispose();
            oConnection.Close();

            MessageBox.Show("Done");

        }
    }
}
