using UnityEngine;
using System.Collections;

public class WeaponNoise : MonoBehaviour
{
    public AudioSource wepSource;
    private AudioClip wepNoise;

    public AudioSource WhooshSource;
    private AudioClip whooshNoise;

    private PlayerValues player;
    private bool isColliding;

	// Use this for initialization
	void Start ()
    {
        isColliding = false;
        wepNoise = wepSource.clip;

        whooshNoise = WhooshSource.clip;

        player = GetComponent<PlayerValues>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //--IF MISS PLAY WHOOSH-- there's a slight delay --
        if (!isColliding && player.PrimaryAttack)
            WhooshSource.Play();

        if (!isColliding && player.SecondaryAttack)
            WhooshSource.Play();

        if (wepSource.isPlaying)
            WhooshSource.Stop();
    }

    void OnTriggerEnter(Collider col)
    {

        if (player.PrimaryAttack && col.gameObject == player.Opponent)
        {
            wepSource.PlayOneShot(wepNoise, 0.5f);
            isColliding = true;
        }
        else isColliding = false;


        if (player.SecondaryAttack && col.gameObject == player.Opponent)
        {
            wepSource.PlayOneShot(wepNoise, 0.5f);
            isColliding = true;
        }
        else isColliding = false;


    }
}
