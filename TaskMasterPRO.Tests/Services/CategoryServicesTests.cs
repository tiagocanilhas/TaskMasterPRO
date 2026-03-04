using Moq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TaskMasterPRO.Data.Domain;
using TaskMasterPRO.Data.Repository.Interfaces;
using TaskMasterPRO.Data.Services;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Tests.Services
{
    public class CategoryServicesTests
    {
        private readonly Mock<ICategoryRepository> _repoMock;
        private readonly CategoryDomain _domain;
        private readonly CategoryServices _service;

        public CategoryServicesTests()
        {
            _repoMock = new Mock<ICategoryRepository>();
            _domain = new CategoryDomain();
            _service = new CategoryServices(_domain, _repoMock.Object);
        }


        /*
         * Create
         */

        [Fact]
        public async void CreateAsync_Success()
        {
            string name = "Name";
            string description = "Description";
            string color = "#FFFFFF";

            Category category = new()
            {
                Id = 2,
                Name = name,
                Description = description,
                Color = color
            };
            _repoMock.Setup(repo => repo.CreateAsync(It.IsAny<Category>())).ReturnsAsync(category);

            var result = await _service.CreateAsync(name, description, color);

            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal(name, result.Name);
            Assert.Equal(description, result.Description);
            Assert.Equal(color, result.Color);
        }

        [Fact]
        public async void CreateAsync_Failure_BlankName()
        {
            string name = "";
            string description = "Description";
            string color = "#FFFFFF";

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.CreateAsync(name, description, color)
            );
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateAsync_Failure_BadColor()
        {
            string name = "Name";
            string description = "Description";
            string color = "abc";

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.CreateAsync(name, description, color)
            );
        }



        /*
         * Read
         */

        [Fact]
        public async void GetAllAsync_Success()
        {
            List<Category> categories = new()
            {
                new () { Id = 1, Name = "Name1", Description = "Description1", Color = "#FFFFFF" },
                new () { Id = 2, Name = "Name2", Description = "Description2", Color = "#000000" }
            };
            _repoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            // Category 1
            Assert.Equal(categories[0].Id, result[0].Id);
            Assert.Equal(categories[0].Name, result[0].Name);
            Assert.Equal(categories[0].Description, result[0].Description);
            Assert.Equal(categories[0].Color, result[0].Color);

            // Category 2
            Assert.Equal(categories[1].Id, result[1].Id);
            Assert.Equal(categories[1].Name, result[1].Name);
            Assert.Equal(categories[1].Description, result[1].Description);
            Assert.Equal(categories[1].Color, result[1].Color);
        }

        [Fact]
        public async void GetAllAsync_Success_Empty()
        {
            List<Category> categories = new();
            _repoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void GetByIdAsync_Success()
        {
            int id = 1;
            Category category = new() { Id = id, Name = "Name", Description = "Description", Color = "#FFFFFF" };
            _repoMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync(category);

            var result = await _service.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(category.Name, result.Name);
            Assert.Equal(category.Description, result.Description);
            Assert.Equal(category.Color, result.Color);
        }

        [Fact]
        public async void GetByIdAsync_Failure_NotFound()
        {
            int id = 1;
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _service.GetByIdAsync(id)
            );
        }



        /*
         * Update
         */

        [Fact]
        public async void UpdateAsync_Success()
        {
            int id = 1;
            string name = "Updated Name";
            string description = "Updated Description";
            string color = "#000000";

            Category category = new() { Id = id, Name = "Name", Description = "Description", Color = "#FFFFFF" };
            _repoMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync(category);
            _repoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Category>())).ReturnsAsync((Category c) => c);

            var result = await _service.UpdateAsync(id, name, description, color);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(name, result.Name);
            Assert.Equal(description, result.Description);
            Assert.Equal(color, result.Color);
        }

        [Fact]
        public async void UpdateAsync_Failure_NotFound()
        {
            int id = 1;
            string name = "Updated Name";
            string description = "Updated Description";
            string color = "#000000";

            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _service.UpdateAsync(id, name, description, color)
            );
        }

        [Fact]
        public async void UpdateAsync_Failure_BlankName()
        {
            int id = 1;
            string name = "";
            string description = "Updated Description";
            string color = "#000000";

            Category category = new() { Id = id, Name = "Name", Description = "Description", Color = "#FFFFFF" };
            _repoMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync(category);

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.UpdateAsync(id, name, description, color)
            );
        }

        [Fact]
        public async void UpdateAsync_Failure_BadColor()
        {
            int id = 1;
            string name = "Updated Name";
            string description = "Updated Description";
            string color = "abc";

            Category category = new() { Id = id, Name = "Name", Description = "Description", Color = "#FFFFFF" };
            _repoMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync(category);

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.UpdateAsync(id, name, description, color)
            );
        }



        /*
         * Delete
         */

        [Fact]
        public async void DeleteAsync_Success()
        {
            int id = 1;
            Category category = new() { Id = id, Name = "Name", Description = "Description", Color = "#FFFFFF" };

            _repoMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync(category);
            _repoMock.Setup(repo => repo.DeleteAsync(category)).Returns(Task.CompletedTask);

            await _service.DeleteAsync(id);

            _repoMock.Verify(repo => repo.DeleteAsync(category), Times.Once);

        }

        [Fact]
        public async void DeleteAsync_Failure_NotFound()
        {
            int id = 1;
            _repoMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync((Category)null!);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _service.DeleteAsync(id)
            );
        }
    }
}
