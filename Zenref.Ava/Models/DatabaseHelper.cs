using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zenref.Ava.Models
{
    public class DatabaseHelper
    {
        public static Reference GetReference(string title)
        {
            using DataContext db = new();
            
            Reference reference = db.References
                .FirstOrDefault(b => b.Title == title) ?? throw new InvalidOperationException();

            return reference;
        }
    }
}