using BigBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBook.DataAccess.Repository.IRepository
{
    public interface IProductRepository :IRepository<Product>
    {
        void Update(Product obj);
    }
}
