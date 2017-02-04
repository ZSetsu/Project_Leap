using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontrol : MonoBehaviour {
    Rigidbody m_rigidbody;
    Touch l_touch = new Touch();
    Touch r_touch = new Touch();

    Touch l_jump = new Touch();
    Touch r_jump = new Touch();

    float l_start_time;
    float r_start_time;

    Vector3 l_start_pos;
    Vector3 l_end_pos;

    Vector3 r_start_pos;
    Vector3 r_end_pos;


    public float touch_sense = 100f;
    public float jump_touch_sense = 70f;

  
    public float maximum_speed = 350f;
    public float stride_force = 100f;
    public float stride_increment = 25f;
    public float jump_force = 450f;

    bool l_used = false;
    bool r_used = false;

    Vector3 movement = new Vector3(0, 0, 1);

    void Start() {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Update() {
        print(stride_force);       
        GetLeftInput();        
        GetRightInput();
        if (isGrounded())
        {
            print("raycasted");
            CheckJump();
        }      
        
        
    }

    void GetLeftInput()
    {
        foreach (Touch t in Input.touches)
        {
            if (t.position.x < Screen.width / 2 && t.phase == TouchPhase.Began)
            {
                l_touch = t;
                l_start_pos.y = t.position.y;
                l_start_time = Time.time;
            }
            else if (!l_used && t.position.x < Screen.width / 2 && t.phase == TouchPhase.Ended)
            {
                l_end_pos.y = t.position.y;
                if (l_start_pos.y - l_end_pos.y > touch_sense)
                {
                    if (stride_force < maximum_speed)
                    {
                        stride_force += stride_increment;
                    }
                    m_rigidbody.AddForce(movement * stride_force);
                    l_used = true;
                    r_used = false;
                }
                l_touch = new Touch();
            }
        }
    }

    void GetRightInput() {
        foreach (Touch t in Input.touches)
        {
            if (t.position.x >= Screen.width / 2 && t.phase == TouchPhase.Began)
            {
                r_touch = t;
                r_start_pos.y = t.position.y;
                r_start_time = Time.time;
            }
            //If two touches, and both have distance greater than x, register jump
            else if (!r_used && t.position.x >= Screen.width / 2 && t.phase == TouchPhase.Ended)
            {
                r_end_pos.y = t.position.y;
                if (r_start_pos.y - r_end_pos.y > touch_sense)
                {

                    if (stride_force < maximum_speed) {
                        stride_force += stride_increment;
                    }
                    m_rigidbody.AddForce(movement * stride_force);
                    r_used = true;
                    l_used = false;
                }
                r_touch = new Touch();
            }
        }
    }

    void CheckJump()
    {
        if(Input.touchCount == 2)
        {
            print("jumping");
            l_jump = Input.GetTouch(0);
            r_jump = Input.GetTouch(1);

            if (l_jump.phase == TouchPhase.Moved && r_jump.phase == TouchPhase.Moved)
            {
                if(l_start_pos.y - l_jump.position.y > jump_touch_sense
                   && r_start_pos.y - r_jump.position.y > jump_touch_sense)
                {
                    m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, 0, m_rigidbody.velocity.z);
                    m_rigidbody.AddForce(jump_force * Vector3.up);
                }
            }
            //m_rigidbody.AddForce(300f * Vector3.up);
            r_used = false;
            l_used = false;
        }
    }

    bool isGrounded()
    {
        return (Physics.Raycast(this.transform.position, -Vector3.up, 1f));   
    }


    void trip()
    {
        stride_force = 0;
        m_rigidbody.velocity = Vector3.zero;
    }

}



