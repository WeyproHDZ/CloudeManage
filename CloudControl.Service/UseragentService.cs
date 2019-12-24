using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class UseragentService : BaseService<Useragent>
    {
        public UseragentService()
        {
            repository = new GenericRepository<Useragent>();
        }

        public UseragentService(CloudControlEntities Context)
        {
            repository = new GenericRepository<Useragent>(Context);
        }
    }
}