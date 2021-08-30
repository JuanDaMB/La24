using System;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class PrizesTableRequest
	{
	}

	[Serializable]
	public class PrizesTableResponse
	{
		public PrizeContent betTypes;
		public PrizeContent maxBetTypes;
	}

	[Serializable]
	public class PrizeContent
	{
		public int Par_Impar;
		public int Pleno;
		public int Altos_Bajos;
		public int Octeto;
		public int Rojo_Negro;
		public int Fila;
		public int Cuadrado;
		public int Linea;
		public int Medio;
	}
}