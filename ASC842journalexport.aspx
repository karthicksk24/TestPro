<%@ Reference Page="~/NTrustSessionPage.aspx" %>
<%@ OutputCache Location="None" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.1043, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page language="c#" Inherits="K2Web.Utility.Asc842Export" CodeFile="ASC842journalexport.aspx.cs" CodeFileBaseClass="K2Web.NTrustSessionPage" %>
<%@ Register TagPrefix="uc1" TagName="HeaderControl" Src="../HeaderControl.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>
			<%=strWindowTitle%>
		</title>
		<base target="_self"/>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
        <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE10" />
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="<%=strCssPath%>" type=text/css rel=stylesheet>
		<script language="javascript" src="../JS/K2.js"></script>
		<script language="javascript" type="text/javascript">
		     function SelectRow(row, iIndex) {
		         var _rows = row.parentNode.childNodes;

		         try {
		             for (i = 1; i < _rows.length; i++) {
		                 _rows[i].className = "GridRowStyle";
		             }

		         }
		         catch (e) { }
		         var _selectedRowFirstCell = row.getElementsByTagName("td")[0];
		         if (iIndex != 0)
		             _row.className = "GridSelectedItem";
		         else
		             row.className = "GridSelectedItem";
		         document.frmSummaryByType.hdnSelRow.value = row.rowIndex - 1;
		     }
		function  isValid()
		{
	
			var fieldarr=new Array();
			cnt=0;
			var ExportTemplate = document.getElementById("lblExportTemplate").innerHTML;
			fieldarr[cnt++] = [ExportTemplate, "ddlNamedFilter", "empty"];	
			
			var ret=CheckForm("frmSummaryByType",fieldarr)

			if (ret==true)
				{

				    var ie = document.all ? true : false;
				    if (ie == true) {
				        document.frmSummaryByType.submit();
				    }
				    else {
				        document.getElementById(frmSummaryByType).submit();
				    }	
				}
				else
				{
					return false;
				}
          }
    </script>
	</head>
	<body class="td_Popup_Body">
		<form id="frmSummaryByType" method="post" runat="server">
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td width="100%">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr class="td_Popup_Header">
								<td class="lblPopupPageTitle" noWrap align="center" width="90%"><asp:label id="lblExportFileSetting" Runat="server"></asp:label></td>
							</tr>
						</table>
						<asp:label id="lblErrorMessage" runat="server" Visible="False"></asp:label></td>
				</tr>
				<tr class="td_Popup_Body" vAlign="top">
					<td class="td_LabelError" width="100%">
						<table borderColor="#000000" width="100%" border="2">
							<tr class="td_Popup_Body" valign="top">
								<td vAlign="top" width="40%">
									<table width="100%" border="0">
										<tr>
											<td class="td_label" width="50%"><asp:label id="lblExportTemplate" Runat="server" Visible="False"></asp:label></td>
										</tr>
										<tr>
											<td class="td_control"><asp:dropdownlist id="ddlNamedFilter" runat="server" CssClass="Dropdown" Width="150px" AutoPostBack="True"
													Visible="False" onselectedindexchanged="ddlNamedFilter_SelectedIndexChanged"></asp:dropdownlist>
												<asp:imagebutton id="imgTemplate" tabIndex="1" runat="server" Visible="False"></asp:imagebutton></td>
										</tr>
										<tr>
											<td class="td_label">&nbsp;</td>
										</tr>
										<tr>
											<td class="td_label" width="50%"><asp:label id="lblExportFile" Runat="server" Visible="False"></asp:label></td>
										</tr>
										<tr>
											<td class="td_control">
												<asp:textbox id="txtSelectedFile" runat="server" CssClass="TextBox_Mandatory" Width="184px" MaxLength="250"
													ReadOnly="True" Visible="False"></asp:textbox>&nbsp;<asp:button id="btnFile" runat="server" CssClass="PopupButton" Height="19px" Visible="False" onclick="btnFile_Click"></asp:button></td>
										</tr>
										<tr>
											<td class="td_label">&nbsp;</td>
										</tr>
										<tr>
											<td class="td_label"><asp:label id="lblDelimiterChar" Runat="server" Visible="False"></asp:label></td>
										</tr>
										<tr>
											<td class="td_control"><asp:dropdownlist id="ddlDelimiter" runat="server" CssClass="Dropdown" Width="112px" Visible="False"></asp:dropdownlist></td>
										</tr>
										<tr>
											<td class="td_control" noWrap><input class="checkbox" id="chkSave" type="checkbox" value="checkbox" name="chkSave" runat="server"
													visible="false"><span id="spSaveMsg" runat="server"></span></td>
										</tr>
										<tr>
											<td  class="td_control"><asp:CheckBox ID="chkExportLocal" Runat="server" Checked="true" ></asp:CheckBox></td>
										</tr>
										<tr>
											<td>&nbsp;</td>
										</tr>
										<tr>
											<td class="td_label">
												<fieldset class="td_label"><legend class="td_label" id="lgdExportOptions" runat="server"></legend>
													<table width="100%" border="0">
														<tr>
															<td class="td_label">&nbsp;</td>
														</tr>
														<tr>
															<td class="td_label" style="HEIGHT: 11px">&nbsp;
																<asp:radiobutton id="chkOverWrite" runat="server" CssClass="radiobutton" GroupName="radiobutton"
																	Checked="True"></asp:radiobutton></td>
														</tr>
														<tr>
															<td class="td_label">&nbsp;</td>
														</tr>
														<tr>
															<td class="td_label">&nbsp;
																<asp:radiobutton id="chkAppend" runat="server" CssClass="radiobutton" GroupName="radiobutton"></asp:radiobutton></td>
														</tr>
													</table>
												</fieldset>
											</td>
										</tr>
									</table>
								</td>
								<td width="50%">
									<%--<table width="50%" border="0">--%> 
                                    <table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr valign="top">
                                        <td> 
                                                        <asp:GridView ID="udgFieldList" runat="server" Height="22px" Width="100%" CellClickActionDefault="RowSelect"
                                                            HorizontalAlign="Left" AutoGenerateColumns="false" CellPadding="2" FooterStyle-Height="15px"
                                                            VerticalAlign="Top" OnRowDataBound ="udgFieldList_RowDataBound" OnDataBound= "udgFieldList_DataBound"
                                                            PagerStyle-Height="15px"
                                                            PageSize="50" ShowHeaderWhenEmpty="True" RowStyle-CssClass="RowStyle" AllowUpdateDefault="RowTemplateOnly"
                                                            CellPaddingDefault="0" Cursor="Hand" selectTypeRowDefault="Single"
                                                            AllowColumnMovingDefault="OnServer" GridLinesDefault="Horizontal" BorderCollapseDefault="Separate"
                                                            CaptionAlign="Top" BorderColor="LightGray" BackColor="White"  Font-Size="X-Small"
                                                            Font-Overline="False" CellSpacing="0" Font-Strikeout="False" ViewStateMode="Enabled"
                                                            AllowPaging="True" Font-Bold="False"> 
                                                           
                                                          <HeaderStyle Font-Underline = "false" />
                                                            <Columns>
                                                                <asp:TemplateField SortExpression="VALUE1" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="70%">
                                                                    <ItemTemplate>
                                                                        <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                            <asp:Label ID="lbl1" runat="server" Text='<%#Eval("VALUE1")%>' />
                                                                        </div>
                                                                    </ItemTemplate>
                                                               </asp:TemplateField>
                                                                <asp:TemplateField SortExpression="VALUE2" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="30%">
                                                                    <ItemTemplate>
                                                                        <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("VALUE2")%>' />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                  </asp:TemplateField>
                                                             </Columns>
                                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GridHeader" Height="20px" Width="30px" />
                                                        <EditRowStyle HorizontalAlign="Left" />
                                                        <EmptyDataRowStyle HorizontalAlign="Left" />
                                                        <FooterStyle Height="15px"></FooterStyle>
                                                        <RowStyle CssClass="GridRowStyle" HorizontalAlign="Left" Height="22px"/>
                                                        <SelectedRowStyle CssClass="GridSelectedItem" />
                                                        <PagerSettings Visible="False" />
                                                        <SortedAscendingHeaderStyle Font-Underline="false" />
                                                        <SortedDescendingHeaderStyle Font-Underline="false" />
                                                        <PagerStyle CssClass="GridPage" ForeColor="White" HorizontalAlign="right" />
                                                        </asp:GridView>
                                                    </td>
										     </tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr class="td_Popup_Body">
					<td vAlign="bottom" align="center" height="30%"><asp:button id="btnExport" runat="server" CssClass="PopupButton" width="80" onclick="btnExport_Click"></asp:button>&nbsp;
						<asp:button id="btnClose" runat="server" CssClass="PopupButton" width="80" onclick="btnClose_Click"></asp:button></td>
				</tr>
				<tr>
					<td height="70%">&nbsp;</td>
				</tr>
			</table>
			<input id="hidchkFlag" type="hidden" value="N" name="hidchkFlag" runat="server">
            <input type="hidden" runat="server" id="hdnSelRow" />
            </form>
	</body>
</html>
