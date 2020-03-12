using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    bool setTarget = false;
    // Start is called before the first frame update
    void Start()
    {
        if (target)
        {
            setTarget = true;
            offset = transform.position - target.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target && !setTarget)
        {
            setTarget = true;
            offset = transform.position - target.position;
        }
        if (!target) {
            setTarget = false;
        }
        transform.position = target.position + offset;
    }
}
