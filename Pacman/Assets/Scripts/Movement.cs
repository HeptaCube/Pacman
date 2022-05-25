using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 8.0f;
    public float speedMultiplier = 1.0f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;

    // new 키워드로 유니티에서 제공하는 잘 안쓰는 rigidbody 변수 이름을 덮어씌울수 있다. (오류가 안뜸!!)
    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction { get; private set; }
    // 
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.startingPosition = this.transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        this.speedMultiplier = 1.0f;
        this.direction = this.initialDirection;
        this.nextDirection = Vector2.zero;
        this.transform.position = this.startingPosition;
        this.rigidbody.isKinematic = false;
        this.enabled = true;
    }

    private void Update() {
        if (this.nextDirection != Vector2.zero)
        {
            SetDirection(this.nextDirection);
        }
    }

    private void FixedUpdate() {
        Vector2 position = this.rigidbody.position;
        // FixedUpdate가 Update에서 프레임이 다를 경우에 생기는 문제를 해결해준다면 Time.fixedDeltaTime는 왜 쓰나요?
        Vector2 translation = this.direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime;

        this.rigidbody.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if (forced || !Occupied(direction))
        {
            // 실제 이동을 허용하는 대신 이동 예정인 방향을 설정하지 않는다.
            this.direction = direction;
            this.nextDirection = Vector2.zero;
        }
        else
        {
            // 이동할 예정인 방향을 미리 설정한다.
            this.nextDirection = direction;
        }
    }

    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.7f, 0.0f, direction, 1.5f, this.obstacleLayer);


        // void에서는 돌려주는 값이 없기 떄문에 return이 종료를 의미하지만
        // 돌려줄 값이 있다면 그렇지 않다.
        return hit.collider;
        // return hit.collider != null; 로 써도 된다.
    }
}
