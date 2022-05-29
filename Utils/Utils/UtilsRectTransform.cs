using UnityEngine;

namespace Utils.Utils
{
    public static class UtilsRectTransform 
    {
        public static void SetDeltaHeight(in RectTransform transform, float height)
        {
            var deltaSize = transform.sizeDelta;
            deltaSize.y = height;
            transform.sizeDelta = deltaSize;
        }

        public static void SetDeltaWidth(in RectTransform transform, float width)
        {
            var deltaSize = transform.sizeDelta;
            deltaSize.x = width;
            transform.sizeDelta = deltaSize;
        }

        public static void SetPivotVertical(in RectTransform transform, float verticalDistance)
        {
            var position = transform.anchoredPosition;
            position.y = verticalDistance;
            transform.anchoredPosition = position;
        }
        public static void SetPivotHorizontal(in RectTransform transform, float horizontalDistance)
        {
            var position = transform.anchoredPosition;
            position.x = horizontalDistance;
            transform.anchoredPosition = position;
        }
    }
}
