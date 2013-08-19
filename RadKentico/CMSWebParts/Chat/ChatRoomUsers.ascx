<%@ Control Language="C#" AutoEventWireup="true" CodeFile="~/CMSWebParts/Chat/ChatRoomUsers.ascx.cs" Inherits="CMSWebParts_Chat_ChatRoomUsers" %>

<%@ Register Src="~/CMSWebParts/Chat/ChatOnlineUsers.ascx" TagName="ChatOnlineUsers" TagPrefix="cms" %>
<%@ Register Src="~/CMSWebParts/Chat/ChatSearchOnlineUsers.ascx" TagName="ChatSearchOnlineUsers" TagPrefix="cms" %>

<asp:Panel runat="server" ID="pnlChatRoomUsersWP" CssClass="ChatWebpartContainer ChatWebpartContainerRoomUsers">

<%-- Panel with the list of users in the room --%>
<div class="ChatWebpartFilterPaging">
    <asp:Panel ID="pnlChatRoomUsersFiltering" runat="server" Visible="false">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:TextBox ID="txtChatRoomUsersFilter" runat="server" Width="100" EnableViewState="false" CssClass="ChatFilterTextbox"></asp:TextBox>
                </td>
                <td>
                    <cms:LocalizedButton ID="btnChatRoomUsersFilter" runat="server" ResourceString="chat.roomusers.filterbtn" CssClass="ContentButton"></cms:LocalizedButton>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlChaRoomUsersInfo" runat="server"></asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pnlChatRoomUsersPaging" runat="server"></asp:Panel>
</div>
<asp:Panel ID="pnlChatRoomUsers" runat="server"></asp:Panel>

<%-- Panel for inviting users --%>
<asp:Panel runat="server" ID="pnlChatRoomUsersInvite">
    <cms:LocalizedButton runat="server" ID="btnChatRoomUsersInvite" CssClass="SubmitButton ChatSubmitButton" EnableViewState="false" ResourceString="chat.invite" />
</asp:Panel>

<%-- Prompt for inviting users to room --%>
<asp:Panel runat="server" ID="pnlChatRoomUsersInvitePrompt" CssClass="ChatDialogBody ChatInviteDialogBody Hidden">
    <div class="ChatDialogHeader">
        <table>
            <tr>
                <td><asp:Image ID="imgChatRoomUsersInvitePrompt" runat="server" /></td>
                <td><cms:LocalizedLabel runat="server" ID="lblChatRoomUsersInvitePromptHeader" SetWindowTitle="false" ResourceString="chat.invite" /></td>
            </tr>
        </table>      
    </div>
    <div class="ChatDialogContent ChatDeleteDialogContent">
        <cms:ChatOnlineUsers runat="server" ID="ChatOnlineUsersElem" />
        <cms:ChatSearchOnlineUsers runat="server" ID="ChatSearchOnlineUsers"/>
    </div>
    <div class="ChatDialogFooter">
        <div class="ChatDialogFooterButtons">
            <cms:LocalizedButton runat="server" ID="btnChatRoomsDeletePromptClose" UseSubmitBehavior="true"
                CssClass="SubmitButton" ResourceString="general.close" />
        </div>
    </div>
</asp:Panel>

</asp:Panel>

