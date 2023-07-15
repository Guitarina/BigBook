using BigBook.DataAccess.Repository.IRepository;
using BigBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBook.DataAccess.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailsRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public void Update(OrderDetails obj)
        {
            _db.OrderDetails.Update(obj);
        }

	}
}
