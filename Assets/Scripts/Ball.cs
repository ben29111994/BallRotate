using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
using UnityEngine.EventSystems;
using DG.Tweening;
using GPUInstancer;

public class Ball : MonoBehaviour
{
    public Color ballColor;
    Vector3 ballPos;
    float distanceCheck;
    public GameObject ballParticle;
    bool isFall = false;
    bool isParticle = false;

    private void Start()
    {
        transform.GetChild(0).GetComponent<TrailRenderer>().startColor = ballColor;
        transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color32((byte)ballColor.r, (byte)ballColor.g, (byte)ballColor.b, 0);
        transform.GetChild(0).GetComponent<TrailRenderer>().startWidth = transform.localScale.x * 0.9f;
    }

    private void Update()
    {
        ballPos = new Vector3(transform.position.x, 0, transform.position.z);
        distanceCheck = Vector3.Distance(ballPos, Controller.holePos);
        if (distanceCheck <= 0.9f && !isFall && Controller.limitFall > 0)
        {
            isFall = true;
            Controller.instance.LimitFallChange();
            transform.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
            transform.parent = null;
            transform.DOLocalMoveY(Controller.holePos.y - 10, 1);
            Controller.instance.Scoring();
            AddRemoveInstances.instance.RemoveInstances(GetComponent<GPUInstancerPrefab>());
            Destroy(gameObject, 0.5f);
        }

        if (transform.position.y <= Controller.holePos.y && Controller.limitParticle > 0 && !isParticle)
        {
            isParticle = true;
            //SoundManager.instance.PlaySoundPitch(SoundManager.instance.hit);
            Controller.instance.PlusEffect(new Vector3(transform.position.x, Controller.holePos.y, transform.position.z));
            var particle = Instantiate(ballParticle, new Vector3(Controller.holePos.x, Controller.holePos.y - 0.5f, Controller.holePos.z), Quaternion.Euler(-90, 0, 0));
            particle.GetComponent<Renderer>().material.color = ballColor;
            Controller.instance.LimitParticleChange();
        }
    }
}
