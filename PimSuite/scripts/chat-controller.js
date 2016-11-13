var chat = $.connection.chatHub;
$(document).ready(function() {
    var chat = $.connection.chatHub;

    $.connection.hub.start().done(function () {
        chat.server.connect($('#fullName').val(), $('#userId').val());
    });

    $('#sendmessage').click(function () {
        chat.server.sendMessage($('input#userId').val(), $('#message').val());
        $('#message').val('').focus();
    });
});

chat.client.onConnected = function(users) {
    $.each(users, function (key, userName) {
        if (!checkIfUserExistOnList(userName)) $('.users-list.clearfix').append('<li>' + userName + '</li>');
    });
}

chat.client.loadChatHistory = function(chatsHistory) {
    $.each(chatsHistory, function (key, chat) {
        $('.direct-chat-messages').append(htmlSendMessage(chat[0], chat[1], chat[2], chat[3]));
    });
}

chat.client.addNewMessageFrom = function(fullName, message, dateNow, senderOrReceiver) {
    $('.direct-chat-messages').append(htmlSendMessage(fullName, message, dateNow, senderOrReceiver));
}

chat.client.addUserMessage = function (fullName, message, dateNow, senderOrReceiver) {
    $('.direct-chat-messages').append(htmlSendMessage(fullName, message, dateNow, senderOrReceiver));
}

chat.client.onNewUserConnected = function (userId, fullName) {
    if (!checkIfUserExistOnList(fullName))
        $('.users-list.clearfix').append('<li>' + fullName + '</li>');
}

chat.client.onUserDisconnected = function(userId) {
    $('li[user-id="' + userId + '"]').remove();
}

function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

function htmlSendMessage(fullName, message, dateNow, senderOrReceiver) {
    var tpl =
        '<div class="direct-chat-msg ';
    if (senderOrReceiver === 'sender') {
        tpl += 'right';
    }

    tpl += '"><div class="direct-chat-info clearfix">';
    if (senderOrReceiver === 'sender') {
        tpl += '<span class="direct-chat-name pull-right">' + fullName + '</span>' +
               '<span class="direct-chat-timestamp pull-left">' + dateNow + '</span>';
    } else {
        tpl += '<span class="direct-chat-name pull-left">' + fullName + '</span>' +
               '<span class="direct-chat-timestamp pull-right">' + dateNow + '</span>';
    }
                
    tpl += '</div>' +
            '<div class="direct-chat-text">' + message + '</div>' +
            '</div>';

    return tpl;
}

function checkIfUserExistOnList(userName) {
    var userExists = false;

    $('.users-list.clearfix li')
        .each(function() {
            if ($(this).text() === userName) userExists = true;
        });

    return userExists;
}