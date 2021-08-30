using System;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class LoginRequest
	{
		public string username;
		public string password;
		public bool rememberMe;
	}

	[Serializable]
	public class LoginResponse
	{
		public string id_token;
	}

	[Serializable]
	public class ErrorTransaction
	{
		public string type;
		public string title;
		public int status;
	}
}