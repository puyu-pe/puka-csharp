namespace puka;

using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

class BifrostResponse
{
	[JsonPropertyName("message")]
	public string? Message { get; set; }

	//TODO: modificar segun la estructura correcta de los datos
	/*
		TODO: key: string => {
			created_at: string,
			data: [tickets: any],
			namespace: string
		} 
	*/
	[JsonPropertyName("data")]
	public Dictionary<string,string>? Data { get; set; }

	[JsonPropertyName("status")]
	public string? Status { get; set; }
}