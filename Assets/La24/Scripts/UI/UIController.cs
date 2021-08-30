using System.Collections.Generic;
using IndieLevelStudio.BoardModule.UX;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public static UIController Instance;

	[SerializeField]
	private List<UIZone> uiZones;

	[SerializeField]
	private Alert alert;

	[SerializeField]
	public Stadistics stadistics;

	[SerializeField]
	private Button btnOmit;

	private void Awake()
	{
		Instance = this;

		alert.Setup();
		stadistics.Setup();

		btnOmit.onClick.AddListener(OmitAnimation);
	}

	public void EnableButtonOmit(bool enable)
	{
		btnOmit.gameObject.SetActive(enable);
	}

	private void OmitAnimation()
	{
		EnableButtonOmit(false);
		GameplayManager.Instance.SkipBallCameraRotation();
	}

	public void MoveToZone(ZoneName newZone)
	{
		uiZones.ForEach(z =>
		{
			z.SetEnabled(z.Zone == newZone);
		});
	}


	public void ShowBetsPromotional(int bets)
	{
		MoveToZone(ZoneName.Bets);
	}

	public void ShowFinalScreen(List<Bet> bets, int ballSelected, NumberColor color)
	{
		FinalResult zone = (FinalResult)uiZones.Find(z => z.Zone == ZoneName.Results);
		zone.ShowResult(bets, ballSelected.ToString(), color);
	}

	public void DisableAll()
	{
		uiZones.ForEach(z => z.SetEnabled(false));
	}

	private void OnDestroy()
	{
		Instance = null;
	}
}