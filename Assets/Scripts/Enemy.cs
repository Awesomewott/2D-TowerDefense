using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    //SerializeField - Allows Inspector to get access to private fields.
    //If we want to get access to this from another class, we'll just need to make public getters
    [SerializeField]
    private Transform exitPoint;
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private float navigationUpdate;
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    private int rewardAmount;

    private int target = 0;
    private Transform enemy;
    private Collider2D enemyCollider;
    private Animator anim;
    private float navigationTime = 0;
    private bool isDead = false;
    private GameManager gameMan;
    private SoundManager soundMan;

    public bool IsDead
    {
        get { return isDead; }
    }

	// Use this for initialization
	void Start () {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        gameMan = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
        soundMan = GameObject.FindGameObjectWithTag("soundManager").GetComponent<SoundManager>();
        gameMan.RegisterEnemy(this);
	}
	
	// Update is called once per frame
	void Update () {
		if (wayPoints != null && !isDead)
        {
            //Lets use change how fast the update occurs
            navigationTime += Time.deltaTime;
            if(navigationTime > navigationUpdate)
            {
                //If enemy is not at the last wayPoint, keep moving towards the wayPoint
                //otherwise move to the exitPoint
                if(target < wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, navigationTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
	}

    //If we trigger the collider2D.tag for checkpoints for finish. 
    //If it hits the checkpoints, increase the index and move to the next checkpoint
    //otherwise enemy is at the finish line and should be destroyed.
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "checkpoint")
            target += 1;
        else if (collider2D.tag == "Finish")
        {
            gameMan.RoundEscaped += 1;
            gameMan.TotalEscape += 1;
            gameMan.UnregisterEnemy(this);
            gameMan.isWaveOver();
        }
        else if(collider2D.tag == "projectile")
        {
            enemyHit(collider2D.gameObject.GetComponent<Projectile>().AttackStrength);
            Destroy(collider2D.gameObject);
        }
    }
    public void enemyHit(int hitPoints)
    {
        healthPoints -= hitPoints;
        gameMan.AudioSource.PlayOneShot(soundMan.Hit);

        if(healthPoints > 0)
        {
            anim.Play("Hurt");
        }
        else
        {
            anim.SetTrigger("didDie");
            die();
        }
    }

    public void die()
    {
        isDead = true;
        enemyCollider.enabled = false;
        gameMan.TotalKilled += 1;
        gameMan.AudioSource.PlayOneShot(soundMan.Death);
        gameMan.AddMoney(rewardAmount);
        gameMan.isWaveOver();
    }
}
