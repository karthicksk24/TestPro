<%@ Reference Page="~/NTrustSessionPage.aspx" %>
<%@ OutputCache Location="None" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.1043, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Page Language="c#" Inherits="K2Web.Utility.Asc842Export" CodeFile="ASC842journalexport.aspx.cs" CodeFileBaseClass="K2Web.NTrustSessionPage" %>

<%@ Register TagPrefix="uc1" TagName="HeaderControl" Src="../HeaderControl.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>
        <%=strWindowTitle%>
    </title>
    <base target="_self" />
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE10" />
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="<%=strCssPath%>" type="text/css" rel="stylesheet">
    <script language="javascript" src="../JS/K2.js"></script>
    <script language="javascript" type="text/javascript">

        function isValidReport() {
            var e = document.getElementById("ddlTypeLedger");
            var strUserSelect = e.options[e.selectedIndex].value;
            if (strUserSelect == "-- Select --") {
                alert("<%=strErrMessage%>");
                return false;
            }
        }
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
        function isValid() {

            var fieldarr = new Array();
            cnt = 0;
            var ExportTemplate = document.getElementById("lblExportTemplate").innerHTML;
            fieldarr[cnt++] = [ExportTemplate, "ddlNamedFilter", "empty"];

            var ret = CheckForm("frmSummaryByType", fieldarr)

            if (ret == true) {

                var ie = document.all ? true : false;
                if (ie == true) {
                    document.frmSummaryByType.submit();
                }
                else {
                    document.getElementById(frmSummaryByType).submit();
                }
            }
            else {
                return false;
            }
        }
    </script>
</head>
<body class="td_Popup_Body">
    <form id="frmSummaryByType" method="post" runat="server">
        <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td width="100%">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr class="td_Popup_Header">
                            <td style="width: 3%"></td>
                            <td style="width: 10%">
                                <asp:Label ID="lblTypeLedger" CssClass="td_label" runat="server"></asp:Label>
                            </td>
                            <td style="width: 20%">
                                <asp:DropDownList ID="ddlTypeLedger" TabIndex="1" runat="server" CssClass="dropdown_Mandatory" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlTypeLedger_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 3%"></td>
                            <td class="lblPopupPageTitle" nowrap align="Left" style="padding-left: 44px !important" width="90%">
                                <asp:Label ID="lblExportFileSetting" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                    <asp:Label ID="lblErrorMessage" runat="server" Visible="False"></asp:Label></td>
            </tr>
            <tr class="td_Popup_Body" valign="top">
                <td class="td_LabelError" width="100%">
                    <div id="divReportData" style="background-color: transparent; text-align: left; height: 450px; overflow: auto; border: 1pt solid Gray; width: 870px;"
                        class="GridCellStyle">
                        <table bordercolor="#000000" width="100%" border="2">
                            <tr class="td_Popup_Body" valign="top">
                                <td width="100%">
                                    <%--<table width="50%" border="0">--%>
                                    <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr valign="top">
                                            <td>
                                                <asp:GridView ID="udgFieldList" runat="server" Height="22px" Width="100%" CellClickActionDefault="RowSelect"
                                                    HorizontalAlign="Left" AutoGenerateColumns="false" CellPadding="2" FooterStyle-Height="15px"
                                                    VerticalAlign="Top" OnRowDataBound="udgFieldList_RowDataBound" OnDataBound="udgFieldList_DataBound"
                                                    PagerStyle-Height="15px"
                                                    PageSize="50" ShowHeaderWhenEmpty="True" RowStyle-CssClass="RowStyle" AllowUpdateDefault="RowTemplateOnly"
                                                    CellPaddingDefault="0" Cursor="Hand" selectTypeRowDefault="Single"
                                                    AllowColumnMovingDefault="OnServer" GridLinesDefault="Horizontal" BorderCollapseDefault="Separate"
                                                    CaptionAlign="Top" BorderColor="LightGray" BackColor="White" Font-Size="X-Small"
                                                    Font-Overline="False" CellSpacing="0" Font-Strikeout="False" ViewStateMode="Enabled"
                                                    AllowPaging="True" Font-Bold="False">

                                                    <HeaderStyle Font-Underline="false" />
                                                    <Columns>
                                                        <asp:TemplateField SortExpression="ColumnName" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="70%">
                                                            <ItemTemplate>
                                                                <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                    <asp:Label ID="lbl1" runat="server" Text='<%#Eval("ColumnName")%>' />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Width" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                    <asp:Label ID="lbl2" runat="server" Text='<%#Eval("Width")%>' />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="GridHeader" Height="20px" Width="30px" />
                                                    <EditRowStyle HorizontalAlign="Left" />
                                                    <EmptyDataRowStyle HorizontalAlign="Left" />
                                                    <FooterStyle Height="15px"></FooterStyle>
                                                    <RowStyle CssClass="GridRowStyle" HorizontalAlign="Left" Height="22px" />
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
                    </div>
                </td>
            </tr>
            <tr height="40px">
                <td height="40px">&nbsp;</td>
            </tr>
            <tr class="td_Popup_Body">
                <td valign="bottom" align="center" height="30%">
                    <asp:Button ID="btnExport" runat="server" CssClass="PopupButton" Width="80" OnClick="btnExport_Click" OnClientClick="var CheckFuncIsTrue=isValidReport(); return CheckFuncIsTrue;"></asp:Button>&nbsp;
						<asp:Button ID="btnClose" runat="server" CssClass="PopupButton" Width="80" OnClick="btnClose_Click"></asp:Button></td>
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
