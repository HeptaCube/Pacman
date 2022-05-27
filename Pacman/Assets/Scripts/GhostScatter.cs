using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScatter : GhostBehavior
{
    private void OnDisable()
    {
        this.ghost.chase.Enable();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // "빈 공간"이고 "이 스크립트가 활성화 된 상태"이고 "this.ghost.frightened가 비활성화된 상태" 라면
        if (node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            // "0~이동 가능한 방향 수" 사이의 랜덤값이 들어가는 변수 생성.
            int index = Random.Range(0, node.availableDirections.Count);

            // "index 변수 번째의 이용 가능한 방향"이 "유령이 이동중인 방향"과 반대 방향"이고
            // "갈수 있는 길이 1개 초과"라면 (안전장치 설정)
            if (node.availableDirections[index] == -this.ghost.movement.direction
                && node.availableDirections.Count > 1)
            {
                // 인덱스를 하나 더함 ())
                index++;

                // 인덱스가 이동 가능한 방향 수보다 많다면
                if (index >= node.availableDirections.Count)
                {
                    // 인덱스를 0으로 초기화함. (같은 방향을 또 선택함을 방지하기 위해)
                    index = 0;
                }
            }
            
            // 유령을 실제로 이동하게 만듬.
            this.ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }
}
