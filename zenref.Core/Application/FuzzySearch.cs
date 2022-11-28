using Fastenshtein;

namespace zenref.Core.Application;

public class FuzzySearch
{
    public bool IsMatch(string source, string target, int threshold = 0)
    {
        int distance = Levenshtein.Distance(source, target);
        return distance <= threshold;
    }
}