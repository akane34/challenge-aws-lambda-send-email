using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.cloud.email.Models
{
    public class EventMessage
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("familyName", NullValueHandling = NullValueHandling.Ignore)]
        public string FamilyName { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }        

        public JObject ToJson()
        {
            return JObject.FromObject(this);
        }
    }
}
