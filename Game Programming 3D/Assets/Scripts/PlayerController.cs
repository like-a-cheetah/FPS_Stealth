using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Photon.Pun;

[RequireComponent (typeof (Animator))]
public class PlayerController : MonoBehaviour {
	public Transform rightGunBone;
	public Transform leftGunBone;
	public Arsenal[] arsenal;

    public PlayerGun gun;

    //private bool canMove;

	private Animator animator;

    public bool canSee;

    public SphereCollider walk;
    public SphereCollider run;
    public SphereCollider shot;

    public Camera aimingCamera;
    public Camera basicCamera;

    float horizontal;
    float vertical;

    public Canvas escPanel;

    //public PhotonView PV;

    public GameObject playerSight;

    void Awake()
    {
        animator = GetComponent<Animator>();

        //PV = GetComponent<PhotonView>();
        //spine = animator.GetBoneTransform(HumanBodyBones.Spine);
        //canMove = true;
		if (arsenal.Length > 0)
			SetArsenal (arsenal[0].name);
    }

	public void SetArsenal(string name) {
		foreach (Arsenal hand in arsenal) {
			if (hand.name == name) {
				if (rightGunBone.childCount > 0)
					Destroy(rightGunBone.GetChild(0).gameObject);
				if (leftGunBone.childCount > 0)
					Destroy(leftGunBone.GetChild(0).gameObject);
				if (hand.rightGun != null) {
					GameObject newRightGun = (GameObject) Instantiate(hand.rightGun);
					newRightGun.transform.parent = rightGunBone;
					newRightGun.transform.localPosition = Vector3.zero;
					newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
					}
				if (hand.leftGun != null) {
					GameObject newLeftGun = (GameObject) Instantiate(hand.leftGun);
					newLeftGun.transform.parent = leftGunBone;
					newLeftGun.transform.localPosition = Vector3.zero;
					newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
				}
				animator.runtimeAnimatorController = hand.controller;
				return;
				}
		}
	}

    private void Update()
    {
        if (escPanel.enabled)
            return;
        //if (PV.IsMine)
        //{
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Aim_Walk")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Aim_Jump"))
        {
            walk.enabled = true;
            run.enabled = false;
            shot.enabled = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Aim"))
        {
            walk.enabled = false;
            run.enabled = true;
            shot.enabled = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fire")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Fire_Walk")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Fire_Jump")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Fire_Wary"))
        {
            walk.enabled = false;
            run.enabled = false;
            shot.enabled = true;
        }
        else
        {
            walk.enabled = false;
            run.enabled = false;
            shot.enabled = false;
        }
        //}
    }

    void FixedUpdate()
    {
        //if(PV.IsMine)
            Move(horizontal, vertical);
    }

    void Move(float horizontal, float vertical)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 goal = new Vector3(horizontal, 0, vertical);

        if (Input.GetMouseButton(1))
        {
            basicCamera.enabled = false;
            aimingCamera.enabled = true;
        }
        else
        {
            basicCamera.enabled = true;
            aimingCamera.enabled = false;
        }

        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftControl)) && !escPanel.enabled)
        {
            animator.SetBool("Isshooting", true);

            gun.Shoot();
        }
        else
        {
            animator.SetBool("Isshooting", false);
        }

        if (Input.GetKeyDown(KeyCode.C)
            && !Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(0))
        {
            Sitting();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Aim_Jump")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Aim_Jump"))
        {
            Jump();

            rb.AddRelativeForce(new Vector3(0, 6f, 0), ForceMode.Impulse);
        }

        if ((horizontal != 0 || vertical != 0)
            && !(animator.GetBool("Squat") && Input.GetMouseButton(0)))
        {
            //basicCamera.transform.localPosition = new Vector3(0.05f, 1.074f, -0.8569999f);
            if (Input.GetKey(KeyCode.LeftShift) && !animator.GetBool("Isshooting"))
            {
                Debug.Log("1");
                animator.SetBool("Squat", false);
                animator.SetBool("Ismove", true);
                animator.SetBool("Isrun", true);

                Vector3 hor = transform.right * horizontal;
                Vector3 ver = transform.forward * vertical;

                Vector3 vel = (hor + ver).normalized * 5f;

                rb.MovePosition(transform.position + vel * Time.deltaTime);
            }
            else
            {
                Debug.Log("2");
                animator.SetBool("Isrun", false);
                animator.SetBool("Ismove", true);

                Vector3 hor = transform.right * horizontal;
                Vector3 ver = transform.forward * vertical;

                Vector3 vel = (hor + ver).normalized * 3f;

                rb.MovePosition(transform.position + vel * Time.deltaTime);
            }
        }
        else
        {
            animator.SetBool("Isrun", false);
            animator.SetBool("Ismove", false);
        }
    }

    [System.Serializable]
	public struct Arsenal {
		public string name;
		public GameObject rightGun;
		public GameObject leftGun;
		public RuntimeAnimatorController controller;
	}

    public void Jump()
    {
        animator.SetBool("Squat", false);
        //animator.SetBool("abc", false);
        animator.SetTrigger("Jump");
    }

    public void Sitting()
    {
        animator.SetBool("Squat", !animator.GetBool("Squat"));

        //basicCamera.transform.localPosition =  new Vector3(0.05f , 1.074f, -0.8569999f);
    }
}
