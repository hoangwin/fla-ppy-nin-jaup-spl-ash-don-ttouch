using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{ // Transform of target object, defaults to player

    public Transform target;
    public Transform GUICameraTranform;
    public Vector3 targetPre;

    // Time in seconds for smoothing

    public float smoothTime = 1f;

    // Distance from target

    public float distance = 4.0f;



    // Local copy of transform to reduce component lookups

    public Transform _transform;

    // Velocity of camera smoothing

    Vector3 _smoothVelocity;
    public static CameraFollow instance;
    void Start()
    {
        _transform = transform;
        if (target == null)
        {            // Point at player by default

            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
                target = player.transform;
        }
        targetPre = new Vector3(target.position.x, target.position.y, target.position.z);
        instance = this;
    }



    void Update()
    {

        if (target != null && false)
        {
            // Point camera towards target

            Vector3 targetPosition = target.position;
            targetPosition.x = 0;

            targetPosition.z -= distance;
            targetPosition.y = targetPosition.y + 1.5f;
          
            _transform.position = Vector3.SmoothDamp(_transform.position, targetPosition, ref _smoothVelocity, smoothTime);
           // if (BackGroundFlow.instance != null)
           //     BackGroundFlow.instance.UpdateBackGround();
        }
        if (targetPre.y < target.position.y)
        {
            targetPre.y = target.position.y;

            Vector3 v3 = target.position;
            v3.x = 0;
            v3.z = transform.position.z;
            //    if (Vector3.Distance(v3, target.position) > deadZone)
            transform.position = Vector3.Lerp(transform.position, v3, distance * Time.deltaTime);
        }
        GUICameraTranform = this.transform;
        ScoreControl.setScore((int)( this.transform.position.y*5));
            
    }
    public void reset()
    {
        //transform.position = new Vector3(0, 0, transform.position.z);
        targetPre = new Vector3(0, 0, target.position.z);
        iTween.MoveTo(CameraFollow.instance.gameObject, iTween.Hash("y", 0, "time", 1.5f));
    }
}
