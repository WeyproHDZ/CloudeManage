using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class FBVMLogService : BaseService<FBVMLog>
    {
        public FBVMLogService()
        {
            repository = new GenericRepository<FBVMLog>();
        }

        public FBVMLogService(CloudControlEntities context)
        {
            repository = new GenericRepository<FBVMLog>(context);
        }
    }
}