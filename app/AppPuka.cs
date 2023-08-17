
namespace puka.app;

using puka.view;

public class AppPuka : ApplicationContext
{
	private string? uri = "not uri";

	public AppPuka()
	{
		try
		{
			Run();
		}
		catch (System.Exception e)
		{
			Program.Logger.Fatal(e, "No se pudo iniciar PUKA error: {0} al conectarse a {1}", e.Message, uri);
			Application.Exit();
		}
	}

	private async void Run()
	{
		if (!LoadConfigBifrost())
		{
			DialogResult dialogResult = new PukaForm().ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				await StartPukaClient();
			}
		}
		else
		{
			await StartPukaClient();
		}
	}

	private async Task StartPukaClient()
	{
		uri = MakeUrlBifrost();
		PukaClient pukaClient = new(uri);
		new TrayIconPrinter(pukaClient).Show();
		await pukaClient.Start();
	}

	private bool LoadConfigBifrost()
	{
		return BifrostConfig.TrySetSuffix(BifrostConfig.GetSuffix(), out var e_suffix)
			&& BifrostConfig.TrySetNamespace(BifrostConfig.GetNamespace(), out var e_namespace)
			&& BifrostConfig.TrySetUrl(BifrostConfig.GetUrl(), out var e_url)
			&& BifrostConfig.TrySetRuc(BifrostConfig.GetRuc(), out var e_ruc);
	}

	private string MakeUrlBifrost()
	{
		string ruc = BifrostConfig.GetRuc();
		string urlBifrost = BifrostConfig.GetUrl();
		string namespaceBifrost = BifrostConfig.GetNamespace();
		string suffix = BifrostConfig.GetSuffix();
		if (suffix.Length > 0)
		{
			suffix = "-" + suffix;
		}
		return string.Concat(urlBifrost, "/", namespaceBifrost, "-", ruc, suffix);
	}

}

