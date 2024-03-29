<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/LiveSite/Dialogs/ModalDialogPage.master"
    Theme="Default" Inherits="CMSModules_Groups_CMSPages_InviteToGroup" CodeFile="InviteToGroup.aspx.cs" %>

<%@ Register Src="~/CMSModules/Groups/Controls/GroupInvite.ascx" TagName="GroupInvite"
    TagPrefix="cms" %>
<asp:Content ID="cntContent" runat="server" ContentPlaceHolderID="plcContent">
    <div class="PageContent">
        <div class="CommunityInviteToGroup">
            <cms:GroupInvite ID="groupInviteElem" IsLiveSite="true" runat="server" DisplayButtons="false" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="cntFooter" runat="server" ContentPlaceHolderID="plcFooter">
    <div class="FloatRight">
        <cms:CMSButton runat="server" CssClass="SubmitButton" ID="btnInvite" EnableViewState="false" /><cms:LocalizedButton
            CssClass="SubmitButton" ID="btnCancel" OnClientClick="Close();" runat="server"
            ResourceString="General.cancel" EnableViewState="false" />
    </div>
</asp:Content>
