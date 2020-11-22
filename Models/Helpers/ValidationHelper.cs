using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Helpers
{
    public static class ModelState
    {
        public static List<string> ErrorMessages = new List<string>();
        public static bool IsValid<TEntity, TEntityMeta>(TEntity model)
        {
            var validationContext = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();

            TypeDescriptor.AddProviderTransparent(
                new AssociatedMetadataTypeTypeDescriptionProvider(typeof(TEntity), typeof(TEntityMeta)), typeof(TEntity));


            if (Validator.TryValidateObject(model, validationContext, results))
            {
                return true;
            }
            else
            {
                ErrorMessages = results.Select(x => x.ErrorMessage).ToList();
                return false;
            }
        }

        
    }
}
