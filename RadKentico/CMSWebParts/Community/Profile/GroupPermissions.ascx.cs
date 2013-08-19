using System;
using System.Web;
using System.Web.UI;

using CMS.CMSHelper;
using CMS.Community;
using CMS.GlobalHelper;
using CMS.PortalControls;
using CMS.UIControls;

public partial class CMSWebParts_Community_Profile_GroupPermissions : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets group name to specify group members.
    /// </summary>
    public string GroupName
    {
        get
        {
            string groupName = ValidationHelper.GetString(GetValue("GroupName"), "");
            if ((string.IsNullOrEmpty(groupName) || groupName == GroupInfoProvider.CURRENT_GROUP) && (CommunityContext.CurrentGroup != null))
            {
                return CommunityContext.CurrentGroup.GroupName;
            }
            return groupName;
        }
        set
        {
            SetValue("GroupName", value);
        }
    }


    /// <summary>
    /// Gets or sets message which should be displayed if user hasn't permissions.
    /// </summary>
    public string NoPermissionMessage
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("NoPermissionMessage"), messageElem.ErrorMessage), messageElem.ErrorMessage);
        }
        set
        {
            SetValue("NoPermissionMessage", value);
            messageElem.ErrorMessage = value;
        }
    }

    #endregion


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            groupPermissions.OnCheckPermissions += groupPermissions_OnCheckPermissions;

            GroupInfo gi = GroupInfoProvider.GetGroupInfo(GroupName, CMSContext.CurrentSiteName);
            if (gi != null)
            {
                groupPermissions.GroupID = gi.GroupID;
            }
            else
            {
                Visible = false;
            }
        }
    }


    /// <summary>
    /// Group permissions - check permissions.
    /// </summary>
    protected void groupPermissions_OnCheckPermissions(string permissionType, CMSAdminControl sender)
    {
        if (!(CMSContext.CurrentUser.IsGroupAdministrator(groupPermissions.GroupID) | CMSContext.CurrentUser.IsAuthorizedPerResource("cms.groups", "Manage")))
        {
            if (sender != null)
            {
                sender.StopProcessing = true;
            }
            groupPermissions.StopProcessing = true;
            groupPermissions.Visible = false;
            messageElem.ErrorMessage = NoPermissionMessage;
            messageElem.Visible = true;
        }
    }


    /// <summary>
    /// Reloads the control data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }
}