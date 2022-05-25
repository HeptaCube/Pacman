using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    // 범위 또한 [Range(0f, 1.0f)]로 설정할 수 있다.
    [Header("Slots to join Object")]
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    // 에디터에서 설정은 불가능하지만 (   ) 할수 있는 public 변수 생성.
    public int score { get; private set; }
    public int lives { get; private set; }

    private void Start()  {
        NewGame();

        // bool a = Temp();
        // print(a);
    }

    // private bool Temp()
    // {
    //     return false;
    // }

    private void Update()
    {
        if (this.lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    private void NewGame()  {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()  {
        // 스테이지에 있는 알맹이를 다 활성화시킨다.
        foreach (Transform pellet in this.pellets)  {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        // 스테이지에 있는 유령들을 다 활성화시킨다.
        for (int i = 0; i < this.ghosts.Length; i++)  {
        this.ghosts[i].gameObject.SetActive(true);
        }

        // 플레이어 오브젝트를 활성화시킨다.
        this.pacman.gameObject.SetActive(true);
    }
    
    private void GameOver()
    {
        // 스테이지에 있는 유령들을 다 비활성화시킨다.
        for (int i = 0; i < this.ghosts.Length; i++)  {
        this.ghosts[i].gameObject.SetActive(false);
        }
        
        // 플레이어 오브젝트를 활성화시킨다.
        this.pacman.gameObject.SetActive(true);

        // 후에 UI를 표시할 예정.
    }

    private void SetScore(int score)  {
        this.score = score;
    }

    private void SetLives(int lives)  {
        this.lives = lives;
    }
    
    public void GhostEaten(Ghost ghost)
    {
        SetScore(this.score + ghost.points);
    }

    public void PacmanEaten()
    {
        this.pacman.gameObject.SetActive(false);

        SetLives(this.lives - 1);

        if (this.lives > 0) {
            Invoke(nameof(ResetState), 3.0f);
        }
        else  {
            GameOver();
        }
    }
}