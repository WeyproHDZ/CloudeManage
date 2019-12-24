using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class YTMembersLoginlogService : BaseService<YTMembersLoginlog>
    {
        public YTMembersLoginlogService()
        {
            repository = new GenericRepository<YTMembersLoginlog>();
        }
        public YTMembersLoginlogService(CloudControlEntities context)
        {
            repository = new GenericRepository<YTMembersLoginlog>(context);
        }
    }
}