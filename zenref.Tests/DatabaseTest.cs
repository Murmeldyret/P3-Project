using System.Linq;
using Xunit;
using Zenref.Ava.Models;

namespace zenref.Tests
{
    public class DatabaseTest
    {

        [Fact]
        public async void CanMakeReference()
        {
            DatabaseHelper.DeleteFile("zenref.db");

            await using DataContext context = new DataContext();
            await context.Database.EnsureCreatedAsync();
            await context.References.AddRangeAsync(new Reference[] {
                new Reference {
                    Author = "Author",
                    BookTitle = "BookTitle",
                    Chapters = 5,
                    Commentary = "Commentary",
                    DOI = "30-30-30",
                    Edu = "Edu",
                    ExamEvent = "ExamEvent",
                    ISBN = "24-24-24",
                    Language = "Language",
                    Location = "Location",
                    Match = 123,
                    Pages = 123,
                }
            });

            await context.SaveChangesAsync();

            Assert.Equal(1, context.References.Count());
            Assert.Equal("Author", context.References.First().Author);
        }
    }
}
