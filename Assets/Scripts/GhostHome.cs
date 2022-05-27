using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    public Transform inside;
    public Transform outside;

    private void OnEnable()
    {

        StopAllCoroutines();
    }


    private void OnDisable()
    {
        if (this.gameObject.activeSelf)
        {
            StartCoroutine(ExitTransition());
            // ExitTransition(); 과 같이 단 한번의 호출로 코루틴의 기능을 구현할 수 없다.
            // StartCoroutine 함수로 유니티에 해당 이뉴머블을 등록해주면 유니티가 매 프레임마다 자동으로
            // 실행시켜 주는 구조이다. 고로 그냥 호출하는 것과 차이가 있다.
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            this.ghost.movement.SetDirection(-this.ghost.movement.direction);
        }
    }

    // Coroutine 메서드.
    private IEnumerator ExitTransition()
    {
        // 이동하는 방향을 설정해줌. forced 변수(후자)가 켜져있으면 앞에 블럭이 있어도 무시하고 간다.
        this.ghost.movement.SetDirection(Vector2.up, true);
        this.ghost.movement.rigidbody.isKinematic = true;
        this.ghost.movement.enabled = false;

        // 현재 포지션을 position 변수로 설정함.
        Vector3 position = this.transform.position;

        // "집과 집 밖을 전환하는 시간을 나타낸 변수" (최대 시간)
        // "시간이 얼마나 흘렀는지 알기 위해 사용할 변수" (경과 시간)
        float duration = 0.5f;
        float elapsed = 0.0f;

        // "집과 집 밖을 전환하는 시간"이 "흐른 시간"보다 적은 경우
        while (elapsed < duration)
        {
            // "보간할 값 1", "보간할 값 2"를 "보간할 때 쓰이는 값"으로 보간함.
            // 공식 : Vector.Lerp(a, b, t) = a + (b - a) * t
            // t는 0~1 사이로 고정됨.
            // 참고 자료 : https://artiper.tistory.com/110
            // * t는 0.1, 0.2 ... 0.9 순서로 변화함.
            // LerpUnclamped 으로 Clamp를 할수도 있으나 Lerp 함수의 경우에는 기본적으로 Clamp가 돼 있음.
            Vector3 newPosition = Vector3.Lerp(position, this.inside.position, elapsed / duration);
            newPosition.z = position.z;

            // 유령의 포지션을 위에서 생성한 포지션으로 바꿔치기함.
            this.ghost.transform.position = newPosition;

            elapsed += Time.deltaTime;

            // 한 프레임을 기다리고 계속함. (반복문을 1프레임씩 미룰수 있음.)
            yield return null;

            //elapsed 가 duration보다 많아질 경우 while 문이 멈춤.
        }

        elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(this.inside.position, this.outside.position, elapsed / duration);
            newPosition.z = position.z;
            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        this.ghost.movement.SetDirection
            (new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, 0.0f), true);
        this.ghost.movement.rigidbody.isKinematic = false;
        this.ghost.movement.enabled = true;
    }
}

