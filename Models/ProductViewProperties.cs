namespace Models
{
    public class ProductViewProperties : IViews
    {
        public PropertyView Name { get; set; }

        public PropertyView MajorDepartment { get; set; }

        public PropertyView Department { get; set; }

        public PropertyView Category { get; set; }

        public PropertyView SubCategory { get; set; }

        public PropertyView Manufacturer { get; set; }

        public PropertyView Brand { get; set; }

        public PropertyView Variants { get; set; }
    }
}
