/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Save level"))
        {
            Level level = (Level)target;
            level.transform.position = Vector3.zero;
            level.transform.rotation = Quaternion.identity;
            var levelRoot = GameObject.Find("Level");
            var ldr = new LevelDataRepresentation();
            var levelItems = new List<LevelItemRepresentation>();

            foreach (Transform t in levelRoot.transform)
            {
                var sr = t.GetComponent<SpriteRenderer>();
                var li = new LevelItemRepresentation()
                {
                    position = t.position,
                    rotation = t.rotation.eulerAngles,
                    scale = t.localScale
                };
                if (t.name.Contains(" "))
                {
                    li.prefabName = t.name.Substring(0, t.name.IndexOf(" "));
                }
                else
                {
                    li.prefabName = t.name;
                }
                if (sr != null)
                {
                    li.spriteLayer = sr.sortingLayerName;
                    li.spriteColor = sr.color;
                    li.spriteOrder = sr.sortingOrder;
                }
                levelItems.Add(li);
            }

            ldr.levelItems = levelItems.ToArray();
            ldr.playerStartPosition =
              GameObject.Find("SoyBoy").transform.position;

            var currentCamSettings = FindObjectOfType<CameraLerpToTransform>();
            if (currentCamSettings != null)
            {
                ldr.cameraSettings = new CameraSettingsRepresentation()
                {
                    cameraTrackTarget = currentCamSettings.camTarget.name,
                    cameraZDepth = currentCamSettings.cameraZDepth,
                    minX = currentCamSettings.minX,
                    minY = currentCamSettings.minY,
                    maxX = currentCamSettings.maxX,
                    maxY = currentCamSettings.maxY,
                    trackingSpeed = currentCamSettings.trackingSpeed
                };
            }

            var levelDataToJson = JsonUtility.ToJson(ldr);
            var savePath = System.IO.Path.Combine(Application.dataPath,
                level.levelName + ".json");
            System.IO.File.WriteAllText(savePath, levelDataToJson);
            Debug.Log("Level saved to " + savePath);
        }
    }
}