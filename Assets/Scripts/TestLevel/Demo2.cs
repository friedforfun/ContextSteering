using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo2 : MonoBehaviour
{
    [SerializeField] private Spawner spawner;

    private void Start()
    {
        spawner.Spawn();
    }
}
