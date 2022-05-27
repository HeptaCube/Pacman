using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8.0f;

    protected override void Eat()
    {
        // GameManager에 있는 powerPelletEaten 메서드로 이 클래스를 전달함.
        FindObjectOfType<GameManager>().PowerPelletEaten(this);
    }
}
