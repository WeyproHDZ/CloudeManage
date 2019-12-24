using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public interface IYTMembersService
    {
        IResult Create(YTMembers entity);
        IResult Update(YTMembers entity);
        IResult Delete(object id);
        IResult Delete(YTMembers entity);
        YTMembers GetByID(object id);
        IEnumerable<YTMembers> Get();
        void SaveChanges();
    }
}