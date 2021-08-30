using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Singlenton Administrador XML
/// </summary>
namespace IndieLevelStudio.Common
{
    public class XMLManager
    {
        public const string prefix = "/La24";

        public static string UserName;
        public static string UserPass;
        public static bool RememberUser;

        public static string UrlGeneric;
        public static string UrlLogin;
        public static string SocketURL;

        public static float HintsTime;

        public static string ScreenshotsDirectory;

        public static string UserMessagesTitle;
        public static List<string> UserMessages;

        public static string GenericTransactionError;

        public static XmlDocument xmlLevelConfiguration;
        public static string textLevelConfiguration;

        public static bool SocketBlock;

        private static string path;
        public static string pathLevelConfiguration;

        //Money Bets
        public static float BetsTime;

        public static List<int> PrizeMultipliers;

        public static void LoadXML()
        {
#if UNITY_WEBGL
			TextAsset resourceFile = (TextAsset)Resources.Load("Configuration File", typeof(TextAsset));
			textLevelConfiguration = resourceFile.text;
#else
            pathLevelConfiguration = GetDataPath("Configuration File.xml");
            textLevelConfiguration = System.IO.File.ReadAllText(pathLevelConfiguration, Encoding.Default);
#endif

            xmlLevelConfiguration = new XmlDocument();
            xmlLevelConfiguration.LoadXml(textLevelConfiguration.ToString());

            LoadMoneyXMLValues();
        }

        private static void LoadMoneyXMLValues()
        {
            UserName = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/UserName").InnerText;
            UserPass = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/UserPass").InnerText;
            SocketURL = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/SocketURL").InnerText;

            if (!GlobalObjects.IsWebGL)
            {
                UrlLogin = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/UrlLogin").InnerText;
                UrlGeneric = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/UrlGeneric").InnerText;

                GlobalObjects.gpUser = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/GameData/GPUser").InnerText;
                GlobalObjects.sysOperator = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/GameData/SysOperator").InnerText;
                GlobalObjects.player = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/GameData/Player").InnerText;
                GlobalObjects.token = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/GameData/Token").InnerText;
                GlobalObjects.game = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/GameData/Game").InnerText;
                GlobalObjects.miniGame = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/GameData/MiniGame").InnerText;
                GlobalObjects.TotalGamesToRestart =int.Parse(xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix+"/GamesToRestart").InnerText);
            }

            GlobalObjects.system = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/GameData/System").InnerText;
            RememberUser = bool.Parse(xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/RememberUser").InnerText);
            GenericTransactionError = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/GenericTexts/GenericTransactionError").InnerText;

            SocketBlock = bool.Parse(xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/SocketBlock").InnerText);

            ScreenshotsDirectory = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/ScreenshotsDirectory").InnerText;
            UserMessagesTitle = xmlLevelConfiguration.DocumentElement.SelectSingleNode(prefix + "/AlertsTitle").InnerText;

            UserMessages = new List<string>();
            XmlNode hints = xmlLevelConfiguration.DocumentElement.SelectSingleNode("UserAlerts");
            foreach (XmlNode node in hints.ChildNodes)
            {
                UserMessages.Add(node.InnerText);
            }
            HintsTime = float.Parse(xmlLevelConfiguration.DocumentElement.SelectSingleNode("AlertTime").InnerText);
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