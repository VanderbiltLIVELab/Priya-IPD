using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;


public class ViveInput : MonoBehaviour
{
    //public SteamVR_Action_Vector1 squeezeAction;
    //public SteamVR_Action_Vector2 touchPadAction;
    public SteamVR_ActionSet actionSet;
    public SteamVR_Action_Boolean northdpadPress;
    public SteamVR_Action_Boolean southdpadPress;
    public SteamVR_Action_Boolean triggerPrint;
    public GameObject redtennisBall;
    public GameObject redsoftBall;
    public GameObject redpingpongBall;
    public GameObject bluetennisBall;
    public GameObject bluesoftBall;
    public GameObject bluepingpongBall;
    public float movementDistance;
    private static int number = 0;
    private string ballName;
    public string initialFile;
    static string fileName;
    bool redOn = false;

    private Trial currentTrial;
    private int trialCount = 0;
    private List<double> distanceList = new List<double>();
    private List<GameObject> redBallList = new List<GameObject>();
    private List<GameObject> blueBallList = new List<GameObject>();
    private List<Tuple<double, GameObject, GameObject>> trialList = new List<Tuple<double, GameObject, GameObject>>();

    //List: [Tuple, Tuple] -> Tuple: (distance, GameObject)

    StreamWriter logFile;

    void Awake()
    {
        northdpadPress = SteamVR_Actions._default.NorthPressDpad;
        southdpadPress = SteamVR_Actions._default.SouthPressDpad;
        triggerPrint = SteamVR_Actions._default.TriggerPrint;

        fileName = initialFile;
        logFile = File.CreateText(fileName);
    }

    void Start()
    {
        //Add Distances and balls
        distanceList.Add(0.5);
        distanceList.Add(0.6);
        distanceList.Add(0.7);
        distanceList.Add(0.8);
        distanceList.Add(0.9);
        distanceList.Add(1.0);
        distanceList.Add(1.1);
        distanceList.Add(1.2);

        redBallList.Add(redtennisBall);
        redBallList.Add(redsoftBall);
        redBallList.Add(redpingpongBall);
        blueBallList.Add(bluetennisBall);
        blueBallList.Add(bluesoftBall);
        redBallList.Add(bluepingpongBall);

        int repeatTrials = 0;
        while (repeatTrials < 3) {
            foreach (double distance in distanceList) {
                for (int i = 0; i < redBallList.Count; i++) {
                    redBallList[i].SetActive(false);
                    blueBallList[i].SetActive(false);
                    trialList.Add(Tuple.Create<double, GameObject, GameObject>(distance, redBallList[i], blueBallList[i]));
                }
            }
            repeatTrials++;
        }

        actionSet.Activate(SteamVR_Input_Sources.Any, 0, true);

        //Starts the first trial
        Trial currentTrial = new Trial(trialList[trialCount].Item1, trialList[trialCount].Item2, trialList[trialCount].Item3);
        print(trialList.Count); //Must be 72
    }

    void OnApplicationExit() {
        logFile.Close();
    }

    void Update()
    {
        print("Trial Number: " + (trialCount + 1));
        /*//where the "east" and "west" part of the dpad is pressed
        if (Input.GetKeyDown(KeyCode.RightArrow) || (eastdpadPress.stateDown))
        {
            redBall.SetActive(true);
            blueBall.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || (westdpadPress.stateDown))
        {
            blueBall.SetActive(true);
            redBall.SetActive(false);
        }
        */

        if (triggerPrint.stateDown)
        {
            if (currentTrial.getTriggerPress() == 0)
            {
                currentTrial.getRedBall().SetActive(false);
                currentTrial.getBlueBall().SetActive(true);
                currentTrial.setTriggerPress(currentTrial.getTriggerPress() + 1);
            }
            else if (currentTrial.getTriggerPress() == 1) {
                Vector3 redVector = currentTrial.getRedBall().transform.position;
                Vector3 blueVector = currentTrial.getBlueBall().transform.position;
                ballName = "Trial Number: " + (trialCount + 1) + "Red Ball " + currentTrial.getRedBall().tag + ": " + redVector.ToString() + "\r\nBlue Ball " + currentTrial.getBlueBall().tag + ": " + blueVector.ToString();
                WriteString(ballName);

                trialCount++;
                if (trialCount < 72) {
                    currentTrial = new Trial(trialList[trialCount].Item1, trialList[trialCount].Item2, trialList[trialCount].Item3);
                }
            }

            /*
            Debug.Log("working");
            if(number % 2 == 0 || redOn == false)
            {
                Debug.Log("blue");
                redBall.SetActive(false);
                blueBall.SetActive(true);
                number++;
                redOn = true;
            }
            else
            {
                Debug.Log("red");
                blueBall.SetActive(false);
                redBall.SetActive(true);
                number++;
                redOn = false;
            }
            */
            //number++;
        }

        //where the "north" and "south" portion of the dpad is pressed
        if (currentTrial.getBlueBall().activeInHierarchy)
        {
            if (((currentTrial.getBlueBall().transform.position.z + movementDistance) < 18) && (northdpadPress.stateDown || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                currentTrial.getBlueBall().transform.Translate(0, 0, movementDistance);
            }
            if (((currentTrial.getBlueBall().transform.position.z - movementDistance) > -18) && (southdpadPress.stateDown || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                currentTrial.getBlueBall().transform.Translate(0, 0, -movementDistance);
            }
            //if (triggerPrint.stateDown)
            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Debug.Log("Closed");
                logFile.Close();
               // to make it move to the next scene - sceneName.Setscene(active/true);

            }
            */
        }

        if (northdpadPress.stateDown && currentTrial.getBlueBall().activeInHierarchy)
        {
            Debug.Log("D pad has been pressed!");
  
        }
        
    }

    void WriteString(string ballname)
    {
        Debug.Log("written");
        logFile.WriteLine(ballName);
        //logFile.Close();
    }

    public Vector3 placeObj(double distance, GameObject obj, Transform camera)
    {
        double cameraX = camera.position.x;
        double cameraZ = camera.position.z;

        //cameraOffset is necessary so that objects are still placed in the correct location when the ZED_Rig_Stereo rotation is changed
        double cameraOffset = camera.eulerAngles.y - 45;
        double cameraAngleY = camera.eulerAngles.y - cameraOffset;

        double minAngle = (cameraAngleY) * Math.PI / 180;
        double maxAngle = (cameraAngleY) * Math.PI / 180;

        double avatarX = cameraX + distance;
        double avatarZ = cameraZ + distance;

        Vector3 findTerrainHeight = new Vector3((float)avatarX, 0.0f, (float)avatarZ);

        obj.transform.position = new Vector3((float)avatarX, (float)obj.transform.position.y, (float)avatarZ);

        return obj.transform.position;
    }

    private class Trial : ViveInput {

        private GameObject redBall;
        private GameObject blueBall;
        private double distance;

        private int triggerPress;

        public Trial(double distance, GameObject redBall, GameObject blueBall) {
            this.redBall = redBall;
            this.blueBall = blueBall;
            this.distance = distance;
            triggerPress = 0;

            this.redBall.SetActive(true);
            this.blueBall.SetActive(false);
        }

        public int getTriggerPress() {
            return triggerPress;
        }
        public void setTriggerPress(int triggerPress) {
            this.triggerPress = triggerPress;
        }
        public GameObject getRedBall() {
            return this.redBall;
        }
        public GameObject getBlueBall()
        {
            return this.blueBall;
        }
    }

    //Trial currentTrial = new Trial(trialList[count].Item1, trialList[count].Item2);
}
