using UnityEngine;

public class EffectObject : MonoBehaviour
{
    void EffectFinished()
    {
        Destroy(this.gameObject);
    }
}