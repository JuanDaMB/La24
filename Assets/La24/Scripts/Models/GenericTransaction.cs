using System;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class GenericTransaction<T> where T : class
	{
		public string gpUser;
		public string system;
		public string msgName;
		public string sysOperator;
		public string player;
		public string token;
		public string game;
		public T msgDescrip;

		public GenericTransaction()
		{
			gpUser = GlobalObjects.gpUser;
			system = GlobalObjects.system;
			sysOperator = GlobalObjects.sysOperator;
			player = GlobalObjects.player;
			token = GlobalObjects.token;
			game = GlobalObjects.game;
		}
	}
}