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
    }
}
