namespace LightningPay.Clients.CLightning
{
	internal class CommandRequest
	{
		[Serializable("id")]
		public int Id { get; set; }

		[Serializable("method")]
		public string Command { get; set; }

		[Serializable("params")]
		public object[] Parameters { get; set; }
	}
}
