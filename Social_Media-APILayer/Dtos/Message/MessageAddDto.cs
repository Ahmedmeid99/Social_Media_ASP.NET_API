namespace Social_Media_APILayer.Dtos.Message
{
	public class MessageAddDto: MessageEditDto
	{
		public int SenderId { get; set; }

		public int ReceiverId { get; set; }

		public DateTime? SentAt { get; set; }
	}
}
