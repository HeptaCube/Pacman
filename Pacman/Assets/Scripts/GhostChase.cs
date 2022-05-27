using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChase : GhostBehavior
{
    private void OnDisable()
    {
        this.ghost.scatter.Enable();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // "앞이 빈 공간"이고 "이 스크립트가 활성화 된 상태"이고 "this.ghost.frightened가 비활성화된 상태" 라면
        if (node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;

            // 최대값, 최소값
            // int.MaxValue; (2147483647)
            // float.MinValue; (-3.402823E+38)

            // 처음에는 아래 if문을 통과할 수 있도록 최대값으로 지정해줌.
            float minDistance = float.MaxValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // newPosition에 "현재 유령의 포지션 + foreach문에서 생성한 x, y 방향"을 넣음.
                // 고로 newPosition은 갈 예정인 포지션이 됨.
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, 
                availableDirection.y, 0.0f);

                // sqrMagnitude를 통해 newPosition(유령의 위치)과 갈 예정인 위치(= 플레이어 위치이자 newPosition)의 벡터 길이에 제곱한 값을 구할수 있음.
                // 참고 : 그냥 magnitude는 square root라는 것을 쓰는데 이것이 매우 느리고 성능이 안좋기 때문에 sqrMagnitude를 사용하였음.
                float distanceBetween = (this.ghost.target.position - newPosition).sqrMagnitude;

                // 위에서 구한 distanceBetween이 minDistance보다 작다면
                if (distanceBetween < minDistance)
                {
                    // 방향을 바꿀 수 있도록 함.
                    direction = availableDirection;

                    // 지금 계산한 거리가 가장 짧은 거리보다 짧다면 , 새로 계산한 거리를 가장 짧은 거리로 갱신하기 위해
                    // minDistance이 distanceBetween와 같도록 초기화를 해줌.
                    
                    // 또 다시 위 과정을 반복하기 위해 minDistance를 "newPosition과 플레이어의 벡터 거리" 로 지정함.
                    minDistance = distanceBetween;
                }
            }

            this.ghost.movement.SetDirection(direction);
        }
    }
}

