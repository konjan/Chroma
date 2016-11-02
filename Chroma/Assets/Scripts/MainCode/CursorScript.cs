using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour {

	public JoystickNum Joystick = JoystickNum.Keyboard;

	public bool isSelected = false;
	private MenuData md;

	private PlayerSelection CharacterObject;


	public int PlayNum = 0;
	private bool namefound = false;
    
    // Use this for initialization
    void Start ()
	{
		CharacterObject = GameObject.Find(this.name + "Objects").GetComponent<PlayerSelection>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (md == null)
		{
			md = GameObject.Find("MenuData").GetComponent<MenuData>();

			if (PlayNum == 1)
				md.cs1 = this;
			else
				md.cs2 = this;
			
		}

		ControlCheck();
    }

    private void RaycastCheck()
	{
        //Ray ray = Camera.main.ScreenPointToRay(this.gameObject.transform.position);
        RaycastHit hit;
		if (Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out hit))
		{
			string characterName = hit.collider.gameObject.name + "_Ultimate_Tpose";


			do
			{
				//Finds a random character for the player to play as
				if(characterName == "Random_Ultimate_Tpose")
				{
					int r = (int)Random.Range(0, md.CharacterList.Length);

					md.Players[PlayNum - 1] = md.CharacterList[r];

					namefound = true;
				}
				//Searches for the character name and assigns it to the player object
				for(int i = 0; i < md.CharacterList.Length; i++)
				{
                    //if the object shares the name of one of the characters, assign it to the object, then exit loop.
                    if (md.CharacterList[i].name == characterName)
                    {
						md.Players[PlayNum - 1] = md.CharacterList[i];
						CharacterObject.TurnOn(i);

						namefound = true;
					}
				}
			} while (namefound == false);

			isSelected = true;
		}
	}
	 
	private void ControlCheck()
	{
		if (Input.GetButtonDown(Joystick + "Jump") && !isSelected)
			RaycastCheck();
		if (Input.GetButtonDown(Joystick + "Primary") && isSelected)
		{
			md.Players[PlayNum - 1] = null;
			CharacterObject.AllOff();
            isSelected = false;
			namefound = false;
		}

		if (!isSelected)
			transform.position = new Vector3(transform.position.x + Input.GetAxis(Joystick + "Horizontal") * 0.1f,	transform.position.y - Input.GetAxis(Joystick + "Vertical") * 0.1f, transform.position.z);
	}
}