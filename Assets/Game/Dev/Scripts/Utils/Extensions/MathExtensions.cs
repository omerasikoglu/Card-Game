using System;
using UnityEngine;

[Serializable] public struct MinMax{
  public float Min;
  public float Max;

  public MinMax(float min, float max){
    Min = min;
    Max = max;
  }
}

namespace RunTogether.Extensions{

  public static class MathExtensions{

    public static float Ceil100(this float number) => Mathf.Ceil(number / 100f) * 100f; // 250 -> 300 | 249 -> 200
    public static float Ceil50(this float number) => Mathf.Ceil(number / 50f) * 50f;
    
    public static float Floor100(this float number) => Mathf.Floor(number / 100f) * 100f;

    public static int Ceil100(this int number) => (int) (Mathf.Ceil((float)number / 100f) * 100f);

    public static float Tan(this float degree){
      float angleRadians = Mathf.Deg2Rad * degree;
      return Mathf.Tan(angleRadians);
    }

    public static float Csc(this float degree){
      float angleRadians = Mathf.Deg2Rad * degree; // Convert angles to radians
      float sinValue     = Mathf.Sin(angleRadians);
      float cscValue     = 1.0f / sinValue;
      return cscValue;
    }
  }

}