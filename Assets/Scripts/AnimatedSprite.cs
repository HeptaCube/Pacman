using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    // 관련 내용 : 캡슐화(encapsulation)

    // 이 클래스에서만 관리되는 변수이므로 setter를 private로 지정해줌.
    // 하지만 다른 클래스에서 GetComponent를 매번 써주지 않고 간편하게 이용하기 위해
    // getter는 public으로 지정해줌.

    // 다른 클래스에서 사용할 때 -> "public AnimatedSprite <변수이름>;"
    // 이때 public 인 경우에만 사용이 가능함.
    // 그 후에 "<변수이름>.spriteRenderer.<내용>" 와 같은 식으로 접근이 가능함.

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
        // 애니메이션이 돌아가는 메서드의 반복을 시작한다.
        InvokeRepeating(nameof(Advance), this.animationTime, this.animationTime);
    }

    private void Advance()
    {
        // if 값이 false면 return한다.
        if(!this.spriteRenderer.enabled)  {
            return;
        }

        this.animationFrame++;

        // "애니메이션 프레임이 최대길이보다 같거나 많은" 상태이고 "loop가 true" 라면
        if (this.animationFrame >= this.sprites.Length && this.loop)
        {
            // 애니메이션의 프레임을 0으로 되돌림.
            this.animationFrame = 0;
        }

        // "애니메이션 프레임이 0 이상"이고 "상태이고 애니메이션 프레임이 스프라이트 길이 미만"인 경우
        // 스프라이트가 동작하게 만든다.
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

    // "And the reason why I made it a public getter but a private setter is because it's possible that, from
    // other scripts, you might reference the animated sprite. And if you wanted to reference the sprite render,
    // it just makes it easy rather than having to get component all the time. So you'll see me do this
    // a lot more throughout the remainder of this."
    // Vide Link : https://youtu.be/TKt_VlMn_aA?t=3073 (51m 13s part)