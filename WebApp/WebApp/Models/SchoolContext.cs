using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebApp.Models
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions options) : base(options)
        {
          
        }
        public DbSet<StudentsModel> Students { get; set; }
        public DbSet<GroupsModel> Groups { get; set; }
        public DbSet<CoursesModel> Courses { get; set; }

    }
}
