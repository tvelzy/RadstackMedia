using System;

using CMS.UIControls;

public partial class CMSModules_Messaging_MyMessages_MyMessages_IgnoreList : CMSMyMessagesPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }


    protected void CheckPermissions(string permissionType, CMSAdminControl sender)
    {
        // Do not check permissions since user can always manage her messages
    }
}