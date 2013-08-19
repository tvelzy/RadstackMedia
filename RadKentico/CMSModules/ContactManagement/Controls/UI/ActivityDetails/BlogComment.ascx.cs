using System;

using CMS.GlobalHelper;
using CMS.OnlineMarketing;
using CMS.SettingsProvider;
using CMS.WebAnalytics;

public partial class CMSModules_ContactManagement_Controls_UI_ActivityDetails_BlogComment : ActivityDetail
{
    #region "Methods"

    public override bool LoadData(ActivityInfo ai)
    {
        if ((ai == null) || !ModuleEntry.IsModuleLoaded(ModuleEntry.BLOGS))
        {
            return false;
        }

        switch (ai.ActivityType)
        {
            case PredefinedActivityType.BLOG_COMMENT:
            case PredefinedActivityType.SUBSCRIPTION_BLOG_POST:
                break;
            default:
                return false;
        }

        // Link to blog post
        int nodeId = ai.ActivityNodeID;
        lblDocIDVal.Text = GetLinkForDocument(nodeId, ai.ActivityCulture);

        // Link to blog
        int blogDocumentID = ai.ActivityItemDetailID;
        if (blogDocumentID > 0)
        {
            plcBlogDocument.Visible = true;
            lblBlogVal.Text = GetLinkForDocument(blogDocumentID, ai.ActivityCulture);
        }

        if (ai.ActivityType == PredefinedActivityType.BLOG_COMMENT)
        {
            GeneralizedInfo iinfo = ModuleCommands.BlogsGetBlogCommentInfo(ai.ActivityItemID);
            if (iinfo != null)
            {
                plcComment.Visible = true;
                txtComment.Text = ValidationHelper.GetString(iinfo.GetValue("CommentText"), null);
            }
        }

        return true;
    }

    #endregion
}