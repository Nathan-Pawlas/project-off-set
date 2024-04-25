using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LevelDesigner : MonoBehaviour
{
    //Width of the level
    private int cols;

    //Height of the level
    private int rows;

    //Dictionary mapping positions in grid to what tile lies at that position
    private Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();

    //Position of the camera for centering the Game Board (TODO: Change Later)
    [SerializeField] private Transform cam;

    //Prefab containing tile objects
    [SerializeField] private EditableTile editableTilePrefab;

    [SerializeField] private LevelDesignerSettings levelSettingsController;

    private void Start()
    {
        levelSettingsController.GetComponent<LevelDesignerSettings>().getConfirm().onClick.AddListener(GenerateGrid);
        rows = levelSettingsController.getHeight();
        cols = levelSettingsController.getWidth();

        //Create grid Representing Start State indicated in Level
        GenerateGrid();
    }

    void GenerateGrid()
    {
        clearGrid();
        levelSettingsController.GetComponent<LevelDesignerSettings>().confirmSettings();
        rows = levelSettingsController.getHeight();
        cols = levelSettingsController.getWidth();
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                //Create a Tile Object at every position within the grid
                var spawnedTile = Instantiate(editableTilePrefab, new Vector3(x, y, 1), Quaternion.identity);
                spawnedTile.name = $"Tile {x}, {y}";
                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        //Center Camera
        cam.transform.position = new Vector3((float)cols / 2 - 0.5f, (float)rows / 2 - 0.5f, -10);
    }

    void clearGrid()
    {
        foreach(Tile tile in tiles.Values)
        {
            Destroy(tile.gameObject);
        }
        tiles = new Dictionary<Vector2, Tile>();
    }

    //Get Tile at position x,y
    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    public void saveLevel()
    {
        LevelData levelData = new LevelData(rows, cols);
        levelData.levelName = levelSettingsController.getName();
        int[,] level = new int[rows,cols];
        int i = 0;
        for(int row = 0; row < rows; row++)
        {
            for(int col = 0; col < cols; col++)
            {
                levelData.level_arr[i] = GetTileAtPosition(new Vector2(col, rows - row - 1)).GetComponent<EditableTile>().getObjectAsInt();
                i++;
            }
        }

        
        //Initialize a Json File
        if (!File.Exists(Application.streamingAssetsPath + "/LevelData.json"))
        {
            LevelDataList jsonData = new LevelDataList(levelData); //Create a new LevelDataList Object with 1 entry (The created Level)
            string json = JsonUtility.ToJson(jsonData); // Turn the LevelDataList into a string
            File.WriteAllText(Application.streamingAssetsPath + "/LevelData.json", json); //Write the string to a json file
        }
        else //Append to Existing Json File
        {
            string jsonToRead = File.ReadAllText(Application.streamingAssetsPath + "/LevelData.json"); //Get String from Json File
            if (jsonToRead.Length > 0)
            {
                LevelDataList append_data = JsonUtility.FromJson<LevelDataList>(jsonToRead); //Turn String into LevelDataList Object
                append_data.data.Add(levelData); //Append the Created Level to the LevelDataList 
                string json = JsonUtility.ToJson(append_data); //Turn the LevelDataList back into a string
                File.WriteAllText(Application.streamingAssetsPath + "/LevelData.json", json); //Write string back to json file
            }
            else
            {
                LevelDataList jsonData = new LevelDataList(levelData); //Create a new LevelDataList Object with 1 entry (The created Level)
                string json = JsonUtility.ToJson(jsonData); // Turn the LevelDataList into a string
                File.WriteAllText(Application.streamingAssetsPath + "/LevelData.json", json); //Write the string to a json file
            }
        }

        Debug.Log(Application.streamingAssetsPath);
    }

}
