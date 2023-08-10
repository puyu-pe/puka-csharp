namespace puka;
using System.Text.Json.Serialization;
class BifrostDeleteRequest
{
	[JsonPropertyName("key")]
	public string? Key { get; set; }

}