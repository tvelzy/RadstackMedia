<%@ Control Language="C#" AutoEventWireup="true" CodeFile="~/CMSWebParts/OnlineMarketing/Activities.ascx.cs" Inherits="CMSWebParts_OnlineMarketing_Activities" %>
<%@ Register Src="~/CMSModules/ContactManagement/Controls/UI/Activity/List.ascx" TagName="ActivityList" TagPrefix="cms" %>
<cms:LocalizedLabel runat="server" ID="lblDis" ResourceString="om.activity.disabled" EnableViewState="false" Visible="false" />
<cms:ActivityList ShowSelection="false" runat="server" ID="listElem" Visible="false" />
