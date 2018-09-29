using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SpriteRect {

    [CustomEditor(typeof(SpriteRect))]
    public class SpriteRectEditor : Editor {

        private SpriteRect spriteRect;
        private Rect rect;
        private Rect cachedRect;

        private Vector3[] verts;

        private int editVertex;
        private Vector3[] originalVerts;
        private Vector3 originalMousePos;
        private bool alt;

        void OnEnable() {
            spriteRect = (SpriteRect)target;
            rect = spriteRect.rect;

            cachedRect = new Rect(0, 0, 0, 0);
            cachedRect.Set(rect);

            verts = new Vector3[4];
            for(int i = 0; i < 4; i++) {
                verts[i] = new Vector3();
            }
            ReadVertices();

            originalVerts = new Vector3[4];
            for(int i = 0; i < 4; i++) {
                originalVerts[i] = new Vector3();
            }

            editVertex = -1;
            alt = false;
        }
        
        void OnSceneGUI() {
            Event curr = Event.current;
            Vector3 mousePos = HandleUtility.GUIPointToWorldRay(curr.mousePosition).origin;
            mousePos.z = 0;
            
            if(cachedRect != rect) {
                ReadVertices();
                cachedRect.Set(rect);
                return;
            }

            Handles.color = Color.cyan;

            // draw the lines and find the closest vertex to mouse position
            int selectedVertIndex = -1;
            float selectedVertDistance = float.MaxValue;
            for (int i = 0; i < 4; i++) {
                float distanceToMouse = Vector3.Distance(mousePos, verts[i]);
                if (distanceToMouse < 0.1f) {
                    if(distanceToMouse < selectedVertDistance) {
                        selectedVertDistance = distanceToMouse;
                        selectedVertIndex = i;
                    }
                }
            }

            if (curr.button == 0 && curr.type == EventType.MouseDown) {
                if(selectedVertIndex != -1) {
                    if(editVertex == -1) {
                        editVertex = selectedVertIndex;
                        SaveOriginal(mousePos);
                        //Debug.Log("Started editing");
                    }
                }
            } else if(curr.button == 0 && curr.type == EventType.MouseUp) {
                if(editVertex != -1) {
                    editVertex = -1;
                    Undo.RecordObject(spriteRect, "Edit Rect");
                    WriteVertices();
                    //Debug.Log("Done editing");
                }
            }

            if(curr.keyCode == KeyCode.Escape && editVertex != -1) {
                editVertex = -1;
                for(int i = 0; i < 4; i++) {
                    verts[i] = originalVerts[i];
                }
            }

            if (curr.type == EventType.MouseDrag && editVertex != -1) {
                Vector3 mouseShift = mousePos - originalMousePos;
                if(curr.shift) {
                    Vector3 newPos = originalVerts[editVertex] + Vector3.Project(mousePos - originalVerts[editVertex], originalVerts[editVertex] - originalVerts[mod4(editVertex + 2)]);
                    mouseShift = newPos - originalVerts[editVertex];
                }

                if (curr.alt) {
                    if (!alt) alt = true;
                    ShiftVertex(mod4(editVertex + 2), -mouseShift);
                } else if (alt) {
                    for (int i = 0; i < 4; i++) {
                        verts[i] = originalVerts[i];
                    }
                    alt = false;
                }
                ShiftVertex(editVertex, mouseShift);
            }
            
            // draw the vertices buttons
            for (int i = 0; i < 4; i++) {
                Handles.DrawLine(verts[i], verts[mod4(i + 1)]);
                Handles.Button(verts[i], Quaternion.identity, 0.07f, 0.03f, Handles.CylinderHandleCap);
            }
            
            HandleUtility.Repaint();
        }

        private void ReadVertices() {
            verts[0].Set(rect.GetXMin(), rect.GetYMax(), 0);
            verts[1].Set(rect.GetXMax(), rect.GetYMax(), 0);
            verts[2].Set(rect.GetXMax(), rect.GetYMin(), 0);
            verts[3].Set(rect.GetXMin(), rect.GetYMin(), 0);
        }

        private void WriteVertices() {
            rect.SetVerts(verts[0], verts[1], verts[2], verts[3]);
            cachedRect.Set(rect);
        }

        private void SaveOriginal(Vector3 mousePos) {
            this.originalMousePos = mousePos;

            for (int i = 0; i < 4; i++) {
                originalVerts[i] = verts[i];
            }
        }

        private void ShiftVertex(int index, Vector3 shift) {
            verts[index] = originalVerts[index] + shift;

            if (index % 2 == 0) {
                verts[mod4(index - 1)].x = verts[index].x;
                verts[mod4(index + 1)].y = verts[index].y;
            } else {
                verts[mod4(index - 1)].y = verts[index].y;
                verts[mod4(index + 1)].x = verts[index].x;
            }
        }

        private int mod4(int n) {
            if (n >= 0)
                return n % 4;
            return (n % 4 + 4)%4;
        }

    }

}