using UnityEngine;
using System.Collections;
// walk, run && jump
public enum JoystickNum
{
		Joystick1,
		Joystick2,
		Joystick3,
		Joystick4,
		Keyboard, 
        AI
};

public class Movement : MonoBehaviour
{
	public PlayerValues Player;

	public Camera cam;

	public AnimationCurve speedCurve;
	public float accerateTime = 1.0f;

	private float speedModifier = 0.1f;
	private Vector3 previousDirection = Vector3.zero;
	private float currentAccelerateTime = 0.0f;
    private bool isMoving;
	private Quaternion desiredDirection;
	float idleSwitch = 0;
    public float fallSpeed;
    // Use this for initialization


    private Quaternion previousPlayerCameraDirection = Quaternion.identity;
    private float controlLockTimer;
    private bool controlLockTimerActive = false;

    public float DefaultControlLockTime = 0.05f;

    void Start()
	{
		if (this.name == "Player 1")
		{
			Player.Joystick = JoystickNum.Joystick1;

			cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		}
		else if (this.name == "Player 2")
		{
			Player.Joystick = JoystickNum.Joystick2;

			cam = GameObject.Find("Camera").GetComponent<Camera>();
		}

		Player.isGrounded = true;
        isMoving = false;
        //jumpAmount = 0;
        fallSpeed = -20.0f;
    }

	// Update is called once per frame
	void Update()
	{
		// use input manager
		IdleSwitch();
	}

	void FixedUpdate()
	{
		if (!Player.isStasis)
		{
			MoveCode();

			if (Input.GetButtonDown(Player.Joystick + "Jump"))
			{
                //Double Jump code
                /*
				if (!Player.isGrounded && Player.DoubleJump && !Player.isAttacking)
				{
					//Update to only activate on Normal Jump loop

					Player.DoubleJump = false;
					Player.PlayerAnimation.SetTrigger("DoubleJump");

					Player.rb.velocity = new Vector3(Player.rb.velocity.x, 0, Player.rb.velocity.z);
					Player.rb.AddForce(0, 8, 0, ForceMode.Impulse);
				}
				*/
                if (Player.isGrounded == true)
                {
                    Player.rb.AddForce(0, 8, 0, ForceMode.Impulse);

                    Player.isGrounded = false;
                    Player.PlayerAnimation.SetBool("isGrounded", Player.isGrounded);
                    Player.PlayerAnimation.SetTrigger("JumpPressed");
                }

             
			}
		}
		else
			transform.position = transform.position;
    }

	void MoveCode()
	{
		//-----Animation Code-----//
		if ((Input.GetAxis(Player.Joystick + "Vertical") < 0 || Input.GetAxis(Player.Joystick + "Vertical") > 0 || Input.GetAxis(Player.Joystick + "Horizontal") < 0 || Input.GetAxis(Player.Joystick + "Horizontal") > 0))
			isMoving = true;
        else
			isMoving = false;

        if (Player.isBlocking)
            isMoving = false;

		if (Player.Targeted)
		{
			Player.PlayerAnimation.SetFloat("MovingX", Input.GetAxis(Player.Joystick + "Horizontal"));
			Player.PlayerAnimation.SetFloat("MovingZ", -Input.GetAxis(Player.Joystick + "Vertical"));
		}
		else
		{
			Player.PlayerAnimation.SetFloat("MovingX", Mathf.Abs(Input.GetAxis(Player.Joystick + "Horizontal")));
			Player.PlayerAnimation.SetFloat("MovingZ", Mathf.Abs(Input.GetAxis(Player.Joystick + "Vertical")));
		}

		Player.PlayerAnimation.SetBool("isMoving", isMoving);

		//-----FREE MOVEMENT CODE-----//

		Vector3 cameraForward = cam.transform.forward;
		cameraForward.y = 0.0f; cameraForward.Normalize();

		Vector3 inputDirection = new Vector3(Input.GetAxis(Player.Joystick + "Horizontal"), 0, -Input.GetAxis(Player.Joystick + "Vertical"));

		if (inputDirection.magnitude > 0.1f)
		{
			inputDirection.Normalize();
			desiredDirection = Quaternion.LookRotation(inputDirection, Vector3.up);

			Quaternion CameraDirection = Quaternion.LookRotation(cameraForward, Vector3.up);
            Quaternion controlRotationModifier = CameraDirection;

            if(controlLockTimerActive)
            {
                Debug.Log("hit");
                controlRotationModifier = previousPlayerCameraDirection;
                controlLockTimer -= Time.deltaTime;
                if (controlLockTimer < 0.0f)
                {
                    controlLockTimerActive = false;
                    previousPlayerCameraDirection = Quaternion.identity;

                }
            }
            else
            {
                if(previousPlayerCameraDirection == Quaternion.identity)
                {
                    previousPlayerCameraDirection = CameraDirection;
                }

                float fDot = Quaternion.Dot(previousPlayerCameraDirection, CameraDirection);
                if(fDot < 0.8f)
                {
                    controlLockTimerActive = true;
                    controlLockTimer = DefaultControlLockTime;
                    controlRotationModifier = previousPlayerCameraDirection;
                }
                else
                {
                    previousPlayerCameraDirection = CameraDirection;
                }
            }

            desiredDirection = controlRotationModifier * desiredDirection;


			Vector3 forwardOffset = controlRotationModifier * new Vector3(0, 0, 1) * -Input.GetAxis(Player.Joystick + "Vertical") * Player.m_Speed * Time.deltaTime;
			Vector3 rightOffset = controlRotationModifier * new Vector3(-1, 0, 0) * -Input.GetAxis(Player.Joystick + "Horizontal") * Player.m_Speed * Time.deltaTime;

			//-----Move Player in the direction of the joystick input
			Vector3 moveDir = forwardOffset + rightOffset;
			moveDir.Normalize();

			float dot = Vector3.Dot(moveDir, previousDirection);
			previousDirection = moveDir;

			if(dot > 0.7f)
			{
				currentAccelerateTime += Time.deltaTime;
				float alpha = currentAccelerateTime / accerateTime;

				speedModifier = speedCurve.Evaluate(alpha);
			}
			else
			{
				currentAccelerateTime = 0.0f;
			}

			Player.rb.MovePosition(Player.rb.transform.position + (forwardOffset + rightOffset) * speedModifier);

			//-----Rotate Player towards desired Joystick direction
			Player.rb.transform.rotation = Quaternion.RotateTowards(Player.rb.transform.rotation, desiredDirection, Player.TurnSpeed * Time.deltaTime);
		}

		//-----Force Player to look at opponent if Targeted
		if (Player.Targeted)
			Player.rb.transform.LookAt(new Vector3(Player.Opponent.transform.position.x, transform.position.y, Player.Opponent.transform.position.z));

		if (Physics.Raycast(transform.position, Vector3.down, 2) == false && Player.isGrounded == true)
		{
			Player.isGrounded = false;
			Player.PlayerAnimation.SetBool("isGrounded", false);

			Player.PlayerAnimation.SetTrigger("FallTrigger");
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Ground")
			Player.PlayerAnimation.SetBool("isGrounded", true);
	}
	//-----Idle ear twitch. Fully completed.
	void IdleSwitch()
	{
		if(isMoving == false)
		{
			if (idleSwitch > 0)
				idleSwitch -= Time.deltaTime;
			else if (idleSwitch <= 0)
			{
				idleSwitch = Random.Range(3.0f, 8.0f);
				Player.PlayerAnimation.SetTrigger("IdleSwitch");

			}
		}
	}
};
