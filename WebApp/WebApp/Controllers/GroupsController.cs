using Microsoft.AspNetCore.Mvc;
using WebApp.Services.Interfaces;

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
            return Json(groups);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeGroupName(int groupId, string newName)
        {
            var group = await _groupService.UpdateGroupName(groupId, newName);
            return Json(group);
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
            return Json(newGroup);
        }
    }

}

