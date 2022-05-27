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

    // 다음 방향과 시작 방향을 지정할 수 있다.
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.startingPosition = this.transform.position;
    }

    // 시작할 때 상태를 리셋해줌.
    private void Start()
    {
        ResetState();
    }

    // 상태 리셋.
    public void ResetState()
    {
        this.speedMultiplier = 1.0f;
        this.direction = this.initialDirection;
        this.nextDirection = Vector2.zero;
        this.transform.position = this.startingPosition;
        this.rigidbody.isKinematic = false;
        this.enabled = true;
    }

    // 매 프레임마다 방향을 지정해준 방향으로 이동함.
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

    // 이동에 관여하는 메서드.
    // forced 라는 bool 형 데이터가 참이거나, 앞에 블럭이 있지 않은 경우 if문이 실행된다.
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

    // 앞이 블럭으로 막혀 있는 경우 true를 리턴함.
    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.7f,
        0.0f, direction, 1.5f, this.obstacleLayer);


        // void에서는 돌려주는 값이 없기 떄문에 return이 종료를 의미하지만
        // 돌려줄 값이 있다면 그렇지 않다.
        // 추신 : return hit.collider != null; 로 써도 된다.
        return hit.collider;
    }
}
