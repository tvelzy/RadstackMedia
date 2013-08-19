using System;
using System.Text;
using System.Web.UI;

using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.PortalControls;
using CMS.DocumentEngine;

public partial class CMSWebParts_CommunityServices_Facebook_FacebookLikeButton : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Url to like.
    /// </summary>
    public string Url
    {
        get
        {
            return ValidationHelper.GetString(GetValue("URL"), string.Empty);
        }
        set
        {
            SetValue("URL", value);
        }
    }


    /// <summary>
    /// Like button layout style.
    /// </summary>
    public string LayoutStyle
    {
        get
        {
            return ValidationHelper.GetString(GetValue("LayoutStyle"), "standard");
        }
        set
        {
            SetValue("LayoutStyle", value);
        }
    }


    /// <summary>
    /// Indicates whether to show faces or not.
    /// </summary>
    public bool ShowFaces
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowFaces"), true);
        }
        set
        {
            SetValue("ShowFaces", value);
        }
    }


    /// <summary>
    /// Indicates whether to include the Send button or not.
    /// </summary>
    public bool IncludeSendButton
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("IncludeSendButton"), true);
        }
        set
        {
            SetValue("IncludeSendButton", value);
        }
    }


    /// <summary>
    /// Width of the element.
    /// </summary>
    public int Width
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("Width"), 450);
        }
        set
        {
            SetValue("Width", value);
        }
    }


    /// <summary>
    /// Verb which will show up on the button.
    /// </summary>
    public string VerbToDisplay
    {
        get
        {
            return ValidationHelper.GetString(GetValue("VerbToDisplay"), "like");
        }
        set
        {
            SetValue("VerbToDisplay", value);
        }
    }


    /// <summary>
    /// Font of the text in iframe.
    /// </summary>
    public string Font
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Font"), "arial");
        }
        set
        {
            SetValue("Font", value);
        }
    }


    /// <summary>
    /// Color scheme of the button and text.
    /// </summary>
    public string ColorScheme
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ColorScheme"), "light");
        }
        set
        {
            SetValue("ColorScheme", value);
        }
    }


    /// <summary>
    /// Indicates whether to generate meta data tags or not.
    /// </summary>
    public bool GenerateMetaData
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("GenerateMetaData"), false);
        }
        set
        {
            SetValue("GenerateMetaData", value);
        }
    }


    /// <summary>
    /// Meta data title.
    /// </summary>
    public string MetaTitle
    {
        get
        {
            return ValidationHelper.GetString(GetValue("MetaTitle"), "");
        }
        set
        {
            SetValue("MetaTitle", value);
        }
    }


    /// <summary>
    /// Meta data type.
    /// </summary>
    public string MetaType
    {
        get
        {
            return ValidationHelper.GetString(GetValue("MetaType"), "");
        }
        set
        {
            SetValue("MetaType", value);
        }
    }


    /// <summary>
    /// Meta data URL.
    /// </summary>
    public string MetaUrl
    {
        get
        {
            return ValidationHelper.GetString(GetValue("MetaUrl"), "");
        }
        set
        {
            SetValue("MetaUrl", value);
        }
    }


    /// <summary>
    /// Meta data site name.
    /// </summary>
    public string MetaSiteName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("MetaSiteName"), "");
        }
        set
        {
            SetValue("MetaSiteName", value);
        }
    }


    /// <summary>
    /// Meta data image URL.
    /// </summary>
    public string MetaImage
    {
        get
        {
            return ValidationHelper.GetString(GetValue("MetaImage"), "");
        }
        set
        {
            SetValue("MetaImage", value);
        }
    }


    /// <summary>
    /// Indicates if HTML 5 output should be generated.
    /// </summary>
    public bool UseHTML5
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseHTML5"), false);
        }
        set
        {
            SetValue("UseHTML5", value);
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// Loads the web part content.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Sets up the control.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            // If paramater URL is empty, set URL of current document
            string url = Url;
            if (string.IsNullOrEmpty(url) && (CMSContext.CurrentDocument != null))
            {
                TreeNode node = CMSContext.CurrentDocument;
                url = CMSContext.GetUrl(node.NodeAliasPath, node.DocumentUrlPath, CMSContext.CurrentSiteName);
            }
            else
            {
                url = ResolveUrl(url);
            }
            url = URLHelper.GetAbsoluteUrl(url);

            // Register Facebook javascript SDK
            ScriptHelper.RegisterFacebookJavascriptSDK(Page, CMSContext.PreferredCultureCode);

            // Get FB code
            StringBuilder sb = new StringBuilder();
            if (UseHTML5)
            {
                sb.Append("<div class=\"fb-like\" data-href=\"", url, "\" data-width=\"", Width,
                          "\" data-send=\"", IncludeSendButton, "\" data-layout=\"", LayoutStyle,
                          "\" data-show-faces=\"", ShowFaces, "\" data-action=\"", VerbToDisplay,
                          "\" data-colorscheme=\"", ColorScheme, "\"");

                if (!string.IsNullOrEmpty(Font))
                {
                    sb.Append(" data-font=\"", Font, "\"");
                }
                sb.Append("></div>");
            }
            else
            {
                sb.Append("<fb:like href=\"", url, "\" layout=\"", LayoutStyle, "\" send=\"",
                          IncludeSendButton ? "true" : "false", "\" show_faces=\"", ShowFaces ? "true" : "false",
                          "\" width=\"", Width, "\" action=\"", VerbToDisplay, "\" font=\"",
                          Font, "\" colorscheme=\"", ColorScheme, "\"></fb:like>");
            }
            ltlLikeButtonCode.Text = sb.ToString();
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            // Generate meta tags
            if (GenerateMetaData)
            {
                StringBuilder sb = new StringBuilder();

                if (MetaTitle != "")
                {
                    sb.AppendLine("<meta property=\"og:title\" content=\"" + MetaTitle + "\" />");
                }
                if (MetaType != "")
                {
                    sb.AppendLine("<meta property=\"og:type\" content=\"" + MetaType + "\" />");
                }
                if (MetaSiteName != "")
                {
                    sb.AppendLine("<meta property=\"og:site_name\" content=\"" + MetaSiteName + "\" />");
                }
                if (MetaImage != "")
                {
                    sb.AppendLine("<meta property=\"og:image\" content=\"" + MetaImage + "\" />");
                }
                if (MetaUrl != "")
                {
                    sb.AppendLine("<meta property=\"og:url\" content=\"" + MetaUrl + "\" />");
                }

                Page.Header.Controls.Add(new LiteralControl(sb.ToString()));
            }
        }
    }

    #endregion
}