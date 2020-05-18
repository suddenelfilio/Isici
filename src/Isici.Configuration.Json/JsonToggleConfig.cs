using System;
using Newtonsoft.Json;

namespace Isici.Configuration.JsonFileConfiguration
{
    public class JsonToggleConfig
    {
        /// <summary>
        /// Gets or sets the name of the feature toggle.
        /// </summary>
        /// <value>
        /// The name of the feature toggle.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this toggle is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this feature is established.
        /// </summary>
        /// <value>
        /// <c>true</c> if this feature is established; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("established")]
        public bool IsEstablished { get; set; }

        /// <summary>
        /// Gets or sets the date that this feature should be turned on.
        /// </summary>
        /// <value>
        /// The date that this toggle should be enabled from.
        /// </value>
        [JsonProperty("from")]
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Gets or sets the date that this feature should be turned off.
        /// </summary>
        /// <value>
        /// The date that this feature should be turned off..
        /// </value>
        [JsonProperty("until")]
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Gets or sets the dependencies.
        /// </summary>
        /// <value>
        /// The dependencies.
        /// </value>
        [JsonProperty("dependencies")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "<Pending>")]
        public string[] Dependencies { get; set; }
    }
}