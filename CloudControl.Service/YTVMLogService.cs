using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class YTVMLogService : BaseService<YTVMLog>
    {
        public YTVMLogService()
        {
            repository = new GenericRepository<YTVMLog>();
        }

        public YTVMLogService(CloudControlEntities context)
        {
            repository = new GenericRepository<YTVMLog>(context);
        }
    }
}