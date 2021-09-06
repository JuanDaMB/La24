using System;

[Serializable]
public class RewardData
{
	public int place;
	public string value;

	public RewardData(int place, string value)
	{
		this.place = place;
		this.value = value;
	}

	public string GetFormatedValue()
	{
		string val = string.Empty;
		try
		{
			val = "$ " + int.Parse(value).ToString("N0");
		}
		catch (Exception)
		{
			val = value;
		}
		return val;
	}

	public int GetIntValue()
	{
		int val = 0;
		if (int.TryParse(value, out val))
			return val;

		return val;
	}
}