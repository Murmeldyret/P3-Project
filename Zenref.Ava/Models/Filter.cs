using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zenref.Ava.Models
{

    public interface IFilterCollection
    {
        public Filter FindFilter(string filter);
        public int FindFilterIndex(string filter);
        public void SaveFilters();
        public void LoadFilters();
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

        public void SaveFilters()
        {
            throw new NotImplementedException();
        }

        public void LoadFilters()
        {
            throw new NotImplementedException();
        }
    }



    /*
    private List<Filter> Filters = new List<Filter>();

    public void deleteFilter(int filterIndex)
    {
        throw new NotImplementedException();
    }

    public Filter findFilter(string categoryName)
    {
        throw new NotImplementedException();
    }

    public int findFilterIndex(string categoryName)
    {
        throw new NotImplementedException();
    }

    public void createFilter(List<string> filters, string categoryName)
    {
        throw new NotImplementedException();
    }*/
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

        public string editFilter()
        {
            throw new NotImplementedException();
        }

        // Search filter query for a specific string.
        public string searchFilter(string query)
        {
            //something something, leder efter en bestemt streng eller et bestemt felt
            throw new NotImplementedException();
        }

        public string categorize()
        {
            //something something, tager searching metoden og på hits sætter den referencen over i den nye fane
            throw new NotImplementedException();
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

