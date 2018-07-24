/*
File Name		:	ASC842journalexportList.aspx.cs
Author			:   Karthikeyan C
Created Date	:	07-13-2018
Purpose			:   Getting Export List Details
Business Object	:   Journal, JounralManager  
 
    

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
using System.Text.RegularExpressions;


namespace K2Web.Utility
{


    /// <summary>
    ///		Summary description for BatchEceptionOutPut.
    /// </summary>
    public partial class ASC842journalexport : NTrustSessionPage
    {

        // Local Variables are declare here.
        protected string ErrExist;
        private string strSettings;
        private string strDisplayName;
        private string strMenuKey;
        private string strBusinessUnit;
        private string strErrExist = "";
        private string strFilter = "";

        //Constant variable declaration
        private const string ENTRY_TYPE = "C";
        private string strName = "";
        protected string ExportFrom;
        protected string ExportTo;
        protected string RecvFrom;
        protected string RecvTo;
        protected string FillExportDateTo;
        protected string FillExportDateFrom;
        protected string FillRecDateTo;
        protected string FillRecDateFrom;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            strSettings = Request.QueryString["ParentName"];
            strDisplayName = Request.QueryString["DisplayName"];
            strMenuKey = Request.QueryString["MenuKey"];

            strBusinessUnit = NTrustSession.SelectedBusinessUnit;

            K2Web.NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
            // ExportFrom = GetString("ExportFrom");
            // ExportTo = GetString("ExportTo");
            // RecvFrom = GetString("RecvFrom");
            // RecvTo = GetString("RecvTo");
            // FillExportDateTo = GetString("FillExportDateTo");
            // FillExportDateFrom = GetString("FillExportDateFrom");
            // FillRecDateTo = GetString("FillRecDateTo");
            // FillRecDateFrom = GetString("FillRecDateFrom");

            try
            {
                if (this.hidchkFlag.Value == "Y")
                {
                    this.hidchkFlag.Value = "N";
                    FillExportList();
                    Session["SortCol1"] = "FILENAME";
                    Session["SortBy1"] = "asc";


                }
                NTrustSession.SelectedGridView = udgExportList;
                if (!this.IsPostBack)
                {
                    InitializePageProperties();
                    strErrExist = "N";
                    FillExportList();
                    Session["SortCol1"] = "FILENAME";
                    Session["SortBy1"] = "asc";
                }
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

        #region InitializePageProperties
        private void InitializePageProperties()
        {

            imgFade.ImageUrl = GetImagePath("fadeList");
            imgSeparator.ImageUrl = GetImagePath("arrow");
            imgbtnExport.ImageUrl = GetImagePath("export");
            imgTasks.Src = GetImagePath("Tasks");
            NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
            lblIntegration.Text = GetString("Integration");
            lblJournalExport.Text = GetString("ASC842JOURNALEXPORT");
        }

        #endregion

        /*
		----------------------------------------------------------------------------------------------------
		Synopsis		:	This method used for FillExportList
		Input			:   
		Output			:	
		-----------------------------------------------------------------------------------------------------
		*/

        private void FillExportList()
        {
            try
            {
                NTrust.K2.BO.Lease objLease;
                objLease = (NTrust.K2.BO.Lease)NTrustSession.CreateBusinessObject("LEASE", 1);
                objLease.Open(NTrustSession.SelectedLease);
                strFilter = "PROCESSS_STATUS='C'";
                string strSort = "SelectedPeriod desc,TRANS_DATE desc";
                DataTable dtExportResult = objLease.GetJournalLeaseData(strFilter, strSort, "TableV_K2_GETJournalExportDef");
                NTrustSession.SortCol = "TRANS_DATE";
                NTrustSession.SortBy = "asc";
                this.udgExportList.DataSource = dtExportResult;
                this.udgExportList.DataBind();
                this.udgExportList.DataBind();
                ShowSortImage(udgExportList, NTrustSessionPage.GetString("ExportType"));
                Session["GridTablepopup"] = dtExportResult;
                Session["GridViewOriginalpopup"] = dtExportResult;
                if (udgExportList.Rows.Count > 0)
                {
                    udgExportList.SelectedIndex = 0;
                    if (hdnPkey.Value == "")
                    {
                        hdnPkey.Value = Convert.ToString(dtExportResult.Rows[0]["Pkey"]);
                    }
                }

                NTrustSession.SelectedGrid = udgExportList;

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
            this.imgbtnExport.Click += new System.Web.UI.ImageClickEventHandler(this.imgbtnExport_Click);
        }
        #endregion

        private void MarkOrUnMark(string strMark)
        {
            string strExportKey = "";
            if (udgExportList.Rows.Count > 0)
            {
                Label lblPkey = new Label();
                lblPkey = (Label)udgExportList.Rows[Convert.ToInt16(hdnSelRow.Value)].Cells[0].Controls[5];
                strExportKey = lblPkey.Text;
                int intSelectedRow = Convert.ToInt16(hdnSelRow.Value);
                NTrust.K2.BO.Export objExport = (NTrust.K2.BO.Export)NTrustSession.CreateBusinessObject("AEXPORT", 1);
                // Here Open Main A_EXport Key
                objExport.Open(strExportKey);
                objExport.EditJournal(strExportKey, strMark);
            }
            else
            {
                K2Web.NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                CreateMessageAlert(this, GetString("AtleaseOneRowSelect"));
            }
        }

        private void imgbtnExport_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string strScript = "";
            try
            {
                if (udgExportList.Rows.Count > 0)
                {
                    udgExportList.Rows[0].RowState = DataControlRowState.Selected;
                    hdnSelRow.Value = Convert.ToString(udgExportList.Rows[0].RowIndex);
                    Label lblPkey = new Label();
                    lblPkey = (Label)udgExportList.Rows[Convert.ToInt16(hdnSelRow.Value)].Cells[0].Controls[5];
                    string strExportKey = lblPkey.Text;
                    // Excute the SCRIPT for JOURNALEXPORT  OPEN IN SHOW Model Dialog 
                    strScript = "<script language=JavaScript>window.showModalDialog('ASC842journalexport.aspx?ParentName=" + strSettings + "&MenuKey=" + strMenuKey + "&Hkey=" + hdnPkey.Value + "&Action=A', window, 'status:no;dialogHeight=550px;dialogWidth=900px;scroll:no;');</script>";
                    this.RegisterStartupScript("Wiclose", strScript);
                }
                else
                {
                    NTrustSessionPage.InitializeResources("K2Web.CommonResource");
                    this.CreateMessageAlert(this, GetString("NoRectoExportCSV"));
                }


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

        private void imgbtnMarkReceived_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                // Update as Mark Received
                MarkOrUnMark("Y");
                FillExportList();
            }
            catch (NTrust.Util.NTrustException nex)
            {
                strErrExist = "Y";
                lblErrorMessage.Visible = true;
                K2Web.NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("ExportFile"));
            }
            catch (Exception ex)
            {
                ShowCustomError(0, ex.Message.ToString());
            }
        }

        /*
		----------------------------------------------------------------------------------------------------
		Synopsis		:	This method used for Mark Un Reveived
		Input			:   
		Output			:	
		-----------------------------------------------------------------------------------------------------
		*/
        private void imgbtnMarkUnReveived_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                // Update as UNMark Received
                MarkOrUnMark("N");
                FillExportList();
            }
            catch (NTrust.Util.NTrustException nex)
            {
                this.strErrExist = "Y";
                lblErrorMessage.Visible = true;
                K2Web.NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("ExportFile"));
            }
            catch (Exception ex)
            {
                ShowCustomError(0, ex.Message.ToString());
            }
        }

        /*
		----------------------------------------------------------------------------------------------------
		Synopsis		:	This method used for undo export
		Input			:   System object
		Output			:	NULL
		-----------------------------------------------------------------------------------------------------
		*/
        private void imgbtnUndoExport_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string strExportKey = "";
            try
            {
                if (udgExportList.Rows.Count > 0)
                {
                    int intSelectedRow = Convert.ToInt16(hdnSelRow.Value);
                    Label lblPkey = new Label();
                    lblPkey = (Label)udgExportList.Rows[Convert.ToInt16(hdnSelRow.Value)].Cells[0].Controls[5];
                    strExportKey = lblPkey.Text;
                    NTrust.K2.BO.Export objExport = (NTrust.K2.BO.Export)NTrustSession.CreateBusinessObject("AEXPORT", 1);
                    objExport.DeleteExport(strExportKey);
                    FillExportList();
                }
                else
                {
                    K2Web.NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                    CreateMessageAlert(this, GetString("AtleaseOneRowSelect"));
                }
            }
            catch (NTrust.Util.NTrustException nex)
            {
                this.strErrExist = "Y";
                lblErrorMessage.Visible = true;
                K2Web.NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("ExportFile"));
            }
            catch (Exception ex)
            {
                ShowCustomError(0, ex.Message.ToString());
            }
        }
               
        
        #region Grid Sorting

        protected void udgExportList_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (udgExportList.Rows.Count > 0)
                {
                    if (Convert.ToInt16(hdnSelRow.Value) >= udgExportList.Rows.Count)
                    {
                        udgExportList.Rows[0].RowState = DataControlRowState.Selected;
                        hdnSelRow.Value = Convert.ToString(udgExportList.Rows[0].RowIndex);
                    }
                    Label lblPkey = (Label)udgExportList.Rows[Convert.ToInt16(hdnSelRow.Value)].Cells[0].Controls[5];
                    if (NTrustSession.SortCol == e.SortExpression)
                    {
                        if (dir == SortDirection.Ascending)
                        {
                            dir = SortDirection.Descending;
                            NTrustSession.SortBy = "Desc";
                        }
                        else
                        {
                            dir = SortDirection.Ascending;
                            NTrustSession.SortBy = "Asc";
                        }
                    }
                    else
                    {
                        dir = SortDirection.Ascending;
                        NTrustSession.SortBy = "Asc";
                    }
                    NTrustSession.SortCol = e.SortExpression;
                    DataView sortedView = new DataView();
                    sortedView = ((DataTable)Session["GridTablepopup"]).DefaultView;
                    sortedView.Sort = e.SortExpression + " " + NTrustSession.SortBy;
                    udgExportList.DataSource = sortedView;
                    udgExportList.DataBind();
                    //udgExportList.DataBind();


                    foreach (GridViewRow row in udgExportList.Rows)
                    {
                        Label lbl1 = (Label)row.Cells[0].Controls[5];
                        if (lbl1.Text == lblPkey.Text)
                        {
                            row.RowState = DataControlRowState.Selected;
                            hdnSelRow.Value = Convert.ToString(row.RowIndex);
                        }
                    }
                }
                GridViewRow gridViewHeaderRow = udgExportList.HeaderRow;
                foreach (TableCell headerCell in gridViewHeaderRow.Cells)
                {
                    if (headerCell.HasControls())
                    {
                        LinkButton headerButton = headerCell.Controls[0] as LinkButton;

                        if (headerButton != null)
                        {
                            HtmlGenericControl div = new HtmlGenericControl("div");
                            Label headerText = new Label();
                            headerText.Text = headerButton.Text + "&nbsp;";
                            div.Controls.Add(headerText);
                            if (e.SortExpression == headerButton.CommandArgument)
                            {
                                System.Web.UI.WebControls.Image headerImage = new System.Web.UI.WebControls.Image();

                                if (NTrustSession.SortBy.ToLower() == "asc")
                                {
                                    headerImage.ImageUrl = "~/Images/Up.png";
                                    headerImage.AlternateText = "Sort Descending";
                                }
                                else
                                {
                                    headerImage.AlternateText = "Sort Ascending";
                                    headerImage.ImageUrl = "~/Images/Down.png";
                                }
                                div.Controls.Add(headerImage);
                            }
                            headerButton.Controls.Add(div);
                            headerButton.Font.Underline = false;
                        }
                    }
                }
                // SortImg1();
            }
            catch (Exception ee)
            {
                CreateMessageAlert(this.Page, ee.Message);
            }
        }
        public SortDirection dir
        {
            get
            {
                if (ViewState["SortDirection"] == null)
                { ViewState["SortDirection"] = SortDirection.Ascending; }
                return (SortDirection)ViewState["SortDirection"];
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        #endregion

        protected void udgExportList_DataBound(Object sender, EventArgs e)
        {
            NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
            udgExportList.Columns[0].HeaderText = NTrustSessionPage.GetString("LEASECLASSIFICATION");
            udgExportList.Columns[1].HeaderText = "";
            udgExportList.Columns[2].HeaderText = NTrustSessionPage.GetString("SELECTEDPERIOD");
            udgExportList.Columns[3].HeaderText = NTrustSessionPage.GetString("TYPEOFFILTER");
            udgExportList.Columns[4].HeaderText = NTrustSessionPage.GetString("LEASEFILTERNAME");
            udgExportList.Columns[5].HeaderText = NTrustSessionPage.GetString("TRANSACTIONDATE");
            udgExportList.Columns[6].HeaderText = NTrustSessionPage.GetString("CREATEDBY");
            udgExportList.Columns[7].HeaderText = "";

        }

        protected void udgExportList_RowDataBound(Object sender, GridViewRowEventArgs e)
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


        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Accessing BoundField Column
            string name = udgExportList.SelectedRow.Cells[0].Text;
            
        }


        protected void udgExportList_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            udgExportList.PageIndex = e.NewPageIndex;
            udgExportList.DataSource = (DataTable)Session["GridTablepopup"];
            udgExportList.DataBind(); udgExportList.DataBind();
            if (udgExportList.Rows.Count > 0)
            {
                udgExportList.Rows[0].RowState = DataControlRowState.Selected;
                hdnSelRow.Value = Convert.ToString(udgExportList.Rows[0].RowIndex);
            }
        }
        protected void udgExportList_SelectedIndexChang(object sender, EventArgs e)
        {
            //hdnSelRow.Value = udgExportList.SelectedRow.FindControl("lblCountry") as Label).Text;
            string name = udgExportList.SelectedRow.Cells[0].Text;
            string name1 = udgExportList.SelectedRow.Cells[0].Text;
        }
        //protected void udgFileList_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        //{
        //    udgFileList.PageIndex = e.NewPageIndex;
        //    udgFileList.DataSource = (DataTable)Session["GridTable1"];
        //    udgFileList.DataBind();
        //    udgFileList.DataBind();
        //    K2Web.NTrustSessionPage.InitializeResources("K2Web.Document");
        //    ShowSortImage(udgFileList, K2Web.NTrustSessionPage.GetString("Name"));
        //    if (udgFileList.Rows.Count > 0)
        //    {
        //        udgFileList.Rows[0].RowState = DataControlRowState.Selected;
        //        hdnSelRow1.Value = Convert.ToString(udgFileList.Rows[0].RowIndex);
        //    }
        //    SortImg();
        //}

        #region Grid Sorting

        //protected void udgFileList_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    try
        //    {
        //        if (udgFileList.Rows.Count > 0)
        //        {
        //            if (hdnSelRow1.Value != "" && hdnSelRow1.Value != null)
        //            {
        //                if (Convert.ToInt16(hdnSelRow1.Value) > 0)
        //                {
        //                    Label lblPkey = (Label)udgFileList.Rows[Convert.ToInt16(hdnSelRow1.Value)].Cells[0].Controls[3];
        //                    strName = lblPkey.Text;
        //                }
        //                else
        //                {
        //                    Label lblPkey = (Label)udgFileList.Rows[0].Cells[0].Controls[3];
        //                    strName = lblPkey.Text;
        //                }
        //            }
        //            else
        //            {
        //                Label lblPkey = (Label)udgFileList.Rows[0].Cells[0].Controls[3];
        //                strName = lblPkey.Text;
        //            }
        //            //string strSortCol = (string)Session["SortCol1"];
        //            if (Session["SortCol1"].ToString() == e.SortExpression)
        //            {
        //                if (dirr == SortDirection.Ascending)
        //                {
        //                    dirr = SortDirection.Descending;
        //                    Session["SortBy1"] = "Desc";
        //                }
        //                else
        //                {
        //                    dirr = SortDirection.Ascending;
        //                    Session["SortBy1"] = "Asc";
        //                }
        //            }
        //            else
        //            {
        //                dirr = SortDirection.Ascending;
        //                Session["SortBy1"] = "Asc";
        //            }
        //            Session["SortCol1"] = e.SortExpression;
        //            DataView sortedView = new DataView();
        //            sortedView = ((DataTable)Session["GridTable1"]).DefaultView;
        //            sortedView.Sort = e.SortExpression + " " + Session["SortBy1"].ToString();
        //            udgFileList.DataSource = sortedView;
        //            udgFileList.DataBind();
        //            udgFileList.DataBind();
        //            foreach (GridViewRow row in udgFileList.Rows)
        //            {
        //                Label lbl1 = (Label)row.Cells[0].Controls[3];
        //                if (lbl1.Text == strName)
        //                {
        //                    row.RowState = DataControlRowState.Selected;
        //                    hdnSelRow1.Value = Convert.ToString(row.RowIndex);
        //                }
        //            }
        //        }

        //        GridViewRow gridViewHeaderRow = udgFileList.HeaderRow;
        //        foreach (TableCell headerCell in gridViewHeaderRow.Cells)
        //        {
        //            if (headerCell.HasControls())
        //            {
        //                LinkButton headerButton = headerCell.Controls[0] as LinkButton;

        //                if (headerButton != null)
        //                {
        //                    HtmlGenericControl div = new HtmlGenericControl("div");
        //                    Label headerText = new Label();
        //                    headerText.Text = headerButton.Text + "&nbsp;";
        //                    div.Controls.Add(headerText);
        //                    if (e.SortExpression == headerButton.CommandArgument)
        //                    {
        //                        System.Web.UI.WebControls.Image headerImage = new System.Web.UI.WebControls.Image();

        //                        if (Session["SortBy1"].ToString().ToLower() == "asc")
        //                        {
        //                            headerImage.ImageUrl = "~/Images/Up.png";
        //                            headerImage.AlternateText = "Sort Descending";
        //                        }
        //                        else
        //                        {
        //                            headerImage.AlternateText = "Sort Ascending";
        //                            headerImage.ImageUrl = "~/Images/Down.png";
        //                        }
        //                        div.Controls.Add(headerImage);
        //                    }
        //                    headerButton.Controls.Add(div);
        //                    headerButton.Font.Underline = false;
        //                }
        //            }
        //        }
        //        SortImg();
        //      }
        //    catch (Exception ee)
        //    {
        //        CreateMessageAlert(this.Page, ee.Message);
        //    }
        //}
        public SortDirection dirr
        {
            get
            {
                if (ViewState["SortDirection1"] == null)
                { ViewState["SortDirection1"] = SortDirection.Ascending; }
                return (SortDirection)ViewState["SortDirection1"];
            }
            set
            {
                ViewState["SortDirection1"] = value;
            }
        }

        #endregion

        protected void udgFileList_DataBound(Object sender, EventArgs e)
        {
            NTrustSessionPage.InitializeResources("K2Web.Document");
            //udgFileList.Columns[0].HeaderText = NTrustSessionPage.GetString("Name");
            //udgFileList.Columns[1].HeaderText = NTrustSessionPage.GetString("Size");
            //udgFileList.Columns[2].HeaderText = NTrustSessionPage.GetString("Type");
            //udgFileList.Columns[3].HeaderText = NTrustSessionPage.GetString("Modified");

        }
        protected void lnkExport_Click(object sender, System.EventArgs e)
        {
            string strDate = "", strMoney = "", strInt = "", strHeader;
            if (hdnExport.Value == "Category")
            {
                if (udgExportList.Rows.Count > 0)
                {
                    NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                    strDate += NTrustSessionPage.GetString("ExportDate") + ",";
                    strDate += NTrustSessionPage.GetString("RecvDate") + ",";
                    strHeader = NTrustSessionPage.GetString("Integration") + " >> " + NTrustSessionPage.GetString("ASC842JOURNALEXPORT");
                    udgExportList.AllowPaging = false;
                    udgExportList.DataSource = (DataTable)Session["GridTablepopup"];
                    udgExportList.DataBind();
                    ShowSortImage(udgExportList, NTrustSessionPage.GetString("ExportType"));
                    GridToExportExcelPopUp(udgExportList, NTrustSessionPage.GetString("JournalExport"), 0, strMoney, strInt, strHeader);
                    udgExportList.Rows[0].RowState = DataControlRowState.Selected;
                    hdnSelRow.Value = Convert.ToString(udgExportList.Rows[0].RowIndex);
                    udgExportList.AllowPaging = true;
                    udgExportList.DataSource = (DataTable)Session["GridTablepopup"];
                    udgExportList.DataBind();
                    ShowGridSortIcon(udgExportList);
                }
                else
                {
                    //NTrustSessionPage.InitializeResources("K2Web.CommonResource");
                    //NTrustSessionPage.InitializeResources("Utility.ASC842JournalExportList");
                    NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                    this.CreateMessageAlert(this, GetString("NoRectoExportCsv"));
                }
            }

            //else if (hdnExport.Value == "Type")
            //{
            //    if (udgFileList.Rows.Count > 0)
            //    {
            //         NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
            //        strDate += NTrustSessionPage.GetString("Modified") + ",";
            //        strHeader = NTrustSessionPage.GetString("Integration") + " >> " + NTrustSessionPage.GetString("JournalExport");
            //        udgFileList.AllowPaging = false;
            //        udgFileList.DataSource = (DataTable)Session["GridTable1"];
            //        udgFileList.DataBind();
            //        ShowSortImage(udgFileList, NTrustSessionPage.GetString("Name"));
            //        GridToExportExcelPopUp(udgFileList, "JournalExport", 0, strMoney, strInt, strHeader);
            //        udgFileList.AllowPaging = false;
            //        udgFileList.Rows[0].RowState = DataControlRowState.Selected;
            //        hdnSelRow1.Value = Convert.ToString(udgFileList.Rows[0].RowIndex);
            //    }
            //    else
            //    {
            //        NTrustSessionPage.InitializeResources("K2Web.CommonResource");
            //        this.CreateMessageAlert(this, GetString("NoRectoExportExcel"));
            //    }
            //}
            else
            {
                NTrustSessionPage.InitializeResources("K2Web.CommonResource");
                this.CreateMessageAlert(this, GetString("NoRectoExportExcel"));
            }

        }
        protected void udgFileList_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Attributes.Add("style", "white-space: nowrap;");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onClick", "javascript:void SelectRow1(this,0);");

            }
        }
        protected void SortImg()
        {
            if (udgExportList.Rows.Count > 0)
                udgExportList.Rows[Convert.ToInt16(hdnSelRow.Value)].RowState = DataControlRowState.Selected;
            GridViewRow gridViewHeaderRow = udgExportList.HeaderRow;
            foreach (TableCell headerCell in gridViewHeaderRow.Cells)
            {
                if (headerCell.HasControls())
                {
                    LinkButton headerButton = headerCell.Controls[0] as LinkButton;

                    if (headerButton != null)
                    {
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        Label headerText = new Label();
                        headerText.Text = headerButton.Text + "&nbsp;";
                        div.Controls.Add(headerText);
                        if (NTrustSession.SortCol == headerButton.CommandArgument)
                        {
                            System.Web.UI.WebControls.Image headerImage = new System.Web.UI.WebControls.Image();

                            if (NTrustSession.SortBy.ToLower() == "asc")
                            {
                                headerImage.ImageUrl = "~/Images/Up.png";
                                headerImage.AlternateText = "Sort Descending";
                            }
                            else
                            {
                                headerImage.AlternateText = "Sort Ascending";
                                headerImage.ImageUrl = "~/Images/Down.png";
                            }
                            div.Controls.Add(headerImage);
                        }
                        headerButton.Controls.Add(div);
                        headerButton.Font.Underline = false;
                    }
                }
            }
        }
        //protected void SortImg1()
        //{
        //    if (udgFileList.Rows.Count > 0)
        //    {
        //        if (hdnSelRow1.Value != "" && hdnSelRow1.Value != null)
        //        {
        //            if (Convert.ToInt16(hdnSelRow1.Value) >= 0)
        //            {
        //                udgFileList.Rows[Convert.ToInt16(hdnSelRow1.Value)].RowState = DataControlRowState.Selected;
        //                hdnSelRow1.Value = Convert.ToString(udgFileList.Rows[Convert.ToInt32(hdnSelRow1.Value)].RowIndex);
        //            }
        //        }
        //        else
        //        {
        //            udgFileList.Rows[0].RowState = DataControlRowState.Selected;
        //            hdnSelRow1.Value = Convert.ToString(udgFileList.Rows[0].RowIndex);
        //        }
        //    }
        //    GridViewRow gridViewHeaderRow = udgFileList.HeaderRow;
        //    foreach (TableCell headerCell in gridViewHeaderRow.Cells)
        //    {
        //        if (headerCell.HasControls())
        //        {
        //            LinkButton headerButton = headerCell.Controls[0] as LinkButton;

        //            if (headerButton != null)
        //            {
        //                HtmlGenericControl div = new HtmlGenericControl("div");
        //                Label headerText = new Label();
        //                headerText.Text = headerButton.Text + "&nbsp;";
        //                div.Controls.Add(headerText);
        //                if (Session["SortCol1"].ToString() == Convert.ToString(headerButton.CommandArgument))
        //                {
        //                    System.Web.UI.WebControls.Image headerImage = new System.Web.UI.WebControls.Image();

        //                    if (Session["SortBy1"].ToString().ToLower() == "asc")
        //                    {
        //                        headerImage.ImageUrl = "~/Images/Up.png";
        //                        headerImage.AlternateText = "Sort Descending";
        //                    }
        //                    else
        //                    {
        //                        headerImage.AlternateText = "Sort Ascending";
        //                        headerImage.ImageUrl = "~/Images/Down.png";
        //                    }
        //                    div.Controls.Add(headerImage);
        //                }
        //                headerButton.Controls.Add(div);
        //                headerButton.Font.Underline = false;
        //            }
        //        }
        //    }
        //}
    }
}
