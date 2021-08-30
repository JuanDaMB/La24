using System;
using System.Diagnostics;
using UnityEngine;

public class LoadLauncher : MonoBehaviour
{

	public void OnLoadLauncher()
	{
		Application.Quit();
		/*process = new Process();
		try
		{
			process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.FileName = XMLLauncherManager.LauncherPath;
			process.StartInfo.Arguments = "/c" + XMLLauncherManager.LauncherPath;
			process.EnableRaisingEvents = true;
			process.Start();
			Application.Quit();
		}
		catch (Exception e)
		{
			print(e);
		}*/
	}
}