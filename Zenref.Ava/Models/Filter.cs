using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenref.Ava.Models
{

    public static class FilterCollection
    {
        private static List<Filter> Filters = new List<Filter>();

        public static void deleteFilter(int filterIndex)
        {
            throw new NotImplementedException();
        }

        public static Filter findFilter(string categoryName)
        {
            throw new NotImplementedException();
        }

        public static int findFilterIndex(string categoryName)
        {
            throw new NotImplementedException();
        }

        public static void createFilter(List<string> filters, string categoryName)
        {
            throw new NotImplementedException();
        }
    }

    public class Filter
    {
        protected List<string> filterQuery;
        protected string categoryName;

        public Filter(List<string> filterQuery, string categoryName)
        {
            this.filterQuery = filterQuery;
            this.categoryName = categoryName;
        }

        public void createFilter(List<string> filter, string category)
        {
            //UI? Button testing?
            throw new NotImplementedException();
        }

        public void addFilterQuery(string query)
        {
            this.filterQuery.Append(query);
        }

        public bool removeFilterQuery(string query)
        {
            return filterQuery.Remove(query);
        }

        public List<string> returnFilterQueries()
        {
            return this.filterQuery;
        }

        public string returnFilterCategory()
        {
            return this.categoryName;
        }

        public string saveFilter()
        {
            throw new NotImplementedException();
        }

        public List<Filter> loadFilters()
        {
            throw new NotImplementedException();
        }

        public string editFilter()
        {
            throw new NotImplementedException();
        }

        public string deleteFilter()
        {
            throw new NotImplementedException();
        }
        
        public string searchFilter()
        {
            //something something, leder efter en bestemt streng eller et bestemt felt
            throw new NotImplementedException();
        }

        public string categorize()
        {
            //something something, tager searching metoden og på hits sætter den referencen over i den nye fane
            throw new NotImplementedException();
        }
    }
}