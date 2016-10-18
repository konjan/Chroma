using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
	public PlayerValues Player;
   // public CollisionSoul soulAttack;
	public GameObject[] colliders;
    public TrailRenderer[] trails;
	public string[] PrimaryCombos;
	public string[] SecondaryCombos;
	public int PrimaryCount = 0;
	public int SecondaryCount = 0;

    public Vector3 DashTarget;

	private AnimatorStateInfo CurrentState;

	AnimatorData Anim;
    
	public float leewayTime = 4.5f;
    public float blockTime;

    // --- DASH VAR --- // 
    private float dashTime;
    private bool isDashing;

    // Use this for initialization
    void Start()
    {
        Player.SecondaryAttack = false;
        Player.PrimaryAttack = false;
        Player.isAttacking = false;

		Anim = GetComponentInChildren<AnimatorData>();

		trails = Anim.Trails;
		colliders = Anim.CollisionBoxes;

        for (int i = 0; i < colliders.Length; i++ )
			colliders[i].GetComponent<BoxCollider>().enabled = false;
   //     soulAttack.GetComponent<CollisionSoul>();
		foreach(TrailRenderer t in trails)
			t.gameObject.SetActive(false);

        dashTime = 0.6f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
		ActionUpdate();
	}

	void ActionUpdate()
	{
        //---- DASH CODE----//
        if (Input.GetButtonDown(Player.Joystick + "Dash") && !isDashing)
		{
			isDashing = true;
			Player.isStasis = true;

			if (Player.Targeted) // IF lockedOn
				DashTarget = Player.Opponent.transform.position;
			else
				DashTarget = transform.position + transform.forward * 50;
		}  
        if (isDashing && dashTime >= 0.0f)
            Dash();
		else if (dashTime <= 0.0f)
		{
			isDashing = false;
			Player.isStasis = false;
			dashTime = 0.6f;
		}




		//-----BLOCK CODE-----//
		if (Player.isGrounded && Input.GetButtonDown(Player.Joystick + "Block"))
        {      
            Player.isBlocking = true;
            Player.PlayerAnimation.SetBool("isBlocking", Player.isBlocking);
            Player.PlayerAnimation.SetTrigger("Block");
        }
		else if (Input.GetButtonUp(Player.Joystick + "Block"))
		{
            Player.isBlocking = false;
            Player.PlayerAnimation.SetBool("isBlocking", Player.isBlocking);
        }
		else if(!Player.isGrounded && Input.GetButtonDown(Player.Joystick + "Block"))
        {
			//Airdoge
		}

        //-----------------------//
        //-----ATTACK SCRIPT-----//
        //-----------------------//
        if (!Player.isBlocking)
        {
            
            //-----LIGHT ATTACK CODE-----//
            if (Input.GetButtonDown(Player.Joystick + "Primary") && Player.SecondaryAttack == false)// punch
            {
                //Setting Attack bool to true
                Player.isAttacking = true;
                Player.PrimaryAttack = true;
                CollidersOn();
                //Activating the animation trigger
                Player.PlayerAnimation.SetTrigger("PrimaryTrigger");
            }


            //-----HEAVY ATTACK CODE-----//
            if (Input.GetButtonDown(Player.Joystick + "Secondary") && Player.PrimaryAttack == false)// punch
            {
                //Setting Attack bool to true
                Player.isAttacking = true;
                Player.SecondaryAttack = true;
                CollidersOn();
                //Activating the animation trigger
                Player.PlayerAnimation.SetTrigger("SecondaryTrigger");
            }
            // --- MOAR DASH--- //
        }
	}

    public void CollidersOn()
    {
		Debug.Log("Colliders On");
		if (Player.PrimaryAttack)
		{
			colliders[0].GetComponent<BoxCollider>().enabled = true;
			colliders[0].tag = "PrimaryAttack";
			TrailsSwitch(true);
		}
		else if (Player.SecondaryAttack)
		{
			colliders[0].GetComponent<BoxCollider>().enabled = true;
			colliders[0].tag = "SecondaryAttack";
			TrailsSwitch(true);
		}
    }

    public void CollidersOff()
    {
		Player.SecondaryAttack = false;
		Player.PrimaryAttack = false;
		Player.isAttacking = false;

		Debug.Log("Colliders Off");

        colliders[0].GetComponent<BoxCollider>().enabled = false;
		TrailsSwitch(false);
    }

    //---DASH---//
    public void Dash()
    {
		dashTime -= Time.deltaTime;
		float speed = 8.0f;

		if (isDashing)
		{
			Player.transform.position = Vector3.MoveTowards(Player.transform.position, DashTarget, Time.deltaTime * speed);

			//--SLOWING DOWN THE PLAYER, FIXES A BUG--//
			float dist = Vector3.Distance(Player.transform.position, Player.Opponent.transform.position);
			if (dist <= 2.0f)
				speed = 5.0f;

			if (dist <= 0.1f)
				speed = 0.0f;
		}
	}

	public void TrailsSwitch(bool Active)
	{
		foreach (TrailRenderer t in trails)
			t.gameObject.SetActive(Active);
	}
}
