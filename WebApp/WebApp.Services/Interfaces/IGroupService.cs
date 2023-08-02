using System.Text.RegularExpressions;
using WebApp.Data.ViewModels;
using WebApp.Models;

namespace WebApp.Services.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<object>> GetAllGroups(int courseId);
        Task<object> GetGroup(int groupId);
        Task<object> UpdateGroupName(int groupId, string newName);
        Task DeleteGroup(int groupId);
        Task<object> AddGroup(int courseId, string groupName);
    }
}
