using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteRect {

    [System.Serializable]
    public class Rect {

        [SerializeField]
        private float x, y, width, height;

        [SerializeField]
        [HideInInspector]
        private float xmin, xmax, ymin, ymax;

        public Rect(float x, float y, float width, float height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            UpdateBounds();
        }

        public void Set(float x, float y, float width, float height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            UpdateBounds();
        }

        public void Set(Rect rect) {
            this.x = rect.x;
            this.y = rect.y;
            this.width = rect.width;
            this.height = rect.height;

            UpdateBounds();
        }

        public static bool operator ==(Rect lhs, Rect rhs) {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
        }
        public static bool operator !=(Rect lhs, Rect rhs) {
            return lhs.x != rhs.x || lhs.y != rhs.y || lhs.width != rhs.width || lhs.height != rhs.height;
        }

        public void UpdateBounds() {
            float halfWidth = width / 2f;
            float halfHeight = height / 2f;

            xmin = x - halfWidth;
            xmax = x + halfWidth;
            ymin = y - halfHeight;
            ymax = y + halfHeight;
        }

        public void SetVerts(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3) {
            width = v1.x - v0.x;
            height = v0.y - v3.y;
            x = v0.x + width / 2f;
            y = v2.y + height / 2f;

            UpdateBounds();
        }

        public float GetX() {
            return x;
        }

        public float GetY() {
            return y;
        }

        public float GetWidth() {
            return width;
        }

        public float GetHeight() {
            return height;
        }
        
        public float GetXMin() {
            return xmin;
        }

        public float GetXMax() {
            return xmax;
        }

        public float GetYMin() {
            return ymin;
        }

        public float GetYMax() {
            return ymax;
        }

    }

}
