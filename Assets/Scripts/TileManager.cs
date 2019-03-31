using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TileManager : MonoBehaviour
{
    public GameObject currentTile;
    public GameObject[] tilePrefabs;
  
    private Stack<GameObject> leftTiles = new Stack<GameObject>();
    private Stack<GameObject> topTiles = new Stack<GameObject>();

    public Stack<GameObject> LeftTiles
    {
        get { return leftTiles; }
        set { leftTiles = value; }
    }

    public Stack<GameObject> TopTiles
    {
        get { return topTiles; }
        set { topTiles = value; }
    }

    private static TileManager instance;
    public static TileManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TileManager>();
            }
            return instance;
        }
    }

    void Start()
    {
        createTiles(50);
        for (int i = 0; i < 50; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {

    }

    public void createTiles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            topTiles.Push(Instantiate(tilePrefabs[1]));
            topTiles.Peek().name = "TopTile";
            topTiles.Peek().SetActive(false);

            leftTiles.Push(Instantiate(tilePrefabs[0]));
            leftTiles.Peek().name = "LeftTile";
            leftTiles.Peek().SetActive(false);

        }
    }


    public void SpawnTile()
    {
        int randomTile;// = Random.Range(0, 2);
        GameObject temp;
        if (leftTiles.Count == 0 || topTiles.Count == 0)
        {
            createTiles(10);
        }


        if (currentTile.gameObject.name == "TopTile")
        {
            if (currentTile.transform.position.x > 3f)
            {
                randomTile = 0;
            }
            else if (currentTile.transform.position.x < -5f)
            {
                randomTile = 1;
            }
            else
            {
                randomTile = Random.Range(0, 2);
            }
        }
        else
        {
            if (currentTile.transform.position.x > 5f)
            {
                randomTile = 0;
            }
            else if (currentTile.transform.position.x < -3f)
            {
                randomTile = 1;
            }
            else
            {
                randomTile = Random.Range(0, 2);
            }
        }


        if (randomTile == 0)
        {
            temp = leftTiles.Pop();
        }
        else
        {
            temp = topTiles.Pop();
        }

        temp.SetActive(true);
        temp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomTile).position;
        currentTile = temp;

        int spawnDimond = Random.Range(0, 10);
        if (spawnDimond == 0)
        {
            currentTile.transform.GetChild(1).gameObject.SetActive(true);
        }

    }
}
