<%@ Page Language="C#" AutoEventWireup="true" Theme="Default" Inherits="CMSModules_Reporting_Tools_Ecommerce_Report_Print"
    CodeFile="Report_Print.aspx.cs" %>

<%@ Register Src="~/CMSModules/Reporting/Controls/DisplayReport.ascx" TagName="DisplayReport"
    TagPrefix="cms" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" enableviewstate="false">
    <title>E-commerce - Report print</title>
    <base target="_self" />
    <style type="text/css">
        body
        {
            padding: 10px;
        }
    </style>
</head>
<body onload="window.print();" class="<%=mBodyClass%>">
    <form id="form1" runat="server">
    <asp:PlaceHolder runat="server" ID="pnlManager" />
    <asp:Panel runat="server" ID="pnlContent">
        <cms:DisplayReport ID="DisplayReport1" runat="server" FormCssClass="ReportFilter"
            IsLiveSite="false" />
    </asp:Panel>
    </form>
</body>
</html>
