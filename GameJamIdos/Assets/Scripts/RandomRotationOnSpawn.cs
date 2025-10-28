using UnityEngine;

public class RandomRotationOnSpawn : MonoBehaviour
{
    public bool RandomizeRotation = true;

    void Start()
    {
        if (RandomizeRotation)
        {
            transform.rotation = Random.rotation;
        }
    }
}