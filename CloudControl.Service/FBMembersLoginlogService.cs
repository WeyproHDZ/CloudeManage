using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class FBMembersLoginlogService : BaseService<FBMembersLoginlog>
    {
        public FBMembersLoginlogService()
        {
            repository = new GenericRepository<FBMembersLoginlog>();
        }
        public FBMembersLoginlogService(CloudControlEntities context)
        {
            repository = new GenericRepository<FBMembersLoginlog>(context);
        }
    }
}