using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;

    public bool eaten { get; private set;}

    // 활성화 됐을 때 blue를 제외한 다른 스프라이트를 꺼준다.
    public virtual void Enabled(float duration)
    {
        base.Enable(duration);
        this.body.enabled = false;
        this.eyes.enabled = false;
        this.blue.enabled = true;
        this.white.enabled = false;

        // duration의 1/2 속도로 깜빡임.
        Invoke(nameof(Flash), duration / 2.0f);
    }

    public override void Disable()
    {
        base.Disable();
        this.body.enabled = true;
        this.eyes.enabled = true;
        this.blue.enabled = false;
        this.white.enabled = false;        
    }


    private void Flash()
    {
        // 먹힌 것이 맞다면 흰색을 키고 AnimatedSprite 스크립트를 재시작함.
        if (!this.eaten)
        {
            this.blue.enabled = false;
            this.white.enabled = true;
            this.white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    private void Eaten()
    {
        this.eaten = true;
        Vector3 position = this.ghost.home.inside.position;
        position.z = this.ghost.transform.position.z;
        this.ghost.transform.position = position;

        this.ghost.home.Enable(this.duration);

        this.body.enabled = false;
        this.eyes.enabled = true;
        this.blue.enabled = false;
        this.white.enabled = false;   
    }

    private void OnEnable()
    {
        this.ghost.movement.speedMultiplier = 0.5f;
        this.eaten = false;
    }

    private void OnDisable()
    {
        this.ghost.movement.speedMultiplier = 1.0f;
        this.eaten = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.enabled)
            {
                Eaten();
            }
        }
    }

    // 자세한 설명은 GhostChase.cs에 있음.
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, 
                availableDirection.y, 0.0f);
                float distanceBetween = (this.ghost.target.position - newPosition).sqrMagnitude;

                if (distanceBetween > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distanceBetween;
                }
            }

            this.ghost.movement.SetDirection(direction);
        }
    }
}
