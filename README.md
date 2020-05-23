Isici - successor of Switcheroo
==========

A lightweight framework for [feature toggling](http://martinfowler.com/bliki/FeatureToggle.html) to enable trunk based development.
Isici aims for simplicity, with a clean syntax and a minimal feature set while not compromising on extensibility and testability.

Isici is the successor of the [Switcheroo](https://github.com/rhanekom/Switcheroo) package developed by Riaan Hanekom. Isici has been split into separate packages for modular development and also now supports .net 4.5.x and up via netstandard 2.0.

Getting Isici
------------------

Isici can be installed via [Nuget](http://nuget.org/packages/Isici.Core).

```powershell
> Install-Package Isici.Core 
```

License
--------

Isici is licensed under the [MIT license](http://opensource.org/licenses/MIT).


Quick Start
------------

**Installation**

Nuget packages:
- [Isici.Core](https://www.nuget.org/packages/Isici.Core): core package needed to implement the feature flags ![Isici.core](https://github.com/suddenelfilio/Isici/workflows/Isici%20core/badge.svg) 
- [Isici.Core.Abstractions](https://www.nuget.org/packages/Isici.Core.Abstractions): abstractions package with implementation in Isici.Core ![Isici.Core.Abstractions Build](https://github.com/suddenelfilio/Isici/workflows/Isici.Core.Abstractions%20Build/badge.svg)

**Add configuration**

Currently there are 2 IconfigurationProviders supported:
- [Isici.Configuration.SystemConfiguration](https://www.nuget.org/packages/Isici.Configuration.SystemConfiguration): uses the System.Configuration.ConfigurationManager to load configuration from *.config files with support for custom sections. This is the original implementtion as was in Switcheroo. ![Isici SystemConfig](https://github.com/suddenelfilio/Isici/workflows/Isici%20SystemConfig/badge.svg)

```xml
<configuration>
  <configSections>
    <section name="features" type="Isici.Configuration.FeatureToggleConfiguration, Isici.Configuration.SystemConfiguration"/>
  </configSections>
  <features>
    <toggles>
      <add name="Log.InColor" enabled="true"/>
    </toggles>
  </features>
</configuration>
```
- [Isici.Configuration.JsonFileConfiguration](https://www.nuget.org/packages/Isici.Configuration.JsonFileConfiguration): uses a json file to load configuration from. This is the original implementation by myself which was a contribution for switcherooo. ![Isici JsonConfig](https://github.com/suddenelfilio/Isici/workflows/Isici%20JsonConfig/badge.svg)

```json
[
  {
    "name": "testDateRange",
    "enabled": true,
    "from": "1 November 2012",
    "until": "2 November 2012"
  }
]
```

**Custom configuration sources**
You can implement your own configuration source by implementing the IConfigurationReader in the Isici.Core.Abstractions package. Feel free to contribute other sources.

Toggle types
--------------

**Boolean (true/false)**

Feature toggles based on a static binary value - either on or off.

```c#
features.Add(new BooleanToggle("Feature1", true));
```
***System.configuration***
```xml
<features>
    <toggles>
      <add name="BooleanToggle.Enabled" enabled="true"/>
      <add name="BooleanToggle.Disabled" enabled="false"/>
    </toggles>
 </features>
```
***Json file***
```json
[
  {
    "name": "Feature1",
    "enabled": true
  }
]
```

**Date Range (true/false, within date range)**

Date Range feature toggles are evaluated on both the binary enabled value and the current date.

```c#
features.Add(new DateRangeToggle("Feature2", true, DateTime.Now.AddDays(5), null));
```
***System.configuration***
```xml
<features>
    <toggles>
      <add name="Date.Enabled.InRange" enabled="true" from="1 January 2010" until="31 December 2050"/>
      <add name="Date.Enabled.Expired" enabled="true" until="31 December 2010"/>
      <add name="Date.Enabled.Future"  enabled="true" from="1 January 2050"/>
      <add name="Date.Disabled" enabled="false"/>
    </toggles>
 </features>
```
***Json file***
```json
[
  {
    "name": "Feature2",
    "enabled": true,
    "from": "1 November 2012",
    "until": "2 November 2012"
  }
]
```
_From_ and _until_ dates can be any valid date format parseable by _DateTime.Parse_.


**Established features**

Marking a feature toggle as established makes the feature toggle throw a _FeatureEstablishedException_ exception to make sure that it is not queried any more.  

```c#
features.Add(new EstablishedFeatureToggle("establishedFeature"));
```
***System.configuration***
```xml
<features>
    <toggles>
      <add name="EstablishedFeature" established="true"/>
    </toggles>
 </features>
```
***Json file***
```json
[
  {
    "name": "establishedFeature",
    "established": true
  }
]
```
**Dependencies**

Features can depend on other features.  For instance, it is sometimes convenient to have a "main" feature, and then sub-features that depend on it.  Dependencies can be specified in configuration as a comma delimited list.

```c#
var mainFeature = new BooleanToggle("mainFeature", true);
var subFeature1 = new BooleanToggle("subFeature1", true);
var subFeature2 = new BooleanToggle("subFeature2", true);

var dependency1 = new DependencyToggle(subFeature1, mainFeature);
var dependency2 = new DependencyToggle(subFeature2, mainFeature);
features.Add(dependency1);
features.Add(dependency2);
```
***System.configuration***
```xml
<features>
    <toggles>
        <add name="SubFeature1" enabled="true" dependencies="MainFeature"/>
        <add name="SubFeature2" enabled="true" dependencies="MainFeature"/>
        <add name="MainFeature" enabled="true" />
    </toggles>
 </features>
```
***Json file***
```json
[
  {
    "name": "mainFeature",
    "enabled": true,
    "dependencies": [
      "subFeature1",
      "subFeature2"
    ]
  },
  {
    "name": "subFeature1",
    "enabled": true
  },
  {
    "name": "subFeature2",
    "enabled": true
  }
]
```
Other features  
----------------

**Code-friendly initialization**

```c#
IFeatureConfiguration features = new FeatureConfiguration(new[]
    {
        new BooleanToggle("Feature1", true),
        new DateRangeToggle(
            "Feature2",
            true,
            DateTime.Now.AddDays(-2),
            DateTime.Now.AddDays(3))
    });
```

**Feature toggle diagnostics : _IFeatureConfiguration.WhatDoIHave_**

```c#
Console.WriteLine(features.WhatDoIHave());
```
```text
Name          Feature1
IsEnabled     True


Name          Feature2
IsEnabled     True
From          11/16/2012 3:32:23 PM
Until         11/21/2012 3:32:23 PM
```