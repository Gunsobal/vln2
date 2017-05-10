$(function () {
    var editor = ace.edit("editor"),
        chat = $.connection.chatHub,
        editorHub = $.connection.editorHub,
        file = $.connection.fileHub,
        silent = false,
        selectedFile = fileID,
        users = [];

    editor.setTheme("ace/theme/twilight");
    editor.getSession().setMode("ace/mode/javascript");

    /* Source: http://stackoverflow.com/questions/24807066/multiple-cursors-in-ace-editor */
    var marker = {}
    marker.cursors = []
    marker.update = function (html, markerLayer, session, config) {
        var start = config.firstRow, end = config.lastRow;
        var cursors = this.cursors
        for (var i = 0; i < cursors.length; i++) {
            var pos = this.cursors[i];
            if (pos.row < start) {
                continue
            } else if (pos.row > end) {
                break
            } else {

                var screenPos = session.documentToScreenPosition(pos)
                var user = users.find(function (user) {
                    console.log(user, cursors[i]);
                    if (user.ID == cursors[i].id) {
                        return true;
                    }
                    return false;
                });

                var height = config.lineHeight;
                var width = config.characterWidth;
                var top = markerLayer.$getTop(screenPos.row, config);
                var left = markerLayer.$padding + screenPos.column * width;

                html.push(
                    "<div class='remote-cursor' style='",
                    "height:", height, "px;",
                    "top:", top, "px;",
                    "border-color: ", user.Color,";",
                    "left:", left, "px; width:", width, "px'></div>"
                );
            }
        }
    }
    marker.redraw = function () {
        this.session._signal("changeFrontMarker");
    }
    marker.session = editor.session;
    marker.session.addDynamicMarker(marker, true)


    $("form").submit(function () {
        $("#hidden_editor").val(editor.getSession().getValue());
    });


    $('.push-body').on("click", function(e) {
        e.preventDefault();
    })
    
    editorHub.client.onChange = function (data) {
        silent = true;
        editor.getSession().getDocument().applyDelta(data);
        silent = false;
    }

    file.client.renameFile = function (fileID, newName) {
        $('a[data-id="' + fileID + '"]')[0].text = newName;
    }

    file.client.removeFile = function (fileID) {
        $('a[data-id="' + fileID + '"]').parent().remove();
    }

    // Updates all remote cursors in the file
    editorHub.client.updateCursor = function (data) {
        var found = false;

        marker.cursors = marker.cursors.map(function (cursor) {
            if (cursor.id === data.id) {
                found = true;
                return data;
            }
            return cursor;
        });

        if (!found) {
            marker.cursors.push(data);
        }
        marker.redraw();
    }

    editorHub.client.userList = function (userList) {
        users = userList;
        renderActiveUsers(users);
    }

    editorHub.client.removeCursor = function (id) {
        marker.cursors = marker.cursors.filter(function (cursor) {
            if (cursor.id === id) {
                return false;
            }
            return true;
        });
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
    
    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (model) {
        // Add the message to the page.
        $('#discussion').append('<li class="text-wrap"><strong>' + htmlEncode(model.Username)
            + '</strong>: ' + htmlEncode(model.Message) + '</li>');
    };
    
    // Start the connection.
    $.connection.hub.start().done(function() {
        chat.server.joinChat(projectID);
        editorHub.server.joinFile(fileID);
        editorHub.server.getUsers(fileID);
        file.server.joinProject(projectID);

        editor.on("change", function (obj) {
            if (silent) {
                return;
            }
            content = editor.getValue();
            editorHub.server.onChange(obj, fileID);
            editorHub.server.save(content, fileID, projectID);
        });

        // When the cursor changes the server needs to be notified
        editor.getSession().selection.on('changeCursor', function (e) {
            var position = editor.getCursorPosition();
            position.id = $.connection.hub.id;
            editorHub.server.updateCursor(position, fileID);
        });
        
        //getting files by id, when file name is clicked
        /*$('.tree-item').click(function () {
            //auto save or not?
            file.server.get($(this).data("id"));
        });*/

        // Chat Box send message
        $('#sendmessage').click(function () {
            // Call the Send method on the hub. 
            chat.server.send(projectID, $('#message').val());
            // Clear text box and reset focus for next comment.
            $('#message').val('').focus();
        });

        $("#context-menu-rename").on('click', function () {
            var element = $('a[data-id="' + selectedFile + '"]')[0];
            var filename = element.text;
            var newFilename = prompt("Enter a new name for " + filename, filename);
            if (newFilename != filename && newFilename != null){
                file.server.renameFile(projectID, selectedFile, newFilename);
            }
            $("#cntnr").hide();
        });

        $("#context-menu-delete").on('click', function () {
            var r = confirm("Are you sure you want to delete this file?");
            if (r){
                file.server.deleteFile(projectID, selectedFile);
            }
            $("#cntnr").hide();
        });
    });

    $('.toggle-menu').jPushMenu();

    // JS for right click context menu
    $(document).on("contextmenu", ".tree-item", function (e) {
        e.preventDefault();
        var href = $(this).attr('href');
        $('#open-in-tab').attr('href', href);

        selectedFile = $(this).data("id");
        $("#context-menu-delete").attr("file-id", selectedFile);
        $("#context-menu-rename").attr("file-id", selectedFile);

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

    function renderActiveUsers(users) {
        var $users = $(".active-users");

        $users.html("")
        users.forEach(function (user) {
            $markup = $('<div class="active-user">' + user.Username + '</div>');
            $markup.css({ borderRight: "5px solid " + user.Color });
            $users.append($markup);
        });
        
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
