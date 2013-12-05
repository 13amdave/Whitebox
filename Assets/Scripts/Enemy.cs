using UnityEngine;
using System.Collections;
using System;

public class Enemy : IComparable <Enemy>
{
	public Transform enemyTransform;
	public Transform unit;
	public Enemy (Transform newUnit, Transform newEnemyTransform)
	{
		enemyTransform = newEnemyTransform;
		unit = newUnit;
	}
	
	public int CompareTo(Enemy other)
    {
        if(other == null)
            return 1;
		else if (Vector3.Distance (unit.position,other.enemyTransform.position) > Vector3.Distance (unit.position,enemyTransform.position)) 
        	return 1;
		else if (Vector3.Distance (unit.position,other.enemyTransform.position) == Vector3.Distance (unit.position,enemyTransform.position))
			return 0;
		return -1;
    }
}
