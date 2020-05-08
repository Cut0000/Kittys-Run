using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CatController : MonoBehaviour
{
    private Rigidbody2D myRigidBody;
    private Animator myAnim;
    public float catJumpForce = 500f;
    private float catHurtTime = -1;
    private Collider2D myCollider;
    public Text scoreText;
    private float startTime;
    private int jumpsLeft = 2;
    public AudioSource jumpSfx;
    public AudioSource deadthSfx;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("MenuUtama");
        }
        if (catHurtTime == -1)
        {
             if (Input.GetButtonUp("Jump")  && jumpsLeft > 0)
            {
                if (myRigidBody.velocity.y < 0)
                {
                    myRigidBody.velocity = Vector2.zero;
                }

                if (jumpsLeft == 1)
                {
                     myRigidBody.AddForce(transform.up *  catJumpForce * 0.75f);
                }
                else
                {
                    myRigidBody.AddForce(transform.up *  catJumpForce);
                }

                 jumpsLeft--;

                 jumpSfx.Play();

            }

             myAnim.SetFloat("vVelocity", myRigidBody.velocity.y);
             scoreText.text = (Time.time - startTime).ToString("0.0");
        }
        else
        {
            if (Time.time > catHurtTime + 2)
            {
                 Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
              foreach (PrefabSpawner spawner in FindObjectsOfType<PrefabSpawner>())
            {
                spawner.enabled = false;
            }

              foreach (MoveLeft moveLefter in FindObjectsOfType<MoveLeft>())
            {
                moveLefter.enabled = false;
            }
             catHurtTime = Time.time;
             myAnim.SetBool("catHurt", true);
             myRigidBody.velocity = Vector2.zero;
             myRigidBody.AddForce(transform.up * catJumpForce);
             myCollider.enabled = false;

             deadthSfx.Play();

             float currentBestScore = PlayerPrefs.GetFloat("BestScore", 0);
             float currentScore = Time.time - startTime;

             if (currentScore > currentBestScore)
             {
                 PlayerPrefs.SetFloat("BestScore", currentScore);
             }
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            jumpsLeft = 2;
        }
    }
}
