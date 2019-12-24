using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class YTOrderlistService : BaseService<YTOrderlist>
    {
        public YTOrderlistService()
        {
            repository = new GenericRepository<YTOrderlist>();
        }

        public YTOrderlistService(CloudControlEntities Context)
        {
            repository = new GenericRepository<YTOrderlist>(Context);
        }
    }
}