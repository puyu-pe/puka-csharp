namespace puka;

using System.Text.Json.Serialization;
using System.Text.Json;

class BifrostResponse
{
	[JsonPropertyName("message")]
	public string? Message { get; set; }

	[JsonPropertyName("data")]
	public Dictionary<string,JsonElement>? Data { get; set; }

	[JsonPropertyName("status")]
	public string? Status { get; set; }
}