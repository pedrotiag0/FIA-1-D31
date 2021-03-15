using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetectorScript : MonoBehaviour
{

    public float angleOfSensors = 10f;
    public float rangeOfSensors = 10f;
    protected Vector3 initialTransformUp;
    protected Vector3 initialTransformFwd;
    public float strength;
    public float angleToClosestObj;
    public int numObjects;
    public bool debugMode;

    public float miu;
    public float sigma;

    public float limiarMinBlock;
    public float limiarMaxBlock;
    public float limiarSupBlock;
    public float limiarInfBlock;

    // Start is called before the first frame update
    void Start()
    {

        initialTransformUp = this.transform.up;
        initialTransformFwd = this.transform.forward;
    }

    // Update is called once per frame
    void FixedUpdate() // META 1
    {
        // YOUR CODE HERE
        ObjectInfo blockObject;
        blockObject = GetClosestWall();
        if (blockObject != null)
        {
            angleToClosestObj = blockObject.angle;
            strength = 1.0f / (blockObject.distance + 1.0f); // Negative strength will be applied in Unity | Meta 1
        }
        else
        { // no wall detected
            strength = 0;
            angleToClosestObj = 0;
        }
    }

    public ObjectInfo GetClosestWall()
    {
        ObjectInfo[] a = (ObjectInfo[])GetVisibleObjects("Wall").ToArray(); // Search objects of type "Wall" | Meta 1
        if (a.Length == 0)
        {
            return null;
        }
        return a[a.Length - 1];
    }

    public List<ObjectInfo> GetVisibleObjects(string objectTag) // Similiar function of "GetVisibleObjects" from ResourceDetectorScript.cs | Meta 1
    {
        RaycastHit hit;
        List<ObjectInfo> objectsInformation = new List<ObjectInfo>();

        for (int i = 0; i * angleOfSensors < 360f; i++)
        {
            if (Physics.Raycast(this.transform.position, Quaternion.AngleAxis(-angleOfSensors * i, initialTransformUp) * initialTransformFwd, out hit, rangeOfSensors))
            {

                if (hit.transform.gameObject.CompareTag(objectTag))
                {
                    if (debugMode)
                    {
                        Debug.DrawRay(this.transform.position, Quaternion.AngleAxis((-angleOfSensors * i), initialTransformUp) * initialTransformFwd * hit.distance, Color.red);
                    }
                    ObjectInfo info = new ObjectInfo(hit.distance, angleOfSensors * i + 90);
                    objectsInformation.Add(info);
                }
            }
        }

        objectsInformation.Sort();

        return objectsInformation;
    }

    public float GetAngleToClosestObstacle()
    {
        return angleToClosestObj;
    }

    public float GetLinearOuput()
    {
        if (strength > limiarMaxBlock) // X max
        {
            strength = limiarMaxBlock;
        }
        else if (strength < limiarMinBlock) // X min
        {
            strength = limiarMinBlock;
        }

        if (strength > limiarSupBlock) // Y max
        {
            strength = limiarSupBlock;
        }
        else if (strength < limiarInfBlock) // Y min
        {
            strength = limiarInfBlock;
        }

        return strength;
    }

    public virtual float GetGaussianOutput() // META 2
    {
        // YOUR CODE HERE
        //return strength;
        if (strength > limiarMaxBlock) // X max
        {
            strength = limiarMaxBlock;
        }
        else if (strength < limiarMinBlock) // X min
        {
            strength = limiarMinBlock;
        }

        float aux = (float)((1.0 / (sigma * Math.Sqrt(2.0 * Math.PI))) * Math.Exp(-0.5 * Math.Pow((strength - miu) / sigma, 2.0)));

        if (aux > limiarSupBlock) // Y max
        {
            aux = limiarSupBlock;
        }
        else if (strength < limiarInfBlock) // Y min
        {
            aux = limiarInfBlock;
        }
        return aux;
        throw new NotImplementedException();
    }

    public virtual float GetLogaritmicOutput() // META 2
    {
        // YOUR CODE HERE
        //return strength;
        if (strength > limiarMaxBlock) // X max
        {
            strength = limiarMaxBlock;
        }
        else if (strength < limiarMinBlock) // X min
        {
            strength = limiarMinBlock;
        }

        float aux = -(float)(Math.Log(strength));

        if (aux > limiarSupBlock) // Y max
        {
            aux = limiarSupBlock;
        }
        else if (strength < limiarInfBlock) // Y min
        {
            aux = limiarInfBlock;
        }
        return aux;
    }
}
