$(function() {
    var updater = $.connection.pageUpdate;

    function init() {
        console.log("Connected to signalr hub");
        updater.server.joinGroup(window.documentId);
    };

    updater.client.pageUpdated = function(type, id) {
        console.log("Updated with type " + type + " and id " + id);
        $("#document-container").load(window.documentPartialUri);
    };

    

    $.connection.hub.start().done(init);
});