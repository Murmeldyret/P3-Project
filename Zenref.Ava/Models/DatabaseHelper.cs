using System;
using System.Linq;

namespace Zenref.Ava.Models
{
    public class DatabaseHelper
    {
        /// <summary>
        /// Get the reference with the specified title from the database
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static Reference GetReference(string title)
        {
            // Create a new instance of DataContext
            using DataContext db = new();
            
            // Search for the first reference with the specified title
            Reference reference = db.References
                .FirstOrDefault(b => b.Title == title) ?? new Reference();
            
            // If the reference is not found set the Title property to an empty string
            if (reference.Title == null)
            {
                reference.Title = "";
            }
            
            // Return the reference
            return reference;
        }
    }
}