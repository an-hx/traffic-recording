"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/trafficHub").build();

$('#loading').text('starting...');
connection.start().then(function () {
    $('#loading').text('Waiting for messages...');
}).catch(function (err) {
    $('#loading').text('error');
    return console.error(err.toString());
});


connection.on("ReceiveMessage", function (message) {
    var count = $("#http-log tbody tr").length;

    var row = $("<tr/>");
    var dateCol = $("<td/>").text((new Date()).toISOString());
    var methodCol = $("<td/>").text(message.method);
    var urlCol = $("<td/>").text(message.url);
    if (message.body.length) {
        var jsonContent = "<pre class='json-content' id='json-content-" + count + "'>" + message.body + "</pre>";
        jsonContent += "</br><a href='javascript:void(0)' id='btn-copy-" + count + "'>Copy</a>";
        var bodyCol = $("<td/>").append(jsonContent);
    } else {
        var bodyCol = $("<td/>").append("");
    }


    row.append(dateCol);
    row.append(methodCol);
    row.append(urlCol);
    row.append(bodyCol);

    $('#http-log tbody').prepend(row);
    setTimeout(function () {
        document.getElementById('btn-copy-' + count).onclick = function (e) {
            //$('#json-content-' + count).select();
            const el = document.createElement('textarea');
            el.className = 'hidden';
            el.value = $('#json-content-' + count).text();
            document.body.appendChild(el);
            el.select();
            document.execCommand('copy');
            document.body.removeChild(el);
        };
        document.getElementById('btn-copy-' + count).addEventListener('copy', function (e) {
            e.preventDefault();
            if (e.clipboardData) {
                e.clipboardData.setData('text/plain', 'custom content');
            } else if (window.clipboardData) {
                window.clipboardData.setData('Text', 'custom content');
            }
        });
    }, 1000);
    
});

//function onCopyClipboard(count) {
//    var $temp = $("<input class='hidden'>");
//    $("body").append($temp);
//    $temp.val($("#json-content-" + count).val()).select();
//    document.execCommand("copy");
//    $temp.remove();
//}