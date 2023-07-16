
namespace puka;

using puka.view;

public class AppPuka : ApplicationContext{
	private	string? uri = "not uri";

	public AppPuka(){
		try
		{
			Run();
		}
		catch (System.Exception e)
		{
			Program.Logger.Fatal(e,"No se pudo iniciar PUKA error: {0} al conectarse a {1}", e.Message ,uri);
			Application.Exit();
		}
	}

	private async void Run(){
			FormConfig formConfig = new();
			uri = UserConfig.Get("uri");

			if (string.IsNullOrEmpty(uri))
			{
				if (!(formConfig.ShowDialog() == DialogResult.OK)){
					Application.Exit();
					return;
				}
				else{
					uri = formConfig.GetUrlBifrostServer();
					if(uri.Length > 8)
						UserConfig.Add("uri",uri);
				}
			}
			new TrayIconPrinter().Show();
			await new PukaClient(uri).Start();
	}
	

}

