using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zenref.Ava.Models;

namespace zenref.Tests
{
    public class FilterTest
    {
        // Test creation of filters.
        [Fact]
        public void SuccessfulFilterCreation()
        {

            List<string> filters = new List<string>()
            {
                "Podcast",
                "Radio",
            };

            string category = "Podcast";

            Filter searchFilters = new Filter(filters, category);

            Assert.Equal(filters, searchFilters.ReturnFilterQueries());
            Assert.Equal(category, searchFilters.ReturnFilterCategory());
        }

        [Fact]
        public void AddFilter()
        {
            List<string> filters = new List<string>()
            {
                "Podcast",
                "Radio",
            };

            string category = "Podcast";

            Filter searchFilters = new Filter(filters, category);

            searchFilters.AddFilterQuery("TV");

            Assert.Equal(new List<string>()
            {
                "Podcast",
                "Radio",
                "TV",
            }, searchFilters.ReturnFilterQueries());
        }

        [Fact]
        public void removeFilter()
        {
            List<string> filters = new List<string>()
            {
                "Podcast",
                "Radio",
                "TV",
            };

            string category = "Podcast";

            Filter searchFilters = new Filter(filters, category);

            searchFilters.RemoveFilterQuery("Radio");

            Assert.Equal(new List<string>()
            {
                "Podcast",
                "TV",
            }, searchFilters.ReturnFilterQueries());
        }

        [Fact]
        public void FilterContainsInList()
        {
            List<string> filters = new List<string>()
            {
                "Podcast",
                "Radio",
                "TV",
            };

            string category = "Podcast";

            Filter searchFilters = new Filter(filters, category);

            Assert.True(searchFilters.ContainsQuery("Radio"));
        }

        [Fact]
        public void FilterDoesNotContainInList()
        {
            List<string> filters = new List<string>()
            {
                "Podcast",
                "Radio",
                "TV",
            };

            string category = "Podcast";

            Filter searchFilters = new Filter(filters, category);

            Assert.False(searchFilters.ContainsQuery("Music"));
        }
    }

    public class FilterCollectionTest
    {
        [Fact]
        public void filterCreation()
        {
            List<string> filterQuery = new List<string>()
            {
                "Podcast",
                "Radio",
            };

            string categoryName = "Podcast";

            Filter filter = new Filter(filterQuery, categoryName);

            IFilterCollection.GetInstance().Add(filter);

            Assert.True(IFilterCollection.GetInstance().Contains(filter));
        }

        [Fact]
        public void filterSearching()
        {
            // Arrange
            // Filter 1
            List<string> filterQuery1 = new List<string>()
            {
                "Podcast",
                "Radio",
            };
            string categoryName1 = "Podcast";

            // Filter 2
            List<string> filterQuery2 = new List<string>()
            {
                "Video",
                "Movie",
            };
            string categoryName2 = "Video";

            // Filter 3
            List<string> filterQuery3 = new List<string>()
            {
                "Journal",
            };
            string categoryName3 = "Journal";

            IFilterCollection.GetInstance().Add(new Filter(filterQuery1, categoryName1));
            IFilterCollection.GetInstance().Add(new Filter(filterQuery2, categoryName2));
            IFilterCollection.GetInstance().Add(new Filter(filterQuery3, categoryName3));

            // Act
            Filter result = IFilterCollection.GetInstance().FindFilter("Video");

            // Assert
            Assert.Equal(filterQuery2, result.ReturnFilterQueries());
            Assert.Equal(categoryName2, result.ReturnFilterCategory());
        }

        [Fact]
        public void filterIndexSearching()
        {
            // Arrange
            // Filter 1
            List<string> filterQuery1 = new List<string>()
            {
                "Podcast",
                "Radio",
            };
            string categoryName1 = "Podcast";

            // Filter 2
            List<string> filterQuery2 = new List<string>()
            {
                "Video",
                "Movie",
            };
            string categoryName2 = "Video";

            // Filter 3
            List<string> filterQuery3 = new List<string>()
            {
                "Journal",
            };
            string categoryName3 = "Journal";

            IFilterCollection.GetInstance().Add(new Filter(filterQuery1, categoryName1));
            IFilterCollection.GetInstance().Add(new Filter(filterQuery2, categoryName2));
            IFilterCollection.GetInstance().Add(new Filter(filterQuery3, categoryName3));

            // Act
            int result = IFilterCollection.GetInstance().FindFilterIndex("Video");

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void filterDeletion()
        {
            // Arrange
            // Filter 1
            List<string> filterQuery1 = new List<string>()
            {
                "Podcast",
                "Radio",
            };
            string categoryName1 = "Podcast";

            // Filter 2
            List<string> filterQuery2 = new List<string>()
            {
                "Video",
                "Movie",
            };
            string categoryName2 = "Video";

            // Filter 3
            List<string> filterQuery3 = new List<string>()
            {
                "Journal",
            };
            string categoryName3 = "Journal";

            IFilterCollection.GetInstance().Add(new Filter(filterQuery1, categoryName1));
            IFilterCollection.GetInstance().Add(new Filter(filterQuery2, categoryName2));
            IFilterCollection.GetInstance().Add(new Filter(filterQuery3, categoryName3));

            // Act
            bool result = IFilterCollection.GetInstance().Remove(IFilterCollection.GetInstance().FindFilter("Video"));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void filterCategorisation()
        {
            // Filter 1
            List<string> filterQuery1 = new List<string>()
            {
                "Podcast",
                "Radio",
            };
            string categoryName1 = "Podcast";

            // Add the filter to the collection
            IFilterCollection.GetInstance().Add(new Filter(filterQuery1, categoryName1));

            // Test reference that matches the filter.
            Reference reference = new Reference("Zenref Author", "Radio, Building a reference metadata scraper", "Podcast", "Gruppe 6", 2022, 2, "Software");

            // Act
            string result = IFilterCollection.GetInstance().categorize(reference);

            // Assert
            Assert.Equal(categoryName1, result);
        }

        [Fact]
        public void filterCategorisationOnlyInTitle()
        {
            // Filter 1
            List<string> filterQuery1 = new List<string>()
            {
                "Podcast",
                "Radio",
            };
            string categoryName1 = "Podcast";

            // Add the filter to the collection
            IFilterCollection.GetInstance().Add(new Filter(filterQuery1, categoryName1));

            // Test reference that matches the filter.
            Reference reference = new Reference("Zenref Author", "Radio, Building a reference metadata scraper", "Something else", "Gruppe 6", 2022, 2, "Software");

            // Act
            string result = IFilterCollection.GetInstance().categorize(reference);

            // Assert
            Assert.Equal(categoryName1, result);
        }
    }
}
