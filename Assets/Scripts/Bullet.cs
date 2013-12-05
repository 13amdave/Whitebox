/*Change Log
Nov 21 11:28am : Changed movement to be more fluid*/

using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public Vector3 TargetPos;
	public string TeamName;
	public float Damage;
	public static bool first = true;
	private bool notFirst = true;
	
	void OnDisable ()
	{
		//transform.localScale = new Vector3 (1f,1f,1f);
	}
	
	void Start ()
	{
		renderer.material.SetColor ("_Color",Color.red);
		if (first)
		{
			transform.position = new Vector3 (1000f,1000f,1000f);
			notFirst = false;
			first = false;
		}
	}
	
	void FixedUpdate ()
	{
		if (notFirst)
		{
			Moving (TargetPos);
			if (Vector3.Distance (transform.position, TargetPos) < 0.5f)
			{
				PathingCentre.Explosion (transform.position, TeamName, Damage);
				Destroy (gameObject);
			}
		}
	}
	
	public float getSpeed (int position, Vector3 target)
	{
		float distance = transform.position[position] - target[position];
		if (Mathf.Abs (distance) > 10)
		{
			if (target[position] > transform.position[position])
			{
				return 1f+Random.Range (-0.5f,1f);
			}
			else if (target[position] < transform.position[position])
			{
				return -(1f+Random.Range (-0.5f,1f));
			}
		}
		else
		{
			distance = Mathf.Abs (distance);
			if (target[position] > transform.position[position])
			{
				return 1f-0.5f*Mathf.Sin(distance*Mathf.PI/20);
			}
			else if (target[position] < transform.position[position])
			{
				return -(1f-0.5f*Mathf.Sin(distance*Mathf.PI/20));
			}
		}
		return 0;
	}
	
	void Moving (Vector3 target)
	{
		
		Vector3 Movement = new Vector3 (0f,0f,0f);
		Movement[0] = getSpeed (0, target);
		Movement[1] = getSpeed (1, target);
		//Movement[2] = getSpeed (2, target);
		rigidbody.velocity = (Movement*5);//, ForceMode.Force);
	}
}
