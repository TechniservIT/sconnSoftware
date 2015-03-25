

//signalR
var connection = $.hubConnection();
var contosoChatHubProxy = connection.createHubProxy('deviceStatusUpdaterHub');
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

    ReloadPanels();

    $('.tgsw-action').on('switchChange.bootstrapSwitch', function () {

        if (!Reloading) {

            //get sender id
            var ElementId = $(this).attr("id");
            var AccessorId = "#".concat(ElementId);

            //TODO load req param input lists
            var StateToSet = $(this).bootstrapSwitch('state');
            //alert("req param :" + StateToSet);
            var ParamList = [StateToSet];

            //perform action
            $.ajax({
                url: '@Url.Action("PerformAction", "Device")',
                type: this.method,
                data: {
                    ActionId: $(AccessorId).attr("title"),
                    ActionParams: ParamList
                },
                success: function (result) {
                    $('#resultview').html(result);
                }
            });
        }

    });

    $("#ReloadModelData").click(function () {
    });

    $("#ApplyPropertyStyle").click(function () {
    });

    $('#actionselector a').click(function (e) {
        e.preventDefault()
        $(this).tab('show')
    })


    $("#toolPropHistoryCont").click(function () {
    });



    $("body").ready(function () {
        //remove current classes
    });

});