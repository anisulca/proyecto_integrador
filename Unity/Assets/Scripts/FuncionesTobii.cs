using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class FuncionesTobii : MonoBehaviour

{
    private GazePoint invalidGazePoint = new GazePoint(new Vector2(float.NaN, float.NaN), -1.0f, -1);
    private GazePoint lastGazePoint = GazePoint.Invalid;
    private HeadPose lastHeadPose = HeadPose.Invalid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GazePoint GetGazeData()
    {
        GazePoint gazePoint = TobiiAPI.GetGazePoint();

        if(gazePoint.IsRecent() && (gazePoint.Timestamp > (lastGazePoint.Timestamp - float.Epsilon)))
        {
            return gazePoint;
        }
        else
        {
            return GazePoint.Invalid;
        }
    }

    public HeadPose GetHeadPoseData()
    {
        HeadPose headPose = TobiiAPI.GetHeadPose();

        if(headPose.IsRecent() && (headPose.Timestamp > (lastHeadPose.Timestamp - float.Epsilon)))
        {
            return headPose;
        }
        else
        {
            return HeadPose.Invalid;
        }
    }
}
