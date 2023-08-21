namespace puka.app;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using puka.util.printer;
using SocketIOClient;
using System.Text.Json;


public class PukaClient
{
	private readonly SocketIO client;
	private int forceConnectIntent = 1;

	public delegate void OnErrorDetected(string error);
	OnErrorDetected onErrorDetected = (string message) =>
	{
		Program.Logger.Error("Ocurrio un error: ", message);
	};


	public delegate void OnReconnectAttemptBifrost(int intent);
	OnReconnectAttemptBifrost onReconnectAttemptBifrost = (int intent) =>
	{
		Program.Logger.Warn($"Tratando de reconectarse a bifrost intento numero ${intent}");
	};

	public delegate void OnConnectedSuccess();
	OnConnectedSuccess onConnectedSuccess = () =>
	{
		Program.Logger.Info("Conexión exitosa a bifrost");
	};

	public delegate void OnAfterPrinting(bool status);
	OnAfterPrinting onAfterPrinting = (bool _) =>
	{
		Program.Logger.Warn("Estado despues de imprimir" + _);
	};

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
		if (queue == null)
			return;
		bool succesPrinting = true;
		Program.Logger.Info($"Se recibe {queue.Count} elementos a imprimir");
		foreach (KeyValuePair<string, JsonElement> kvp in queue)
		{
			try
			{
				JObject register = JObject.Parse(kvp.Value.ToString());
				if (!register.TryGetValue("tickets", out JToken? dataToPrintFromServer))
				{
					Program.Logger.Warn($"PrintTickets: No se obtuvieron los tickets de ${kvp.Key} del servidor");
					continue;
				}

				if (!TryConvertObjectFromJson(dataToPrintFromServer, out dynamic? dataToPrintObject))
				{
					Program.Logger.Warn($"PrintTickets: No se pudo convertir los tickets de ${kvp.Key} a un dynamic object");
					continue;
				}

				if (dataToPrintObject == null)
				{
					Program.Logger.Warn($"PrintTickets: la información a imprimir de ${kvp.Key} es null");
					continue;
				}

				var tickets = dataToPrintObject[0];
				await new EscPosClass(tickets).PrinterDocument();
				await client.EmitAsync("printer:printed", new BifrostDeleteRequest { Key = kvp.Key });
			}
			catch (Exception e)
			{
				Program.Logger.Warn(e, e.Message);
				succesPrinting = false;
			}
		}
		onAfterPrinting(succesPrinting);
	}

	public void SetOnErrorDetected(OnErrorDetected onErrorDetected)
	{
		this.onErrorDetected = onErrorDetected;
	}

	public void SetOnReconnectAttemptBifrost(OnReconnectAttemptBifrost onReconnectAttemptBifrost)
	{
		this.onReconnectAttemptBifrost = onReconnectAttemptBifrost;
	}

	public void SetOnconnectedSuccess(OnConnectedSuccess onConnectedSuccess)
	{
		this.onConnectedSuccess = onConnectedSuccess;
	}

	public void SetOnAfterPrinting(OnAfterPrinting onAfterPrinting)
	{
		this.onAfterPrinting = onAfterPrinting;
	}

	public async Task RequestToLoadPrintQueue()
	{
		await client.EmitAsync("printer:start");
	}

	private bool TryConvertObjectFromJson<Type>(JToken json, out Type? obj)
	{
		obj = JsonConvert.DeserializeObject<Type>(json.ToString());
		return obj != null;
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
		onConnectedSuccess();
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
			onErrorDetected(e);
			Program.Logger.Error("No se pudo conectar a {0}", client.ServerUri.AbsoluteUri);
			Application.Exit();
		}
	}

	private void OnReconnectAttempt(object? sender, int intent)
	{
		Program.Logger.Info("Reintentando conexión con el servidor intento: {0}", intent);
		onReconnectAttemptBifrost(intent);
	}

	private async void OnReconnected(object? sender, int intent)
	{
		await RequestToLoadPrintQueue();
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