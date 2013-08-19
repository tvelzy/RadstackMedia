<%@ Page Language="C#" AutoEventWireup="true" Theme="Default"
    Inherits="CMSFormControls_Selectors_InsertImageOrMedia_Tabs_Anchor" EnableEventValidation="false" CodeFile="Tabs_Anchor.aspx.cs" %>

<%@ Register Src="~/CMSModules/Content/Controls/Dialogs/Properties/HTMLAnchorProperties.ascx"
    TagName="AnchorProperties" TagPrefix="cms" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server" enableviewstate="false">
    <title>Insert image or media - anchor</title>
    <style type="text/css">
        body
        {
            margin: 0px;
            padding: 0px;
            height: 100%;
        }
    </style>
</head>
<body class="<%=mBodyClass%>">
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="scrManager" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <cms:AnchorProperties ID="anchorProperties" runat="server" IsLiveSite="false" />
    <asp:Literal ID="ltlScript" runat="server" EnableViewState="false" />
    </form>
</body>
</html>
