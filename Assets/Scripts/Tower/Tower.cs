﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    [SerializeField]
    private float timeBetweenAttacks;   //AKA - Attack Speed
    [SerializeField]
    private float attackRange;          //AKA - Attack Radius
    [SerializeField]
    public Projectile projectile;      //Type of Projectile
    private Enemy targetEnemy = null;
    private float attackCounter;
    private bool isAttacking = false;
    private GameManager gameMan;
    private SoundManager soundMan;

	// Use this for initialization
	void Start ()
    {
        gameMan = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
        soundMan = GameObject.FindGameObjectWithTag("soundManager").GetComponent<SoundManager>();
    }
	
	// Update is called once per frame
	void Update () {
        //If our closest enemy in range and if its within our attackRange, set our target enemy to the closest enemy in range.
        if (targetEnemy == null || targetEnemy.IsDead)
        {
            Enemy closestEnemy = GetClosestEnemyInRange();
            if(closestEnemy != null && Vector2.Distance(transform.localPosition, closestEnemy.transform.position) <= attackRange)
            {
                targetEnemy = closestEnemy;
            }
        }
        else
        {
            if(Time.time >= attackCounter)
            {
                isAttacking = true;
                attackCounter = Time.time + timeBetweenAttacks; //reset attack counter
            }
            else
            {
                isAttacking = false;
            }
            //If enemy gets out of attack range, then that enemy can no longer be targeted
            if (Vector2.Distance(transform.position, targetEnemy.transform.position) > attackRange)
            {
                targetEnemy = null;
            }
        }

	}
    void FixedUpdate()
    {
        if (isAttacking) { Attack(); }

    }
    public void Attack()
    {
        isAttacking = false;
        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.localPosition = transform.localPosition;

        if(newProjectile.ProjectileType == ProjectileType.arrow)
        {
            gameMan.AudioSource.PlayOneShot(soundMan.Arrow);
        } else if(newProjectile.ProjectileType == ProjectileType.fireball)
        {
            gameMan.AudioSource.PlayOneShot(soundMan.Fireball);
        } else if (newProjectile.ProjectileType == ProjectileType.rock)
        {
            gameMan.AudioSource.PlayOneShot(soundMan.Rock);
        }
        //If we have a target enemy, start a coroutine to shoot projectile to target enemy
        if(targetEnemy == null)
        {
            Destroy(newProjectile);
        }
        else
        {
            StartCoroutine(MoveProjectile(newProjectile));
        }
    }
    ///Move Projectile to Target Enemy
    IEnumerator MoveProjectile(Projectile projectile)
    {
        while(getTargetDistance(targetEnemy) > 0.20f && projectile != null && targetEnemy != null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;                         //Angle of the projectile
            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);  //Rotation of projectile
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime); //Move Projectile
            yield return null;
        }
        if (projectile != null || targetEnemy == null)
        {
            Destroy(projectile);
        }
    }

    ///Get the current target's distance
    private float getTargetDistance(Enemy enemy)
    {
        if(enemy == null)
        {
            enemy = GetClosestEnemyInRange();
            if(enemy == null)
            {
                return 0f;
            }
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, enemy.transform.localPosition));
    }
    ///Get Enemies in Attack Range
    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();

        //Check if enemies are in range
        foreach(Enemy enemy in gameMan.EnemyList)
        {
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRange && !enemy.IsDead)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
    ///Get Closest Enemy - Foreach enemy in range, get the closest enemy
    private Enemy GetClosestEnemyInRange()
    {
        Enemy closestEnemy = null;
        float smallestDistance = float.PositiveInfinity; 

        foreach(Enemy enemy in GetEnemiesInRange())
        {
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
}
