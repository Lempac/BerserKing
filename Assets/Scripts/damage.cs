using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damageperframe = 5;
    public int hp = 100;
    async void DoDamage(Collision2D enemy)
    {

        if (gameObject.name == "Player" && enemy.gameObject.name == "enemy")

        {
            hp -= damageperframe;
            await Task.Delay(500);
            cooldown = false;
        }
    }
    
    bool cooldown = false;
    private void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (!cooldown)
        {
            cooldown = true;
            DoDamage(collision);
        }
    }
}
