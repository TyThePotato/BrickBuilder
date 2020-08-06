using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickGO : MonoBehaviour
{
    public Brick brick;
    public bool outlined;

    public void SetOutline (bool value) {
        if (value) {
            MapBuilder.instance.UpdateBrickColor(brick, Shader.Find("Brick/Wireframe")); // set shader to wireframe brick
            if (brick.brickShape.assetGO != null) {
                brick.brickShape.assetGO.GetComponent<MeshRenderer>().material.shader = Shader.Find("Brick/Wireframe");
            }
        } else {
            MapBuilder.instance.UpdateBrickColor(brick); // set shader to normal brick
            if (brick.brickShape.assetGO != null) {
                brick.brickShape.assetGO.GetComponent<MeshRenderer>().material.shader = Shader.Find("Brick/Normal");
            }
        }
        outlined = value;
    }

    public void ResetMaterial () {
        SetOutline(outlined);
    }
}
