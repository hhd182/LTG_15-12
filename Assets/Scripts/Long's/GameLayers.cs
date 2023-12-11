using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask solidLayer;

    public static GameLayers Instance { get; set; }

    public LayerMask PlayerLayer { get { return playerLayer; } }
    public LayerMask SolidLayer { get { return solidLayer; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }
}
