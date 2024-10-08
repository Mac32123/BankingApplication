﻿namespace BankApp.Server.Models
{
	public class Response
	{
		public string RequestId => $"{Guid.NewGuid().ToString()}";
		public string ResponseCode { get; set; }
		public string ResponseMessage { get; set; }
		public string FromName { get; set; }
		public string ToName { get; set; }
		public object? Data { get; set; }

	}
}
