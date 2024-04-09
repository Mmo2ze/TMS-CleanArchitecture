namespace TMS.Infrastructure.Services.WhatsappSender.ApiDefinition.Responses;

public record GetMessagesResponse
{
		public List<Message> Messages { get; set; }
		public bool HasMorePages { get; set; }
}
	public record Message
	{
		public string id { get; set; }
		public string from { get; set; }
		public string to { get; set; }
		public object author { get; set; }
		public string pushname { get; set; }
		public string message_type { get; set; }
		public string status { get; set; }
		public string body { get; set; }
		public string caption { get; set; }
		public int forwarded { get; set; }
		public object quoted_message_id { get; set; }
		public string mentioned_ids { get; set; }
		public DateTime timestamp { get; set; }
	}
