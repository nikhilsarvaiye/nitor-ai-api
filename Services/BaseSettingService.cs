namespace Services
{
    using Configuration.Options;
    using DotnetStandardQueryBuilder.Core;
    using FluentValidation;
    using Models;
    using Repositories;
    using Services.Validators;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class BaseSettingService<T> : BaseService<T>
        where T: Setting, new()
    {
        protected SettingType Type { get; set; }

        protected BaseSettingService(SettingType type, IAppOptions appOptions, ICacheService<T> cacheService, ISettingRepository<T> settingRepository)
            : base(appOptions, cacheService, settingRepository)
        {
            Type = type;
        }

        public override async Task<List<T>> GetAsync(IRequest request = null)
        {
            return await Repository.GetAsync(AddTypeFilter(request)).ConfigureAwait(false);
        }

        protected override async Task<T> OnCreating(T t)
        {
            new SettingValidator().ValidateAndThrow(t);

            return await Task.FromResult(t).ConfigureAwait(false);
        }

        private IRequest AddTypeFilter(IRequest request)
        {
            request ??= new Request();
            var existingFilter = request.Filter;
            request.Filter = new CompositeFilter
            {
                LogicalOperator = LogicalOperator.And,
                Filters = new List<IFilter>
                {
                    existingFilter,
                    new Filter
                    {
                        Operator = FilterOperator.IsEqualTo,
                        Property = nameof(Setting.Type),
                        Value = Type
                    }
                }
            };
            return request;
        }
    }
}
