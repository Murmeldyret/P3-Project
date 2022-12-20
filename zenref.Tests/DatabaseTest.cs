using System;
using Xunit;
using System.Linq;
using Zenref.Ava.Models;
using Zenref.Ava.Factories;

namespace zenref.Tests
{
    public class DatabaseTest : IDisposable
    {
        [Fact]
        public void DatabaseConnection()
        {
            // Use a DataContext object to test database connectivity
            using DataContext context = new();

            // If the CanConnect method returns true, the test is considered to have passed.
            Assert.True(context.Database.CanConnect());
        }

        [Fact]
        public void CreateReference()
        {
            // Use a DataContext object
            using DataContext context = new();

            // Create a new reference object
            var factory = new ReferenceFactory();
            var reference = factory.CreateReference();

            // Add the reference to the database
            context.References.Add(reference);
            context.SaveChanges();

            // Check if there are any records in the References table
            Assert.True(context.References.Any());
        }

        [Fact]
        public void DeleteReference()
        {
            // Use a DataContext object
            using DataContext context = new();

            // Create a new reference object
            var factory = new ReferenceFactory();
            var reference = factory.CreateReference();

            // Add the reference to the database
            context.References.Add(reference);
            context.SaveChanges();

            // Remove the reference to the database
            context.References.Remove(reference);
            context.SaveChanges();

            Assert.Empty(context.References);
        }

        [Fact]
        public void FindReference()
        {
            // Use a DataContext object
            using DataContext context = new();

            // Create a new reference object
            var factory = new ReferenceFactory();
            var reference = factory.CreateReference();

            // Add the reference to the database
            context.References.Add(reference);
            context.SaveChanges();

            // Find the reference in the database
            Reference? reference2 = context.References.Find(reference);

            // Check if the reference was found
            Assert.Equal(reference.Title, reference2?.Title);
            Assert.Equal(reference.Author, reference2?.Author);
        }

        [Fact]
        public void UpdateReference()
        {
            // Use a DataContext object
            using DataContext context = new();

            // Create a new reference object
            var factory = new ReferenceFactory();
            var reference = factory.CreateReference();

            // Add the reference to the database
            context.References.Add(reference);
            context.SaveChanges();

            // Update the reference
            reference.Title = "Test Title 2";
            reference.Author = "Test Author 2";

            // Save the changes
            context.References.Update(reference);
            context.SaveChanges();

            // Find the reference in the database
            Assert.Equal("Test Title 2", reference.Title);
            Assert.Equal("Test Author 2", reference.Author);
        }
        
        [Fact]
        public void ReferenceToDatabaseEmpty()
        {
            // Use a DataContext object
            using DataContext context = new();

            // Create a new reference object
            var factory = new ReferenceFactory();
            var reference = factory.CreateReference();

            // Add the reference to the database
            context.References.Add(reference);
            context.SaveChanges();

            // Check if there are any records in the References table
            Assert.True(context.References.Any());

            // Find the reference in the database
            Reference? reference2 = context.References.Find(reference);

            // Check if the reference was found
            Assert.Equal(reference.Title, reference2?.Title);
            Assert.Equal(reference.Author, reference2?.Author);
        }

        public void Dispose()
        {
            // Use a DataContext object
            using DataContext context = new();

            // Delete all records in the References table in the database
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Dispose();
        }
    }
}
