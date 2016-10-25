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

	public CursorScript cs1;
	public CursorScript cs2;

	public string levelSelected = "ForestTemple"; // farting goose

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this);

		for(int i = 0; i < Players.Length; i++)
		{
			Players[i] = null;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("Start") && (cs1.isSelected && cs2.isSelected))
			Application.LoadLevel(levelSelected);
    }

    public void OnClick()
    {
        SceneManager.LoadScene(levelSelected, LoadSceneMode.Single);
    }

}
