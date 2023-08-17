using puka.app;
using puka.util;

namespace puka;

public class TrayIconPrinter
{
	private readonly NotifyIcon trayIcon;
	private readonly PukaClient pukaClient;
	private ToolStripItem loadPrinterQueueItem;

	public TrayIconPrinter(PukaClient pukaClient)
	{
		loadPrinterQueueItem = new ToolStripMenuItem("Cargar cola de impresión", null, LoadPrinterQueue, "LOAD-QUEUE");
		this.pukaClient = pukaClient;
		trayIcon = new NotifyIcon()
		{
			Text = "PUKA - YURES",
			Icon = new Icon("printer.ico"),
			ContextMenuStrip = new ContextMenuStrip(),
			Visible = true,
		};

		this.pukaClient.SetOnAfterPrinting(OnAfterPrinting);


		trayIcon.ContextMenuStrip.Items.AddRange(new ToolStripItem[]
		{
				loadPrinterQueueItem,
				new ToolStripMenuItem("salir", null,Close, "OPEN"),
		});
	}


	public void Show()
	{
		trayIcon.Visible = true;
		loadPrinterQueueItem.Enabled = false;
	}

	private void OnAfterPrinting(bool successPrint)
	{
		if (!successPrint)
		{
			NotifyUserOnFailedToPrint();
		}
		loadPrinterQueueItem.Enabled = !successPrint;
	}

	private void NotifyUserOnFailedToPrint()
	{
		string message = "Revise conexión con la impresora";
		trayIcon.ShowBalloonTip(2000, "Algunos tickets no se imprimieron", message, ToolTipIcon.Warning);
	}

	private async void LoadPrinterQueue(object? sender, EventArgs e)
	{
		try
		{
			loadPrinterQueueItem.Enabled = false;
			await pukaClient.RequestToLoadPrintQueue();
		}
		catch (System.Exception ex)
		{
			Program.Logger.Error(ex, "TrayIcon error: LoadPrinterQueue: ", ex.Message);
		}
	}

	private void Close(object? sender, EventArgs e)
	{
		trayIcon.Visible = false;
		trayIcon.Dispose();
		Application.Exit();
	}

}