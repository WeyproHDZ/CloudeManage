using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class IGMembersService : IIGMembersService
    {
        private IRepository<IGMembers> repository;
        public IGMembersService()
        {
            repository = new GenericRepository<IGMembers>();
        }

        public IGMembersService(CloudControlEntities Context)
        {
            repository = new GenericRepository<IGMembers>(Context);
        }

        public IResult Create(IGMembers entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);
            try
            {
                this.repository.Insert(entity);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
        public IResult Update(IGMembers entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);
            try
            {
                this.repository.Update(entity);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
        public IResult SpecificUpdate(IGMembers entity, string[] Includeproperties)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);
            try
            {
                this.repository.SpecificUpdate(entity, Includeproperties);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
        public IResult Delete(object id)
        {
            IResult result = new Result(false);

            var entity = this.GetByID(id);

            if (entity == null)
            {
                result.Message = "找不到資料";
            }

            try
            {
                this.repository.Delete(entity);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
        public IResult Delete(IGMembers entity)
        {
            IResult result = new Result(false);

            if (entity == null)
            {
                result.Message = "找不到資料";
            }

            try
            {
                this.repository.Delete(entity);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
        public IGMembers GetByID(object id)
        {
            return this.repository.GetByID(id);
        }
        public IEnumerable<IGMembers> Get()
        {
            return this.repository.Get().Where(a => a.Isenable == 1);
        }

        public void SaveChanges()
        {
            this.repository.SaveChanges();
        }
    }
}