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

            // Update the group name in the UI immediately
            saveButton.closest('.group').find('.group-button').text(newName);
        }).fail(function () {
            alert("Failed to change group name.");
        });
    } else {
        saveButton.siblings('.change-group-name').hide();
    }
    // Hide the input field after saving
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

function loadStudents(groupId) {
    $.get(GetStudentsUrl, { groupId: groupId }, function (students) {
        var buttons = "";
        students.forEach(s => {
            buttons += `<div class="student">
                            <button class="student-button" data-student-id="${s.id}">${s.firstName} ${s.lastName}</button>
                            <button class="change-student-button">Change</button>
                            <div class="change-student-name" style="display: none;">
                                <input type="text" class="new-first-name" placeholder="New first name" />
                                <input type="text" class="new-last-name" placeholder="New last name" />
                                <button class="save-student-button">Save</button>
                            </div>
                        </div>`;
        });
        $("#students").html(buttons);
    }).fail(function () {
        $("#students").html("");
    });
}

$(document).on('click', '.change-student-button', function () {
    $(this).siblings('.change-student-name').show();
});

$(document).on('click', '.save-student-button', function () {
    var studentId = $(this).parent().siblings('.student-button').data("student-id");
    var newFirstName = $(this).siblings('.new-first-name').val();
    var newLastName = $(this).siblings('.new-last-name').val();
    var saveButton = $(this);
    if (studentId && (newFirstName || newLastName)) {
        $.post(ChangeStudentNameUrl, { studentId: studentId, newFirstName: newFirstName, newLastName: newLastName }, function (updatedStudent) {
            saveButton.closest('.student').find('.student-button').text(`${updatedStudent.firstName} ${updatedStudent.lastName}`);
            saveButton.siblings('.change-student-name').hide();

            // Update the student name in the UI immediately

        }).fail(function () {
            alert("Failed to change student name.");
        });
    } else {
        saveButton.siblings('.change-student-name').hide();
    }
    // Hide the input field after saving
    $(this).parent().hide();
});

$(document).on('click', '.course-button', function () {
    var courseId = $(this).data("course-id");
    if (courseId) {
        loadGroups(courseId);
        $("#students").html(""); // Clear the students
    } else {
        $("#groups").html("");
        $("#students").html("");
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
