using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace puka;

public class UserConfig
{
	private static Configuration? config;
	private static string configFilePath = "";

	public static void Load()
	{
		string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

		string appFolder = Path.Combine(appDataPath, "puka");
		Directory.CreateDirectory(appFolder);

		configFilePath = Path.Combine(appFolder, "puka.ini");
		if (!File.Exists(configFilePath))
			File.WriteAllText(configFilePath, "<configuration></configuration>");

		ExeConfigurationFileMap configFileMap = new()
		{
			ExeConfigFilename = configFilePath
		};
		config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
	}

	public static void Add(string key, string value)
	{
		try
		{
			if (config == null)
			{
				Program.Logger.Warn("No se ha cargado el archivo de configuraciones, UserConfig.Load()");
				return;
			}
			config.AppSettings.Settings.Add(key, value);
			config.Save(ConfigurationSaveMode.Modified);
		}
		catch (System.Exception)
		{
			throw;
		}
	}

	public static string? Get(string key)
	{
		if (config == null)
			return "";
		return config.AppSettings.Settings[key]?.Value;
	}


}
