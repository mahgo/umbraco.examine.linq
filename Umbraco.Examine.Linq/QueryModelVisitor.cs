using Remotion.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Examine.Linq.Attributes;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using System.Linq.Expressions;
using Umbraco.Examine.Linq.Models;

namespace Umbraco.Examine.Linq
{
    public class QueryModelVisitor : QueryModelVisitorBase
    {
        public List<StringBuilder> andQueries = new List<StringBuilder>();
        public List<StringBuilder> notQueries = new List<StringBuilder>();
        public List<OrderByModel> orderings = new List<OrderByModel>();
        public int take = -1;
        public int skip = -1;

        public override void VisitWhereClause(Remotion.Linq.Clauses.WhereClause whereClause, QueryModel queryModel, int index)
        {
            bool alreadyHasQuery = false;

            var query = new StringBuilder();

            if (whereClause.Predicate.NodeType == ExpressionType.Not || whereClause.Predicate.NodeType == ExpressionType.NotEqual)
            {
                notQueries.Add(query);
            }
            else
            {
                andQueries.Add(query);
            }

            ExpressionTreeVisitor visitor = new ExpressionTreeVisitor(query);
            visitor.VisitExpression(whereClause.Predicate);

            base.VisitWhereClause(whereClause, queryModel, index);
        }

        public override void VisitMainFromClause(Remotion.Linq.Clauses.MainFromClause fromClause, QueryModel queryModel)
        {
            andQueries = new List<StringBuilder>();

            NodeTypeAliasAttribute attribute = (NodeTypeAliasAttribute)fromClause.ItemType.GetCustomAttributes(typeof(NodeTypeAliasAttribute), false).FirstOrDefault();

            if (attribute != null)
            {
                andQueries.Add(new StringBuilder(string.Format("nodeTypeAlias:{0}", attribute.Name)));
            }
            
            base.VisitMainFromClause(fromClause, queryModel);
        }

        public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index)
        {
            base.VisitResultOperator(resultOperator, queryModel, index);
        }

        public override void VisitQueryModel(QueryModel queryModel)
        {
            foreach (var resultOperator in queryModel.ResultOperators)
            {
                if (resultOperator is TakeResultOperator)
                    take = (int)((ConstantExpression)((TakeResultOperator)resultOperator).Count).Value;
                else if (resultOperator is SkipResultOperator)
                    skip = (int)((ConstantExpression)((SkipResultOperator)resultOperator).Count).Value;
            }
            base.VisitQueryModel(queryModel);
        }

        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index)
        {
            if (orderByClause != null && orderByClause.Orderings != null)
            {
                foreach (Ordering ordering in orderByClause.Orderings)
                {
                    MemberExpression expression = (MemberExpression)ordering.Expression;
                    OrderByModel orderByModel = new OrderByModel(ordering.OrderingDirection, GetFieldAttribute(expression));
                    this.orderings.Add(orderByModel);
                }
            }
                  
            base.VisitOrderByClause(orderByClause, queryModel, index);
        }

        private string GetFieldAttribute(MemberExpression expression)
        {
            FieldAttribute fieldAttribute = ExpressionTreeVisitor.GetReferenceSourceAttributeOrSelf(expression);
            string fieldName;
            if (fieldAttribute != null)
            {
                fieldName = fieldAttribute.Name;
            }
            else
            {
                fieldName = expression.Member.Name;
            }

            return fieldName;
        }
    }
}
