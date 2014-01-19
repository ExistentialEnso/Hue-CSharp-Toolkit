using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ExistentialEnso.HueCSharpToolkit.Models
{
    /// <summary>
    /// Model data class. Represents a single light in the Hue setup.
    /// </summary>
    public class Light
    {
        [JsonProperty(PropertyName = "id")]
        public int InternalId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "modelid")]
        public string ModelId { get; set; }

        [JsonProperty(PropertyName = "swversion")]
        public string SoftwareVersion { get; set; }

        public Bridge Bridge { get; set; }

        public LightState State { get; set; }
    }
}
