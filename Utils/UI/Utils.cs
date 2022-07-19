using UnityEngine;

namespace Utils.UI
{
    public static class UtilsUserInterface
    {
        public static Vector3 CalculateWorldToScreenPoint(Camera camera, Vector3 targetPoint)
        {
            return camera.WorldToScreenPoint(targetPoint);
        }
    }
}
