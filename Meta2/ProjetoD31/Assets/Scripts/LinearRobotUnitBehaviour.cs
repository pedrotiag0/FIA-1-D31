using System;
using System.Collections;
using UnityEngine;

public class LinearRobotUnitBehaviour : RobotUnit
{
    public float weightResource;
    public float resourceValue;
    public float resouceAngle;

    public float weightWall;
    public float wallValue;
    public float wallAngle;

    void Update()
    {

        // get sensor data of resources
        resouceAngle = resourcesDetector.GetAngleToClosestResource();
        resourceValue = weightResource * resourcesDetector.GetLinearOuput();

        // get sensor data of walls | Meta 1
        wallAngle = blockDetector.GetAngleToClosestObstacle();
        wallValue = weightWall * blockDetector.GetLinearOuput();

        // apply to the ball
        applyForce(resouceAngle, resourceValue);    // go towards resources
        applyForce(wallAngle, wallValue);           // go away from walls (negative force will be applied in Unity) | Meta 1

        

    }


}






