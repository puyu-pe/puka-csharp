
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
		DialogResult dialogResult = new PukaForm().ShowDialog();
		if (dialogResult == DialogResult.OK)
		{
			uri = MakeUrlBifrost();
			new TrayIconPrinter().Show();
			await new PukaClient(uri).Start();
		}
	}

	private string MakeUrlBifrost()
	{
		string ruc = BifrostConfig.GetRuc();
		string urlBifrost = BifrostConfig.GetUrl();
		string namespaceBifrost = BifrostConfig.GetNamespace();
		string suffix = BifrostConfig.GetSuffix();
		return string.Concat(urlBifrost, "/", namespaceBifrost, "-", ruc, "-", suffix);
	}

}

