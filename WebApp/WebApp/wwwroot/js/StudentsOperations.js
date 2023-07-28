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
        }).fail(function () {
            alert("Failed to change student name.");
        });
    } else {
        saveButton.siblings('.change-student-name').hide();
    }
    $(this).parent().hide();
});
