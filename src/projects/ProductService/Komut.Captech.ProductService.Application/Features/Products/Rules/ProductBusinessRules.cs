using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Komut.Captech.ProductService.Application.Services.Repositories;
using Komut.Captech.ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komut.Captech.ProductService.Application.Features.Products.Rules
{
    public class ProductBusinessRules
    {
        private readonly IProductRepository _productRepository;

        public ProductBusinessRules(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task ProductNameCanNotBeDuplicatedWhenInserted(string name)
        {
            IPaginate<Product> result = await _productRepository.GetListAsync(b => b.Name == name);
            if (result.Items.Any()) throw new BusinessException("Product name exists.");
        }
    }
}
