using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public interface IFBMembersService
    {
        IResult Create(FBMembers entity);
        IResult Update(FBMembers entity);
        IResult Delete(object id);
        IResult Delete(FBMembers entity);
        FBMembers GetByID(object id);
        IQueryable<FBMembers> Get();
        void SaveChanges();
    }
}