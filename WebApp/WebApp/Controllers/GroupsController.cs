using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        public async Task<IActionResult> GetGroups(int courseId)
        {
            var groups = await _groupService.GetAllGroups(courseId);
            return Json(groups.Select(g => new { Id = g.GROUP_ID, Name = g.NAME }));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeGroupName(int groupId, string newName)
        {
            await _groupService.UpdateGroupName(groupId, newName);
            var group = await _groupService.GetGroup(groupId);
            return Json(new { Id = group.GROUP_ID, Name = group.NAME });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            try
            {
                await _groupService.DeleteGroup(groupId);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup(int courseId, string groupName)
        {
            var newGroup = await _groupService.AddGroup(courseId, groupName);
            return Json(new { Id = newGroup.GROUP_ID, Name = newGroup.NAME });
        }
    }
}

