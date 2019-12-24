using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class IGMembersLoginlogService : BaseService<IGMembersLoginlog>
    {
        public IGMembersLoginlogService()
        {
            repository = new GenericRepository<IGMembersLoginlog>();
        }
        public IGMembersLoginlogService(CloudControlEntities context)
        {
            repository = new GenericRepository<IGMembersLoginlog>(context);
        }
    }
}