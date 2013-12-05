/*ChangeLog
 * Nov 21 11:44am : Added Arrow for ranged combat
 * */

using UnityEngine;
using System.Collections;

public class PathingCentre : MonoBehaviour 
{	
	public static int globalCount = 0;
	public static float Team0Count = 0;	// Needs changeing ********
	public static float Team0Cap = 100;
	public static float Team1Count = 0;
	public static float Team1Cap = 100;
	public int capacity = 1000;
	public string[] EnemyTeam; 				// Enemy team's <- As of right now only uses the first slot, just incase we use more than one team
	public string TeamTag;					// The team tag name 0 or 1
	public float SpawnTime;					// Time it takes to spawn a unit in seconds
	public int TeamNum;						// Same as team tag, but an integer rather than a string
	public int PathNumber = 0;				// Path to take leave as zero unless more than 1 path available
	public Rigidbody spawnerPrefab;			// Unit to be spawned
	public Transform[] Paths;				// First destination for unit, versitile incase of more than 1 path
	private bool waiting = false;
	Unit movementScript;
	
	void OnGUI()
    {
        if(GUI.Button(new Rect(Screen.width - 100, 5, 100, 30), "Create Ant"))
        {
			createBody (TeamNum);
        }
    }
	
	void Update ()
	{
		if (! waiting)
			spawn ();
	}
	
	void spawn ()
	{
		if (globalCount < 400 && capacity > 0)
		{
			capacity --;
			StartCoroutine ("Timer");
			if (TeamNum == 0)
			{
				if (Team0Count < Team0Cap)
				{
					createBody (TeamNum);
					globalCount++;
					Team0Count++;
				}
			}
			else if (Team1Count < Team1Cap)
				{
					createBody (TeamNum);
					globalCount++;
					Team1Count++;
				}
		}
	}
	
	void createBody (int team)
	{
		Rigidbody handler;
		//Debug.Log ("Created");
		handler = Instantiate (spawnerPrefab, getStartLocation (transform.position), transform.rotation) as Rigidbody;
		movementScript = handler.GetComponent <Unit> ();
		movementScript.Team = team;
		handler.gameObject.tag = TeamTag;
		movementScript.EnemyTeamName = EnemyTeam[0];
		//movementScript.CheckPointTarget = target[(int)(Random.value * 2)];
		movementScript.CheckPointTarget = Paths[PathNumber];
	}
	
	IEnumerator Timer ()
	{
		waiting = true;
		yield return new WaitForSeconds (SpawnTime);
		waiting = false;
	}
	
	public void switchPath ()
	{
		PathNumber++;
		if (PathNumber >= Paths.Length)
			PathNumber = 0;
	}
	
	public static Vector3 getStartLocation (Vector3 startPosition)
	{
		float x = startPosition[0]+3*Mathf.Cos(globalCount*(2*Mathf.PI/8));
		float y = startPosition[1]+5+3*Mathf.Cos(globalCount*(2*Mathf.PI/8));
		//float z = startPosition[2]+3*Mathf.Sin(globalCount*(2*Mathf.PI/8));
		Vector3 Location = new Vector3 (x,y,0.0f);
		return Location;
	}
	
// Global Combat
	
	public static void Explosion (Vector3 pos, string EnemyTeamName, float Damage)
	{
		GameObject Bomb = GameObject.FindWithTag("Bomb");
		Bomb = Instantiate(Bomb, pos, Bomb.transform.rotation) as GameObject;
		Bomb bombscript = Bomb.GetComponent <Bomb> ();
		bombscript.EnemyTeamName = EnemyTeamName;
		bombscript.Damage = Damage;
	}
	
	public static void Missile (Vector3 start, Vector3 finish, string EnemyTeamName, float Damage)
	{
		GameObject Bullet = GameObject.FindWithTag("Missile");
		Bullet = Instantiate(Bullet, new Vector3 (start.x, start.y,  -1.5f), Bullet.transform.rotation) as GameObject;
		Bullet missileScript = Bullet.GetComponent <Bullet> ();
		missileScript.TargetPos = new Vector3 (finish.x, finish.y, -1.5f);
		missileScript.TeamName = EnemyTeamName;
		missileScript.Damage = Damage;
	}

	public static void Arrow (Vector3 start, Transform target, Unit targetScript, float Damage)
	{
		GameObject arrow = GameObject.FindWithTag("Arrow");
		arrow = Instantiate(arrow, new Vector3 (start.x, start.y,  -1f), arrow.transform.rotation) as GameObject;
		Arrow arrowScript = arrow.GetComponent <Arrow> ();
		arrowScript.Target = target;
		arrowScript.targetScript = targetScript;
		arrowScript.Damage = Damage;
	}
	
	public static void AntiMissile (Vector3 start, Vector3 finish, string EnemyTeamName, float Damage)
	{
		GameObject Bullet = GameObject.FindWithTag("Missile");
		Bullet = Instantiate(Bullet, new Vector3 (start.x, start.y,  -4f), Bullet.transform.rotation) as GameObject;
		Bullet missileScript = Bullet.GetComponent <Bullet> ();
		missileScript.TargetPos = new Vector3 (finish.x, finish.y, -4f);
		missileScript.TeamName = EnemyTeamName;
		missileScript.Damage = Damage;
	}
}
