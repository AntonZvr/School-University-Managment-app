using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class GroupsModel
    {
        [ForeignKey("Course")]
        public int COURSE_ID { get; set; }
        [Key]
        public int GROUP_ID { get; set; }
        public string NAME { get; set; }

        public CoursesModel Course { get; set; }
    }
}
