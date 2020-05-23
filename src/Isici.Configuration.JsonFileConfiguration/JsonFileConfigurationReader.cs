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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Isici.Core.Abstractions;
using Isici.Core.Abstractions.Configuration;
using Isici.Core.Exceptions;
using Isici.Core.Toggles;
using Newtonsoft.Json;

namespace Isici.Configuration.JsonFileConfiguration
{
    public class JsonFileConfigurationReader : IConfigurationReader
    {

        private readonly string jsonFile;

        public JsonFileConfigurationReader(string jsonFile)
        {
            this.jsonFile = jsonFile;
        }

        public IEnumerable<IFeatureToggle> GetFeatures()
        {
            if (File.Exists(jsonFile))
            {
                string fileContents = File.ReadAllText(jsonFile);
                if (!string.IsNullOrEmpty(fileContents))
                {
                    JsonToggleConfig[] jsonToggles = JsonConvert.DeserializeObject<JsonToggleConfig[]>(fileContents);

                    Dictionary<string, KeyValuePair<JsonToggleConfig, IFeatureToggle>> toggles =
                        jsonToggles
                            .ToDictionary(x => x.Name, x => new KeyValuePair<JsonToggleConfig, IFeatureToggle>(x, ConvertToFeatureToggle(x)));
                    return BuildDependencies(toggles).ToList();
                }
            }

            return new List<IFeatureToggle>(0);
        }

        private IEnumerable<IFeatureToggle> BuildDependencies(Dictionary<string, KeyValuePair<JsonToggleConfig, IFeatureToggle>> toggles)
        {
            foreach (var t in toggles)
            {
                JsonToggleConfig config = t.Value.Key;
                IFeatureToggle toggle = t.Value.Value;

                if (toggle is DependencyToggle dependencyToggle)
                {
                    BuildDependencies(dependencyToggle, toggles, config.Dependencies);
                }

                yield return toggle;
            }
        }

        private void BuildDependencies(DependencyToggle dependencyToggle, Dictionary<string, KeyValuePair<JsonToggleConfig, IFeatureToggle>> toggles, IEnumerable<string> dependencyNames)
        {
            foreach (var dependencyName in dependencyNames)
            {
                string cleanName = dependencyName.Trim();

                if (!toggles.TryGetValue(cleanName, out var dependency))
                {
                    throw new InvalidConfigurationException($"Could not find dependency with name \"{cleanName}\".");
                }

                dependencyToggle.AddDependency(dependency.Value);
            }
        }

        private IFeatureToggle ConvertToFeatureToggle(JsonToggleConfig config)
        {
            IFeatureToggle toggle;

            if (config.IsEstablished)
            {
                toggle = new EstablishedFeatureToggle(config.Name);
            }
            else if ((config.FromDate != null) || (config.ToDate != null))
            {
                toggle = new DateRangeToggle(config.Name, config.Enabled, config.FromDate, config.ToDate);
            }
            else
            {
                toggle = new BooleanToggle(config.Name, config.Enabled);
            }

            return config.Dependencies == null || !config.Dependencies.Any()
                ? toggle
                : new DependencyToggle(toggle);
        }
    }
}