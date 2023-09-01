using puka.app;
using puka.util;

namespace puka;

public class TrayIconPrinter
{
	private readonly NotifyIcon trayIcon;
	private readonly PukaClient pukaClient;

	private ToolStripMenuItem actionsPrintQueue;

	private int numberItemsQueueSnapshot = 0;

	public TrayIconPrinter(PukaClient pukaClient)
	{
		this.pukaClient = pukaClient;
		actionsPrintQueue = new ToolStripMenuItem("Cola de impresión: 0", null, ActionsPrintQueue, "ACTIONS-PRINT-QUEUE")
		{
			Enabled = false
		};
		trayIcon = new NotifyIcon()
		{
			Text = "PUKA - YURES",
			Icon = new Icon("puka.ico"),
			ContextMenuStrip = new ContextMenuStrip(),
			Visible = true,
		};

		actionsPrintQueue.DropDownItems.AddRange(new ToolStripItem[]{
			new ToolStripMenuItem("ELIMINAR",null,ReleasePrinterQueue,"RELEASE-QUEUE"),
			new ToolStripMenuItem("IMPRIMIR",null,LoadPrinterQueue,"LOAD-QUEUE")
		});


		this.pukaClient.SetOnChangePrintTicketsEnabled(OnChangePrintTicketsEnabled);
		this.pukaClient.SetOnFailedToPrint(OnFailedToPrint);
		this.pukaClient.SetOnChangeNumberItemsQueue(OnChangeNumberItemsQueue);
		this.pukaClient.SetOnErrorDetected(OnErrorDetectedOnPukaClient);
		this.pukaClient.SetOnReconnectAttemptBifrost(OnReconnectAttemptBifrost);
		this.pukaClient.SetOnconnectedSuccess(OnConnectedSuccessBifrost);


		trayIcon.ContextMenuStrip.Items.AddRange(new ToolStripItem[]
		{
			actionsPrintQueue,
			new ToolStripMenuItem("Cambiar logo",null,ChangeCompanyLogo,"CHANGE-LOGO")
		});
	}

	public void Show()
	{
		trayIcon.Visible = true;
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

	private void OnChangeNumberItemsQueue(int numberItemsQueue)
	{
		actionsPrintQueue.Text = "Cola de impresión: " + numberItemsQueue;
		actionsPrintQueue.Enabled = numberItemsQueue != 0;
		numberItemsQueueSnapshot = numberItemsQueue;
	}

	private void OnFailedToPrint(string details)
	{
		trayIcon.ShowBalloonTip(2000, "Hubo un fallo al imprimir un ticket", $"No se logro imprimir un ticket error: {details}", ToolTipIcon.Error);
	}

	private void OnChangePrintTicketsEnabled(bool enabled)
	{
		actionsPrintQueue.Enabled = enabled && numberItemsQueueSnapshot != 0;
	}

	private async void LoadPrinterQueue(object? sender, EventArgs e)
	{
		try
		{
			DialogResult result = MessageBox.Show("¿Quieres volver a imprimir los tickets en cola de impresión?", "VENTANA DE CONFIRMACIÓN", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
			if (result == DialogResult.OK)
			{
				await pukaClient.RequestToLoadPrintQueue();
			}
		}
		catch (System.Exception ex)
		{
			Program.Logger.Error(ex, "TrayIcon error: LoadPrinterQueue: ", ex.Message);
		}
	}

	private async void ReleasePrinterQueue(object? sender, EventArgs e)
	{
		try
		{
			DialogResult result = MessageBox.Show("¿Seguro que quieres eliminar los tickets en cola de impresión?", "VENTANA DE CONFIRMACIÓN", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
			if (result == DialogResult.OK)
			{
				await pukaClient.RequestToReleaseQueue();
			}
		}
		catch (System.Exception ex)
		{
			Program.Logger.Error(ex, $"TrayIcon error: LoadPrinterQueue: {ex.Message}");
		}
	}


	private void ActionsPrintQueue(object? sender, EventArgs e)
	{
		//TODO: Abrir dialogo de cola de impresión
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