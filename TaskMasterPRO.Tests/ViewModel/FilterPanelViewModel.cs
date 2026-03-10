

using TaskMasterPRO.Data.Domain;
using TaskMasterPRO.ViewModel.Components.Filter;

namespace TaskMasterPRO.Tests.ViewModel
{
    public class FilterPanelViewModelTests
    {
        private readonly FilterPanelViewModel _viewModel;

        public FilterPanelViewModelTests()
        {
            _viewModel = new FilterPanelViewModel();
        }

        /*
         * Search text logic
         */

        [Fact]
        public void SearchText_ShouldBeEmptyInitially()
        {
            FilterPanelViewModel vm = new ();

            Assert.Equal(string.Empty, vm.SearchText);
            Assert.NotNull(vm.SearchText);
        }

        [Fact]
        public void SearchText_WhenChanged_ShouldInvokeFiltersChanged()
        {
            FilterPanelViewModel vm = new();
            bool eventRaised = false;
            vm.FiltersChanged += () => eventRaised = true;

            vm.SearchText = "text";

            Assert.True(eventRaised);
        }

        [Fact]
        public void SearchText_WhenResettingToEmpty_ShouldInvokeFiltersChanged()
        {
            FilterPanelViewModel vm = new() { SearchText = "text" };
            bool eventRaised = false;
            vm.FiltersChanged += () => eventRaised = true;

            vm.SearchText = string.Empty;

            Assert.True(eventRaised);
        }

        [Fact]
        public void SearchText_WhenSettingSameValue_ShouldNotInvokeFiltersChanged()
        {
            FilterPanelViewModel vm = new();
            vm.SearchText = "text";

            int callCount = 0;
            vm.FiltersChanged += () => callCount++;

            vm.SearchText = "text";

            Assert.Equal(0, callCount);
        }



        /*
         * Show only active logic
         */

        [Fact]
        public void ShowOnlyActive_ShouldBeTrueInitially()
        {
            FilterPanelViewModel vm = new();

            Assert.True(vm.ShowOnlyActive);
        }

        [Fact]
        public void ShowOnlyActive_WhenChanged_ShouldInvokeFiltersChanged()
        {
            FilterPanelViewModel vm = new();
            bool eventRaised = false;
            vm.FiltersChanged += () => eventRaised = true;

            vm.ShowOnlyActive = false;

            Assert.True(eventRaised);
        }




        /*
         * All filtering logic
         */

        [Fact]
        public void ToggleFilter_SelectingSpecificCategory_ShouldDeselectAll()
        {
            FilterPanelViewModel vm = new();
            FilterItem<Category> allItem = new() { DisplayName = "All", IsSelected = true };
            FilterItem<Category> workItem = new() { DisplayName = "Work", IsSelected = false };
            vm.CategoryFilters.Add(allItem);
            vm.CategoryFilters.Add(workItem);

            workItem.IsSelected = true;
            vm.ToggleFilterCommand.Execute(workItem);

            Assert.False(allItem.IsSelected);
            Assert.True(workItem.IsSelected);
        }

        [Fact]
        public void ToggleFilter_DeselectingAll_ShouldSelectAll()
        {
            FilterPanelViewModel vm = new();
            FilterItem<Category> allItem = new() { DisplayName = "All", IsSelected = true };
            FilterItem<Category> workItem = new() { DisplayName = "Work", IsSelected = false };
            vm.CategoryFilters.Add(allItem);
            vm.CategoryFilters.Add(workItem);

            allItem.IsSelected = false;
            vm.ToggleFilterCommand.Execute(allItem);

            Assert.True(allItem.IsSelected);
            Assert.False(workItem.IsSelected);
        }

        [Fact]
        public void ToggleFilter_SelectingMoreThanOneSpecificCategory_ShouldDeselectAllAndKeepSelectedTheOthers()
        {
            FilterPanelViewModel vm = new();
            FilterItem<Category> allItem = new() { DisplayName = "All", IsSelected = true };
            FilterItem<Category> workItem = new() { DisplayName = "Work", IsSelected = false };
            FilterItem<Category> homeItem = new() { DisplayName = "Home", IsSelected = false };
            vm.CategoryFilters.Add(allItem);
            vm.CategoryFilters.Add(workItem);
            vm.CategoryFilters.Add(homeItem);

            workItem.IsSelected = true;
            vm.ToggleFilterCommand.Execute(workItem);

            Assert.False(allItem.IsSelected);
            Assert.True(workItem.IsSelected);
            Assert.False(homeItem.IsSelected);

            homeItem.IsSelected = true;
            vm.ToggleFilterCommand.Execute(homeItem);

            Assert.False(allItem.IsSelected);
            Assert.True(workItem.IsSelected);
            Assert.True(homeItem.IsSelected);
        }


        [Fact]
        public void ToggleFilter_DeselectingCategory_ShouldSelectAllIfNoOtherSelected()
        {
            FilterPanelViewModel vm = new();
            FilterItem<Category> allItem = new() { DisplayName = "All", IsSelected = false };
            FilterItem<Category> workItem = new() { DisplayName = "Work", IsSelected = true };
            vm.CategoryFilters.Add(allItem);
            vm.CategoryFilters.Add(workItem);

            workItem.IsSelected = false;
            vm.ToggleFilterCommand.Execute(workItem);

            Assert.True(allItem.IsSelected);
            Assert.False(workItem.IsSelected);
        }

        [Fact]
        public void ToggleFilter_DeselectingCategoryIfMoreSelected_ShouldJustDeselectItself()
        {
            FilterPanelViewModel vm = new();
            FilterItem<Category> allItem = new() { DisplayName = "All", IsSelected = false };
            FilterItem<Category> workItem = new() { DisplayName = "Work", IsSelected = true };
            FilterItem<Category> homeItem = new() { DisplayName = "Home", IsSelected = true };
            vm.CategoryFilters.Add(allItem);
            vm.CategoryFilters.Add(workItem);
            vm.CategoryFilters.Add(homeItem);

            workItem.IsSelected = false;
            vm.ToggleFilterCommand.Execute(workItem);

            Assert.False(allItem.IsSelected);
            Assert.False(workItem.IsSelected);
            Assert.True(homeItem.IsSelected);
        }

        [Fact]
        public void ToggleFilter_SelectingAll_ShouldSelectItselfAndDeselectTheOthers()
        {
            FilterPanelViewModel vm = new();
            FilterItem<Category> allItem = new() { DisplayName = "All", IsSelected = false };
            FilterItem<Category> workItem = new() { DisplayName = "Work", IsSelected = true };
            FilterItem<Category> homeItem = new() { DisplayName = "Home", IsSelected = true };
            vm.CategoryFilters.Add(allItem);
            vm.CategoryFilters.Add(workItem);
            vm.CategoryFilters.Add(homeItem);

            allItem.IsSelected = true;
            vm.ToggleFilterCommand.Execute(allItem);

            Assert.True(allItem.IsSelected);
            Assert.False(workItem.IsSelected);
            Assert.False(homeItem.IsSelected);
        }
    }
}
