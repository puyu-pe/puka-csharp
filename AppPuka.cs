
namespace puka;

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
		// FormConfig formConfig = new();
		// uri = UserConfig.Get("uri");

		// if (string.IsNullOrEmpty(uri))
		// {
		// 	formConfig.ShowDialog();
		// 	uri = formConfig.GetUrlBifrostServer();
		// 	if (uri.Length > 8)
		// 	{
		// 		UserConfig.Add("uri", uri);
		// 	}
		// }
		// else
		// {
		uri = MakeUrlBifrost();
		new PukaForm().ShowDialog();
		new TrayIconPrinter().Show();
		await new PukaClient(uri).Start();
		// }
	}

	private string MakeUrlBifrost()
	{
		string ruc = UserConfig.Get("ruc") ?? "";
		string suffix = UserConfig.Get("suffix") ?? "";
		string urlBifrost = UserConfig.Get("url-bifrost") ?? "";
		string namespaceBifrost = UserConfig.Get("namespace") ?? "";
		return string.Concat(urlBifrost, "/", namespaceBifrost, "-", ruc, "-", suffix);
	}

}

