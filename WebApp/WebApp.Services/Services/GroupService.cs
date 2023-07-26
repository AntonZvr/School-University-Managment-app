using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data.ViewModels;
using WebApp.Models;
using WebApp.Services;

public class GroupService : IGroupService
{
    private readonly SchoolContext _context;
    private readonly IMapper _mapper;

    public GroupService(SchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroupViewModel>> GetAllGroups(int courseId)
    {
        var groups = await _context.Groups
           .Where(g => g.COURSE_ID == courseId)
           .ToListAsync();

        var groupViewModels = _mapper.Map<List<GroupViewModel>>(groups);

        return groupViewModels;
    }

    public async Task<GroupViewModel> GetGroup(int groupId)
    {
        var group = await _context.Groups.FindAsync(groupId);

        if (group == null)
        {
            return null;
        }

        var groupViewModel = _mapper.Map<GroupViewModel>(group);

        return groupViewModel;
    }

    public async Task UpdateGroupName(int groupId, string newName)
    {
        var group = await _context.Groups.FindAsync(groupId);

        if (group != null)
        {
            group.NAME = newName;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteGroup(int groupId)
    {
        var group = await _context.Groups.FindAsync(groupId);

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
