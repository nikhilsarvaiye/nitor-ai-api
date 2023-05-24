namespace Models
{
    public class PatientSchemaProperties : ISchema
    {
        public PropertySchema Name { get; set; }

        public PropertySchema MajorDepartment { get; set; }

        public PropertySchema Department { get; set; }

        public PropertySchema Category { get; set; }

        public PropertySchema SubCategory { get; set; }

        public PropertySchema Manufacturer { get; set; }

        public PropertySchema Brand { get; set; }

        public PropertySchema Variants { get; set; }
    }
}
