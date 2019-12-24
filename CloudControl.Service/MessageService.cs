using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class MessageService : BaseService<Message>
    {
        public MessageService()
        {
            repository = new GenericRepository<Message>();
        }

        public MessageService(CloudControlEntities Context)
        {
            repository = new GenericRepository<Message>(Context);
        }
    }
}