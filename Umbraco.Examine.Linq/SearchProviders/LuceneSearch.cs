using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Examine;
using Examine.LuceneEngine.Providers;
using Examine.SearchCriteria;
using System.Linq.Expressions;
using Umbraco.Examine.Linq.Models;
using Remotion.Linq.Clauses;
using Umbraco.Examine.Linq.Mapper;
using Lucene.Net.Search;

namespace Umbraco.Examine.Linq.SearchProviders
{
    public class LuceneSearch : ISearcher
    {
        protected string IndexName { get; set; }

        public static Dictionary<string, ISearchCriteria> searchQueryCache { get; set; }

        static LuceneSearch()
        {
            searchQueryCache = new Dictionary<string, ISearchCriteria>();
        }

        public LuceneSearch(string indexName)
        {
            IndexName = indexName;
        }

        public IEnumerable<SearchResult> Search(string query, List<OrderByModel> orderings)
        {
            LuceneSearcher searcher = ExamineManager.Instance.SearchProviderCollection[IndexName] as LuceneSearcher;
            ISearchCriteria searchCriteria = searcher.CreateSearchCriteria().RawQuery(query);
            //searchCriteria = SearchHelper.AddOrderBy(searchCriteria, orderings);
            
            return searcher.Search(searchCriteria).ToList();
        }

        public int Count(string query)
        {
            LuceneSearcher searcher = ExamineManager.Instance.SearchProviderCollection[IndexName] as LuceneSearcher;
            ISearchCriteria searchCriteria = searcher.CreateSearchCriteria().RawQuery(query);
            ISearchResults searchResults = searcher.Search(searchCriteria);
            return searchResults.TotalItemCount;
        }
    }
}
