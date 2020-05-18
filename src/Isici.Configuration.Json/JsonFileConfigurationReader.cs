using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Isici.Core.Abstractions;
using Isici.Core.Abstractions.Configuration;
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
                    throw new ConfigurationErrorsException("Could not find dependency with name \"" + cleanName + "\".");
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