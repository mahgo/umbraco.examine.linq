﻿using Examine;
using Examine.SearchCriteria;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Examine.Linq.Mapper;
using Umbraco.Examine.Linq.Models;

namespace Umbraco.Examine.Linq.SearchProviders
{
    public class ExamineSearch : ISearcher
    {
        protected string IndexName { get; set; }

        public static Dictionary<string, ISearchCriteria> searchQueryCache { get; set; }

        static ExamineSearch()
        {
            searchQueryCache = new Dictionary<string, ISearchCriteria>();
        }

        public ExamineSearch(string indexName)
        {
            IndexName = indexName;
        }

        public IEnumerable<SearchResult> Search(string query, List<OrderByModel> orderings)
        {
            ISearchCriteria criteria = null;
            var searcher = ExamineManager.Instance.SearchProviderCollection[IndexName];

            if (searchQueryCache.ContainsKey(query))
                criteria = searchQueryCache[query];
            else
            {
                criteria = searcher.CreateSearchCriteria();
                criteria = criteria.RawQuery(query);
                searchQueryCache.Add(query, criteria);
            }

            //criteria = SearchHelper.AddOrderBy(criteria, orderings);

            return searcher.Search(criteria);
        }

        public int Count(string query)
        {
            ISearchCriteria criteria = null;
            var searcher = ExamineManager.Instance.SearchProviderCollection[IndexName];

            if (searchQueryCache.ContainsKey(query))
                criteria = searchQueryCache[query];
            else
            {
                criteria = searcher.CreateSearchCriteria();
                criteria = criteria.RawQuery(query);
                searchQueryCache.Add(query, criteria);
            }

            ISearchResults searchResults = searcher.Search(criteria);
            return searchResults.TotalItemCount;
        }
    }
}
