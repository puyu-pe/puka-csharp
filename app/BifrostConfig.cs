using System.Text.RegularExpressions;
using puka.util;

namespace puka.app;


public static class BifrostConfig
{

	public static bool TrySetUrl(string url, out List<string> errors)
	{
		errors = new List<string>();
		url = url.Trim();
		if (url.Length == 0)
		{
			errors.Add("La url no debe ser un campo vacio");
		}
		bool isValidUrl = Regex.IsMatch(url, @"^(https?://).+[^/]$", RegexOptions.Multiline);
		if (!isValidUrl)
		{
			errors.Add("La url no tiene un valor valido correcto");
		}
		bool existErrors = errors.Count() > 0;
		if (!existErrors)
		{
			UserConfig.Set("url-bifrost", url);
		}
		return !existErrors;
	}

	public static bool TrySetRuc(string ruc, out List<string> errors)
	{
		errors = new List<string>();
		ruc = ruc.Trim();
		if (ruc.Length == 0)
		{
			errors.Add("El ruc no debe ser un campo vacio");
		}
		bool isValidRuc = Regex.IsMatch(ruc, @"^\d{11}$", RegexOptions.Multiline);
		if (!isValidRuc)
		{
			errors.Add("El ruc debe tener 11 digitos numericos");
		}
		bool existErrors = errors.Count() > 0;
		if (!existErrors)
		{
			UserConfig.Set("ruc", ruc);
		}
		return !existErrors;
	}

	public static bool TrySetNamespace(string namespaceBifrost, out List<string> errors)
	{
		errors = new List<string>();
		namespaceBifrost = namespaceBifrost.Trim();
		if (namespaceBifrost.Length == 0)
		{
			errors.Add("El namespace no debe ser un campo vacio");
		}
		bool isValidNamespace = Regex.IsMatch(namespaceBifrost, @"^[a-zA-Z]+:[a-zA-Z]+$", RegexOptions.Multiline);
		if (!isValidNamespace)
		{
			errors.Add("El namespace no tiene un formato correcto");
		}
		bool existsErrors = errors.Count() > 0;
		if (!existsErrors)
		{
			UserConfig.Set("namespace", namespaceBifrost);
		}
		return !existsErrors;
	}

	public static bool TrySetSuffix(string suffix, out List<string> errors)
	{
		errors = new List<string>();
		suffix = suffix.Trim();
		bool isValidSuffix = Regex.IsMatch(suffix, @"^\d?$", RegexOptions.Multiline);
		if (!isValidSuffix)
		{
			errors.Add("El sufijo solo debe tener un digito numérico");
		}
		bool existsErrors = errors.Count() > 0;
		if (!existsErrors)
		{
			UserConfig.Set("suffix", suffix);
		}
		return !existsErrors;
	}


	public static string GetUrl()
	{
		return UserConfig.Get("url-bifrost") ?? "";
	}

	public static string GetRuc()
	{
		return UserConfig.Get("ruc") ?? "";
	}

	public static string GetSuffix()
	{
		return UserConfig.Get("suffix") ?? "";
	}

	public static string GetNamespace()
	{
		return UserConfig.Get("namespace") ?? "";
	}
}