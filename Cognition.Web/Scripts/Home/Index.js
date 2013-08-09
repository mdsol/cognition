$(function() {
    var updater = $.connection.pageUpdate;
    function init() {
        console.log("Connected to signalr hub");
        updater.server.joinGroup("all");
    };
    updater.client.allChangeNotification = function(changeId) {
        console.log("Change Id " + changeId);
        $.ajax({
            url: window.documentPartialUri.replace("[id]", changeId),   
            success: function(response) {
                $(response).hide().prependTo("#changes-all-container").fadeIn();
                jQuery("abbr.timeago").timeago();
            },
            dataType: 'html'
        });
    };

    $.connection.hub.start().done(init);
});