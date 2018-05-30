using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { PLAYING, GAME_OVER, WAITING_END_MOVE }

public class GameManager : MonoBehaviour {

    [Range(4, 5)]
    public int FIELD_SIZE;

    public GameObject winGameText;
    public GameObject gameOverText;
    public Text gameOverScoreText;

    public GameObject gameFieldSize4x4;
    public GameObject gameFieldSize5x5;

    public GameObject gameOverPanel;
    private bool hasMove;

    public GameState state;
    [Range(0f, 0.5f)]
    public float delay;

    private bool[] lineMoveEnded;
    private Tile[,] allTiles;
    private Stack<Tile[,]> stack = new Stack<Tile[,]>();
    private List<Tile[]> columns = new List<Tile[]>();
    private List<Tile[]> rows = new List<Tile[]>();
    private List<Tile> emptyTiles = new List<Tile>();
    private bool isWin;

    void Start() {
        initFieldSize();
        initArrayAllTiles();
        copyToStack();
        state = GameState.PLAYING;
        Generate();
        Generate();
    }

    private void initArrayAllTiles() {
        lineMoveEnded = new bool[FIELD_SIZE];
        for (int i = 0; i < lineMoveEnded.Length; i++) {
            lineMoveEnded[i] = true;
        }

        allTiles = new Tile[FIELD_SIZE, FIELD_SIZE];
        Tile[] gameTiles = GameObject.FindObjectsOfType<Tile>();
        foreach (Tile tile in gameTiles) {
            tile.Number = 0;
            this.allTiles[tile.indexRow, tile.indexColumn] = tile;
            emptyTiles.Add(tile);
        }

        for (int i = 0; i < FIELD_SIZE; i++) {
            Tile[] columnCell = new Tile[FIELD_SIZE];
            Tile[] rowCell = new Tile[FIELD_SIZE];

            for (int j = 0; j < FIELD_SIZE; j++) {
                columnCell[j] = this.allTiles[j, i];
                rowCell[j] = this.allTiles[i, j];
            }
            columns.Add(columnCell);
            rows.Add(rowCell);
        }
    }

    public void Undo() {
        Debug.Log("Undo " + stack.Count);
        if (stack.Count == 1)
            return;

        Tile[,] tmp = stack.Pop();

        //bool isRepeat = true;
        //for (int i = 0; i < allTiles.GetLength(0); i++) {
        //    for (int j = 0; j < allTiles.GetLength(1); j++) {
        //        if (tmp[i, j].Number != allTiles[i, j].Number) {
        //            isRepeat = false;
        //            break;
        //        }
        //    }
        //    if (!isRepeat) {
        //        break;
        //    }
        //}
        //if (isRepeat) {
        //    tmp = stack.Pop();
        //}

        if (Array.Equals(allTiles, tmp)) {
            tmp = stack.Pop();
        }

        foreach (Tile tile in tmp) {
            this.allTiles[tile.indexRow, tile.indexColumn].Number = tile.Number;
            emptyTiles.Add(tile);
        }
        UpdateEmptyTiles();

    }

    private void copyToStack() {
        if (stack.Count > 10) {
            stack.Clear();
        }
        Copy();
    }

    private void Copy() {
        Tile[,] tmp = new Tile[FIELD_SIZE, FIELD_SIZE];
        for (int i = 0; i < allTiles.GetLength(0); i++) {
            for (int j = 0; j < allTiles.GetLength(1); j++) {
                tmp[i, j] = allTiles[i, j].Clone() as Tile;
            }
        }
        if (stack.Count == 0) {
            stack.Push(tmp);
        } else {
            checkRep(tmp);
        }
    }

    private void checkRep(Tile[,] tmpTiles) {
        Tile[,] old = stack.Pop();
        //for (int i = 0; i < tmpTiles.GetLength(0); i++) {
        //    for (int j = 0; j < tmpTiles.GetLength(1); j++) {
        //        if (old[i, j].Number != tmpTiles[i, j].Number) {
        //            stack.Push(old);
        //            stack.Push(tmpTiles);
        //            return;
        //        }
        //    }
        //}

        if (!Array.Equals(allTiles, old)) {
            stack.Push(old);
            stack.Push(tmpTiles);
            return;
        }
    }

    private void initFieldSize() {
        switch (FIELD_SIZE) {
            case 4:
                gameFieldSize4x4.SetActive(true);
                gameFieldSize5x5.SetActive(false);
                break;
            case 5:
                gameFieldSize4x4.SetActive(false);
                gameFieldSize5x5.SetActive(true);
                break;
        }
    }

    public void Move(MoveDirection moveDirection) {
        ResetMergedFlags();
        copyToStack();
        hasMove = false;
        if (delay > 0) {
            StartCoroutine(MoveCoroutine(moveDirection));
        } else {
            for (int i = 0; i < rows.Count; i++) {
                switch (moveDirection) {
                    case MoveDirection.DOWN:
                        while (MoveTowardsIndexDecrease(columns[i])) {
                            hasMove = true;
                        }
                        break;
                    case MoveDirection.LEFT:
                        while (MoveTowardsIndexIncrease(rows[i])) {
                            hasMove = true;
                        }
                        break;
                    case MoveDirection.RIGHT:
                        while (MoveTowardsIndexDecrease(rows[i])) {
                            hasMove = true;
                        }
                        break;
                    case MoveDirection.UP:
                        while (MoveTowardsIndexIncrease(columns[i])) {
                            hasMove = true;
                        }
                        break;
                }
            }

            //if (hasMove) {
            //    Debug.Log(hasMove);
            //    UpdateEmptyTiles();
            //    Generate();
            //    if (!canMove()) {
            //        GameOver();
            //    }
            //} 
        }
    }

    private IEnumerator MoveCoroutine(MoveDirection moveDirection) {
        state = GameState.WAITING_END_MOVE;

        switch (moveDirection) {
            case MoveDirection.DOWN:
                for (int i = 0; i < columns.Count; i++) {
                    StartCoroutine(MoveTowardsIndexDecreaseCoroutine(columns[i], i));
                }
                break;
            case MoveDirection.LEFT:
                for (int i = 0; i < rows.Count; i++) {
                    StartCoroutine(MoveTowardsIndexIncreaseCoroutine(rows[i], i));
                }
                break;
            case MoveDirection.RIGHT:
                for (int i = 0; i < rows.Count; i++) {
                    StartCoroutine(MoveTowardsIndexDecreaseCoroutine(rows[i], i));
                }
                break;
            case MoveDirection.UP:
                for (int i = 0; i < columns.Count; i++) {
                    StartCoroutine(MoveTowardsIndexIncreaseCoroutine(columns[i], i));
                }
                break;
        }

        while (!checkEndMove()) {
            yield return null;
        }

        if (hasMove) {
            UpdateEmptyTiles();
            Generate();
            if (!canMove()) {
                GameOver();
            }
        }
        state = GameState.PLAYING;
        StopAllCoroutines();
    }

    private bool checkEndMove() {
        //bool checkEndedMove = true;
        for (int i = 0; i < lineMoveEnded.Length; i++) {
            if (lineMoveEnded[i]) {
                continue;
            } else {
                return lineMoveEnded[i];
            }
        }
        return true;
    }

    private IEnumerator MoveTowardsIndexIncreaseCoroutine(Tile[] line, int index) {
        lineMoveEnded[index] = false;
        while (MoveTowardsIndexIncrease(line)) {
            hasMove = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveEnded[index] = true;
    }

    private IEnumerator MoveTowardsIndexDecreaseCoroutine(Tile[] line, int index) {
        lineMoveEnded[index] = false;
        while (MoveTowardsIndexDecrease(line)) {
            hasMove = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveEnded[index] = true;
    }

    bool MoveTowardsIndexIncrease(Tile[] lineOfTiles) {
        for (int i = 0; i < lineOfTiles.Length - 1; i++) {
            //MOVE BLOCK 
            if (lineOfTiles[i].Number == 0 && lineOfTiles[i + 1].Number != 0) {
                lineOfTiles[i].Number = lineOfTiles[i + 1].Number;
                lineOfTiles[i + 1].Number = 0;
                return true;
            }
            // MERGE BLOCK
            if (lineOfTiles[i].Number != 0 && lineOfTiles[i].Number == lineOfTiles[i + 1].Number &&
                lineOfTiles[i].isMerged == false && lineOfTiles[i + 1].isMerged == false) {

                lineOfTiles[i].Number *= 2;
                lineOfTiles[i + 1].Number = 0;
                lineOfTiles[i].isMerged = true;
                lineOfTiles[i].DoMergeAnimation();
                ScoreManager.instance.Score += lineOfTiles[i].Number;
                if (lineOfTiles[i].Number == 2048 && !isWin) {
                    WonGame();
                }
                return true;
            }
        }
        return false;
    }

    bool MoveTowardsIndexDecrease(Tile[] lineOfTiles) {
        for (int i = lineOfTiles.Length - 1; i > 0; i--) {
            //MOVE BLOCK 
            if (lineOfTiles[i].Number == 0 && lineOfTiles[i - 1].Number != 0) {
                lineOfTiles[i].Number = lineOfTiles[i - 1].Number;
                lineOfTiles[i - 1].Number = 0;
                return true;
            }
            // MERGE BLOCK
            if (lineOfTiles[i].Number != 0 && lineOfTiles[i].Number == lineOfTiles[i - 1].Number &&
                  lineOfTiles[i].isMerged == false && lineOfTiles[i - 1].isMerged == false) {

                lineOfTiles[i].Number *= 2;
                lineOfTiles[i - 1].Number = 0;
                lineOfTiles[i].isMerged = true;
                lineOfTiles[i].DoMergeAnimation();
                ScoreManager.instance.Score += lineOfTiles[i].Number;
                if (lineOfTiles[i].Number == 2048 && !isWin) {
                    WonGame();
                }
                return true;
            }
        }
        return false;
    }

    private void ResetMergedFlags() {
        foreach (var item in allTiles) {
            item.isMerged = false;
        }
    }

    void Generate() {
        if (emptyTiles.Count > 0) {
            int indexForNewNumber = UnityEngine.Random.Range(0, emptyTiles.Count);
            int randomNum = UnityEngine.Random.Range(0, 10);
            if (randomNum == 0) {
                emptyTiles[indexForNewNumber].Number = 4;
            } else {
                emptyTiles[indexForNewNumber].Number = 2;
            }
            emptyTiles[indexForNewNumber].DoEmergenceAnimation();
            emptyTiles.RemoveAt(indexForNewNumber);
        }
    }

    private void UpdateEmptyTiles() {
        emptyTiles.Clear();
        foreach (var item in allTiles) {
            if (item.Number == 0) {
                emptyTiles.Add(item);
            }
        }
    }

    private bool canMove() {
        if (emptyTiles.Count > 0) {
            return true;
        } else {
            for (int i = 0; i < columns.Count; i++) {
                for (int j = 0; j < rows.Count - 1; j++) {
                    if (allTiles[j, i].Number == allTiles[j + 1, i].Number) {
                        return true;
                    }
                }
            }
            for (int i = 0; i < rows.Count; i++) {
                for (int j = 0; j < columns.Count - 1; j++) {
                    if (allTiles[i, j].Number == allTiles[i, j + 1].Number) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void ChangeStyle() {
        TileStyleHolder.instance.isStyleNum = !TileStyleHolder.instance.isStyleNum;
        for (int i = 0; i < allTiles.GetLength(0); i++) {
            for (int j = 0; j < allTiles.GetLength(1); j++) {
                allTiles[i, j].Number = allTiles[i, j].Number;
            }
        }
    }

    public void ContinueGameButHandler() {
        if (gameOverText.activeSelf) {
            StartNewGameButHandler();
        } else {
            gameOverPanel.SetActive(false);
        }
    }

    private void WonGame() {
        isWin = true;
        state = GameState.GAME_OVER;
        gameOverText.SetActive(false);
        winGameText.SetActive(true);
        gameOverScoreText.text = ScoreManager.instance.Score.ToString();
        gameOverPanel.SetActive(true);
    }

    private void GameOver() {
        state = GameState.GAME_OVER;
        gameOverScoreText.text = ScoreManager.instance.Score.ToString();
        gameOverPanel.SetActive(true);
    }

    public void StartNewGameButHandler() {
        SceneManager.LoadScene("MainScenes");
    }

    public void SaveField() {
        DataTilesUtil.SaveGame(allTiles);
    }

    public void LoadField() {
        //DataTilesUtil.LoadGame(allTiles, FIELD_SIZE);

        //foreach (Tile item in allTiles) {
        //    Debug.Log(item.Number + " " + item.indexRow + " " + item.indexColumn);
        //}
        //for (int i = 0; i < allTiles.GetLength(0); i++) {
        //    for (int j = 0; j < allTiles.GetLength(1); j++) {
        //        allTiles[i, j].Number = allTiles[i, j].Number;
        //    }
        //}
        //Generate();
    }

}
