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
        var jsonContent = "<textarea class='json-content' id='json-content-" + count + "'>" + message.body + "</textarea>";
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
            const input = document.getElementById('json-content-' + count);
            // 2) Select the text
            input.focus();
            input.select();

            // 3) Copy text to clipboard
            const isSuccessful = document.execCommand('copy');
        };
    }, 1000);
    
});