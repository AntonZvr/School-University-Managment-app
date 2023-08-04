using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly SchoolContext _context;

        public GroupRepository(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupsModel>> GetAllGroups(int courseId)
        {
            return await _context.Groups.Where(g => g.COURSE_ID == courseId).ToListAsync();
        }

        public async Task<GroupsModel> GetGroup(int groupId)
        {
            return await _context.Groups.FindAsync(groupId);
        }

        public async Task<GroupsModel> AddGroup(int courseId, string groupName)
        {
            var newGroup = new GroupsModel
            {
                COURSE_ID = courseId,
                NAME = groupName
            };

            _context.Groups.Add(newGroup);
            await _context.SaveChangesAsync();

            return newGroup;
        }

        public async Task<GroupsModel> UpdateGroupName(int groupId, string newName)
        {
            var group = await _context.Groups.FindAsync(groupId);

            if (group != null)
            {
                group.NAME = newName;
                await _context.SaveChangesAsync();
            }

            return group;
        }

        public async Task<bool> HasAssociatedStudents(int groupId)
        {
            return await _context.Students.AnyAsync(s => s.GROUP_ID == groupId);
        }

        public async Task DeleteGroup(GroupsModel group)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }
    }
}
