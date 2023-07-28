function loadGroups(courseId) {
    $.get(GetGroupsUrl, { courseId: courseId }, function (groups) {
        var buttons = "";
        groups.forEach(g => {
            buttons += `<div class="group">
                            <button class="group-button" data-group-id="${g.id}">${g.name}</button>
                            <button class="change-button">Change</button>
                            <div class="change-group-name" style="display: none;">
                                <input type="text" class="new-group-name" />
                                <button class="save-button">Save</button>
                            </div>
                            <button class="delete-button">Delete</button>
                        </div>`;
        });
        $("#groups").html(buttons);
    }).fail(function () {
        $("#groups").html("");
    });
}

$(document).on('click', '.change-button', function () {
    $(this).siblings('.change-group-name').show();
});

$(document).on('click', '.save-button', function () {
    var groupId = $(this).parent().siblings('.group-button').data("group-id");
    var newName = $(this).siblings('.new-group-name').val();
    var saveButton = $(this);
    if (groupId && newName) {
        $.post(ChangeGroupNameUrl, { groupId: groupId, newName: newName }, function (updatedGroup) {
            saveButton.closest('.group').find('.group-button').text(updatedGroup.Name);
            saveButton.siblings('.change-group-name').hide();
            saveButton.closest('.group').find('.group-button').text(newName);

        }).fail(function () {
            alert("Failed to change group name.");
        });
    } else {
        saveButton.siblings('.change-group-name').hide();
    }
    $(this).parent().hide();
});

$(document).on('click', '.delete-button', function () {
    var groupId = $(this).siblings('.group-button').data("group-id");
    var deleteButton = $(this);
    if (groupId) {
        $.ajax({
            url: DeleteGroupUrl,
            type: "POST",
            data: { groupId: groupId },
            success: function () {
                deleteButton.closest('.group').remove();
            },
            error: function () {
                alert("Failed to delete group. >= 1 student in this group");
            }
        });
    }
});

$(document).on('click', '.group-button', function () {
    var groupId = $(this).data("group-id");
    if (groupId) {
        loadStudents(groupId);
    } else {
        $("#students").html("");
    }
});

$("#add-group-button").click(function () {
    console.log("Add button clicked");
    var courseId = $(".course-button.selected").data("course-id");
    var groupName = $("#new-group-name").val();
    if (courseId && groupName) {
        $.post(AddGroupUrl, { courseId: courseId, groupName: groupName }, function (newGroup) {
            console.log("Inside success callback");
            console.log("New g:", newGroup);
            console.log("Inside success callback2");
            var newGroupHtml = `<div class="group">
                                    <button class="group-button" data-group-id="${newGroup.Id}">${newGroup.Name}</button>
                                    <button class="change-button">Change</button>
                                    <div class="change-group-name" style="display: none;">
                                        <input type="text" class="new-group-name" />
                                        <button class="save-button">Save</button>
                                    </div>
                                    <button class="delete-button">Delete</button>
                                </div>`;
            $("#groups").append(newGroupHtml);
            $("#new-group-name").val("");
            console.log("New group added:", newGroup);
        }).fail(function () {
            alert("Failed to add group.");
        });
    }
});



