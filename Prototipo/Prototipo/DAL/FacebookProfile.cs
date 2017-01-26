using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Prototipo.Facebook
{
    public class FacebookProfile
    {
        public string Name { get; set; }
        [JsonProperty("picture")]
        public Picture Picture { get; set; }
        public string Locale { get; set; }
        public string Link { get; set; }
        public Cover Cover { get; set; }
        [JsonProperty("age_range")]
        public AgeRange AgeRange { get; set; }
        public Device[] Devices { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        public string Gender { get; set; }
        public bool IsVerified { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
    }

    public class Picture
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("is_silhouette")]
        public bool IsSilhouette { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Cover
    {
        public string Id { get; set; }
        public int OffsetY { get; set; }
        public string Source { get; set; }
    }

    public class AgeRange
    {
        public int Min { get; set; }
    }

    public class Device
    {
        public string Os { get; set; }
    }
}
