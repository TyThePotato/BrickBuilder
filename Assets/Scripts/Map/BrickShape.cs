using System;
using UnityEngine;
using Utils;

public class BrickShape : MonoBehaviour
{
    public BrickGO brickGO;

    public Transform[] elements;
    private GameObject[] segments; // used for vents and bars to keep track of all the child gameobjects

    public GameObject assetGO;

    public void UpdateShape () {
        Brick brick = brickGO.brick;
        Vector3 size = brick.Scale;
        // clamp size if necessary
        if (MapBuilder.instance.ShapeConstraints.TryGetValue(brick.Shape, out ShapeSizeConstraint ssc)) {
            size = size.Clamp(ssc.min, ssc.max);
        }
        size = BB.CorrectScale(size, brick.Rotation).SwapYZ(); // correct scale based on rotation then swap y and z to convert brick hill xyz to unity xyz

        if (brick.Shape == Brick.ShapeType.cube) {
            // 0 = brick
            elements[0].localScale = size;
        } else if (brick.Shape == Brick.ShapeType.slope) {
            // 0 = base, 1 = main, 2 = back
            elements[0].localScale = new Vector3(size.x, 0.3f, size.z);
            elements[0].localPosition = new Vector3(0, -size.y / 2, 0);
            elements[1].localScale = new Vector3(size.x - 1f, size.y - 0.3f, size.z);
            elements[1].localPosition = new Vector3(size.x / 2, -size.y / 2 + 0.3f, 0);
            elements[2].localScale = new Vector3(1f, size.y, size.z);
            elements[2].localPosition = new Vector3(-size.x / 2 + 0.5f, -size.y / 2, 0);
        } else if (brick.Shape == Brick.ShapeType.wedge) {
            // 0 = main, 1 = side
            elements[0].localScale = new Vector3(size.x - 1f, size.y, size.z);
            elements[0].localPosition = new Vector3(size.x / 2 - 1, 0, 0);
            elements[1].localScale = new Vector3(1f, size.y, size.z);
            elements[1].localPosition = new Vector3(size.x / 2 - 0.5f, 0, 0);
        } else if (brick.Shape == Brick.ShapeType.spawnpoint) {
            // 0 = spawnpoint
            elements[0].localScale = brick.Scale.SwapYZ();
        } else if (brick.Shape == Brick.ShapeType.arch) {
            // 0 = archway, 1 = top, 2 = side 1, 3 = side 2
            elements[0].localScale = new Vector3(size.x, size.y - 0.3f, size.z - 2);
            elements[0].localPosition = new Vector3(0, -size.y / 2, 0);
            elements[1].localScale = new Vector3(size.x, 0.3f, size.z);
            elements[1].localPosition = new Vector3(0, size.y / 2, 0);
            elements[2].localScale = new Vector3(size.x, size.y, 1);
            elements[2].localPosition = new Vector3(0, -size.y / 2, size.z / 2 - 0.5f);
            elements[3].localScale = new Vector3(size.x, size.y, 1);
            elements[3].localPosition = new Vector3(0, -size.y / 2, -size.z / 2 + 0.5f);
        } else if (brick.Shape == Brick.ShapeType.dome) {
            // 0 = dome
            elements[0].localScale = brick.Scale.SwapYZ();
        } else if (brick.Shape == Brick.ShapeType.bars) {
            // 0 = bar segment, 1 = base, 2 = pole, 3 = top
            int segmentCountX = Mathf.CeilToInt(size.x);
            int segmentCountZ = Mathf.CeilToInt(size.z);
            float firstPosX = -segmentCountX / 2f + 0.5f; // ebic
            float firstPosZ = -segmentCountZ / 2f + 0.5f; // also ebic

            if (segments != null) {
                for (int i = 0; i < segments.Length; i++) {
                    Destroy(segments[i]);
                }
            }
            segments = new GameObject[segmentCountX*segmentCountZ];

            elements[0].localPosition = new Vector3(firstPosX, 0, firstPosZ);
            elements[1].localPosition = new Vector3(0, -size.y / 2, 0);
            elements[2].localScale = new Vector3(1, size.y, 1);
            elements[3].localPosition = new Vector3(0, size.y / 2, 0);

            for (int z = 1; z < segmentCountZ; z++) {
                GameObject newSegment = Instantiate(elements[0].gameObject);
                newSegment.transform.SetParent(transform, false);
                newSegment.transform.localPosition = new Vector3(0, 0, firstPosZ + z);
                segments[z] = newSegment;
            }

            for (int x = 1; x < segmentCountX; x++) {
                GameObject newSegment = Instantiate(elements[0].gameObject);
                newSegment.transform.SetParent(transform, false);
                newSegment.transform.localPosition = new Vector3(firstPosX + x, 0, 0);
                segments[segmentCountZ+x] = newSegment;
            }
        } else if (brick.Shape == Brick.ShapeType.flag) {
            // 0 = flag
            elements[0].localScale = size;
        } else if (brick.Shape == Brick.ShapeType.pole) {
            // 0 = base, 1 = mid, 2 = tip
            elements[0].localPosition = new Vector3(0, -size.y / 2, 0);
            elements[1].localScale = new Vector3(1, size.y - 0.6f, 1);
            elements[2].localPosition = new Vector3(0, 0.5f * size.y - 0.3f, 0);
        } else if (brick.Shape == Brick.ShapeType.cylinder) {
            // 0 = cyl, 1 = base
            elements[0].localScale = new Vector3(size.x, size.y - 0.3f, size.z);
            elements[0].localPosition = new Vector3(0, -size.y / 2 + 0.3f, 0);
            elements[1].localScale = new Vector3(size.x-0.2f, 0.3f, size.z-0.2f);
            elements[1].localPosition = new Vector3(0, -size.y / 2, 0);
        } else if (brick.Shape == Brick.ShapeType.round_slope) {
            // 0 = slope
            elements[0].localScale = size;
        } else if (brick.Shape == Brick.ShapeType.vent) {
            // 0 = main
            int segmentCountX = Mathf.CeilToInt(size.x);
            float firstPosX = -segmentCountX / 2f + 0.5f; // ebic

            if (segments != null) {
                for (int i = 0; i < segments.Length; i++) {
                    Destroy(segments[i]);
                }
            }
            segments = new GameObject[segmentCountX];

            elements[0].localPosition = new Vector3(firstPosX, 0, 0);
            elements[0].localScale = new Vector3(1, 0.3f, Mathf.CeilToInt(size.z));

            for (int x = 0; x < segmentCountX; x++) {
                if (x == 0) continue; // skip the first piece since we already have it
                GameObject newSegment = Instantiate(elements[0].gameObject);
                newSegment.transform.SetParent(transform, false);
                newSegment.transform.localPosition = new Vector3(firstPosX + x, 0, 0);
                segments[x] = newSegment;
            }
        }

        GetComponent<BoxCollider>().size = size;
    }

    public void SetAssetGameobject (Mesh mesh, Texture2D texture) {
        if (assetGO == null) {
            assetGO = new GameObject("Asset");
            assetGO.transform.SetParent(transform);
            assetGO.transform.localPosition = new Vector3(0, -5, 0); // models have a bit of offset
            assetGO.transform.localScale = new Vector3(-1, 1, -1); // models need to be flipped on x and z axis
            assetGO.transform.localRotation = Quaternion.identity; // zero the rotation
            assetGO.tag = "DontRecolor";
        }
        MeshFilter mf = assetGO.GetComponent<MeshFilter>();
        if (mf == null) mf = assetGO.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        MeshRenderer mr = assetGO.GetComponent<MeshRenderer>();
        if (mr == null) mr = assetGO.AddComponent<MeshRenderer>();

        if (mr.material == null) {
            mr.material = new Material(brickGO.outlined ? Shader.Find("Brick/Wireframe") : Shader.Find("Brick/Normal"));
        }
        mr.material.mainTexture = texture;
    }

    public void RemoveAssetGameobject () {
        if (assetGO != null) {
            Destroy(assetGO);
            assetGO = null;
        }
    }
}
