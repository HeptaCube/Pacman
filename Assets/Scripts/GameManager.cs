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

    // 아래 foreach 문에 사용됨.
    public Transform pellet;

    public int ghostMultiplier { get; private set; } = 1;

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
        // 질문할거
        // 스테이지에 있는 알맹이를 다 활성화시킨다.
        foreach (Transform pellet in this.pellet)  {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        ResetGhostMultiplier();
        // 스테이지에 있는 유령들을 다 활성화시킨다.
        for (int i = 0; i < this.ghosts.Length; i++)  {
        this.ghosts[i].ResetState();
        }

        // 플레이어 오브젝트를 활성화시킨다.
        this.pacman.ResetState();
    }
    
    private void GameOver()
    {
        // 스테이지에 있는 유령들을 다 비활성화시킨다.
        for (int i = 0; i < this.ghosts.Length; i++)  {
        this.ghosts[i].gameObject.SetActive(false);
        }
        
        // 플레이어 오브젝트를 비활성화시킨다.
        this.pacman.gameObject.SetActive(false);
        
        // 후에 UI를 표시할 예정.
    }

    private void SetScore(int score)  {
        this.score = score;
    }

    private void SetLives(int lives)  {
        this.lives = lives;
    }
    
    // 귀신 몬스터를 플레이어가 먹으면 점수 상승 단위가 올라감.
    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;
    }

    public void PacmanEaten()
    {
        this.pacman.gameObject.SetActive(false);

        SetLives(this.lives - 1);

        // 생명이 0보다 크다면 3.0(float) 초 후에 ResetState()를 활성화함.
        if (this.lives > 0) {
            Invoke(nameof(ResetState), 3.0f);
        }
        else  {
            GameOver();
        }
    }

    // 알맹이를 먹을 때 알맹이가 사라지게 하고 점수를 더함.
    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);

        // 만약 남은 알맹이가 없다면 새로운 라운드를 시작함.
        if(!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void PowerPelletEaten(PowerPellet powerpellet)
    {
        // 파워 알맹이를 먹었을 때, ????????
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enabled(powerpellet.duration);
        }
        PelletEaten(powerpellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ghostMultiplier), powerpellet.duration);
    }

    // 남은 알맹이가 있다면 true를, 그렇지 않다면 flase를 반환하는 메서드.
    // pellet 오브젝트의 Transform(클래스) 의 gameObject(메서드)에 있는 activeSelf 변수 값을 확인함.
    // 참고 : https://docs.unity3d.com/ScriptReference/Transform.html
    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellet)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    // 파워 알맹이 효과가 끝나면 ghostMultiplier를 1로 되돌린다.
    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}