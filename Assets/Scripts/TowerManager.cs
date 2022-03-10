using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour {
    public TowerButton towerButtonPressed { get; set; }
    private SpriteRenderer spriteRenderer;  //Setting image to our tower
    private List<Tower> TowerList = new List<Tower>();
    private List<Collider2D> BuildList = new List<Collider2D>();
    private Collider2D buildTile;
    public GameManager gameMan;
    public SoundManager soundMan;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();
        if (spriteRenderer != null) spriteRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            //worldPoint is the position of the mouse click.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            /* Ray Cast involves intersecting a ray with the object in an environment.
             * The ray cast tells you what objects in the environment the ray runs into.
             * and may return additional information as well, such as intersection point
             */
            //Finding the worldPoint of where we click, from Vector2.zero (which is buttom left corner)
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //Check to see if mouse press location is on buildSites

            if (hit.collider != null && hit.collider.tag == "buildSite" && towerButtonPressed != null)
            {
                buildTile = hit.collider;
                buildTile.tag = "buildSiteFull";     //This prevents us from stacking towers ontop of each other.
                RegisterBuildSite(buildTile);
                placeTower(hit);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if(hit.collider != null && hit.collider.tag == "buildSiteFull")
            {
                buildTile = hit.collider;
                buildTile.tag = "buildSite";
                removeTower(hit);
            }
        }

        //When we have a sprite enabled, have it follow the mouse (I.E - Placing a Tower)
        if (spriteRenderer != null && spriteRenderer.enabled)
        {
            followMouse();
        }
    }

    public void RegisterBuildSite(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }

    public void RegisterTower(Tower tower)
    {
        TowerList.Add(tower);
    }

    public void RenameTagsBuildSites()
    {
        foreach(Collider2D buildTag in BuildList)
        {
            buildTag.tag = "buildSite";
        }
        BuildList.Clear();
    }

    public void DestroyAllTower()
    {
        foreach (Tower tower in TowerList)
        {
            if (tower != null)
            {
                Destroy(tower.gameObject);
            }
        }
        TowerList.Clear();
    }
    //Place new tower on the mouse click location
    public void placeTower(RaycastHit2D hit)
    {
        //If the pointer is not over the Tower Button GameObject && the tower button has been pressed
        //Created new tower at the click location
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Tower newTower = Instantiate(towerButtonPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            buyTower(towerButtonPressed.TowerPrice);
            gameMan.AudioSource.PlayOneShot(soundMan.TowerBuilt);
            RegisterTower(newTower);
            disableDragSprite();
            towerButtonPressed = null;
        }
    }

    public void removeTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            for(int i = 0; i < TowerList.Count; i++)
            {
                if (TowerList[i] != null)
                {
                    if (TowerList[i].transform.position == hit.transform.position)
                    {
                        switch (TowerList[i].projectile.projectileType)
                        {
                            case ProjectileType.arrow:
                                sellTower(5);
                                break;
                            case ProjectileType.rock:
                                sellTower(10);
                                break;
                            case ProjectileType.fireball:
                                sellTower(15);
                                break;
                            default:
                                sellTower(5);
                                break;
                        }

                        Destroy(TowerList[i].gameObject);
                        continue;
                    }
                }
            }
        }
    }

    public void buyTower(int price)
    {
        gameMan.SubtractMoney(price);
    }
    public void sellTower(int price)
    {
        gameMan.AddMoney((int)(price / 2));
    }

    public void selectedTower(TowerButton towerSelected)
    {
        if(gameMan.TotalMoney - towerSelected.TowerPrice >= 0)
        {
            towerButtonPressed = towerSelected;
            enableDragSprite(towerSelected.DragSprite);
        }
    }

    public void followMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void enableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite; //Set sprite to the one we passed in the parameter
        spriteRenderer.sortingOrder = 10;
    }
    public void disableDragSprite()
    {
        spriteRenderer.enabled = false;
    }
}
