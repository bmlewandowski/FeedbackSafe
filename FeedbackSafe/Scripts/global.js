jQuery(document).ready(function () {

    jQuery(".tab:not(:first)").hide();

    //to fix u know who
    jQuery(".tab:first").show();

    jQuery(".htabs a").click(function () {
        stringref = jQuery(this).attr("href").split('#')[1];

        jQuery('.tab:not(#' + stringref + ')').hide();

        if (jQuery.browser.msie && jQuery.browser.version.substr(0, 3) == "6.0") {
            jQuery('.tab#' + stringref).show();
        }
        else
            jQuery('.tab#' + stringref).fadeIn();

        return false;
    });

});