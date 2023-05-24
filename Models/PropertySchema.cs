namespace Models
{
    using System.Collections.Generic;
    
    public class PropertySchema
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public string Type { get; set; }

        public string ValidationType { get; set; }

        public object DefaultValue { get; set; }

        public List<PropertyValidation> Validations { get; set; }
    }
}
