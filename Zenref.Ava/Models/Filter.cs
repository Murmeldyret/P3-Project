using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.IO;
using Zenref.Ava.ViewModels;

namespace Zenref.Ava.Models
{
    public interface ISingleton
    {
        // : Property to hold the singleton instance of FilterCollection
        public static FilterCollection? instance { get; set; }
        
        // : Method to get the singleton instance of FilterCollection
        public static FilterCollection GetInstance()
        {
            // : Check if the instance is null, if it is create a new instance
            if (instance == null)
            {
                instance = new FilterCollection();
            }
            
            // : Return the instance
            return instance;
        }

    }
    public interface IFilterCollection : ISingleton
    {
        // : Find filter by its name.
        public Filter FindFilter(string filter);
        
        // : Find the index of a filter in the collection.
        public int FindFilterIndex(string filter);
        
        // : Save all filters to a file.
        public void SaveFilters();
        
        // : Load filters from a file.
        public void LoadFilters();
        
        // : Categorize a reference into a filter.
        public string categorize(Reference reference);
        
        // : Clear all filters from the collection.
        public void Clear();
    }

    public interface IFilter
    {
        // : Add a filter query to a filter.
        public void AddFilterQuery(string query);
        
        // : Remove a filter query from a filter.
        public bool RemoveFilterQuery(string query);
        
        // : Get the list of filter queries for a filter.
        public List<string> ReturnFilterQueries();
        
        // : Get the name of a filter.
        public string ReturnFilterCategory();
        
        // : Check if a filter contains a specific query.
        public bool ContainsQuery(string query);
    }

    // : Define a FilterCollection class that implements ICollection<Filter> and IFilterCollection interface
    public class FilterCollection : ICollection<Filter>, IFilterCollection
    {
        // : Private list of filters in the collection
        private List<Filter> Filters { get; set; } // The list of filters in the collection.

        // : Property to get the number of filters in the collection
        public int Count => ((ICollection<Filter>)Filters).Count; // The number of filters in the collection.

        // : Property to check if the collection is read-only
        public bool IsReadOnly => ((ICollection<Filter>)Filters).IsReadOnly;

        // : Constructor to initialize the Filters list and load filters from the file if it exists
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
            // : Loop through all filters in the filter collection.
            for (int i = 0; i < Filters.Count; i++)
            {
                // : Check if the filter category of the current filter matches the input filter.
                if (Filters[i].ReturnFilterCategory() == filter)
                {
                    // : If a match is found, return the index of the filter.
                    return i;
                }
            }
            
            // : If no match is found, return -1.
            return -1;
        }

        /// <summary>
        /// SaveFilters method to save filters to file
        /// </summary>
        public void SaveFilters()
        {
            // : Create a new write stream to "Filters.csv" file
            using (StreamWriter writer = new StreamWriter("Filters.csv"))
            {
                // : loop through each filter in Filters collection
                foreach (Filter filter in Filters)
                {
                    // : write the filter category returned by ReturnFilterCategory method to the file
                    writer.Write(filter.ReturnFilterCategory());
                    
                    // : loop through each filter query returned by ReturnFilterQueries method 
                    foreach (string query in filter.ReturnFilterQueries())
                    {
                        // : write the "," separator between filter queries
                        writer.Write(",");
                        
                        // : write the filter query to the file
                        writer.Write(query);
                    }
                    
                    // : write a new line after each filter
                    writer.WriteLine();
                }
            }
        }

        /// <summary>
        /// Loads the filters from a file.
        /// </summary>
        public void LoadFilters()
        {
            // : Clear the current filter collection.
            Filters.Clear();

            // : Check if the file "Filters.csv" exists.
            if (File.Exists("Filters.csv"))
            {
                // : Open the file "Filters.csv" for reading.
                using (StreamReader sr = new StreamReader("Filters.csv"))
                {

                    // Read all lines from the file.
                    // : Loop until the end of the file.
                    while (!sr.EndOfStream)
                    {
                        // : Read a line from the file.
                        string line = sr.ReadLine()!;
                        
                        // : Split the line into an array of strings using comma as a separator.
                        string[] values = line.Split(',');
                        
                        // : Create a new filter using the first value in the array.
                        Filter filter = new Filter(values[0]);
                        
                        // Add all queries to the filter.
                        // : Loop through the rest of the values in the array.
                        for (int i = 1; i < values.Length; i++)
                        {
                            // : Add the filter query to the filter.
                            filter.AddFilterQuery(values[i]);
                        }
                        
                        // : Add the filter to the collection of filters.
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
                        string[] textWords = new string[] {""};
                        if (text != null)
                        {

                            // Splits the query into words.
                            textWords = text.Split(' ');
                        }
                        // Match every word in the query to the reference.

                        foreach (string queryWord in textWords)
                        {
                            // Using the Levenshtein distance algorithm to determine if the filter query is similar to the reference.
                            int distance = Fastenshtein.Levenshtein.Distance(queryWord, filterQueries);
                            // Calculating the percentage certainty of the match.
                            double certainty = 1 - (double)distance / Math.Max(queryWord.Length, filterQueries.Length);

                            // If the certainty is greater than the global percent, return the filter category.
                            if (certainty > 0.7)              
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
        public ObservableCollection<SearchTerms> filtQ { get; set; }
        public List<string> filterQuery { get; set; }  // The queries of the filter.
        public string categoryName { get; set; }       // The name of the filter.
        public bool cancel { get; set; }

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

        public Filter(ObservableCollection<SearchTerms> filtQ, List<string> filterQuery, string categoryName)
        {
            this.filtQ = filtQ;
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
