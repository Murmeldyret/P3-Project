using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Zenref.Ava.Models
{
    public interface ISingleton
    {
        public static FilterCollection? instance { get; set; }
        public static FilterCollection GetInstance()
        {
            if (instance == null)
            {
                instance = new FilterCollection();
            }
            return instance;
        }

    }
    public interface IFilterCollection : ISingleton
    {
        public Filter FindFilter(string filter);
        public int FindFilterIndex(string filter);
        public void SaveFilters();
        public void LoadFilters();
        public string categorize(Reference reference);
    }

    public interface IFilter
    {
        public void AddFilterQuery(string query);
        public bool RemoveFilterQuery(string query);
        public List<string> ReturnFilterQueries();
        public string ReturnFilterCategory();
        public bool ContainsQuery(string query);


    }

    public class FilterCollection : ICollection<Filter>, IFilterCollection
    {
        private List<Filter> Filters { get; set; }

        public int Count => ((ICollection<Filter>)Filters).Count;

        public bool IsReadOnly => ((ICollection<Filter>)Filters).IsReadOnly;

        public FilterCollection()
        {
            Filters = new List<Filter>();
        }

        public void Add(Filter item)
        {
            ((ICollection<Filter>)Filters).Add(item);
        }

        public void Clear()
        {
            ((ICollection<Filter>)Filters).Clear();
        }

        public bool Contains(Filter item)
        {
            return ((ICollection<Filter>)Filters).Contains(item);
        }

        public void CopyTo(Filter[] array, int arrayIndex)
        {
            ((ICollection<Filter>)Filters).CopyTo(array, arrayIndex);
        }

        public bool Remove(Filter item)
        {
            return ((ICollection<Filter>)Filters).Remove(item);
        }

        public IEnumerator<Filter> GetEnumerator()
        {
            return ((IEnumerable<Filter>)Filters).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Filters).GetEnumerator();
        }

        public Filter FindFilter(string filter)
        {
            return Filters.FirstOrDefault(f => f.ReturnFilterCategory() == filter);
        }

        public int FindFilterIndex(string filter)
        {
            for (int i = 0; i < Filters.Count; i++)
            {
                if (Filters[i].ReturnFilterCategory() == filter)
                {
                    return i;
                }
            }
            return -1;
        }

        // <summary>
        // Saves the filters to a file
        // </summary>
        public void SaveFilters()
        {
            // Create a new write stream.
            using (StreamWriter writer = new StreamWriter("Filters.csv"))
            {
                // Write the filterCollction to the file.
                foreach (Filter filter in Filters)
                {
                    writer.Write(filter.ReturnFilterCategory());
                    foreach (string query in filter.ReturnFilterQueries())
                    {
                        writer.Write(",");
                        writer.Write(query);
                    }
                    writer.WriteLine();
                }
            }
        }

        public void LoadFilters()
        {
            // Clear the current filter collection.
            Filters.Clear();

            // Check if file exists.
            if (File.Exists("Filters.csv"))
            {
                // Load the filters from the file.
                using (StreamReader sr = new StreamReader("Filters.csv"))
                {

                    // Read all lines from the file.
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine()!;
                        string[] values = line.Split(',');
                        Filter filter = new Filter(values[0]);
                        // Add all queries to the filter.
                        for (int i = 1; i < values.Length; i++)
                        {
                            filter.AddFilterQuery(values[i]);
                        }
                        Filters.Add(filter);
                    }
                }
            }
        }

        public string categorize(Reference reference)
        {
            // Format reference to a list of strings.
            List<string> referenceList = ReferenceFormatter(reference);

            // Loop through each filter.
            foreach (Filter filter in Filters)
            {
                // Loop through each query in the filter.
                foreach (string filterQueries in filter.ReturnFilterQueries())
                {
                    // Loop through each word in the reference.
                    foreach (string text in referenceList)
                    {
                        // Splits the query into words.
                        string[] textWords = text.Split(' ');
                        // Match every word in the query to the reference.

                        foreach (string queryWord in textWords)
                        {
                            // Using the Levenshtein distance algorithm to determine if the filter query is similar to the reference.
                            int distance = Fastenshtein.Levenshtein.Distance(queryWord, filterQueries);
                            // Calculating the percentage certainty of the match.
                            double certainty = 1 - (double)distance / Math.Max(queryWord.Length, filterQueries.Length);

                            // If the certainty is greater than the global percent, return the filter category.
                            if (certainty > 0.7)              //! Replace this with a global variable.
                            {
                                return filter.ReturnFilterCategory();
                            }
                        }
                    }
                }
            }

            return "Uncategorized";

        }

        private List<string> ReferenceFormatter(Reference reference)
        {
            // Format reference to a large string with space for sep
            // and return a list of strings.
            List<string> formattedReference = new List<string>()
            {
                reference.Author,
                reference.Title,
                reference.PubType,
                reference.Publisher,
                reference.Location,
                reference.Source,
                reference.BookTitle,
            };

            return formattedReference;
        }
    }

    public class Filter : IFilter, IEnumerable
    {
        protected List<string> filterQuery;
        protected string categoryName;

        public Filter(string categoryName)
        {
            filterQuery = new List<string>();
            this.categoryName = categoryName;
        }

        public Filter(List<string> filterQuery, string categoryName)
        {
            this.filterQuery = filterQuery;
            this.categoryName = categoryName;
        }

        public void AddFilterQuery(string query)
        {
            this.filterQuery.Add(query);
        }

        public bool RemoveFilterQuery(string query)
        {
            return filterQuery.Remove(query);
        }

        public List<string> ReturnFilterQueries()
        {
            return this.filterQuery;
        }

        public string ReturnFilterCategory()
        {
            return this.categoryName;
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)filterQuery).GetEnumerator();
        }

        public bool ContainsQuery(string query)
        {
            return filterQuery.Contains(query);
        }
    }



}