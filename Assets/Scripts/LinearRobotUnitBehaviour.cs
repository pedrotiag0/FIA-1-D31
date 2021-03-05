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

        // get sensor data of walls
        wallAngle = blockDetector.GetAngleToClosestObstacle();
        wallValue = weightWall * blockDetector.GetLinearOuput();

        // apply to the ball
        applyForce(resouceAngle, resourceValue);    // go towards resource
        applyForce(wallAngle, wallValue);           // go away from wall

        

    }


}






