using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageImporter : MonoBehaviour
{
    public Vector3 Position;
    public Vector3 Size;
    public int Orientation; // 0: Flat, 1: Standing
    public float HeightmapScale;

    public Texture2D importedImage;
    public Texture2D importedHeightmap;

    // load image from path
    public void LoadImage (string path, bool heightmap) {
        Texture2D imported = new Texture2D(64,64);
        imported.LoadImage(File.ReadAllBytes(path));
        imported.name = Path.GetFileNameWithoutExtension(path);

        if (heightmap) {
            importedHeightmap = imported;
        } else {
            importedImage = imported;
        }
    }

    // resize image
    public void ResizeImage (int x, int y) {
        TextureScale.Bilinear(importedImage, x, y);
    }

    // actually construct image
    public void BuildImage () {
        Vector3 pixelSize = new Vector3(Size.x / importedImage.width, Size.y / (Orientation == 1 ? importedImage.height : 1), Size.z / (Orientation == 0 ? importedImage.height : 1));
        if (importedHeightmap != null) TextureScale.Bilinear(importedHeightmap, importedImage.width, importedImage.height);
        for (int x = 0; x < importedImage.width; x++) {
            for (int y = 0; y < importedImage.height; y++) {
                Color pixelColor = importedImage.GetPixel(x,y);
                if (pixelColor.a == 0f) continue; // skip fully transparent pixels
                
                float heightmapValue = 1;
                if (importedHeightmap != null) heightmapValue = importedHeightmap.GetPixel(x,y).r;
                if (heightmapValue == 0f) continue; // skip pixels where the heightmap has a value of 0
                if (importedHeightmap != null) heightmapValue *= HeightmapScale;

                BrickData pixel = new BrickData();
                pixel.Scale = pixelSize;

                Vector3 pixelPos = Position;
                pixelPos.x += x * pixelSize.x;
                if (Orientation == 1) {
                    pixelPos.y += y * pixelSize.y;
                    pixel.Scale.z *= heightmapValue;  
                    pixelPos.z += pixel.Scale.z / 2;
                }
                if (Orientation == 0) {
                    pixelPos.z += y * pixelSize.z;
                    pixel.Scale.y *= heightmapValue;
                    pixelPos.y += pixel.Scale.y / 2;
                }
                pixelPos -= Size/2;
                pixel.Position = pixelPos;

                pixel.BrickColor = pixelColor;
                pixel.Transparency = 1f;
                pixel.Collision = true;
                pixel.Name = $"Imported Image - {x},{y}";

                EditorUI.instance.ImportBrick(pixel.ToBrick(), true);
            }
        }

        // delete images
        Destroy(importedImage);
        if (importedHeightmap != null) Destroy(importedHeightmap);
    }
}
