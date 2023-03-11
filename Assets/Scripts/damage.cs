using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    public int damageperframe = 5;
    public int hp = 100;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameObject.name=="Player")
        {
            if (Time.frameCount % 6 == 0)
            {
                hp -= damageperframe;
            }
            
            
        }
    }
}
