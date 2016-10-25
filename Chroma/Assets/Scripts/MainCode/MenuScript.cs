using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
	// Use this for initialization
	void Start()
    {
		GameObject MenuDataObject = Instantiate(Resources.Load("MenuData", typeof(GameObject))) as GameObject;
		MenuDataObject.name = "MenuData";
    }

    // Update is called once per frame
    void Update()
    {
    }

	private void LoadLevel(string Level)
	{
        Application.LoadLevel(Level);
		//SceneManager.LoadScene(Level, LoadSceneMode.Single);
	}

	public void SinglePlayer(string Level)
	{
		//md.Players = new GameObject[1];

		Debug.Log("No SinglePlayer for you.");

		VSMatch(Level);
	}
	public void VSMatch(string Level)
	{
		//md.Players = new GameObject[2];

		LoadLevel(Level);
	}
	public void GameExit()
	{
		Application.Quit();
	}
}