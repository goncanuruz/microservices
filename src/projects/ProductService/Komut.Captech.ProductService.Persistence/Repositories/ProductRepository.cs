using Core.Persistence.Repositories;
using Komut.Captech.ProductService.Application.Services.Repositories;
using Komut.Captech.ProductService.Domain.Entities;
using Komut.Captech.ProductService.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komut.Captech.ProductService.Persistence.Repositories
{
    public class ProductRepository : EfRepositoryBase<Product, BaseDbContext>, IProductRepository
    {
        public ProductRepository(BaseDbContext context) : base(context)
        {
        }
    }
}
