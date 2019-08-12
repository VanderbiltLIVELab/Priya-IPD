using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLevel : MonoBehaviour
{
    public GameObject cam;
    public GameObject redBall;
    public GameObject blueBall;
    /*
    public GameObject redtennisBall;
    public GameObject redsoftBall;
    public GameObject redpingpongBall;
    public GameObject bluetennisBall;
    public GameObject bluesoftBall;
    public GameObject bluepingpongBall;
    */

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var ypos = this.transform.position.y;
        Vector3 redTransform = redBall.transform.position;
        redTransform.y = ypos;
        redBall.transform.position = redTransform;
        Vector3 blueTransform = blueBall.transform.position;
        blueTransform.y = ypos;
        blueBall.transform.position = blueTransform;

    }
}
