using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class IGOrderlistService : BaseService<IGOrderlist>
    {
        public IGOrderlistService()
        {
            repository = new GenericRepository<IGOrderlist>();
        }

        public IGOrderlistService(CloudControlEntities Context)
        {
            repository = new GenericRepository<IGOrderlist>(Context);
        }
    }
}