﻿//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Valve.VR.InteractionSystem;

//public class ShowControllers : MonoBehaviour
//{
//    public bool showControllers;

//    private void Update() {
//        foreach (Hand hand in Player.instance.hands) {
//            if (showControllers) {
//                hand.ShowController();
//                hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithController);
//            } else {
//                hand.HideController();
//                hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithoutController);
//            }
//        }
//    }
//}
