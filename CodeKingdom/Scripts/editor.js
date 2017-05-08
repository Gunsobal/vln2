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


    // Get the user name and store it to prepend to messages.
    $('#displayname').val("@User.Identity.Name");
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

        $('#sendmessage').click(function () {
            // Call the Send method on the hub.
            chat.server.send($('#displayname').val(), $('#message').val());
            // Clear text box and reset focus for next comment.
            $('#message').val('').focus();
        });
        //getting files by id, when file name is clicked
        $('.tree-item').click(function () {
            //auto save or not?
            file.server.get($(this).data("id"));
        });
    });

    $('.toggle-menu').jPushMenu();

    /* JS for right click context menu*/
    $(document).on("contextmenu", ".menu-list", function (e) {
        e.preventDefault();
        console.log(e.pageX + "," + e.pageY);
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
});
