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
    }

    public class FilterCollectionTest
    {
        [Fact]
        public void filterCreation()
        {
            FilterCollection filters = new FilterCollection();
            List<string> filterQuery = new List<string>()
            {
                "Podcast",
                "Radio",
            };

            string categoryName = "Podcast";

            Filter filter = new Filter(filterQuery, categoryName);

            filters.Add(filter);

            Assert.True(filters.Contains(filter));
        }

        [Fact]
        public void filterSearching()
        {
            // Arrange

            FilterCollection filters = new FilterCollection();
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

            filters.Add(new Filter(filterQuery1, categoryName1));
            filters.Add(new Filter(filterQuery2, categoryName2));
            filters.Add(new Filter(filterQuery3, categoryName3));

            // Act
            Filter result = filters.FindFilter("Video");

            // Assert
            Assert.Equal(filterQuery2, result.ReturnFilterQueries());
            Assert.Equal(categoryName2, result.ReturnFilterCategory());
        }

        [Fact]
        public void filterIndexSearching()
        {
            // Arrange
            FilterCollection filters = new FilterCollection();
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

            filters.Add(new Filter(filterQuery1, categoryName1));
            filters.Add(new Filter(filterQuery2, categoryName2));
            filters.Add(new Filter(filterQuery3, categoryName3));

            // Act
            int result = filters.FindFilterIndex("Video");

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void filterDeletion()
        {
            // Arrange
            FilterCollection filters = new FilterCollection();
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

            filters.Add(new Filter(filterQuery1, categoryName1));
            filters.Add(new Filter(filterQuery2, categoryName2));
            filters.Add(new Filter(filterQuery3, categoryName3));

            // Act
            bool result = filters.Remove(filters.FindFilter("Video"));

            // Assert
            Assert.True(result);
        }
    }
}
