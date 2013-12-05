using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	
	public string EnemyTeamName;
	public float Damage;
	public static bool first = true;
	private bool notFirst = true;
	private int count = 0;
	private float randomScale;
	Unit unitScript;
	
	void OnDisable ()
	{
		transform.localScale = new Vector3 (1f,1f,1f);
	}
	
	void Start ()
	{
		renderer.material.SetColor ("_Color",Color.red);
		if (! first)
		{
			randomScale = 0.15f;
			Destroy (gameObject, 0.4f);
		}
		else
		{
			transform.position = new Vector3 (1000f,1000f,1000f);
			notFirst = false;
			first = false;
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Equals (EnemyTeamName))
			{
			try
			{
				unitScript = other.GetComponent <Unit> ();
				unitScript.health -= Damage;
				unitScript.checkHealth (Damage);
			}
			catch{}
			}
	}
	
	void FixedUpdate ()
	{
		if (notFirst)
		{
			count++;
			transform.localScale += new Vector3(randomScale,randomScale,randomScale);
			renderer.material.SetColor ("_Color",new Vector4 (1f-count*0.025f,0.0f,0.0f,1f));
		}
	}
}
