using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BreastCancerAPI.Data
{
    public abstract class BaseEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
