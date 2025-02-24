using UnityEngine;

namespace RunTogether.Extensions{

  public static class Vector3Extensions{
    public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null){
      return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }

    public static Vector3 OnlyWithX(this Vector3 vector) => Vector3.zero.With(x: vector.x);
    public static Vector3 OnlyWithY(this Vector3 vector) => Vector3.zero.With(y: vector.y);
    public static Vector3 OnlyWithZ(this Vector3 vector) => Vector3.zero.With(z: vector.z);

    public static Vector3 Add(this Vector3 vector, float x = 0, float y = 0, float z = 0){
      return new Vector3(vector.x + x, vector.y + y, vector.z + z);
    }

    public static bool InRangeOf(this Vector3 current, Vector3 target, float range){
      return (current - target).sqrMagnitude <= range * range;
    }
  }

}