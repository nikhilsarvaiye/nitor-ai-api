namespace Models
{
    using System;

    public class User : BaseModel
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public UserType Type { get; set; } = UserType.System;

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string PasswordHash { get; set; }

        public File Picture { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime? DOB { get; set; }

        public string Theme { get; set; }

        public Gender Gender { get; set; } = Gender.Unknown;

        public string Address { get; set; }

        public string Area { get; set; }

        public string Landmark { get; set; }

        public string City { get; set; }

        public string PinCode { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string Mobile { get; set; }

        public string AlternateMobile { get; set; }
    }
}
