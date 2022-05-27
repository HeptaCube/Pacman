using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public List<Vector2> availableDirections { get; private set; }

    // 이동 가능한 방향을 List Collection으로 만들어줌.
    private void Start()
    {
        this.availableDirections = new List<Vector2>();

        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    // 이용 가능한 방향을 확인하는 메서드.
    private void CheckAvailableDirection(Vector2 direction)
    {
        // BoxCast를 이용해서
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f,
        0.0f, direction, 1.0f, this.obstacleLayer);

        // 앞이 빈 공간이라면 availableDirections라는 List Collection에 추가해줌.
        if (hit.collider == null)
        {
            this.availableDirections.Add(direction);
        }
    }
}

// https://youtu.be/TKt_VlMn_aA?t=9211 까지함.