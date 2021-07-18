namespace LightningPay.Clients.CLightning
{
	internal class CommandResponse<Response>
	{
		[Serializable("error")]
		public CommandResponseError Error { get; set; }

		[Serializable("result")]
		public Response Result { get; set; }

		public class CommandResponseError
		{
			[Serializable("message")]
			public string Message { get; set; }

			[Serializable("code")]
			public int Code { get; set; }
		}
	}
}
