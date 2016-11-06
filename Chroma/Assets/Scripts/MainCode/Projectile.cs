using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public PlayerValues Player;
    public PlayerValues Target;
    private float m_speed;
    public float ProjectileDamage = 8;

	public float timer = 5.0f;

	public bool active = true;

	public SphereCollider sc;
	public ParticleSystem Impact;
	public GameObject Orb;
	private bool isTargeted;
    // Use this for initialization
    void Start ()
    {
		sc = this.GetComponent<SphereCollider>();
        m_speed = 5.0f;
    }

	// Update is called once per frame
	void FixedUpdate()
	{
		if (Impact.isPlaying == true)
			active = false;
		else if (active == false && Impact.isPlaying == false)
			ProjectileExplode();

		//if (isTargeted)
		//	this.transform.position = Vector3.MoveTowards(this.transform.position, Target.transform.position, Time.deltaTime * m_speed);
		//else
		transform.position += transform.forward * (Time.deltaTime * m_speed);

		timer -= Time.deltaTime;
		if(timer <= 0)
		{
			timer = 0;
			ProjectileExplode();
        }
	}
    void OnCollisionEnter(Collision col)
    {
		Orb.SetActive(false);
		Impact.Play(true);

		if (col.gameObject == Target.gameObject)
        {
			sc.enabled = false;

			Target.m_Health -= ProjectileDamage;
		}
		else
		{
			sc.enabled = false;
		}
    }

	public void Startup(PlayerValues p, PlayerValues t)
	{
		active = true;
		Orb.SetActive(true);
		timer = 5.0f;

		Player = p;
		Target = t;
		isTargeted = p.Targeted;

		transform.position = transform.position + new Vector3(0, 0.5f, 0);
		sc.enabled = true;
	}

	private void ProjectileExplode()
	{
		Player.ProjectileActive = false;

		GameObject.Destroy(this.gameObject);
	}
}
