using System;
using System.Linq;

namespace Zenref.Ava.Models
{
    public class DatabaseHelper
    {
        public static Reference GetReference(string title)
        {
            using DataContext db = new();

            Reference reference = db.References
                .FirstOrDefault(b => b.Title == title) ?? new Reference();

            if (reference.Title == null)
            {
                reference.Title = "";
            }

            return reference;
        }
    }
}