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
            appendCourseButton(newCourse);
            console.log("New course added:", newCourse);
            $("#course-name").val("");
            $("#course-desc").val("");
            updateCourseList();
        }).fail(function () {
            alert("Failed to add course.");
        });
    }
});

// Function to update the course list
function updateCourseList() {
    $.get(GetCoursesUrl, function (courses) {
        $("#courses").empty();
        courses.forEach(function (course) {
            appendCourseButton(course);
        });
        console.log(courses);
    }).fail(function () {
        alert("Failed to update course list.");
    });
}

// Function to append a new course button
function appendCourseButton(course) {
    var newCourseHtml = `<div>
           <button class="course-button" data-course-id="${course.id}">${course.name}</button>
            <button class="change-coursename-button">Change</button>
        </div>
        <div class="change-course-name" style="display: none;">
            <input type="text" class="new-course-name" />
            <button class="save-coursename-button">Save</button>
        </div>`;
    $("#courses").append(newCourseHtml);
}

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

$(document).on('click', '.change-coursename-button', function () {
    console.log("clicked change");
    $(this).parent().next('.change-course-name').show();
});

$(document).on('click', '.save-coursename-button', function () {
    var courseId = $(this).parent().prev().find('.course-button').data("course-id");
    var newName = $(this).siblings('.new-course-name').val();
    var saveButton = $(this);
    console.log(courseId, newName);
    if (courseId && newName) {
        console.log("inside if statement")
        $.post(UpdateCourseNameUrl, { courseId: courseId, newName: newName }, function (updateCourse) {
            console.log("inside POST");
            saveButton.closest('.course').find('.course-button').text(updateCourse.Name);
            saveButton.siblings('.change-course-name').hide();
            saveButton.closest('.course').find('.course-button').text(newName);
            updateCourseList();
        }).fail(function (xhr, status, error) {
            console.log(xhr.responseText);
            alert("Failed to change course name. Error: " + error);
        });
    } else {
        saveButton.siblings('.change-course-name').hide();
    }
    $(this).parent().hide();
});
