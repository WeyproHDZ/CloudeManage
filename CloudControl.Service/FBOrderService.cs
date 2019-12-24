using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class FBOrderService : BaseService<FBOrder>
    {
        public FBOrderService()
        {
            repository = new GenericRepository<FBOrder>();
        }

        public FBOrderService(CloudControlEntities Context)
        {
            repository = new GenericRepository<FBOrder>(Context);
        }
    }
}