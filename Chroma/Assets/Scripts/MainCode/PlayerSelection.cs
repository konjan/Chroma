using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour {

	public GameObject[] CharacterList;

	[HideInInspector]
	public int Index = 0;

	public JoystickNum Joystick = JoystickNum.Keyboard;

	// Use this for initialization
	void Start ()
	{
		CharacterList = new GameObject[transform.childCount];


        for (int i = 0; i < transform.childCount; i++)
            CharacterList[i] = transform.GetChild(i).gameObject;

		//Set all to off
		AllOff();
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void AllOff()
	{
        foreach (GameObject g in CharacterList)
            g.SetActive(false);
	}

	public void TurnOn(int i)
	{
		CharacterList[i].SetActive(true);
	}
}
