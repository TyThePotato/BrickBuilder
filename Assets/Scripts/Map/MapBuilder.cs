using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Utils;

public class MapBuilder : MonoBehaviour
{
    public static MapBuilder instance;

    public Camera mainCam;
    public Light sun;
    public bool rotateSun = false;
    public PostProcessProfile postProcessProfile; // for ambient color

    public GameObject baseplate;
    public MeshRenderer baseplateRenderer; // for fast access

    public Mesh baseplateMesh;
    public Mesh cubeModel;

    public Shader BrickShader;
    public Shader BrickOutlineShader;

    public GameObject[] ShapePrefabs;

    public Dictionary<Brick.ShapeType, ShapeSizeConstraint> ShapeConstraints = new Dictionary<Brick.ShapeType, ShapeSizeConstraint> {
        { Brick.ShapeType.flag, new ShapeSizeConstraint(new Vector3(1,1,1), new Vector3(1,1,1)) },
        { Brick.ShapeType.dome, new ShapeSizeConstraint(new Vector3(2,2,1), new Vector3(2,2,1)) },
        { Brick.ShapeType.arch, new ShapeSizeConstraint(new Vector3(0,0,3), Math.BigVector3) },
        { Brick.ShapeType.wedge, new ShapeSizeConstraint(new Vector3(2,0,0), Math.BigVector3) }
    };

    // this will allow the mapbuilder to be easily accessed from anywhere without needing a reference
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    private void Start() {
        postProcessProfile.GetSetting<ColorGrading>().enabled.value = true; // enable ppp
    }

    private void Update() {
        if (rotateSun) {
            sun.transform.rotation = mainCam.transform.rotation; // set sun rotation to camera rotation
        }
    }

    public void Build (Map map) {
        // set map properties
        SetMapAmbient(map.AmbientColor);
        mainCam.backgroundColor = map.SkyColor;
        //sun.intensity = map.SunIntensity / 200f;

        // create baseplate
        baseplate = new GameObject("Baseplate");
        baseplate.transform.position = map.BaseplateSize % 2 == 0 ? Vector3.zero : new Vector3(0.5f, 0f, -0.5f); // if the baseplate size is an odd number, the baseplate position needs to be offset a little for some reason
        baseplate.transform.localScale = new Vector3(map.BaseplateSize, 1.0f, map.BaseplateSize);
        baseplate.AddComponent<MeshFilter>().mesh = baseplateMesh;
        baseplateRenderer = baseplate.AddComponent<MeshRenderer>();
        baseplateRenderer.material = MaterialCache.instance.GetMaterial( (map.BaseplateColor, 1.0f, MaterialCache.FaceType.Stud, new Vector2(map.BaseplateSize, map.BaseplateSize), BrickShader));
        BoxCollider bc = baseplate.AddComponent<BoxCollider>();
        bc.size = Vector3.one;
        bc.center = new Vector3(0, -0.5f, 0);

        // create bricks
        for (int i = 0; i < map.Bricks.Count; i++) {
            CreateBrickGameObject(map.Bricks[i]);
        }
    }

    public GameObject CreateBrickGameObject (Brick source) {
        GameObject brickGameobject = Instantiate(ShapePrefabs[(int)source.Shape]); // get correct shape prefab using enum index
        brickGameobject.name = source.Name;
        brickGameobject.layer = 9;
        source.gameObject = brickGameobject;

        // Set up for BrickGO
        BrickGO bg = brickGameobject.GetComponent<BrickGO>();
        bg.brick = source;
        source.brickGO = bg;

        // Set up the BrickShape
        BrickShape bs = brickGameobject.GetComponent<BrickShape>();
        source.brickShape = bs;

        // Set transform info
        Vector3 pos = source.Position.SwapYZ() + source.Scale.SwapYZ() / 2; // Brick-Hill positions are based around some corner of the brick, while Unity positions are based around the pivot point (usually center) of the transform
        pos.x *= -1; // Flip the x axis
        brickGameobject.transform.position = pos;

        source.Rotation = source.Rotation.Mod(360); // correct rotations >360 degrees
        brickGameobject.transform.eulerAngles = new Vector3(0, source.Rotation * -1, 0);

        // Set Material
        for (int i = 0; i < bs.elements.Length; i++) {
            MeshFilter mf = bs.elements[i].GetComponent<MeshFilter>();
            if (mf == null) continue; // skip iteration, this element doesn't have a mesh
            int submeshCount = mf.mesh.subMeshCount;
            MeshRenderer renderer = bs.elements[i].GetComponent<MeshRenderer>();
            Material[] brickMaterials = new Material[submeshCount];
            brickMaterials[0] = MaterialCache.instance.GetMaterial((source.BrickColor, source.Transparency, MaterialCache.FaceType.Smooth, Vector2.one, BrickShader));
            if (submeshCount > 1) {
                if (source.Shape == Brick.ShapeType.spawnpoint) {
                    brickMaterials[1] = MaterialCache.instance.GetMaterial((source.BrickColor, source.Transparency, MaterialCache.FaceType.Spawnpoint, BB.CorrectScale(source.Scale, source.Rotation), BrickShader));
                } else {
                    brickMaterials[1] = MaterialCache.instance.GetMaterial((source.BrickColor, source.Transparency, MaterialCache.FaceType.Stud, BB.CorrectScale(source.Scale, source.Rotation), BrickShader));
                }
            }
            if (submeshCount > 2) brickMaterials[2] = MaterialCache.instance.GetMaterial((source.BrickColor, source.Transparency, MaterialCache.FaceType.Inlet, BB.CorrectScale(source.Scale, source.Rotation), BrickShader));
            renderer.materials = brickMaterials;
        }

        bs.UpdateShape(); // updates the shape
        source.UpdateModel(); // sets the model

        return brickGameobject;
    }

    public void UpdateBrickColor (Brick b, Shader shader = null) {
        Shader shaderToUse = shader ?? b.Selected ? BrickOutlineShader : BrickShader; // nice
        Transform[] allChildren = b.gameObject.transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < allChildren.Length; i++) {
            if (allChildren[i].CompareTag("DontRecolor")) continue; // this gameobject is not allowed to be recolored
            MeshFilter mf = allChildren[i].GetComponent<MeshFilter>();
            if (mf == null) continue; // skip iteration, this element doesn't have a mesh
            int submeshCount = mf.mesh.subMeshCount;
            MeshRenderer renderer = allChildren[i].GetComponent<MeshRenderer>();
            Material[] brickMaterials = new Material[submeshCount];
            brickMaterials[0] = MaterialCache.instance.GetMaterial((b.BrickColor, b.Transparency, MaterialCache.FaceType.Smooth, Vector2.one, shaderToUse));
            if (submeshCount > 1) {
                if (b.Shape == Brick.ShapeType.spawnpoint) {
                    brickMaterials[1] = MaterialCache.instance.GetMaterial((b.BrickColor, b.Transparency, MaterialCache.FaceType.Spawnpoint, BB.CorrectScale(b.Scale, b.Rotation), shaderToUse));
                } else {
                    brickMaterials[1] = MaterialCache.instance.GetMaterial((b.BrickColor, b.Transparency, MaterialCache.FaceType.Stud, BB.CorrectScale(b.Scale, b.Rotation), shaderToUse));
                }
            }
            if (submeshCount > 2) brickMaterials[2] = MaterialCache.instance.GetMaterial((b.BrickColor, b.Transparency, MaterialCache.FaceType.Inlet, BB.CorrectScale(b.Scale, b.Rotation), shaderToUse));
            renderer.materials = brickMaterials;
        }
    }

    public void UpdateBrickTransform (Brick b) {
        // Set transform info
        Vector3 pos = b.Position.SwapYZ() + b.Scale.SwapYZ() / 2; // Brick-Hill positions are based around some corner of the brick, while Unity positions are based around the pivot point (usually center) of the transform
        pos.x *= -1; // Flip the x axis
        b.gameObject.transform.position = pos;

        b.Rotation = b.Rotation.Mod(360); // correct rotations >360 degrees
        b.gameObject.transform.eulerAngles = new Vector3(0, b.Rotation * -1, 0);

        b.CheckIfScuffed();

        b.UpdateShape();
    }

    public void UpdateBrickShape (Brick b) {
        if (b.Selected) EditorUI.instance.gizmo.RemoveTarget(b.gameObject.transform); // deselect transform if brick was selected
        Destroy(b.gameObject);
        CreateBrickGameObject(b);
        // reselect GO if brick was selected
        if (b.Selected) {
            b.brickGO.SetOutline(true);
            EditorUI.instance.gizmo.AddTarget(b.gameObject.transform);
        }
    }

    public void UpdateBrick (Brick b) {
        UpdateBrickTransform(b);
        UpdateBrickColor(b);
    }


    public void UpdateEnvironment (Map source) {
        SetMapAmbient(source.AmbientColor); // ambient color
        //sun.intensity = source.SunIntensity; // sun intensity, disabled because its inaccurate

        mainCam.backgroundColor = source.SkyColor; // sky color

        // baseplate stuff
        baseplate.transform.position = source.BaseplateSize % 2 == 0 ? Vector3.zero : new Vector3(0.5f, 0f, -0.5f); // if the baseplate size is an odd number, the baseplate position needs to be offset a little for some reason
        baseplate.transform.localScale = new Vector3(source.BaseplateSize, 1.0f, source.BaseplateSize); // baseplate size
        baseplateRenderer.material = MaterialCache.instance.GetMaterial((source.BaseplateColor, 1.0f, MaterialCache.FaceType.Stud, new Vector2(source.BaseplateSize, source.BaseplateSize), BrickShader)); // baseplate color
        BoxCollider bc = baseplate.GetComponent<BoxCollider>();
        bc.size = Vector3.one; // collider size
        bc.center = new Vector3(0, -0.5f, 0); // collider offset, shouldnt need to be reset but ill do it just to be safe
    }

    public void SetMapAmbient (Color ambient) {
        ColorGrading cg = postProcessProfile.GetSetting<ColorGrading>(); // get colorgrading setting
        cg.mixerRedOutRedIn.value = ambient.r * 200 + 100; // r.r
        cg.mixerRedOutGreenIn.value = ambient.r * 200; // r.g
        cg.mixerRedOutBlueIn.value = ambient.r * 200; // r.b
        cg.mixerGreenOutRedIn.value = ambient.g * 200; // g.r
        cg.mixerGreenOutGreenIn.value = ambient.g * 200 + 100; // g.g
        cg.mixerGreenOutBlueIn.value = ambient.g * 200; // g.b
        cg.mixerBlueOutRedIn.value = ambient.b * 200; // b.r
        cg.mixerBlueOutGreenIn.value = ambient.b * 200; // b.g
        cg.mixerBlueOutBlueIn.value = ambient.b * 200 + 100; // b.b
        cg.lift.value = new Vector4(ambient.r, ambient.g, ambient.b, 0f);
    }
}

public struct ShapeSizeConstraint {
    public Vector3 min;
    public Vector3 max;

    public ShapeSizeConstraint (Vector3 minimum, Vector3 maximum) {
        min = minimum;
        max = maximum;
    }
}
