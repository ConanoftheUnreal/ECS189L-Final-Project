using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Factory : MonoBehaviour
{
    public abstract IEnemy GetEnemy();
    public abstract IEnemy GetEnemy(Vector3 position);
    public abstract IEnemy GetEnemy(IEnemy enemy);

}
