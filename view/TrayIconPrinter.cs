namespace puka;

public class TrayIconPrinter 
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
		});
	}

	public void Show(){
		trayIcon.Visible = true;
	}

	private void Close(object? sender, EventArgs e)
	{
		trayIcon.Visible = false;
		trayIcon.Dispose();
		Application.Exit();
	}

}