﻿function ChatDialogsObj() {

    var that = this;

    this.GetDialogMask = function() {
        return {
            mask: {
                color: '#939393',
                loadSpeed: 200,
                opacity: 0.8
            },
            load: false
        };
    }


    this.SetDialogOverlay = function(id) {
        jQuery(id).overlay(that.GetDialogMask());
    }
    

    this.DisplayDialog = function(id) {
        var prompt = jQuery(id);
        prompt.overlay().load();
    }


    this.CloseDialog = function(id) {
        jQuery(id).overlay().close();
    }
    

    this.TryCloseDialog = function(arrClass) {
        if (arrClass == null) {
            return;
        }
        for (var i = 0; i < arrClass.length; i++) {
            var dialog = jQuery("." + arrClass[i]).filter(":visible");
            if (dialog == null) {
                return;
            }
            if ((dialog.length > 0) && (dialog.overlay != null)) {
                dialog.overlay().close();

            }
        }
    }


    this.HandleTextAreaMaxLength = function(obj, limit) {
        jQuery(obj).keyup(function() {
            var txt = jQuery(this);
            var val = txt.val();
            if (val.length > limit) {
                txt.val(val.substring(0, limit));
                txt.scrollTop(99999);
            }
        });
    }
}

if (!window.ChatDialogs) {
    ChatDialogs = new ChatDialogsObj();
}