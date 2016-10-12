﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour {

	public JoystickNum Joystick = JoystickNum.Keyboard;

	public bool isSelected = false;
    public Button charButton1;
    public Button charButton2;

    // Use this for initialization
    void Start ()
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		ControlCheck();

        

    }

    private void RaycastCheck()
	{
        Ray ray = Camera.main.ScreenPointToRay(this.gameObject.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            Debug.Log("Talk shit get hit");
	}
	 
	private void ControlCheck()
	{
		if (Input.GetButtonDown(Joystick + "Jump") && !isSelected)
			isSelected = true;
		if (Input.GetButtonDown(Joystick + "Primary") && isSelected)
			isSelected = false;

		if (!isSelected)
			transform.position = new Vector3(transform.position.x + Input.GetAxis(Joystick + "Horizontal") * 0.1f,	transform.position.y - Input.GetAxis(Joystick + "Vertical") * 0.1f, transform.position.z);
	}
}
