using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class AdminsService : BaseService<Admins>
    {
        public AdminsService()
        {
            repository = new GenericRepository<Admins>();
        }

        public AdminsService(CloudControlEntities context)
        {
            repository = new GenericRepository<Admins>(context);
        }
    }
}