namespace Models
{
    using System.Collections.Generic;
    
    public class PropertyValidation
    {
        public string Type { get; set; }

        public string ErrorMessage { get; set; }

        public List<object> Params { get; set; } = new List<object>();
    }
}
