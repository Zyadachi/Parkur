using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public GameObject player;
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    float movementFactor;
    [SerializeField] float period = 2f;
    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (player.transform.parent == null && player.GetComponent<Collider>().bounds.Intersects(GetComponent<Collider>().bounds))
        {
            player.transform.parent = transform;
        }
        if (player.transform.parent == transform && !player.GetComponent<Collider>().bounds.Intersects(GetComponent<Collider>().bounds))
        {
            player.transform.parent = null;
        }
        if (period <= Mathf.Epsilon){return; }
        float cycles = Time.time / period;
        
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau); // going from -1 to 1

        movementFactor = (rawSinWave + 1f) /2f; //recalculated to go from 0 to 1

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
