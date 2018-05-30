using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTilesUtil {
    
    public static void SaveGame(Tile[,] gameTiles) {
        string json = "";
        switch (gameTiles.GetLength(0)) {
            case 3:
                json = JsonUtility.ToJson(gameTiles);
                PlayerPrefs.SetString("3x3", json);
                break;
            case 4:
                 json = JsonUtility.ToJson(gameTiles);
                PlayerPrefs.SetString("4x4", json);
                break;
            case 5:
                json = JsonUtility.ToJson(gameTiles);
                PlayerPrefs.SetString("5x5", json);
                break;
        }
                Debug.Log(json);

    }

    internal static void LoadGame(Tile[,] gameTiles, int fieldSize) {
        switch (fieldSize) {
            case 3:
                gameTiles = JsonUtility.FromJson<Tile[,]>(PlayerPrefs.GetString("3x3"));
                break;
            case 4:
                gameTiles = JsonUtility.FromJson<Tile[,]>(PlayerPrefs.GetString("4x4"));
                break;
            case 5:
                gameTiles = JsonUtility.FromJson<Tile[,]>(PlayerPrefs.GetString("5x5"));
                break;
        }        
    }
}
