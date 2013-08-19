using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.PortalControls;
using CMS.PortalEngine;
using CMS.WorkflowEngine;
using CMS.DocumentEngine;
using CMS.SettingsProvider;

using TreeNode = CMS.DocumentEngine.TreeNode;

public partial class CMSWebParts_General_pageplaceholder : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// Gets or sets the value than indicates whether placeholder checks access permissions for the editable web parts content.
    /// </summary>
    public bool CheckPermissions
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("CheckPermissions"), partPlaceholder.CheckPermissions);
        }
        set
        {
            SetValue("CheckPermissions", value);
            partPlaceholder.CheckPermissions = value;
        }
    }


    /// <summary>
    /// Gets or sets the default page template of the page place holder.
    /// </summary>
    public string PageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PageTemplate"), "");
        }
        set
        {
            SetValue("PageTemplate", value);
        }
    }


    /// <summary>
    /// If true, the default template is used also on subpages of the document, otherwise the default template is used only on current document while the child documents use standard inheritance rules.
    /// </summary>
    public bool UseDefaultTemplateOnSubPages
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseDefaultTemplateOnSubPages"), false);
        }
        set
        {
            SetValue("UseDefaultTemplateOnSubPages", value);
        }
    }


    /// <summary>
    /// Gets or sets the path of the document to display within this placeholder if the default page template is used.
    /// </summary>
    public string Path
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Path"), "");
        }
        set
        {
            SetValue("Path", value);
        }
    }


    /// <summary>
    /// Gets or sets the cache minutes.
    /// </summary>
    public override int CacheMinutes
    {
        get
        {
            return base.CacheMinutes;
        }
        set
        {
            base.CacheMinutes = value;
            partPlaceholder.CacheMinutes = value;
        }
    }

    #endregion


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();

        partPlaceholder.PagePlaceholderID = ID;

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
            partPlaceholder.CheckPermissions = CheckPermissions;
            partPlaceholder.CacheMinutes = CacheMinutes;

            // Load content only when default page template or path is defined
            string templateName = PageTemplate;
            string path = Path;

            if ((templateName != "") || (path != ""))
            {
                ViewModeEnum viewMode = ViewModeEnum.Unknown;

                // Process template only if the control is on the last hierarchy page
                PageInfo currentPage = PagePlaceholder.PageInfo;
                PageInfo usePage = null;
                PageTemplateInfo ti = null;

                if (String.IsNullOrEmpty(path))
                {
                    // Use the same page
                    usePage = PagePlaceholder.PageInfo;

                    if (UseDefaultTemplateOnSubPages || (currentPage.ChildPageInfo == null) || (currentPage.ChildPageInfo.UsedPageTemplateInfo == null) || (currentPage.ChildPageInfo.UsedPageTemplateInfo.PageTemplateId == 0))
                    {
                        ti = PageTemplateInfoProvider.GetPageTemplateInfo(templateName);
                    }
                }
                else
                {
                    // Resolve the path first
                    path = CMSContext.ResolveCurrentPath(path);

                    // Get specific page
                    usePage = PageInfoProvider.GetPageInfo(CMSContext.CurrentSiteName, path, CMSContext.PreferredCultureCode, null, false);
                    if (PortalManager.ViewMode != ViewModeEnum.LiveSite)
                    {
                        viewMode = ViewModeEnum.Preview;

                        // Set design mode for document's placeholder if is currently displayed
                        TreeNode tn = CMSContext.CurrentDocument;
                        if ((tn != null) && (CMSContext.ViewMode == ViewModeEnum.Design) && tn.NodeAliasPath.EqualsCSafe(path, true))
                        {
                            viewMode = ViewModeEnum.Design;
                        }

                        // Get current document content
                        if (usePage != null)
                        {
                            TreeNode node = DocumentHelper.GetDocument(usePage.DocumentID, null);
                            if (node != null)
                            {
                                usePage.LoadVersion(node);
                            }
                        }
                    }

                    // Get the appropriate page template
                    if (String.IsNullOrEmpty(templateName))
                    {
                        ti = usePage.UsedPageTemplateInfo;
                    }
                    else
                    {
                        ti = PageTemplateInfoProvider.GetPageTemplateInfo(templateName);
                    }
                }

                if ((usePage != null) && (ti != null))
                {
                    // If same template as current page, avoid cycling
                    if (ti.PageTemplateId == currentPage.UsedPageTemplateInfo.PageTemplateId)
                    {
                        lblError.Text = GetString("WebPart.PagePlaceHolder.CurrentTemplateNotAllowed");
                        lblError.Visible = true;
                    }
                    else
                    {
                        usePage = usePage.Clone();

                        // Setup the page template
                        int templateId = ti.PageTemplateId;

                        usePage.SetPageTemplateId(templateId);
                        usePage.UsedPageTemplateInfo = ti;
                        
                        // Load the current page info with the template and document
                        if (viewMode != ViewModeEnum.Unknown)
                        {
                            partPlaceholder.ViewMode = viewMode;
                        }

                        partPlaceholder.UsingDefaultPageTemplate = !string.IsNullOrEmpty(templateName);
                        partPlaceholder.UsingDefaultDocument = !string.IsNullOrEmpty(path);
                        partPlaceholder.PageLevel = PagePlaceholder.PageLevel;
                        partPlaceholder.LoadContent(usePage, true);
                    }
                }
            }
        }
    }
}