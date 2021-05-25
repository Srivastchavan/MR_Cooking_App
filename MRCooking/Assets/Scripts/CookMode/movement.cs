using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class movement : MonoBehaviour
{
    [SerializeField]
    float speed = 20f;

    [SerializeField]
    float defaultDistance = 10f;

    Transform camPosition;

    int direc = 1;

    public bool isSet;
    Vector3 setPostion;
    // Start is called before the first frame update
    void Start()
    {
        isSet = false;
        camPosition = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (true)
        {
            Vector3 Vec;
            if (Input.GetButtonDown("Fire1"))
            {
                direc *= -1;
            }
            Vec.y = Input.GetAxis("Jump") * direc;
            Vec.x = Input.GetAxis("Horizontal");
            Vec.z = Input.GetAxis("Vertical");
            //transform.position = Vec;
            transform.Translate(new Vector3(Vec.x, Vec.y, Vec.z) * Time.deltaTime * speed);
            //transform.rotation = new Quaternion(0.0f, camPosition.rotation.y, 0.0f, camPosition.rotation.w);
            if (Input.GetKeyDown("return"))
            {
                isSet = true;
                setPostion = transform.position;
                //SetPosition.Invoke();
            }
        }

        if (Input.GetKeyDown("return"))
        {
            isSet = false;
            //Destroy(GetComponent<ARAnchor>());
            //transform.LookAt(Camera.main.transform);
            //transform.position = camPosition.position + camPosition.forward * defaultDistance;
            //transform.localEulerAngles += 180f * Vector3.up;
            transform.position = camPosition.position + camPosition.forward * defaultDistance;
            transform.rotation = new Quaternion(0.0f, camPosition.rotation.y, 0.0f, camPosition.rotation.w);
        }

    }
}
