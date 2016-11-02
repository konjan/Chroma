using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	private PlayerValues Player;
    private PlayerValues Target;
    private float m_speed;
    public float ProjectileDamage = 8;

	public float timer = 5.0f;

	public bool active = true;

	public SphereCollider sc;
	public ParticleSystem Impact;
	public ParticleSystem Orb;
    // Use this for initialization
    void Start ()
    {
		sc = this.GetComponent<SphereCollider>();
        m_speed = 2.5f;
    }

	// Update is called once per frame
	void FixedUpdate()
	{
		if (Impact.isPlaying == true)
			active = false;
		else if (active == false && Impact.isPlaying == false)
			this.gameObject.SetActive(false);

		this.transform.position = Vector3.Lerp(this.transform.position, Target.transform.position, Time.deltaTime * m_speed);
		timer -= Time.deltaTime;
		if(timer <= 0)
		{
			timer = 0;
			this.gameObject.SetActive(false);
		}
	}
    void OnCollisionEnter(Collision col)
    {
		Orb.Stop();
		Impact.Play();

		if (col.gameObject == Target.gameObject)
        {
			Target.m_Health -= ProjectileDamage;

			sc.enabled = false;
		}
		else
		{
			this.gameObject.SetActive(false);
		}
    }

	public void Startup(PlayerValues p, PlayerValues t)
	{
		active = true;
		timer = 5.0f;

		Player = p;
		Target = p;

		transform.position = Player.transform.position;
		sc.enabled = true;
	}
}
