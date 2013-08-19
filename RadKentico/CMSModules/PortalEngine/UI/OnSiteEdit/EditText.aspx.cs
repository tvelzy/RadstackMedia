﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.UIControls;
using CMS.GlobalHelper;
using CMS.SettingsProvider;
using CMS.Controls;
using CMS.ExtendedControls;
using CMS.CMSHelper;

public partial class CMSModules_PortalEngine_UI_OnSiteEdit_EditText : CMSAbstractEditablePage
{
    protected override void OnInit(EventArgs e)
    {
        DocumentManager.OnValidateData += new EventHandler<DocumentManagerEventArgs>(DocumentManager_OnValidateData);
        DocumentManager.OnAfterAction += new EventHandler<DocumentManagerEventArgs>(DocumentManager_OnAfterAction);

        // Set the region type manually for ASPX mode
        if (QueryHelper.Contains("regiontype"))
        {
            ucEditableText.RegionType = CMSEditableRegionTypeEnumFunctions.GetRegionTypeEnum(QueryHelper.GetString("regiontype", string.Empty));
            if (ucEditableText.RegionType == CMSEditableRegionTypeEnum.HtmlEditor)
            {
                // HtmlEditor needs toolbar location defined (due to toolbar positioning and editing area padding)
                ucEditableText.HtmlAreaToolbarLocation = "Out:CKToolbar";
            }
        }

        ucEditableText.ViewMode = CheckPermissions();
        ucEditableText.DataControl = CurrentWebPartInstance as ISimpleDataContainer;
        ucEditableText.CurrentPageInfo = CurrentPageInfo;
        ucEditableText.IsDialogEdit = true;
        ucEditableText.SetupControl();

        string title = GetString("Content.EditTextTitle");
        if (!String.IsNullOrEmpty(PageTitleSuffix))
        {
            title += " - " + HTMLHelper.HTMLEncode(PageTitleSuffix);
        }
        SetTitle("CMSModules/CMS_PortalEngine/OnSiteEdit/EditText.png", title, null, null);

        base.OnInit(e);

        CSSHelper.RegisterCSSLink(Page, "Design", "OnSiteEdit.css");
        ScriptHelper.RegisterJQuery(this.Page);

        menuElem.ShowSaveAndClose = true;

        if (ucEditableText.RegionType == CMSEditableRegionTypeEnum.TextArea)
        {
            string resizeScript = @"
            var resizeTextAreaTimer;

            // DOM ready
            jQuery(document).ready( function() { ResizeEditableArea(200); });

            // Window resize
            jQuery(window).resize(function () { ResizeEditableArea(100); });

            function ResizeEditableArea(timeout) {
                clearTimeout(resizeTextAreaTimer);
                resizeTextAreaTimer = window.setTimeout(function () {
                    var textarea = jQuery('.EditableTextTextBox');
                    var editableTextContainer = jQuery('.EditableTextContainer');
                    var editableTextEdit = jQuery('.EditableTextEdit');
                    var borderMargin1 = textarea.outerHeight(true) - textarea.height();
                    var borderMargin2 = editableTextEdit.outerHeight(true) - editableTextEdit.height();
                    var borderMargin3 = editableTextContainer.outerHeight(true) - editableTextContainer.height();
                    var height = jQuery('.ModalDialogContent').height() - borderMargin1 - borderMargin2 - borderMargin3;
                    textarea.height(height);
            }, timeout); }";

            ScriptHelper.RegisterClientScriptBlock(this, typeof(Page), "ResizeEditableArea", ScriptHelper.GetScript(resizeScript));
        }

        CurrentDeviceInfo device = CMSContext.CurrentDevice;

        // Resize of HTML area is handled from editor itself
        if ((ucEditableText.RegionType == CMSEditableRegionTypeEnum.HtmlEditor) && !device.IsMobile)
        {
            // Hide editor area before its fully loaded
            pnlEditor.Style.Add("visibility", "hidden");
        }

        if (device.IsMobile)
        {
            // Do not use fixed positioning for mobile devices
            (CurrentMaster.HeaderContainer as CMSPanel).FixedPosition = false;
        }


    }


    /// <summary>
    /// Load content
    /// </summary>
    public override void LoadContent(string content, bool forceReload)
    {
        ucEditableText.LoadContent(content, true);
    }


    /// <summary>
    /// Get content
    /// </summary>
    public override string GetContent()
    {
        // Return editable content when valid
        if (ucEditableText.IsValid())
        {
            return ucEditableText.GetContent();
        }
        else
        {
            ShowError(ucEditableText.ErrorMessage);
        }

        return null;
    }


    /// <summary>
    /// Handles the OnValidateData event of the DocumentManager control.
    /// </summary>
    protected void DocumentManager_OnValidateData(object sender, DocumentManagerEventArgs e)
    {
        if (!ucEditableText.IsValid())
        {
            // Set the error message when an error occurs
            e.IsValid = false;
            e.ErrorMessage = ucEditableText.ErrorMessage;
        }
    }


    /// <summary>
    /// Handles the OnAfterAction event of the DocumentManager control.
    /// </summary>
    protected void DocumentManager_OnAfterAction(object sender, DocumentManagerEventArgs e)
    {
        // Update the ViewMode in order to enable/disable the edit text control (used for workflow actions).
        ucEditableText.ViewMode = CheckPermissions();
    }
}
