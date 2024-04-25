using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    [SerializeField] GameObject PlayerPrefab;

    private GridController gridController;

    public void spawnPlayer(bool mirrored_player)
    {
        var player = Instantiate(PlayerPrefab, new Vector3(transform.position.x, transform.position.y, -2), Quaternion.identity);
        player.GetComponent<GridMovement>().setIsMirror(mirrored_player);
        player.GetComponent<GridMovement>().setGridController(gridController);
        if (mirrored_player)
        {
            player.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            player.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    public void setGridController(GridController gridController)
    {
        this.gridController = gridController;
    }

}
