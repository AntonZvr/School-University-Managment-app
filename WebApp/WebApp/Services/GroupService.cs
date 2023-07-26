using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public class GroupService : IGroupService
    {
        private readonly SchoolContext _context;

        public async Task<IEnumerable<GroupsModel>> GetAllGroups(int courseId)
        {
            return await _context.Groups
           .Where(g => g.COURSE_ID == courseId)
           .ToListAsync();
        }

        public GroupService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<GroupsModel> GetGroup(int groupId)
        {
            return await _context.Groups.FirstOrDefaultAsync(g => g.GROUP_ID == groupId);
        }

        public async Task UpdateGroupName(int groupId, string newName)
        {
            var group = await GetGroup(groupId);

            if (group != null)
            {
                group.NAME = newName;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteGroup(int groupId)
        {
            var group = await GetGroup(groupId);

            if (group != null)
            {
                var students = await _context.Students.Where(s => s.GROUP_ID == groupId).ToListAsync();
                if (students.Count == 0)
                {
                    _context.Groups.Remove(group);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException($"Group {groupId} includes students and can't be deleted.");
                }
            }
        }

    }
}
