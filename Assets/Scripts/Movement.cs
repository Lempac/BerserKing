using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed = 1f;
<<<<<<< HEAD
=======
    public Animator animator;
    // Update is called once per frame
>>>>>>> 2063117ee1183c20e8e609e156a4763ca2837311
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.GetAxis("Horizontal") * Speed * Time.deltaTime, Input.GetAxis("Vertical") * Speed * Time.deltaTime),ForceMode2D.Impulse);
    }
}
