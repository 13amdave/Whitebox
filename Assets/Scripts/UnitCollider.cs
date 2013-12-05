using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitCollider : MonoBehaviour {
	private List<Enemy> enemy = new List<Enemy>();
	public Enemy enemyHolder;
	public Rigidbody Unit;
	private string enemyTeam;
	Unit unitScript;
	// Use this for initialization
	void Start () 
	{
		unitScript = Unit.GetComponent <Unit> ();			
	}
	
	void OnDisable ()
	{
		enemy.Clear ();
	}
	
	void Update ()
	{
		if (! unitScript.CurrentTask.Equals ("Fighting"))
			if (checkEmpties ())
			{
				enemy.Sort ();
				enemyHolder = enemy[0];
				unitScript.targetScript = enemyHolder.enemyTransform.GetComponent <Unit> ();
				unitScript.EnemyTarget = enemyHolder.enemyTransform;
				unitScript.CurrentTask = "Fighting";
			}
	}
	
	bool checkEmpties ()
	{
		int enemyCount = enemy.Count;
		Enemy E;
		if (enemyCount == 0)
			return false;
		for (int count = 0; count < enemy.Count; count ++)
		{
			E = enemy[count];
			try
			{
				if (Vector3.Distance (unitScript.transform.position, E.enemyTransform.position) > unitScript.aggroRange)
				{
					enemy.RemoveAt (count);
					break;
				}
				else if (Vector3.Distance (unitScript.transform.position, E.enemyTransform.position) <= unitScript.attackRange+E.enemyTransform.localScale.x / 2)
				{
					unitScript.targetScript = E.enemyTransform.GetComponent <Unit> ();
					unitScript.EnemyTarget = E.enemyTransform;
					unitScript.CurrentTask = "Fighting";
					return false;
				}
			}
			catch
			{
				enemy.RemoveAt (count);
				count--;
			}
		}
		if (enemy.Count > 0)
			return true;
		return false;
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag.Equals (unitScript.EnemyTeamName) || other.gameObject.tag.Equals ("Neutral"))
		{
			enemy.Add (new Enemy (unitScript.transform, other.transform));
			if (! unitScript.CurrentTask.Equals ("Fighting"))
			{
				unitScript.targetScript = other.GetComponent <Unit> ();
				unitScript.EnemyTarget = other.transform;
				unitScript.CurrentTask = "Fighting";
			}
		}
	}
}
