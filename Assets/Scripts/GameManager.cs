﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    next, play, gameover, win
}
public class GameManager : MonoBehaviour {
    //SerializeField - Allows Inspector to get access to private fields.
    //If we want to get access to this from another class, we'll just need to make public getters
    [SerializeField]
    private int totalWaves = 10;
    [SerializeField]
    private Enemy[] enemies;
    [SerializeField]
    private int totalEnemies = 3;
    [SerializeField]
    private int enemiesPerSpawn;
    [SerializeField]
    private Button playButton;

    public Text totalMoneyLabel;   //Refers to money label at upper left corner
    public Text currentWaveLabel;
    public Text totalEscapedLabel;
    public GameObject spawnPoint;
    public Text playButtonLabel;

    public int waveNumber = 0;
    public int totalMoney = 20;
    public int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
    private int whichEnemiesToSpawn = 0;
    private int enemiesToSpawn = 0;
    private gameStatus currentState = gameStatus.play;
    private AudioSource audioSource;
    public TowerManager towerMan;
    public SoundManager soundMan;
    public GameObject GOPanel;

    public List<Enemy> EnemyList = new List<Enemy>();
    const float spawnDelay = 2f; //Spawn Delay in seconds

    public int TotalMoney
    {
        get { return totalMoney; }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = totalMoney.ToString();
        }
    }

    public int TotalEscape
    {
        get { return totalEscaped; }
        set { totalEscaped = value; }
    }
 
    public int RoundEscaped
    {
        get { return roundEscaped; }
        set { roundEscaped = value; }
    }
    public int TotalKilled
    {
        get { return totalKilled; }
        set { totalKilled = value; }
    }

    public AudioSource AudioSource
    {
        get { return audioSource; }
    }
    
    // Use this for initialization
    void Start () {
        playButton.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        GOPanel.SetActive(false);
        //Time.timeScale = 3;
        ShowMenu();
	   }

	
	// Update is called once per frame
	void Update () {
        handleEscape();
        
  }

    //This will spawn enemies, wait for the given spawnDelay then call itself again to spawn another enemy
    IEnumerator spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    Enemy newEnemy = Instantiate(enemies[Random.Range(0, enemiesToSpawn)]);
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(spawn());
        }
    }

    ///Register - when enemy spawns
    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }
    ///Unregister - When they escape the screen
    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    ///Destroy - At the end of the wave
    public void DestroyAllEnemies()
    {
        foreach(var pro in GameObject.FindGameObjectsWithTag("projectile"))
        {
            Destroy(pro);
        }
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public void AddMoney(int amount)
    {
        TotalMoney += amount;
    }

    public void SubtractMoney(int amount)
    {
        TotalMoney -= amount;
    }

    public void isWaveOver()
    {
        totalEscapedLabel.text = "Escaped " + TotalEscape + "/10";

        if (RoundEscaped + TotalKilled == totalEnemies)
        {
            if(waveNumber <= enemies.Length)
            {
                enemiesToSpawn = waveNumber;
            }
            
            
            setCurrentGameState();
            ShowMenu();
            
        }
        else if (totalEscaped >= 10) 
        {
          setCurrentGameState();
          ShowMenu();
        }
  }

    public void setCurrentGameState()
    {
        if(totalEscaped >= 10)
        {
            currentState = gameStatus.gameover;
        }
        else if(waveNumber == 0 && (TotalKilled + RoundEscaped) == 0)
        {
            currentState = gameStatus.play;
        }
        else if(waveNumber >= totalWaves)
        {
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }

    public void ShowMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playButtonLabel.text = "Play Again!";
                AudioSource.PlayOneShot(soundMan.Gameover);
                GOPanel.SetActive(true);
                break;
            case gameStatus.next:
                playButtonLabel.text = "Next Wave";
                break;
            case gameStatus.play:
                playButtonLabel.text = "Play";
                break;
            case gameStatus.win:
                playButtonLabel.text = "Play";
                break;
        }
        playButton.gameObject.SetActive(true);
    }
    public void playButtonPressed()
    {
        Debug.Log("Play Button Pressed");
        switch (currentState)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            default:
                totalEnemies = 3;
                totalEscaped = 0;
                TotalMoney = 30;
                towerMan.DestroyAllTower();
                towerMan.RenameTagsBuildSites();
                totalMoneyLabel.text = TotalMoney.ToString();
                totalEscapedLabel.text = "Escaped " + totalEscaped + "/10";
                AudioSource.PlayOneShot(soundMan.NewGame);
                break;
        }
        DestroyAllEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWaveLabel.text = "Wave " + (waveNumber + 1);
        StartCoroutine(spawn());
        playButton.gameObject.SetActive(false);
    }
    private void handleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            towerMan.disableDragSprite();
            towerMan.towerButtonPressed = null;
        }
    }

}
