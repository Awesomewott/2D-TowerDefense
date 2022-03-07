using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    //SerializeField - Allows Inspector to get access to private fields.
    //If we want to get access to this from another class, we'll just need to make public getters
    
    public int healthPoints;
    public int maxHealth;
    [SerializeField]
    private int rewardAmount;

    public HealthBarBeahaviour healthBar;

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
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        gameMan = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
        soundMan = GameObject.FindGameObjectWithTag("soundManager").GetComponent<SoundManager>();
        gameMan.RegisterEnemy(this);
        maxHealth = healthPoints;
        healthBar.SetHealth(healthPoints, maxHealth);
	}

    //If we trigger the collider2D.tag for checkpoints for finish. 
    //If it hits the checkpoints, increase the index and move to the next checkpoint
    //otherwise enemy is at the finish line and should be destroyed.
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Finish")
        {
            gameMan.RoundEscaped += 1;
            gameMan.TotalEscape += 1;
            //gameMan.UnregisterEnemy(this);
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
        healthBar.SetHealth(healthPoints, maxHealth);
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
