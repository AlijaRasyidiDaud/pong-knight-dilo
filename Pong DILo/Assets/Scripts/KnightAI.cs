using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAI : MonoBehaviour {

	private Animator anim;
	[SerializeField] private Transform[] patrolPoint;
	[SerializeField]private int currentPatrolIndex;
	[SerializeField]private int prevPatrolIndex;
	[SerializeField]private bool isDelay = true;
	[SerializeField] private float delayTime = 8f;
	[SerializeField] private float speed = 5f;
	[SerializeField] private GameObject[] launchAttack;

	void Awake ()
	{
		anim = GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start ()
	{
		StartCoroutine (Delay());
		currentPatrolIndex = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (transform.position.x < patrolPoint[currentPatrolIndex].position.x && !isDelay)
		{
			anim.Play ("Knight_walk");
			transform.localScale = new Vector3 (5f, 5f, 1f);
			transform.position = Vector3.Lerp (transform.position, patrolPoint[currentPatrolIndex].position, speed * Time.deltaTime);
		}

		if (transform.position.x > patrolPoint[currentPatrolIndex].position.x && !isDelay)
		{
			anim.Play ("Knight_walk");
			transform.localScale = new Vector3 (-5f, 5f, 1f);
			transform.position = Vector3.Lerp (transform.position, patrolPoint[currentPatrolIndex].position, speed * Time.deltaTime);
		}

		if (Vector3.Distance (transform.position, patrolPoint[currentPatrolIndex].position) < .1f && !isDelay)
		{
			anim.Play ("Knight_idle");

			isDelay = true;

			if (currentPatrolIndex == 1)
			{
				if (prevPatrolIndex == 0)
				{
					currentPatrolIndex = 2;
				} else // prevPatrolIndexnya = 2
				{
					currentPatrolIndex = 0;
				}
			} else // Selain dari index 1 pasti menuju 1
			{
				prevPatrolIndex = currentPatrolIndex;
				currentPatrolIndex = 1;
			}

			Invoke ("LaunchAttack", 3f);
		}
	}

	private IEnumerator Delay ()
	{
		while (true)
		{
			if (isDelay)
			{
				yield return new WaitForSeconds(delayTime);
				isDelay = false;
			}

			yield return null;

		}
	}

	void LaunchAttack ()
	{
		anim.Play ("Knight_attack");
		int rd = Random.Range (0, 2);
		GameObject newLaunchAttack = Instantiate (launchAttack[rd], transform.position, transform.rotation);
		newLaunchAttack.GetComponent<Rigidbody2D> ().AddForce (new Vector2(Random.Range(-100f, 101f), Random.Range(50f, 101f)));
	}
}
