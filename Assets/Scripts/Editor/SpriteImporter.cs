using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

public class MySpriteImporter : AssetPostprocessor
{
    const float PIXELS_PER_UNIT = 128.0f;
   

    void OnPreprocessTexture()
    {
        if (assetPath.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase))
        {
            TextureImporter textureImporter = assetImporter as TextureImporter;

            //only process sprites that are not processed yet (not processed == pixelsPerUnit == 100) otherwise you cant change settings done in unity
            if (textureImporter.textureType == TextureImporterType.Sprite && textureImporter.spritePixelsPerUnit == 100)
            {
                Debug.Log("Imported Sprite: " + assetPath);
                textureImporter.spritePixelsPerUnit = PIXELS_PER_UNIT;
                textureImporter.mipmapEnabled = false;
                textureImporter.filterMode = FilterMode.Bilinear;
                textureImporter.textureFormat = TextureImporterFormat.RGBA32;
            }
        }
    }

}

