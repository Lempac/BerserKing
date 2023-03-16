using System.Threading.Tasks;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damageperframe = 5;
    public int playerHealth;
    public int maxHealth = 100;
    
    public Health health;
    private void Start()
    {
        playerHealth = maxHealth;
        health.SetMaxHealth(maxHealth);
    }
    async void DoDamage(Collision2D enemy)
    {
        
        if (gameObject.name == "Player" && enemy.gameObject.name == "Enemy")
        {
                
            playerHealth -= damageperframe;
            await Task.Delay(500);
            cooldown = false;
            health.SetHealth(playerHealth);
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
