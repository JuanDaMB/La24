using System.Xml;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Singlenton Administrador XML
/// </summary>
namespace IndieLevelStudio.Common
{
	public class XMLModulesManager
	{
		public const string prefix = "/ModulesConfig";

		public static int MaxAwards;
		public static int MaxAwards2;

		public static string SignMoney;
		public static string SignCredit;
		public static string MessageDayPlayed;
		public static string MessageDayPlayed2;
		public static string Nit;
		public static string Operators;
		public static string EmailCompany;
		public static string ServerURL;
		public static string TimeAfkToVideo;
		public static string AdminUser;
		public static string AdminPassword;
		public static bool ModeAdmin;

		public static string MessageWin;

		public static string Award1;
		public static string Award2;
		public static string Award3;
		public static string Award4;
		public static string Award5;
		public static string Award6;
		public static string Award7;

		public static string DefaultUserName;
		public static string DefaultUserCC;
		public static string DefaultUserMail;

		public static XmlDocument xmlLevelConfiguration;
		public static string textLevelConfiguration;

		private static string path;
		public static string pathLevelConfiguration;

		public static string GNANumber;

		public static bool UseGNA;

		//Money Bets
		public static int BetsTime;

		public static List<int> PrizeMultipliers;

		public static void LoadXML()
		{
#if UNITY_WEBGL
			TextAsset resourceFile = (TextAsset)Resources.Load("Config Modules File", typeof(TextAsset));
			textLevelConfiguration = resourceFile.text;
#else
			pathLevelConfiguration = GetDataPath("Configuration File.xml");
			textLevelConfiguration = System.IO.File.ReadAllText(pathLevelConfiguration);
#endif

			xmlLevelConfiguration = new XmlDocument();
			xmlLevelConfiguration.LoadXml(textLevelConfiguration.ToString());

			LoadXMLValues();
			LoadMoneyXMLValues();
		}

		private static void LoadMoneyXMLValues()
		{
			BetsTime = int.Parse(xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/MoneyBet/BetsTime").InnerText);

			PrizeMultipliers = new List<int>();
			for (int i = 1; i <= 7; i++)
				PrizeMultipliers.Add(int.Parse(xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/MoneyBet/BetMultipliers/BetMultiplier" + i).InnerText));
		}

		private static void LoadXMLValues()
		{
			string numberMaxAwards = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/MaxAwards").InnerText;
			MaxAwards = int.Parse(numberMaxAwards);

			string numberMaxAwards2 = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/MaxAwards2").InnerText;
			MaxAwards2 = int.Parse(numberMaxAwards2);

			UseGNA = bool.Parse(xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/UseGNA").InnerText);
			GNANumber = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/DebugNumber").InnerText;

			SignMoney = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/SignMoney").InnerText;
			SignCredit = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/SignCredit").InnerText;
			MessageDayPlayed = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/MessageDayPlayed").InnerText;
			MessageDayPlayed2 = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/MessageDayPlayed2").InnerText;
			Nit = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/Nit").InnerText;
			Operators = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/Operators").InnerText;
			EmailCompany = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/EmailCompany").InnerText;
			ServerURL = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/ServerURL").InnerText;
			TimeAfkToVideo = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/TimeAfkToVideo").InnerText;
			AdminUser = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/AdminUser").InnerText;
			AdminPassword = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/AdminPassword").InnerText;
			string modeAdminString = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/ModeAdmin").InnerText;
			ModeAdmin = bool.Parse(modeAdminString);

			MessageWin = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/MessageWin").InnerText;
			//////////////////////////////////////////////////////////////////////////////////////////////////

			Award1 = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/Awards/Award1").InnerText;
			Award2 = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/Awards/Award2").InnerText;
			Award3 = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/Awards/Award3").InnerText;
			Award4 = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/Awards/Award4").InnerText;
			Award5 = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/Awards/Award5").InnerText;
			Award6 = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/Awards/Award6").InnerText;
			Award7 = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/Awards/Award7").InnerText;

			/////////////////////////////////////////////////////////////////////////////////////////////////

			DefaultUserName = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/DefaultUserData/DefaultName").InnerText;
			DefaultUserCC = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/DefaultUserData/DefaultId").InnerText;
			DefaultUserMail = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/DefaultUserData/DefaultMail").InnerText;
		}

		private static string GetDataPath(string fileName)
		{
			string path = System.IO.Path.Combine(Application.persistentDataPath, fileName);

			Debug.Log("persistentDataPath:: " + path);

			//original path
			string sourcePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

			//if file does not exist in persistent data folder (folder "Documents" on iOS) or source data is newer then copy it
			if (!System.IO.File.Exists(path) || (System.IO.File.GetLastWriteTimeUtc(sourcePath) > System.IO.File.GetLastWriteTimeUtc(path)))
			{
				if (sourcePath.Contains("://"))
				{
					// Android
					WWW www = new WWW(sourcePath);
					// Wait for download to complete - not pretty at all but easy hack for now
					// and it would not take long since the data is on the local device.
					while (!www.isDone)
					{
						;
					}

					if (string.IsNullOrEmpty(www.error))
					{
						System.IO.File.WriteAllBytes(path, www.bytes);
					}
				}
				else
				{
					// Mac, Windows, Iphone

					//validate the existens of the DB in the original folder (folder "streamingAssets")
					if (System.IO.File.Exists(sourcePath))
					{
						//copy file - alle systems except Android
						System.IO.File.Copy(sourcePath, path, true);
						Debug.Log("                 Copy:: source:" + sourcePath + " dest:" + path);
					}
					else
					{
						Debug.Log("ERROR: the file named " + fileName + " doesn't exist in the StreamingAssets Folder, please copy it there.");
					}
				}
			}
			Debug.Log("path:: " + path);

			return path;
		}
	}
}