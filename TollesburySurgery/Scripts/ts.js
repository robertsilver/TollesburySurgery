ts = function () {
    //#region Public functions
    function setupCheckCurrentPasswordOnChange() {
        /// <summary>Local function? No
        /// <para>
        /// Checks the user's current password is correct. 
        /// Does not verify the new passwords.
        /// </para>
        /// </summary>
        $('#CurrentPassword').change(function () {
            var isThisAConfirmationPwd = false;
            var userId = urlParams["u"];
            if (userId !== undefined && userId !== '')
                isThisAConfirmationPwd = true;

            var params = '{\'userId\':\'' + userId + '\', \'userName\':\'' + $('#userName').val() + '\', \'pwd\':\'' + $(this).val() + '\', \'isThisAConfirmationPwd\':\'' + isThisAConfirmationPwd + '\'}';
            $.ajax({
                type: 'POST',
                url: '/WebMethods.aspx/IsExistingPwdValid',
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d == true) {
                        $('#CurrentPassword').removeClass('error-red-border');
                        $('#CurrentPassword').addClass('passwordEntry');
                        $('#lblError').hide();
                        $('#ChangePasswordPushButton').show();
                        $('#user-Id').val(data.d);
                    } else {
                        $('#CurrentPassword').removeClass('passwordEntry');
                        $('#CurrentPassword').addClass('error-red-border');
                        $('#lblError').text('Sorry, but we cannot verify your current password.  Please try again.');
                        $('#lblError').show();
                        $('#ChangePasswordPushButton').hide();
                        $('#user-Id').val('');
                    }
                },
                error: function (e) {
                    var error = e;
                    var temp = '';
                }
            });
        });
    }

    function setupCheckNewPasswordsAreTheSame() {
        /// <summary>Local function? No
        /// <para>
        /// Ensures that the new pwd & confirmation
        /// pwd are identical.
        /// </para>
        /// </summary>
        $('#ConfirmNewPassword').change(function () {
            if ($('#NewPassword').val() == $(this).val()) {
                $('#NewPassword').removeClass('error-red-border');
                $('#NewPassword').addClass('passwordEntry');
                $('#ConfirmNewPassword').removeClass('error-red-border');
                $('#ConfirmNewPassword').addClass('passwordEntry');
                $('#lblError').hide();
            } else {
                $('#NewPassword').removeClass('passwordEntry');
                $('#NewPassword').addClass('error-red-border');
                $('#ConfirmNewPassword').removeClass('passwordEntry');
                $('#ConfirmNewPassword').addClass('error-red-border');
                $('#lblError').text('Your new password & confirmation password do not match.  Please try again.');
                $('#lblError').show();
            }
        });
    }

    function test() {
        $('#ChangePasswordPushButton').click(function (e) {
            if ($('#NewPassword').val() != $('#ConfirmNewPassword').val()) {
                e.preventDefault();
            } else
                return true;
        });
    }

    function setupForgottenPassword() {
        /// <summary>Local function? No
        /// <para>
        /// If the check box is ticked, it checks if the
        /// username field is populated.  If it is, then 
        /// it will hide the password lable and text box.
        /// </para>
        /// </summary> 
        $('#ForgottenPwd').click(function () {
            if ($(this).attr('checked')) {
                ValidatorEnable(document.getElementById('PasswordRequired'), false);
                var visible = false;
                $('#Password').hide();
                $('#PasswordLabel').hide();
                if ($('#UserName').text() === '') {
                    $('#lblError').text('Please enter your email address.');
                    visible = true;
                }

                $('#lblError').visible = visible;
                $('#LoginButton').visible = visible;
            }
            else {
                ValidatorEnable(document.getElementById('PasswordRequired'), true);
                // User has unticked the box, so make the password box visible, 
                // but hide the error label.
                $('#Password').show();
                $('#PasswordLabel').show();
                $('#lblError').hide();
            }
        });
    }

    function setupLoginUserNameOnChange() {
        /// <summary>Local function? No
        /// <para>
        /// </para>
        /// </summary>
        $('#UserName').change(function () {
            var username = '{\'userName\':\'' + $(this).val() + '\'}';
            $.ajax({
                type: 'POST',
                url: '/WebMethods.aspx/CheckUserNameForLogin',
                data: username,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d >= 0) {
                        $('#UserName').removeClass('error-red-border');
                        $('#UserName').addClass('textEntry');
                        $('#lblError').hide();
                        $('#LoginButton').show();
                        $('#user-Id').val(data.d);
                    } else {
                        $('#UserName').removeClass('textEntry');
                        $('#UserName').addClass('error-red-border');
                        $('#lblError').text('Sorry, but we cannot find your email address.  Please try again.');
                        $('#lblError').show();
                        $('#LoginButton').hide();
                        $('#user-Id').val('');
                    }
                },
                error: function (e) {
                    var error = e;
                    var temp = '';
                }
            });
        });
    }

    function setupMasterPage() {
        /// <summary>Local function? No
        /// <para>
        /// </para>
        /// </summary>
        $('.left-menu li').click(function () {
            var link = $(this).attr('data-link');
            var dataId = $(this).attr('data-id').toString();
            if (link !== '' && link !== undefined)
                window.location = link;

            getAndDisplayMenuContent(dataId);
        });

        $('.left-menu li.menu-header[data-header="Yes"]').click(function (event, asd) {
            hideAllMenusThatDoNotBelongToClickedValue(this);
            showAllMenusThatDoBelongToClickedValue(this);
        });

        $('.left-menu li[data-default-item="No"]').hide();

        if (areWeOnDefaultPage() || isTheAddressBarBlankAfterHttp()) {
            rebuildCenterCol();
            getAndDisplayMenuContent('1.0.0');
        }

        if ($('.main-content').val() === '' || $('.main-content').val() === undefined ||
            $('.main-content').text() === '' || $('.main-content').text() === undefined)
            ; // getAndDisplayMenuContent('1');
    }

    //#endregion Public functions

    //#region Private functions
    function areWeOnDefaultPage() {
        /// <summary>Local function? Yes
        /// <para>
        /// </para>
        /// </summary> 
        var path = window.location.pathname;
        var page = path.substring(path.lastIndexOf('/') + 1);
        if (page.toLowerCase() === 'default.aspx')
            return true;

        return false;
    }

    function areWeOnUploadPage() {
        /// <summary>Local function? Yes
        /// <para>
        /// </para>
        /// </summary> 
        var path = window.location.pathname;
        var page = path.substring(path.lastIndexOf('/') + 1);

        if (page.toLowerCase() === 'upload.aspx' || page.toLowerCase() === 'login.aspx' || page.toLowerCase() === 'files.aspx' || page.toLowerCase() === 'friendsandfamily.aspx')
            return true;

        return false;
    }

    function hideAllMenusThatDoNotBelongToClickedValue(sender) {
        /// <summary>Local function? Yes
        /// <para>
        /// Hide values that are not part of the clicked value
        /// </para>
        /// </summary> 
        $('.left-menu li.menu-child[data-parent-data-id!=\"' + $(sender).attr('data-id') + '\"]').each(function () {
            if ($(this).is(':visible') === true) {
                $(this).hide('slow');
            }
        });
    }

    function isTheAddressBarBlankAfterHttp() {
        /// <summary>Local function? Yes
        /// <para>
        /// </para>
        /// </summary> 
        var path = window.location.pathname;
        var page = path.substring(path.lastIndexOf('/') + 1);
        if (page === '')
            return true;

        return false;
    }

    function rebuildCenterCol() {
        /// <summary>Local function? Yes
        /// <para>
        /// </para>
        /// </summary>   
        $('.centercol').text('');
        $('.centercol').append('<div class="main-content"></div>');
    }

    function showAllMenusThatDoBelongToClickedValue(sender) {
        /// <summary>Local function? Yes
        /// <para>
        /// Show values that are part of the clicked value
        /// </para>
        /// </summary>  
        $('.left-menu li.menu-child[data-parent-data-id=\"' + $(sender).attr('data-id') + '\"]').each(function () {
            if ($(this).is(':visible') === false) {
                $(this).show('slow');
            }
        });

        getAndDisplayMenuContent($(sender).attr('data-id'));
    }

    function getAndDisplayMenuContent(dataId) {
        /// <summary>Local function? Yes
        /// <para>
        /// </para>
        /// </summary>
        // By putting \' around the key and value, you're
        // passing over a string.  If you don't, then jQuery
        // assumes you're passing over a number.
        var params = '{\'dataIdValue\':\'' + dataId + '\'}';

        if (areWeOnUploadPage())
            rebuildCenterCol();

        $.ajax({
            type: 'POST',
            url: '/WebMethods.aspx/GetData',
            data: params,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('div.main-content').html('');
                $('div.main-content').html('<img alt="Loading" src="/Content/Images/large-loader-strip.gif"/>');
                setTimeout(function () {
                    var $html = $(data.d.Text);
                    $('div.main-content').html('');
                    $('div.main-content').html($html);
                    document.title = data.d.Title;

                    $(".carousel").slick({
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        dots: true,
                        speed: 1500,
                        autoplay: true,
                        autoplaySpeed: 10000,
                        draggable: false,
                        arrows: false,
                        customPaging: function (a, b) {
                            return '<div class="pager-item"></div>';
                        }
                    });
                }, 500);
            },
            error: function (e) {
                var error = e;
                var temp = '';
            }
        });
    }


    jQuery.popup = {
        show: function (title, message, options) {
            if (!this.initialized) {
                //inject needed elements in DOM
                var domElements = '<div id="LayoutDialogBoxFadedBackground" class="Theme0Bg"></div>';
                domElements += '<div id="LayoutDialogBox"' + 'class="Theme0Bg ThemeB2Border">';
                domElements += '<div id="LayoutDialogBoxHeader"' + 'class="ThemeA1" height="24px"><a id="LayoutDialogBoxClose"><img src="../Content/Images/iconCancelColour_24x24.png" alt="close" width="24px" height="24px" class="ThemeB2Bg" /></a></div>';
                domElements += '<div id="LayoutDialogBoxTitle" class="ThemeB2Bg Theme0"></div>';
                domElements += '<div id="LayoutDialogBoxMessage"></div>';
                domElements += '</div>';

                jQuery('body').append(domElements);
                jQuery('#LayoutDialogBoxFadedBackground').click(function () { hidePopup(); });
                jQuery('#LayoutDialogBoxClose').click(function () { hidePopup(); });

                this.initialized = true;
                //alert('1: ' + this.showing);
            }

            if (!isIPhone() && !false) {
                message = message.replace(/\n/g, "<br/>");

                // prepare popup Layout
                jQuery('#LayoutDialogBoxTitle').html(title);
                jQuery('#LayoutDialogBoxMessage').html(message);

                // display
                showPopup(this.showing);

            } else {
                alert(message);

            }

            // show popup
            function showPopup(showingTemp) {
                //alert('2: ' + this.showing);
                if ((!this.showing) || (!showingTemp)) {
                    if ($.browser.msie && $.browser.version.substr(0, 1) < 7) {
                        jQuery('#dropdownlistSitesOrServices').css({ "opacity": "0.3" });
                    }
                    jQuery('#LayoutDialogBoxFadedBackground').css({ "opacity": "0.6" });
                    jQuery('#LayoutDialogBoxFadedBackground').fadeIn("slow");
                    jQuery('#LayoutDialogBox').fadeIn("slow");
                    this.showing = true;
                }
            }

            // hide popup
            function hidePopup() {
                if (this.showing) {
                    if ($.browser.msie && $.browser.version.substr(0, 1) < 7) {
                        jQuery('#dropdownlistSitesOrServices').css({ "opacity": "1" });
                    }
                    jQuery('#LayoutDialogBoxFadedBackground').fadeOut("normal");
                    jQuery('#LayoutDialogBox').fadeOut("normal");
                    this.showing = false;
                }
            }

            // detects if browser is iPhone/iPod Safari
            function isIPhone() {
                if ((navigator.userAgent.match(/iPhone/i)) || (navigator.userAgent.match(/iPod/i))) {
                    return true;
                }

                //return false;
            }

            return jQuery;

        }
    }; // end show function
    


    var urlParams = {};
    (function () {
        var e,
        a = /\+/g,  // Regex for replacing addition symbol with a space
        r = /([^&=]+)=?([^&]*)/g,
        d = function (s) { return decodeURIComponent(s.replace(a, " ")); },
        q = window.location.search.substring(1);

        while (e = r.exec(q))
            urlParams[d(e[1])] = d(e[2]);
    })();
    //#endregion Private functions

    //#region Setup methods
    return {
        setupCheckCurrentPasswordOnChange: setupCheckCurrentPasswordOnChange,
        setupCheckNewPasswordsAreTheSame: setupCheckNewPasswordsAreTheSame,
        setupForgottenPassword: setupForgottenPassword,
        setupLoginUserNameOnChange: setupLoginUserNameOnChange,
        setupMasterPage: setupMasterPage,
        test: test
    }
    //#endregion Setup methods
} ();