using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using Zenref.Ava.ViewModels;

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
        public void Clear();
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
        private List<Filter> Filters { get; set; } // The list of filters in the collection.

        public int Count => ((ICollection<Filter>)Filters).Count; // The number of filters in the collection.

        public bool IsReadOnly => ((ICollection<Filter>)Filters).IsReadOnly;

        public FilterCollection()
        {
            Filters = new List<Filter>();
            // Load filters from file if it exists.
            LoadFilters();
        }

        /// <summary>
        /// Adds a filter to the collection.
        /// </summary>
        /// <param name="item">The filter to add.</param>
        public void Add(Filter item)
        {
            ((ICollection<Filter>)Filters).Add(item);
        }

        /// <summary>
        /// Removes all filters from the collection.
        /// </summary>
        public void Clear()
        {
            ((ICollection<Filter>)Filters).Clear();
        }
        /// <summary>
        /// Checks if the collection contains a filter.
        /// </summary>
        /// <param name="item">The filter to check for.</param>
        /// <returns>True if the collection contains the filter, false otherwise.</returns>
        public bool Contains(Filter item)
        {
            return ((ICollection<Filter>)Filters).Contains(item);
        }

        /// <summary>
        /// Copies the filters in the collection to an array.
        /// </summary>
        /// <param name="array">The array to copy the filters to.</param>
        /// <param name="arrayIndex">The index to start copying at.</param>
        public void CopyTo(Filter[] array, int arrayIndex)
        {
            ((ICollection<Filter>)Filters).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="FilterCollection"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="FilterCollection"/>.</param>
        /// <returns>true if item was successfully removed from the <see cref="FilterCollection"/>;
        /// otherwise, false. This method also returns false if item is not found in the original
        /// <see cref="FilterCollection"/>.</returns>
        public bool Remove(Filter item)
        {
            return ((ICollection<Filter>)Filters).Remove(item);
        }

        // This code iterates through the collection of filters, returning each one.
        // It is used by the FilterCollection class to allow the collection to be
        // enumerated in a foreach statement.
        public IEnumerator<Filter> GetEnumerator()
        {
            return ((IEnumerable<Filter>)Filters).GetEnumerator();
        }

        // Returns an enumerator that iterates through a collection.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Filters).GetEnumerator();
        }

        /// <summary>
        /// Finds a filter in the collection.
        /// </summary>
        /// <param name="filter">The name of the filter to find.</param>
        /// <returns>The filter if it exists, null otherwise.</returns>
        public Filter FindFilter(string filter)
        {
            return Filters.FirstOrDefault(f => f.ReturnFilterCategory() == filter);
        }
        
        /// <summary>
        /// Finds the index of a filter in the collection.
        /// </summary>
        /// <param name="filter">The name of the filter to find.</param>
        /// <returns>The index of the filter if it exists, -1 otherwise.</returns>
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

        /// <summary>
        /// Loads the filters from a file.
        /// </summary>
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

        /// <summary>
        /// Categorizes a reference.
        /// </summary>
        /// <param name="reference">The reference to categorize.</param>
        /// <returns>The category of the reference returned as a string</returns>
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

        /// <summary>
        /// Formats a reference to a list of strings.
        /// </summary>
        /// <param name="reference">The reference to format.</param>
        /// <returns>The reference formatted as a list of strings.</returns>
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

    /// <summary>
    /// A filter for categorizing references. It implements the <see cref="IEnumerable"/> and <see cref="IFilter"/> interfaces.
    /// </summary>
    public class Filter : IFilter, IEnumerable
    {
        public List<string> filterQuery { get; set; }  // The queries of the filter.
        public string categoryName { get; set; }       // The name of the filter.

        /// <summary>
        /// The constructor that initializes a new instance of the <see cref="Filter"/> class with a category name.
        /// </summary>
        public Filter(string categoryName)
        {
            filterQuery = new List<string>();
            this.categoryName = categoryName;
        }

        /// <summary>
        /// The constructor that initializes a new instance of the <see cref="Filter"/> class with a category name and a list of queries.
        /// </summary>
        public Filter(List<string> filterQuery, string categoryName)
        {
            this.filterQuery = filterQuery;
            this.categoryName = categoryName;
        }

        /// <summary>
        /// A method that adds a query to the filter.
        /// </summary>
        /// <param name="query">The query to add.</param>
        public void AddFilterQuery(string query)
        {
            this.filterQuery.Add(query);
        }

        /// <summary>
        /// A method that removes a query from the filter.
        /// </summary>
        /// <param name="query">The query to remove.</param>
        /// <returns>True if the query was removed, false otherwise.</returns>
        public bool RemoveFilterQuery(string query)
        {
            return filterQuery.Remove(query);
        }

        /// <summary>
        /// A method that returns the queries of the filter.
        /// </summary>
        /// <returns>The queries of the filter.</returns>
        public List<string> ReturnFilterQueries()
        {
            return this.filterQuery;
        }
        
        /// <summary>
        /// A method that returns the category name of the filter.
        /// </summary>
        /// <returns>The category name of the filter as a string.</returns>
        public string ReturnFilterCategory()
        {
            return this.categoryName;
        }
        
        /// <summary>
        /// A method that returns the number of queries in the filter.
        /// </summary>
        /// <returns>The number of queries in the filter.</returns>
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)filterQuery).GetEnumerator();
        }

        /// <summary>
        /// A method that checks if a query is in the filter.
        /// </summary>
        /// <param name="query">The query to check.</param>
        /// <returns>True if the query is in the filter, false otherwise.</returns>
        public bool ContainsQuery(string query)
        {
            return filterQuery.Contains(query);
        }
    }



}