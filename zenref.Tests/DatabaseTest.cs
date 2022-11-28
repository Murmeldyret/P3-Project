using System.Linq;
using Xunit;
using zenref.Core.Models;
using System;
using zenref.Core.Factories;

namespace zenref.Tests
{
    public sealed class DatabaseTest : IDisposable
    {
        [Fact]
        public void DatabaseConnection()
        {
            using ApplicationContext context = new();
            Assert.True(context.Database.CanConnect());
        }

        [Fact]
        public void CreateBookReference()
        {
            // Arrange
            using ApplicationContext context = new();

            ReferenceFactory referenceFactory = new();
            Reference book = referenceFactory.CreateReference();
            
            // Act
            context.References.Add(book);
            context.SaveChanges();

            // Assert
            Assert.True(context.References.Any());
        }

        [Fact]
        public void DeleteReference()
        {
            using ApplicationContext context = new();

            ReferenceFactory referenceFactory = new();
            Reference reference = referenceFactory.CreateReference();

            context.References.Add(reference);
            context.SaveChanges();

            context.References.Remove(reference);
            context.SaveChanges();

            Assert.Empty(context.References);
        }

        [Fact]
        public void FindReference()
        {
            using ApplicationContext context = new();

            ReferenceFactory referenceFactory = new();
            Reference reference = referenceFactory.CreateReference();

            context.References.Add(reference);
            context.SaveChanges();

            Reference? reference2 = context.References.Find(reference);

            Assert.Equal(reference.Title, reference2?.Title);
            Assert.Equal(reference.Author, reference2?.Author);
        }

        [Fact]
        public void UpdateReference()
        {
            using ApplicationContext context = new();

            ReferenceFactory referenceFactory = new();
            Reference reference = referenceFactory.CreateReference();

            context.References.Add(reference);
            context.SaveChanges();

            reference.Title = "Test Title 2";
            reference.Author = "Test Author 2";

            context.References.Update(reference);
            context.SaveChanges();

            Assert.Equal("Test Title 2", reference.Title);
            Assert.Equal("Test Author 2", reference.Author);
        }

        public void Dispose()
        {
            using ApplicationContext context = new();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Dispose();
        }
    }
}
