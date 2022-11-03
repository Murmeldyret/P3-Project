using System.Linq;
using Xunit;
using Zenref.Ava.Models;
using System;

namespace zenref.Tests
{
    public class DatabaseTest : IDisposable
    {
        [Fact]
        public void DatabaseConnection()
        {
            using DataContext context = new DataContext();

            Assert.True(context.Database.CanConnect());
        }

        [Fact]
        public void CreateReference()
        {
            using DataContext context = new DataContext();

            Reference reference = new Reference
            {
                Title = "Test Title",
                Author = "Test Author",
                ISBN = "Test ISBN",
                DOI = "Test DOI",
            };

            context.References.Add(reference);
            context.SaveChanges();

            Assert.True(context.References.Any());
        }

        [Fact]
        public void DeleteReference()
        {
            using DataContext context = new DataContext();

            Reference reference = new Reference
            {
                Title = "Test Title",
                Author = "Test Author",
                ISBN = "Test ISBN",
                DOI = "Test DOI",
            };

            context.References.Add(reference);
            context.SaveChanges();

            context.References.Remove(reference);
            context.SaveChanges();

            Assert.Empty(context.References);
        }

        [Fact]
        public void FindReference()
        {
            using DataContext context = new DataContext();

            Reference reference = new Reference
            {
                Title = "Test Title",
                Author = "Test Author",
                ISBN = "Test ISBN",
                DOI = "Test DOI",
            };

            context.References.Add(reference);
            context.SaveChanges();

            Reference reference2 = context.References.Find(reference.Id);

            Assert.Equal(reference.Id, reference2.Id);
            Assert.Equal(reference.Title, reference2.Title);
            Assert.Equal(reference.Author, reference2.Author);
            Assert.Equal(reference.ISBN, reference2.ISBN);
            Assert.Equal(reference.DOI, reference2.DOI);
        }

        [Fact]
        public void UpdateReference()
        {
            using DataContext context = new DataContext();

            Reference reference = new Reference
            {
                Title = "Test Title",
                Author = "Test Author",
                ISBN = "Test ISBN",
                DOI = "Test DOI",
            };

            context.References.Add(reference);
            context.SaveChanges();

            reference.Title = "Test Title 2";
            reference.Author = "Test Author 2";
            reference.ISBN = "Test ISBN 2";
            reference.DOI = "Test DOI 2";

            context.References.Update(reference);
            context.SaveChanges();

            Assert.Equal("Test Title 2", reference.Title);
            Assert.Equal("Test Author 2", reference.Author);
            Assert.Equal("Test ISBN 2", reference.ISBN);
            Assert.Equal("Test DOI 2", reference.DOI);
        }

        public void Dispose()
        {
            using DataContext context = new DataContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Dispose();
        }
    }
}
