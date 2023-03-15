using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public AudioClip speedtakesound;
    public float Speed = 1f;
    public Animator animator;
    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.GetAxis("Horizontal") * Speed * Time.deltaTime, Input.GetAxis("Vertical") * Speed * Time.deltaTime),ForceMode2D.Impulse);
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if(collision.name == "speed")
        {
            Speed += 10f;
            AudioSource.PlayClipAtPoint(speedtakesound,transform.position, .5f);
           /* collision.gameObject.SetActive(false);*/
            collision.gameObject.GetComponent<speed>().respawn(gameObject);
        }
        

    }
}
