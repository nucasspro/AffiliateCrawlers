using System.Windows;

[assembly: ThemeInfo(
    //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page,
    // or application resource dictionaries)
    ResourceDictionaryLocation.None,

    //where the generic resource dictionary is located
    //(used if a resource is not found in the page,
    // app, or any theme specific resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly
)]
[assembly: log4net.Config.XmlConfigurator(
    ConfigFile = "log4net.config",
    Watch = true
)]