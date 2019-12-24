using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class FBVMStatusService : BaseService<FBVMStatus>
    {
        public FBVMStatusService()
        {
            repository = new GenericRepository<FBVMStatus>();
        }

        public FBVMStatusService(CloudControlEntities context)
        {
            repository = new GenericRepository<FBVMStatus>(context);
        }
    }
}