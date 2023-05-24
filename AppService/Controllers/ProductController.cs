namespace AppService.Controllers
{
    using CacheManager.Core;
    using DotnetStandardQueryBuilder.Core;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductController : BaseController<Product>
    {
        private readonly IProductService _productService;

        private readonly IProductVariantService _productVariantService;

        private readonly ICacheManager<string> _cache;

        public ProductController(IProductService productService, IProductVariantService productVariantService)
            : base(productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));

            _productVariantService = productVariantService ?? throw new ArgumentNullException(nameof(productVariantService));
        }

        [HttpGet]
        public override async Task<dynamic> GetAsync(string? id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var product = new List<Product>() { await base.GetAsync(id).ConfigureAwait(false) }.FirstOrDefault();

                if (product == null)
                {
                    return null;
                }

                return (await _productVariantService.UpdateProducts(new List<Product> { product }).ConfigureAwait(false)).FirstOrDefault();
            }

            var request = GetRequestFromQueryString();

            if (request.Count)
            {
                var productsResponse = await Service.PaginateAsync(request).ConfigureAwait(false);

                if (SelectVariants(request))
                {
                    productsResponse.Items = await _productVariantService.UpdateProducts(productsResponse.Items).ConfigureAwait(false);
                }

                return productsResponse;
            }

            var products = await Service.GetAsync(request).ConfigureAwait(false);

            if (SelectVariants(request))
            {
                products = await _productVariantService.UpdateProducts(products).ConfigureAwait(false);
            }

            return products;
        }

        [HttpPost]
        public override async Task<Product> CreateAsync(Product product)
        {
            var createdProduct = await base.CreateAsync(product).ConfigureAwait(false);

            var variants = _productService.ParseVariants(createdProduct.Id, createdProduct);

            await _productVariantService.BulkCreateAsync(variants).ConfigureAwait(false);

            return createdProduct;
        }

        [HttpPut]
        public override async Task<string> UpdateAsync(string id, Product product)
        {
            if (string.IsNullOrEmpty(Convert.ToString(id)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var updatedId = await base.UpdateAsync(id, product).ConfigureAwait(false);

            var variants = _productService.ParseVariants(id, product);

            await _productVariantService.BulkRemoveAsync(new Filter
            {
                Operator = FilterOperator.IsEqualTo,
                Property = nameof(ProductVariant.ProductId),
                Value = id
            }).ConfigureAwait(false);

            await _productVariantService.BulkCreateAsync(variants).ConfigureAwait(false);

            return updatedId;
        }

        [HttpDelete]
        public override async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(id)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            await base.DeleteAsync(id).ConfigureAwait(false);

            await _productVariantService.BulkRemoveAsync(new Filter
            {
                Operator = FilterOperator.IsEqualTo,
                Property = nameof(ProductVariant.ProductId),
                Value = id
            }).ConfigureAwait(false);
        }

        private bool SelectVariants(IRequest request)
        {
            return request.Select?.Contains(_productService.VariantsPropertyName.ToLowerInvariant()) == true;
        }
    }
}
