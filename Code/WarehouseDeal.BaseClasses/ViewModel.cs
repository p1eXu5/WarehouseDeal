using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.BaseClasses
{
    public abstract class ViewModel : ObservableObject, IDataErrorInfo
    {
        public string this[string columnName] => OnValidate (columnName);

        public string Error => throw new NotImplementedException ();

        protected virtual string OnValidate (string propertyName)
        {
            var context = new ValidationContext (this)
                            {
                                MemberName = propertyName
                            };

            var results = new Collection<ValidationResult> ();
            var isValid = Validator.TryValidateObject (this, context, results, true);

            if (!isValid) {

                ValidationResult result = results.SingleOrDefault(p => p.MemberNames.Any(memberName => memberName == propertyName));

                return result?.ErrorMessage;
            }

            return null;

        }
    }
}
