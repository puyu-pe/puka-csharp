namespace puka;

public class TrayIconPrinter : ApplicationContext
{
	private readonly NotifyIcon trayIcon;

	public TrayIconPrinter()
	{
		trayIcon = new NotifyIcon()
		{
			Text = "PUKA - YURES",
			Icon = new Icon("printer.ico"),
			ContextMenuStrip = new ContextMenuStrip(),
			Visible = true,
		};

		trayIcon.ContextMenuStrip.Items.AddRange(new ToolStripItem[]
		{
				new ToolStripMenuItem("salir", null,Close, "OPEN"),
				// new ToolStripMenuItem("EXIT", null, new EventHandler(Exit), "EXIT")
		});
	}

	private void Close(object? sender, EventArgs e)
	{
		trayIcon.Visible = false;
		trayIcon.Dispose();
		Application.Exit();
	}

	private void Settings(object? sender, EventArgs e)
	{

	}

}