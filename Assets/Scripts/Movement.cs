using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject blueBall;
    public GameObject redBall;
    public int movementDistance;
    private static int number = 0;

	private void Start()
	{
        blueBall.SetActive(false);
        redBall.SetActive(false);
	}
		
	private void Update()
    {


        if (Input.GetKeyDown(KeyCode.RightArrow))
		{
            //if (number % 2 == 0) {
                redBall.SetActive(true);
                blueBall.SetActive(false);
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow)){
                blueBall.SetActive(true);
                redBall.SetActive(false);
            }
            //number++;
        


        if (blueBall.activeInHierarchy) {
            //Here we say that Z is forward

            if (Input.GetKeyDown(KeyCode.UpArrow) && (blueBall.transform.position.z + movementDistance) < 18)
            {
                blueBall.transform.Translate(0, 0, movementDistance);
            }


            //Here we say that Z is backward

            else if (Input.GetKeyDown(KeyCode.DownArrow) && (blueBall.transform.position.z - movementDistance) > -18)
            {
                blueBall.transform.Translate(0, 0, -movementDistance);
            }
        }
    }
}