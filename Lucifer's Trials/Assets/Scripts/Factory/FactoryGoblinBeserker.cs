using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FactoryGoblinBeserker : Factory
{
    [SerializeField] private Polarith.AI.Move.AIMSteeringPerceiver perceiver;
    [SerializeField] private GoblinBeserker prefab;
    [SerializeField] private GameObject enemies;
    private float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (perceiver == null)
        {
            Debug.LogWarning("AIM Steering Perceiver not found!!!");
        }

        if (prefab == null)
        {
            Debug.LogWarning("EnemyPrefab Not Set");
        }
    }

    public override IEnemy GetEnemy()
    {
        throw new System.NotImplementedException();
    }

    public override IEnemy GetEnemy(Vector3 position)
    {
        GameObject instance = Instantiate(prefab.gameObject, position, Quaternion.identity);
        GoblinBeserker newGoblin = instance.GetComponent<GoblinBeserker>();

        Polarith.AI.Move.AIMSteeringFilter tmp = instance.GetComponentInChildren<Polarith.AI.Move.AIMSteeringFilter>();
        if (tmp == null) { Debug.LogWarning("help"); }
        Debug.Log(tmp);
        tmp.SteeringPerceiver = perceiver;

        float orbit = newGoblin.Stats.Orbit;
        orbit = Random.Range(orbit * .9f, orbit * 1.1f);
        newGoblin.Stats.Orbit = orbit; 

        return newGoblin;
    }

    public override IEnemy GetEnemy(IEnemy enemy)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (time > 5f)
        {
            GoblinBeserker tmp = (GoblinBeserker) GetEnemy(Vector3.zero);

            GameObject t = tmp.gameObject;
            t.transform.SetParent(enemies.transform, true);
            t.SetActive(true);
            time = 0;
        }

        time += Time.deltaTime;
    }
}
