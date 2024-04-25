using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelDataList
{
   public List<LevelData> data = new List<LevelData>();

    public LevelDataList(LevelData levelData)
    {
        data.Add(levelData);
    }
}
