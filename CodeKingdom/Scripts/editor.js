$(function () {
    var editor = ace.edit("editor"),
        chat = $.connection.chatHub,
        editorHub = $.connection.editorHub,
        file = $.connection.fileHub,
        silent = false;

    editor.setTheme("ace/theme/twilight");
    editor.getSession().setMode("ace/mode/javascript");

    $("form").submit(function () {
        $("#hidden_editor").val(editor.getSession().getValue());
    });


    $('.push-body').on("click", function(e) {
        e.preventDefault();
    })

    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message) {
        // Add the message to the page.
        $('#discussion').append('<li><strong>' + htmlEncode(name)
            + '</strong>: ' + htmlEncode(message) + '</li>');
    };


    editorHub.client.onChange = function (data) {
        silent = true;
        editor.getSession().getDocument().applyDelta(data);
        silent = false;
    }

    // Change file
    file.client.ReturnFile = function (id, content, type) {
        silent = true;
        editorHub.server.leaveFile(fileID);
        fileID = id;
        editorHub.server.joinFile(fileID);
        editor.getSession().setMode("ace/mode/" + type);
        editor.setValue(content);
        silent = false;
    }

    var chatContent = "";
    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message) {
        // Add the message to the page.
        $('#discussion').append('<li class="text-wrap"><strong>' + htmlEncode(name)
            + '</strong>: ' + htmlEncode(message) + '</li>');

        //TODO: save content to Chat class
        

    };

    // Get the user name and store it to prepend to messages
    // variable user is initialized in the Details.cshtml file for the 
    // Product controller, there it also extracts it's value.
    $('#displayname').val(user);
    // Set initial focus to message input box.
    $('#message').focus();

    // Start the connection.
    $.connection.hub.start().done(function () {
        editorHub.server.joinFile(fileID)

        editor.on("change", function (obj) {
            if (silent) {
                return;
            }
            content = editor.getValue();
            editorHub.server.onChange(obj, fileID);
            editorHub.server.save(content, fileID);
        });

        //getting files by id, when file name is clicked
        $('.tree-item').click(function () {
            //auto save or not?
            file.server.get($(this).data("id"));
        });

        // Chat Box send message
        $('#sendmessage').click(function () {
            // Call the Send method on the hub. 
            chat.server.send($('#displayname').val(), $('#message').val());
            // Clear text box and reset focus for next comment.
            $('#message').val('').focus();
        });

    });

    $('.toggle-menu').jPushMenu();

    /* JS for right click context menu*/
    $(document).on("contextmenu", ".tree-item", function (e) {
        e.preventDefault();
        var href = $(this).attr('href');
        $('#open-in-tab').attr('href', href);
        $("#cntnr").css("left", e.pageX);
        $("#cntnr").css("top", e.pageY);
        // $("#cntnr").hide(100);        
        $("#cntnr").fadeIn(200, startFocusOut());
    });
    
    $(".tree-item").click(function () {
        var id = $(this).data("id");
        $(".tree-item").removeClass("active");
        $(this).addClass("active");
    });
    
    function startFocusOut() {
        $(document).on("click", function () {
            $("#cntnr").hide();
            $(document).off("click");
        });
    };

    function htmlEncode(value) {
        var encodedValue = $('<div />').text(value).html();
        return encodedValue;

    }

    $('#chat-bubble').click(function () {
        var chat = $('.chat-container');
        if (chat.is(':visible')) {
            chat.hide();
        }
        else {
            chat.show();
        }
    });

});
