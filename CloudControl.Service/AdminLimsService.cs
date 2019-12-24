using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class AdminLimsService : BaseService<AdminLims>
    {
        public AdminLimsService()
        {
            repository = new GenericRepository<AdminLims>();
        }

        public AdminLimsService(CloudControlEntities context)
        {
            repository = new GenericRepository<AdminLims>(context);
        }
    }
}