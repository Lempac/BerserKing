using UnityEngine;

public class Speed : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Item speed;
    [SerializeField] private AudioClip speedtakesound;
    [SerializeField] private float speedModifyer;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if (collider.gameObject != player) return;
        MenuHandeler.Instance.ShowItemMenu(speed);
        collider.GetComponent<Movement>().Speed += speedModifyer;
        AudioSource.PlayClipAtPoint(speedtakesound, transform.position, .5f);
        transform.position = new Vector3(Random.Range(1, 6), Random.Range(1, 6));
    }
}
