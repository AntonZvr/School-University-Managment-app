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
    var newCourseHtml = `<button class="course-button" data-course-id="${course.id}">${course.name}</button>`;
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
