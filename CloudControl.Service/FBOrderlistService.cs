using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class FBOrderlistService : BaseService<FBOrderlist>
    {
        public FBOrderlistService()
        {
            repository = new GenericRepository<FBOrderlist>();
        }

        public FBOrderlistService(CloudControlEntities Context)
        {
            repository = new GenericRepository<FBOrderlist>(Context);
        }
    }
}