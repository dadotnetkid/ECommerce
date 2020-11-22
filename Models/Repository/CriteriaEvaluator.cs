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

namespace Helpers
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

   
}
