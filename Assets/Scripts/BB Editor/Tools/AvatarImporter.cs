using System;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class AvatarImporter : MonoBehaviour
{
    public AvatarRoot GetAvatarFromID (string userID) {
        string avatarJSON = GetAvatarJSON(userID);
        if (!string.IsNullOrEmpty(avatarJSON)) {
            AvatarRoot ar = JsonUtility.FromJson<AvatarRoot>(avatarJSON);
            return ar;
        }
        return null;
    }

    public AvatarRoot GetAvatarFromName (string name) {
        return GetAvatarFromID("");
    }

    public void BuildAvatar (AvatarRoot avatar, Vector3 position, bool tool) {

        EditorUI.instance.DeselectAllElements();

        // Main Body

        BrickData avatarHead = new BrickData();
        avatarHead.Scale = new Vector3(1.125f,1.25f,1.125f);
        avatarHead.Position = new Vector3(0.5f,4.33f,0) + position;
        Debug.Log(avatar);
        avatarHead.BrickColor = ColorFromHex(avatar.colors.head);
        avatarHead.Shape = (int)Brick.ShapeType.cylinder;
        avatarHead.Transparency = 1f;
        avatarHead.Collision = true;
        avatarHead.Name = "Head";
        EditorUI.instance.ImportBrick(avatarHead.ToBrick(), true);

        BrickData avatarTorso = new BrickData();
        avatarTorso.Scale = new Vector3(2,2,1);
        avatarTorso.Position = new Vector3(0.5f,3,0) + position;
        avatarTorso.BrickColor = ColorFromHex(avatar.colors.torso);
        avatarTorso.Transparency = 1f;
        avatarTorso.Collision = true;
        avatarTorso.Name = "Torso";
        EditorUI.instance.ImportBrick(avatarTorso.ToBrick(), true);

        BrickData avatarLeftArm = new BrickData();
        if (tool) {
            avatarLeftArm.Scale = new Vector3(1,1,2);
            avatarLeftArm.Position = new Vector3(-1,3.5f,-0.5f) + position;
        } else {
            avatarLeftArm.Scale = new Vector3(1,2,1);
            avatarLeftArm.Position = new Vector3(-1,3,0) + position;
        }
        avatarLeftArm.BrickColor = ColorFromHex(avatar.colors.left_arm);
        avatarLeftArm.Transparency = 1f;
        avatarLeftArm.Collision = true;
        avatarLeftArm.Name = "Left Arm";
        EditorUI.instance.ImportBrick(avatarLeftArm.ToBrick(), true);

        BrickData avatarRightArm = new BrickData();
        avatarRightArm.Scale = new Vector3(1,2,1);
        avatarRightArm.Position = new Vector3(2,3,0) + position;
        avatarRightArm.BrickColor = ColorFromHex(avatar.colors.right_arm);
        avatarRightArm.Transparency = 1f;
        avatarRightArm.Collision = true;
        avatarRightArm.Name = "Right Arm";
        EditorUI.instance.ImportBrick(avatarRightArm.ToBrick(), true);

        BrickData avatarLeftLeg = new BrickData();
        avatarLeftLeg.Scale = new Vector3(1,2,1);
        avatarLeftLeg.Position = new Vector3(0,1,0) + position;
        avatarLeftLeg.BrickColor = ColorFromHex(avatar.colors.left_leg);
        avatarLeftLeg.Transparency = 1f;
        avatarLeftLeg.Collision = true;
        avatarLeftLeg.Name = "Left Leg";
        EditorUI.instance.ImportBrick(avatarLeftLeg.ToBrick(), true);

        BrickData avatarRightLeg = new BrickData();
        avatarRightLeg.Scale = new Vector3(1,2,1);
        avatarRightLeg.Position = new Vector3(1,1,0) + position;
        avatarRightLeg.BrickColor = ColorFromHex(avatar.colors.right_leg);
        avatarRightLeg.Transparency = 1f;
        avatarRightLeg.Collision = true;
        avatarRightLeg.Name = "Right Leg";
        EditorUI.instance.ImportBrick(avatarRightLeg.ToBrick(), true);

        // Hats

        for (int i = 0; i < avatar.items.hats.Length; i++) {
            int hatId = avatar.items.hats[i];
            if (hatId != 0) {
                BrickData hat = new BrickData();
                hat.Scale = Vector3.one;
                hat.Position = new Vector3(0.5f, 4.77f, 0) + position;
                hat.BrickColor = Color.white;
                hat.Transparency = 0f;
                hat.Collision = false;
                hat.Name = "Hat " + (i+1);
                hat.Model = hatId.ToString();
                EditorUI.instance.ImportBrick(hat.ToBrick(), true);
            }
        }

        // Tool

        if (tool && avatar.items.tool != 0) {
            BrickData toolBrick = new BrickData();
            toolBrick.Scale = Vector3.one;
            toolBrick.Position = new Vector3(-2.5f, 5f, 0) + position;
            toolBrick.BrickColor = Color.white;
            toolBrick.Transparency = 0f;
            toolBrick.Collision = false;
            toolBrick.Name = "Tool";
            toolBrick.Model = avatar.items.tool.ToString();
            EditorUI.instance.ImportBrick(toolBrick.ToBrick(), true);
        }

        EditorUI.instance.UpdateInspector();
    }

    public Color ColorFromHex (string hex) {
        if (ColorUtility.TryParseHtmlString("#"+hex, out Color returnColor)) {
            return returnColor;
        }
        return Color.white; // default color
    }

    public string GetAvatarJSON (string id) {
        string api = "https://api.brick-hill.com/v1/games/retrieveAvatar?id=" + id;
        try {
            using (WebClient w = new WebClient()) {
                string s = w.DownloadString(api);
                return s;
            }
        } catch (Exception e) {
            Debug.LogException(e);
            return "";
        }
    }
}

[Serializable]
public class AvatarItems {
    public int face;
    public int[] hats;
    public int head;
    public int tool;
    public int pants;
    public int shirt;
    public int figure;
    public int tshirt;
}

[Serializable]
public class AvatarColors {
    public string head;
    public string torso;
    public string left_arm;
    public string right_arm;
    public string left_leg;
    public string right_leg;
}

[Serializable]
public class AvatarRoot {
    public int user_id;
    public AvatarItems items;
    public AvatarColors colors;
}
