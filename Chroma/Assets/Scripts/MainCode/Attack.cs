using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
	public PlayerValues Player;
	// public CollisionSoul soulAttack;


	public AnimatorData anim;
	public GameObject[] colliders;
    public TrailRenderer[] trails;

	private AnimatorStateInfo CurrentState;

	public float leewayTime = 4.5f;
    public float blockTime;
    // Use this for initialization
    void Start()
    {
        Player.SecondaryAttack = false;
        Player.PrimaryAttack = false;
        Player.isAttacking = false;

		anim = GetComponentInChildren<AnimatorData>();

		colliders = anim.CollisionBoxes;
		trails = anim.Trails;

        for (int i = 0; i < colliders.Length; i++ )
			colliders[i].GetComponent<BoxCollider>().enabled = false;
		for (int i = 0; i < trails.Length; i++)
			trails[i].gameObject.SetActive(false);
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
   


        }
        
	}

    public void CollidersOn()
    {
		Debug.Log("Colliders On");

		if (Player.PrimaryAttack)
			colliders[0].tag = "PrimaryAttack";
		else if (Player.SecondaryAttack)
			colliders[0].tag = "SecondaryAttack";

		colliders[0].GetComponent<BoxCollider>().enabled = true;
		foreach (TrailRenderer i in trails)
			i.gameObject.SetActive(true);
	}

    public void CollidersOff()
    {
		Player.SecondaryAttack = false;
		Player.PrimaryAttack = false;
		Player.isAttacking = false;

        colliders[0].GetComponent<BoxCollider>().enabled = false;
    }

	 public void TrailsOff()
	{
		foreach (TrailRenderer i in trails)
			i.gameObject.SetActive(false);
	}
}
