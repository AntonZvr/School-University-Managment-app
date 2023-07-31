// Add Course
$("#add-course-button").click(function () {
    console.log("Add button clicked");
    var desc = $("#course-desc").val();
    var courseName = $("#course-name").val();
    console.log(courseName);
    console.log(desc);
    if (desc && courseName) {
        $.post(AddCourseUrl, { courseName: courseName, description: desc }, function (newCourse) {
            console.log("Inside success callback");
            console.log("New c:", newCourse);
            var newGroupHtml = `<div class="group">
                                    <button class="group-button" data-group-id="${newCourse.Id}">${newCourse.Name}</button>
                                    <button class="change-button">Change</button>
                                    <div class="change-group-name" style="display: none;">
                                        <input type="text" class="new-group-name" />
                                        <button class="save-button">Save</button>
                                    </div>
                                    <button class="delete-button">Delete</button>
                                </div>`;
            $("#groups").append(newGroupHtml);
            $("#new-group-name").val("");
            loadGroups(courseId);
            console.log("New group added:", newCourse);
        }).fail(function () {
            alert("Failed to add group.");
        });
    }
});
// Select Course
$(document).on('click', '.course-button', function () {
    // Deselect the previously selected course
    $('.course-button.selected').removeClass('selected');
    // Select the clicked course
    $(this).addClass('selected');
    var courseId = $(this).data("course-id");
    if (courseId) {
        loadGroups(courseId);
        $("#students").html("");
    } else {
        $("#groups").html("");
        $("#students").html("");
    }
});

$(document).on('click', '#add-course-button', function () {
    console.log('Button clicked!');
});

