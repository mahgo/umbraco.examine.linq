using Examine.SearchCriteria;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Examine.Linq.Models;

namespace Umbraco.Examine.Linq.Mapper
{
    public static class SearchHelper
    {
        public static ISearchCriteria AddOrderBy(ISearchCriteria searchCriteria, List<OrderByModel> orderings)
        {
            IBooleanOperation boolOperation = null;
            foreach (OrderByModel orderByModel in orderings)
            {
                if (orderByModel.Direction == OrderingDirection.Asc)
                {
                    if (boolOperation != null)
                    {
                        boolOperation = boolOperation.And().OrderBy(orderByModel.Field);
                    }
                    else
                    {
                        boolOperation = searchCriteria.OrderBy(orderByModel.Field);
                    }
                }
                else
                {
                    if (boolOperation != null)
                    {
                        boolOperation = boolOperation.And().OrderByDescending(orderByModel.Field);
                    }
                    else
                    {
                        boolOperation = searchCriteria.OrderByDescending(orderByModel.Field);
                    }
                }
            }

            if (boolOperation != null)
            {
                searchCriteria = boolOperation.Compile();
            }

            return searchCriteria;
        }
    }
}
