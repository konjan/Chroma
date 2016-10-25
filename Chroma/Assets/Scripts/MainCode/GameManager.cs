using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GameMode
{
	SinglePlayer,
	MultiPlayer,
	MultiPlayerConfined
};


public class GameManager : MonoBehaviour {

	private GameMode Mode = GameMode.MultiPlayer;

	//-----Camerastate and Splitscreen Variables-----//
	public Camera MainCamera;
	public Camera Player2Camera;

	public MenuData md;

	public Vector3[] PlayerSpawn;

	//-----Timer Variables-----//
	private float Timer;
	public Text TimeText;
	public float maxTime;

	//-----Canvas Variables-----//

	public Image KOText;
	public Text Win;
	public Slider[] PlayerSliders;
	public Slider[] SoulSliders;


	private CursorLockMode lockMode;
	// Use this for initialization
	void Start ()
	{
		lockMode = CursorLockMode.None;

		MainCamera = GameObject.Find("Camera Pivot").GetComponentInChildren<Camera>();
		Player2Camera = GameObject.Find("Camera 2 Pivot").GetComponentInChildren<Camera>();

		Timer = maxTime;

		Win.gameObject.SetActive(false);
	}

	void Update()
	{
		if (md == null)
		{
			md = GameObject.Find("MenuData").GetComponent<MenuData>();

			SceneSetup();
		}

		//-----Cast to int to get rounded timer
		//-----Cast to string to show on Canvas
		int timer = (int)Timer;
		TimeText.text = timer.ToString();

		//-----Reduce timer by deltaTime
		Timer -= Time.deltaTime;

		//-----Reset to 0 if it falls below
		if (Timer < 0)
			Timer = 0;

		//-----Swaps Cursor free on Keyboard
		if (Input.GetKeyDown(KeyCode.Z))
		{
			SwitchCursorMode();

			Cursor.lockState = lockMode;
		}

		//----Swaps Player mode -- Also changes screen view if necessary-----//
		if (Input.GetKeyDown(KeyCode.P))
		{
			if (Mode == GameMode.SinglePlayer)
				Mode = GameMode.MultiPlayer;
			else
				Mode = GameMode.SinglePlayer;
		}

		SplitScreen();
	}

	void SwitchCursorMode()
	{
		switch (Cursor.lockState)
		{
			case CursorLockMode.Locked:
				lockMode = CursorLockMode.None;
				Cursor.visible = true;
				break;
			case CursorLockMode.None:
				lockMode = CursorLockMode.Locked;
				Cursor.visible = false;
				break;
		}
	}

	private void SplitScreen()
	{
		if(Mode == GameMode.SinglePlayer || Mode == GameMode.MultiPlayerConfined)
		{
			//Resets Player screens for single Player
			MainCamera.rect = new Rect(0, 0, 1, 1);
		}
		else if(Mode == GameMode.MultiPlayer)
		{
			//Updates Player screens to show 2 Players
			MainCamera.rect = new Rect(0, 0, 0.5f, 1);
			Player2Camera.rect = new Rect(0.5f, 0, 0.5f, 1);
		}
	}

	private void SceneSetup()
	{
		for (int i = 1; i <= 2; i++)
		{
			GameObject PlayerObject = Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
			PlayerObject.name = "Player " + i;
			PlayerObject.transform.position = PlayerSpawn[i - 1];

			GameObject Character = Instantiate(Resources.Load(md.Players[i - 1].name, typeof(GameObject))) as GameObject;
			Character.transform.parent = PlayerObject.transform;
			Character.transform.localPosition = new Vector3(0, -1, 0);

			PlayerValues temp = PlayerObject.GetComponent<PlayerValues>();
			temp.GMInt = i - 1;
			GameObject.Find(PlayerObject.name + " Icon").GetComponent<Image>().sprite = Character.GetComponent<AnimatorData>().HUDIcon;
		}
	}
}
