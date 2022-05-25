using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    // "And the reason why I made it a public getter but a private setter is because it's possible that, from
    // other scripts, you might reference the animated sprite. And if you wanted to reference the sprite render,
    // it just makes it easy rather than having to get component all the time. So you'll see me do this
    // a lot more throughout the remainder of this."
    // Vide Link : https://youtu.be/TKt_VlMn_aA?t=3073 (51m 13s part)

    public Sprite[] sprites;
    public float animationTime = 0.25f;
    public int animationFrame { get; private set; }

    public bool loop = true;

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), this.animationTime, this.animationTime);
    }

    private void Advance()
    {
        // if 값이 false면 return한다.
        if(!this.spriteRenderer.enabled)  {
            return;
        }

        this.animationFrame++;

        if (this.animationFrame >= this.sprites.Length && this.loop)
        {
            this.animationFrame = 0;
        }

        if (this.animationFrame >= 0 && this.animationFrame < this.sprites.Length)  {
            this.spriteRenderer.sprite = this.sprites[this.animationFrame];
        }
    }
    
    public void Restart()
    {
        // 이 값을 0으로 하면 Advance() 메서드에서 this.animationFrame++ 가 적용되므로
        // 0이 아니라 1부터 시작하게 된다. 그렇기 때문에 -1로 초기화한다.
        this.animationFrame = -1;
        Advance();
    }
}
