using UnityEngine;


public class Attractor : MonoBehaviour 
{

    public float noiseScale = 1f;
    public float placementThreshold = 0.5f;
    public float prefabRandomValue = 0.5f;

    public bool Active = false;
    public float ActivatedTime = 10;
    public float AttractStrength = 1;

    public bool IsFood = false;

}
