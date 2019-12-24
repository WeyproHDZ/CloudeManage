using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;
namespace CloudControl.Service
{
    public class IGVMLogService : BaseService<IGVMLog>
    {
        public IGVMLogService()
        {
            repository = new GenericRepository<IGVMLog>();
        }

        public IGVMLogService(CloudControlEntities context)
        {
            repository = new GenericRepository<IGVMLog>(context);
        }
    }
}