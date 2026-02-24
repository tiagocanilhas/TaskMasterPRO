using Microsoft.EntityFrameworkCore;
using TaskMasterPRO.Data;
using TaskMasterPRO.Repository;
using TaskMasterPRO.Model;

namespace TaskMasterPRO.Tests.Repository
{
    public class CategoryRepositoryTests
    {
        private readonly TaskMasterPROContext _context;
        private readonly CategoryRepository _repository;

        public CategoryRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TaskMasterPROContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new (options);
            _repository = new (_context);


            PopulateDatabase();
        }


        /*  ====================================================
         *  | Id  | Name     | Description     | Color         |
         *  |-----|----------|-----------------|---------------|
         *  |  1  | "Name"   | "Description"   | "#FFFFFF"     |
         *  ====================================================
         */

        private void PopulateDatabase()
        {
            _context.Category.Add(new()
            {
                Name = "Name",
                Description = "Description",
                Color = "#FFFFFF"
            });
            _context.SaveChangesAsync();
        }




        /*
         * Create
         */

        [Fact]
        public async void CreateTest_AddCategory()
        {
            Category category = new() { 
                Name = "Name",
                Description = "Description",
                Color = "#FFFFFF"
            };
            var res = await _repository.CreateAsync(category);

            Assert.NotNull(res);
            Assert.True(res.Id > 0);
        }



        /*
         * Read
         */

        [Fact]
        public async void GetTest_GetAllCategories()
        {
            var res = await _repository.GetAllAsync();

            Assert.NotNull(res);
            Assert.Equal(1, res.Count);
        }

        [Fact]
        public async void GetTest_GetCategory()
        {
            int id = 1;
            var res = await _repository.GetAsync(id);

            Assert.NotNull(res);
            Assert.Equal(id, res.Id);
        }
        
        [Fact]
        public async void GetTest_FailedToGetCategory()
        {
            int id = -1;
            var res = await _repository.GetAsync(id);

            Assert.Null(res);
        }



        /*
         * Update
         */

        [Fact]
        public async void UpdateTest_UpdateCategory()
        {
            int id = 1;
            Category category = await _repository.GetAsync(id) ?? throw new Exception("Category not found");
            category.Name = "Updated Category";

            var res = await _repository.UpdateAsync(category);

            Assert.Equal("Updated Category", res.Name);
        }



        /*
         * Delete
         */

        [Fact]
        public async void DeleteTest_DeleteCategory()
        {
            int id = 1;
            Category category = await _repository.GetAsync(id) ?? throw new Exception("Category not found");

            await _repository.DeleteAsync(category);
            Category? checkCategory = await _repository.GetAsync(id);

            Assert.Null(checkCategory);
        }
    }
}