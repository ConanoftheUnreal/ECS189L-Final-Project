using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FactoryGoblinBeserker : Factory
{
    [SerializeField] private Polarith.AI.Move.AIMSteeringPerceiver perceiver;
    [SerializeField] private GoblinBeserker prefab;
    [SerializeField] private GameObject enemies;
    private GameObject player;
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

        player = GameObject.FindWithTag("Player");
    }

    public override IEnemy GetEnemy()
    {
        throw new System.NotImplementedException();
    }

    public override IEnemy GetEnemy(Vector3 position)
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");

            if (player == null) { Debug.LogWarning("Player Cannot be found in scene!!"); }
        }

        GameObject instance = Instantiate(prefab.gameObject, position, Quaternion.identity);
        GoblinBeserker newGoblin = instance.GetComponent<GoblinBeserker>();

        Polarith.AI.Move.AIMSteeringFilter tmp = instance.GetComponentInChildren<Polarith.AI.Move.AIMSteeringFilter>();
        if (tmp == null) { Debug.LogWarning("AIMSteeringFilter not found!!"); }
        Debug.Log(tmp);
        tmp.SteeringPerceiver = perceiver;

        float orbit = newGoblin.Stats.Orbit;
        orbit = Random.Range(orbit * .9f, orbit * 1.1f);
        newGoblin.Stats.Orbit = orbit;

        Polarith.AI.Move.AIMOrbit[] orb = instance.GetComponentsInChildren<Polarith.AI.Move.AIMOrbit>();
        orb[0].Orbit.Radius = orbit;
        orb[1].Orbit.Radius = orbit * .75f;

        foreach (Polarith.AI.Move.AIMSeek seek in instance.GetComponentsInChildren<Polarith.AI.Move.AIMSeek>())
        {
            if (seek.Label == "Seek Player" || seek.Label == "Flee Player")
            {
                seek.GameObjects[0] = player;
                continue;
            }
        }

        newGoblin.transform.SetParent(transform, true);

        return newGoblin;
    }

    public override IEnemy GetEnemy(IEnemy enemy)
    {
        throw new System.NotImplementedException();
    }
}
