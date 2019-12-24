using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class YTOrderService : BaseService<YTOrder>
    {
        public YTOrderService()
        {
            repository = new GenericRepository<YTOrder>();
        }

        public YTOrderService(CloudControlEntities Context)
        {
            repository = new GenericRepository<YTOrder>(Context);
        }
    }
}