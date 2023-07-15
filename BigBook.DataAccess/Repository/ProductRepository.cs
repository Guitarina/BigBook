﻿using BigBook.DataAccess.Repository.IRepository;
using BigBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product> ,IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(p => p.Id == obj.Id);
            if(objFromDb != null)
            {
                objFromDb.Title= obj.Title;
                objFromDb.ISBN= obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.Price100= obj.Price100;
                objFromDb.Description= obj.Description;
                objFromDb.CategoryId= obj.CategoryId;
                objFromDb.CoverTypeId= obj.CoverTypeId;
                objFromDb.Author = obj.Author;
                objFromDb.ListPrice = obj.ListPrice;
                if(obj.ImageUrl!=null)
                {
                    objFromDb.ImageUrl= obj.ImageUrl;
                }
            }
            _db.Products.Update(objFromDb);
        }
    }
}
