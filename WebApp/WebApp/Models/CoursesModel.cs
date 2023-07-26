using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Models
{
    public class CoursesModel
    {
        [Key]
        public int COURSE_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
    }
}
