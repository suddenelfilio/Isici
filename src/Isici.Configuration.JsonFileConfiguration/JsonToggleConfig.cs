/*
The MIT License

Copyright (c) 2020 Filip Stas

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
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
        public string[] Dependencies { get; set; }
    }
}