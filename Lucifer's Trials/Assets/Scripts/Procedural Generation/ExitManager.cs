using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitManager : MonoBehaviour
{

    private int _id;

    void Start()
    {
        _id = int.Parse(this.gameObject.name.Substring(this.gameObject.name.IndexOf("#") + 1));
    }   

    void OnCollisionEnter2D(Collision2D test)
    {

        Debug.Log(_id);

    }

}
