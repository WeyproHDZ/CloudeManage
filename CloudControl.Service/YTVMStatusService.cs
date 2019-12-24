using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class YTVMStatusService : BaseService<YTVMStatus>
    {
        public YTVMStatusService()
        {
            repository = new GenericRepository<YTVMStatus>();
        }

        public YTVMStatusService(CloudControlEntities context)
        {
            repository = new GenericRepository<YTVMStatus>(context);
        }
    }
}