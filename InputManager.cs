using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection {
    LEFT, RIGHT, UP, DOWN
}

public class InputManager : MonoBehaviour {

    private GameManager gameManager;

    private void Awake() {
        gameManager = GetComponent<GameManager>(); //  GameObject.FindObjectOfType<GameManager>(); // 
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            //rightMove();
            gameManager.Move(MoveDirection.RIGHT);
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            gameManager.Move(MoveDirection.LEFT);
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            gameManager.Move(MoveDirection.DOWN);
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            gameManager.Move(MoveDirection.UP);
        }
    }
}
