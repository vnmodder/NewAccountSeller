using AccountSeller.Application.Common.Helpers;
using Newtonsoft.Json;

namespace AccountSeller.Application.Common.Models
{
    public class KeyValueModel
    {
        [JsonProperty("value")]
        public object? Key { get; set; }

        [JsonProperty("labelMenu")]
        public object? Value { get; set; }

        public KeyValueModel(object? key, object? value)
        {
            Key = key is String ? key.ToString().TrimEndNullAble() : key;
            Value = value is String ? value.ToString().TrimEndNullAble() : value;
        }

    }
}
