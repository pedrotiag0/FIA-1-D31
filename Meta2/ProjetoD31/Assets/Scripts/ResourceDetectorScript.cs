﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDetectorScript : MonoBehaviour
{

    public float angleOfSensors = 10f;
    public float rangeOfSensors = 0.1f;
    protected Vector3 initialTransformUp;
    protected Vector3 initialTransformFwd;
    public float strength;
    public float angle;
    public int numObjects;
    public bool debug_mode;

    public float miu;
    public float sigma;

    public float limiarMinResource;
    public float limiarMaxResource;
    public float limiarSupResource;
    public float limiarInfResource;

    // Start is called before the first frame update
    void Start()
    {

        initialTransformUp = this.transform.up;
        initialTransformFwd = this.transform.forward;
    }

    // FixedUpdate is called at fixed intervals of time
    void FixedUpdate()
    {
        ObjectInfo anObject;
        anObject = GetClosestPickup();
        if (anObject != null)
        {
            angle = anObject.angle;
            strength = 1.0f / (anObject.distance + 1.0f);
        }
        else
        { // no object detected
            strength = 0;
            angle = 0;
        }
        
    }

    public float GetAngleToClosestResource()
    {
        return angle;
    }


    public float GetLinearOuput()
    {
        if ( strength > limiarMaxResource ) // X max
        {
            strength = limiarMaxResource;
        } else if( strength < limiarMinResource ) // X min
        {
            strength = limiarMinResource;
        }

        if ( strength > limiarSupResource) // Y max
        {
            strength = limiarSupResource;
        } else if ( strength < limiarInfResource) // Y min
        {
            strength = limiarInfResource;
        }

        return strength;
    } 

    public virtual float GetGaussianOutput()
    {
        // YOUR CODE HERE
        //return strength;
        if (strength > limiarMaxResource) // X max
        {
            strength = limiarMaxResource;
        }
        else if (strength < limiarMinResource) // X min
        {
            strength = limiarMinResource;
        }

        float aux = (float)((1.0 / (sigma * Math.Sqrt(2.0 * Math.PI))) * Math.Exp(-0.5 * Math.Pow((strength - miu) / sigma, 2.0)));

        if (aux > limiarSupResource) // Y max
        {
            aux = limiarSupResource;
        }
        else if (strength < limiarInfResource) // Y min
        {
            aux = limiarInfResource;
        }
        return aux;
        throw new NotImplementedException();
    }

    public virtual float GetLogaritmicOutput()
    {
        // YOUR CODE HERE
        //return strength;
        if (strength > limiarMaxResource) // X max
        {
            strength = limiarMaxResource;
        }
        else if (strength < limiarMinResource) // X min
        {
            strength = limiarMinResource;
        }

        float aux = -(float)(Math.Log(strength));

        if (aux > limiarSupResource) // Y max
        {
            aux = limiarSupResource;
        }
        else if (strength < limiarInfResource) // Y min
        {
            aux = limiarInfResource;
        }
        return aux;
        throw new NotImplementedException();
    }


    public ObjectInfo[] GetVisiblePickups()
    {
        return (ObjectInfo[]) GetVisibleObjects("Pickup").ToArray();
    }

    public ObjectInfo GetClosestPickup()
    {
        ObjectInfo [] a = (ObjectInfo[])GetVisibleObjects("Pickup").ToArray();
        if(a.Length == 0)
        {
            return null;
        }
        return a[a.Length-1];
    }

    public List<ObjectInfo> GetVisibleObjects(string objectTag)
    {
        RaycastHit hit;
        List<ObjectInfo> objectsInformation = new List<ObjectInfo>();

        for (int i = 0; i * angleOfSensors < 360f; i++)
        {
            if (Physics.Raycast(this.transform.position, Quaternion.AngleAxis(-angleOfSensors * i, initialTransformUp) * initialTransformFwd, out hit, rangeOfSensors))
            {

                if (hit.transform.gameObject.CompareTag(objectTag))
                {
                    if (debug_mode)
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


    private void LateUpdate()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.transform.parent.rotation.z * -1.0f);

    }
}
