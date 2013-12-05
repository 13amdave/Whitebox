using UnityEngine;
using System.Collections;

public class GoodGuyMovement : MonoBehaviour 
{
	//Base Variables
	public float health;
	public float attackDamage;
	public float attackSpeed;
	public float attackRange;
	public float movementSpeed = 10f;
	
	public string attackType;
	public string movementType;
	
	public int Team;
	
	private string CurrentTask = "Setup";
	//Transforms
	public Transform target;
	public Vector3 home = new Vector3 (-50,0,-50);
	//Scripts
	public CheckPoint checkscript; 
	
	void OnEnable()
    {
		CurrentTask = "CheckPoint";
		//GetFirstDestination
    }
	
	void Start () {
		Debug.Log (Team);
		if (Team == 0)
			renderer.material.SetColor ("_Color",Color.red);
		else
		{
			renderer.material.SetColor ("_Color",Color.blue);
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
	switch (other.gameObject.tag)
		{
		case "CheckPoint":
			{
				if (target.position == other.transform.position)
				{	
					checkscript = other.GetComponent<CheckPoint>();
					target = checkscript.Paths[checkscript.TeamDestination[Team]];
					rigidbody.velocity = new Vector3(0, 10, 0);
				}
			break;
			}
		}
	}
	
	public float getSpeed (int position)
	{
		if (target.position[position] > transform.position[position]-0.05f)
		{
			return movementSpeed+Random.Range (-0.5f,20f);
		}
		else if (target.position[position] < transform.position[position]+0.05f)
		{
			return -(movementSpeed+Random.Range (-0.5f,20f));
		}
		return 0;
	}
	
	void Moving ()
	{
		Vector3 Movement = new Vector3 (0f,1f,0f);
		Movement[0] = getSpeed (0);
		Movement[2] = getSpeed (2);
		rigidbody.AddForce (Movement);//, ForceMode.Force);
	}
	
	void Update ()
	{
		if (CurrentTask.Equals ("CheckPoint"))
			Moving ();
		else if (CurrentTask.Equals ("Fighting"))
		{
		}
		else
		{
		}
	}
}
