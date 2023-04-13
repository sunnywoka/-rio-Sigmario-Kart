using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge_Basic_CameraController : MonoBehaviour
{
    //Target to follow
    [SerializeField] private Transform m_Target;

    //Offset for better control
    [SerializeField] private Vector3 m_offset; 

    //Speed at which the camera smoothly reaches its target position
    [SerializeField] private float m_cameraSmoothSpeed;

    //Only for reference parameter in the smoothing function
    private Vector3 m_cameraVelocity = Vector3.zero;


    //We use LateUpdate because it is called after Update() where animations and computations logic resides
    //Since we want the camera to follow after all the computations have been carried out
   void LateUpdate()
   {        
        FollowCameraSimple();
   }
   
   private void FollowCameraSimple()
   {
          
        //i.e. direction from camera --> target  
         Vector3 targetPos = m_Target.position + m_offset;
         
        //Smooth from camera position --> target position using the follow speed
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_cameraVelocity, m_cameraSmoothSpeed);
      
   }
   
   
}
