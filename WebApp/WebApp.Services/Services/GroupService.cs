using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.ViewModels;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;

    public GroupService(IGroupRepository groupRepository, IMapper mapper)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<object>> GetAllGroups(int courseId)
    {
        var groups = await _groupRepository.GetAllGroups(courseId);
        var groupViewModels = _mapper.Map<List<GroupViewModel>>(groups);

        return groupViewModels.Select(g => new { Id = g.GROUP_ID, Name = g.NAME });
    }

    public async Task<object> GetGroup(int groupId)
    {
        var group = await _groupRepository.GetGroup(groupId);

        if (group == null)
        {
            return null;
        }

        var groupViewModel = _mapper.Map<GroupViewModel>(group);

        return new { Id = groupViewModel.GROUP_ID, Name = groupViewModel.NAME };
    }

    public async Task<object> UpdateGroupName(int groupId, string newName)
    {
        var group = await _groupRepository.UpdateGroupName(groupId, newName);

        var groupViewModel = _mapper.Map<GroupViewModel>(group);

        return new { Id = groupViewModel.GROUP_ID, Name = groupViewModel.NAME };
    }

    public async Task DeleteGroup(int groupId)
    {
        var group = await _groupRepository.GetGroup(groupId);

        if (group != null)
        {
            if (await _groupRepository.HasAssociatedStudents(groupId))
            {
                throw new ArgumentException($"Group {groupId} includes students and can't be deleted.");
            }
            else
            {
                await _groupRepository.DeleteGroup(group);
            }
        }
    }

    public async Task<object> AddGroup(int courseId, string groupName)
    {
        var newGroup = await _groupRepository.AddGroup(courseId, groupName);

        var groupViewModel = _mapper.Map<GroupViewModel>(newGroup);

        return new { Id = groupViewModel.GROUP_ID, Name = groupViewModel.NAME };
    }
}
