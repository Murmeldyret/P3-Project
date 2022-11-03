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

        
    }
}
