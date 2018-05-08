using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealTimeThemingEngine.Web.Common.Exceptions
{
    public class SiteValidationException : Exception
    {
        public SiteValidationException(IEnumerable<ValidationResult> validationResults) : base()
        {
            ValidationErrors = validationResults;
        }

        public IEnumerable<ValidationResult> ValidationErrors { get; private set; }
    }
}