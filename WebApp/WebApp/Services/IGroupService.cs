using System.Text.RegularExpressions;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupsModel>> GetAllGroups(int courseId);
        Task<GroupsModel> GetGroup(int groupId);
        Task UpdateGroupName(int groupId, string newName);
        Task DeleteGroup(int groupId);
    }
}
