namespace Models
{
    public class PatientSetting : Setting
    {
        public PatientSchema Schema { get; set; }

        public PatientViews Views { get; set; }
    }
}
