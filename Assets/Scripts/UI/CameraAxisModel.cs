using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAxisModel : MonoBehaviour
{
    private Transform playerCamera;

    public Transform AxisModel;
    public Transform[] Billboards;
    public Transform[] BillboardTargets;

    private void Start() {
        playerCamera = MapBuilder.instance.mainCam.transform;
    }

    private void Update() {
        AxisModel.rotation = Quaternion.Inverse(playerCamera.rotation);

        for (int i = 0; i < Billboards.Length; i++) {
            Billboards[i].position = BillboardTargets[i].position;
        }
    }
}
