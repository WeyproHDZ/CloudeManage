using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class IGVMStatusService : BaseService<IGVMStatus>
    {
        public IGVMStatusService()
        {
            repository = new GenericRepository<IGVMStatus>();
        }

        public IGVMStatusService(CloudControlEntities context)
        {
            repository = new GenericRepository<IGVMStatus>(context);
        }
    }
}