using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
            Reference reference = new Reference(new RawReference("Edu", "Location", "Semester", "2", "OriReference"), "Zenref Author", "Radio, Building a reference metadata scraper", "Podcast", "Gruppe 6", 2022);

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
            Reference reference = new Reference(new RawReference("Edu", "Location", "Semester", "2", "OriReference"), "Zenref Author", "Radio, Building a reference metadata scraper", "Something else", "Gruppe 6", 2022);

            // Act
            string result = IFilterCollection.GetInstance().categorize(reference);

            // Assert
            Assert.Equal(categoryName1, result);
        }

        [Fact]
        public void loadFilterFileExists()
        {
            // Arrange
            // Save test file to disk.
            StreamWriter writer = new StreamWriter("Filters.csv");
            try
            {
                writer.WriteLine("Podcast,Podcast,Radio\nVideo,Video,Movie\nJournal,Journal");
            }
            finally
            {
                // Close the writer regardless of what happens...
                writer.Close();
            }

            // Create assert filters that matches the test file.
            List<string> filterQuery1 = new List<string>()
            {
                "Podcast",
                "Radio",
            };
            string categoryName1 = "Podcast";
            List<string> filterQuery2 = new List<string>()
            {
                "Video",
                "Movie",
            };
            string categoryName2 = "Video";
            List<string> filterQuery3 = new List<string>()
            {
                "Journal",
            };
            string categoryName3 = "Journal";

            // Act
            IFilterCollection.GetInstance().LoadFilters();

            // Assert
            Assert.Equal(filterQuery1, IFilterCollection.GetInstance().FindFilter("Podcast").ReturnFilterQueries());
            Assert.Equal(categoryName1, IFilterCollection.GetInstance().FindFilter("Podcast").ReturnFilterCategory());
            Assert.Equal(filterQuery2, IFilterCollection.GetInstance().FindFilter("Video").ReturnFilterQueries());
            Assert.Equal(categoryName2, IFilterCollection.GetInstance().FindFilter("Video").ReturnFilterCategory());
            Assert.Equal(filterQuery3, IFilterCollection.GetInstance().FindFilter("Journal").ReturnFilterQueries());
            Assert.Equal(categoryName3, IFilterCollection.GetInstance().FindFilter("Journal").ReturnFilterCategory());

            // Delete the test file.
            File.Delete("Filters.csv");
        }

        [Fact]
        public void loadFilterFileDoesNotExist()
        {
            // Arrange
            // Delete the test file.
            File.Delete("Filters.csv");

            // Act
            IFilterCollection.GetInstance().LoadFilters();

            // Assert
            // Assert that the collection is empty.
            Assert.Equal(IFilterCollection.GetInstance().Count, 0);
        }

        [Fact]
        public void saveFilterCollectionExists()
        {
            // Arrange
            // Add filters to filtercollection.
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
            IFilterCollection.GetInstance().SaveFilters();

            // Assert
            // Read the file and assert that it matches the filters.
            using (StreamReader reader = new StreamReader("Filters.csv"))
            {
                string line = reader.ReadLine();
                Assert.Equal("Podcast,Podcast,Radio", line);
                line = reader.ReadLine();
                Assert.Equal("Video,Video,Movie", line);
                line = reader.ReadLine();
                Assert.Equal("Journal,Journal", line);
            }
        }

        [Fact]
        public void saveFilterCollectionDoesNotExist()
        {
            // Arrange
            // Delete the test file.
            File.Delete("Filters.csv");

            IFilterCollection filterCollection = IFilterCollection.GetInstance();
            filterCollection.Clear();

            // Act
            filterCollection.SaveFilters();

            // Assert
            // Check that the file is empty.
            using (StreamReader reader = new StreamReader("Filters.csv"))
            {
                string line = reader.ReadLine();
                Assert.Null(line);
            }
        }
    }
}

