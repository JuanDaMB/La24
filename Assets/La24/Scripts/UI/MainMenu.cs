using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIZone
{
	public override ZoneName Zone
	{
		get { return ZoneName.Main; }
	}

	[SerializeField]
	private Button btnStartGame;

	private void Start()
	{
		btnStartGame.onClick.AddListener(StartGameplay);
		StartGameplay();
	}

	private void StartGameplay()
	{
		UIController.Instance.MoveToZone(ZoneName.LoginMoney);
	}
}