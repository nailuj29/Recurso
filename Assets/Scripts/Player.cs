using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Singleton<Player> {

    //float startX;
    //float startY;

    public LevelData level;
 
    GameManager manager;
    public GameObject managerObject;
    private void Start() {
        //startX = transform.position.x; 
        //startY = transform.position.y;

        manager = managerObject.GetComponent<GameManager>();
    }


    public IEnumerator Execute(int times, Queue<Move> moves) {
        manager.locked = true;
        if (times == 0) {
            manager.locked = false;
            Die();
            yield break; 
        }
        print("Execute()");
        Vector3 pos = transform.position;
        foreach (Move move in moves) {
            switch (move) {
                case Move.up:
                    pos.y += 1;
                    break;

                case Move.down:
                    pos.y -= 1;
                    break;

                case Move.left:
                    pos.x -= 1;
                    break;

                case Move.right:
                    pos.x += 1;
                    break;

            }

            if (level.goal.Equals(pos)) {
                Win();
                yield break;
            }
            if (!level.tiles.Contains(pos)) {
                manager.locked = false;
                Die();
                yield break;
            }
            transform.position = pos;
            yield return new WaitForSeconds(0.5f);
        }

        // It'd be wrong not to use recursion. ;)
        yield return StartCoroutine(Execute(times - 1, moves));
    }

    public void Die() {
        manager.ResetQueue();
        transform.position = level.playerStart;
    }

    public void Win() {
        manager.ResetQueue();
        manager.NextLevel();
        manager.locked = false;
    }
}
