using UnityEngine;

public class AnimatingObject : MonoBehaviour
{
    void Start()
    {
        var animator = gameObject.GetComponent<Animator>();
        animator.Play("Animation");
        animator.speed = Random.Range(0.75f, 1.0f);
    }

    // Solely for effect-based animating objects
    void EffectFinished()
    {
        Destroy(this.gameObject);
    }
}