
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SampleHosting.Configuration {
    public static class DataAnnotationsValidator {
        public static bool TryValidate (object instance, out ICollection<ValidationResult> results) {
            var context = new ValidationContext (instance, serviceProvider : null, items : null);
            results = new List<ValidationResult> ();
            return Validator.TryValidateObject (
                instance, context, results,
                validateAllProperties : true
            );
        }
    }
}