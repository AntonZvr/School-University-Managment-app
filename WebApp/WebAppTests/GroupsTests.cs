using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data.ViewModels;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services.Interfaces;

namespace WebApp.Tests
{
    [TestClass]
    public class GroupServiceTests
    {
        private Mock<IGroupRepository> _groupRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IGroupService _groupService;

        [TestInitialize]
        public void Setup()
        {
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _mapperMock = new Mock<IMapper>();
            _groupService = new GroupService(_groupRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task GetAllGroups_ReturnsGroupViewModels()
        {
            // Arrange
            int courseId = 1;
            var groups = new List<GroupsModel>
             {
                new GroupsModel { GROUP_ID = 1, NAME = "Group 1" },
                new GroupsModel { GROUP_ID = 2, NAME = "Group 2" }
             };
            var groupViewModels = new List<GroupViewModel>
            {
                new GroupViewModel { GROUP_ID = 1, NAME = "Group 1" },
                new GroupViewModel { GROUP_ID = 2, NAME = "Group 2" }
            };

            _groupRepositoryMock.Setup(repo => repo.GetAllGroups(courseId)).ReturnsAsync(groups);
            _mapperMock.Setup(mapper => mapper.Map<List<GroupViewModel>>(groups)).Returns(groupViewModels);

            // Act
            var result = await _groupService.GetAllGroups(courseId);

            // Assert
            Assert.AreEqual(groupViewModels.Count, result.Count());
            foreach (var expectedGroup in groupViewModels)
            {
                var actualGroup = result.FirstOrDefault(g => g.GetType().GetProperty("Id").GetValue(g).Equals(expectedGroup.GROUP_ID) && g.GetType().GetProperty("Name").GetValue(g).Equals(expectedGroup.NAME));
                Assert.IsNotNull(actualGroup);
            }
        }

        [TestMethod]
        public async Task GetGroup_ExistingGroupId_ReturnsGroupViewModel()
        {
            // Arrange
            int groupId = 1;
            var group = new GroupsModel { GROUP_ID = groupId, NAME = "Group 1" };
            var groupViewModel = new GroupViewModel { GROUP_ID = groupId, NAME = "Group 1" };

            _groupRepositoryMock.Setup(repo => repo.GetGroup(groupId)).ReturnsAsync(group);
            _mapperMock.Setup(mapper => mapper.Map<GroupViewModel>(group)).Returns(groupViewModel);

            // Act
            var result = await _groupService.GetGroup(groupId);

            // Assert
            Assert.AreEqual(groupViewModel.GROUP_ID, result.GetType().GetProperty("Id").GetValue(result));
            Assert.AreEqual(groupViewModel.NAME, result.GetType().GetProperty("Name").GetValue(result));
        }

        [TestMethod]
        public async Task AddGroup_ReturnsNewGroupViewModel()
        {
            // Arrange
            int courseId = 1;
            string groupName = "New Group";
            var newGroup = new GroupsModel { GROUP_ID = 3, NAME = groupName };
            var newGroupViewModel = new GroupViewModel { GROUP_ID = 3, NAME = groupName };

            _groupRepositoryMock.Setup(repo => repo.AddGroup(courseId, groupName)).ReturnsAsync(newGroup);
            _mapperMock.Setup(mapper => mapper.Map<GroupViewModel>(newGroup)).Returns(newGroupViewModel);

            // Act
            var result = await _groupService.AddGroup(courseId, groupName);

            // Assert
            Assert.AreEqual(newGroupViewModel.GROUP_ID, result.GetType().GetProperty("Id").GetValue(result));
            Assert.AreEqual(newGroupViewModel.NAME, result.GetType().GetProperty("Name").GetValue(result));
        }

        [TestMethod]
        public async Task UpdateGroupName_ReturnsUpdatedGroupViewModel()
        {
            // Arrange
            int groupId = 1;
            string newName = "Updated Group";
            var group = new GroupsModel { GROUP_ID = groupId, NAME = "Group 1" };
            var updatedGroup = new GroupsModel { GROUP_ID = groupId, NAME = newName };
            var updatedGroupViewModel = new GroupViewModel { GROUP_ID = groupId, NAME = newName };

            _groupRepositoryMock.Setup(repo => repo.GetGroup(groupId)).ReturnsAsync(group);
            _groupRepositoryMock.Setup(repo => repo.UpdateGroupName(groupId, newName)).ReturnsAsync(updatedGroup);
            _mapperMock.Setup(mapper => mapper.Map<GroupViewModel>(updatedGroup)).Returns(updatedGroupViewModel);

            // Act
            var result = await _groupService.UpdateGroupName(groupId, newName);

            // Assert
            Assert.AreEqual(updatedGroupViewModel.GROUP_ID, result.GetType().GetProperty("Id").GetValue(result));
            Assert.AreEqual(updatedGroupViewModel.NAME, result.GetType().GetProperty("Name").GetValue(result));
        }

        [TestMethod]
        public async Task DeleteGroup_ExistingGroupId_DeletesGroup()
        {
            // Arrange
            int groupId = 1;
            var group = new GroupsModel { GROUP_ID = groupId, NAME = "Group 1" };

            _groupRepositoryMock.Setup(repo => repo.GetGroup(groupId)).ReturnsAsync(group);
            _groupRepositoryMock.Setup(repo => repo.HasAssociatedStudents(groupId)).ReturnsAsync(false);

            // Act
            await _groupService.DeleteGroup(groupId);

            // Assert
            _groupRepositoryMock.Verify(repo => repo.DeleteGroup(group), Times.Once);
        }

        [TestMethod]
        public async Task DeleteGroup_ExistingGroupIdWithAssociatedStudents_ThrowsArgumentException()
        {
            // Arrange
            int groupId = 1;
            var group = new GroupsModel { GROUP_ID = groupId, NAME = "Group 1" };

            _groupRepositoryMock.Setup(repo => repo.GetGroup(groupId)).ReturnsAsync(group);
            _groupRepositoryMock.Setup(repo => repo.HasAssociatedStudents(groupId)).ReturnsAsync(true);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _groupService.DeleteGroup(groupId));
        }

        [TestMethod]
        public async Task DeleteGroup_NonExistingGroupId_DoesNothing()
        {
            // Arrange
            int groupId = 1;

            _groupRepositoryMock.Setup(repo => repo.GetGroup(groupId)).ReturnsAsync((GroupsModel)null);

            // Act
            await _groupService.DeleteGroup(groupId);

            // Assert
            _groupRepositoryMock.Verify(repo => repo.DeleteGroup(It.IsAny<GroupsModel>()), Times.Never);
        }
    }
}
