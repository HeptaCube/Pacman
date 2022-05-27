using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }

    public GhostBehavior initialBehavior;
    public Transform target;
    public int points = 200;


    private void Awake()
    {
        this.movement = GetComponent<Movement>();
        this.home = GetComponent<GhostHome>();
        this.scatter = GetComponent<GhostScatter>();
        this.chase = GetComponent<GhostChase>();
        this.frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        // 상태를 리셋하는 메서드를 각자 오브젝트에서 꺼내 실행해줌.
        this.gameObject.SetActive(true);
        this.movement.ResetState();

        this.frightened.Disable();
        this.chase.Disable();
        this.scatter.Enable();


        // 유령이 집에 없으면
        if (this.home != this.initialBehavior)
        {
            this.home.Disable();
        }

        // 유
        if (this.initialBehavior != null )
        {
            this.initialBehavior.Enable();
        }
    }

    // 유령과 플레이어의 collision을 감지하고 frightened(유령 먹기가 가능할 때의 유령 상태)가
    // 켜진 상태면 GhostEaten() 메서드에 "이 스크립트가 가진 오브젝트 / 이 스크립트" 를 전달해줌.
    // 꺼진 상태면 플레이어가 먹힌 것으로 인식함.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            else
            {
                FindObjectOfType<GameManager>().PacmanEaten();
            }
        }
    }
}
