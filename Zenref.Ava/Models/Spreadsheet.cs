using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Zenref.Ava.Models
{
    public class Spreadsheet
    {
        [Key]
        public int Id { get; set; } // Primary key
        public int ReferenceCount { get; }
        public bool State { get; }
    }
}
