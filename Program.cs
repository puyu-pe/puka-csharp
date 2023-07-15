using puka.view;

namespace puka;


static internal class Program
{
	public static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
	[STAThread]
	static async Task Main()
	{
		try
		{
			ConfigLogger.ToDirectory(@"C:\Users\OSCAR\Desktop\escritorio\puyu_practices\puka\logs");
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			FormConfig formConfig = new(); 
			if(formConfig.ShowDialog() == DialogResult.OK){
				PukaClient client = new("https://bifrost-io.puyu.pe/yures:printer-123432-pe");
				await client.Start();
				Application.Run(new TrayIconPrinter());
			}
		}
		catch (Exception e)
		{
			Logger.Error(e, "ocurrio un error al iniciar el programa: {0}", e.Message);
			throw;
		}
	}
}