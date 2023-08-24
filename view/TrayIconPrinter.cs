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
			Icon = new Icon("icono.ico"),
			ContextMenuStrip = new ContextMenuStrip(),
			Visible = true,
		};

		this.pukaClient.SetOnAfterPrinting(OnAfterPrinting);
		this.pukaClient.SetOnErrorDetected(OnErrorDetectedOnPukaClient);
		this.pukaClient.SetOnReconnectAttemptBifrost(OnReconnectAttemptBifrost);
		this.pukaClient.SetOnconnectedSuccess(OnConnectedSuccessBifrost);


		trayIcon.ContextMenuStrip.Items.AddRange(new ToolStripItem[]
		{
				loadPrinterQueueItem,
				new ToolStripMenuItem("Cambiar logo",null,ChangeCompanyLogo,"CHANGE-LOGO")
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

	private void OnErrorDetectedOnPukaClient(string message)
	{
		trayIcon.ShowBalloonTip(2000, "Se detecto un error en el cliente", message, ToolTipIcon.Error);
	}

	private void OnReconnectAttemptBifrost(int intent)
	{
		if (intent == 1 || intent % 2 == 0)
		{
			trayIcon.ShowBalloonTip(3000, "No hay conexión con el servidor", "Puede deberse a una mala conexión a internet, o el servidor a caido.", ToolTipIcon.Error);
		}
	}

	private void OnConnectedSuccessBifrost()
	{
		trayIcon.ShowBalloonTip(2000, "Conexión exitosa al servidor", "Se logro establecer una conexión exitosa al servidor", ToolTipIcon.Info);
	}

	private void NotifyUserOnFailedToPrint()
	{
		string message = "Revise conexión con la impresora, posiblemente este desconectada";
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

	private void ChangeCompanyLogo(object? sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new()
		{
			Filter = "Archivos de imagen|*.png;*.jpg;*.jpeg|Archivos PNG (*.png)|*.png|Archivos JPG (*.jpg, *.jpeg)|*.jpg;*.jpeg"
		};
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			string userFolderPath = UserConfig.GetPukaFolderPath();
			string logoFileSourcePath = openFileDialog.FileName;
			string logoFileDestinyPath = Path.Combine(userFolderPath, "logo_empresa" + Path.GetExtension(logoFileSourcePath));

			if (File.Exists(logoFileDestinyPath))
			{
				File.Delete(logoFileDestinyPath);
			}
			File.Copy(logoFileSourcePath, logoFileDestinyPath);
			UserConfig.Set("logo-path", logoFileDestinyPath);
			if (File.Exists(UserConfig.GetLogoPath()))
			{
				trayIcon.ShowBalloonTip(2000, "Se cambio el logo", "se modifico el logo exitosamente", ToolTipIcon.Info);
			}
			else
			{
				trayIcon.ShowBalloonTip(2000, "No se pudo cambiar el logo", "Solo se permiten imagenes .png, .jpg y .jpeg", ToolTipIcon.Info);
			}
		}
	}
}