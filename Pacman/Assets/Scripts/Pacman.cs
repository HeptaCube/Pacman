using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    public Movement movement { get; private set; }

    private void Awake()
    {
        this.movement = GetComponent<Movement>();
    }

    // 키를 인식하여 방향을 전환함 + 플레이어가 바라보는 방향을 정함.
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.movement.SetDirection(Vector2.right);
        }

        // 각도를 "Mathf.Atan2(float.y, float.x)"로 계산할 수 있다.
        // "Mathf.Atan2"의 반환값이 라디안이기 때문에 도수법을 사용하기 위해 "Mathf.Rad2Deg"을 이용한다.
        // 참고 자료 : https://blog.naver.com/sang9151/220821255191
        float angle = Mathf.Atan2(this.movement.direction.y, this.movement.direction.x);

        // rotation은 보통 quaternion값을 가진다.
        this.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
}
