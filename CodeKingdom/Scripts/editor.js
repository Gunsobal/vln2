$(function () {
    var editor = ace.edit("editor"),
        chat = $.connection.chatHub,
        editorHub = $.connection.editorHub,
        file = $.connection.fileHub,
        silent = false,
        selectedFile = fileID,
        selectedFolder = 0,
        users = [];

    editor.setTheme("ace/theme/" + colorscheme);

    if (keyBinding != "ace")
    {
        editor.setKeyboardHandler("ace/keyboard/" + keyBinding);
    }
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

    
    file.client.deleteFolder = function (folderID) {
        $("#folder-" + folderID).parent().remove();
    }

    file.client.updateFolder = function (folderID, newName) {
        $($('a[data-folderid="' + folderID + '"]')[0].children[1]).context.textContent = newName;
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
        var username = model.Username.split('@');
        $('#discussion').append('<li class="show-time"><i>' + htmlEncode(model.DateAndTime) + '</i></li><li class="show-name-msg"><strong>' + htmlEncode(username[0]) + '</strong>: ' + htmlEncode(model.Message) + '</li>');
        scrollBottom();
        if ($(".chat-container").is(":hidden")) {
            $("#chat-bubble").addClass("bubble-Expand").delay(3000).queue(function(next){
                $(this).removeClass("bubble-Expand");
                next();
            });
        }
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
        
        $('.tree-item').click(function (e) {
            e.preventDefault();
            file.server.get($(this).data("id"), projectID);
        });

        $('.folder').click(function () {
            if ($(this).find(".fa").hasClass("fa-minus")) {
                $(this).find(".fa").removeClass("fa-minus").addClass("fa-plus");
            } else {
                $(this).find(".fa").removeClass("fa-plus").addClass("fa-minus");
            }
            
        })

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

        $('#folder-menu-delete').on('click', function () {
            var r = confirm("Are you sure you want to delete this folder along with all its subfolders and files?")
            if (r) {
                file.server.deleteFolder(projectID, selectedFolder);
            }
            $("#folderRightClickMenu").hide();
        });

        $('#folder-menu-rename').on('click', function () {
            var element = $($('a[data-folderid="' + selectedFolder + '"]')[0].children[1]);
            var folderName = element.context.textContent;
            var newFolderName = prompt("Enter a new name for " + folderName, folderName);
            if (newFolderName != folderName && newFolderName != null) {
                file.server.renameFolder(projectID, selectedFolder, newFolderName);
            }
            $("folderRightClickMenu").hide();
        });
    });

    $('.toggle-menu').jPushMenu();

    // JS for right click context menu
    $(document).on("contextmenu", ".tree-item", function (e) {
        e.preventDefault();
        $("#folderRightClickMenu").hide();
        var href = $(this).attr('href');
        $('#open-in-tab').attr('href', href);

        selectedFile = $(this).data("id");

        $("#cntnr").css("left", e.pageX);
        $("#cntnr").css("top", e.pageY);
        // $("#cntnr").hide(100);        
        $("#cntnr").fadeIn(200, startFocusOut("cntnr"));
    });

    $(document).on("contextmenu", ".folder", function (e) {
        e.preventDefault();
        $("#cntnr").hide();

        $("#folderRightClickMenu").css("left", e.pageX);
        $("#folderRightClickMenu").css("top", e.pageY);
        $("#folderRightClickMenu").fadeIn(200, startFocusOut("folderRightClickMenu"));

        selectedFolder = $(this).data("folderid");
    });
    
    
    $(".tree-item").click(function () {
        var id = $(this).data("id");
        $(".tree-item").removeClass("active");
        $(this).addClass("active");
    });
  
    function startFocusOut(id) {
        $(document).on("click", function () {
            $("#" + id).hide();
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

    // Positions the user at the bottom of the chat content window
    function scrollBottom() {
        var divScroll = $('.chat-content');
        var height = divScroll[0].scrollHeight;
        divScroll.scrollTop(height);
    }

    $('#chat-bubble').click(function () {
        var chat = $('.chat-container');
        $(this).removeClass("bubble-Expand");
        chat.show();
        $(this).hide();
        scrollBottom();
    });

    $('.close-chat').click(function () {
        var bubble = $('#chat-bubble');
        $('.chat-container').hide();
        bubble.show();
    });

    $('#message').keypress(function (e) {
        if (e.which == 13) {
            $('#sendmessage').click();
            $('#message').val('').focus();
            return false;
        }
    });

    $('.chat-container').keyup(function (e) {
        if (e.which == 27) {
            $(this).hide();
            $('#chat-bubble').show();
            return false;
        }
    });
});
