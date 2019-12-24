using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class ProductService : BaseService<Product>
    {
        public ProductService()
        {
            repository = new GenericRepository<Product>();
        }

        public ProductService(CloudControlEntities Context)
        {
            repository = new GenericRepository<Product>(Context);
        }
    }
}