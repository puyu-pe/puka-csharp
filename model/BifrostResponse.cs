namespace puka;

using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

class BifrostResponse
{
	[JsonPropertyName("message")]
	public string? Message { get; set; }

	[JsonPropertyName("data")]
	public JObject? Data { get; set; }

	[JsonPropertyName("status")]
	public string? Status { get; set; }
}