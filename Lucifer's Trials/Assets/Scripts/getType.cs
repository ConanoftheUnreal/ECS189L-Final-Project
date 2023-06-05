using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getType : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var x = this.GetComponents(typeof(Component));
        
        foreach (var c in x)
        {
            Debug.Log(c.GetType());
        }
    }
}
