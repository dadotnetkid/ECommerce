using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;

namespace Models
{
    public class CriteriaValidator : EvaluatorCriteriaValidator
    {
        private bool isCriteriaOperatorValid = true;
        private CriteriaValidator() : base(null) { }
        public static bool IsCriteriaOperatorValid(CriteriaOperator criteria)
        {
            CriteriaValidator validator = new CriteriaValidator();
            validator.Validate(criteria);
            return validator.isCriteriaOperatorValid;
        }
        public override void Visit(OperandValue theOperand)
        {
            if (theOperand.Value == null)
                isCriteriaOperatorValid = false;
        }
        public override void Visit(JoinOperand theOperand) { }
        public override void Visit(OperandProperty theOperand) { }
        public override void Visit(AggregateOperand theOperand) { }
    }

    //public static class LinqHelper
    //{
    //    static ICriteriaToExpressionConverter Converter { get { return new CriteriaToExpressionConverter(); } }
    //    public static IQueryable<TEntity> ApplyFilter<TEntity>(this IQueryable query, string filterExpression)
    //    {
    //        CriteriaOperator criteria = CriteriaOperator.Parse(filterExpression);
    //        if (CriteriaValidator.IsCriteriaOperatorValid(criteria))
    //            query = query.AppendWhere(Converter, criteria);
    //        return query as IQueryable<TEntity>;
    //    }
    //}
}
