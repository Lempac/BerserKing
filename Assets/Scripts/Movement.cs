using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed = 1f;
<<<<<<< HEAD
    public Animator animator;
    
=======
    public float HorizontalMove;
    public float VerticalMove;
    private bool IsFacingRight = true;

    private bool CanDash = true;
    private bool IsDashing;
    private float DashingPower = 100f;
    private float DashingTime = 0.75f;
    private float DashingCooldown = 1f;

    [SerializeField] private Animator an;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;

    void Update()
    {
        Flip();

        if (IsDashing)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && CanDash)
        {
            StartCoroutine(Dash());
        }
    }

>>>>>>> c5a05edbde0b54c412b9dd03a37a6e1e97ec455f
    void FixedUpdate()
    {
        HorizontalMove = Input.GetAxis("Horizontal");
        VerticalMove = Input.GetAxis("Vertical");
        GetComponent<Rigidbody2D>().AddForce(new Vector2(HorizontalMove * Speed * Time.deltaTime, VerticalMove * Speed * Time.deltaTime),ForceMode2D.Impulse);
        an.SetFloat("Speed", Mathf.Abs(VerticalMove)+Mathf.Abs(HorizontalMove));

        if (IsDashing)
        {
            return;
        }

    }

    private void Flip()
    {
        if (IsFacingRight && HorizontalMove < 0f || !IsFacingRight && HorizontalMove > 0f)
        {
            IsFacingRight = !IsFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        CanDash = false;
        IsDashing = true;
        an.SetBool("Dashing", true);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * DashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(DashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        IsDashing = false;
        an.SetBool("Dashing", false);
        yield return new WaitForSeconds(DashingCooldown);
        CanDash = true;
    }
}
