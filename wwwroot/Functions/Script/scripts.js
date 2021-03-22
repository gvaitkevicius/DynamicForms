/// <reference path="perfect-scrollbar/perfect-scrollbar.js" />
/**
 * @Package: Complete Admin Responsive Theme
 * @Since: Complete Admin 1.0
 * This file is part of Complete Admin Responsive Theme.
 */


$j1112(function ($) {

    'use strict';

    var CMPLTADMIN_SETTINGS = window.CMPLTADMIN_SETTINGS || {};

    CMPLTADMIN_SETTINGS.disparEventoResize = function () {
        setTimeout(function () {
            $j1112(document).trigger('pageResize');
        }, 500);
    }
    CMPLTADMIN_SETTINGS.triggerEventToggleMenu = function () {
        setTimeout(function () {
            $j1112(document).trigger('clickMenuLateral');
        }, 200);
        setTimeout(function () {
            $j1112(document).trigger('clickMenuLateral');
        }, 300);
        setTimeout(function () {
            $j1112(document).trigger('clickMenuLateral');
        }, 500);
    }
    CMPLTADMIN_SETTINGS.allTransitionEnd = function (element) {
        element.on('transitionend webkitTransitionEnd oTransitionEnd', function () {
            $j1112(document).trigger('allTransitionEnd');
        });
    }
    /*--------------------------------
         Window Based Layout
     --------------------------------*/
    CMPLTADMIN_SETTINGS.windowBasedLayout = function () {
        var width = window.innerWidth;
        //console.log(width);

        if ($j1112("body").hasClass("chat-open") || $j1112("body").hasClass("sidebar-collapse")) {

            CMPLTADMIN_SETTINGS.mainmenuCollapsed();

        } else if (width < 767) {

            // small window
            $j1112(".page-topbar").addClass("sidebar_shift").removeClass("chat_shift");
            $j1112(".page-sidebar").addClass("collapseit").removeClass("expandit");
            $j1112("#main-content").addClass("sidebar_shift").removeClass("chat_shift");
            $j1112(".page-chatapi").removeClass("showit").addClass("hideit");
            $j1112(".chatapi-windows").removeClass("showit").addClass("hideit");
            CMPLTADMIN_SETTINGS.mainmenuCollapsed();

        } else {

            // large window
            $j1112(".page-topbar").removeClass("sidebar_shift chat_shift");
            $j1112(".page-sidebar").removeClass("collapseit chat_shift");
            $j1112("#main-content").removeClass("sidebar_shift chat_shift");
            CMPLTADMIN_SETTINGS.mainmenuScroll();
        }
        CMPLTADMIN_SETTINGS.disparEventoResize();
    }


    /*--------------------------------
         Window Based Layout
     --------------------------------*/
    CMPLTADMIN_SETTINGS.onLoadTopBar = function () {

        $j1112(".page-topbar .message-toggle-wrapper").addClass("showopacity");
        $j1112(".page-topbar .notify-toggle-wrapper").addClass("showopacity");
        $j1112(".page-topbar .searchform").addClass("showopacity");
        $j1112(".page-topbar li.profile").addClass("showopacity");
    }


    /*--------------------------------
         CHAT API
     --------------------------------*/
    CMPLTADMIN_SETTINGS.chatAPI = function () {


        $j1112('.page-topbar .toggle_chat').on('click', function () {
            var chatarea = $j1112(".page-chatapi");
            var chatwindow = $j1112(".chatapi-windows");
            var topbar = $j1112(".page-topbar");
            var mainarea = $j1112("#main-content");
            var menuarea = $j1112(".page-sidebar");

            if (chatarea.hasClass("hideit")) {
                chatarea.addClass("showit").removeClass("hideit");
                chatwindow.addClass("showit").removeClass("hideit");
                topbar.addClass("chat_shift");
                mainarea.addClass("chat_shift");
                menuarea.addClass("chat_shift");
                CMPLTADMIN_SETTINGS.mainmenuCollapsed();
            } else {
                chatarea.addClass("hideit").removeClass("showit");
                chatwindow.addClass("hideit").removeClass("showit");
                topbar.removeClass("chat_shift");
                mainarea.removeClass("chat_shift");
                menuarea.removeClass("chat_shift");
                //CMPLTADMIN_SETTINGS.mainmenuScroll();
                CMPLTADMIN_SETTINGS.windowBasedLayout();
            }
        });
        //$j1112('.main-wrapper').on('click', function () {
        //    if (!menuarea.hasClass("collapseit")) {
        //        menuarea.addClass("collapseit").removeClass("expandit").removeClass("chat_shift");
        //        topbar.addClass("sidebar_shift").removeClass("chat_shift");
        //        mainarea.addClass("sidebar_shift").removeClass("chat_shift");
        //        CMPLTADMIN_SETTINGS.mainmenuCollapsed();
        //        menuarea.removeClass('page-sidebar-show');
        //    }
        //});
        $j1112('.page-topbar .sidebar_toggle').on('click', function () {
            var chatarea = $j1112(".page-chatapi");
            var chatwindow = $j1112(".chatapi-windows");
            var topbar = $j1112(".page-topbar");
            var mainarea = $j1112("#main-content");
            var menuarea = $j1112(".page-sidebar");

            if (menuarea.hasClass("collapseit") || menuarea.hasClass("chat_shift")) {
                menuarea.addClass("expandit").removeClass("collapseit").removeClass("chat_shift");
                topbar.removeClass("sidebar_shift").removeClass("chat_shift");
                mainarea.removeClass("sidebar_shift").removeClass("chat_shift");
                chatarea.addClass("hideit").removeClass("showit");
                chatwindow.addClass("hideit").removeClass("showit");
                CMPLTADMIN_SETTINGS.mainmenuScroll();
                menuarea.addClass('page-sidebar-show');
            } else {
                menuarea.addClass("collapseit").removeClass("expandit").removeClass("chat_shift");
                topbar.addClass("sidebar_shift").removeClass("chat_shift");
                mainarea.addClass("sidebar_shift").removeClass("chat_shift");
                CMPLTADMIN_SETTINGS.mainmenuCollapsed();
                menuarea.removeClass('page-sidebar-show');
            }
            CMPLTADMIN_SETTINGS.triggerEventToggleMenu();
        });

    };


    /*--------------------------------
         CHAT API Scroll
     --------------------------------*/
    CMPLTADMIN_SETTINGS.chatApiScroll = function () {

        var topsearch = $j1112(".page-chatapi .search-bar").height();
        var height = window.innerHeight - topsearch;
        $j1112('.chat-wrapper').height(height).perfectScrollbar({
            suppressScrollX: true
        });
    };


    /*--------------------------------
         CHAT API window
     --------------------------------*/
    CMPLTADMIN_SETTINGS.chatApiWindow = function () {

        var chatarea = $j1112(".page-chatapi");

        $j1112('.page-chatapi .user-row').on('click', function () {

            var name = $j1112(this).find(".user-info h4 a").html();
            var img = $j1112(this).find(".user-img a img").attr("src");
            var id = $j1112(this).attr("data-user-id");
            var status = $j1112(this).find(".user-info .status").attr("data-status");

            if ($j1112(this).hasClass("active")) {
                $j1112(this).toggleClass("active");

                $j1112(".chatapi-windows #user-window" + id).hide();

            } else {
                $j1112(this).toggleClass("active");

                if ($j1112(".chatapi-windows #user-window" + id).length) {

                    $j1112(".chatapi-windows #user-window" + id).removeClass("minimizeit").show();

                } else {
                    var msg = chatformat_msg('Wow! What a Beautiful theme!', 'receive', name);
                    msg += chatformat_msg('Yes! Complete Admin Theme ;)', 'sent', 'You');
                    var html = "<div class='user-window' id='user-window" + id + "' data-user-id='" + id + "'>";
                    html += "<div class='controlbar'><img src='" + img + "' data-user-id='" + id + "' rel='tooltip' data-animate='animated fadeIn' data-toggle='tooltip' data-original-title='" + name + "' data-placement='top' data-color-class='primary'><span class='status " + status + "'><i class='fa fa-circle'></i></span><span class='name'>" + name + "</span><span class='opts'><i class='fa fa-times closeit' data-user-id='" + id + "'></i><i class='fa fa-minus minimizeit' data-user-id='" + id + "'></i></span></div>";
                    html += "<div class='chatarea'>" + msg + "</div>";
                    html += "<div class='typearea'><input type='text' data-user-id='" + id + "' placeholder='Type & Enter' class='form-control'></div>";
                    html += "</div>";
                    $j1112(".chatapi-windows").append(html);
                }
            }

        });

        $j1112(document).on('click', ".chatapi-windows .user-window .controlbar .closeit", function (e) {
            var id = $j1112(this).attr("data-user-id");
            $j1112(".chatapi-windows #user-window" + id).hide();
            $j1112(".page-chatapi .user-row#chat_user_" + id).removeClass("active");
        });

        $j1112(document).on('click', ".chatapi-windows .user-window .controlbar img, .chatapi-windows .user-window .controlbar .minimizeit", function (e) {
            var id = $j1112(this).attr("data-user-id");

            if (!$j1112(".chatapi-windows #user-window" + id).hasClass("minimizeit")) {
                $j1112(".chatapi-windows #user-window" + id).addClass("minimizeit");
                CMPLTADMIN_SETTINGS.tooltipsPopovers();
            } else {
                $j1112(".chatapi-windows #user-window" + id).removeClass("minimizeit");
            }

        });

        $j1112(document).on('keypress', ".chatapi-windows .user-window .typearea input", function (e) {
            if (e.keyCode == 13) {
                var id = $j1112(this).attr("data-user-id");
                var msg = $j1112(this).val();
                msg = chatformat_msg(msg, 'sent', 'You');
                $j1112(".chatapi-windows #user-window" + id + " .chatarea").append(msg);
                $j1112(this).val("");
                $j1112(this).focus();
            }
            $j1112(".chatapi-windows #user-window" + id + " .chatarea").perfectScrollbar({
                suppressScrollX: true
            });
        });

    };

    function chatformat_msg(msg, type, name) {
        var d = new Date();
        var h = d.getHours();
        var m = d.getMinutes();
        return "<div class='chatmsg msg_" + type + "'><span class='name'>" + name + "</span><span class='text'>" + msg + "</span><span class='ts'>" + h + ":" + m + "</span></div>";
    }


    /*--------------------------------
         Login Page
     --------------------------------*/
    CMPLTADMIN_SETTINGS.loginPage = function () {

        var height = window.innerHeight;
        var formheight = $j1112("#login").height();
        var newheight = (height - formheight) / 2;
        //console.log(height+" - "+ formheight + " / "+ newheight);
        $j1112('#login').css('margin-top', +newheight + 'px');

        if ($j1112('#login #user_login').length) {
            var d = document.getElementById('user_login');
            d.focus();
        }

    };



    /*--------------------------------
         Search Page
     --------------------------------*/
    CMPLTADMIN_SETTINGS.searchPage = function () {

        $j1112('.search_data .tab-pane').perfectScrollbar({
            suppressScrollX: true
        });
        var search = $j1112(".search-page-input");
        if (search.length) {
            search.focus();
        }
    };


    /*--------------------------------
        Viewport Checker
     --------------------------------*/
    CMPLTADMIN_SETTINGS.viewportElement = function () {

        if ($.isFunction($.fn.viewportChecker)) {

            $j1112('.inviewport').viewportChecker({
                callbackFunction: function (elem, action) {
                    //setTimeout(function(){
                    //elem.html((action == "add") ? 'Callback with 500ms timeout: added class' : 'Callback with 500ms timeout: removed class');
                    //},500);
                }
            });


            $j1112('.number_counter').viewportChecker({
                classToAdd: 'start_timer',
                offset: 10,
                callbackFunction: function (elem) {
                    $j1112('.start_timer:not(.counted)').each(count);
                    //$j1112(elem).removeClass('number_counter');
                }
            });

        }

        // start count
        function count(options) {
            var $this = $j1112(this);
            options = $.extend({}, options || {}, $this.data('countToOptions') || {});
            $this.countTo(options).addClass("counted");
        }
    };



    /*--------------------------------
        Sortable / Draggable Panels
     --------------------------------*/
    CMPLTADMIN_SETTINGS.draggablePanels = function () {

        if ($.isFunction($.fn.sortable)) {
            $j1112(".sort_panel").sortable({
                connectWith: ".sort_panel",
                handle: "header.panel_header",
                cancel: ".panel_actions",
                placeholder: "portlet-placeholder"
            });
        }
    };



    /*--------------------------------
         Breadcrumb autoHidden
     --------------------------------*/
    CMPLTADMIN_SETTINGS.breadcrumbAutoHidden = function () {

        $j1112('.breadcrumb.auto-hidden a').on('mouseover', function () {
            $j1112(this).removeClass("collapsed");
        });
        $j1112('.breadcrumb.auto-hidden a').on('mouseout', function () {
            $j1112(this).addClass("collapsed");
        });

    };





    /*--------------------------------
         Section Box Actions
     --------------------------------*/
    CMPLTADMIN_SETTINGS.sectionBoxActions = function () {

        $j1112('section.box .actions .box_toggle').on('click', function () {

            var content = $j1112(this).parent().parent().parent().find(".content-body");
            if (content.hasClass("collapsed")) {
                content.removeClass("collapsed").slideDown(500);
                $j1112(this).removeClass("fa-chevron-up").addClass("fa-chevron-down");
            } else {
                content.addClass("collapsed").slideUp(500);
                $j1112(this).removeClass("fa-chevron-down").addClass("fa-chevron-up");
            }

        });

        $j1112('section.box .actions .box_close').on('click', function () {
            content = $j1112(this).parent().parent().parent().remove();
        });



    };






    /*--------------------------------
         Main Menu Scroll
     --------------------------------*/
    CMPLTADMIN_SETTINGS.mainmenuScroll = function () {

        //console.log("expand scroll menu");

        var topbar = $j1112(".page-topbar").height();
        var projectinfo = $j1112(".project-info").innerHeight();

        var height = window.innerHeight - topbar - projectinfo;

        $j1112('.fixedscroll #main-menu-wrapper').height(height).perfectScrollbar({
            suppressScrollX: true
        });
        $j1112("#main-menu-wrapper .wraplist").height('auto');


        /*show first sub menu of open menu item only - opened after closed*/
        // > in the selector is used to select only immediate elements and not the inner nested elements.
        $j1112("li.open > .sub-menu").attr("style", "display:block;");


    };


    /*--------------------------------
         Collapsed Main Menu
     --------------------------------*/
    CMPLTADMIN_SETTINGS.mainmenuCollapsed = function () {

        if ($j1112(".page-sidebar.chat_shift #main-menu-wrapper").length > 0 || $j1112(".page-sidebar.collapseit #main-menu-wrapper").length > 0) {
            //console.log("collapse menu");
            var topbar = $j1112(".page-topbar").height();
            var windowheight = window.innerHeight;
            var minheight = windowheight - topbar;
            var fullheight = $j1112(".page-container #main-content .wrapper").height();

            var height = fullheight;

            if (fullheight < minheight) {
                height = minheight;
            }

            $j1112('.fixedscroll #main-menu-wrapper').perfectScrollbar('destroy');

            $j1112('.page-sidebar.chat_shift #main-menu-wrapper .wraplist, .page-sidebar.collapseit #main-menu-wrapper .wraplist').height(height);

            /*hide sub menu of open menu item*/
            $j1112("li.open .sub-menu").attr("style", "");
        }

    };




    /*--------------------------------
         Main Menu
     --------------------------------*/
    CMPLTADMIN_SETTINGS.mainMenu = function () {
        $j1112('#main-menu-wrapper li a').click(function (e) {

            if ($j1112(this).next().hasClass('sub-menu') === false) {
                return;
            }

            var parent = $j1112(this).parent().parent();
            var sub = $j1112(this).next();

            parent.children('li.open').children('.sub-menu').slideUp(200);
            parent.children('li.open').children('a').children('.arrow').removeClass('open');
            parent.children('li').removeClass('open');

            if (sub.is(":visible")) {
                $j1112(this).find(".arrow").removeClass("open");
                sub.slideUp(200);
            } else {
                $j1112(this).parent().addClass("open");
                $j1112(this).find(".arrow").addClass("open");
                sub.slideDown(200);
            }

        });

        $j1112("body").click(function (e) {
            $j1112(".page-sidebar.collapseit .wraplist li.open .sub-menu").attr("style", "");
            $j1112(".page-sidebar.collapseit .wraplist li.open").removeClass("open");
            $j1112(".page-sidebar.chat_shift .wraplist li.open .sub-menu").attr("style", "");
            $j1112(".page-sidebar.chat_shift .wraplist li.open").removeClass("open");
        });

    };



    /*--------------------------------
         Mailbox
     --------------------------------*/
    CMPLTADMIN_SETTINGS.mailboxInbox = function () {

        $j1112('.mail_list table .star i').click(function (e) {
            $j1112(this).toggleClass("fa-star fa-star-o");
        });

        $j1112('.mail_list .open-view').click(function (e) {
            window.location = '/Mailbox/MailView';
        });

        $j1112('.mail_view_info .labels .cc').click(function (e) {
            var ele = $j1112(".mail_compose_cc");
            if (ele.is(":visible")) {
                ele.hide();
            } else {
                ele.show();
            }
        });

        $j1112('.mail_view_info .labels .bcc').click(function (e) {
            var ele = $j1112(".mail_compose_bcc");
            if (ele.is(":visible")) {
                ele.hide();
            } else {
                ele.show();
            }
        });

    };




    /*--------------------------------
         Top Bar
     --------------------------------*/
    CMPLTADMIN_SETTINGS.pageTopBar = function () {
        $j1112('.page-topbar li.searchform .input-group-addon').click(function (e) {
            $j1112(this).parent().parent().parent().toggleClass("focus");
            $j1112(this).parent().parent().find("input").focus();
        });

        $j1112('.page-topbar li .dropdown-menu .list').perfectScrollbar({
            suppressScrollX: true
        });

    };


    /*--------------------------------
         Extra form settings
     --------------------------------*/
    CMPLTADMIN_SETTINGS.extraFormSettings = function () {

        // transparent input group focus/blur
        $j1112('.input-group .form-control').focus(function (e) {
            $j1112(this).parent().find(".input-group-addon").addClass("input-focus");
            $j1112(this).parent().find(".input-group-btn").addClass("input-focus");
        });

        $j1112('.input-group .form-control').blur(function (e) {
            $j1112(this).parent().find(".input-group-addon").removeClass("input-focus");
            $j1112(this).parent().find(".input-group-btn").removeClass("input-focus");
        });

    };



    /*--------------------------------
         js tree
     --------------------------------*/
    CMPLTADMIN_SETTINGS.jsTreeINIT = function () {


        if ($.isFunction($.fn.jstree)) {
            $j1112(function () {
                var to = false;
                $j1112('#treedata_q').keyup(function () {
                    if (to) {
                        clearTimeout(to);
                    }
                    to = setTimeout(function () {
                        var v = $j1112('#treedata_q').val();
                        $j1112('#jstree_treedata').jstree(true).search(v);
                    }, 250);
                });

                $j1112('#jstree_treedata')
                    .jstree({
                        "core": {
                            "animation": 0,
                            "check_callback": true,
                            "themes": {
                                "stripes": true
                            },
                            'data': {
                                'url': function (node) {
                                    return node.id === '#' ? '/data/ajax_demo_roots_jstree.json' : 'data/ajax_demo_children_jstree.json';
                                },
                                'data': function (node) {
                                    return {
                                        'id': node.id
                                    };
                                }
                            }
                        },
                        "types": {
                            "#": {
                                "max_children": 1,
                                "max_depth": 4,
                                "valid_children": ["root"]
                            },
                            "root": {
                                "icon": "~/lib/jstree/images/tree_icon.png",
                                "valid_children": ["default"]
                            },
                            "default": {
                                "valid_children": ["default", "file"]
                            },
                            "file": {
                                "icon": "fa fa-file",
                                "valid_children": []
                            }
                        },
                        "checkbox": {
                            "keep_selected_style": false
                        },
                        "plugins": ["checkbox", "contextmenu", "dnd", "search", "sort", "state", "types", "unique", "wholerow"]
                    });
            });

        }
    };



    /*--------------------------------
         Vector maps
     --------------------------------*/
    CMPLTADMIN_SETTINGS.jvectorMaps = function () {

        if ($.isFunction($.fn.vectorMap)) {

            if ($j1112("#world-map-markers").length) {
                //@code_start
                $j1112(function () {
                    $j1112('#world-map-markers').vectorMap({
                        map: 'world_mill_en',
                        scaleColors: ['#363537', '#363537'],
                        normalizeFunction: 'polynomial',
                        hoverOpacity: 0.7,
                        hoverColor: false,
                        regionsSelectable: true,
                        markersSelectable: true,
                        markersSelectableOne: true,

                        onRegionOver: function (event, code) {
                            //console.log('region-over', code);
                        },
                        onRegionOut: function (event, code) {
                            //console.log('region-out', code);
                        },
                        onRegionClick: function (event, code) {
                            //console.log('region-click', code);
                        },
                        onRegionSelected: function (event, code, isSelected, selectedRegions) {
                            //console.log('region-select', code, isSelected, selectedRegions);
                            if (window.localStorage) {
                                window.localStorage.setItem(
                                    'jvectormap-selected-regions',
                                    JSON.stringify(selectedRegions)
                                );
                            }
                        },

                        panOnDrag: true,

                        focusOn: {
                            x: 1.5,
                            y: 1.5,
                            scale: 1,
                            animate: true
                        },


                        regionStyle: {
                            initial: {
                                fill: '#cccccc',
                                'fill-opacity': 1,
                                stroke: 'none',
                                'stroke-width': 0,
                                'stroke-opacity': 1
                            },
                            hover: {
                                fill: '#E91E63',
                                'fill-opacity': 1,
                                cursor: 'pointer'
                            },
                            selected: {
                                fill: '#E91E63'
                            },
                            selectedHover: {}
                        },



                        markerStyle: {
                            initial: {
                                fill: '#673AB7',
                                stroke: '#ffffff',
                                "stroke-width": 2,
                                r: 10
                            },
                            hover: {
                                stroke: '#FFC107',
                                "stroke-width": 2,
                                cursor: 'pointer'
                            },
                            selected: {
                                fill: '#FFC107',
                                "stroke-width": 0,
                            },
                        },
                        backgroundColor: '#ffffff',
                        markers: [{
                            latLng: [41.90, 12.45],
                            name: 'Vatican City'
                        }, {
                            latLng: [43.73, 7.41],
                            name: 'Monaco'
                        }, {
                            latLng: [-0.52, 166.93],
                            name: 'Nauru'
                        }, {
                            latLng: [-8.51, 179.21],
                            name: 'Tuvalu'
                        }, {
                            latLng: [43.93, 12.46],
                            name: 'San Marino'
                        }, {
                            latLng: [47.14, 9.52],
                            name: 'Liechtenstein'
                        }, {
                            latLng: [7.11, 171.06],
                            name: 'Marshall Islands'
                        }, {
                            latLng: [17.3, -62.73],
                            name: 'Saint Kitts and Nevis'
                        }, {
                            latLng: [3.2, 73.22],
                            name: 'Maldives'
                        }, {
                            latLng: [35.88, 14.5],
                            name: 'Malta'
                        }, {
                            latLng: [12.05, -61.75],
                            name: 'Grenada'
                        }, {
                            latLng: [13.16, -61.23],
                            name: 'Saint Vincent and the Grenadines'
                        }, {
                            latLng: [13.16, -59.55],
                            name: 'Barbados'
                        }, {
                            latLng: [17.11, -61.85],
                            name: 'Antigua and Barbuda'
                        }, {
                            latLng: [-4.61, 55.45],
                            name: 'Seychelles'
                        }, {
                            latLng: [7.35, 134.46],
                            name: 'Palau'
                        }, {
                            latLng: [42.5, 1.51],
                            name: 'Andorra'
                        }, {
                            latLng: [14.01, -60.98],
                            name: 'Saint Lucia'
                        }, {
                            latLng: [6.91, 158.18],
                            name: 'Federated States of Micronesia'
                        }, {
                            latLng: [1.3, 103.8],
                            name: 'Singapore'
                        }, {
                            latLng: [1.46, 173.03],
                            name: 'Kiribati'
                        }, {
                            latLng: [-21.13, -175.2],
                            name: 'Tonga'
                        }, {
                            latLng: [15.3, -61.38],
                            name: 'Dominica'
                        }, {
                            latLng: [-20.2, 57.5],
                            name: 'Mauritius'
                        }, {
                            latLng: [26.02, 50.55],
                            name: 'Bahrain'
                        }, {
                            latLng: [0.33, 6.73],
                            name: 'São Tomé and Príncipe'
                        }]
                    });
                });
                //@code_end
            }

            var mapid = "";
            mapid = $j1112('#europe_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'europe_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 1,
                        animate: true
                    },
                });
            } // Europe 
            mapid = $j1112('#in_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'in_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // India
            mapid = $j1112('#us_aea_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'us_aea_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // USA
            mapid = $j1112('#pt_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'pt_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Portugal
            mapid = $j1112('#cn_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'cn_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // China
            mapid = $j1112('#nz_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'nz_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // New Zealand
            mapid = $j1112('#no_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'no_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Norway
            mapid = $j1112('#es_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'es_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Spain
            mapid = $j1112('#au_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'au_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Australia
            mapid = $j1112('#fr_regions_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'fr_regions_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // France - Regions
            mapid = $j1112('#th_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'th_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Thailand
            mapid = $j1112('#co_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'co_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Colombia
            mapid = $j1112('#be_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'be_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Belgium
            mapid = $j1112('#ar_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'ar_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Argentina
            mapid = $j1112('#ve_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 've_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Venezuela
            mapid = $j1112('#it_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'it_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Italy
            mapid = $j1112('#dk_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'dk_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Denmark
            mapid = $j1112('#at_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'at_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Austria
            mapid = $j1112('#ca_lcc_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'ca_lcc_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Canada
            mapid = $j1112('#nl_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'nl_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Netherlands
            mapid = $j1112('#se_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'se_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Sweden
            mapid = $j1112('#pl_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'pl_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Poland
            mapid = $j1112('#de_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'de_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Germany
            mapid = $j1112('#fr_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'fr_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // France - Departments
            mapid = $j1112('#za_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'za_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // South Africa
            mapid = $j1112('#ch_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'ch_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Switzerland
            mapid = $j1112('#us-ny-newyork_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'us-ny-newyork_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // New York City
            mapid = $j1112('#us-il-chicago_mill_en-map');
            if (mapid.length) {
                mapid.vectorMap({
                    map: 'us-il-chicago_mill_en',
                    regionsSelectable: true,
                    backgroundColor: '#363537',
                    regionStyle: {
                        initial: {
                            fill: 'white',
                            stroke: 'none',
                        },
                        hover: {
                            fill: '#E91E63',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#E91E63'
                        }
                    },
                    focusOn: {
                        x: 0,
                        y: 0,
                        scale: 5,
                        animate: true
                    },
                });
            } // Chicago

        }

    };


    ///*--------------------------------
    //     DataTables
    // --------------------------------*/
    //CMPLTADMIN_SETTINGS.dataTablesInit = function() {

    //    if ($.isFunction($.fn.dataTable)) {

    //        /*--- start ---*/

    //        $j1112("#example-1").dataTable({
    //            responsive: true,
    //            aLengthMenu: [
    //                [10, 25, 50, 100, -1],
    //                [10, 25, 50, 100, "All"]
    //            ]
    //        });

    //        /*--- end ---*/

    //        /*--- start ---*/

    //        $j1112('#example-4').dataTable();

    //        /*--- end ---*/



    //        /* Set the defaults for DataTables initialisation */
    //        $.extend(true, $.fn.dataTable.defaults, {
    //            "sDom": "<'row'<'col-md-6'l><'col-md-6'f>r>t<'row'<'col-md-12'p i>>",
    //            "sPaginationType": "bootstrap",
    //            "oLanguage": {
    //                "sLengthMenu": "_MENU_"
    //            }
    //        });


    //        /* Default class modification */
    //        $.extend($.fn.dataTableExt.oStdClasses, {
    //            "sWrapper": "dataTables_wrapper form-inline"
    //        });


    //        /* API method to get paging information */
    //        $.fn.dataTableExt.oApi.fnPagingInfo = function(oSettings) {
    //            return {
    //                "iStart": oSettings._iDisplayStart,
    //                "iEnd": oSettings.fnDisplayEnd(),
    //                "iLength": oSettings._iDisplayLength,
    //                "iTotal": oSettings.fnRecordsTotal(),
    //                "iFilteredTotal": oSettings.fnRecordsDisplay(),
    //                "iPage": oSettings._iDisplayLength === -1 ?
    //                    0 : Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength),
    //                "iTotalPages": oSettings._iDisplayLength === -1 ?
    //                    0 : Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength)
    //            };
    //        };


    //        /* Bootstrap style pagination control */
    //        $.extend($.fn.dataTableExt.oPagination, {
    //            "bootstrap": {
    //                "fnInit": function(oSettings, nPaging, fnDraw) {
    //                    var oLang = oSettings.oLanguage.oPaginate;
    //                    var fnClickHandler = function(e) {
    //                        e.preventDefault();
    //                        if (oSettings.oApi._fnPageChange(oSettings, e.data.action)) {
    //                            fnDraw(oSettings);
    //                        }
    //                    };

    //                    $j1112(nPaging).addClass('').append(
    //                        '<ul class="pagination pull-right">' +
    //                        '<li class="prev disabled"><a href="#"><i class="fa fa-angle-double-left"></i></a></li>' +
    //                        '<li class="next disabled"><a href="#"><i class="fa fa-angle-double-right"></i></a></li>' +
    //                        '</ul>'
    //                    );
    //                    var els = $j1112('a', nPaging);
    //                    $j1112(els[0]).bind('click.DT', {
    //                        action: "previous"
    //                    }, fnClickHandler);
    //                    $j1112(els[1]).bind('click.DT', {
    //                        action: "next"
    //                    }, fnClickHandler);
    //                },

    //                "fnUpdate": function(oSettings, fnDraw) {
    //                    var iListLength = 5;
    //                    var oPaging = oSettings.oInstance.fnPagingInfo();
    //                    var an = oSettings.aanFeatures.p;
    //                    var i, ien, j, sClass, iStart, iEnd, iHalf = Math.floor(iListLength / 2);

    //                    if (oPaging.iTotalPages < iListLength) {
    //                        iStart = 1;
    //                        iEnd = oPaging.iTotalPages;
    //                    } else if (oPaging.iPage <= iHalf) {
    //                        iStart = 1;
    //                        iEnd = iListLength;
    //                    } else if (oPaging.iPage >= (oPaging.iTotalPages - iHalf)) {
    //                        iStart = oPaging.iTotalPages - iListLength + 1;
    //                        iEnd = oPaging.iTotalPages;
    //                    } else {
    //                        iStart = oPaging.iPage - iHalf + 1;
    //                        iEnd = iStart + iListLength - 1;
    //                    }

    //                    for (i = 0, ien = an.length; i < ien; i++) {
    //                        // Remove the middle elements
    //                        $j1112('li:gt(0)', an[i]).filter(':not(:last)').remove();

    //                        // Add the new list items and their event handlers
    //                        for (j = iStart; j <= iEnd; j++) {
    //                            sClass = (j == oPaging.iPage + 1) ? 'class="active"' : '';
    //                            $j1112('<li ' + sClass + '><a href="#">' + j + '</a></li>')
    //                                .insertBefore($j1112('li:last', an[i])[0])
    //                                .bind('click', function(e) {
    //                                    e.preventDefault();
    //                                    oSettings._iDisplayStart = (parseInt($j1112('a', this).text(), 10) - 1) * oPaging.iLength;
    //                                    fnDraw(oSettings);
    //                                });
    //                        }

    //                        // Add / remove disabled classes from the static elements
    //                        if (oPaging.iPage === 0) {
    //                            $j1112('li:first', an[i]).addClass('disabled');
    //                        } else {
    //                            $j1112('li:first', an[i]).removeClass('disabled');
    //                        }

    //                        if (oPaging.iPage === oPaging.iTotalPages - 1 || oPaging.iTotalPages === 0) {
    //                            $j1112('li:last', an[i]).addClass('disabled');
    //                        } else {
    //                            $j1112('li:last', an[i]).removeClass('disabled');
    //                        }
    //                    }
    //                }
    //            }
    //        });


    //        /*
    //         * TableTools Bootstrap compatibility
    //         * Required TableTools 2.1+
    //         */

    //        // Set the classes that TableTools uses to something suitable for Bootstrap
    //        $.extend(true, $.fn.DataTable.TableTools.classes, {
    //            "container": "DTTT ",
    //            "buttons": {
    //                "normal": "btn btn-white",
    //                "disabled": "disabled"
    //            },
    //            "collection": {
    //                "container": "DTTT_dropdown dropdown-menu",
    //                "buttons": {
    //                    "normal": "",
    //                    "disabled": "disabled"
    //                }
    //            },
    //            "print": {
    //                "info": "DTTT_print_info modal"
    //            },
    //            "select": {
    //                "row": "active"
    //            }
    //        });

    //        // Have the collection use a bootstrap compatible dropdown
    //        $.extend(true, $.fn.DataTable.TableTools.DEFAULTS.oTags, {
    //            "collection": {
    //                "container": "ul",
    //                "button": "li",
    //                "liner": "a"
    //            }
    //        });


    //        /* Table initialisation */
    //        $j1112(document).ready(function() {
    //            var responsiveHelper = undefined;
    //            var breakpointDefinition = {
    //                tablet: 1024,
    //                phone: 480
    //            };
    //            var tableElement = $j1112('#example');

    //            tableElement.dataTable({
    //                "sDom": "<'row'<'col-md-6'l T><'col-md-6'f>r>t<'row'<'col-md-12'p i>>",
    //                "oTableTools": {
    //                    "aButtons": [{
    //                        "sExtends": "collection",
    //                        "sButtonText": "<i class='fa fa-cloud-download'></i>",
    //                        "aButtons": ["csv", "xls", "pdf", "copy"]
    //                    }]
    //                },
    //                "sPaginationType": "bootstrap",
    //                "aoColumnDefs": [{
    //                    'bSortable': false,
    //                    'aTargets': [0]
    //                }],
    //                "aaSorting": [
    //                    [1, "asc"]
    //                ],
    //                "oLanguage": {
    //                    "sLengthMenu": "_MENU_ ",
    //                    "sInfo": "Exibindo _START_ de _END_ de _TOTAL_ registros"
    //                },
    //                bAutoWidth: false,
    //                fnPreDrawCallback: function() {
    //                    // Initialize the responsive datatables helper once.
    //                    if (!responsiveHelper) {
    //                        //responsiveHelper = new ResponsiveDatatablesHelper(tableElement, breakpointDefinition);
    //                    }
    //                },
    //                fnRowCallback: function(nRow) {
    //                    //responsiveHelper.createExpandIcon(nRow);
    //                },
    //                fnDrawCallback: function(oSettings) {
    //                    //responsiveHelper.respond();
    //                }
    //            });

    //            $j1112('#example_wrapper .dataTables_filter input').addClass("input-medium "); // modify table search input
    //            $j1112('#example_wrapper .dataTables_length select').addClass("select2-wrapper"); // modify table per page dropdown



    //            $j1112('#example input').click(function() {
    //                $j1112(this).parent().parent().parent().toggleClass('row_selected');
    //            });


    //            /*
    //             * Insert a 'details' column to the table
    //             */
    //            var nCloneTh = document.createElement('th');
    //            var nCloneTd = document.createElement('td');
    //            nCloneTd.innerHTML = '<i class="fa fa-plus-circle"></i>';
    //            nCloneTd.className = "center";

    //            $j1112('#example2 thead tr').each(function() {
    //                this.insertBefore(nCloneTh, this.childNodes[0]);
    //            });

    //            $j1112('#example2 tbody tr').each(function() {
    //                this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
    //            });

    //            /*
    //             * Initialse DataTables, with no sorting on the 'details' column
    //             */
    //            var oTable = $j1112('#example2').dataTable({
    //                "sDom": "<'row'<'col-md-6'l><'col-md-6'f>r>t<'row'<'col-md-12'p i>>",
    //                "aaSorting": [],
    //                "oLanguage": {
    //                    "sLengthMenu": "_MENU_ ",
    //                    "sInfo": "Exibindo _START_ de _END_ de _TOTAL_ registros"
    //                },
    //            });


    //            $j1112("div.toolbar").html('<div class="table-tools-actions"><button class="btn btn-primary" style="margin-left:12px" id="test2">Add</button></div>');


    //            $j1112('#example2_wrapper .dataTables_filter input').addClass("input-medium ");
    //            $j1112('#example2_wrapper .dataTables_length select').addClass("select2-wrapper");

    //            /* Add event listener for opening and closing details
    //             * Note that the indicator for showing which row is open is not controlled by DataTables,
    //             * rather it is done here
    //             */
    //            $j1112('#example2 tbody td i').on('click', function() {
    //                var nTr = $j1112(this).parents('tr')[0];
    //                if (oTable.fnIsOpen(nTr)) {
    //                    /* This row is already open - close it */
    //                    this.removeClass = "fa fa-plus-circle";
    //                    this.addClass = "fa fa-minus-circle";
    //                    oTable.fnClose(nTr);
    //                } else {
    //                    /* Open this row */
    //                    this.removeClass = "fa fa-minus-circle";
    //                    this.addClass = "fa fa-plus-circle";
    //                    oTable.fnOpen(nTr, fnFormatDetails(oTable, nTr), 'details');
    //                }


    //                /* Formating function for row details */
    //                function fnFormatDetails(oTable, nTr) {
    //                    var aData = oTable.fnGetData(nTr);
    //                    var sOut = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;" class="inner-table">';
    //                    sOut += '<tr><td>Rendering engine:</td><td>' + aData[1] + ' ' + aData[4] + '</td></tr>';
    //                    sOut += '<tr><td>Link to source:</td><td>Could provide a link here</td></tr>';
    //                    sOut += '<tr><td>Extra info:</td><td>And any further details here (images etc)</td></tr>';
    //                    sOut += '</table>';

    //                    return sOut;
    //                }

    //            });

    //        });
    //    }
    //};



    /*--------------------------------
         Pretty Photo
     --------------------------------*/
    CMPLTADMIN_SETTINGS.loadPrettyPhoto = function () {

        if ($.isFunction($.fn.prettyPhoto)) {
            //Pretty Photo
            $j1112("a[rel^='prettyPhoto']").prettyPhoto({
                social_tools: false
            });
        }
    };




    /*--------------------------------
         Gallery
     --------------------------------*/
    CMPLTADMIN_SETTINGS.isotopeGallery = function () {
        if ($.isFunction($.fn.isotope)) {

            var $portfolio_selectors = $j1112('.portfolio-filter >li>a');
            var $portfolio = $j1112('.portfolio-items');
            $portfolio.isotope({
                itemSelector: '.portfolio-item',
                layoutMode: 'sloppyMasonry'
            });

            $portfolio_selectors.on('click', function () {
                $portfolio_selectors.removeClass('active');
                $j1112(this).addClass('active');
                var selector = $j1112(this).attr('data-filter');
                $portfolio.isotope({
                    filter: selector
                });
                return false;
            });


        }
    };


    /*--------------------------------
         Tocify
     --------------------------------*/
    CMPLTADMIN_SETTINGS.tocifyScrollMenu = function () {
        if ($.isFunction($.fn.tocify)) {
            var toc = $j1112("#toc").tocify({
                selectors: "h2,h3,h4,h5",
                context: ".tocify-content",
                extendPage: false
            }).data("toc-tocify");
        }
    };



    /*--------------------------------
         Full Calendar
     --------------------------------*/
    CMPLTADMIN_SETTINGS.uiCalendar = function () {


        if ($.isFunction($.fn.fullCalendar)) {

            /* initialize the external events
                 -----------------------------------------------------------------*/

            $j1112('#external-events .fc-event').each(function () {

                // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
                // it doesn't need to have a start or end
                var eventObject = {
                    title: $.trim($j1112(this).text()) // use the element's text as the event title
                };

                // store the Event Object in the DOM element so we can get to it later
                $j1112(this).data('eventObject', eventObject);

                // make the event draggable using jQuery UI
                $j1112(this).draggable({
                    zIndex: 999,
                    revert: true, // will cause the event to go back to its
                    revertDuration: 0 //  original position after the drag
                });

            });


            /* initialize the calendar
             -----------------------------------------------------------------*/

            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();

            $j1112('#calendar').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,basicWeek,basicDay'
                },
                editable: true,
                eventLimit: true, // allow "more" link when too many events
                droppable: true, // this allows things to be dropped onto the calendar !!!
                drop: function (date, allDay) { // this function is called when something is dropped

                    // retrieve the dropped element's stored Event Object
                    var originalEventObject = $j1112(this).data('eventObject');

                    // we need to copy it, so that multiple events don't have a reference to the same object
                    var copiedEventObject = $.extend({}, originalEventObject);

                    // assign it the date that was reported
                    copiedEventObject.start = date;
                    copiedEventObject.allDay = allDay;

                    // render the event on the calendar
                    // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
                    $j1112('#calendar').fullCalendar('renderEvent', copiedEventObject, true);

                    // is the "remove after drop" checkbox checked?
                    if ($j1112('#drop-remove').is(':checked')) {
                        // if so, remove the element from the "Draggable Events" list
                        $j1112(this).remove();
                    }

                },
                events: [{
                    title: 'All Day Event',
                    start: new Date(y, m, 1)
                }, {
                    title: 'Long Event',
                    start: new Date(y, m, d - 5),
                    end: new Date(y, m, d - 2)
                }, {
                    id: 999,
                    title: 'Repeating Event',
                    start: new Date(y, m, d - 3, 16, 0),
                    allDay: false
                }, {
                    id: 999,
                    title: 'Repeating Event',
                    start: new Date(y, m, d + 4, 16, 0),
                    allDay: false
                }, {
                    title: 'Meeting',
                    start: new Date(y, m, d, 10, 30),
                    allDay: false
                }, {
                    title: 'Lunch',
                    start: new Date(y, m, d, 12, 0),
                    end: new Date(y, m, d, 14, 0),
                    allDay: false
                }, {
                    title: 'Conference',
                    start: new Date(y, m, d + 1, 19, 0),
                    end: new Date(y, m, d + 1, 22, 30),
                    allDay: false
                }, {
                    title: 'Staff Meeting',
                    start: new Date(y, m, 28),
                    end: new Date(y, m, 29),
                    url: 'http://google.com/'
                }]
            });





            /*Add new event*/
            // Form to add new event

            $j1112("#add_event_form").on('submit', function (ev) {
                ev.preventDefault();

                var $event = $j1112(this).find('.new-event-form'),
                    event_name = $event.val();

                if (event_name.length >= 3) {

                    var newid = "new" + "" + Math.random().toString(36).substring(7);
                    // Create Event Entry
                    $j1112("#external-events").append(
                        '<div id="' + newid + '" class="fc-event bg-accent">' + event_name + '</div>'
                    );


                    var eventObject = {
                        title: $.trim($j1112("#" + newid).text()) // use the element's text as the event title
                    };

                    // store the Event Object in the DOM element so we can get to it later
                    $j1112("#" + newid).data('eventObject', eventObject);

                    // Reset draggable
                    $j1112("#" + newid).draggable({
                        revert: true,
                        revertDuration: 0,
                        zIndex: 999
                    });

                    // Reset input
                    $event.val('').focus();
                } else {
                    $event.focus();
                }
            });



        }

    };



    /*--------------------------------
         Sortable (Nestable) List
     --------------------------------*/
    CMPLTADMIN_SETTINGS.nestableList = function () {

        $j1112("#nestableList-1").on('stop.uk.nestable', function (ev) {
            var serialized = $j1112(this).data('nestable').serialize(),
                str = '';

            str = nestableIterate(serialized, 0);

            $j1112("#nestableList-1-ev").val(str);
        });


        function nestableIterate(items, depth) {
            var str = '';

            if (!depth)
                depth = 0;

            //console.log(items);

            jQuery.each(items, function (i, obj) {
                str += '[ID: ' + obj.itemId + ']\t' + nestableRepeat('—', depth + 1) + ' ' + obj.item;
                str += '\n';

                if (obj.children) {
                    str += nestableIterate(obj.children, depth + 1);
                }
            });

            return str;
        }

        function nestableRepeat(s, n) {
            var a = [];
            while (a.length < n) {
                a.push(s);
            }
            return a.join('');
        }
    };









    /*--------------------------------
         Tooltips & Popovers
     --------------------------------*/
    CMPLTADMIN_SETTINGS.tooltipsPopovers = function () {

        $j1112('[rel="tooltip"]').each(function () {
            var animate = $j1112(this).attr("data-animate");
            var colorclass = $j1112(this).attr("data-color-class");
            $j1112(this).tooltip({
                template: '<div class="tooltip ' + animate + ' ' + colorclass + '"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>'
            });
        });

        $j1112('[rel="popover"]').each(function () {
            var animate = $j1112(this).attr("data-animate");
            var colorclass = $j1112(this).attr("data-color-class");
            $j1112(this).popover({
                template: '<div class="popover ' + animate + ' ' + colorclass + '"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
            });
        });

    };





    /*--------------------------------
         iCheck
     --------------------------------*/
    CMPLTADMIN_SETTINGS.iCheck = function () {



        if ($.isFunction($.fn.iCheck)) {


            $j1112('input[type="checkbox"].iCheck').iCheck({
                checkboxClass: 'icheckbox_minimal',
                radioClass: 'iradio_minimal',
                increaseArea: '20%'
            });


            var x;
            var colors = ["-green", "-red", "-yellow", "-blue", "-aero", "-orange", "-grey", "-pink", "-purple", "-white"];

            for (x = 0; x < colors.length; x++) {

                if (x == 0) {
                    $j1112('input.icheck-minimal').iCheck({
                        checkboxClass: 'icheckbox_minimal' + colors[x],
                        radioClass: 'iradio_minimal' + colors[x],
                        increaseArea: '20%'
                    });

                    $j1112('input.skin-square').iCheck({
                        checkboxClass: 'icheckbox_square' + colors[x],
                        radioClass: 'iradio_square' + colors[x],
                        increaseArea: '20%'
                    });

                    $j1112('input.skin-flat').iCheck({
                        checkboxClass: 'icheckbox_flat' + colors[x],
                        radioClass: 'iradio_flat' + colors[x],
                    });


                    $j1112('input.skin-line').each(function () {
                        var self = $j1112(this),
                            label = self.next(),
                            label_text = label.text();

                        label.remove();
                        self.iCheck({
                            checkboxClass: 'icheckbox_line' + colors[x],
                            radioClass: 'iradio_line' + colors[x],
                            insert: '<div class="icheck_line-icon"></div>' + label_text
                        });
                    });

                } // end x = 0

                $j1112('input.icheck-minimal' + colors[x]).iCheck({
                    checkboxClass: 'icheckbox_minimal' + colors[x],
                    radioClass: 'iradio_minimal' + colors[x],
                    increaseArea: '20%'
                });


                $j1112('input.skin-square' + colors[x]).iCheck({
                    checkboxClass: 'icheckbox_square' + colors[x],
                    radioClass: 'iradio_square' + colors[x],
                    increaseArea: '20%'
                });


                $j1112('input.skin-flat' + colors[x]).iCheck({
                    checkboxClass: 'icheckbox_flat' + colors[x],
                    radioClass: 'iradio_flat' + colors[x],
                });


                $j1112('input.skin-line' + colors[x]).each(function () {
                    var self = $j1112(this),
                        label = self.next(),
                        label_text = label.text();

                    label.remove();
                    self.iCheck({
                        checkboxClass: 'icheckbox_line' + colors[x],
                        radioClass: 'iradio_line' + colors[x],
                        insert: '<div class="icheck_line-icon"></div>' + label_text
                    });
                });

            } // end for loop


        }
    };




    /*--------------------------------
         Form Editors
     --------------------------------*/
    CMPLTADMIN_SETTINGS.formEditors = function () {

        if ($.isFunction($.fn.wysihtml5)) {
            $j1112('.bootstrap-wysihtml5-textarea').wysihtml5({
                toolbar: {
                    "font-styles": true, //Font styling, e.g. h1, h2, etc. Default true
                    "emphasis": true, //Italics, bold, etc. Default true
                    "lists": true, //(Un)ordered lists, e.g. Bullets, Numbers. Default true
                    "html": true, //Button which allows you to edit the generated HTML. Default false
                    "link": true, //Button to insert a link. Default true
                    "image": true, //Button to insert an image. Default true,
                    "color": true, //Button to change color of font  
                    "blockquote": true, //Blockquote  
                    "size": "none" //default: none, other options are xs, sm, lg
                }
            });


            $j1112('.mail-compose-editor').wysihtml5({
                toolbar: {
                    "font-styles": true, //Font styling, e.g. h1, h2, etc. Default true
                    "emphasis": true, //Italics, bold, etc. Default true
                    "lists": false, //(Un)ordered lists, e.g. Bullets, Numbers. Default true
                    "html": true, //Button which allows you to edit the generated HTML. Default false
                    "link": true, //Button to insert a link. Default true
                    "image": true, //Button to insert an image. Default true,
                    "color": true, //Button to change color of font  
                    "blockquote": false, //Blockquote  
                    "size": "none" //default: none, other options are xs, sm, lg
                }
            });

        }

        if ($.isFunction($.fn.CKEDITOR)) {
            // This code is generally not necessary, but it is here to demonstrate
            // how to customize specific editor instances on the fly. This fits well
            // this demo because we have editable elements (like headers) that
            // require less features.

            // The "instanceCreated" event is fired for every editor instance created.
            CKEDITOR.on('instanceCreated', function (event) {
                var editor = event.editor,
                    element = editor.element;

                // Customize editors for headers and tag list.
                // These editors don't need features like smileys, templates, iframes etc.
                if (element.is('h1', 'h2', 'h3') || element.getAttribute('id') == 'taglist') {
                    // Customize the editor configurations on "configLoaded" event,
                    // which is fired after the configuration file loading and
                    // execution. This makes it possible to change the
                    // configurations before the editor initialization takes place.
                    editor.on('configLoaded', function () {

                        // Remove unnecessary plugins to make the editor simpler.
                        editor.config.removePlugins = 'colorbutton,find,flash,font,' +
                            'forms,iframe,image,newpage,removeformat,' +
                            'smiley,specialchar,stylescombo,templates';

                        // Rearrange the layout of the toolbar.
                        editor.config.toolbarGroups = [{
                            name: 'editing',
                            groups: ['basicstyles', 'links']
                        }, {
                            name: 'undo'
                        }, {
                            name: 'clipboard',
                            groups: ['selection', 'clipboard']
                        }, {
                            name: 'about'
                        }];
                    });
                }
            });
        }
    };


    /*--------------------------------
         Custom Dropzone
     --------------------------------*/
    CMPLTADMIN_SETTINGS.customDropZone = function () {



        if ($.isFunction($.fn.dropzone)) {

            var i = 1,
                $custom_droplist = $j1112("#custom-droptable"),
                example_dropzone = $j1112("#customDZ").dropzone({
                    url: 'data/upload-file.php',

                    // Events
                    addedfile: function (file) {
                        if (i == 1) {
                            $custom_droplist.find('tbody').html('');
                        }

                        var size = parseInt(file.size / 1024, 10);
                        size = size < 1024 ? (size + " KB") : (parseInt(size / 1024, 10) + " MB");

                        var $el = $j1112('<tr>\
                                                    <td class="text-center">' + (i++) + '</td>\
                                                    <td>' + file.name + '</td>\
                                                    <td><div class="progress"><div class="progress-bar progress-bar-warning"></div></div></td>\
                                                    <td>' + size + '</td>\
                                                </tr>');

                        $custom_droplist.find('tbody').append($el);
                        file.fileEntryTd = $el;
                        file.progressBar = $el.find('.progress-bar');
                    },

                    uploadprogress: function (file, progress, bytesSent) {
                        file.progressBar.width(progress + '%');
                        $j1112('.custom-dropzone .drop-table').perfectScrollbar({
                            suppressScrollX: true
                        });
                    },

                    success: function (file) {
                        file.progressBar.removeClass('progress-bar-warning').addClass('progress-bar-success');
                    },

                    error: function (file) {
                        file.progressBar.removeClass('progress-bar-warning').addClass('progress-bar-red');
                    }
                });

        }

    };


    /*--------------------------------
         Other Form component Scripts
     --------------------------------*/
    CMPLTADMIN_SETTINGS.otherScripts = function () {



        /*--------------------------------*/


        if ($.isFunction($.fn.autosize)) {
            $j1112(".autogrow").autosize();
        }

        /*--------------------------------*/




        // Input Mask
        if ($.isFunction($.fn.inputmask)) {
            $j1112("[data-mask]").each(function (i, el) {
                var $this = $j1112(el),
                    mask = $this.data('mask').toString(),
                    opts = {
                        numericInput: getValue($this, 'numeric', false),
                        radixPoint: getValue($this, 'radixPoint', ''),
                        rightAlign: getValue($this, 'numericAlign', 'left') == 'right'
                    },
                    placeholder = getValue($this, 'placeholder', ''),
                    is_regex = getValue($this, 'isRegex', '');

                if (placeholder.length) {
                    opts[placeholder] = placeholder;
                }


                if (mask.toLowerCase() == "phone") {
                    mask = "(999) 999-9999";
                }

                if (mask.toLowerCase() == "email") {
                    mask = 'Regex';
                    opts.regex = "[a-zA-Z0-9._%-]+@[a-zA-Z0-9-]+\\.[a-zA-Z]{2,4}";
                }

                if (mask.toLowerCase() == "fdecimal") {
                    mask = 'decimal';
                    $.extend(opts, {
                        autoGroup: true,
                        groupSize: 3,
                        radixPoint: getValue($this, 'rad', '.'),
                        groupSeparator: getValue($this, 'dec', ',')
                    });
                }


                if (mask.toLowerCase() == "currency" || mask.toLowerCase() == "rcurrency") {

                    var sign = getValue($this, 'sign', '$');;

                    mask = "999,999,999.99";
                    if (mask.toLowerCase() == 'rcurrency') {
                        mask += ' ' + sign;
                    } else {
                        mask = sign + ' ' + mask;
                    }

                    opts.numericInput = true;
                    opts.rightAlignNumerics = false;
                    opts.radixPoint = '.';

                }

                if (is_regex) {
                    opts.regex = mask;
                    mask = 'Regex';
                }

                $this.inputmask(mask, opts);
            });
        }


        /*---------------------------------*/

        // autoNumeric
        if ($.isFunction($.fn.autoNumeric)) {
            $j1112('.autoNumeric').autoNumeric('init');
        }

        /*---------------------------------*/

        // Slider
        if ($.isFunction($.fn.slider)) {
            $j1112(".slider").each(function (i, el) {
                var $this = $j1112(el),
                    $label_1 = $j1112('<span class="ui-label"></span>'),
                    $label_2 = $label_1.clone(),

                    orientation = getValue($this, 'vertical', 0) != 0 ? 'vertical' : 'horizontal',

                    prefix = getValue($this, 'prefix', ''),
                    postfix = getValue($this, 'postfix', ''),

                    fill = getValue($this, 'fill', ''),
                    $fill = $j1112(fill),

                    step = getValue($this, 'step', 1),
                    value = getValue($this, 'value', 5),
                    min = getValue($this, 'min', 0),
                    max = getValue($this, 'max', 100),
                    min_val = getValue($this, 'min-val', 10),
                    max_val = getValue($this, 'max-val', 90),

                    is_range = $this.is('[data-min-val]') || $this.is('[data-max-val]'),

                    reps = 0;


                // Range Slider Options
                if (is_range) {
                    $this.slider({
                        range: true,
                        orientation: orientation,
                        min: min,
                        max: max,
                        values: [min_val, max_val],
                        step: step,
                        slide: function (e, ui) {
                            var min_val = (prefix ? prefix : '') + ui.values[0] + (postfix ? postfix : ''),
                                max_val = (prefix ? prefix : '') + ui.values[1] + (postfix ? postfix : '');

                            $label_1.html(min_val);
                            $label_2.html(max_val);

                            if (fill)
                                $fill.val(min_val + ',' + max_val);

                            reps++;
                        },
                        change: function (ev, ui) {
                            if (reps == 1) {
                                var min_val = (prefix ? prefix : '') + ui.values[0] + (postfix ? postfix : ''),
                                    max_val = (prefix ? prefix : '') + ui.values[1] + (postfix ? postfix : '');

                                $label_1.html(min_val);
                                $label_2.html(max_val);

                                if (fill)
                                    $fill.val(min_val + ',' + max_val);
                            }

                            reps = 0;
                        }
                    });

                    var $handles = $this.find('.ui-slider-handle');

                    $label_1.html((prefix ? prefix : '') + min_val + (postfix ? postfix : ''));
                    $handles.first().append($label_1);

                    $label_2.html((prefix ? prefix : '') + max_val + (postfix ? postfix : ''));
                    $handles.last().append($label_2);
                }
                // Normal Slider
                else {

                    $this.slider({
                        range: getValue($this, 'basic', 0) ? false : "min",
                        orientation: orientation,
                        min: min,
                        max: max,
                        value: value,
                        step: step,
                        slide: function (ev, ui) {
                            var val = (prefix ? prefix : '') + ui.value + (postfix ? postfix : '');

                            $label_1.html(val);


                            if (fill)
                                $fill.val(val);

                            reps++;
                        },
                        change: function (ev, ui) {
                            if (reps == 1) {
                                var val = (prefix ? prefix : '') + ui.value + (postfix ? postfix : '');

                                $label_1.html(val);

                                if (fill)
                                    $fill.val(val);
                            }

                            reps = 0;
                        }
                    });

                    var $handles = $this.find('.ui-slider-handle');
                    //$fill = $j1112('<div class="ui-fill"></div>');

                    $label_1.html((prefix ? prefix : '') + value + (postfix ? postfix : ''));
                    $handles.html($label_1);

                    //$handles.parent().prepend( $fill );

                    //$fill.width($handles.get(0).style.left);
                }

            })
        }



        /*------------- Color Slider widget---------------*/

        function hexFromRGB(r, g, b) {
            var hex = [
                r.toString(16),
                g.toString(16),
                b.toString(16)
            ];
            $.each(hex, function (nr, val) {
                if (val.length === 1) {
                    hex[nr] = "0" + val;
                }
            });
            return hex.join("").toUpperCase();
        }

        function refreshSwatch() {
            var red = $j1112("#slider-red").slider("value"),
                green = $j1112("#slider-green").slider("value"),
                blue = $j1112("#slider-blue").slider("value"),
                hex = hexFromRGB(red, green, blue);
            $j1112("#slider-swatch").css("background-color", "#" + hex);
        }


        if ($.isFunction($.fn.slider)) {

            $j1112(function () {
                $j1112("#slider-red, #slider-green, #slider-blue").slider({
                    orientation: "horizontal",
                    range: "min",
                    max: 255,
                    value: 127,
                    slide: refreshSwatch,
                    change: refreshSwatch
                });
                $j1112("#slider-red").slider("value", 235);
                $j1112("#slider-green").slider("value", 70);
                $j1112("#slider-blue").slider("value", 60);
            });
        }



        /*-------------------------------------*/

        /*--------------------------------*/


        // Spinner
        if ($.isFunction($.fn.spinner)) {

            $j1112("#spinner").spinner();

            $j1112("#spinner2").spinner({
                min: 5,
                max: 2500,
                step: 25,
                start: 1000,
                numberFormat: "C"
            });


            $j1112("#spinner3").spinner({
                spin: function (event, ui) {
                    if (ui.value > 10) {
                        $j1112(this).spinner("value", -10);
                        return false;
                    } else if (ui.value < -10) {
                        $j1112(this).spinner("value", 10);
                        return false;
                    }
                }
            });
        }
        /*------------------------------------*/

        // tagsinput
        if ($.isFunction($.fn.tagsinput)) {

            // categorize tags input
            var i = -1,
                colors = ['primary', 'info', 'warning', 'success'];

            colors = shuffleArray(colors);

            $j1112("#tagsinput-2").tagsinput({
                tagClass: function () {
                    i++;
                    return "label label-" + colors[i % colors.length];
                }
            });


            $j1112(".mail_compose_to").tagsinput({
                tagClass: function () {
                    i++;
                    return "label label-" + colors[i % colors.length];
                }
            });


        }

        // Just for demo purpose
        function shuffleArray(array) {
            for (var i = array.length - 1; i > 0; i--) {
                var j = Math.floor(Math.random() * (i + 1));
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
            return array;
        }

        /*----------------------------*/


        // datepicker
        if ($.isFunction($.fn.datepicker)) {
            $j1112(".datepicker").each(function (i, e) {
                var $this = $j1112(e),
                    options = {
                        minViewMode: getValue($this, 'minViewMode', 0),
                        format: getValue($this, 'format', 'mm/dd/yyyy'),
                        startDate: getValue($this, 'startDate', ''),
                        endDate: getValue($this, 'endDate', ''),
                        daysOfWeekDisabled: getValue($this, 'disabledDays', ''),
                        startView: getValue($this, 'startView', 0)
                    },
                    $nxt = $this.next(),
                    $prv = $this.prev();


                $this.datepicker(options);

                if ($nxt.is('.input-group-addon') && $nxt.has('a')) {
                    $nxt.on('click', function (ev) {
                        ev.preventDefault();
                        $this.datepicker('show');
                    });
                }

                if ($prv.is('.input-group-addon') && $prv.has('a')) {
                    $prv.on('click', function (ev) {
                        ev.preventDefault();

                        $this.datepicker('show');
                    });
                }
            });
        }



        /*-------------------------------------------*/



        // Date Range Picker
        if ($.isFunction($.fn.daterangepicker)) {
            $j1112(".daterange").each(function (i, e) {
                // Change the range as you desire
                var ranges = {
                    'Today': [moment(), moment()],
                    'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
                    'Last 7 Days': [moment().subtract('days', 6), moment()],
                    'Last 30 Days': [moment().subtract('days', 29), moment()],
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
                };

                var $this = $j1112(e),
                    options = {
                        format: getValue($this, 'format', 'MM/DD/YYYY'),
                        timePicker: getValue($this, 'timePicker', false),
                        timePickerIncrement: getValue($this, 'timePickerIncrement', false),
                        separator: getValue($this, 'separator', ' - '),
                    },
                    min_date = getValue($this, 'minDate', ''),
                    max_date = getValue($this, 'maxDate', ''),
                    start_date = getValue($this, 'startDate', ''),
                    end_date = getValue($this, 'endDate', '');

                if ($this.hasClass('add-date-ranges')) {
                    options['ranges'] = ranges;
                }

                if (min_date.length) {
                    options['minDate'] = min_date;
                }

                if (max_date.length) {
                    options['maxDate'] = max_date;
                }

                if (start_date.length) {
                    options['startDate'] = start_date;
                }

                if (end_date.length) {
                    options['endDate'] = end_date;
                }


                $this.daterangepicker(options, function (start, end) {
                    var drp = $this.data('daterangepicker');

                    if ($this.hasClass('daterange-text')) {
                        //                        $this.find('span').html(start.format(drp.format) + drp.separator + end.format(drp.format));
                        $this.find('span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
                    }
                });

                if (typeof options['ranges'] == 'object') {
                    $this.data('daterangepicker').container.removeClass('show-calendar');
                }
            });
        }




        /*-------------------------------------*/


        // Timepicker
        if ($.isFunction($.fn.timepicker)) {
            $j1112(".timepicker").each(function (i, e) {
                var $this = $j1112(e),
                    options = {
                        template: getValue($this, 'template', false),
                        showSeconds: getValue($this, 'showSeconds', false),
                        defaultTime: getValue($this, 'defaultTime', 'current'),
                        showMeridian: getValue($this, 'showMeridian', true),
                        minuteStep: getValue($this, 'minuteStep', 15),
                        secondStep: getValue($this, 'secondStep', 15)
                    },
                    $nxt = $this.next(),
                    $prv = $this.prev();

                $this.timepicker(options);

                if ($nxt.is('.input-group-addon') && $nxt.has('a')) {
                    $nxt.on('click', function (ev) {
                        ev.preventDefault();

                        $this.timepicker('showWidget');
                    });
                }

                if ($prv.is('.input-group-addon') && $prv.has('a')) {
                    $prv.on('click', function (ev) {
                        ev.preventDefault();

                        $this.timepicker('showWidget');
                    });
                }
            });
        }



        /*-------------------------------------*/


        // DateTimepicker
        if ($.isFunction($.fn.datetimepicker)) {

            $j1112('.form_datetime').datetimepicker({
                //language:  'fr',
                format: "yyyy-mm-dd hh:ii",
                weekStart: 1,
                todayBtn: 1,
                autoclose: 1,
                todayHighlight: 1,
                startView: 2,
                forceParse: 0,
                showMeridian: 0
            });


            $j1112('.form_datetime_meridian').datetimepicker({
                //language:  'fr',
                format: "dd MM yyyy - hh:ii",
                weekStart: 1,
                todayBtn: 1,
                autoclose: 1,
                todayHighlight: 1,
                startView: 2,
                forceParse: 0,
                showMeridian: 1
            });


            $j1112('.form_datetime_lang').datetimepicker({
                language: 'fr',
                format: "yyyy-mm-dd hh:ii",
                weekStart: 1,
                todayBtn: 1,
                autoclose: 1,
                todayHighlight: 1,
                startView: 2,
                forceParse: 0,
                showMeridian: 0
            });


            /*    $j1112('.form_date').datetimepicker({
                    weekStart: 1,
                    todayBtn:  1,
                    autoclose: 1,
                    todayHighlight: 1,
                    startView: 2,
                    minView: 2,
                    forceParse: 0
                });
                $j1112('.form_time').datetimepicker({
                    //language:  'fr',
                    weekStart: 1,
                    todayBtn:  1,
                    autoclose: 1,
                    todayHighlight: 1,
                    startView: 1,
                    minView: 0,
                    maxView: 1,
                    forceParse: 0
                });*/

        }

        /*-------------------------------------*/





        // Colorpicker
        if ($.isFunction($.fn.colorpicker)) {
            $j1112(".colorpicker").each(function (i, e) {
                var $this = $j1112(e),
                    options = {},
                    $nxt = $this.next(),
                    $prv = $this.prev(),
                    $view = $this.siblings('.input-group-addon').find('.sel-color');

                $this.colorpicker(options);

                if ($nxt.is('.input-group-addon') && $nxt.has('a')) {
                    $nxt.on('click', function (ev) {
                        ev.preventDefault();

                        $this.colorpicker('show');
                    });
                }

                if ($prv.is('.input-group-addon') && $prv.has('a')) {
                    $prv.on('click', function (ev) {
                        ev.preventDefault();

                        $this.colorpicker('show');
                    });
                }

                if ($view.length) {
                    $this.on('changeColor', function (ev) {

                        $view.css('background-color', ev.color.toHex());
                    });

                    if ($this.val().length) {
                        $view.css('background-color', $this.val());
                    }
                }
            });
        }


        /*--------------------------------------*/


        // select2
        if ($.isFunction($.fn.select2)) {

            $j1112("#s2example-1").select2({
                placeholder: 'Select your country...',
                allowClear: true
            }).on('select2-open', function () {
                // Adding Custom Scrollbar
                $j1112(this).data('select2').results.addClass('overflow-hidden').perfectScrollbar();
            });


            $j1112("#s2example-2").select2({
                placeholder: 'Choose your favorite US Countries',
                allowClear: true
            }).on('select2-open', function () {
                // Adding Custom Scrollbar
                $j1112(this).data('select2').results.addClass('overflow-hidden').perfectScrollbar();
            });


            $j1112("#s2example-4").select2({
                minimumInputLength: 1,
                placeholder: 'Search',
                ajax: {
                    url: "data/select2-remote-data.php",
                    dataType: 'json',
                    quietMillis: 100,
                    data: function (term, page) {
                        return {
                            limit: -1,
                            q: term
                        };
                    },
                    results: function (data, page) {
                        return {
                            results: data
                        }
                    }
                },
                formatResult: function (student) {
                    return "<div class='select2-user-result'>" + student.name + "</div>";
                },
                formatSelection: function (student) {
                    return student.name;
                }

            });
        }
        /*------------------------------------*/




        //multiselect start

        if ($.isFunction($.fn.multiSelect)) {

            $j1112('#my_multi_select1').multiSelect();
            $j1112('#my_multi_select2').multiSelect({
                selectableOptgroup: true
            });

            $j1112('#my_multi_select3').multiSelect({
                selectableHeader: "<input type='text' class='form-control search-input' autocomplete='off' placeholder='search...'>",
                selectionHeader: "<input type='text' class='form-control search-input' autocomplete='off' placeholder='search...'>",
                afterInit: function (ms) {
                    var that = this,
                        $selectableSearch = that.$selectableUl.prev(),
                        $selectionSearch = that.$selectionUl.prev(),
                        selectableSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selectable:not(.ms-selected)',
                        selectionSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selection.ms-selected';

                    that.qs1 = $selectableSearch.quicksearch(selectableSearchString)
                        .on('keydown', function (e) {
                            if (e.which === 40) {
                                that.$selectableUl.focus();
                                return false;
                            }
                        });

                    that.qs2 = $selectionSearch.quicksearch(selectionSearchString)
                        .on('keydown', function (e) {
                            if (e.which == 40) {
                                that.$selectionUl.focus();
                                return false;
                            }
                        });
                },
                afterSelect: function () {
                    this.qs1.cache();
                    this.qs2.cache();
                },
                afterDeselect: function () {
                    this.qs1.cache();
                    this.qs2.cache();
                }
            });

        }
        //multiselect end









        /*---------------------------------------*/


        if ($.isFunction($.fn.typeahead)) {

            // basic typeahead

            var substringMatcher = function (strs) {
                return function findMatches(q, cb) {
                    var matches, substrRegex;

                    // an array that will be populated with substring matches
                    matches = [];

                    // regex used to determine if a string contains the substring `q`
                    substrRegex = new RegExp(q, 'i');

                    // iterate through the pool of strings and for any string that
                    // contains the substring `q`, add it to the `matches` array
                    $.each(strs, function (i, str) {
                        if (substrRegex.test(str)) {
                            // the typeahead jQuery plugin expects suggestions to a
                            // JavaScript object, refer to typeahead docs for more info
                            matches.push({
                                value: str
                            });
                        }
                    });

                    cb(matches);
                };
            };

            var states = ['Alabama', 'Alaska', 'Arizona', 'Arkansas', 'California',
                'Colorado', 'Connecticut', 'Delaware', 'Florida', 'Georgia', 'Hawaii',
                'Idaho', 'Illinois', 'Indiana', 'Iowa', 'Kansas', 'Kentucky', 'Louisiana',
                'Maine', 'Maryland', 'Massachusetts', 'Michigan', 'Minnesota',
                'Mississippi', 'Missouri', 'Montana', 'Nebraska', 'Nevada', 'New Hampshire',
                'New Jersey', 'New Mexico', 'New York', 'North Carolina', 'North Dakota',
                'Ohio', 'Oklahoma', 'Oregon', 'Pennsylvania', 'Rhode Island',
                'South Carolina', 'South Dakota', 'Tennessee', 'Texas', 'Utah', 'Vermont',
                'Virginia', 'Washington', 'West Virginia', 'Wisconsin', 'Wyoming'
            ];

            $j1112('#typeahead-1').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
            }, {
                name: 'states',
                displayKey: 'value',
                source: substringMatcher(states)
            });



            // prefetch typeahead

            var names = new Bloodhound({
                datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                limit: 10,
                prefetch: {
                    url: 'data/names.json',
                    filter: function (list) {
                        return $.map(list, function (name) {
                            return {
                                name: name
                            };
                        });
                    }
                }
            });

            names.initialize();

            $j1112('#typeahead-2').typeahead(null, {
                name: 'names',
                displayKey: 'name',
                source: names.ttAdapter()
            });


            // remote data


            var name_randomizer = new Bloodhound({
                datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                // You can also prefetch suggestions
                // prefetch: 'data/typeahead-generate.php',
                remote: 'data/typeahead-generate.php?q=%QUERY'
            });

            name_randomizer.initialize();

            $j1112('#typeahead-3').typeahead({
                hint: true,
                highlight: true
            }, {
                name: 'string-randomizer',
                displayKey: 'value',
                source: name_randomizer.ttAdapter()
            });


            // templating

            var oscar_movies = new Bloodhound({
                datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                remote: 'data/typeahead-hp-movies.php?q=%QUERY'
            });

            oscar_movies.initialize();

            $j1112('#typeahead-4').typeahead(null, {
                name: 'oscar-movies',
                displayKey: 'value',
                source: oscar_movies.ttAdapter(),
                templates: {
                    empty: [
                        '<div class="empty-message">',
                        'We cannot find this movie title',
                        '</div>'
                    ].join('\n'),
                    suggestion: Handlebars.compile('<div class="clearfix" style="width:100%;display:inline-block;min-height:60px;height:auto;"><img src="data/{{cover}}" class="img-responsive pull-left" width="30" style="margin-right:10px;" /><strong>{{value}}</strong> &mdash; {{year}}<br /><span style="display:inline-block; height: 22px; overflow: hidden; white-space:nowrap; text-overflow:ellipsis; max-width: 400px;"></span></div>')
                }
            })
                .bind('typeahead:opened', function () {
                    $j1112(this).data('ttTypeahead').dropdown.$menu.addClass('overflow-hidden').perfectScrollbar();
                })
                .on('keyup', function () {
                    $j1112(this).data('ttTypeahead').dropdown.$menu.perfectScrollbar('update');
                });

        }
        /*------------------------------------*/



        /*------------------------------------------*/

    };



    /*--------------------------------
        Widgets
     --------------------------------*/
    CMPLTADMIN_SETTINGS.cmpltadminWidgets = function () {

        /*notification widget*/
        var notif_widget = $j1112(".notification-widget").height();
        $j1112('.notification-widget').height(notif_widget).perfectScrollbar({
            suppressScrollX: true
        });

    };



    /*--------------------------------
        weather widget
     --------------------------------*/
    CMPLTADMIN_SETTINGS.cmpltadminWidgetWeather = function () {

        /*notification widget*/
        /*var wid = $j1112(".wid-weather");
        var notif_widget = $j1112(".notification-widget").height();
        $j1112('.notification-widget').height(notif_widget).perfectScrollbar({suppressScrollX: true});

        $j1112('.wid-weather').each( function () {
                var days = $j1112(this).find(".weekdays");
                var today = $j1112(this).find(".today");

                var height = days.height();
                if(days.height() < today.height()){
                    height = today.height();
                }

                days.height(height);
                today.height(height);
        });*/


        $j1112('.wid-weather .weekdays ul').perfectScrollbar({
            suppressScrollX: true
        });


    };





    /*--------------------------------
        To Do Task Widget
     --------------------------------*/
    CMPLTADMIN_SETTINGS.cmpltadminToDoWidget = function () {

        /*todo task widget*/
        $j1112(".icheck-minimal-white.todo-task").on('ifChecked', function (event) {
            $j1112(this).parent().parent().addClass("checked");
        });
        $j1112(".icheck-minimal-white.todo-task").on('ifUnchecked', function (event) {
            $j1112(this).parent().parent().removeClass("checked");
        });

        $j1112(".wid-all-tasks ul").perfectScrollbar({
            suppressScrollX: true
        });

    };



    /*--------------------------------
        To Do Add Task Widget
     --------------------------------*/
    CMPLTADMIN_SETTINGS.cmpltadminToDoAddTaskWidget = function () {

        $j1112(".wid-add-task input").on('keypress', function (e) {
            if (e.keyCode == 13) {
                var i = Math.random().toString(36).substring(7);
                var msg = $j1112(this).val();
                var msg = '<li><input type="checkbox" id="task-' + i + '" class="icheck-minimal-white todo-task"><label class="icheck-label form-label" for="task-' + i + '">' + msg + '</label></li>';
                $j1112(this).parent().parent().find(".wid-all-tasks ul").append(msg);
                $j1112(this).val("");
                $j1112(this).focus();
                CMPLTADMIN_SETTINGS.iCheck();
                CMPLTADMIN_SETTINGS.cmpltadminToDoWidget();
                $j1112(this).parent().parent().find(".wid-all-tasks ul").perfectScrollbar('update');
            }
        });

    };







    /*--------------------------------
         Vector maps
     --------------------------------*/
    CMPLTADMIN_SETTINGS.dbjvectorMap = function () {

        if ($.isFunction($.fn.vectorMap)) {
            //@code_start
            $j1112(function () {
                $j1112('#db-world-map-markers').vectorMap({
                    map: 'world_mill_en',
                    scaleColors: ['#363537', '#363537'],
                    normalizeFunction: 'polynomial',
                    hoverOpacity: 0.7,
                    hoverColor: false,
                    regionsSelectable: true,
                    markersSelectable: true,
                    markersSelectableOne: true,
                    updateSize: true,
                    onRegionOver: function (event, code) {
                        //console.log('region-over', code);
                    },
                    onRegionOut: function (event, code) {
                        //console.log('region-out', code);
                    },
                    onRegionClick: function (event, code) {
                        //console.log('region-click', code);
                    },
                    onRegionSelected: function (event, code, isSelected, selectedRegions) {
                        //console.log('region-select', code, isSelected, selectedRegions);
                        if (window.localStorage) {
                            window.localStorage.setItem(
                                'jvectormap-selected-regions',
                                JSON.stringify(selectedRegions)
                            );
                        }
                    },

                    panOnDrag: true,

                    focusOn: {
                        x: 0.5,
                        y: 0.5,
                        scale: 1.2,
                        animate: true
                    },


                    regionStyle: {
                        initial: {
                            fill: '#aaaaaa',
                            'fill-opacity': 1,
                            stroke: 'false',
                            'stroke-width': 0,
                            'stroke-opacity': 1
                        },
                        hover: {
                            fill: '#363537',
                            'fill-opacity': 1,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#363537'
                        },
                        selectedHover: {}
                    },



                    markerStyle: {
                        initial: {
                            fill: '#E91E63',
                            stroke: '#ffffff',
                            r: 5
                        },
                        hover: {
                            stroke: '#FFC107',
                            "stroke-width": 2,
                            cursor: 'pointer'
                        },
                        selected: {
                            fill: '#FFC107',
                            "stroke-width": 0,
                        },
                    },
                    backgroundColor: '#ffffff',
                    markers: [{
                        latLng: [41.90, 12.45],
                        name: 'Vatican City'
                    }, {
                        latLng: [43.73, 7.41],
                        name: 'Monaco'
                    }, {
                        latLng: [-0.52, 166.93],
                        name: 'Nauru'
                    }, {
                        latLng: [-8.51, 179.21],
                        name: 'Tuvalu'
                    }, {
                        latLng: [43.93, 12.46],
                        name: 'San Marino'
                    }, {
                        latLng: [47.14, 9.52],
                        name: 'Liechtenstein'
                    }, {
                        latLng: [7.11, 171.06],
                        name: 'Marshall Islands'
                    }, {
                        latLng: [17.3, -62.73],
                        name: 'Saint Kitts and Nevis'
                    }, {
                        latLng: [3.2, 73.22],
                        name: 'Maldives'
                    }, {
                        latLng: [35.88, 14.5],
                        name: 'Malta'
                    }, {
                        latLng: [12.05, -61.75],
                        name: 'Grenada'
                    }, {
                        latLng: [13.16, -61.23],
                        name: 'Saint Vincent and the Grenadines'
                    }, {
                        latLng: [13.16, -59.55],
                        name: 'Barbados'
                    }, {
                        latLng: [17.11, -61.85],
                        name: 'Antigua and Barbuda'
                    }, {
                        latLng: [-4.61, 55.45],
                        name: 'Seychelles'
                    }, {
                        latLng: [7.35, 134.46],
                        name: 'Palau'
                    }, {
                        latLng: [42.5, 1.51],
                        name: 'Andorra'
                    }, {
                        latLng: [14.01, -60.98],
                        name: 'Saint Lucia'
                    }, {
                        latLng: [6.91, 158.18],
                        name: 'Federated States of Micronesia'
                    }, {
                        latLng: [1.3, 103.8],
                        name: 'Singapore'
                    }, {
                        latLng: [1.46, 173.03],
                        name: 'Kiribati'
                    }, {
                        latLng: [-21.13, -175.2],
                        name: 'Tonga'
                    }, {
                        latLng: [15.3, -61.38],
                        name: 'Dominica'
                    }, {
                        latLng: [-20.2, 57.5],
                        name: 'Mauritius'
                    }, {
                        latLng: [26.02, 50.55],
                        name: 'Bahrain'
                    }, {
                        latLng: [0.33, 6.73],
                        name: 'São Tomé and Príncipe'
                    }]
                });
            });
            //@code_end
        }

    };



    /*--------------------------------
        IOS7 Switchery
     --------------------------------*/
    CMPLTADMIN_SETTINGS.ios7Switchery = function () {

        if ($j1112(".js-switch").length > 0) {

            var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
            var defaults = {
                color: '#17a0d9'
                , secondaryColor: '#dfdfdf'
                , jackColor: '#fff'
                , jackSecondaryColor: null
                , className: 'switchery'
                , disabled: false
                , disabledOpacity: 0.5
                , speed: '0.5s'
                , size: 'large'
            }
            var count = 0;
            var colors = ['#f44336', '#e91e63', '#9c27b0', '#673ab7', '#363537', '#2196f3', '#03a9f4', '#00bcd4', '#009688', '#4caf50', '#8bc34a', '#cddc39', '#ffeb3b', '#ffc107', '#ff9800', '#ff5722', '#795548', '#9e9e9e', '#607d8b', '#000000'];
            elems.forEach(function (html) {
                count = count + 1;
                var size = 'default';
                var color = colors[count];
                if (count > 20) {
                    var size = 'large';
                    var color = colors[count - 20];
                }
                if (count > 40) {
                    var size = 'small';
                    var color = colors[count - 40];
                }
                var defaults = {
                    color: color
                    , secondaryColor: '#dfdfdf'
                    , jackColor: '#fff'
                    , jackSecondaryColor: null
                    , className: 'switchery'
                    , disabled: false
                    , disabledOpacity: 0.5
                    , speed: '0.5s'
                    , size: size
                }

                var switchery = new Switchery(html, defaults);
            });
        }


    };





    /*--------------------------------
        Sparkline Chart - Widgets
     --------------------------------*/
    CMPLTADMIN_SETTINGS.widgetSparklineChart = function () {

        if ($.isFunction($.fn.sparkline)) {

            $j1112('.wid_dynamicbar').sparkline([8.4, 9, 8.8, 8, 9.5, 9.2, 9.9, 9, 9, 8, 7, 8, 9, 8, 7, 9, 9, 9.5, 8, 9.5, 9.8], {
                type: 'bar',
                barColor: '#f5f5f5',
                height: '60',
                barWidth: '12',
                barSpacing: 1,
            });

            $j1112('.wid_linesparkline').sparkline([2000, 3454, 5454, 2323, 3432, 4656, 2897, 3545, 4232, 4656, 2897, 3545, 4232, 5434, 4656, 3567, 4878, 3676, 3787], {
                type: 'line',
                width: '100%',
                height: '60',
                lineWidth: 2,
                lineColor: '#f5f5f5',
                fillColor: 'rgba(255,255,255,0.2)',
                highlightSpotColor: '#ffffff',
                highlightLineColor: '#ffffff',
                spotRadius: 3,
            });


            // Bar + line composite charts
            $j1112('.wid_compositebar').sparkline([4, 6, 7, 7, 4, 3, 2, 4, 6, 7, 7, 8, 8, 4, 4, 3, 1, 4, 6, 5, 9], {
                type: 'bar',
                barColor: '#f5f5f5',
                height: '60',
                barWidth: '12',
                barSpacing: 1,
            });

            $j1112('.wid_compositebar').sparkline([4, 1, 5, 7, 9, 9, 8, 8, 4, 7, 8, 4, 7, 9, 9, 8, 8, 4, 2, 5, 6, 7], {
                composite: true,
                fillColor: 'rgba(103,58,183,0)',
                type: 'line',
                width: '100%',
                height: '40',
                lineWidth: 2,
                lineColor: '#673AB7',
                highlightSpotColor: '#E91E63',
                highlightLineColor: '#673AB7',
                spotRadius: 3,
            });



        }

    };








    // Element Attribute Helper
    function getValue($el, data_var, default_val) {
        if (typeof $el.data(data_var) != 'undefined') {
            return $el.data(data_var);
        }

        return default_val;
    }


    /******************************
     initialize respective scripts 
     *****************************/
    $j1112(document).ready(function () {
        CMPLTADMIN_SETTINGS.windowBasedLayout();
        CMPLTADMIN_SETTINGS.mainmenuScroll();
        CMPLTADMIN_SETTINGS.mainMenu();
        CMPLTADMIN_SETTINGS.mainmenuCollapsed();
        CMPLTADMIN_SETTINGS.pageTopBar();
        CMPLTADMIN_SETTINGS.otherScripts();
        CMPLTADMIN_SETTINGS.iCheck();
        CMPLTADMIN_SETTINGS.customDropZone();
        CMPLTADMIN_SETTINGS.formEditors();
        CMPLTADMIN_SETTINGS.extraFormSettings();
        CMPLTADMIN_SETTINGS.tooltipsPopovers();
        CMPLTADMIN_SETTINGS.nestableList();
        CMPLTADMIN_SETTINGS.uiCalendar();
        CMPLTADMIN_SETTINGS.tocifyScrollMenu();
        CMPLTADMIN_SETTINGS.loadPrettyPhoto();
        CMPLTADMIN_SETTINGS.jvectorMaps();
        //CMPLTADMIN_SETTINGS.dataTablesInit();
        CMPLTADMIN_SETTINGS.jsTreeINIT();
        CMPLTADMIN_SETTINGS.breadcrumbAutoHidden();
        CMPLTADMIN_SETTINGS.chatAPI();
        CMPLTADMIN_SETTINGS.chatApiScroll();
        CMPLTADMIN_SETTINGS.chatApiWindow();
        CMPLTADMIN_SETTINGS.mailboxInbox();
        CMPLTADMIN_SETTINGS.cmpltadminWidgets();
        CMPLTADMIN_SETTINGS.sectionBoxActions();
        CMPLTADMIN_SETTINGS.draggablePanels();
        CMPLTADMIN_SETTINGS.viewportElement();
        CMPLTADMIN_SETTINGS.searchPage();
        CMPLTADMIN_SETTINGS.cmpltadminToDoAddTaskWidget();
        CMPLTADMIN_SETTINGS.cmpltadminToDoWidget();
        CMPLTADMIN_SETTINGS.dbjvectorMap();
        CMPLTADMIN_SETTINGS.widgetSparklineChart();
        CMPLTADMIN_SETTINGS.cmpltadminWidgetWeather();
        CMPLTADMIN_SETTINGS.onLoadTopBar();
        CMPLTADMIN_SETTINGS.ios7Switchery();
    });

    $j1112(window).resize(function () {
        CMPLTADMIN_SETTINGS.windowBasedLayout();
        //CMPLTADMIN_SETTINGS.mainmenuScroll();
        //CMPLTADMIN_SETTINGS.cmpltadminWidgetWeather();
        CMPLTADMIN_SETTINGS.isotopeGallery();
        CMPLTADMIN_SETTINGS.loginPage();
        CMPLTADMIN_SETTINGS.widgetSparklineChart();
    });

    $j1112(window).load(function () {
        CMPLTADMIN_SETTINGS.isotopeGallery();
        CMPLTADMIN_SETTINGS.loginPage();
    });

});
