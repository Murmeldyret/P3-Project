namespace zenref.Core.Application;

public class NGramiser
{
    public static List<string> GetNGrams(string text, int n)
    {
        var ngrams = new List<string>();
        
        var words = text.Split(' ');
        for (int i = 0; i < words.Length - n + 1; i++)
        {
            ngrams.Add(string.Join(" ", words.Skip(i).Take(n)));
        }
        return ngrams;
    }
}