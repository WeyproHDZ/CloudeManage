using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class LimsService : BaseService<Lims>
    {
        public LimsService()
        {
            repository = new GenericRepository<Lims>();
        }

        public LimsService(CloudControlEntities context)
        {
            repository = new GenericRepository<Lims>(context);
        }
    }
}