using System.Text.RegularExpressions;
using WebApp.Data.ViewModels;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupViewModel>> GetAllGroups(int courseId);
        Task<GroupViewModel> GetGroup(int groupId);
        Task UpdateGroupName(int groupId, string newName);
        Task DeleteGroup(int groupId);
    }
}
