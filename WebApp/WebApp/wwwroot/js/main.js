$(document).on('click', '.course-button', function () {
    var courseId = $(this).data("course-id");
    if (courseId) {
        loadGroups(courseId);
        $("#students").html("");
    } else {
        $("#groups").html("");
        $("#students").html("");
    }
});
