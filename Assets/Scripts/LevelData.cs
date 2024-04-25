using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

[Serializable]
public class LevelData
{
    public string levelName;
    public int row_num;
    public int col_num;
    public int[] level_arr;

    public LevelData(int r, int c)
    {
        row_num = r; col_num = c;
        level_arr = new int[r*c];
    }

    public static int[,] GetLevel(LevelData levelData)
    {
        int[,] level = new int[levelData.row_num, levelData.col_num];
        int i = 0;
        for (int r = 0; r < levelData.row_num; r++)
        {
            for (int c = 0; c < levelData.col_num; c++)
            {
                level[r, c] = levelData.level_arr[i];
                i++;
            }
        }

        return level;
    }

    public string GetName()
    {
        return this.levelName;
    }
}
