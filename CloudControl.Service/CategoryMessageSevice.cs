using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class CategoryMessageSevice : BaseService<CategoryMessage>
    {
        public CategoryMessageSevice()
        {
            repository = new GenericRepository<CategoryMessage>();
        }

        public CategoryMessageSevice(CloudControlEntities Context)
        {
            repository = new GenericRepository<CategoryMessage>(Context);
        }
    }
}