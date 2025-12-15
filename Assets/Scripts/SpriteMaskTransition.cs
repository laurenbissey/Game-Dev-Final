using UnityEngine;

public class SpriteMaskTransition : MonoBehaviour
{
    [SerializeField] private float maxScaleFactor = 1f;
    [SerializeField] private float duration = 1f;

    private Vector3 initialScale;
    private bool spriteRevealed = false;
    private float timer = 0f;

    private void Update()
    {
        if (spriteRevealed)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            transform.localScale = Vector3.Lerp(initialScale, Vector2.one * maxScaleFactor, t);
        }
    }

    public void Reveal()
    {
        initialScale = new Vector2(0.01f, 0.01f);

        spriteRevealed = true;
        timer = 0f;
    }
}
