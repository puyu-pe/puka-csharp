namespace puka;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using printer_aplication_desktop.components;
using SocketIOClient;
using System.Text.Json;

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

	private async Task PrintTickets(Dictionary<string, JsonElement>? queue)
	{
		if (queue != null)
		{
			foreach (KeyValuePair<string, JsonElement> kvp in queue)
			{
				JObject register = JObject.Parse(kvp.Value.ToString());
				if (register != null)
				{
					var tickets = JsonConvert.DeserializeObject<dynamic>(register.GetValue("tickets").ToString())[0];
					EscPosClass printer = new EscPosClass(tickets);
					printer.PrinterDocument();
					await client.EmitAsync("printer:printed", new BifrostDeleteRequest { Key = kvp.Key });
				}
			}
		}
	}

	private async void OnLoadQueue(SocketIOResponse response)
	{
		var bifrostResponse = response.GetValue<BifrostResponse>();
		if (bifrostResponse.Status == "success")
		{
			Program.Logger.Info("Servidor: " + bifrostResponse.Message);
			await PrintTickets(bifrostResponse.Data);
		}
		else
		{
			Program.Logger.Warn("PukaClient: no se pudo recuperar la cola de impresión");
		}
	}

	private async void OnToPrint(SocketIOResponse response)
	{
		var bifrostResponse = response.GetValue<BifrostResponse>();
		Program.Logger.Info("Servidor: " + bifrostResponse.Message);
		await PrintTickets(bifrostResponse.Data);
	}

	private async void OnConnected(object? sender, EventArgs e)
	{

		Program.Logger.Info("Conexión a {0}", client.ServerUri.AbsoluteUri);
		await client.EmitAsync("printer:start");
	}

	private async void OnError(object? sender, string e)
	{
		Program.Logger.Error("SocketIOClient error: {0} server uri: {1}", e, client.ServerUri.AbsoluteUri);
		if (forceConnectIntent < 2)
		{
			await client.ConnectAsync();
			Program.Logger.Info("Forzando conexión con {0} intento: {1}", client.ServerUri.AbsoluteUri, forceConnectIntent);
			forceConnectIntent++;
		}
		else
		{
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

	private void OnDisconnected(object? sender, string e)
	{
		Program.Logger.Info("PukaClient: Se desconecto {0}", e);
	}

	public async Task Start()
	{
		await client.ConnectAsync();
	}
}