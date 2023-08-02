using WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Repositories
{
    public interface IGroupRepository
    {
        Task<IEnumerable<GroupsModel>> GetAllGroups(int courseId);
        Task<GroupsModel> GetGroup(int groupId);
        Task<GroupsModel> AddGroup(int courseId, string groupName);
        Task<GroupsModel> UpdateGroupName(int groupId, string newName);
        Task<bool> HasAssociatedStudents(int groupId);
        Task DeleteGroup(GroupsModel group);
    }
}
