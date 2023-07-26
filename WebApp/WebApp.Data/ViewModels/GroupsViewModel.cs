using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Data.ViewModels
{
    public class GroupViewModel
    {
        public int COURSE_ID { get; set; }
        public int GROUP_ID { get; set; }
        public string NAME { get; set; }

        public CoursesModel Course { get; set; }
    }
}
