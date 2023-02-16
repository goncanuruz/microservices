using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Komut.Captech.ProductService.Domain.Entities;
using System.Threading.Tasks;

namespace Komut.Captech.ProductService.Application.Services.Repositories
{
    public interface IProductRepository : IAsyncRepository<Product>, IRepository<Product>
    {
    }
}
