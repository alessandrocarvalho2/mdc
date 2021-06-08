using Newtonsoft.Json;
using System;
using static Volvo.Ecash.Dto.Enum.EnumCommon;

namespace Volvo.Ecash.Domain.Entities
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SystemMessageModel
    {
        private bool _showBadge;

        /// <summary>
        /// Related neurum record on collection
        /// </summary>
        [JsonProperty(PropertyName = "messageId", NullValueHandling = NullValueHandling.Ignore)]
        public string MessageId { get; set; }

        /// <summary>
        /// Related object ID
        /// </summary>
        [JsonProperty(PropertyName = "objectId", NullValueHandling = NullValueHandling.Ignore)]
        public string ObjectId { get; set; }

        /// <summary>
        /// Info, warning or error
        /// </summary>
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Include)]
        public SystemMessageTypeEnum Type { get; set; } = SystemMessageTypeEnum.Info;

        /// <summary>
        /// Message text
        /// </summary>
        [JsonProperty(PropertyName = "message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "messageUs", NullValueHandling = NullValueHandling.Ignore)]
        public string MessageUs { get; set; }

        /// <summary>
        /// Last update
        /// </summary>
        [JsonProperty(PropertyName = "lastUpdate", NullValueHandling = NullValueHandling.Include)]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Force to UI show a message for this (always for errors)
        /// </summary>
        [JsonProperty(PropertyName = "showBadge", NullValueHandling = NullValueHandling.Include)]
        public bool ShowBadge
        {
            get
            {
                if (Type == SystemMessageTypeEnum.Error)
                {
                    return true;
                }
                return _showBadge;
            }
            set
            {
                _showBadge = value;
            }
        }
    }
}
