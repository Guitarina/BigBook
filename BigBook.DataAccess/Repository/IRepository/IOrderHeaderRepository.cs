using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigBook.DataAccess.Repository;
using BigBook.Models;

namespace BigBook.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        void UpdateStatuc(int id, string orderStatus, string? paymentStatus = null);
        //todo: implement orderheader and orderdetails repositories
    }
}
