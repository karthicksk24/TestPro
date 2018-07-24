<%@ Reference Page="~/NTrustSessionPage.aspx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderControl" Src="../HeaderControl.ascx" %>

<%@ Page Language="c#" Inherits="K2Web.Utility.ASC842journalexport" CodeFile="ASC842journalexportlist.aspx.cs" CodeFileBaseClass="K2Web.NTrustSessionPage" %>

<%@ Register TagPrefix="uc1" TagName="Export" Src="../Export.ascx" %>
<%@ Register TagPrefix="uc3" TagName="GridNav" Src="../GridNavigationpopup.ascx" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.3, Version=7.3.20073.1043, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
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
    <script type="text/javascript" language="javascript" src="../../JS/K2.js"></script>
    <script type="text/javascript" src="../../JS/jquery.js"></script>
    <script type="text/javascript" src="../../JS/thickbox.js"></script>
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


            //$("#udgExportList tr").live("click", function () {

            //    alert($(this).closest('tr').find(".GetEmail").attr("Email"));
            //});

           // alert(row.rowIndex);

            <%--var fufile = document.getElementById('<%= udgExportList.rows[row.rowIndex].Cells[2].FindControl("fuFile").ClientID %>').value;--%>

            var gv = document.getElementById("<%=udgExportList.ClientID %>");
            //   alert(gv.rows[row.rowIndex].cells[0].childNodes[1].innerHTML);

            //var rowIndex = row.rowIndex - 1;
            //var customerId = row.cells[0].innerHTML;
            var Pkey = gv.rows[row.rowIndex].cells[0].getElementsByTagName("input")[0].value;

          //  alert(Pkey);

            //var hdntxt = $(gv.rows[row.rowIndex].cells[0].childNodes[1].innerHTML).next("input[name$=HiddenField1]").val();
            //alert(hdntxt);

            // alert(row.cells[0].innerHTML);            
            //alert(lblCategoryID);
           <%-- var grid = document.getElementById('<%=udgExportList.ClientID %>');
            var cell = grid.rows[row.rowIndex].cells[0];--%>
            // alert(cell.innerHTML);

            //var label = grid.rows[row.rowIndex].cells[0].elements[0];
            //alert(label.InnerHTML);
            document.frmSummaryByType.hdnPkey.value = Pkey;
           // alert(document.frmSummaryByType.hdnPkey.value);
            document.frmSummaryByType.hdnSelRow.value = row.rowIndex;
            document.frmSummaryByType.hdnExport.value = "lblExportDesc";
        }

        function showGrid() {
            alert("show");

            var colIndOldIcon = $("table[id*=udgExportList] th").index($("table[id*=udgExportList] th:contains('lblExportDesc')"));
            alert(colIndOldIcon);
            var uOldValue = $('#' + cellId).closest("tr").find('td:nth(' + colIndOldIcon + ')').text();
            alert(uOldValue)

            var tbl = $("[id$=udgExportList]");
            alert(tbl);
            var rows = tbl.find('tr');
            alert(rows);
            for (var index = 0; index < rows.length; index++) {
                var row = rows[index];
                var userName = $(row).find("[id*=lblExportDesc]").text();
                var userId = $(row).find("[id*=lbl2]").text();

                alert(userName + " - " + userId);
            }
        }

        function SelectRow1(row, iIndex) {
            var _rows = row.parentNode.childNodes;
            var hRow = 1;
            if (!document.all) {
                hRow = 2;
            }
            try {
                for (i = 1; i < _rows.length - hRow; i++) {
                    _rows[i].className = "GridRowStyle";
                }

            }
            catch (e) { }
            var _selectedRowFirstCell = row.getElementsByTagName("td")[0];
            if (iIndex != 0)
                _row.className = "GridSelectedItem";
            else
                row.className = "GridSelectedItem";
            document.frmSummaryByType.hdnSelRow1.value = row.rowIndex - 1;
            document.frmSummaryByType.hdnExport.value = "Type";
            //                document.frmSummaryByType.submit();
        }
        //            function fOpen() {
        //                var strurl = '../ApplicationConfiguration/ExcelLocalSave.aspx?FileName=ExportToExcel123.xls&CSS=' + '<%=strCssPath%>' + '&VS=' + '<%=strWindowTitle%>';
        //                window.open(strurl, 'window', 'location=no,toolbar=no,menubar=no,height=300,width=300,left=300,top=100');
        //            }
        function fOpen() {
            var grid = document.getElementById('udgExportList');
            var RCount = grid.rows.length - 1;
            if (RCount > 0) {
                var strurl = '../ApplicationConfiguration/ExcelLocalSave.aspx?FileName=ExportToExcel123.xls&CSS=' + '<%=strCssPath%>' + '&VS=' + '<%=strWindowTitle%>';
                window.open(strurl, 'window', 'location=no,toolbar=no,menubar=no,height=300,width=300,left=300,top=100');
            }
        }
       

        function isValid() {
            var fieldarr = new Array();
            cnt = 0;
            fieldarr[cnt++] = ["<%=ExportFrom%>", "txtExportFrom_t", "date"];
            fieldarr[cnt++] = ["<%=ExportTo %>", "txtExportTo_t", "date"];
            fieldarr[cnt++] = ["<%=RecvFrom %>", "txtReceivedFrom_t", "date"];
            fieldarr[cnt++] = ["<%=RecvTo%>", "txtReceivedTo_t", "date"];
            var ret = CheckForm("frmModSpaceExport", fieldarr);

            if (ret == true) {
                var ExportFromDate = document.getElementById("txtExportFrom_t").value;
                var ExportToDate = document.getElementById("txtExportTo_t").value;
                var ReceivedFromDate = document.getElementById("txtReceivedFrom_t").value;
                var ReceivedToDate = document.getElementById("txtReceivedTo_t").value;

                var ret1 = true;
                if (ExportFromDate != null && ExportFromDate != "") {
                    if (ExportToDate == null || ExportToDate == "") {
                        alert("<%=FillExportDateTo%>");
                        eval("if (!document.frmModSpaceExport.txtExportTo_t.disabled){document.frmModSpaceExport.txtExportTo_t.focus()}")

                        ret1 = false;
                        return false;
                    }
                }
                if (ExportToDate != null && ExportToDate != "") {
                    if (ExportFromDate == null || ExportFromDate == "") {
                        alert("<%=FillExportDateFrom%>");
                        eval("if (!document.frmModSpaceExport.txtExportFrom_t.disabled){document.frmModSpaceExport.txtExportFrom_t.focus()}")

                        ret1 = false;
                        return false;
                    }
                }
                if (ReceivedFromDate != null && ReceivedFromDate != "") {
                    if (ReceivedToDate == null || ReceivedToDate == "") {
                        alert("<%=FillRecDateTo%>");
                        eval("if (!document.frmModSpaceExport.txtReceivedTo_t.disabled){document.frmModSpaceExport.txtReceivedTo_t.focus()}")

                        ret1 = false;
                        return false;
                    }
                }
                if (ReceivedToDate != null && ReceivedToDate != "") {
                    if (ReceivedFromDate == null || ReceivedFromDate == "") {
                        alert("<%=FillRecDateFrom%>");
                        eval("if (!document.frmModSpaceExport.txtReceivedFrom_t.disabled){document.frmModSpaceExport.txtReceivedFrom_t.focus()}")

                        ret1 = false;
                        return false;
                    }
                }


                if (ret1 == true) {
                    if (document.all.item("txtExportFrom").value != '' && document.all.item("txtExportTo").value != '') {
                        if (frmModSpaceExport.txtExportFrom_t != null && frmModSpaceExport.txtExportFrom_t != "" && frmModSpaceExport.txtExportTo_t != null && frmModSpaceExport.txtExportTo_t != "") {
                            var ret2 = DateCompare(frmModSpaceExport.txtExportFrom_t, frmModSpaceExport.txtExportTo_t, "<%=ExportFrom%>", "<%=ExportTo%>");
                            if (ret2 == false) {

                                return false;
                            }
                        }
                    }

                    if (document.all.item("txtReceivedFrom").value != '' && document.all.item("txtReceivedTo").value != '') {
                        if (frmModSpaceExport.txtReceivedFrom_t != null && frmModSpaceExport.txtReceivedFrom_t != "" && frmModSpaceExport.txtReceivedTo_t != null && frmModSpaceExport.txtReceivedTo_t != "") {
                            var ret2 = DateCompare(frmModSpaceExport.txtReceivedFrom_t, frmModSpaceExport.txtReceivedTo_t, "<%=RecvFrom%>", "<%=RecvTo%>");
                            if (ret2 == false) {

                                return false;
                            }
                        }
                    }
                    document.frmModSpaceExport.submit();
                }

                else {
                    return false;
                }
            }
            else {
                return false;
            }

        }
        function ShowHistory() {
            window.showModalDialog('ModSpaceExportHistory.aspx', window, 'status:no;dialogHeight=550px;dialogWidth=930px;scroll:no;');
        }
    </script>
    <style>
        .GetEmail {
            display: none;
        }
    </style>
</head>
<body>
    <form id="frmSummaryByType" method="post" runat="server">
        <uc1:HeaderControl ID="HeaderControl1" runat="server"></uc1:HeaderControl>
        <input id="hidchkFlag" type="hidden" value="N" name="hidchkFlag" runat="server">
        <table height="800px" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td class="td_TaskAreaBody" width="10%" rowspan="2">
                    <table height="800px" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td align="center" class="td_Left_BackGround">
                                <img id="imgTasks" border="0" runat="server"><br>
                                <asp:ImageButton ID="imgbtnExport" runat="server" AlternateText="Export"></asp:ImageButton><br>
                                <%--<asp:ImageButton id="imgbtnMarkReceived" runat="server" AlternateText="Mark received"></asp:ImageButton><br>
									<asp:ImageButton id="imgbtnMarkUnReveived" runat="server" AlternateText="Mark unreceived"></asp:ImageButton><br>
									<asp:ImageButton id="imgbtnUndoExport" runat="server" AlternateText="Undo export"></asp:ImageButton><br>--%>
                                <%--<asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" OnClientClick="javascript:fOpen();">
                                    <img id="imgExport" runat="server" border="0" alt="Export To Excel">
                                </asp:LinkButton><br>--%>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="td_Left_BackGround" valign="top" width="91%" height="85%"></td>
                        </tr>
                        <tr>
                            <td class="td_img_Fade">
                                <asp:Image ID="imgFade" runat="server"></asp:Image></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="td_WorkAreaBody" valign="top" colspan="2">
                    <br>
                    <table>
                        <tr>
                            <td width="1%"></td>
                            <td class="pagetitle" width="99%">
                                <asp:Label ID="lblIntegration" runat="server" CssClass="lblPageTitle"></asp:Label>&nbsp;
									<asp:Image ID="imgSeparator" runat="server"></asp:Image>&nbsp;
									<asp:Label ID="lblJournalExport" runat="server"  CssClass="lblSubPageTitle"></asp:Label></td>
                        </tr>
                        <tr>
                            <td width="1%"></td>
                            <td class="td_LabelError">
                                <asp:Label ID="lblErrorMessage" runat="server" Visible="False"></asp:Label>
                                <table width="100%" class="xy">
                                    <tr valign="top">
                                        <td class="td_LabelError">

                                            <div id="div2" style="background-color: transparent; text-align: left; overflow: auto; border: 1pt solid Gray; width: 870px;"
                                                class="GridCellStyle">
                                                <uc3:GridNav ID="GridNav1" runat="server"></uc3:GridNav>
                                            </div>

                                            <div id="divReportData" style="background-color: transparent; text-align: left; height: 350px; overflow: auto; border: 1pt solid Gray; width: 870px;"
                                                class="GridCellStyle">
                                                <table width="100%" border="0px" style="vertical-align: top;" cellpadding="0" cellspacing="0">
                                                    <tr valign="top">
                                                        <td>
                                                            <asp:GridView ID="udgExportList" runat="server" Height="30px"
                                                                Width="100%" CellClickActionDefault="RowSelect"
                                                                HorizontalAlign="Left" AutoGenerateColumns="false" CellPadding="2" FooterStyle-Height="15px"
                                                                VerticalAlign="Top" RowStyle-Height="18"
                                                                OnSorting="udgExportList_Sorting" PagerStyle-Height="15px"
                                                                ShowHeaderWhenEmpty="True" PageSize="50"
                                                                RowStyle-CssClass="RowStyle" AllowUpdateDefault="RowTemplateOnly"
                                                                CellPaddingDefault="0" Cursor="Hand" selectTypeRowDefault="Single" RowHeightDefault="18px"
                                                                AllowColumnMovingDefault="OnServer" GridLinesDefault="Horizontal" BorderCollapseDefault="Separate"
                                                                CaptionAlign="Top" BorderColor="LightGray" BackColor="White" Font-Overline="False"
                                                                Font-Size="X-Small" CellSpacing="0" Font-Strikeout="False" ViewStateMode="Enabled"                                                                
                                                                AllowSorting="True" AllowPaging="true"
                                                                OnRowDataBound="udgExportList_RowDataBound"
                                                                OnDataBound="udgExportList_DataBound">
                                                                <Columns>
                                                                    <asp:TemplateField SortExpression="LeaseClass_desc" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="140px" HeaderStyle-CssClass="GridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="GridAltItem">
                                                                        <ItemTemplate>
                                                                            <div style="width: 140px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                                <asp:Label ID="lblExportDesc" runat="server" Text='<%#Eval("LeaseClass_desc")%>' />
                                                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("PKEY") %>' />
                                                                                <asp:Label ID="lbl1" runat="server" Text='<%#Eval("LeaseClass_desc")%>' Visible="false" />
                                                                                <asp:Label ID="lblPkey1" runat="server" Visible="false" Text='<%#Eval("PKEY")%>' />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField SortExpression="PKEY" Visible="false" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="140px" HeaderStyle-CssClass="GridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="GridAltItem">
                                                                        <ItemTemplate>
                                                                            <div style="width: 140px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                                <asp:Label ID="lblPKEY" runat="server" Text='<%#Eval("PKEY")%>' />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%-- <asp:TemplateField SortExpression="EXPORTDATE" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="140px"  HeaderStyle-CssClass="GridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="GridAltItem">
                                                                    <ItemTemplate>
                                                                        <div style="width: 140px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("EXPORTDATE","{0:" + DATE_FORMAT + "}")%>' />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                    <asp:TemplateField SortExpression="Period" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="140px" HeaderStyle-CssClass="GridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="GridAltItem">
                                                                        <ItemTemplate>
                                                                            <div style="width: 140px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                                <asp:Label ID="lblPeriod" runat="server" Text='<%#Eval("Period")%>' />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField SortExpression="FilterType_desc" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="140px" HeaderStyle-CssClass="GridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="GridAltItem">
                                                                        <ItemTemplate>
                                                                            <div style="width: 140px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                                <asp:Label ID="lblFilterType_desc" runat="server" Text='<%#Eval("FilterType_desc")%>' />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField SortExpression="FILTERNAME_desc" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="140px" HeaderStyle-CssClass="GridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="GridAltItem">
                                                                        <ItemTemplate>
                                                                            <div style="width: 140px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                                <asp:Label ID="lblFILTERNAME_desc" runat="server" Text='<%#Eval("FILTERNAME_desc")%>' />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField SortExpression="TRANS_DATE" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="140px" HeaderStyle-CssClass="GridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="GridAltItem">
                                                                        <ItemTemplate>
                                                                            <div style="width: 140px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                                <asp:Label ID="lblTRANS_DATE" runat="server" Text='<%#Eval("TRANS_DATE","{0:" + DATE_FORMAT + "}")%>' />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField SortExpression="PROCESS_BY" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="140px" HeaderStyle-CssClass="GridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="GridAltItem">
                                                                        <ItemTemplate>
                                                                            <div style="width: 140px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                                <asp:Label ID="lblPROCESS_BY" runat="server" Text='<%#Eval("PROCESS_BY")%>' />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField SortExpression="PROCESSS_STATUS" Visible="false" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="140px" HeaderStyle-CssClass="GridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="GridAltItem">
                                                                        <ItemTemplate>
                                                                            <div style="width: 140px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                                <asp:Label ID="lblPROCESSS_STATUS" runat="server" Text='<%#Eval("PROCESSS_STATUS")%>' />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                                <HeaderStyle HorizontalAlign="Left" CssClass="GridHeader" Height="20px" />
                                                                <EditRowStyle HorizontalAlign="Left" />
                                                                <EmptyDataRowStyle HorizontalAlign="Left" />
                                                                <FooterStyle Height="15px"></FooterStyle>
                                                                <RowStyle CssClass="GridRowStyle" HorizontalAlign="Left" Height="22px" />
                                                                <SelectedRowStyle CssClass="GridSelectedItem" />
                                                                <SortedAscendingHeaderStyle Font-Underline="false" />
                                                                <SortedDescendingHeaderStyle Font-Underline="false" />
                                                                <PagerSettings Visible="false" />
                                                                <PagerStyle CssClass="GridPage" ForeColor="White" HorizontalAlign="right" Height="20px" Font-Size="8" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%--<tr height="0">
								<td>&nbsp;</td>
							</tr>
							<tr class="">
								<td width="1%"></td>
								<td>
									<table width="100%" border="0">
										<tr>
											<td class="" width="4%" align="left"><asp:imagebutton id="ImageButton1" runat="server" Height="20px" ToolTip="Open File"></asp:imagebutton></td>
											<td width="96%" align="left"><asp:label id="lblExportFile" Runat="server" CssClass="td_label"></asp:label></td>
										</tr>
                                        <table width="100%" class="yz">
										<tr>
											<td>
                                         <div id="div1"  style="background-color: transparent; text-align: left; height: 220px;
                                                                                 overflow: auto; border: 1pt solid Gray; width: 870px;" class="GridCellStyle">
                                             <table width="100%" border="0px"  style="vertical-align:top;" cellpadding="0" cellspacing="0">
										<tr valign="top">
											<td>
                                                      <asp:GridView ID="udgFileList" runat="server" Height="22px" 
                                                            Width="100%" CellClickActionDefault="RowSelect"
                                                            HorizontalAlign="Left" AutoGenerateColumns="false" CellPadding="2" FooterStyle-Height="15px"
                                                            VerticalAlign="Top" RowStyle-Height="18" PageSize="7"
                                                            OnSorting="udgFileList_Sorting" PagerStyle-Height="15px" 
                                                            ShowHeaderWhenEmpty="True" RowStyle-CssClass="RowStyle" AllowUpdateDefault="RowTemplateOnly"
                                                            CellPaddingDefault="0" Cursor="Hand" selectTypeRowDefault="Single" RowHeightDefault="18px"
                                                            AllowColumnMovingDefault="OnServer" GridLinesDefault="Horizontal" BorderCollapseDefault="Separate"
                                                            CaptionAlign="Top" BorderColor="LightGray" BackColor="White" Font-Overline="False"
                                                            Font-Size="X-Small" CellSpacing="0" Font-Strikeout="False" ViewStateMode="Enabled"
                                                            AllowPaging="True" AllowSorting="True" OnPageIndexChanging="udgFileList_SelectedIndexChanged"
                                                            OnRowDataBound="udgFileList_RowDataBound" 
                                                            OnDataBound="udgFileList_DataBound">
                                                            <HeaderStyle Font-Underline = "false" />
                                                            <Columns>
                                                                <asp:TemplateField SortExpression="FILENAME" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="200px"  HeaderStyle-CssClass="GridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="GridAltItem">
                                                                    <ItemTemplate>
                                                                        <div style="width: 200px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                            <asp:Label ID="lbl1" runat="server" Text='<%#Eval("FILENAME")%>' />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField SortExpression="FILESIZE" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="160px"  HeaderStyle-CssClass="GridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="GridAltItem">
                                                                    <ItemTemplate>
                                                                        <div style="width: 160px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("FILESIZE")%>' />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField SortExpression="FILETYPE" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="160px"  HeaderStyle-CssClass="GridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="GridAltItem">
                                                                    <ItemTemplate>
                                                                        <div style="width: 160px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("FILETYPE")%>' />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField SortExpression="FILEMODIFIED" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="false" HeaderStyle-Width="160px"  HeaderStyle-CssClass="GridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="GridAltItem">
                                                                    <ItemTemplate>
                                                                        <div style="width: 160px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("FILEMODIFIED","{0:" + DATE_FORMAT + "}")%>' />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                               </Columns>
                                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GridHeader" Height="20px" />
                                                        <EditRowStyle HorizontalAlign="Left" />
                                                        <EmptyDataRowStyle HorizontalAlign="Left" />
                                                        <FooterStyle Height="15px"></FooterStyle>
                                                        <RowStyle CssClass="GridRowStyle" HorizontalAlign="Left" Height="22px" />
                                                        <SelectedRowStyle CssClass="GridSelectedItem" />
                                                        <SortedAscendingHeaderStyle Font-Underline="false" />
                                                        <SortedDescendingHeaderStyle Font-Underline="false" />
                                                        <PagerStyle CssClass="GridPage" ForeColor="Black" HorizontalAlign="right" Height="20px" Font-Size="8"/>
                                                        </asp:GridView>
                                                         </td>
										</tr>
                                        
									</table>
                                    </div>
                               </td>
						    </tr>
                        </table>
					</td>
				</tr>--%>
                    </table>
                </td>
            </tr>
        </table>
        <input type="hidden" runat="server" id="hdnSelRow" />
        <input type="hidden" runat="server" id="hdnPkey" />
        <input type="hidden" runat="server" id="hdnSelRow1" />
        <input type="hidden" runat="server" id="hdnExport" />
    </form>
</body>
</html>




