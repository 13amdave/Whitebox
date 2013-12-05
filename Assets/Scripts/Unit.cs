/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*\
 * Kevin Wyckoff Infectoids 2013
 * Unit Class deals closely with UnitCollider for combat. Pathing Centre
 * makes reference to Unit in order to setup, and vice versa for scoring
 * and population caps.
 *
 * Unit holds the individual unit stats and attack/movement types. Each 
 * unit type is just an adjustment of the base unit. 
 * 
 * Standard stats
 * health			: 
 * attackDamage		: How much damage each attack will do, negative heals (Under Construction).
 * attackSpeed		: One to one 1 second is 1 attackspeed.
 * attackRange		: The maximum distance a unit can attack
 * aggroRange		: The range at which the unit aquires enemy targets
 * movementSpeed	: 10 movement should be your base speed, 5 for slow and 20 for very fast.
 * 
 * Unit Type Specific
 * attackType 		: Shoot: basic attack attack range 0.6 for melee range, Bomb: suicide attack set attack range to 0.5 or will just commit suicide, or Missile range attack<- Case Sensitive
 * movementType		: Flying, or Normal <- Case Sensitive
 * 
 * Team Specific
 * Team				: 0, or 1 <- see below
 * EnemyTeamName	: Temporary <- until we get more teams, in which case EnemyTeamName will become an array
 * 
 * Targets and in game variables
 * CheckPointTarget : Current check point to head towards
 * EnemyTarget		: if Fighting head towards and attack enemy unit
 * CurrentTask		: only two states CheckPoint and Fighting. Determines what the unit is doing
 * Dead				: Determines if the unit is dead, and applies any actions associated with death
 * reloading		: True when able to fire
 * 
 * ChangeLog 
 * Nov 21 11:44am: Added Melee for melee combat and changed Shoot to fire a projectile
 * 					CheckHealth now applies damage, and healing;
 * 					Added Arrow to Tag Manager
 * 					Adjusted movement for missiles
\*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	//Base Variables
	public float health;
	public float attackDamage;
	public float attackSpeed;
	public float attackRange;
	public float aggroRange;
	public float movementSpeed = 1f;
	
	public string attackType;
	public string movementType;
	public string EnemyTeamName;
	
	public int Team;
	
	public string CurrentTask = "Setup";
	//Transforms
	public Transform CheckPointTarget;
	public Transform EnemyTarget;
	//Scripts
	public Unit targetScript;
	public CheckPoint checkscript; 
	
	private bool Dead = false;
	private bool reloading = false;
	
	void OnEnable()
    {
		CurrentTask = "CheckPoint";
    }
	
	// On start set flying, and unit color based on team <- Just until an actual team image is created
	void Start () 
	{
		if (Team == 0)
			renderer.material.SetColor ("_Color",Color.red);
		else
			renderer.material.SetColor ("_Color",Color.blue);
		if (movementType.Equals ("Flying"))
			transform.position = new Vector3 (transform.position[0],transform.position[1],-3);
	}
	
	// Collision with a CheckPoint of which is equal to the CheckPointTarget
	// aquire next target, and continue to next destination.
	void OnTriggerEnter (Collider other)
	{
	switch (other.gameObject.tag)
		{
		case "CheckPoint":
			{
				if (CheckPointTarget.position == other.transform.position)
				{	
					checkscript = other.GetComponent<CheckPoint>();
					CheckPointTarget = checkscript.Paths[Team];
				}
			break;
			}
		}
	}
	
	// straight forward checks health is less than zero and not already dead
	// then changes global stats accordingly. 
	public void checkHealth (float damage)
	{
		health -= damage;
		if (health <= 0 && ! Dead)
		{
			Dead = true;
			if (Team == 0)
			{
				PathingCentre.globalCount --;
				PathingCentre.Team0Count--;
			}
			else
			{
				PathingCentre.globalCount --;
				PathingCentre.Team1Count--;
			}	
			Destroy (gameObject);
		}
	}
	
	// Might change later for smoother turning, just sets the movement towards a point
	public float getSpeed (int position, Transform target)
	{
		if (target.position[position] > transform.position[position]+10f)
		{
			return 1.2f+Random.Range (-0.5f,1f);
		}
		else if (target.position[position] < transform.position[position]-10f)
		{
			return -(1.2f+Random.Range (-0.5f,1f));
		}
		else if (target.position[position] > transform.position[position]+0.05f)
		{
			return 1f+Random.Range (-0.5f,1f);
		}
		else if (target.position[position] < transform.position[position]-0.05f)
		{
			return -(1f+Random.Range (-0.5f,1f));
		}
		return 0;
	}
	
	// Sets the velocity of the unit towards a point
	void Moving (Transform target)
	{
		
		Vector3 Movement = new Vector3 (0f,0f,0f);
		Movement[0] = getSpeed (0, target);
		Movement[1] = getSpeed (1, target);
		//Movement[2] = getSpeed (2, target);
		rigidbody.AddForce (Movement*movementSpeed);//, ForceMode.Force);
	}
	
	// During each check step, moves towards a checkpoint if CurrentTask  = "CheckPoint"
	// else if CurrentTask = "Fighting" moves towards enemy till in range then uses the 
	// appropriate attackType.
	void Update ()
	{
		if (CurrentTask.Equals ("CheckPoint"))
			Moving (CheckPointTarget);
		if (CurrentTask.Equals ("Fighting"))
		{
			try 
			{
				if (Vector3.Distance (transform.position, EnemyTarget.position) <= attackRange+EnemyTarget.localScale.x / 2)
				{
					rigidbody.isKinematic = true;
					if (! reloading)
					{
						StartCoroutine (attackType);
					}
				}
				else 
				{
					rigidbody.isKinematic = false;
					Moving (EnemyTarget);
					CurrentTask = "CheckPoint";
				}
			}
			catch
			{
				rigidbody.isKinematic = false;
				CurrentTask = "CheckPoint";
			}
		}
	}
	
	// Below are different attackTypes
	IEnumerator Melee ()
	{
		reloading = true;
		targetScript.checkHealth (attackDamage);
		yield return new WaitForSeconds (attackSpeed);
		reloading = false;
	}
	
	IEnumerator Shoot ()
	{
		reloading = true;
		PathingCentre.Arrow (transform.position, EnemyTarget, targetScript, attackDamage);
		yield return new WaitForSeconds (attackSpeed);
		reloading = false;
	}
	
	IEnumerator Bomb()
	{
		reloading = true;
		PathingCentre.Explosion (transform.position, EnemyTeamName, attackDamage);
		checkHealth (health);
		yield return new WaitForSeconds (attackSpeed);
		reloading = false;
	}
	
	IEnumerator AOEHeal()
	{
		reloading = true;
		PathingCentre.Explosion (transform.position, gameObject.tag, attackDamage);
		yield return new WaitForSeconds (attackSpeed);
		reloading = false;
	}
	
	IEnumerator Earthquake()
	{
		reloading = true;
		PathingCentre.Explosion (transform.position, EnemyTeamName, attackDamage);
		yield return new WaitForSeconds (attackSpeed);
		reloading = false;
	}
	
	IEnumerator Missile()
	{
		reloading = true;
		PathingCentre.Missile (transform.position, EnemyTarget.position, EnemyTeamName, attackDamage);
		yield return new WaitForSeconds (attackSpeed);
		reloading = false;
	}
	
	IEnumerator AntiAirMissile()
	{
		reloading = true;
		PathingCentre.AntiMissile (transform.position, EnemyTarget.position, EnemyTeamName, attackDamage);
		yield return new WaitForSeconds (attackSpeed);
		reloading = false;
	}
}
