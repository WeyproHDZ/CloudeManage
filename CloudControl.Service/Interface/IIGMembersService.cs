using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public interface IIGMembersService
    {
        IResult Create(IGMembers entity);
        IResult Update(IGMembers entity);
        IResult Delete(object id);
        IResult Delete(IGMembers entity);
        IGMembers GetByID(object id);
        IEnumerable<IGMembers> Get();
        void SaveChanges();
    }
}