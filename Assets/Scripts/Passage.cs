using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passage : MonoBehaviour
{
    public Transform connection;
    
    // 반대편으로 이동하는 통로가 작동하게 해줌.
    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 position = other.transform.position;
        position.x = this.connection.position.x;
        position.y = this.connection.position.y;

        // other.transform.position = this.connection.position;
        // 라고 쓰면 z 값도 바뀌기 때문에 따로 변수를 생성함.
        other.transform.position = position;
    }
}
