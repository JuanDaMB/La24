using System;

[Serializable]
public class PreviousBet
{
	public string value;
	public string color;

	public PreviousBet(string value, string color)
	{
		this.value = value;
		this.color = color;
	}
}