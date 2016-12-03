using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Examine.Linq.Models;

namespace Umbraco.Examine.Linq
{
    public interface ISearcher
    {
        IEnumerable<SearchResult> Search(string query, List<OrderByModel> orderings);
    }
}
