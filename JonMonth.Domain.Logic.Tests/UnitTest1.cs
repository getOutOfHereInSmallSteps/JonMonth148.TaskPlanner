using System;
using System.Linq;
using Moq;
using Xunit;
using Domain.Models;
using Domain.Models.enums;
using Domain.Logic;
using DataAccess;
using DataAccess.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Domain.Logic.Tests
{
    public class SimpleTaskPlannerTests
    {
        [Fact]
        public void CreatePlan_SortsTasksCorrectly()
        {
            var mockRepository = new Mock<TWorkItemsRepository>();
            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            var tasks = new[]
            {
                new WorkItem { Id = Guid.NewGuid(), Priority = Priority.High, DueDate = DateTime.Now.AddDays(2), IsCompleted = false },
                new WorkItem { Id = Guid.NewGuid(), Priority = Priority.Low, DueDate = DateTime.Now.AddDays(1), IsCompleted = false },
                new WorkItem { Id = Guid.NewGuid(), Priority = Priority.Medium, DueDate = DateTime.Now.AddDays(3), IsCompleted = false }
            };

            mockRepository.Setup(repo => repo.GetAll()).Returns(tasks);

            var result = taskPlanner.CreatePlan();

            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
            Assert.Equal(Priority.Low, result[0].Priority);
            Assert.Equal(Priority.Medium, result[1].Priority);
            Assert.Equal(Priority.High, result[2].Priority);
        }

        [Fact]
        public void CreatePlan_ExcludesCompletedTasks()
        {
            var mockRepository = new Mock<TWorkItemsRepository>();
            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            var tasks = new[]
            {
                new WorkItem { Id = Guid.NewGuid(), Priority = Priority.High, DueDate = DateTime.Now.AddDays(2), IsCompleted = false },
                new WorkItem { Id = Guid.NewGuid(), Priority = Priority.Low, DueDate = DateTime.Now.AddDays(1), IsCompleted = true }, // Completed task
                new WorkItem { Id = Guid.NewGuid(), Priority = Priority.Medium, DueDate = DateTime.Now.AddDays(3), IsCompleted = false }
            };

            mockRepository.Setup(repo => repo.GetAll()).Returns(tasks);

            var result = taskPlanner.CreatePlan();

            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.DoesNotContain(result, task => task.IsCompleted);
        }
    }
}