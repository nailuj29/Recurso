using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Move {
    up, down, left, right
}

public class GameManager : MonoBehaviour {

    public LevelData[] levels;
    public LevelData level;
    public Queue<Move> moves = new Queue<Move>();

    public GameObject canvas;
    public Image arrow;
    public TextMeshProUGUI levelNumber, moveNumber, loopsNumber;
    public int arrowY = -300;
    public int arrowStartX = -575;
    public int arrowOffset = 125;

    public bool locked = false;

    public GameObject tileObject;
    public GameObject goal;

    public GameObject player;

    GameObject arrows;
    GameObject levelElements;
    private int levelNum = 0;

    Dictionary<Move, int> rotations = new Dictionary<Move, int>() {
        { Move.up, 0 },
        { Move.left, 90 },
        { Move.down, 180 },
        { Move.right, 270 }
    };

    private void Start() {
        levelElements = new GameObject("level");
        levelElements.transform.position = new Vector3(0, 0, 0);
        level = levels[levelNum];
        foreach (Vector3 tile in level.tiles) {
            GameObject gameObject = Instantiate(tileObject);

            gameObject.transform.position = tile;
            gameObject.transform.SetParent(levelElements.transform);
        }

        GameObject go = Instantiate(goal);
        go.transform.position = level.goal;
        go.transform.SetParent(levelElements.transform);

        Instantiate(player);
        Player.Instance.transform.position = level.playerStart;
        Player.Instance.managerObject = gameObject;

        levelNumber.text = $"Level {levelNum + 1}";
        moveNumber.text = $"{level.maxMoves} Moves";
        loopsNumber.text = $"{level.loops} Loops";
    }


    // Update is called once per frame
    void Update() {
        //print("In update");
        if (!locked) {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                AddMove(Move.up);
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                AddMove(Move.down);
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                AddMove(Move.left);
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                AddMove(Move.right);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(Player.Instance.Execute(level.loops, moves));
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            ResetQueue();
        }
    }

    public void ResetQueue() {
        moves.Clear();
        Destroy(arrows);
    }

    void AddMove(Move move) { 
        if (moves.Count == level.maxMoves) {
            moves.Dequeue();
        }

        print("Adding move...");

        moves.Enqueue(move);
        Destroy(arrows);
        arrows = new GameObject("Arrows");
        arrows.transform.parent = canvas.transform;
        int currentX = arrowStartX;
        foreach (var moveInQueue in moves) {
            print("Adding arrow");
            Image arrw = Instantiate(arrow);
            arrw.transform.parent = arrows.transform;
            arrw.rectTransform.position = new Vector3(currentX, arrowY);
            arrw.rectTransform.rotation = Quaternion.Euler(0, 0, rotations[moveInQueue]);
            currentX += arrowOffset;
        }
    }

    public void NextLevel() {
        Destroy(levelElements);
        levelNum++;

        levelElements = new GameObject("level");
        levelElements.transform.position = new Vector3(0, 0, 0);
        try {
            level = levels[levelNum];
        } catch (IndexOutOfRangeException) {
            print("IOORE");
            Destroy(Player.Instance.gameObject);
            levelNumber.text = "You Win!";
            Destroy(moveNumber.gameObject);
            Destroy(loopsNumber.gameObject);
            return;
        }
        foreach (Vector3 tile in level.tiles) {
            GameObject gameObject = Instantiate(tileObject);

            gameObject.transform.position = tile;
            gameObject.transform.SetParent(levelElements.transform);
        }

        GameObject go = Instantiate(goal);
        go.transform.position = level.goal;
        go.transform.SetParent(levelElements.transform);

        Player.Instance.transform.position = level.playerStart;
        Player.Instance.level = level;
        if (levelNum >= levels.Count()) {
            Destroy(Player.Instance.gameObject);
            levelNumber.text = "You Win!";
            Destroy(moveNumber.gameObject);
            Destroy(moveNumber.gameObject);
        }
        levelNumber.text = $"Level {levelNum + 1}";
        moveNumber.text = $"{level.maxMoves} Moves";
        loopsNumber.text = $"{level.loops} Loops";
    }
}
