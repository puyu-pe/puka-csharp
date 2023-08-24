namespace puka;
using puka.app;
using puka.util;


static internal class Program
{
	public static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	private static void InitConfiguration()
	{
		ConfigLogger.Load();
		UserConfig.Load();
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(true);
	}

	[STAThread]
	static void Main()
	{
		try
		{
			InitConfiguration();
			Application.Run(new AppPuka());
		}
		catch (Exception e)
		{
			Logger.Fatal(e, "ocurrio un error al iniciar el programa: {0} no se pudo conectar", e.Message);
			Application.Exit();
		}
	}
}