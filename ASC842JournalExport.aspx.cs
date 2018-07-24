/*
File Name		:	ASC842JournalExport.aspx
Author			:   Karthikeyan C
Created Date	:	07-13-2018
Purpose			:   Getting Export Fields Setting
Business Object	:   Journal, JournalManager 
 
   

----------------------------------------------------------------------
''''''''''''''''''''''''''''''''''''''''''
*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using NTrust.Common;
using NTrust.K2.BO;
using System.IO;
using System.Text;

namespace K2Web.Utility
{
    /// <summary>
    ///		Summary description for BatchEceptionOutPut.
    /// </summary>
    public partial class Asc842Export : NTrustSessionPage
    {

        //Constant variable declaration
        private const string SAVE_FILE_PATH = "SaveFilePath";//Changed to Get Save file path.
        private const string FIELDSLIST = "APEXPORTFIELDSLIST";
        private const string FIELDSETTINGS = "APEXPORTFIELDSETTINGS";
        private const string DELIMITCHAR = "APExportDelimitChar";
        private const string EXPORTDIRECTORY = "APExportDirectory";
        private const string GLOBAL_TYPE = "GLOBAL_TYPE";
        private const string GLOBAL_DESC = "GLOBAL_DESC";
        private const string GLOBAL_CODE = "GLOBAL_CODE";
        private const string SELECT_VARNAME = "VARNAME";
        private const string SELECT_FIELDNAME = "VALUE1";
        private const string SELECT_FIELDLEN = "VALUE2";
        private const string SELECT_FIELDKEY = "VALUE3";
        private const string SELECT_FIELDVALUE = "VALUE_NUMBER";
        private const string SELECT_FKEY = "VALUE_FKEY";
        private const string ADD_FILE_PATH = "APExport\\";

        private const string strExportType = "P";
        // Local Variables are declare here.
        protected string ErrExist;
        private string strSettings;
        private string strDisplayName;
        private string strMenuKey;
        private string strFilter = "";
        private string strSort = "";

        private string strDEFAULT_SELECTION_TEXT = System.Configuration.ConfigurationSettings.AppSettings["Select"];

        private string strDEFAULT_SELECTION_VALUE = System.Configuration.ConfigurationSettings.AppSettings["EmptyValue"];
        string file_name = "NTrust-edi-";
        string mon = DateTime.Today.Month.ToString();
        string day = DateTime.Today.Day.ToString();
        string strPkey = string.Empty;
        protected string strRedirectPath = ""; //added by Biru.
        private string ExportFile;
        protected string strErrMessage;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Getting Query String  Values to Local Variables
            strSettings = Request.QueryString["ParentName"];
            strDisplayName = Request.QueryString["DisplayName"];
            strMenuKey = Request.QueryString["MenuKey"];
            strPkey = Request.QueryString["Hkey"];
            NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
            ExportFile = GetString("ExportFile");
            lblTypeLedger.Text = GetString("ASC842TEMPLATENAME");
            strErrMessage = GetString("PLEASESELECTTEMPLATENAME");
            try
            {
                if (this.hidchkFlag.Value == "Y")
                {
                    this.hidchkFlag.Value = "N";
                    string scriptString = "<script language=JavaScript>";
                    scriptString += "dialogArguments.document.forms[0].hidchkFlag.value='Y';dialogArguments.document.forms[0].submit();";
                    scriptString += " window.close();</script>";
                    this.RegisterStartupScript("Windclose", scriptString);
                }
                //if (!this.IsPostBack || hdnSelRow.Value == "true")
                if (!this.IsPostBack)
                {
                    //string confirm = "return isValid()";
                    //btnExport.Attributes.Add("onclick",confirm);
                    hdnSelRow.Value = "0";
                    InitializePageProperties();
                    //FillDelimiter();
                    //GetFilterDetails();
                    //FillFieldsList();
                    //SetDefaultValue();
                    FillTypeofTemplate();

                }
                //added by Biru to handle session expire problem
                NTrust.Common.Session Sess_user = (NTrust.Common.Session)Session[NTRUST_SESSION];

                NTrust.K2.BO.User ObjUser = (NTrust.K2.BO.User)Sess_user.LoginUser;
                if (ObjUser == null)
                {
                    strRedirectPath = Request.ApplicationPath.ToString() + "/Login.aspx";
                    string strQueryString = "?SE=Y";
                    strQueryString += "&ModalWindow=Y";
                    Response.Redirect(strRedirectPath + strQueryString);
                }//End

            }
            catch (NTrust.Util.NTrustException nex)
            {
                ErrExist = "Y";
                lblErrorMessage.Visible = true;
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, ExportFile);

            }
            catch (Exception ex)
            {
                ErrExist = "Y";
                ShowCustomError(0, ex.Message.ToString());

            }



        }


        #region InitializePageProperties
        private void InitializePageProperties()
        {

            //	imgTemplate.ImageUrl		=	GetImagePath("Templatefilter");		

            InitializeResources("K2Web.Utility.Integration");
            lblExportFileSetting.Text = GetString("ExportSettings");
            //lblExportTemplate.Text		=	GetString("ExportTemplate");

            //lblExportFile.Text			=	GetString("ExportFile");
            //lblExportFile.Text="CHECKREG.TXT";

            //lblDelimiterChar.Text		=	GetString("Delimiter");
            //lgdExportOptions.InnerText	=	GetString("ExportOptions");
            //chkOverWrite.Text			=	GetString("OverwriteFile");
            //chkAppend.Text				=	GetString("Append");

            //spSaveMsg.InnerText			=	GetString("SaveMsg");
            // if (mon.Length == 1) mon = "0" + mon;
            //if (day.Length == 1) day = "0" + day;
            //file_name = file_name + DateTime.Today.Year.ToString() + mon + day + ".csv";
            //spSaveMsg.InnerText = GetString("The") + file_name + GetString("Filesave") + System.Configuration.ConfigurationSettings.AppSettings[SAVE_FILE_PATH] + ADD_FILE_PATH;

            btnExport.Text = GetString("Export");
            //btnFile.Text				=	GetString("Browse");
            btnClose.Text = GetString("Close");
            // chkExportLocal.Text = GetString("APExporttoLocal");

        }

        #endregion

        /*
		----------------------------------------------------------------------------------------------------
		Synopsis		:	This function will Set the Default value to respectively controls
		Input			:   udgFieldList as object E
		Output			:	
		-----------------------------------------------------------------------------------------------------
		*/
        private void SetDefaultValue()
        {
            NTrust.K2.BO.ExportManager ObjExpMan = null;
            ObjExpMan = (NTrust.K2.BO.ExportManager)NTrustSession.CreateBusinessObject("ExportManager", 1);
            strFilter = "varname ='" + EXPORTDIRECTORY + "'";
            strSort = "";
            DataTable dtExportResult = ObjExpMan.GetExportVariable(strFilter, strSort);
            if (dtExportResult.Rows.Count > 0)
            {
                string mon = DateTime.Today.Month.ToString();
                string day = DateTime.Today.Day.ToString();
                //txtSelectedFile.Text = file_name;
            }
            strFilter = "varname ='" + DELIMITCHAR + "'";
            dtExportResult = ObjExpMan.GetExportVariable(strFilter, strSort);
            if (dtExportResult.Rows.Count > 0)
            {
                //setDefault(this.ddlDelimiter,dtExportResult.Rows[0][SELECT_FIELDNAME].ToString());
            }
        }

        #region Type of Template
        private void FillTypeofTemplate()
        {
            try
            {
                DataTable dtTypeofTemplate = null;
                NTrust.K2.BO.Common Objcom = (NTrust.K2.BO.Common)NTrustSession.CreateBusinessObject("Common", 1);
                string strFilter = "TEMPLATENAME <>''";
                string strSort = "TEMPLATENAME";
                dtTypeofTemplate = Objcom.GetTypeofTemplate(strFilter, strSort);
                FillCombo(ddlTypeLedger, dtTypeofTemplate, "TEMPLATENAME", "TEMPLATENAME");
                //ddlTypeLedger.SelectedIndex = 1;
                //ddlTypeLedger.Items.IndexOf(ddlTypeLedger.Items.FindByText("1"));
                ddlTypeLedger.SelectedIndex = 2;
                //ddlTypeLedger.SelectedItem.Text = "GLOBAL";
                //NTrust.K2.BO.Lease objLease;
                //objLease = (NTrust.K2.BO.Lease)NTrustSession.CreateBusinessObject("LEASE", 1);
                //objLease.Open(NTrustSession.SelectedLease);
                //strFilter = "TEMPLATENAME<>''";
                //string strSort = "PKEY";
                ////DataTable dtExportResult = ObjExpMan.GetExportList(strFilter);
                //DataTable dtExportResult = objLease.GetJournalLeaseData(strFilter, strSort, "TableV_K2_ASC842_Template_HdrDef");
                FillFieldsList();
            }
            catch (NTrust.Util.NTrustException nex)
            {
                NTrustSessionPage.InitializeResources("K2Web.Lease.Lease");
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("LSOptions"));
            }
            catch (Exception ex)
            {
                ShowCustomError(0, ex.Message.ToString());
            }

        }
        #endregion
        /*
		----------------------------------------------------------------------------------------------------
		Synopsis		:	This function will populate the Export  Fields List
		Input			:   udgFieldList as object E
		Output			:	
		-----------------------------------------------------------------------------------------------------
		*/
        private void FillFieldsList()
        {
            DataTable dtTarget = new DataTable();
            udgFieldList.DataSource = null;
            udgFieldList.DataBind();
            udgFieldList.DataBind();

            string str = ddlTypeLedger.SelectedItem.Value;
            string str1 = ddlTypeLedger.SelectedItem.Text;
            if (ddlTypeLedger.SelectedItem.Text != "")
            {
                NTrust.K2.BO.Lease objLease;
                objLease = (NTrust.K2.BO.Lease)NTrustSession.CreateBusinessObject("LEASE", 1);
                objLease.Open(NTrustSession.SelectedLease);
                strFilter = "TEMPLATENAME = '" + ddlTypeLedger.SelectedItem.Text + "'";
                string strSort = "PKEY";
                dtTarget = objLease.GetJournalLeaseData(strFilter, strSort, "TableV_K2_ASC842_Template_DtlDef");


                udgFieldList.DataSource = dtTarget;
                udgFieldList.DataBind();
            }

        }

        #region ddlFilterType_SelectedIndexChanged
        /* ----------------------------------------------------------------------------------------------------
        Synopsis		:	Fills the Filter Name Control with the values based on the selected filter type
        Input			:   
        Output			:	
        -----------------------------------------------------------------------------------------------------
        */
        protected void ddlTypeLedger_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                // hdnFilterChanged.Value = "Y";
                //FillFilterNameDTL();
                FillFieldsList();
            }
            catch (NTrust.Util.NTrustException nex)
            {
                ErrExist = "Y";
                NTrustSessionPage.InitializeResources("K2Web.Accounts");
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("BPayable"));
            }
            catch (Exception ex)
            {
                ErrExist = "Y";
                ShowCustomError(0, ex.Message.ToString());
            }
        }
        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //this.imgTemplate.Click += new System.Web.UI.ImageClickEventHandler(this.imgTemplate_Click);
            //this.udgFieldList.InitializeLayout += new Infragistics.WebUI.UltraWebGrid.InitializeLayoutEventHandler(this.udgFieldList_InitializeLayout);

        }
        #endregion

        #region FillDelimiter

        // This Method will Fill Pay Month Details 
        private void FillDelimiter()
        {

            DataTable dtList = null;
            NTrust.K2.BO.Common Objcom = (NTrust.K2.BO.Common)NTrustSession.CreateBusinessObject("Common", 1);
            string strFilter = GLOBAL_TYPE + "='DELIMITCHAR'";
            string strSort = "SORTORDER";
            dtList = Objcom.GetGlobalItemList(strFilter, strSort);
            //FillCombo(this.ddlDelimiter ,dtList,GLOBAL_CODE,GLOBAL_DESC);
        }
        #endregion



        protected void ddlNamedFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {


                FillFieldsList();
            }
            catch (NTrust.Util.NTrustException nex)
            {
                ErrExist = "Y";
                lblErrorMessage.Visible = true;
                K2Web.NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("ExportFile"));

            }
            catch (Exception ex)
            {
                ErrExist = "Y";
                ShowCustomError(0, ex.Message.ToString());

            }
        }

        #region GetFilterDetails
        /*
		----------------------------------------------------------------------------------------------------
		Synopsis		:	Used to get the Filter Details
		Input			:   
		Output			:	
		-----------------------------------------------------------------------------------------------------
		*/
        private void GetFilterDetails()
        {
            NTrust.K2.BO.ExportManager ObjExpMan = null;
            ObjExpMan = (NTrust.K2.BO.ExportManager)NTrustSession.CreateBusinessObject("ExportManager", 1);
            string strFilter = "varname ='" + FIELDSETTINGS + "'";
            string strSort = "";
            DataTable dtExportResult = ObjExpMan.GetExportNamedFilter(strFilter, strSort);

            DataRow drNewRow = null;
            drNewRow = dtExportResult.NewRow();
            drNewRow[SELECT_VARNAME] = "";
            drNewRow[SELECT_FKEY] = strDEFAULT_SELECTION_VALUE;
            drNewRow["FILTERNAME"] = strDEFAULT_SELECTION_TEXT;
            dtExportResult.Rows.Add(drNewRow);
            dtExportResult.DefaultView.Sort = SELECT_FKEY;
            //FillCombo(this.ddlNamedFilter,dtExportResult,SELECT_FKEY,"FILTERNAME");



        }
        #endregion

        private void imgTemplate_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string strQueryURL = "";
            try
            {
                strQueryURL = "JournalExportFields.aspx?ParentName=" + strSettings + "&MenuKey=" + strMenuKey + "&Action=A";
            }
            catch (NTrust.Util.NTrustException nex)
            {
                ErrExist = "Y";
                lblErrorMessage.Visible = true;
                K2Web.NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("ExportFile"));

            }
            catch (Exception ex)
            {
                ErrExist = "Y";
                ShowCustomError(0, ex.Message.ToString());

            }
            finally
            {
                Response.Redirect(strQueryURL);
            }

        }

        /*
		----------------------------------------------------------------------------------------------------
		Synopsis		:	This Method will Close the Current window
		Input			:   
		Output			:	
		-----------------------------------------------------------------------------------------------------
		*/
        private void imgClose_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                string strScript = "<script language=JavaScript>window.close();</script>";
                this.RegisterStartupScript("Wiclose", strScript);
            }
            catch (NTrust.Util.NTrustException nex)
            {
                ErrExist = "Y";
                lblErrorMessage.Visible = true;
                K2Web.NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("ExportFile"));

            }
            catch (Exception ex)
            {
                ErrExist = "Y";
                ShowCustomError(0, ex.Message.ToString());

            }
        }


        #region IsValidNumber

        protected bool IsValidNumber(string NumberAsString)
        {
            try
            {
                double dblNumber;
                if (double.TryParse(NumberAsString, out dblNumber))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region SafePipeSymbol
        public string SafePipeSymbol(string strValue)
        {
            if (strValue.Contains("|"))
            {

                return "\"" + strValue + "\"";
            }
            return strValue;
        }
        #endregion
        private string fn_AddSpace(string StrName, int iCount)
        {
            if (StrName.Trim().Length < iCount)
            {
                iCount = iCount - StrName.Trim().Length;
                StrName = StrName.PadRight(StrName.Trim().Length + iCount, ' ');
            }
            else if (StrName.Trim().Length > iCount)
            {
                iCount = StrName.Trim().Length - iCount;
                StrName = StrName.Remove(StrName.Trim().Length - iCount);
            }
            else
                return StrName;
            return StrName;
        }

        protected void WriteCsv(string strjsonData, string _LogLocation, string strPath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_LogLocation, true))
                {
                    sw.Write(strjsonData);
                    sw.WriteLine();
                }

                strPath = strPath.Replace("\\", "~");
                if (strPath.Trim().EndsWith("~"))
                {
                    strPath = strPath.Remove(strPath.Length - 1);
                }
                this.RegisterStartupScript("DisableProgress", "<script type='text/javascript'>window.open('ThrowZip.aspx?Path=" + strPath + "');</script>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool IsValidDate(string DateAsString)
        {
            try
            {
                DateTime tmpDate;
                return (DateTime.TryParse(DateAsString, out tmpDate));
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /*
		----------------------------------------------------------------------------------------------------
		Synopsis		:	This Method will EXPORT the Data  to a TEXT FILE 
		Input			:   
		Output			:	
		-----------------------------------------------------------------------------------------------------
		*/
        protected void btnExport_Click(object sender, System.EventArgs e)
        {
            //bool blnWriteFlag = false;
            //string[] strSQLScript = new string[4];
            NTrust.K2.BO.Lease objLease;
            DataTable dtTarget = new DataTable();
            objLease = (NTrust.K2.BO.Lease)NTrustSession.CreateBusinessObject("LEASE", 1);
            try
            {
                string strFilterReport = string.Empty, strSortReport = string.Empty, strLedgerName = string.Empty, strSelectedPeriod = string.Empty, strValue = string.Empty, strmonth = string.Empty, strSelectedPrdfile = string.Empty, strPeriodMaxval = string.Empty, strCurrMaxVal = string.Empty;
                bool iflag = false;
                StringBuilder JEData = new StringBuilder();
                DataTable dtGetJournalReportData = new DataTable();
                if (strPkey != "" && ddlTypeLedger.SelectedItem.Text != "")
                {
                    if (ddlTypeLedger.SelectedItem.Text != "")
                    {
                        strFilter = "TEMPLATENAME = '" + ddlTypeLedger.SelectedItem.Text + "'";
                        string strSort = "PKEY";
                        dtTarget = objLease.GetJournalLeaseData(strFilter, strSort, "TableV_K2_ASC842_Template_DtlDef");
                        if (dtTarget.Rows.Count > 0)
                        {
                            if (dtTarget.Select("ColumnName='PERIOD NAME'").Length > 0)
                            {
                                DataRow[] dr = dtTarget.Select("ColumnName='PERIOD NAME'");
                                strPeriodMaxval = Convert.ToString(dr[0]["Width"]);
                            }
                            else
                                strPeriodMaxval = "15";
                            if (dtTarget.Select("ColumnName='CURRENCY COVERSION RATE'").Length > 0)
                            {
                                DataRow[] dr = dtTarget.Select("ColumnName='CURRENCY COVERSION RATE'");
                                strCurrMaxVal = Convert.ToString(dr[0]["Width"]);
                            }
                            else
                                strCurrMaxVal = "14";
                        }
                    }
                    if (ddlTypeLedger.SelectedItem.Text.Trim().ToUpper().Contains("GLOBAL"))
                        strLedgerName = "GLOBAL";
                    else if (ddlTypeLedger.SelectedItem.Text.Trim().ToUpper().Contains("ROW"))
                        strLedgerName = "ROW";
                    else if (ddlTypeLedger.SelectedItem.Text.Trim().ToUpper().Contains("ERP"))
                        strLedgerName = "ERPLEGACY";
                    //strLedgerName = ddlTypeLedger.SelectedItem.Text;
                    strFilterReport = "HPKEY='" + strPkey.Trim() + "'";
                    strSortReport = "FKEY_LEASE,DPKEY";
                    dtGetJournalReportData = objLease.GetJournalLeaseData(strFilterReport, strSortReport, "TableV_K2_GETJEReportDataDef");
                    if (dtGetJournalReportData != null && dtGetJournalReportData.Rows.Count > 0)
                    {
                        if (Convert.ToString(dtGetJournalReportData.Rows[0]["PROCESS_MONTH"]) != "" && Convert.ToString(dtGetJournalReportData.Rows[0]["PROCESS_YEAR"]) != "")
                        {
                            if (Convert.ToString(dtGetJournalReportData.Rows[0]["PROCESS_MONTH"]).Trim().Length == 1)
                                strmonth = "0" + Convert.ToString(dtGetJournalReportData.Rows[0]["PROCESS_MONTH"]).Trim();
                            else
                                strmonth = Convert.ToString(dtGetJournalReportData.Rows[0]["PROCESS_MONTH"]).Trim();
                            DateTime dt = new DateTime();
                            dt = Convert.ToDateTime(strmonth + " / 01 / " + Convert.ToString(dtGetJournalReportData.Rows[0]["PROCESS_YEAR"]));
                            strSelectedPeriod = dt.ToString("MMM-yy");
                            strSelectedPrdfile = dt.ToString("MMMM-yyyy");
                        }

                        for (int rowReport1 = 0; rowReport1 < dtGetJournalReportData.Rows.Count; rowReport1++)
                        {
                            string strCurrChangeDate = string.Empty, strAmount = string.Empty, strJECRDate = string.Empty;
                            if (IsValidDate(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["CURRENCY_CONVERSION_DATE"])))
                            {
                                DateTime dtChangeDate = new DateTime();
                                dtChangeDate = Convert.ToDateTime(dtGetJournalReportData.Rows[rowReport1]["CURRENCY_CONVERSION_DATE"]);
                                strCurrChangeDate = dtChangeDate.ToString("MM/dd/yy");
                            }
                            if (IsValidDate(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["DATECREATED"])))
                            {
                                DateTime dtChangeDate = new DateTime();
                                dtChangeDate = Convert.ToDateTime(dtGetJournalReportData.Rows[rowReport1]["DATECREATED"]);
                                strJECRDate = dtChangeDate.ToString("MM/dd/yy");
                            }
                            if (IsValidNumber(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["DEBIT"])))
                                strAmount = Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["DEBIT"]);
                            else if (IsValidNumber(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["CREDIT"])))
                                strAmount = "-" + Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["CREDIT"]);
                            if (iflag)
                                JEData.Append("\n");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["CADENCY_UNIQUE_IDENTIFIER"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["BATCH_NAME"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["BATCH_DESCRIPTION"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["JOURNAL_NAME"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["JOURNAL_DESCRIPTION"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["GROUP_ID"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["REVERSAL_FLAG"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["REVERSAL_PERIOD"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["LEDGER_NAME"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["SOURCE"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["CATEGORY"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["CURRENCY_CODE"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(strJECRDate)) + "|");
                            //JEData.Append(SafePipeSymbol(fn_AddSpace(Convert.ToString(strSelectedPeriod), Convert.ToInt32(strPeriodMaxval))) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(strSelectedPeriod)) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(strAmount)) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["LINE_DESCRIPTION"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["ENTITY"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["Location"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["Product"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["Function_Value"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["GLACCOUNT"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["SUBACCOUNT"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["INTERCOMPANY"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["FUTURE1"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["FUTURE2"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(strJECRDate)) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["CURRENCY_CONVERSION_TYPE"])) + "|");
                            JEData.Append(SafePipeSymbol(Convert.ToString(strCurrChangeDate)) + "|");

                            if (strLedgerName.Trim().ToUpper() == "ROW")
                            {
                                //JEData.Append(SafePipeSymbol(fn_AddSpace(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["CURRENCY_RATE"]), Convert.ToInt32(strCurrMaxVal))) + "");                                
                                JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["CURRENCY_RATE"])) + "");
                            }
                            else if (strLedgerName.Trim().ToUpper() == "GLOBAL" || strLedgerName.Trim().ToUpper() == "ERPLEGACY")
                            {
                                //JEData.Append(SafePipeSymbol(fn_AddSpace(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["CURRENCY_RATE"]), Convert.ToInt32(strCurrMaxVal))) + "|");
                                JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["CURRENCY_RATE"])) + "|");
                                JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["LINE_DFF"])) + "|");
                                JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["CUSTOMER_CONTRACT"])) + "|");
                                JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["THIRD_PARTY"])) + "|");
                                JEData.Append(SafePipeSymbol(Convert.ToString(dtGetJournalReportData.Rows[rowReport1]["POST_ALL_FLAG"])) + "");
                            }
                            iflag = true;
                        }
                        strValue = Convert.ToString(JEData);
                        string strCurrentDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                        if (strValue != "")
                        {
                            string strPath = System.Configuration.ConfigurationSettings.AppSettings[SAVE_FILE_PATH] + "\\" + strSelectedPrdfile + "JEORACLE" + strLedgerName + strCurrentDate + "\\";
                            if (!Directory.Exists(strPath))
                                Directory.CreateDirectory(strPath);
                            string filename = strPath + strSelectedPrdfile + "JEORACLE" + strLedgerName + strCurrentDate + ".csv";
                            WriteCsv(strValue, filename, strPath);
                        }
                        else
                            CreateMessageAlert(this, GetString("NODATAFOUND"));
                    }
                    else
                        CreateMessageAlert(this, GetString("NODATAFOUND"));

                }                
            }


            catch (NTrust.Util.NTrustException nex)
            {
                if (nex.ErrorCode == 500000)
                {
                    ErrExist = "Y";
                    lblErrorMessage.Visible = true;
                    NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                    CreateMessageAlert(this, GetString("FileAccess"));
                    //lblErrorMessage.Text    = "File is accessed by Another Process. Close the file and export again";
                }
                else
                {
                    ErrExist = "Y";
                    lblErrorMessage.Visible = true;
                    NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                    SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("Export File"));
                }

            }
            catch (System.IO.FileNotFoundException Ferr)
            {
                ErrExist = "Y";
                lblErrorMessage.Visible = true;
                SetErrorMessage(lblErrorMessage, 0, Ferr.Message.ToString(), "");
            }
            catch (Exception ex)
            {
                ErrExist = "Y";
                ShowCustomError(0, ex.Message.ToString());

            }
        }



        protected void btnFile_Click(object sender, System.EventArgs e)
        {
            // Excute the SCRIPT for Directory  OPEN IN SHOW Model Dialog 
            string strScript = "<script language=JavaScript>window.showModalDialog('UtilityDirectory.aspx?formname=frmSummaryByType&Fieldname=txtSelectedFile', window, 'status:no;dialogHeight=420px;dialogWidth=620px;scroll:no;');</script>";
            this.RegisterStartupScript("Wiclose", strScript);
        }

        protected void btnClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                string strScript = "<script language=JavaScript>window.close();</script>";
                this.RegisterStartupScript("Wiclose", strScript);
            }
            catch (NTrust.Util.NTrustException nex)
            {
                ErrExist = "Y";
                lblErrorMessage.Visible = true;
                NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("ExportFile"));

            }
            catch (Exception ex)
            {
                ErrExist = "Y";
                ShowCustomError(0, ex.Message.ToString());

            }
        }
        protected void udgFieldList_DataBound(Object sender, EventArgs e)
        {
            NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
            udgFieldList.Columns[0].HeaderText = NTrustSessionPage.GetString("FieldName");
            udgFieldList.Columns[1].HeaderText = NTrustSessionPage.GetString("FieldLength");


        }

        protected void udgFieldList_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Attributes.Add("style", "white-space: nowrap;");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onClick", "javascript:void SelectRow(this,0);");

            }
        }
    }

}
