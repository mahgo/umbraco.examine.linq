using Examine.LuceneEngine.SearchCriteria;
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
                        boolOperation = boolOperation.And().OrderBy(orderByModel.Field, orderByModel.SortType);
                    }
                    else
                    {
                        boolOperation = searchCriteria.OrderBy(orderByModel.Field, orderByModel.SortType);
                    }
                }
                else
                {
                    if (boolOperation != null)
                    {
                        boolOperation = boolOperation.And().OrderByDescending(orderByModel.Field, orderByModel.SortType);
                    }
                    else
                    {
                        boolOperation = searchCriteria.OrderByDescending(orderByModel.Field, orderByModel.SortType);
                    }
                }
            }

            if (boolOperation != null)
            {
                searchCriteria = boolOperation.Compile();
            }

            return searchCriteria;
        }

        public static IBooleanOperation OrderBy(this IQuery query, string field, SortType? sortType)
        {
            if (sortType.HasValue)
            {
                return query.OrderBy(new SortableField(field, sortType.Value));
            }
            else
            {
                return query.OrderBy(new SortableField(field));
            }
        }

        public static IBooleanOperation OrderByDescending(this IQuery query, string field, SortType? sortType)
        {
            if (sortType.HasValue)
            {
                return query.OrderByDescending(new SortableField(field, sortType.Value));
            }
            else
            {
                return query.OrderByDescending(new SortableField(field));
            }
        }
    }
}
