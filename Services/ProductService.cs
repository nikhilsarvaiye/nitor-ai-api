namespace Services
{
    using FluentValidation;
    using Models;
    using DotnetStandardQueryBuilder.Core;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Validators;
    using Configuration.Options;
    using System.Collections;
    using Common;

    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IExcelService _excelService;

        public string VariantsPropertyName { get; }

        public ProductService(IAppOptions appOptions, ICacheService<Product> cacheService, IProductRepository productRepository, IExcelService excelService)
            : base(appOptions, cacheService, productRepository)
        {
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            VariantsPropertyName = "Variants";
        }

        public List<ProductVariant> ParseVariants(string id, Product product)
        {
            var defaultProductVariants = new List<ProductVariant>()
            {
                new ProductVariant
                {
                    ProductId = id,
                    Name = product.Name,
                }
            };


            if (product.Metadata.Count == 0 || !product.Metadata.ContainsKey(VariantsPropertyName.ToLowerInvariant()))
            {
                return defaultProductVariants;
            }

            product.Metadata.TryGetValue(VariantsPropertyName.ToLowerInvariant(), out object variantsValue);

            var variants = variantsValue is IEnumerable ? (variantsValue as IEnumerable)?.ToList()?.ToDictionary() : null;

            if (variants == null || variants.Count == 0)
            {
                return defaultProductVariants;
            }

            return variants.ConvertAll(x => new ProductVariant
            {
                ProductId = id,
                Name = x.BuildName(new List<string> { product.Name }),
                Type = ProductVariantType.Variant,
                Values = x
            });
        }

        protected override async Task<Product> OnCreating(Product product)
        {
            new ProductValidator().ValidateAndThrow(product);

            return await Task.FromResult(product).ConfigureAwait(false);
        }
    }
}
