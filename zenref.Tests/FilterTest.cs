using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using zenref.Models;

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

            Assert.Equal(filters, searchFilters.returnFilterQueries());
            Assert.Equal(category, searchFilters.returnFilterCategory());
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

            searchFilters.addFilterQuery("TV");

            Assert.Equal(new List<string>()
            {
                "Podcast",
                "Radio",
                "TV",
            }, searchFilters.returnFilterQueries());
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

            searchFilters.removeFilterQuery("Radio");

            Assert.Equal(new List<string>()
            {
                "Podcast",
                "TV",
            }, searchFilters.returnFilterQueries());
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

            FilterCollection.createFilter(filterQuery, categoryName);

            Filter result = FilterCollection.findFilter("Podcast");

            Assert.Equal(filterQuery, result.returnFilterQueries());
            Assert.Equal(categoryName, result.returnFilterCategory());
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

            FilterCollection.createFilter(filterQuery1, categoryName1);
            FilterCollection.createFilter(filterQuery2, categoryName2);
            FilterCollection.createFilter(filterQuery3, categoryName3);

            // Act
            Filter result = FilterCollection.findFilter("Video");

            // Assert
            Assert.Equal(filterQuery2, result.returnFilterQueries());
            Assert.Equal(categoryName2, result.returnFilterCategory());
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

            FilterCollection.createFilter(filterQuery1, categoryName1);
            FilterCollection.createFilter(filterQuery2, categoryName2);
            FilterCollection.createFilter(filterQuery3, categoryName3);

            // Act
            int result = FilterCollection.findFilterIndex("Video");

            // Assert
            Assert.Equal(2, result);
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

            FilterCollection.createFilter(filterQuery1, categoryName1);
            FilterCollection.createFilter(filterQuery2, categoryName2);
            FilterCollection.createFilter(filterQuery3, categoryName3);

            // Act
            FilterCollection.deleteFilter(2);

            // Assert
            Assert.NotEqual(filterQuery2, FilterCollection.findFilter("Video").returnFilterQueries());
            Assert.NotEqual(categoryName2, FilterCollection.findFilter("Video").returnFilterCategory());
        }
    }
}
