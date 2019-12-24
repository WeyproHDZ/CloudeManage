using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudControl.Model;

namespace CloudControl.Service
{
    public class FBMembersService : IFBMembersService
    {
        private IRepository<FBMembers> repository;

        public FBMembersService()
        {
            repository = new GenericRepository<FBMembers>();
        }

        public FBMembersService(CloudControlEntities context)
        {
            repository = new GenericRepository<FBMembers>(context);
        }

        public IResult Create(FBMembers entity)
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
        public IResult Update(FBMembers entity)
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
        public IResult SpecificUpdate(FBMembers entity, string[] Includeproperties)
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
        public IResult Delete(FBMembers entity)
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
        public FBMembers GetByID(object id)
        {
            return this.repository.GetByID(id);
        }
        public IQueryable<FBMembers> Get()
        {
            return this.repository.Get().Where(a => a.Isenable == 1);
        }

        /*** 取得預備及正式 ****/
        public IQueryable<FBMembers> GetNoDel()
        {
            return this.repository.Get().Where(a => a.Isenable != 0);
        }

        public void SaveChanges()
        {
            this.repository.SaveChanges();
        }
    }
}