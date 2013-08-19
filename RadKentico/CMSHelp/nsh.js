/*! 
--- General topic page management functions ---
--- For Premium Pack Version 1.51 for Help & Manual 6 --- 
--- (c) 2008-2011 by Tim Green --- 
--- All Rights Reserved --- 
*/
function addCss(b) {
    var a = document.createElement("style");
    a.type = "text/css";
    if (a.styleSheet) {
        a.styleSheet.cssText = b;
    } else {
        a.appendChild(document.createTextNode(b));
    }
    document.getElementsByTagName("head")[0].appendChild(a);
}
addCss("body,html {overflow: hidden;}#idcontent {position: absolute; left: 0; right: 0; bottom: 0; overflow-x: auto; overflow-y: scroll;}#noScriptNavHead {display: none;}");

function addEvent(e, d, b, a) {
    if (e.addEventListener) {
        e.addEventListener(d, b, a);
        return true;
    } else {
        if (e.attachEvent) {
            var c = e.attachEvent("on" + d, b);
            return c;
        } else {
            alert("Could not add event!");
        }
    }
}
function trim(a) {
    return a.replace(/^\s+|\s+$/g, "");
}
function isFlash() {
    var c = false;
    if (navigator.platform != "iPad" && navigator.platform != "iPhone") {
        if (window.ActiveXObject) {
            try {
                control = new ActiveXObject("ShockwaveFlash.ShockwaveFlash");
            } catch (b) {
                return;
            }
            if (control) {
                c = true;
            }
        } else {
            if (navigator.plugins && navigator.plugins.length) {
                c = navigator.plugins["Shockwave Flash 2.0"] || navigator.plugins["Shockwave Flash"] ? true : false;
            } else {
                if (navigator.mimeTypes && navigator.mimeTypes.length) {
                    var a = navigator.mimeTypes["application/x-shockwave-flash"];
                    c = a && a.enabledPlugin;
                }
            }
        }
    }
    return c;
}
function doResize() {
    $("#idcontent").css("top", $("#idheader").outerHeight() + "px");
}
function nsrInit() {
    if (self.innerHeight) {
        document.getElementById("scriptNavHead").style.display = "table-row";
    } else {
        document.getElementById("scriptNavHead").style.display = "block";
    }
}
function getLink(a) {
    var b = "";
    b = document.location.href.replace(/\#.*$/, "");
    b = b.replace(/\?.*?$/, "");
    b = b.replace(/\/(?!.*?\/)/, "/" + a + "?");
    return b;
}
function doPermalink(b, f, a, c) {
    var e = "";
    if (f) {
        e = getLink(b);
    } else {
        e = a;
    }
    if (c === "show") {
        var d = $("#innerdiv").width() - 55;
        if (d > 500) {
            d = 500;
        } else {
            if (d < 420) {
                d = 420;
            }
        }
        $("#plinkBox").text(e).css({
            height: "33px",
            "min-height": "33px",
            width: d + "px",
            "min-width": d + "px",
            "max-width": d + "px"
        });
        $("div#permalink").css({
            width: ($("#plinkBox").outerWidth() + 20) + "px",
            visibility: "visible"
        });
    } else {
        if (c === "standard") {
            return e;
        }
    }
}
function PLclick(c, b) {
    switch (c) {
    case "show":
        doPermalink(b[0], b[1], b[2], c);
        break;
    case "select":
        $("#plinkBox").select();
        break;
    case "close":
        $("#permalink").css("visibility", "hidden");
        break;
    case "bookmark":
        var d = b[3];
        var a = getLink(b[0]);
        if (/^https??:\/\//im.test(a)) {
            if (window.sidebar) {
                window.sidebar.addPanel(d, a, "");
            } else {
                if (document.all) {
                    window.external.AddFavorite(a, d);
                } else {
                    alert(b[4]);
                }
            }
        } else {
            alert(b[5]);
        }
        break;
    case "standard":
        return doPermalink(b[0], b[1], b[2], c);
        break;
    }
    return false;
}
function writePermalink(f, e, a, d, c) {
    document.write('<p class="help-url"><b>');
    if (f && /^https??:\/\//im.test(document.location)) {
        document.write('<a href="javascript:void(0);" onclick="PLclick(\'show\',window.permalinkData);" ');
        document.write('title="' + e + '">' + a + "</a></b>\n");
    } else {
        var b = PLclick("standard", window.permalinkData);
        document.write(c + '&nbsp;</b><a href="' + b + '" target="_top" title="' + d + '">');
        document.write(b + "</a>\n");
    }
}
function mailFB(h) {
    var f = unQuot(getMailParams("mailsubject"));
    var a = unQuot(getMailParams("mailpath"));
    var e = unQuot(getMailParams("mailbody"));
    var d = unQuot(getMailParams("mailid"));
    if (!h) {
        var c = "mailto:" + escape(window.recipient) + "?subject=" + f;
        var b = "&body=Ref:%20" + a + "%20ID:%20" + d + "%0A%0D%0A%0D" + e + "%0A%0D%0A%0D";
    } else {
        var c = "mailto:" + escape(window.recipient) + "?subject=" + window.subjectnojs;
        var b = "&body=Ref%20ID:%20" + d + "%0A%0D";
    }
    var g = c + b;
    document.write('<a href="' + g + '" target="_blank"');
    document.write("onmouseover=\"document.images.feedback.src='mail_h.gif'\"");
    document.write('onmouseout="document.images.feedback.src=\'mail.gif\'"><img name="feedback" src="mail.gif" border="0" alt="' + window.mailtip + '" title="' + window.mailtip + '" /></a>');
}
function getMailParams(a) {
    var b = document.getElementById(a).innerHTML;
    return b;
}
function unQuot(a) {
    a = a.replace(/&gt;/g, ">");
    a = a.replace(/&lt;/g, "<");
    a = a.replace(/&quot;/g, '"');
    a = a.replace(/&amp;/g, "&");
    a = escape(a);
    a = a.replace(/%E2|%E0|%E5|%E1|%E3/g, "a");
    a = a.replace(/%C5|%C0|%C1|%C2|%C3/g, "A");
    a = a.replace(/%C7/g, "C");
    a = a.replace(/%E7/g, "c");
    a = a.replace(/%E9|%EA|%EB|%E8/g, "e");
    a = a.replace(/%C9|%CA|%C8|%CB/g, "E");
    a = a.replace(/%u0192/g, "f");
    a = a.replace(/%EF|%EE|%EC|%ED/g, "i");
    a = a.replace(/%CF|%CD|%CE|%CC/g, "I");
    a = a.replace(/%F1/g, "n");
    a = a.replace(/%D1/g, "N");
    a = a.replace(/%F4|%F2|%F3|%F5|%F8/g, "o");
    a = a.replace(/%D4|%D2|%D3|%D5|%D8/g, "O");
    a = a.replace(/%u0161/g, "s");
    a = a.replace(/%u0160/g, "S");
    a = a.replace(/%FB|%FA|%F9/g, "u");
    a = a.replace(/%DB|%DA|%D9/g, "U");
    a = a.replace(/%FF|%FD/g, "y");
    a = a.replace(/%DD|%u0178/g, "Y");
    a = a.replace(/%FC/g, "ue");
    a = a.replace(/%DC/g, "Ue");
    a = a.replace(/%E4|%E6/g, "ae");
    a = a.replace(/%C4|%C6/g, "Ae");
    a = a.replace(/%F6|%u0153/g, "oe");
    a = a.replace(/%D6/g, "Oe");
    a = a.replace(/%DF/g, "ss");
    return (a);
}
function SearchCheck() {
    var c = window.location.search.lastIndexOf("zoom_highlight") > 0;
    if (!c) {
        var a = document.getElementsByTagName("FONT");
        if (a.length > 0) {
            var b = "";
            for (var d = 0; d < a.length; d++) {
                b = a[d].style.cssText;
                if (b.indexOf("BACKGROUND-COLOR") == 0) {
                    c = true;
                    break;
                }
            }
        }
    }
    return c;
}
function toggleToggles() {
    if (HMToggles.length != null) {
        var a = true;
        for (var b = 0; b < HMToggles.length; b++) {
            if (HMToggles[b].getAttribute("hm.state") == "1") {
                a = false;
                break;
            }
        }
        HMToggleExpandAll(a);
    }
}
function toggleCheck(e) {
    var a = $(e).parents("div[id^='TOGGLE'][hm.state='0']");
    if (a.length > 0) {
        for (var b = a.length - 1; b > -1; b--) {
            var d = $(a[b]).attr("id");
            var c = d + "_ICON";
            if ($("img[id='" + c + "']").length > 0) {
                HMToggle("expand", d, c);
            } else {
                HMToggle("expand", d);
            }
        }
        return a.length * 200;
    } else {
        return 0;
    }
}
function openTargetToggle(e, b) {
    var a;
    var d = false;
    if (b == "menu") {
        a = $(e[0]).parent("span:has(a.dropdown-toggle)").find("a.dropdown-toggle").attr("href");
        if (!a) {
            a = $(e[0]).parent("p:has(a.dropdown-toggle)").find("a.dropdown-toggle").attr("href");
        }
    } else {
        a = $(e[0]).parent("p:has(a.dropdown-toggle)");
        if (a) {
            a = $(a).find("a.dropdown-toggle").attr("href");
        }
        if (!a) {
            a = $(e[0]).siblings("img[id^='TOGGLE']");
            if (a.length > 0) {
                a = $(a).parents("div")[0];
            }
            if (a) {
                a = $(a).find("a.dropdown-toggle").attr("href");
            }
        }
    }
    var f = false;
    var c = "";
    if (a) {
        if (a.indexOf("ICON") != -1) {
            f = true;
        }
        a = a.replace(/^.*?,'/, "");
        a = a.replace(/'.*/, "");
        if (f) {
            c = a + "_ICON";
        }
        if (!f) {
            HMToggle("expand", a);
            return true;
        } else {
            HMToggle("expand", a, c);
            return true;
        }
    } else {
        return false;
    }
}
var intLoc = location.hash;

function pollLocation(a) {
    var b = a;
    if (b.length > 0 && intLoc != b) {
        intLoc = b;
        toggleJump();
    }
}
function aniScroll(a, c) {
    if (hmAnimate) {
        hmAnimate = false;
        var b = true;
    }
    var d = toggleCheck(a[0]);
    if (d < 200) {
        d = 200;
    }
    openTargetToggle(a, c);
    setTimeout(function (e) {
        $("#idcontent").scrollTo(0, 0);
        $("#idcontent").scrollTo($(a), 600, {
            offset: -12,
            axis: "y",
            onAfter: function () {
                if (b) {
                    hmAnimate = true;
                }
            }
        });
    }, d);
}
function toggleJump() {
    var d = "";
    try {
        if (window.frameElement && parent.location.hash) {
            d = parent.location.hash.toString();
        }
        if (location.hash) {
            d = location.hash.toString();
        }
    } catch (a) {}
    if (d.length > 1) {
        var c = d.replace(/\#/, "");
        var b;
        if ($("a[id='" + c + "']").length > 0) {
            b = $("a[id='" + c + "']");
        } else {
            if ($("a[name='" + c + "']").length > 0) {
                b = $("a[name='" + c + "']");
            } else {
                return false;
            }
        }
        aniScroll(b, "page");
        return false;
    }
}
function printTopic(a) {
    window.open(a, "", "toolbar=1,scrollbars=1,location=0,status=1,menubar=1,titlebar=1,resizable=1");
}
$(document).ready(function () {
    var b = /toc=0&printWindow/g;
    if (b.test(location.search)) {
        $("body").hide();
        setTimeout(function () {
            HMToggleExpandAll(true);
            $("body").css({
                "overflow.x": "auto",
                "overflow-y": "scroll"
            });
            $("#idcontent").css("overflow", "hidden");
            $("#idnav,.idnav,.idnav a,#breadcrumbs,#autoTocWrapper,#idheader,#permalink,.popups,.help-url").css("display", "none");
            $('<style media="screen">#printheader {display: block; font-weight: bold; padding: 11px 0px 0px 11px;}</style>').appendTo("head");
            $("#printtitle").css("fontWeight", "bold");
            $("#idcontent,#innerdiv").css({
                margin: "0",
                position: "static",
                padding: "0"
            });
            $("a").attr({
                href: "#",
                onclick: "javascript:void(0);"
            });
            $("body").show();
            setTimeout(function () {
                print();
            }, 700);
        }, 150);
    }
    if (location.href.search("::") > 0 && $("a[name]").length > 0 && $("a.dropdown-toggle").length > 0) {
        setInterval(function () {
            pollLocation(location.hash);
        }, 300);
    }
    var a = /msie 6|MSIE 6/.test(navigator.userAgent);
    var c = document.location.pathname;
    c = c.replace(/^.*[/\\]|[?#&].*$/, "");
    $("a[href^='" + c + "#'],a[href^='#']:not(a[href='#']),area[href^='" + c + "#'],area[href^='#']:not(area[href='#'])").click(function () {
        var d = $(this).attr("href").replace(/.*?\#/, "");
        var e = $("a[id='" + d + "']");
        if (!e.length > 0) {
            e = $("a[name='" + d + "']");
        }
        aniScroll(e, "page");
        return false;
    });
    $(window).bind("resize", function () {
        doResize();
    });
    /*if (!$.browser.msie) {
        $(window).bind("load", function () {
            toggleJump();
        });
    }*/
    nsrInit();
    doResize();
});

function togTOCinit(d, b, a, c) {
    var f = (/msie 6|MSIE 6/.test(navigator.userAgent));
    if ((location.search.indexOf("toc=0") > -1) || (navigator.platform.indexOf("iPhone") > -1) || (navigator.platform.indexOf("iPad") > -1) || c || f) {
        if ($("img#togtoc").length > 0) {
            $("img#togtoc").css("display", "none");
        }
        if (location.search.indexOf("toc=0") > -1) {
            return;
        }
    }
    var e = frameWidth();
    if (!c) {
        if (e == "0%,100%") {
            $("img#togtoc").attr("src", "togtoc_show.gif").attr("alt", b).attr("title", b);
        } else {
            $("img#togtoc").attr("src", "togtoc_hide.gif").attr("alt", a).attr("title", a);
        }
        $("img#togtoc").mouseenter(function () {
            $(this).animate({
                marginLeft: -12
            }, 100);
        });
        $("img#togtoc").mouseleave(function () {
            $(this).animate({
                marginLeft: -23
            }, 100);
        });
        $("img#togtoc").click(function () {
            togTOC(d);
            var g = frameWidth();
            if (g == "0%,100%") {
                $("img#togtoc").attr("src", "togtoc_show.gif").attr("alt", b).attr("title", b);
            } else {
                $("img#togtoc").attr("src", "togtoc_hide.gif").attr("alt", a).attr("title", a);
            }
        });
    } else {
        if (!f) {
            $("td#togtocheader").css("padding-left", "5px");
            $("td#togbuttoncell").show();
            $("img#togtocbutton").css("margin-left", "5px").mouseover(function () {
                $(this).attr("src", "toc_h.gif").css("cursor", "pointer");
            }).mouseout(function () {
                $(this).attr("src", "toc.gif");
            }).click(function () {
                togTOC(d);
            });
        }
    }
}
function readCookie(b) {
    var e = b + "=";
    var a = document.cookie.split(";");
    for (var d = 0; d < a.length; d++) {
        var f = a[d];
        while (f.charAt(0) == " ") {
            f = f.substring(1, f.length);
        }
        if (f.indexOf(e) == 0) {
            return f.substring(e.length, f.length);
        }
    }
    return null;
}
function frameWidth() {
    var a = $("#hmframeset", window.parent.document).attr("cols");
    var c = a.split(",");
    if (c[0].indexOf("%") == -1) {
        var b = parseInt(c[0]);
        if (b > 1000) {
            b = b / 100;
        }
        a = b + "," + c[1];
    }
    return a;
}
function togTOC(c) {
    var b = frameWidth();
    var a = /Opera|opera/.test(navigator.userAgent) ? "5" : "2";
    if (readCookie("colWidth")) {
        var d = readCookie("colWidth");
    } else {
        var d = "0";
    }
    if (b.substring(0, 2) == "0%") {
        if (d != "0") {
            $("#hmframeset", window.parent.document).attr("cols", d);
        } else {
            $("#hmframeset", window.parent.document).attr("cols", c + ",*");
        }
        $("#hmframeset", window.parent.document).attr("frameBorder", "1").attr("frameSpacing", a).attr("border", "6");
    } else {
        document.cookie = "colWidth=" + b;
        $("#hmframeset", window.parent.document).attr("cols", "0%,100%");
        $("#hmframeset", window.parent.document).attr("frameBorder", "0").attr("frameSpacing", "0").attr("border", "0");
    }
}
function chromeBugCheck(a) {
    if ((a == "block") && /WebKit|webkit/.test(navigator.userAgent) && $("#togtoc").attr("alt")) {
        $("#togtoc").css("display", "none");
    }
    var b = "Google Chrome";
    if ((location.href.indexOf("http:") < 0) && (location.href.indexOf("https:") < 0)) {
        if ((!$("#hmframeset", window.parent.document).attr("cols")) && (location.search.indexOf("toc=0") < 0)) {
            if (!/Chrome|chrome/.test(navigator.userAgent)) {
                b = "your web browser";
            }
            alert("HELP SYSTEM ALERT:\n\nThis version of " + b + " blocks cross-frame\nreferences in a way that is not compatible with HTML\nstandards. This feature makes " + b + " unable\nto display this WebHelp help system correctly when\nloaded directly from a local computer drive without\na web server.\n\nPlease use a different browser to view this help from\nthis location.\n\nThis version of " + b + " will display the help\ncorrectly when the help is stored on a web server.");
        }
    }
}