namespace Services.Transformers
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class UserTransformer : BaseTransformer<User>
    {
        public override User Transform(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                var names = new List<string> { user.FirstName, user.MiddleName, user.LastName };
                user.Name = string.Join(" ", names.Where(x => !string.IsNullOrWhiteSpace(x))).Trim();
            }

            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                user.FirstName = user.Name.Split(' ')[0];
            }

            // Only Create scenario
            if(string.IsNullOrEmpty(user.Id))
            {
                if (string.IsNullOrWhiteSpace(user.UserId))
                {
                    user.UserId = user.Name.Replace(" ", string.Empty);
                }

                if (string.IsNullOrWhiteSpace(user.Password))
                {
                    user.Password = $"{user.UserId}@{user.Mobile}";
                    user.ConfirmPassword = user.Password;
                }
            }

            return user;
        }
    }
}
