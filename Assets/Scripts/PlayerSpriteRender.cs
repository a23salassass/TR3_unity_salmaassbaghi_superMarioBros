using UnityEngine;

public class PlayerSpriteRender : MonoBehaviour
{
    private PlayerController movement;
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public AnimatedSprite run;

private void Awake()
{
    movement = GetComponentInParent<PlayerController>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    run = GetComponent<AnimatedSprite>(); // Esto es nuevo

}
    private void LateUpdate()
    {
        run.enabled = movement.running;

        if (movement.jumping) {
            spriteRenderer.sprite = jump;
        } else if (movement.sliding) {
            spriteRenderer.sprite = slide;
        } else if (!movement.running) {
            spriteRenderer.sprite = idle;
        }
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        run.enabled = false;
    }

}
