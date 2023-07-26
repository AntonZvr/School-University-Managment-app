using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class StudentsModel
    {
        [Key]
        public int STUDENT_ID { get; set; }
        [ForeignKey("Group")]
        public int GROUP_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }

        public GroupsModel Group { get; set; }
    }
}
