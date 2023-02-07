using BrickBuilder.User;
using BrickBuilder.UI;
using BrickBuilder.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using BrickBuilder.Commands;
using BrickBuilder.Rendering;
using BrickBuilder.Utilities;
using RuntimeGizmos;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace BrickBuilder.World
{
    
    /// <summary>
    /// Handles the editing of a loaded map.
    /// Has functions for brick creation, modification, deletion, etc.
    /// </summary>
    public class MapEditor : MonoBehaviour
    {
        public static MapEditor instance;
        
        public static List<Brick> SelectedBricks = new List<Brick>();
        public static List<Chunk> SelectedChunks = new List<Chunk>();
        
        public LayerMask SelectionLayerMask;
        public float SelectionOffset = 0.01f;

        public static bool CanDragSelect = false;
        private static bool selectButtonDown = false; // for supporting drag selecting
        private static Vector2 mouseBeginPosition;
        
        public static bool addToSelection = false;

        private void Awake()
        {
            // singleton
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
            
            //InputHelper.select.performed += _ => TrySelectBrick();

            InputHelper.select.started += _ => SelectButtonBegin();
            InputHelper.select.canceled += _ => SelectButtonEnd();
            InputHelper.cancel.performed += _ => DeselectAllBricks();

            // InputSystem moment
            InputHelper.selectionAdd.started += _ => addToSelection = true;
            InputHelper.selectionAdd.canceled += _ => addToSelection = false;
        }

        private void Update()
        {
            // Update drag selection box
            if (selectButtonDown && CanDragSelect)
            {
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                
                // only update selection box if the mouse moves
                if (mouseDelta.x != 0 || mouseDelta.y != 0)
                {
                    EditorUI.UpdateScreenSelectionBox(mouseBeginPosition, Mouse.current.position.ReadValue());
                }
            }
        }
        
        // ===============
        // BRICK SELECTION
        // ===============

        private static void SelectButtonBegin()
        {
            selectButtonDown = true;

            mouseBeginPosition = Mouse.current.position.ReadValue();
            
            if (CanDragSelect)
                EditorUI.ShowScreenSelectionBox();
        }

        private static void SelectButtonEnd()
        {
            selectButtonDown = false;

            Vector2 mousePosition = Mouse.current.position.ReadValue();

            if (EditorMain.OpenedMap != null) {
                if (mousePosition == mouseBeginPosition) {
                    // select brick under cursor
                    TrySelectBrick();
                } else if (CanDragSelect) {
                    // select area of bricks
                    TrySelectArea(mouseBeginPosition, mousePosition);
                }
            }
            
            EditorUI.HideScreenSelectionBox();
        }

        private static void TrySelectArea(Vector2 screenStart, Vector2 screenEnd)
        {
            // deselect all bricks if desired
            if (!addToSelection)
            {
                DeselectAllBricks();
            }
            
            // calculate smallest and largest box selection points
            Vector2 min = Vector2.Min(screenStart, screenEnd);
            Vector2 max = Vector2.Max(screenStart, screenEnd);
            
            // Iterate through all bricks in map
            for (int i = 0; i < EditorMain.OpenedMap.Bricks.Count; i++)
            {
                // get position of brick in screen coordinates
                Vector3 brickScreenPosition =
                    EditorCamera.Camera.WorldToScreenPoint(EditorMain.OpenedMap.Bricks[i].Position);
                
                // check if brick lies within selection box bounds
                if (brickScreenPosition.x > min.x && brickScreenPosition.x < max.x &&
                    brickScreenPosition.y > min.y && brickScreenPosition.y < max.y)
                {
                    // select brick (without generating selection chunks yet)
                    SelectBrick(EditorMain.OpenedMap.Bricks[i], true, false, false);
                }
            }
        }
        
        private static void TrySelectBrick()
        {
            // check if mouse is over ui
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            // select brick under mouse using raycast
            RaycastHit hit;
            Ray ray = EditorCamera.Camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit, 1000f, instance.SelectionLayerMask))
            {
                // there is a brick under the cursor :]

                // in order for GetBrickFromPoint function to work right, we must offset
                // the hit point a little so that it is inside of the target brick
                Vector3 offsetPoint = hit.point + ray.direction.normalized * 0.001f; // if a brick is smaller than this, it aint getting selected sorry bud
                Brick hitBrick = GetBrickFromPoint(hit.point, offsetPoint);

                if (hitBrick == null)
                {
                    // probably clicked baseplate or something
                    // deselect all if shift not held
                    if (!addToSelection)
                        DeselectAllBricks();
                    
                    return;
                }

                if (ColorPicker.CurrentMode == ColorPicker.ColorPickerMode.Paintbrush)
                {
                    // paint brick
                }
                else
                {
                    // select / deselect brick

                    if (!addToSelection)
                    {
                        // When not holding shift, attempt to select brick regardless of if its selected
                        // This is so that in the event that multiple bricks are selected, clicking a 
                        // brick will deselect everything else and select only that one brick.
                        SelectBrick(hitBrick, false);
                    }
                    else
                    {
                        if (SelectedBricks.Contains(hitBrick))
                        {
                            // When holding shift and brick is already selected, deselect it
                            DeselectBrick(hitBrick);
                        }
                        else
                        {
                            // When holding shift and brick is not selected, add to selection
                            SelectBrick(hitBrick, true);
                        }
                    }
                }
            }
            else
            {
                // clicked literally nothing, what a loser
                // deselect all if shift not held
                if (!addToSelection)
                {
                    DeselectAllBricks();
                }
            }
        }

        public static void SelectBrick(Brick brick, bool addToSelection = false, bool fromUI = false, bool generateChunks = true)
        {
            // Check if brick is being added to selection or replacing selection
            if (!addToSelection) DeselectAllBricks();
            
            // Ensure target brick isn't already selected
            if (SelectedBricks.Contains(brick)) return;

            SelectedBricks.Add(brick);
            
            // add glow effect to brick
            MapBuilder.SetBrickGlow(brick, true);
            
            // update inspector
            if (!fromUI)
                EditorUI.HierarchyElementSelectState(EditorUI.GetHierarchyElement(brick.ID), true);
            //EditorUI.SetInspector(SelectedBricks);
            
            // Update gizmos
            TransformGizmo.instance.AddTarget(MapBuilder.BrickGOs[brick.ID].transform, brick);
        }

        public static void DeselectBrick(Brick brick, bool fromUI = false)
        {
            // Ensure target brick is selected
            if (!SelectedBricks.Contains(brick)) return;
            
            // remove glow effect from brick
            MapBuilder.SetBrickGlow(brick, false);
            
            // remove brick from selection
            SelectedBricks.Remove(brick);

            // update ui
            if (!fromUI)
                EditorUI.HierarchyElementSelectState(EditorUI.GetHierarchyElement(brick.ID), false);
            //EditorUI.SetInspector(SelectedBricks);
            
            // Update gizmos
            TransformGizmo.instance.RemoveTarget(MapBuilder.BrickGOs[brick.ID].transform);
        }

        public static void DeselectAllBricks(bool fromUI = false)
        {
            // Ensure there are selected bricks
            if (SelectedBricks.Count == 0) return;

            // Remove glow effect from bricks
            foreach (Brick brick in SelectedBricks) {
                MapBuilder.SetBrickGlow(brick, false);
            }
            
            // remove all selected bricks from selection
            SelectedBricks.Clear();

            // update ui
            if (!fromUI)
                EditorUI.SetAllHierarchyElementsSelected(false);
            //EditorUI.SetInspector();
            
            // Update gizmos
            TransformGizmo.instance.ClearTargets();
        }

        // Gets the brick that the point is touching
        // Takes a point that is TOUCHING the brick and a point that is INSIDE the brick
        // For bricks larger than the selection offset length, the inside point is used to determine the brick
        // For smaller bricks, the touching point is used
        private static Brick GetBrickFromPoint(Vector3 point, Vector3 pointOffset)
        {
            // store a list of potential bricks so we can intelligently select an appropriate one
            // TODO: use some sort of oriented bounding box system so rotations work nicely
            List<Brick> hits = new List<Brick>();
            
            // use meshrenderer bounds as a trick to get bounds that scale with rotation
            GameObject boundsGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
            MeshRenderer boundsRenderer = boundsGO.GetComponent<MeshRenderer>();

            // iterate through all bricks
            for (int i = 0; i < EditorMain.OpenedMap.Bricks.Count; i++)
            {
                // set transform properties
                boundsGO.transform.position = EditorMain.OpenedMap.Bricks[i].Position;
                boundsGO.transform.localScale = EditorMain.OpenedMap.Bricks[i].Scale;
                boundsGO.transform.eulerAngles = EditorMain.OpenedMap.Bricks[i].Rotation;

                // determine point to use
                bool thickBrick = (boundsRenderer.bounds.size.x > instance.SelectionOffset &&
                                   boundsRenderer.bounds.size.y > instance.SelectionOffset &&
                                   boundsRenderer.bounds.size.z > instance.SelectionOffset);

                // check if point intersects brick
                if (boundsRenderer.bounds.Contains(thickBrick ? pointOffset : point))
                {
                    hits.Add(EditorMain.OpenedMap.Bricks[i]);
                }
            }

            // bounds go no longer needed
            Destroy(boundsGO);

            // check if there were any hits
            if (hits.Count > 0)
            {
                // check if theres only 1 hit first
                if (hits.Count == 1)
                {
                    return hits[0];
                }
                
                // there are multiple hits, go with smallest one since that's the one the user
                // *probably* wanted to click. if not, SUCKS TO BE YOU
                
                Brick smallestHit = hits[0];
                
                // NO i dont mean to multiply
                // trust me i know what im doing
                float smallestHitSize = smallestHit.Scale.x + smallestHit.Scale.y + smallestHit.Scale.z;

                for (int i = 0; i < hits.Count; i++)
                {
                    // if a scale is somehow negative, SUCKS TO BE YOU
                    float hitSize = hits[i].Scale.x + hits[i].Scale.y + hits[i].Scale.z;
                    
                    // compare to size of last checked hit
                    if (hitSize < smallestHitSize)
                    {
                        smallestHit = hits[i];
                        smallestHitSize = hitSize;
                    }
                }

                return smallestHit;
            }
            
            return null; // no brick
        }
        
        // ==============
        // BRICK CREATION
        // ==============
        
        public static Brick CreateBrick(Vector3 position, Vector3 scale)
        {
            // Create Brick
            Brick brick = new Brick();
            brick.Position = position;
            brick.Scale = scale;

            // create and register command
            Command command = CommandManager.RegisterCommand(new List<Brick>() {brick}, false);
            
            // run command
            CommandManager.DoCommand(command);
            
            return brick;
        }

        public static Brick CreateBrick(bool raycastPosition = false)
        {
            Vector3 pos = Vector3.zero;
            if (raycastPosition)
            {
                RaycastHit hit;
                Ray ray = EditorCamera.Camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
                if (Physics.Raycast(ray, out hit, 1000f, instance.SelectionLayerMask))
                {
                    pos = hit.point;
                    
                    // round all values that will not be changed
                    // i hate this i hate this i hate this so much
                    if (hit.normal.x == 0f)
                    {
                        pos.x = Mathf.Round(pos.x) + 0.5f;
                    }
                    if (hit.normal.y == 0f)
                    {
                        pos.y = Mathf.Round(pos.y) + 0.5f;
                    }
                    if (hit.normal.z == 0f)
                    {
                        pos.z = Mathf.Round(pos.z) + 0.5f;
                    }
                    
                    // offset position so that brick is touching point rather than inside point
                    Vector3 offset = hit.normal;
                    offset.Scale(new Vector3(0.5f, 0.5f, 0.5f));
                    pos += offset;
                }
                else
                {
                    pos = EditorCamera.Camera.transform.TransformPoint(new Vector3(0f, 0f, 10f));
                }
            }
            
            return CreateBrick(pos, Vector3.one);
        }

        public static void ImportBricks(List<Brick> bricks, bool newID = true)
        {
            for (int i = 0; i < bricks.Count; i++) {
                Brick copy = bricks[i].Clone(); // use a copy so that original bricks dont get edited over
                
                if (newID)
                    copy.ID = Guid.NewGuid();
                
                EditorMain.OpenedMap.Bricks.Add(copy);
                EditorUI.CreateHierarchyElement(copy,false);
                MapBuilder.BuildBrick(copy);
            }
        }

        public static void RemoveBrick(Brick brick, bool registerCommand = true)
        {
            if (registerCommand)
            {
                // register brick deletion command THEN delete brick
                Command command = CommandManager.RegisterCommand(new List<Brick>() {brick}, true);
                CommandManager.DoCommand(command);
            }
            else
            {
                // actually delete brick
                
                // Delete Brick GameObject
                MapBuilder.RemoveBrick(brick.ID);

                // delete hierarchy element
                EditorUI.RemoveHierarchyElement(brick.ID);

                // Delete Brick Object
                Brick targetBrick = EditorMain.OpenedMap.GetBrick(brick.ID);
                if (targetBrick != null)
                    EditorMain.OpenedMap.Bricks.Remove(targetBrick);
            }
        }

        public static void RemoveBricks(List<Brick> bricks, bool registerCommand = true)
        {
            if (registerCommand)
            {
                // register brick deletion command THEN delete bricks
                Command command = CommandManager.RegisterCommand(bricks, true);
                CommandManager.DoCommand(command);
            }
            else
            {
                // ACTUALLY delete bricks

                for (int i = 0; i < bricks.Count; i++) {
                    MapBuilder.RemoveBrick(bricks[i].ID);
                    EditorUI.RemoveHierarchyElement(bricks[i].ID);

                    Brick targetBrick = EditorMain.OpenedMap.GetBrick(bricks[i].ID); // is this slow?
                    if (targetBrick != null)
                        EditorMain.OpenedMap.Bricks.Remove(targetBrick);
                }
            }
        }
        
        // ==================
        // BRICK MODIFICATION
        // ==================

        public static void UpdateBricks(List<Brick> bricks)
        {
            // is this overkill
            for (int i = 0; i < bricks.Count; i++) {
                // Copy brick data
                EditorMain.OpenedMap.GetBrick(bricks[i].ID).Copy(bricks[i]);
                
                // Update visual
                MapBuilder.UpdateBrick(bricks[i].ID);
            }
        }
        
        public static void UpdateBrick(Brick brick)
        {
            // Copy brick data
            EditorMain.OpenedMap.GetBrick(brick.ID).Copy(brick);
            
            // Update visual
            MapBuilder.UpdateBrick(brick.ID);
        }
        
        // ==================
        // WORLD MODIFICATION
        // ==================

        public static void UpdateEnvironment(Color ambientColor, Color baseplateColor, Color skyColor,
            int baseplateSize, int sunIntensity)
        {
            // TODO
        }
        
        public static void ChangeSelectionColor(Color color)
        {
            // make sure bricks are selected
            if (SelectedBricks.Count == 0) return;
            
            // edit selected bricks
            for (int i = 0; i < SelectedBricks.Count; i++)
            {
                SelectedBricks[i].Color = color;
                MapBuilder.UpdateBrick(SelectedBricks[i].ID);
            }
        }
        
        // DEBUG
        private void DrawBounds(Bounds bounds)
        {
            Vector3 corner1 = bounds.min;
            Vector3 corner2 = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            Debug.DrawLine(corner1, corner2, Color.green, 30f, true);

            corner2 = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            Debug.DrawLine(corner1, corner2, Color.green, 30f, true);
            
            corner2 = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            Debug.DrawLine(corner1, corner2, Color.green, 30f, true);

            corner1 = bounds.max;
            corner2 = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
            Debug.DrawLine(corner1, corner2, Color.green, 30f, true);
            
            corner2 = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            Debug.DrawLine(corner1, corner2, Color.green, 30f, true);
            
            corner2 = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
            Debug.DrawLine(corner1, corner2, Color.green, 30f, true);
        }
    }
}