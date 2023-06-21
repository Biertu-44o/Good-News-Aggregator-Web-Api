using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Article.ArticleRate.jsonModels
{
    public class Match
    {
        public int id { get; set; }
        public string segment { get; set; }
        public string translation { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public string quality { get; set; }
        public string reference { get; set; }

        [JsonProperty("usage-count")]
        public int usagecount { get; set; }
        public string subject { get; set; }

        [JsonProperty("created-by")]
        public string createdby { get; set; }

        [JsonProperty("last-updated-by")]
        public string lastupdatedby { get; set; }

        [JsonProperty("create-date")]
        public string createdate { get; set; }

        [JsonProperty("last-update-date")]
        public string lastupdatedate { get; set; }
        public double match { get; set; }
        public string model { get; set; }
    }
}
