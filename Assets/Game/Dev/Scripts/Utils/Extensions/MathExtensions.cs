using UnityEngine;

namespace RunTogether.Extensions{

  public static class MathExtensions{
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