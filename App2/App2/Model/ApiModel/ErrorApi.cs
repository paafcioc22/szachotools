using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model.ApiModel
{
    public partial class ErrorApi
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public string AttemptedValue { get; set; }
        public object CustomState { get; set; }
        public long? Severity { get; set; }
        public string ErrorCode { get; set; }
        public FormattedMessagePlaceholderValues FormattedMessagePlaceholderValues { get; set; }
    }

    public partial class FormattedMessagePlaceholderValues
    {
        public string RegularExpression { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
    }
}
