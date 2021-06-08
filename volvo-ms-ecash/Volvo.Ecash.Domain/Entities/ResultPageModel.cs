using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Domain.Entities
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ResultPagesModel
    {

        /// <summary>
        /// Quantidade items total
        /// </summary>
        [JsonProperty(PropertyName = "totalItems", NullValueHandling = NullValueHandling.Include)]
        public long TotalItems { get; set; } = 0;

        /// <summary>
        /// Quantidade de total de paginas
        /// </summary>
        [JsonProperty(PropertyName = "total", NullValueHandling = NullValueHandling.Include)]
        public long Total { get; set; } = 0;

        /// <summary>
        /// Pagina atual
        /// </summary>
        [JsonProperty(PropertyName = "actual", NullValueHandling = NullValueHandling.Include)]
        public long Actual { get; set; } = 0;

        /// <summary>
        /// Quantidade items por pagina
        /// </summary>
        [JsonProperty(PropertyName = "offset", NullValueHandling = NullValueHandling.Include)]
        public long Offset { get; set; } = 0;
    }
}
