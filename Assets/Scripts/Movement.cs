using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.GetAxis("Horizontal") * Speed * Time.deltaTime, Input.GetAxis("Vertical") * Speed * Time.deltaTime));
    }
}
