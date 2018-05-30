
using UnityEngine;

public class SaveGame {

    private SaveGame instance;

    private SaveGame() {
        instance = new SaveGame();
    }

    public SaveGame getInstance() {
        if (instance == null) {
            return new SaveGame();
        } else {
            return instance;
        }
    }

    public void save(Tile[] gameTiles) {
      //  PlayerPrefs.SetInt("score3x3", ScoreManager.instance.Score);
    }
}
