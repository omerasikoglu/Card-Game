using System.Collections.Generic;
using RunTogether.Extensions;
using UnityEngine;

namespace RunTogether.Utils{

  public class ColliderCombiner : MonoBehaviour{
    [SerializeField] bool IsTriggerOnAwake = false;
    [SerializeField] bool IsPivotCentered  = false; // !: bound centered needs to object scale 1
    [SerializeField] PhysicsMaterial physicsMmaterial;

    void Awake(){
      var boxCollider  = gameObject.GetOrAdd<BoxCollider>();
      var allRenderers = transform.GetComponentsInChildren<MeshRenderer>();
      FitToChildren(allRenderers, boxCollider);
      boxCollider.isTrigger = IsTriggerOnAwake;
      boxCollider.material  = physicsMmaterial;
      Destroy(this);
    }

    void FitToChildren(MeshRenderer[] renderers, BoxCollider collider){
      var childrenSize = CalculateColliderSize(renderers);
      collider.size = childrenSize;

      var centerPosition = IsPivotCentered ? 
        transform.InverseTransformPoint(GetPivotCenter(renderers)) : GetBoundsCenter(renderers);
      collider.center = centerPosition;
    }

    Vector3 CalculateColliderSize(IEnumerable<MeshRenderer> renderers){
      var xBounds = new List<float>();
      var yBounds = new List<float>();
      var zBounds = new List<float>();

      foreach (var meshRenderer in renderers){
        var bounds = meshRenderer.bounds;
        xBounds.Add(bounds.min.x);
        xBounds.Add(bounds.max.x);
        yBounds.Add(bounds.min.y);
        yBounds.Add(bounds.max.y);
        zBounds.Add(bounds.min.z);
        zBounds.Add(bounds.max.z);
      }

      xBounds.Sort();
      yBounds.Sort();
      zBounds.Sort();

      var xSize = Mathf.Abs(xBounds[0] - xBounds[^1]);
      var ySize = Mathf.Abs(yBounds[0] - yBounds[^1]);
      var zSize = Mathf.Abs(zBounds[0] - zBounds[^1]);

      return new Vector3(xSize, ySize, zSize);
    }

    Vector3 GetPivotCenter(MeshRenderer[] renderers){
      var positionSum = new Vector3();

      foreach (var meshRenderer in renderers){
        var position = meshRenderer.transform.position;
        positionSum += position;
      }

      return positionSum / renderers.Length;
    }

    Vector3 GetBoundsCenter(MeshRenderer[] renderers){
      var bounds = new Bounds(renderers[0].bounds.center, Vector3.zero);

      foreach (var meshRenderer in renderers){
        bounds.Encapsulate(meshRenderer.bounds);
      }

      return transform.InverseTransformPoint(bounds.center);
    }
  }

}