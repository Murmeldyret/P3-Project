using System.ComponentModel.DataAnnotations;

namespace zenref.Core.Models;

public class Reference
{
    [Key]
    public int Id { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string PubType { get; set; }
    public string Publisher { get; set; }
    public int Year { get; set; }
    public int IDRef { get; set; }
    public string Edu { get; set; }
    public string Location { get; set; }
    public string Semester { get; set; }
    public string Language { get; set; }
    public int YearReport { get; set; }
    public string OriReference { get; set; }
    public double Match { get; set; }
    public string Commentary { get; set; }
    public string Syllabus { get; set; }
    public string Season { get; set; }
    public string ExamEvent { get; set; }
    public string Source { get; set; }
    public int Pages { get; set; }
    public string ISBN { get; set; }
    public string Volume { get; set; }
    public string Chapters { get; set; }
    public string BookTitle { get; set; }
}