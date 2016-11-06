using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public enum ElemTrait
{
    FIRE,
    WATER,
    EARTH,
    AIR,
    SOUL,
    UNASPECTED
}

public class PlayerValues : MonoBehaviour {

    //-----CONST VARIABLES-----//
	private const float MAX_HEALTH = 200;

	public Rigidbody rb;
   
    //-----Status Variables, Damage to be moved to upcoming "Move Struct" in "Attack.cs"
    public float m_Health;

    public ElemTrait Attribute = ElemTrait.UNASPECTED;

	private bool isStunned = false;
    public bool isStasis = false;
	[HideInInspector]
	public bool isBlocking = false;
	[HideInInspector]
    public float m_damage = 5.0f;
	//Opponent Variable
	public PlayerValues Opponent;

    //-----SOUL VARIABLES-----//
	[HideInInspector]
    public float m_soulAmount = 0.0f;
    private float timeThing = 5.0f;
	//-----CONTROLLER VARIABLES-----//

	public JoystickNum Joystick = JoystickNum.Keyboard;
	[HideInInspector]
	public bool Targeted = false;

    //----------MOVEMENT VARIABLES-----//

	//-----Variable used in jump, is turned off if above 3u above the ground, or if Jump is pressed
    public bool isGrounded;
    //-----DoubleJump variable used in determining how many jumps have been used up
    //-----True if Player HAS NOT used up their double jump
    //public bool DoubleJump = true;

    //-----Speed variable, 5 roughly matches the speed at which the animations play
    //-----Characters skate if variable is higher
    [HideInInspector]
    public float m_Speed = 5;

    //-----Access to the animator, used for all animations
	public Animator PlayerAnimation = new Animator();

    //----------ATTACK VARIABLES-----//


    //-----Attack variables used in Attack.cs, stored here for cleanliness
	[HideInInspector]
    public bool isAttacking = false;
	[HideInInspector]
    public bool PrimaryAttack = false;
	[HideInInspector]
	public bool SecondaryAttack = false;

	public bool ProjectileActive;

	[HideInInspector]
	public bool Hit = false;

    //----------EXTRA VARIABLES-----//
	//-----Gravity Variables
	//____Might be unneccessary_____//
	private float gravityEdit = 1;

    //-----Turn Speed. Increase to hasten the rate at which a character turns on the spot. default is 780
	[HideInInspector]
    public float TurnSpeed = 780.0f;

	//-----UI pieces

	public GameManager GM;
	public int GMInt;
    public bool isKO;
    public ParticleSystem Fire;
    public ParticleSystem Earth;
    public ParticleSystem Air;
	public ParticleSystem Water;
	public ParticleSystem Soul;
    public ParticleSystem pow;
    private float powActive = 0.2f;
	private float changescene = 5;

	private float SoulTime = 0.5f;
	private bool SoulRaise = false;

    //-----Stun Timer
    private float staticTime = 0.0f;

    // Use this for initialization
    void Start ()
    {
		m_Health = MAX_HEALTH;

		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        GM.SoulSliders[GMInt].maxValue = 100;
        pow.gameObject.SetActive(false);
	}

	void Update()
	{
		if(PlayerAnimation == null)
			PlayerAnimation = GetComponentInChildren<Animator>();
		if(Opponent == null)
		{
			if(this.name == "Player 1")
				Opponent = GameObject.Find("Player 2").GetComponent<PlayerValues>();
			else if(this.name == "Player 2")
				Opponent = GameObject.Find("Player 1").GetComponent<PlayerValues>();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        rb.AddForce(Physics.gravity * gravityEdit * rb.mass);

		GM.PlayerSliders[GMInt].value = m_Health;
		GM.SoulSliders[GMInt].value = m_soulAmount;

		//Check(Attribute, Opponent.GetComponent<PlayerValues>().Attribute);

		if (m_Health <= 0)
        {
            GM.KOText.gameObject.SetActive(true);
			GM.Win.text = tag.ToString();
			GM.Win.gameObject.SetActive(true);

			changescene -= Time.deltaTime;

			if(changescene <= 0)
			{
				Application.LoadLevel("Main Menu");
			}
        }

		if (SoulRaise == true)
		{
			SoulTime -= Time.deltaTime;
			Opponent.m_soulAmount += 25.0f * Time.deltaTime;
			if (SoulTime < 0)
			{
				SoulRaise = false;
				SoulTime = 0.5f;
			}
		}


        if (m_soulAmount > 33.3f)
        {
            ChangeElement();

			if (m_soulAmount > 100)
			{
				m_soulAmount = 100;
			}
		}

        if (Attribute != ElemTrait.UNASPECTED)
        {
            timeThing -= Time.deltaTime;        
        }

        if (timeThing <= 0.0f)
        {
            Attribute = ElemTrait.UNASPECTED;
            Fire.gameObject.SetActive(false);
            Earth.gameObject.SetActive(false);
            Air.gameObject.SetActive(false);
            Water.gameObject.SetActive(false);
            timeThing = 5.0f;
        }

        if (isStunned)
        {
            staticTime -= Time.deltaTime;
            if (staticTime <= 0.0f)
                isStunned = false;
        }

       // pow.Play();

        if (pow.gameObject.activeSelf == true)
        {
            pow.Play();
            powActive -= Time.deltaTime;
            if (powActive < 0)
            {
                pow.gameObject.SetActive(false);
                powActive = 0.2f;
            }
        }
	}

	void OnTriggerEnter(Collider col)
	{
		if (!isBlocking && !Hit)
		{
            if (col.gameObject.tag == "PrimaryAttack")
            {
				m_Health -= Opponent.m_damage;
				isAttacking = true;
                PlayerAnimation.SetTrigger("TempHit");
                pow.gameObject.SetActive(true);
                pow.transform.position = col.transform.position;			
                //pow.Play();
				SoulRaise = true;
			}
            else if (col.gameObject.tag == "SecondaryAttack")
            {
				m_Health -= Opponent.m_damage;
				isAttacking = true;
                PlayerAnimation.SetTrigger("TempHit");
                pow.gameObject.SetActive(true);
                pow.transform.position = col.transform.position;				
               // pow.Play();
				SoulRaise = true;
			}

		}
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            PlayerAnimation.SetBool("isGrounded", true);
            isGrounded = true;
        }
        if (col.gameObject.tag == "projectile")
            m_Health -= col.gameObject.GetComponent<Projectile>().ProjectileDamage * Opponent.m_damage;
    }


   ElemTrait Strength(ElemTrait elem)
    {
        return elem;
    }

    public void MakePlayerStunned(float time)
    {
        isStunned = true;
        staticTime = time;
    }

	private void ChangeElement()
	{
        if (Input.GetAxis(Joystick + "DpadVertical") > 0 && Attribute != ElemTrait.FIRE) // FIRE
        {
            ResetElement();
            Debug.Log("Element Change; Fire");
            Attribute = ElemTrait.FIRE;
            Fire.gameObject.SetActive(true);
        }
        if (Input.GetAxis(Joystick + "DpadHorizontal") > 0 && Attribute != ElemTrait.EARTH) // EARTH
        {
            ResetElement();
            Debug.Log("Element Change; Earth");
            Attribute = ElemTrait.EARTH;
            Earth.gameObject.SetActive(true);
        }
        if (Input.GetAxis(Joystick + "DpadVertical") < 0 && Attribute != ElemTrait.AIR) //AIR
        {
            ResetElement();
            Debug.Log("Element Change; Air");
            Attribute = ElemTrait.AIR;
            Air.gameObject.SetActive(true);
        }
        if (Input.GetAxis(Joystick + "DpadHorizontal") < 0 && Attribute != ElemTrait.WATER) // WATER
        {
            ResetElement();
            Debug.Log("Element Change; Water");
            Attribute = ElemTrait.WATER;
            Water.gameObject.SetActive(true);
        }
	}
    private void ResetElement()
    {
        Fire.gameObject.SetActive(false);
        Earth.gameObject.SetActive(false);
        Air.gameObject.SetActive(false);
        Water.gameObject.SetActive(false);
        m_soulAmount -= 33.3f;

    }

	// ELEMENT STUFF
	public void Check(ElemTrait Trait1, ElemTrait Trait2)
	{
		//-----Water > Fire > Earth > Air > Water-----//
		if (Trait1 == ElemTrait.FIRE)
		{
			if (Trait2 == ElemTrait.WATER)
				m_damage = 2.5f;
			else if (Trait2 == ElemTrait.EARTH)
				m_damage = 10.0f;
			else
				m_damage = 5.0f;
		}
		else if (Trait1 == ElemTrait.EARTH)
		{
			if (Trait2 == ElemTrait.FIRE)
				m_damage = 2.5f;
			else if (Trait2 == ElemTrait.AIR)
				m_damage = 10.0f;
			else
				m_damage = 5.0f;
		}
		else if (Trait1 == ElemTrait.AIR)
		{
			if (Trait2 == ElemTrait.EARTH)
				m_damage = 2.5f;
			else if (Trait2 == ElemTrait.WATER)
				m_damage = 10.0f;
			else
				m_damage = 5.0f;
		}
		else if (Trait1 == ElemTrait.WATER)
		{
			if (Trait2 == ElemTrait.AIR)
				m_damage = 2.5f;
			else if (Trait2 == ElemTrait.FIRE)
				m_damage = 10.0f;
			else
				m_damage = 5.0f;
		}
		else if (Trait1 == ElemTrait.UNASPECTED)
			m_damage = 5.0f;
		else if (Trait1 == ElemTrait.SOUL)
			m_damage = 7.5f;
	}
}
