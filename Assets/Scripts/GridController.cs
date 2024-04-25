using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridController : MonoBehaviour
{
    //Width of the level
    private int cols;
    
    //Height of the level
    private int rows;

    private GameObject centerBorder;

    //Dictionary mapping positions in grid to what tile lies at that position
    private Dictionary<Vector2, Tile> tiles;

    private List<Key> keys = new List<Key>();

    private List<Door> doors = new List<Door>();

    //Prefab containing tile objects
    [SerializeField] private Tile tilePrefab;

    //Impassable Wall Game Object
    [SerializeField] private GameObject wallPrefab;

    [SerializeField] private GameObject playerEnd;

    [SerializeField] private GameObject centerBorderPrefab;

    [SerializeField] private GameObject playerStartPrefab;

    [SerializeField] private GameObject keyPrefab;

    [SerializeField] private GameObject doorPrefab;

    [SerializeField] private GameObject endOfLevel;

    [SerializeField] private GameObject tutorialPopUp;

    [SerializeField] private GameObject movablePrefab;

    public static int levelSelector = 0;

    private LevelDataList levelDataList;

    private LevelData levelData;

    private int[,] level;

    //How many players on the grid
    private int players = 0;
    //How many have processed their move yet
    private int playersMoved = 0;
    //Maintain a list of interactables to be updated at the end of the turn
    private List<Interactable> InteractablesToBeActivated = new List<Interactable>();

    private VideoPlayer videoPlayer;

    [SerializeField] public GameObject levelNamePopUp;

    public AudioSource swooshClip;

    public AudioSource moveSound;

    public VideoClip[] clips;

    private void Start()
    {
        videoPlayer = FindObjectOfType<VideoPlayer>();

        //Create grid Representing Start State indicated in Level
        StartLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { EndLevel(); }  
        if(Input.GetKeyDown(KeyCode.R)) { ResetLevel(); }
        if (Input.GetKeyDown(KeyCode.Space) && levelSelector == 0) { tutorialPopUp.SetActive(false); }
    }

    void StartLevel()
    {
        if (levelSelector == 0)
        {
            tutorialPopUp.SetActive(true);
        }

        string jsonRead = File.ReadAllText(Application.streamingAssetsPath + "/LevelData.json");
        levelDataList = JsonUtility.FromJson<LevelDataList>(jsonRead);
        try
        {
            levelData = levelDataList.data[levelSelector];
        }
        catch
        {
            levelData = levelDataList.data[0];
        }
        rows = levelData.row_num;
        cols = levelData.col_num;

        level = new int[rows, cols];
        level = LevelData.GetLevel(levelData);

        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                //Create a Tile Object at every position within the grid
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y, 3), Quaternion.identity);
                spawnedTile.name = $"Tile {x}, {y}";
                GameObject newObject;
                //Determine GameObject to go into tile at position x,y based on level start state
                switch (level[y, x])
                {
                    case 1:
                        newObject = Instantiate(wallPrefab);
                        spawnedTile.addObject(newObject);
                        break;
                    case 2:
                        newObject = Instantiate(playerStartPrefab);
                        newObject.GetComponent<PlayerStart>().setGridController(this);
                        players++;
                        spawnedTile.addObject(newObject);
                        break;
                    case 3:
                        newObject = Instantiate(playerEnd);
                        spawnedTile.addObject(newObject);
                        break;
                    case 4:
                        newObject = Instantiate(keyPrefab);
                        newObject.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                        newObject.GetComponent<Key>().setLinkNumber(0);
                        keys.Add(newObject.GetComponent<Key>());
                        spawnedTile.addObject(newObject);
                        break;
                    case 5:
                        newObject = Instantiate(doorPrefab);
                        newObject.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                        newObject.GetComponent<Door>().setLinkNumber(0);
                        doors.Add(newObject.gameObject.GetComponent<Door>());
                        spawnedTile.addObject(newObject);
                        break;
                    case 6:
                        newObject = Instantiate(keyPrefab);
                        newObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                        newObject.GetComponent<Key>().setLinkNumber(1);
                        keys.Add(newObject.GetComponent<Key>());
                        spawnedTile.addObject(newObject);
                        break;
                    case 7:
                        newObject = Instantiate(doorPrefab);
                        newObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                        newObject.GetComponent<Door>().setLinkNumber(1);
                        doors.Add(newObject.gameObject.GetComponent<Door>());
                        spawnedTile.addObject(newObject);
                        break;
                    case 8:
                        newObject = Instantiate(keyPrefab);
                        newObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                        newObject.GetComponent<Key>().setLinkNumber(2);
                        keys.Add(newObject.GetComponent<Key>());
                        spawnedTile.addObject(newObject);
                        break;
                    case 9:
                        newObject = Instantiate(doorPrefab);
                        newObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                        newObject.GetComponent<Door>().setLinkNumber(2);
                        doors.Add(newObject.gameObject.GetComponent<Door>());
                        spawnedTile.addObject(newObject);
                        break;
                    case 10:
                        newObject = Instantiate(movablePrefab);
                        spawnedTile.addObject(newObject);
                        break;
                }
                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        centerBorder = Instantiate(centerBorderPrefab, new Vector2(cols / 2 - 0.5f, rows / 2 - 0.5f), Quaternion.identity);
        centerBorder.transform.localScale = new Vector2(1f, rows);

        linkDoorsAndKeys();

        //Center Camera
        Camera.main.transform.position = new Vector3((float)cols / 2 - 0.5f, (float)rows / 2 - 0.5f, -10);

        //Set Background Video
        int i = Random.Range(0, clips.Length);
        videoPlayer.clip = clips[i];

        //Start PopUp Animation
        levelNamePopUp.GetComponentInChildren<TextMeshProUGUI>().text = levelData.levelName;
        levelNamePopUp.SetActive(true);

        //Spawn Players
        spawnPlayers();
    }

    private void linkDoorsAndKeys()
    {
        foreach(Key key in keys)
        {
            foreach(Door door in doors)
            {
                if(key.getLinkNumber() == door.getLinkNumber())
                {
                    key.addDoor(door);
                }
            }
        }
    }


    private void spawnPlayers()
    {
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Tile tile = GetTileAtPosition(new Vector2(x, y));
                foreach (var obj in tile.getContainedObjects())
                {
                    if (obj.GetComponent<PlayerStart>() != null)
                    {
                        bool isMirror = x > cols / 2 -1;
                        obj.GetComponent<PlayerStart>().spawnPlayer(isMirror);
                    }
                }
            }
        }
    }

    //Get Tile at position x,y
    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out var tile)){
            return tile;
        }
        return null;
    }

    private bool isImmovable(Tile tile)
    {
        foreach (GameObject go in tile.getContainedObjects())
        {
            if(go.tag == "Immovable")
            {
                return true;
            }
        }
        return false;
    }

    private bool crossCenter(Vector2 src, Vector2 dest)
    {
        if (src.x == cols / 2 && dest.x == (cols / 2) - 1)
        {
            return true;
        }
        if (src.x == (cols / 2) - 1 && dest.x == cols / 2)
        {
            return true;
        }
        return false;
    }

    private bool atEdge(Vector2 dest)
    {
        if(dest.x >= cols || dest.x < 0)
        {
            return true;
        }
        if(dest.y >= rows || dest.y < 0)
        {
            return true;
        }
        return false;
    }

    private bool isDoorAndClosed(Tile tile)
    {
        foreach (GameObject go in tile.getContainedObjects())
        {
            if (go.tag == "Door")
            {
                if (go.GetComponent<Door>().isClosed)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool isMovableBlocked(Tile tile, Vector2 src, Vector2 dest)
    {
        Vector2 dir = dest - src;
        foreach (GameObject go in tile.getContainedObjects())
        {
            if (go.tag == "Movable")
            {
                if (movableCanMove(dest, dest + dir))
                {
                    go.GetComponent<MoveableBlock>().StartCoroutine(go.GetComponent<MoveableBlock>().Move(dir));
                    tile.moveObjectTo(go, GetTileAtPosition(dest + dir));
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool movableCanMove(Vector2 src, Vector2 dest)
    {
        if (atEdge(dest))
        {
            return false;
        }
        Tile tile = GetTileAtPosition(dest);

        if (isImmovable(tile))
        {
            return false;
        }
        if (isDoorAndClosed(tile))
        {
            return false;
        }
        if(isMovableBlocked(tile, src, dest))
        {
            return false;
        }
        return true;
    }

    //Logic for whether player movement is valid
    public bool playerCanMove(Vector2 src, Vector2 dest)
    {
        //Find tile at the destination of the movement
        Tile tile = GetTileAtPosition(dest);
        if (crossCenter(src, dest) || atEdge(dest))
        {
            return false;
        }

        //If destination is empty or not immovable then allow movement
        if (isImmovable(tile))
        {
            return false;
        }

        if (isDoorAndClosed(tile))
        {
            return false;
        }

        if(isMovableBlocked(tile, src, dest))
        {
            return false;
        }
        //Movement is not valid dont move player
        return true;
    }

    public void processMoveInteractables(List<Interactable> interactables)
    {
        //Wait for both players to finish moves
        foreach (Interactable interactable in interactables)
        {
            InteractablesToBeActivated.Add(interactable);
        }
        playersMoved++;
        if (playersMoved == players)
        {
            playersMoved = 0;
            foreach (Interactable go in InteractablesToBeActivated)
            {
                if (!go.currentlyActive)
                {
                    go.interact();
                }
                else
                {
                    go.stopInteract();
                }
            }
            InteractablesToBeActivated.Clear();
        }
    }

    public List<Interactable> tileContainsInteractable(Vector2 dest)
    {
        Tile tile = GetTileAtPosition(dest);
        return tile.containsInteractable();
    }

    public bool CheckEndConditions()
    {
        GridMovement[] players = FindObjectsByType<GridMovement>(FindObjectsSortMode.None);
        foreach (GridMovement player in players)
        {
            if (!player.onGoal)
                return false;
        }

        swooshClip.Play();
        return true;
    }

    void clearGrid()
    {
        players = 0;
        Destroy(centerBorder);
        InteractablesToBeActivated.Clear();
        doors.Clear();
        keys.Clear();
        foreach (Tile tile in tiles.Values)
        {
            tile.clearObjects();
            Destroy(tile.gameObject);
        }
        tiles = new Dictionary<Vector2, Tile>();
    }

    void DespawnPlayers()
    {
        GridMovement[] players = FindObjectsByType<GridMovement>(FindObjectsSortMode.None);
        foreach (GridMovement player in players)
        {
            Destroy(player.gameObject);
        }
    }

    public void TransitionScreen()
    {
        endOfLevel.SetActive(true);
    }

    public void EndLevel()
    {
        endOfLevel.SetActive(false);
        levelNamePopUp.SetActive(false);
        clearGrid();
        DespawnPlayers();
        levelSelector += 1;
        //If on Last Level than go to Main Menu
        if (levelDataList.data.Count <= levelSelector)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartLevel();
        }
    }

    public void ResetLevel()
    {
        endOfLevel.SetActive(false);
        levelNamePopUp.SetActive(false);
        clearGrid();
        DespawnPlayers();
        //If on Last Level than go to Main Menu
        if (levelDataList.data.Count <= levelSelector)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartLevel();
        }
    }

}
