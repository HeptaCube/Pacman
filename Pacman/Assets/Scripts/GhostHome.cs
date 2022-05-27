using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    public Transform inside;
    public Transform outside;

    private void OnDisable() {
        {
            StartCoroutine(ExitTransition());
            // ExitTransition(); 과 차이가 뭘까요?
        }
    }

    private void OnEnable()
    {
        StopAllCoroutines();
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

        // "집과 집 밖을 전환하는 시간을 나타낸 변수"
        // "시간이 얼마나 흘렀는지 알기 위해 사용할 변수"
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
            Vector3 newPosition = Vector3.Lerp(position, this.inside.position, elapsed / duration);
            newPosition.z = position.z;

            // 유령의 포지션을 위에서 생성한 포지션으로 바꿔치기함.
            this.ghost.transform.position = position;

            elapsed += Time.deltaTime;

            // 한 프레임을 기다리고 계속함. (반복문을 1프레임씩 미룰수 있음.)
            yield return null;

            //elapsed 가 duration보다 많아질 경우 while 문이 멈춤.
        }

        elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(position, outside.position, elapsed / duration);
            newPosition.z = position.z;
            this.ghost.transform.position = position;
            elapsed += Time.deltaTime;
            yield return null;
            //elapsed 가 duration보다 많아질 경우 while 문이 멈춤.
        }

        this.ghost.movement.SetDirection
            (new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, 0.0f), true);
        this.ghost.movement.rigidbody.isKinematic = false;
        this.ghost.movement.enabled = true;
    }
}