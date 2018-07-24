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

namespace K2Web.Utility
{
	/// <summary>
	///		Summary description for BatchEceptionOutPut.
	/// </summary>
	public partial class Asc842Export : NTrustSessionPage
	{
		
		//Constant variable declaration
        private const string SAVE_FILE_PATH = "SaveFilePath";//Changed to Get Save file path.
		private const string FIELDSLIST			= "APEXPORTFIELDSLIST";
		private const string FIELDSETTINGS		= "APEXPORTFIELDSETTINGS";
		private const string DELIMITCHAR		= "APExportDelimitChar";
		private const string EXPORTDIRECTORY	= "APExportDirectory";
		private const string GLOBAL_TYPE		= "GLOBAL_TYPE";
		private const string GLOBAL_DESC		= "GLOBAL_DESC";
		private const string GLOBAL_CODE		= "GLOBAL_CODE";
		private const string SELECT_VARNAME		= "VARNAME";
		private const string SELECT_FIELDNAME	= "VALUE1";
		private const string SELECT_FIELDLEN	= "VALUE2";
		private const string SELECT_FIELDKEY	= "VALUE3";
		private const string SELECT_FIELDVALUE	= "VALUE_NUMBER";
		private const string SELECT_FKEY		= "VALUE_FKEY";
        private const string ADD_FILE_PATH      = "APExport\\";

        private const string strExportType = "P";
		// Local Variables are declare here.
		protected string ErrExist;
		private string strSettings ;
		private string strDisplayName;
		private string strMenuKey ;
		private string strFilter	= "";
		private string strSort		= "";
		
		private string strDEFAULT_SELECTION_TEXT = System.Configuration.ConfigurationSettings.AppSettings["Select"];
		
		private string strDEFAULT_SELECTION_VALUE = System.Configuration.ConfigurationSettings.AppSettings["EmptyValue"];
		string file_name = "NTrust-edi-";
		string mon=DateTime.Today.Month.ToString();
		string day=DateTime.Today.Day.ToString();

		protected string strRedirectPath = ""; //added by Biru.
        private string ExportFile;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Getting Query String  Values to Local Variables
			strSettings		= Request.QueryString["ParentName"];
			strDisplayName	= Request.QueryString["DisplayName"];
			strMenuKey		= Request.QueryString["MenuKey"];
            NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
            ExportFile = GetString("ExportFile");	
				try
				{
					if(this.hidchkFlag.Value =="Y")
					{
						this.hidchkFlag.Value ="N";
						string scriptString = "<script language=JavaScript>";
						scriptString		+= "dialogArguments.document.forms[0].hidchkFlag.value='Y';dialogArguments.document.forms[0].submit();";
						scriptString		+= " window.close();</script>";
						this.RegisterStartupScript("Windclose", scriptString);
					}
                    if (!this.IsPostBack || hdnSelRow.Value == "true")
					{
                         //string confirm = "return isValid()";
                        //btnExport.Attributes.Add("onclick",confirm);
                        hdnSelRow.Value = "0";
						InitializePageProperties();
						FillDelimiter();
						GetFilterDetails();
						FillFieldsList();
						SetDefaultValue();
						
					}
					//added by Biru to handle session expire problem
					NTrust.Common.Session Sess_user = (NTrust.Common.Session) Session[NTRUST_SESSION];			
			
					NTrust.K2.BO.User ObjUser = (NTrust.K2.BO.User) Sess_user.LoginUser;
					if (ObjUser==null)
					{
						strRedirectPath = Request.ApplicationPath.ToString()+ "/Login.aspx";  
						string strQueryString = "?SE=Y";
						strQueryString += "&ModalWindow=Y";
						Response.Redirect(strRedirectPath+strQueryString);
					}//End
					
				}
				catch(NTrust.Util.NTrustException nex)
				{
					ErrExist = "Y";
					lblErrorMessage.Visible =true;
                    SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, ExportFile);
							
				}
				catch(Exception ex)
				{
					ErrExist = "Y";
					ShowCustomError(0,ex.Message.ToString()); 
								
				}
		
			

		}


		#region InitializePageProperties
		private void InitializePageProperties()
		{
			
			imgTemplate.ImageUrl		=	GetImagePath("Templatefilter");		
			
			InitializeResources("K2Web.Utility.Integration");
			lblExportFileSetting.Text	=	GetString("ExportSettings");
			lblExportTemplate.Text		=	GetString("ExportTemplate");

			lblExportFile.Text			=	GetString("ExportFile");
			//lblExportFile.Text="CHECKREG.TXT";

			lblDelimiterChar.Text		=	GetString("Delimiter");
			lgdExportOptions.InnerText	=	GetString("ExportOptions");
			chkOverWrite.Text			=	GetString("OverwriteFile");
			chkAppend.Text				=	GetString("Append");
			
			//spSaveMsg.InnerText			=	GetString("SaveMsg");
			if (mon.Length ==1) mon="0"+mon;
			if (day.Length ==1) day="0"+day;
			file_name=file_name+DateTime.Today.Year.ToString()+mon+day+".csv";
            spSaveMsg.InnerText = GetString("The") + file_name + GetString("Filesave") + System.Configuration.ConfigurationSettings.AppSettings[SAVE_FILE_PATH] + ADD_FILE_PATH;

			btnExport.Text				=	GetString("Export");
			btnFile.Text				=	GetString("Browse");
			btnClose.Text				=	GetString("Close");
            chkExportLocal.Text = GetString("APExporttoLocal");
			
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
			NTrust.K2.BO.ExportManager   ObjExpMan = null;
			ObjExpMan		= (NTrust.K2.BO.ExportManager) NTrustSession.CreateBusinessObject("ExportManager", 1);
			strFilter		= "varname ='"+ EXPORTDIRECTORY  + "'";
			strSort			= "";
			DataTable dtExportResult = ObjExpMan.GetExportVariable(strFilter,strSort);
			if(dtExportResult.Rows.Count >0)
			{
				string mon=DateTime.Today.Month.ToString();
				string day=DateTime.Today.Day.ToString();
				txtSelectedFile.Text = file_name;
			}
			strFilter		= "varname ='"+ DELIMITCHAR + "'";
			dtExportResult  = ObjExpMan.GetExportVariable(strFilter,strSort);
			if(dtExportResult.Rows.Count >0)
			{
				setDefault(this.ddlDelimiter,dtExportResult.Rows[0][SELECT_FIELDNAME].ToString());
			}
		}
		
		/*
		----------------------------------------------------------------------------------------------------
		Synopsis		:	This function will populate the Export  Fields List
		Input			:   udgFieldList as object E
		Output			:	
		-----------------------------------------------------------------------------------------------------
		*/
		private void FillFieldsList()
		{
			udgFieldList.DataSource = null;
			udgFieldList.DataBind();
            udgFieldList.DataBind();
                       
			DataTable dtTarget = new DataTable();
			System.Type colType = System.Type.GetType("System.Decimal");
			dtTarget.Columns.Add(SELECT_VARNAME,typeof(string));
			dtTarget.Columns.Add(SELECT_FIELDNAME,typeof(string));
			dtTarget.Columns.Add(SELECT_FIELDLEN,typeof(string));
			dtTarget.Columns.Add(SELECT_FIELDKEY,typeof(string));
			dtTarget.Columns.Add(SELECT_FIELDVALUE,colType);
			dtTarget.DefaultView.Sort = SELECT_FIELDVALUE;

			string[,] fields_arr = new string[,] {
				{"Savista Company/Store No.","8"},
				{"Invoice No.","20"},
				{"Invoice Date","8"},
				{"Invoice Amount","11"},
				{"Savista Vendor No.","8"},
				{"GL Account","8"},
				{"Payment Desc.","30"}
												 };

			
			for (int j=0; j<=(fields_arr.Length/2)-1;j++)
			{
				DataRow newField	= dtTarget.NewRow();
				newField[SELECT_VARNAME]		= j.ToString();
				newField[SELECT_FIELDNAME]	= fields_arr[j,0];
				newField[SELECT_FIELDLEN]		= fields_arr[j,1];
				newField[SELECT_FIELDKEY]		= j.ToString();
				newField[SELECT_FIELDVALUE]	= j;
				dtTarget.Rows.Add(newField);
			}

	        udgFieldList.DataSource = dtTarget;
			udgFieldList.DataBind();
          
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
            //this.imgTemplate.Click += new System.Web.UI.ImageClickEventHandler(this.imgTemplate_Click);
            //this.udgFieldList.InitializeLayout += new Infragistics.WebUI.UltraWebGrid.InitializeLayoutEventHandler(this.udgFieldList_InitializeLayout);

		}
		#endregion
        
		#region FillDelimiter

		// This Method will Fill Pay Month Details 
		private void FillDelimiter()
		{
				
			DataTable dtList		= null;
			NTrust.K2.BO.Common Objcom	= (NTrust.K2.BO.Common ) NTrustSession.CreateBusinessObject("Common", 1);
			string strFilter			= GLOBAL_TYPE +"='DELIMITCHAR'";
			string strSort				= "SORTORDER";
			dtList						= Objcom.GetGlobalItemList(strFilter,strSort);
			FillCombo(this.ddlDelimiter ,dtList,GLOBAL_CODE,GLOBAL_DESC);
		}
		#endregion

	

		protected void ddlNamedFilter_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				
				
				FillFieldsList();
			}
			catch(NTrust.Util.NTrustException nex)
			{
				ErrExist = "Y";
				lblErrorMessage.Visible =true;
                K2Web.NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("ExportFile"));
							
			}
			catch(Exception ex)
			{
				ErrExist = "Y";
				ShowCustomError(0,ex.Message.ToString()); 
								
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
			NTrust.K2.BO.ExportManager   ObjExpMan = null;
			ObjExpMan		= (NTrust.K2.BO.ExportManager) NTrustSession.CreateBusinessObject("ExportManager", 1);
			string strFilter		= "varname ='"+ FIELDSETTINGS +"'";
			string strSort			= "";
			DataTable dtExportResult = ObjExpMan.GetExportNamedFilter(strFilter,strSort);
		
			DataRow drNewRow = null;
			drNewRow = dtExportResult.NewRow();
			drNewRow[SELECT_VARNAME]			= "";
			drNewRow[SELECT_FKEY]		= strDEFAULT_SELECTION_VALUE;
			drNewRow["FILTERNAME"]		= strDEFAULT_SELECTION_TEXT;
			dtExportResult.Rows.Add(drNewRow);
			dtExportResult.DefaultView.Sort=SELECT_FKEY;
			FillCombo(this.ddlNamedFilter,dtExportResult,SELECT_FKEY,"FILTERNAME");
			
		
			
		}
		#endregion

		private void imgTemplate_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			string strQueryURL ="";
			try
			{
					 strQueryURL = "JournalExportFields.aspx?ParentName="+strSettings+"&MenuKey="+strMenuKey+"&Action=A";
			}
			catch(NTrust.Util.NTrustException nex)
			{
				ErrExist = "Y";
				lblErrorMessage.Visible =true;
                K2Web.NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("ExportFile"));
							
			}
			catch(Exception ex)
			{
				ErrExist = "Y";
				ShowCustomError(0,ex.Message.ToString()); 
								
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
			catch(NTrust.Util.NTrustException nex)
			{
				ErrExist = "Y";
				lblErrorMessage.Visible =true;
                K2Web.NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("ExportFile"));
							
			}
			catch(Exception ex)
			{
				ErrExist = "Y";
				ShowCustomError(0,ex.Message.ToString()); 
								
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
			bool blnWriteFlag =false;
			string[] strSQLScript = new string[4];
			try
			{
				NTrust.K2.BO.Export objExport = (NTrust.K2.BO.Export)NTrustSession.CreateBusinessObject("AEXPORT", 1);
				NTrust.K2.BO.Export.ExportFieldStruct  ExpFldStruct  = new  NTrust.K2.BO.Export.ExportFieldStruct();
					
				string strPKEY	= ddlNamedFilter.SelectedValue;	
				string strDelimiter = this.ddlDelimiter.SelectedValue;
				string strFilePath = this.txtSelectedFile.Text ;

				bool blnOpenOrOverWrite = (this.chkAppend.Checked == true)?true:false;

				string strFilter	= "";
				/*
					if (this.chkSave.Checked ) 
					{
						// Here Deleting the Demlimiter Record
						strFilter		= "varname='" + DELIMITCHAR + "'";
						strSQLScript[0] = "DELETE FROM Y_VARIABLES WHERE " + strFilter ;
						// Here Deleting the Export File Directory Record
						strFilter		= "varname='" + EXPORTDIRECTORY + "'";
						strSQLScript[1] = "DELETE FROM Y_VARIABLES WHERE " + strFilter ;
						// Here Forming the Insert Script for the Demlimiter Record
						ExpFldStruct.strTemplateName	= "";
						ExpFldStruct.strValue1		= strDelimiter;
						ExpFldStruct.strValue2		= "";
						ExpFldStruct.strValue3		= "";
						ExpFldStruct.strVarName		= DELIMITCHAR;
						ExpFldStruct.strValueDate	= "";
						ExpFldStruct.strSeqence		= "0";
						strSQLScript[2] = objExport.GenInsExportFieldsSetting(ExpFldStruct);

					// Here Forming the Insert Script for the  Export File Directory Record
						ExpFldStruct.strTemplateName	= "";
						ExpFldStruct.strValue1		= strFilePath;
						ExpFldStruct.strValue2		= "";
						ExpFldStruct.strValue3		= "";
						ExpFldStruct.strVarName		= EXPORTDIRECTORY;
						ExpFldStruct.strValueDate	= "";
						ExpFldStruct.strSeqence		= "0";
						strSQLScript[3] = objExport.GenInsExportFieldsSetting(ExpFldStruct);
						// Here Execution the Script  Both Deleting and Inserting in a Single Transaction
						objExport.AddExportFieldsSetting(strSQLScript);
						
					}

					// Here Getting the Template details
					NTrust.K2.BO.ExportManager   ObjExpMan = null;
					ObjExpMan		= (NTrust.K2.BO.ExportManager) NTrustSession.CreateBusinessObject("ExportManager", 1);
					strFilter		= "varname ='"+ FIELDSETTINGS +"' and VALUE_FKEY='"+strPKEY +"'";
					strSort			= SELECT_FIELDVALUE;
					DataTable dtExportResult = ObjExpMan.GetExportVariable(strFilter,strSort);

*/
				NTrust.K2.BO.ExportManager   ObjExpMan = null;
				ObjExpMan = (NTrust.K2.BO.ExportManager) NTrustSession.CreateBusinessObject("ExportManager", 1);
				// Here forming the Select Statement Based on the Template Selected
				int [] intSize= new int[50];
				intSize[0]=8;
				intSize[1]=20;
				intSize[2]=8;
				intSize[3]=11;
				intSize[4]=8;
				intSize[5]=8;
				intSize[6]=30;

				string strQuery = "SELECT * FROM V_K2_EXPORTJOURNALFIELDSCOCOS";
				strQuery +=" ORDER BY PARENT_ID,GLCODE,VENDOR_NUMBER,DUE_DATE,CHECK_KEY";

				if(strFilePath != "")
				{
					//Cocos Result data to Text File
					strDelimiter=","; // For CSV file.

                    string strpath = System.Configuration.ConfigurationSettings.AppSettings[SAVE_FILE_PATH] + ADD_FILE_PATH;
                    bool folderExists = Directory.Exists(strpath);
                    if (!folderExists)
                    {
                        Directory.CreateDirectory(strpath);
                    }
					strFilePath = strpath + strFilePath;
					blnWriteFlag = ObjExpMan.ExportToCOCOSFile(strQuery,strDelimiter,strFilePath,blnOpenOrOverWrite,intSize);
					if(blnWriteFlag)
					{
                        string url = "";//changes made for CR - Journal Accrual Export for not to update Accrual Records. by Biru
						if(chkExportLocal.Checked)
						{
                            url = "JounralExportMessage.aspx?ParentName=" + strSettings + "&DisplayName=" + strDisplayName + "&MenuKey=" + strMenuKey + "&ExportType=" + strExportType + "&FileName=" + txtSelectedFile.Text + "";
						}
						else
						{
                            url = "JounralExportMessage.aspx?ParentName=" + strSettings + "&DisplayName=" + strDisplayName + "&MenuKey=" + strMenuKey + "&ExportType=" + strExportType + "&FileName=";
						}
						string strScript = "<script language=JavaScript>window.showModalDialog('"+url+"',window,'status:no;dialogWidth:465px;dialogHeight:175px;');</script>";
						this.RegisterStartupScript("Winclose", strScript);
					}
					else
					{
                        NTrustSessionPage.InitializeResources("K2Web.CommonResource");
                        this.CreateMessageAlert(this, GetString("NoRectoExportExcel"));
							
					}
				}
			}
			
			
			catch(NTrust.Util.NTrustException nex)
			{
				if(nex.ErrorCode == 500000)
				{
					ErrExist = "Y";
					lblErrorMessage.Visible =true;
                    NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                    CreateMessageAlert(this, GetString("FileAccess"));
                   //lblErrorMessage.Text    = "File is accessed by Another Process. Close the file and export again";
				}
				else
				{
					ErrExist = "Y";
					lblErrorMessage.Visible =true;
                    NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
					SetErrorMessage(lblErrorMessage,nex.ErrorCode,nex.Message,GetString("Export File"));
				}
							
			}
			catch(System.IO.FileNotFoundException Ferr)
			{
				ErrExist = "Y";
				lblErrorMessage.Visible =true;
				SetErrorMessage(lblErrorMessage,0,Ferr.Message.ToString(),"");							
			}			
			catch(Exception ex)
			{
				ErrExist = "Y";
				ShowCustomError(0,ex.Message.ToString()); 
								
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
			catch(NTrust.Util.NTrustException nex)
			{
				ErrExist = "Y";
				lblErrorMessage.Visible =true;
                NTrustSessionPage.InitializeResources("K2Web.Utility.Integration");
                SetErrorMessage(lblErrorMessage, nex.ErrorCode, nex.Message, GetString("ExportFile"));
							
			}
			catch(Exception ex)
			{
				ErrExist = "Y";
				ShowCustomError(0,ex.Message.ToString()); 
								
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
