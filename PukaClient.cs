namespace puka;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using printer_aplication_desktop.components;
using SocketIOClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class PukaClient
{
	private readonly SocketIO client;
	private int forceConnectIntent = 1; 
	public PukaClient(string uri)
	{
		client = new SocketIO(new Uri(uri), new SocketIOOptions
		{
			Reconnection = true,
			ReconnectionDelay = 2000,
		});

		client.On("printer:load-queue", OnLoadQueue);
		client.On("printer:to-print", OnToPrint);
		client.OnConnected += OnConnected;
		client.OnError += OnError;
		client.OnReconnectAttempt += OnReconnectAttempt;
		client.OnReconnected += OnReconnected;
		client.OnDisconnected += OnDisconnected;
	}

	private async void OnLoadQueue(SocketIOResponse response)
	{
		var bifrostResponse = response.GetValue<BifrostResponse>();
		if (bifrostResponse.Status == "success")
		{
			Program.Logger.Info("PukaClient: Se obtuvo toda la cola de impresión");

			//if (bifrostResponse.Data != null) {
                foreach (dynamic register in bifrostResponse.Data)
                {
					var json = register[""];

                    dynamic dataPrinter = JsonConvert.DeserializeObject<dynamic>(json.GetValue("tickets").ToString());
				
                    EscPosClass connectorPrinterFinal = new EscPosClass(dataPrinter);

                    connectorPrinterFinal.PrinterDocument();
                }
            //}          
        }
        else{
			Program.Logger.Warn("PukaClient: no se pudo recuperar la cola de impresión");
		}
	}

	private async void OnToPrint(SocketIOResponse response)
	{
		// var bifrostResponse = response.GetValue<BifrostResponse>();
		//bifrostResponse.Data , es lo unico que llega {key  => {created_at, tickets, namespace}} ;
		Program.Logger.Info("PukaClient: Se obutvo un ticket para imprimir");
		//Todo: llamar a la impresora
		await client.EmitAsync("printer:printed", new JObject { ["key"] = "dsjfskey" });
	}

	private async void OnConnected(object? sender, EventArgs e)
	{

		Program.Logger.Info("Conexión a {0}", client.ServerUri.AbsoluteUri);
		await client.EmitAsync("printer:start");
	}

	private async void OnError(object? sender, string e)
	{
		Program.Logger.Error("SocketIOClient error: {0} server uri: {1}",e, client.ServerUri.AbsoluteUri);
		if(forceConnectIntent < 2){
			await client.ConnectAsync();
			Program.Logger.Info("Forzando conexión con {0} intento: {1}", client.ServerUri.AbsoluteUri, forceConnectIntent);
			forceConnectIntent++;
		}else{
			Program.Logger.Error("No se pudo conectar a {0}", client.ServerUri.AbsoluteUri);
			Application.Exit();
		}
	}

	private void OnReconnectAttempt(object? sender, int intent)
	{
		Program.Logger.Info("Reintentando conexión con el servidor intento: {0}", intent);
	}

	private async void OnReconnected(object? sender, int intent)
	{
		await client.EmitAsync("printer:start");
		Program.Logger.Info("Se Recupero conexión con el servidor intento: {0}", intent);
	}

	private void OnDisconnected(object? sender, string e){
		Program.Logger.Info("PukaClient: Se desconecto {0}",e);
	}

	public async Task Start()
	{
		await client.ConnectAsync();
	}
}