using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public Vector3 moveTo;

	public float speed;
	public float spin;
	public float deathDelay;

	public AudioSource deathSound;

	private MobileUnit target;
	private int targetDamage;
	private bool dead = false;

    // Start is called before the first frame update
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
        if (spin == 0) {
        	//transform.LookAt(moveTo);
        	transform.up = moveTo - transform.position;
        	transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        }
        else {
        	transform.eulerAngles += new Vector3(0, 0, spin * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);

        if (transform.position == moveTo) {
        	Die();
        }
    }

    public void Launch (Vector3 position) {
    	moveTo = new Vector3(position.x, position.y, 0);
    }

    public void Launch (MobileUnit target, int damage) {
    	moveTo = new Vector3(target.transform.position.x, target.transform.position.y, 0);

    	this.target = target;
    	targetDamage = damage;
    }

    void Die () {
    	if (dead) {
    		return;
    	}

    	if (deathSound != null) {
    		deathSound.Play();
    	}

    	if (target != null) {
    		target.TakeDamage(targetDamage);
    	}

    	dead = true;
    	Destroy(gameObject, deathDelay);
    }
}