$(function () {

    var editor = ace.edit("editor"),
        editorHub = $.connection.editorHub,
        projectHub = $.connection.projectHub,
        silent = false,
        selectedFile = fileID,
        selectedFolder = 0,
        users = [];

    editor.setTheme("ace/theme/" + colorscheme);

    if (keyBinding != "ace")
    {
        editor.setKeyboardHandler("ace/keyboard/" + keyBinding);
    }
    editor.getSession().setMode("ace/mode/"+type);

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

    $('.toggle-menu').jPushMenu();

    $("form").submit(function () {
        $("#hidden_editor").val(editor.getSession().getValue());
    });

    $('.push-body').on("click", function(e) {
        e.preventDefault();
    })

    /* Talking to EditorHub */

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
    
    editorHub.client.onChange = function (data) {
        silent = true;
        editor.getSession().getDocument().applyDelta(data);
        silent = false;
    }

    /* Talking to ProjectHub */

    projectHub.client.renameFile = function (fileID, newName) {
        $('a[data-id="' + fileID + '"]')[0].text = newName;
    }

    projectHub.client.removeFile = function (fileID) {
        $('a[data-id="' + fileID + '"]').parent().remove();
    }

    
    projectHub.client.deleteFolder = function (folderID) {
        $("#folder-" + folderID).parent().remove();
    }

    projectHub.client.updateFolder = function (folderID, newName) {
        $($('a[data-folderid="' + folderID + '"]')[0].children[1]).context.textContent = newName;
    }

    // Change file
    projectHub.client.ReturnFile = function (id, content, type) {
        silent = true;
        editorHub.server.leaveFile(fileID);
        fileID = id;
        window.history.pushState({}, "", "/Project/Details/" + projectID + "/" + fileID);
        editorHub.server.joinFile(fileID);
        editor.getSession().setMode("ace/mode/" + type);
        console.log(type);
        editor.setValue(content);
        silent = false;
    }
    
    // Function for the hub to call to add a new message to the chat
    projectHub.client.addNewMessageToPage = function (model) {
        var username = model.Username.split('@');
        $('#discussion').append('<li class="show-time"><i>' + htmlEncode(model.DateAndTime) + '</i></li><li class="show-name-msg"><strong>' + htmlEncode(username[0]) + '</strong>: ' + htmlEncode(model.Message) + '</li>');
        scrollBottom();
        if ($(".chat-container").is(":hidden")) {
            $.notify({
                icon: 'glyphicon glyphicon-comment',
                title: username[0] + ": ",
                message: model.Message,
            }, {
                type: "info",
                timer: 5000,
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                },
            });
            $("#chat-bubble").addClass("bubble-expand").delay(3000).queue(function(next){
                $(this).removeClass("bubble-expand");
                next();
            });
        }
    };
    
    // Start the connection.
    $.connection.hub.start().done(function() {
        editorHub.server.joinFile(fileID);
        editorHub.server.getUsers(fileID);
        projectHub.server.joinProject(projectID);

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
            projectHub.server.get($(this).data("id"), projectID);
        });

        // Event that updates the chat box and sends the messages to the hub
        $('#sendmessage').click(function () {
            projectHub.server.send(projectID, $('#message').val());
            $('#message').val('').focus();
        });

        $("#context-menu-rename").on('click', function () {
            var element = $('a[data-id="' + selectedFile + '"]')[0];
            var filename = element.text;
            var newFilename = prompt("Enter a new name for " + filename, filename);
            if (newFilename != filename && newFilename){
                projectHub.server.renameFile(projectID, selectedFile, newFilename);
            }
            $("#cntnr").hide();
        });

        $("#context-menu-delete").on('click', function () {
            var r = confirm("Are you sure you want to delete this file?");
            if (r){
                projectHub.server.deleteFile(projectID, selectedFile);
            }
            $("#cntnr").hide();
        });

        $('#folder-menu-delete').on('click', function () {
            var r = confirm("Are you sure you want to delete this folder along with all its subfolders and files?")
            if (r) {
                projectHub.server.deleteFolder(projectID, selectedFolder);
            }
            $("#folderRightClickMenu").hide();
        });

        $('#folder-menu-rename').on('click', function () {
            var element = $($('a[data-folderid="' + selectedFolder + '"]')[0].children[1]);
            var folderName = element.context.textContent;
            var newFolderName = prompt("Enter a new name for " + folderName, folderName);
            if (newFolderName != folderName && newFolderName) {
                projectHub.server.renameFolder(projectID, selectedFolder, newFolderName);
            }
            $("folderRightClickMenu").hide();
        });
    });

    /* Functions and Event handlers */
    
    // JS for right click context menu
    $(document).on("contextmenu", ".tree-item", function (e) {
        e.preventDefault();
        $("#folderRightClickMenu").hide();
        var href = $(this).attr('href');
        $('#open-in-tab').attr('href', href);

        selectedFile = $(this).data("id");

        $("#cntnr").css("left", e.pageX);
        if (e.pageY + 150 >= $("html").height()) {
            $("#cntnr").css("top", e.pageY-150);
        } else {
            $("#cntnr").css("top", e.pageY);
        }     
        $("#cntnr").fadeIn(200, startFocusOut("cntnr"));
    });

    $('.folder').click(function () {
        if ($(this).find(".fa").hasClass("fa-minus")) {
            $(this).find(".fa").removeClass("fa-minus").addClass("fa-plus");
        } else {
            $(this).find(".fa").removeClass("fa-plus").addClass("fa-minus");
        }
    });

    $(document).on("contextmenu", ".folder", function (e) {
        e.preventDefault();
        $("#cntnr").hide();
        if ($(this).hasClass("root")) {
            return;
        }
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

    // Positions chat-content div at the bottom so that new messages are always shown first.
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
