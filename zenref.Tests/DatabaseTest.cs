using System.Linq;
using Xunit;
using Zenref.Ava.Models;
using System;
using zenref.Ava.Models;
using System.Collections.Generic;

namespace zenref.Tests
{
    public class DatabaseTest : IDisposable
    {
        [Fact]
        public void DatabaseConnection()
        {
            using DataContext context = new();

            Assert.True(context.Database.CanConnect());
        }

        [Fact]
        public void CreateReference()
        {
            using DataContext context = new();

            Reference reference = new()
            {
                Title = "Test Title",
                Author = "Test Author",
            };

            context.References.Add(reference);
            context.SaveChanges();

            Assert.True(context.References.Any());
        }

        [Fact]
        public void DeleteReference()
        {
            using DataContext context = new();

            Reference reference = new()
            {
                Title = "Test Title",
                Author = "Test Author",
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
            using DataContext context = new();

            Reference reference = new()
            {
                Title = "Test Title",
                Author = "Test Author",
            };

            context.References.Add(reference);
            context.SaveChanges();

            Reference? reference2 = context.References.Find(reference);

            Assert.Equal(reference.Title, reference2?.Title);
            Assert.Equal(reference.Author, reference2?.Author);
        }

        [Fact]
        public void UpdateReference()
        {
            using DataContext context = new();

            Reference reference = new()
            {
                Title = "Test Title",
                Author = "Test Author",
            };

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
            using DataContext context = new();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Dispose();
        }
    }
}
