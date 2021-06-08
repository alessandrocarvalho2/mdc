using Microsoft.EntityFrameworkCore;
using System;
using Volvo.Ecash.Application.Utils;
using Volvo.Ecash.Domain.Entities;
using Volvo.Ecash.Domain.Filters;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository;
using static Volvo.Ecash.Dto.Enum.EnumCommon;

namespace Volvo.Ecash.Business
{
    public class UserBusiness : BaseBusiness<UnitOfWork>
    {
        public UserBusiness() : base(new ECashContext())
        {
        }
        public UserBusiness(DbContext context) : base(context)
        {
        }
        public ResultModel<UserVR> Get(long id)
        {
            return this.GetList(new UserFilter() { Id = id });
        }
        public ResultModel<UserVR> GetList(UserFilter filter)
        {
            try
            {
                if (filter.ResultPagesModel != null &&
                   (filter.ResultPagesModel.Offset == 0 &&
                   filter.ResultPagesModel.Actual == 0))
                {
                    filter.ResultPagesModel = new PaginationFilter().ResultPagesModel;
                }

                var resultModel = UnitOfWork.UserRepository.GetList(filter);
                resultModel.Messages = null;
                return resultModel;

            }
            catch (Exception ex)
            {

                var resultModel = new ResultModel<UserVR>(false);
                resultModel.Pages = null;
                resultModel.AddMessage(ex.Message, SystemMessageTypeEnum.Error);
                return resultModel;
            }

        }
        public ResultModel<UserVR> Insert(UserVR user)
        {
            try
            {
                var resultModel = new ResultModel<UserVR>(true);

                var data = UnitOfWork.UserRepository.Add(user);
                if (data is null)
                {
                    resultModel.IsOk = false;
                    resultModel.AddMessage(string.Format(ErrorMessage.MSG007, "Usuário"), SystemMessageTypeEnum.Info);
                }
                else
                {
                    resultModel.Items.Add(data);
                    resultModel.AddMessage(string.Format(SuccessMessage.MSG001), SystemMessageTypeEnum.Success);
                    this.Commit();
                }
                resultModel.Pages = null;

                return resultModel;
            }
            catch (Exception ex)
            {
                var resultModel = new ResultModel<UserVR>(false);
                resultModel.Pages = null;
                resultModel.AddMessage(ex.Message, SystemMessageTypeEnum.Error);
                return resultModel;
            }
        }
        public ResultModel<UserVR> Update(UserVR user)
        {
            try
            {
                var resultModel = new ResultModel<UserVR>(true);

                var data = UnitOfWork.UserRepository.Update(user);
                if (data is null)
                {
                    resultModel.IsOk = false;
                    resultModel.AddMessage(string.Format(ErrorMessage.MSG007, "Usuário"), SystemMessageTypeEnum.Info);
                }
                else
                {
                    resultModel.Items.Add(data);
                    resultModel.AddMessage(string.Format(SuccessMessage.MSG001), SystemMessageTypeEnum.Success);
                    this.Commit();
                }
                resultModel.Pages = null;

                return resultModel;
            }
            catch (Exception ex)
            {
                var resultModel = new ResultModel<UserVR>(false);
                resultModel.Pages = null;
                resultModel.AddMessage(ex.Message, SystemMessageTypeEnum.Error);
                return resultModel;
            }
        }
        public ResultModel<UserVR> Delete(int id)
        {
            try
            {

                var resultModel = new ResultModel<UserVR>(true);

                var user = UnitOfWork.UserRepository.Search(id);

                var data = UnitOfWork.UserRepository.Delete(user);
                if (data is null)
                {
                    resultModel.IsOk = false;
                    resultModel.AddMessage(string.Format(ErrorMessage.MSG019, "Usuário"), SystemMessageTypeEnum.Info);
                }
                else
                {
                    resultModel.Items.Add(user);
                    resultModel.AddMessage(string.Format(SuccessMessage.MSG004), SystemMessageTypeEnum.Success);
                    this.Commit();
                }
                resultModel.Pages = null;

                return resultModel;
            }
            catch (Exception ex)
            {
                var resultModel = new ResultModel<UserVR>(false);
                resultModel.Pages = null;
                resultModel.AddMessage(ex.Message, SystemMessageTypeEnum.Error);
                return resultModel;
            }
        }
    }
}
