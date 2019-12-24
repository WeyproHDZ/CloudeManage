using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class CategoryProductService : BaseService<CategoryProduct>
    {
        public CategoryProductService()
        {
            repository = new GenericRepository<CategoryProduct>();
        }

        public CategoryProductService(CloudControlEntities Context)
        {
            repository = new GenericRepository<CategoryProduct>(Context);
        }
    }
}