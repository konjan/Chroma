using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public struct PlayerData
{
	public GameObject characterSelected;
	public int skinSelected;
}
public class MenuData : MonoBehaviour {

	public GameObject[] Players = new GameObject[2];
	public GameObject[] CharacterList = new GameObject[4];

	private bool allselected = false;

	public string levelSelected = "ForestTemple"; // farting goose

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (GameObject g in Players)
		{
			if (g != null)
			{
				allselected = false;

				break;
			}
			else
				allselected = true;
		}

		if (Input.GetButtonDown("Start") && allselected)
		{
			//load next scene
		}
    }

    public void OnClick()
    {
        SceneManager.LoadScene(levelSelected, LoadSceneMode.Single);
    }

}
