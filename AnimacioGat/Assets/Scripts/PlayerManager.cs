using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public float speedX;
	public float jumpSpeedY;
	public float delayBeforeDoubleJump;
	public GameObject leftBullet, rightBullet;


	bool facingRight, Jumping, isGrounded, canDoubleJump;
	float speed;
	Transform firePos;

	Animator anim;
	Rigidbody2D rb;

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		facingRight = true;

		firePos = transform.Find ("firePos");
	}
	
	// Update is called once per frame
	void Update ()
	{
		// player movement

		MovePlayer (speed);

		HandleJumpandFall ();

		Flip ();


		//left player movement

		if (Input.GetKeyDown (KeyCode.J)) 
		{
			speed = -speedX;
		}

		if (Input.GetKeyUp (KeyCode.J)) 
		{
			speed = 0;
		}		

		//right player movement

		if (Input.GetKeyDown (KeyCode.L)) 
		{
			speed = speedX;
		}

		if (Input.GetKeyUp (KeyCode.L)) 
		{
			speed = 0;
		}	

		//jump player movement

		if (Input.GetKeyDown (KeyCode.I))
		{
			Jump ();
		}


		// Passar de Idle a Walking i l'inversa

		if (Input.GetKeyDown (KeyCode.W)) 
		{
			anim.SetInteger ("State", 1);
		} 
		if (Input.GetKeyUp (KeyCode.W))
		{
			anim.SetInteger ("State", 0);
		}

		// Passar de Idle a Running i l'inversa

		if (Input.GetKeyDown (KeyCode.R)) 
		{
			anim.SetInteger ("State", 2);
		}

		if (Input.GetKeyUp (KeyCode.R)) 
		{
			anim.SetInteger ("State", 0);
		}

		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			Fire ();
		}

	}

	void MovePlayer (float playerSpeed)
	{
		// code for player movement

		if (playerSpeed < 0 && !Jumping || playerSpeed > 0 && !Jumping) 
		{
			anim.SetInteger ("State", 2);
		}

		if (playerSpeed == 0 && !Jumping) 
		{
			anim.SetInteger ("State", 0);
		}

		rb.velocity = new Vector3 (speed, rb.velocity.y, 0);
	}


	void HandleJumpandFall ()
	{
		if (Jumping) 
		{
			if (rb.velocity.y> 0)
			{
				anim.SetInteger ("State", 3);
			}
			else
			{
				anim.SetInteger ("State", 1);
			}
		}
	}


	void Flip ()
	{
		// code for change direction of the player
		if (speed > 0 && !facingRight || speed < 0 && facingRight)
		{
			facingRight = !facingRight;
			Vector3 temp = transform.localScale;
			temp.x *= -1;
			transform.localScale = temp;
		}
	}


	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "GROUND") 
		{
			isGrounded = true;
			canDoubleJump = false;
			Jumping = false;
			anim.SetInteger ("State", 0);
		}
	}

	public void Jump ()
	{
		//single jump
		if (isGrounded)
		{
			Jumping = true;
			isGrounded = false;
			rb.AddForce(new Vector2(rb.velocity.x, jumpSpeedY));
			anim.SetInteger ("State", 3);
			Invoke ("EnableDoubleJump", delayBeforeDoubleJump);
		}

		//double jump
		if (canDoubleJump) 
		{
			canDoubleJump = false;
			rb.AddForce(new Vector2(rb.velocity.x, jumpSpeedY));
			anim.SetInteger ("State", 3);

		}
		
	}

	void EnableDoubleJump ()
	{
		canDoubleJump = true;
	}

	public void Fire()
	{
		if (facingRight) 
		{
			Instantiate (rightBullet, firePos.position, Quaternion.identity);
		}

		if (!facingRight) 
		{
			Instantiate (leftBullet, firePos.position, Quaternion.identity);
		}

	}

	public void WalkLeft ()
	{
		speed = -speedX;
	}

	public void WalkRight ()
	{
		speed = speedX;
	}

	public void StopMoving ()
	{
		speed = 0;
	}

}
