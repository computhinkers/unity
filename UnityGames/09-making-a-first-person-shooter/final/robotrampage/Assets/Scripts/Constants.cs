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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {
  // Scenes
  public const string SceneBattle = "Battle";
  public const string SceneMenu = "MainMenu";

  // Gun Types
  public const string Pistol = "Pistol";
  public const string Shotgun = "Shotgun";
  public const string AssaultRifle = "AssaultRifle";

  // Robot Types
  public const string RedRobot = "RedRobot";
  public const string BlueRobot = "BlueRobot";
  public const string YellowRobot = "YellowRobot";

  // Pickup Types
  public const int PickUpPistolAmmo = 1;
  public const int PickUpAssaultRifleAmmo = 2;
  public const int PickUpShotgunAmmo = 3;
  public const int PickUpHealth = 4;
  public const int PickUpArmor = 5;

  // Misc
  public const string Game = "Game";
  public const float CameraDefaultZoom = 60f;

  public static readonly int[] AllPickupTypes = new int[5] {
    PickUpPistolAmmo,
    PickUpAssaultRifleAmmo,
    PickUpShotgunAmmo,
    PickUpHealth,
    PickUpArmor
  };
}
