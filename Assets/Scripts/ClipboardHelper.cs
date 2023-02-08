using System.Collections;
using System.Collections.Generic;
using BrickBuilder.World;
using UnityEngine;

namespace BrickBuilder.Clipboard {
    public class ClipboardHelper : MonoBehaviour {
        private static List<BrickData> clipBoardBricks;

        public static void Copy(List<BrickData> bricks) {
            // save to list
            clipBoardBricks = bricks;

            // also copy data to OS clipboard
            string data = ToEncodedBricks(bricks);
            Debug.Log(data);
        }

        public static List<BrickData> Paste() {
            // firstly try to parse clipboard data
            return null;

        }

        private static string ToEncodedBricks(List<BrickData> bricks) {
            EncodedBrickData data = new EncodedBrickData(bricks);
            return JsonUtility.ToJson(data);
        }

        private static List<BrickData> FromEncodedBricks(string data) {
            EncodedBrickData brickData = JsonUtility.FromJson<EncodedBrickData>(data);
            return brickData.bricks;
        }
    }

    struct EncodedBrickData {
        public List<BrickData> bricks;

        public EncodedBrickData (List<BrickData> data) {
            bricks = data;
        }
    }
}
