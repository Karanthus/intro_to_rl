using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;

public class PlayerAgent : Agent
{

    public Vector3 startingPosition = new Vector3(-22.5f, 1.519f, -31.6f);
    
    public float m_speed = 30f; // Player movement speed

    public GameObject Enemy_Bullets;
    public List<GameObject> activeBullets = new List<GameObject>();
    
    
    //public GameObject CP0;
    //public GameObject CP1;
    //public GameObject CP2;

    private Rigidbody rb;


    private float boundXLeft = 12.9f;
    private float boundXRight = -76f;
    //private float boundZBackward = -22f;

    public bool isAtWall = false; //Unused atm




    private enum ACTIONS
    {
        LEFT = 0,
        FORWARD = 1,
        RIGHT = 2,
        NOTHING = 3,

    }



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        //CP0 = GameObject.Find("CP0");
        //CP1 = GameObject.Find("CP1");
        //CP2 = GameObject.Find("CP2");
    }
    void ReactivateObject()
    {
        //CP0.SetActive(true);
        //CP1.SetActive(true); // Reactivate the object
        //CP2.SetActive(true);
    }
    public void DestroyAllBullets()
    {
        foreach (GameObject bullet in activeBullets)
        {
            Destroy(bullet);
        }
        activeBullets.Clear();
    }
    public void RegisterBullet(GameObject bullet)
    {
        activeBullets.Add(bullet);
    }

    public override void OnEpisodeBegin()
    {
        DestroyAllBullets();
        transform.localPosition = new Vector3(-22.5f, 1.519f, -31.6f); //startingPosition;
        // Reset the agent's position for a new episode
        
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(TargetTransform.localPosition.x);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.DiscreteActions[0];

        switch (actionTaken)
        {
            case (int)ACTIONS.NOTHING:
                break;
            case (int)ACTIONS.LEFT:
                if (transform.localPosition.z < boundXLeft)
                    transform.Translate(Vector3.left * m_speed * Time.fixedDeltaTime);
                break;
            case (int)ACTIONS.RIGHT:
                if (transform.localPosition.z > boundXRight)
                    transform.Translate(Vector3.right * m_speed * Time.fixedDeltaTime);
                break;
            case (int)ACTIONS.FORWARD:
                transform.Translate(Vector3.forward * m_speed * Time.fixedDeltaTime);
                break;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> actions = actionsOut.DiscreteActions;

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == +1)
        {
            actions[0] = (int)ACTIONS.RIGHT;
        }
        else if (horizontal == -1)
        {
            actions[0] = (int)ACTIONS.LEFT;
        }
        else if (vertical == +1)
        {
            actions[0] = (int)ACTIONS.FORWARD;
            
            
        }
        else if (vertical == -1)
        {
            //actions[0] = (int)ACTIONS.BACKWARD;
        }
        else
        {
            actions[0] = (int)ACTIONS.NOTHING;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyProjectile")
        {
            AddReward(-1f); // Negative reward for getting hit by an enemy projectile
            Debug.Log("HIT");
            EndEpisode();
        }
        if (other.tag == "Finish")
        {
            AddReward(1f);
            Debug.Log("Reached FINISH");
            EndEpisode();
        }
        if (other.tag == "CP")
        {
            AddReward(0.3f);
            //other.gameObject.SetActive(false);
            Debug.Log("CP reached");
        }
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        isAtWall = true;
        
    }
    private void OnCollisionExit(Collision collision)
    {
        isAtWall = false; //Unused atm
    }
    private void OnCollisionStay(Collision collision)
    {
        AddReward(-0.01f);
    }

}


    
