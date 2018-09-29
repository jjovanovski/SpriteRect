using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteRect;

namespace SpriteRect {

    public enum Stretch {
        NONE,
        WIDTH,
        HEIGHT,
        BOTH
    };

    public enum Anchor {
        TOP_LEFT,       TOP_CENTER,         TOP_RIGHT,
        CENTER_LEFT,    CENTER_CENTER,      CENTER_RIGHT,
        BOTTOM_LEFT,    BOTTOM_CENTER,      BOTTOM_RIGHT
    };

    [ExecuteInEditMode]
    public class SpriteRect : MonoBehaviour {
        
        public Rect rect = new Rect(0, 0, 1, 1);

        public Stretch stretch;
        public Anchor anchor;
        
        void Start() {
        }

        void Update() {
        }

        void OnValidate() {
            rect.UpdateBounds();
        }

    }

}