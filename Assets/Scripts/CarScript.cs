using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    private float carSpeed = 0;

    GameController gameController;

    private MeshRenderer parentMesh;

    public GameObject target;

    private List<Vector3> positions;
    private List<Quaternion> rotations;

    Rigidbody rb;

    public bool isReplay = false;
    public bool canRecord;
    public bool callNextCar = false;
    bool isStart = false;
    //bool isStart = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameController = FindObjectOfType<GameController>();

        parentMesh = transform.parent.transform.GetComponent<MeshRenderer>();

        target.SetActive(true);

        positions = new List<Vector3>();
        rotations = new List<Quaternion>();

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.tag != Tags.OLDCAR_TAG)
        {
            DriveCar();
        }
        else if(transform.tag == Tags.OLDCAR_TAG && !gameController.isStart && Input.GetMouseButton(1) && !isStart)
        {
            PlayCarRecord();
        }
    }

    private void FixedUpdate()
    {
        if (isReplay)
        {
            canRecord = false;
            StartCoroutine(Replay());            
        }
            
        else if(canRecord)
            Record();
    }

    void DriveCar()
    {
        if (Input.GetMouseButton(1))
        {
            carSpeed = 5;
            canRecord = true;
        }


        transform.Translate(Vector3.forward * Time.deltaTime * carSpeed);

        if (Input.GetMouseButton(0))
        {
            transform.Rotate(Vector3.down);
        }

        if (Input.GetMouseButton(1))
        {
            transform.Rotate(Vector3.up);
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 1f)
        {
            transform.tag = Tags.OLDCAR_TAG;

            transform.position = positions[0];
            transform.rotation = rotations[0];

            carSpeed = 0;

            if (!callNextCar)
            {
                gameController.CreateCar();
                callNextCar = true;
            }
            target.SetActive(false);
            parentMesh.enabled = false;
            // StartReplay();
        }
    }

    void PlayCarRecord()
    {
        canRecord = false;
        StartReplay(); 
        isStart = true;

    }

    void Record()
    {
        positions.Add(transform.position);
        rotations.Add(transform.rotation);
    }

    public IEnumerator Replay()
    {
        rb.isKinematic = true;
        isReplay = false;
        carSpeed = 0;
        for (int i = 0; i < positions.Count; i++)
        {  
            transform.position = positions[i];
            transform.rotation = rotations[i];
            yield return new WaitForSeconds(2*Time.deltaTime);

        }
        yield return new WaitForSeconds(0.1f);
        isStart = false;
        Debug.Log("MERHABA");
        

    }
    void StartReplay()
    {
        isReplay = true;
    }

    public void GoFirstPos()
    {
        transform.position = positions[0];
        transform.rotation = rotations[0];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == Tags.OBSTACLE_TAG)
        {
            if(transform.tag != Tags.OLDCAR_TAG)
            {
                carSpeed = 0;
                transform.position = positions[0];
                transform.rotation = rotations[0];
                positions.Clear();
                rotations.Clear();
                gameController.ResetPos();
                
            }
            else
            {
                transform.position = positions[0];
                transform.rotation = rotations[0];
                gameController.ResetPos();

            }
        }

    }
}
