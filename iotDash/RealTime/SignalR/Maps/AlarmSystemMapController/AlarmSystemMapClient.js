

//signalR
var connection = $.hubConnection();
var contosoChatHubProxy = connection.createHubProxy('AlarmSystemMapHub');
contosoChatHubProxy.on('updateDevice', function (deviceData) {
    var obj = jQuery.parseJSON(deviceData);
    var str = JSON.stringify(obj, undefined, 4);

    console.log("Recieved : " + str);

    $.each(obj, function () {
        var cnode = $(this);
        if (cnode[0]['PropertyId'] != null) {   //is a property
            var PropId = cnode[0]['PropertyId'];
            var ResultParams = cnode[0]['ResultParameters'];
            var ResVal = ResultParams[0]['Value'];
            var ValDispIdHash = "#propvaldisp-" + PropId;
            var ValDispId = "propvaldisp-" + PropId;
            var element = document.getElementById(ValDispId);
            element.innerHTML = ResVal.toString();
            element.setAttribute("title", ResVal.toString());

        }
        else if (cnode[0]['ActionId'] != null) {    // is an action
            var ActId = cnode[0]['ActionId'];
            var ResultParams = cnode[0]['ResultParameters'];
            var ResVal = ResultParams[0]['Value'];
            var ValDispIdHash = "#actvaldisp-" + ActId;
            var ValDispId = "actvaldisp-" + ActId;
            var element = document.getElementById(ValDispId);
            element.innerHTML = ResVal.toString();
            element.setAttribute("title", ResVal.toString());

        }
    });
    ReloadPanels();


});


connection.start()
    .done(function () { console.log('SignalR Now connected, connection ID=' + connection.id); })
    .fail(function () { console.log('SignalR Could not connect'); });



$(function () {

   

});