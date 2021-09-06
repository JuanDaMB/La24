using UnityEngine;

public class ShowCursor : MonoBehaviour {

	private bool showCursor;
	[SerializeField] private Texture2D deffaultSprite;

	private CursorMode cursorMode;
	private Vector2 cursorHotspot;

	private void Start()
	{
		showCursor = false;
		Cursor.visible = showCursor;
		cursorMode = CursorMode.Auto;
		cursorHotspot = Vector2.zero;

		Cursor.SetCursor(deffaultSprite, cursorHotspot, cursorMode);
	}

	private void Update()
	{	
		if(Input.GetKeyDown(KeyCode.M))
		{
			showCursor = !showCursor;
			Cursor.visible = showCursor;
		}
	}

}
