namespace puka;

using Newtonsoft.Json.Linq;
using SocketIOClient;

public class PukaClient
{
	private readonly SocketIO client;
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
			//TODO: recorrer los tickets
			//bifrostResponse.Data;
			await client.EmitAsync("printer:printed", new JObject { ["key"] = "34234" });
			//imprimir la los tickets , Data[{ key =>  {created_at, tickets, namespace}}, ...]
		}else{
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
		Program.Logger.Info("Conexión exitosa a bifrost.io");
		await client.EmitAsync("printer:start");
	}

	private void OnError(object? sender, string e)
	{
		Program.Logger.Error("SocketIOClient error: {0}", e);
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