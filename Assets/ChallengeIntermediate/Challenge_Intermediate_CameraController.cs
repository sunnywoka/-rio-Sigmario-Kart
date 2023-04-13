using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge_Intermediate_CameraController : MonoBehaviour
{
   //Target to follow
    [SerializeField] private Transform m_Target;


    //Offset for better control
    [SerializeField] private Vector3 m_offset; 

    //Camera distance from player
    [SerializeField] private float m_distFromTarget;


    //Speed at which the camera smoothly reaches its target position
    [SerializeField] private float m_cameraSmoothSpeed;

    //Only for reference parameter in the smoothing function
    private Vector3 m_cameraVelocity = Vector3.zero;

    public enum CameraFollowType
    {
        LookAt,
        Simple,
        FixedDistance
    }

    [SerializeField] CameraFollowType cameraFollowType;

    
    [SerializeField] private bool smoothFollow;


    //We use LateUpdate because it is called after Update() where animations and computations logic resides
    //Since we want the camera to follow after all the computations have been carried out
   void LateUpdate()
   {    
        if(cameraFollowType == CameraFollowType.LookAt)
           FollowCameraLookAtTarget();
        else if(cameraFollowType == CameraFollowType.Simple)
            FollowCameraSimple();
        else
            FollowCameraFixedDistance();

   }
   
   private void FollowCameraSimple()
   {
          
        //i.e. direction from camera --> target  
         Vector3 targetPos = m_Target.position + m_offset;

        if(smoothFollow)
            //Smooth from camera position --> target position using the follow speed
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_cameraVelocity, m_cameraSmoothSpeed);
        else
            transform.position = targetPos;
   }
   
   private void FollowCameraFixedDistance()
   {    
       
        //Camera's target position to reach 
        //i.e. direction from camera --> target (so that the camera always stays focused on target even if we rotate)  
        //Here distance will be how far the camera stays from the player
         Vector3 targetPos = m_Target.position - transform.forward * m_distFromTarget;

        if(smoothFollow)
            //Smooth from camera position --> target position using the follow speed
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_cameraVelocity, m_cameraSmoothSpeed);
        else
            transform.position = targetPos;

       
   }

   private void FollowCameraLookAtTarget()
   {
        
        Vector3 targetPos = (m_Target.position - transform.forward) + m_offset;

        if(smoothFollow)
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_cameraVelocity, m_cameraSmoothSpeed);
        else
            transform.position = targetPos;

        transform.LookAt(m_Target);
   }
   

}
