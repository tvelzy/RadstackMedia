<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_BannerManagement_Controls_CategoryEdit" CodeFile="CategoryEdit.ascx.cs" %>

<cms:UIForm runat="server" ID="EditForm" ObjectType="cms.bannercategory" RedirectUrlAfterCreate="Category_Edit.aspx?categoryid={%EditedObject.ID%}&siteid={?siteid?}&saved=1" />

