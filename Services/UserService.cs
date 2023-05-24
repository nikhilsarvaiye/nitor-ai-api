namespace Services
{
    using Common.Handlers;
    using Configuration.Options;
    using DotnetStandardQueryBuilder.Core;
    using FluentValidation;
    using Models;
    using Repositories;
    using Services.Transformers;
    using Services.Validators;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserService : BaseService<User>, IUserService
    {
        private readonly IPatientSettingService _patientSettingService;

        public UserService(IAppOptions appOptions, ICacheService<User> cacheService, IUserRepository userRepository, IPatientSettingService patientSettingService)
            : base(appOptions, cacheService, userRepository)
        {
            _patientSettingService = patientSettingService;
        }

        public override async Task<User> CreateAsync(User user)
        {
            user = new UserTransformer().Transform(user);

            await OnCreating(user).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(user.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = null;
                user.ConfirmPassword = null;
            }

            return await Repository.CreateAsync(user).ConfigureAwait(false);
        }

        public override async Task<List<User>> GetAsync(IRequest request = null)
        {
            var users = await Repository.GetAsync(request).ConfigureAwait(false);

            foreach (var user in users)
            {
                user.Picture = user.Files?.FirstOrDefault();
            }

            return users;
        }

        public override async Task<string> UpdateAsync(string id, User user)
        {
            user = new UserTransformer().Transform(user);

            return await base.UpdateAsync(id, user).ConfigureAwait(false);
        }

        public async Task<LoggedInUser> LogInAsync(string id, string password)
        {
            var user = (await GetAsync(new Request
            {
                Filter = new CompositeFilter
                {
                    LogicalOperator = LogicalOperator.Or,
                    Filters = new List<IFilter>
                    {
                        new Filter
                        {
                            Operator = FilterOperator.IsEqualTo,
                            Property = nameof(User.UserId),
                            Value = id
                        },
                        new Filter
                        {
                            Operator = FilterOperator.IsEqualTo,
                            Property = nameof(User.Email),
                            Value = id
                        },
                        new Filter
                        {
                            Operator = FilterOperator.IsEqualTo,
                            Property = nameof(User.Mobile),
                            Value = id
                        },
                        new Filter
                        {
                            Operator = FilterOperator.IsEqualTo,
                            Property = nameof(User.AlternateMobile),
                            Value = id
                        }
                    }
                },
                PageSize = 1
            }).ConfigureAwait(false)).FirstOrDefault();

            if (user == null)
            {
                throw new Exception($"No User found with User Id {id}.");
            }

            if (string.IsNullOrEmpty(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new Exception($"Invalid Password, please try again.");
            }

            var accesssToken = new JwtSecurityTokenHandler().GenerateJwtToken(user, AppOptions);

            // Clear fields
            user.Password = null;
            user.ConfirmPassword = null;
            user.PasswordHash = null;

            return new LoggedInUser
            {
                User = user,
                AccessToken = accesssToken,

                // TODO: Cache and do better
                AppSettings = new AppSettings
                {
                    Patient = (await _patientSettingService.GetAsync(new Request
                    {
                        Filter =
                        new Filter
                        {
                            Operator = FilterOperator.IsEqualTo,
                            Property = nameof(Setting.Type),
                            Value = SettingType.Patient
                        }
                    }).ConfigureAwait(false)).FirstOrDefault()
                }
            };
        }

        public async Task<LoggedInUser> RegisterAsync(User user)
        {
            var password = user.Password;

            var createdUser = await CreateAsync(user).ConfigureAwait(false);

            return await LogInAsync(createdUser.UserId, password).ConfigureAwait(false);
        }

        public async Task UpdateThemeAsync(string id, string theme)
        {
            var user = await GetOrThrowAsync(id).ConfigureAwait(false);

            user.Theme = theme;

            await UpdateAsync(id, user).ConfigureAwait(false);
        }

        protected override async Task<User> OnCreating(User user)
        {
            await new UserValidator(this).ValidateAndThrowAsync(user);

            return await Task.FromResult(user).ConfigureAwait(false);
        }
    }
}
