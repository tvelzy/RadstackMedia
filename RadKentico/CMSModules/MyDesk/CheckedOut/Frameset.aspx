<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frameset.aspx.cs" Inherits="CMSModules_MyDesk_CheckedOut_Frameset" %>

<%@ Register TagPrefix="cms" TagName="TabsFrameset" Src="~/CMSAdminControls/UI/PageElements/TabsFrameset.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My desk - Checked out</title>
</head>
<cms:TabsFrameset ID="frameset" ShortID="fs" runat="server" ContentUrl="~/CMSPages/blank.aspx" />
</html>
