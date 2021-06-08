using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static Volvo.Ecash.Dto.Enum.EnumCommon;

namespace Volvo.Ecash.Domain.Entities
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Include)]
    public class ResultModel<T>
    {
        public ResultModel()
        {

        }

        public ResultModel(bool _isOk = false)
        {
            IsOk = _isOk;
        }

        public ResultModel(bool isError, string message)
        {
            IsOk = isError;
            if (isError)
            {
                AddMessage(message, SystemMessageTypeEnum.Error);
            }
            else
            {
                AddMessage(message, SystemMessageTypeEnum.Info);
            }
        }

        /// <summary>
        /// Result state
        /// </summary>
        [JsonProperty(PropertyName = "isOk", NullValueHandling = NullValueHandling.Include)]
        public bool IsOk { get; set; } = true;

        /// <summary>
        /// Pagination metadata
        /// </summary>
        [JsonProperty(PropertyName = "pages", NullValueHandling = NullValueHandling.Ignore)]
        public ResultPagesModel Pages { get; set; } = new ResultPagesModel();

        /// <summary>
        /// Messages result for requests
        /// </summary>
        [JsonProperty(PropertyName = "messages", NullValueHandling = NullValueHandling.Ignore)]
        public List<SystemMessageModel> Messages { get; set; } = new List<SystemMessageModel>();

        [JsonProperty(PropertyName = "items", NullValueHandling = NullValueHandling.Include)]
        public List<T> Items { get; set; } = new List<T>();

        public void AddMessage(string _text, SystemMessageTypeEnum _type = SystemMessageTypeEnum.Info, string _id = null)
        {
            Messages = Messages ?? new List<SystemMessageModel>();
            Messages.Add(new SystemMessageModel
            {
                Message = _text,
                Type = _type,
                MessageId = _id
            });

            if (_type == SystemMessageTypeEnum.Error) IsOk = false;
        }

    }
}
