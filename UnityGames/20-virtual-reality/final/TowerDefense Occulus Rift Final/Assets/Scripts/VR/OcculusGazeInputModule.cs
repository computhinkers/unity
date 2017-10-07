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

using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

// To use:
// 1. Drag onto your EventSystem game object. 
// 2. Disable any other Input Modules (eg: StandaloneInputModule & TouchInputModule) as they will fight over selections.
// 3. Make sure your Canvas is in world space and has a GraphicRaycaster (should by default).
// 4. If you have multiple cameras then make sure to drag your VR (center eye) camera into the canvas.
public class OcculusGazeInputModule : PointerInputModule {
  //1
  public GameObject reticle;
  //2
  public Transform centerEyeTransform;
  //3
  public float reticleSizeMultiplier = 0.02f; // The size of the reticle will get scaled with this value
  //4
  private PointerEventData pointerEventData;
  //5
  private RaycastResult currentRaycast;
  //6
  private GameObject currentLookAtHandler;


  public override void Process() {
    HandleLook();
    HandleSelection();
  }

  void HandleLook() {
    if (pointerEventData == null) {
      pointerEventData = new PointerEventData(eventSystem);
    }

    pointerEventData.position = Camera.main.ViewportToScreenPoint(new Vector3(.5f, .5f)); // Set a virtual pointer to the center of the screen
    List<RaycastResult> raycastResults = new List<RaycastResult>(); // A list to hold all the raycast results
    eventSystem.RaycastAll(pointerEventData, raycastResults); // Do a raycast using all enabled raycasters in the scene
    currentRaycast = pointerEventData.pointerCurrentRaycast = FindFirstRaycast(raycastResults); // Get the first hit an set both the local and pointerEventData results

    reticle.transform.position = centerEyeTransform.position + (centerEyeTransform.forward * currentRaycast.distance); // Move reticle
    float reticleSize = currentRaycast.distance * reticleSizeMultiplier;
    reticle.transform.localScale = new Vector3(reticleSize, reticleSize, reticleSize); //Scale reticle so it's always the same size

    ProcessMove(pointerEventData); // Pass the pointer data to the event system so entering and exiting of objects is detected
  }

  void HandleSelection() {
    if (pointerEventData.pointerEnter != null) {
      //Get the OnPointerClick handler of the entered object
      currentLookAtHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(pointerEventData.pointerEnter);

      if (currentLookAtHandler != null && OVRInput.GetDown(OVRInput.Button.One)) {
        // Object in sight with a OnPointerClick handler & pressed the main button
        ExecuteEvents.ExecuteHierarchy(currentLookAtHandler, pointerEventData, ExecuteEvents.pointerClickHandler);
      }
    }
    else {
      currentLookAtHandler = null;
    }
  }

}