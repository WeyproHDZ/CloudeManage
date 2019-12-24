using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class IGOrderService : BaseService<IGOrder>
    {
        public IGOrderService()
        {
            repository = new GenericRepository<IGOrder>();
        }

        public IGOrderService(CloudControlEntities Context)
        {
            repository = new GenericRepository<IGOrder>(Context);
        }
    }
}