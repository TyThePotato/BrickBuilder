using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrickBuilder.Commands;
using BrickBuilder.World;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace BrickBuilder.UI
{
    /// <summary>
    /// Handles all UI related things
    /// </summary>
    public class EditorUI : MonoBehaviour
    {
        // Singleton
        public static EditorUI instance;
        
        // General UI Stuff
        [Space(5)] 
        [Header("General UI")] 
        public Canvas MainCanvas;
        public RectTransform CanvasRectTransform;

        public const int SidebarTop = 96;
        
        public Color ShadedUI;
        public Color UnshadedUI;

        public Image SelectionBox;
        
        // Menu Bar Stuff
        [Space(5)] 
        [Header("Menu Bar")] 
        public Image[] MenuBarButtons;
        public TMP_Text AppInfoLabel;
        
        // Tool Bar Stuff
        [Space(5)] 
        [Header("Tool Bar")] 
        public GameObject[] ToolBarCategories;
        
        // General Side Bar Stuff
        // nothing yet lmao gottem
        
        // Hierarchy Stuff
        [Space(5)]
        [Header("Hierarchy Variables")]
        public RectTransform HierarchyTransform;
        public Transform HierarchyElementRoot; // Transform of object with LayoutGroup
        public GameObject HierarchyElementPrefab;

        public Color HierarchyElementColor;
        public Color HierarchyElementColorSelected;
        
        public HierarchyElement EnvironmentHierarchyElement;

        public Sprite BrickSprite;
        public Sprite EnvironmentSprite;
        public Sprite GroupSprite;
        
        private static List<HierarchyElement> HierarchyElements = new List<HierarchyElement>();
        
        // Inspector Stuff
        [Space(5)]
        [Header("Inspector Variables")]
        public RectTransform InspectorTransform;

        public InspectorElement[] InspectorEnvironmentElements;
        public InspectorElement[] InspectorBrickElements;

        // Misc Stuff
        private int framerate = 0;
        private int fpsFrameSampleCount = 15; // FPS counter shows the average fps of the last X frames
        private int currentFPSFrame = 0;
        private float collectiveDeltaTime = 0f;

        private StringBuilder appInfoSB;

        // ===============
        // Unity Functions
        // ===============

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            appInfoSB = new StringBuilder();
        }

        private void Start()
        {
            SwitchMenuBarTab(0); // Open "File" tab on start
            SetInspector(); // clear inspector
            
            // Set Inspector Color Element Callbacks
            InspectorEnvironmentElements[0].SetColorButtonCallback(ColorPickerUpdated);
            InspectorEnvironmentElements[1].SetColorButtonCallback(ColorPickerUpdated);
            InspectorEnvironmentElements[2].SetColorButtonCallback(ColorPickerUpdated);
            InspectorBrickElements[4].SetColorButtonCallback(ColorPickerUpdated);
        }

        private void Update()
        {
            // Calculate FPS
            collectiveDeltaTime += Time.deltaTime;
            currentFPSFrame++;
            if (currentFPSFrame >= fpsFrameSampleCount)
            {
                framerate = (int)(fpsFrameSampleCount / collectiveDeltaTime);
                collectiveDeltaTime = 0f;
                currentFPSFrame = 0;

                fpsFrameSampleCount = Mathf.Clamp((int) (framerate / 4f), 2, 60);
            }

            // Set Info Label
            appInfoSB.Clear();

            if (EditorMain.OpenedMap != null)
            {
                appInfoSB.AppendFormat("{0} Bricks | ", new object[]
                {
                    EditorMain.OpenedMap.Bricks.Count
                });
            }

            appInfoSB.AppendFormat("{0} FPS | {1} {2}", new object[]
            {
                framerate,
                Application.productName,
                Application.version
            });
            
            AppInfoLabel.SetText(appInfoSB.ToString());
        }

        // ==================
        // Menu Bar Functions
        // ==================

        // Call when clicking a menu bar tab
        // Displays the associated toolbar buttons
        public void SwitchMenuBarTab(int index)
        {
            // Set menu bar button colors
            for (int i = 0; i < MenuBarButtons.Length; i++)
            {
                MenuBarButtons[i].color = i == index ? UnshadedUI : ShadedUI;
            }
            
            // set active state of toolbar tabs
            for (int i = 0; i < ToolBarCategories.Length; i++)
            {
                ToolBarCategories[i].SetActive(i == index);
            }
        }
        
        // ==================
        // Tool Bar Functions
        // ==================

        // Clears current map and creates a new one
        public void NewFileButton()
        {
            EditorMain.NewFile();
        }

        // Loads a map from disk
        public void OpenFileButton()
        {
            // TODO: move elsewhere so this can be reused
            ExtensionFilter[] extensions = new[]
            {
                new ExtensionFilter("Brick-Hill Map", new[] {"brk", "bb"})
            };
            
            // async file dialog
            StandaloneFileBrowser.OpenFilePanelAsync("Open Map", "", extensions, false, file =>
            {
                if (file.Length > 0 && !string.IsNullOrEmpty(file[0]))
                    EditorMain.OpenFile(file[0]);
            });
        }

        // Saves map to the same file it was loaded from
        // If current map is a new map, call SaveAs
        public void SaveFileButton()
        {
            if (EditorMain.OpenedMap == null) 
                return;

            if (EditorMain.FilePath == "")
            {
                // Current map was not opened - Save as
                SaveAsFileButton();
            }
            else
            {
                // Save to current file
                EditorMain.SaveFile(EditorMain.FilePath);
            }
        }

        // Saves map to a new file
        public void SaveAsFileButton()
        {
            if (EditorMain.OpenedMap == null) 
                return;
            
            // TODO: move elsewhere so this can be reused
            ExtensionFilter[] extensions = new[]
            {
                new ExtensionFilter("Brick-Hill Map", new[] {"brk", "bb"})
            };
            
            // async save dialog
            StandaloneFileBrowser.SaveFilePanelAsync("Save Map", "", "New Map.brk", extensions, path =>
            {
                if (!string.IsNullOrEmpty(path))
                    EditorMain.SaveFile(path);
            });

            // TODO: extra settings dialog (brk version, bb data, etc)
        }

        public void NewBrickButton()
        {
            MapEditor.CreateBrick(true);
        }

        public void RemoveBrickButton()
        {
            if (MapEditor.SelectedBricks.Count == 0) return;

            MapEditor.RemoveBricks(MapEditor.SelectedBricks);
        }

        public void UndoButton()
        {
            CommandManager.HistoryBack();  
        }

        public void RedoButton()
        {
            CommandManager.HistoryForward();
        }

        // ===================
        // Hierarchy Functions
        // ===================

        // Arranges hierarchy elements based on their set position
        public static void ArrangeHierarchyElements()
        {
            // First reorder list by element position
            List<HierarchyElement> sortedList = HierarchyElements.OrderBy(element => element.Position).ToList();
            HierarchyElements = sortedList;
            
            // Then, reposition hierarchy elements
            // Because the list is now in order of position,
            // we can loop backwards and move each item to the start.
            // There is probably a better way of sorting the hierarchy,
            // but I thought this looked pretty cool. 
            for (int i = HierarchyElements.Count - 1; i >= 0; i--)
            {
                HierarchyElements[i].transform.SetAsFirstSibling();
            }
            
            // Finally, move environment element to top
            instance.EnvironmentHierarchyElement.transform.SetAsFirstSibling();
        }

        // Bulk adds hierarchy elements from map
        public static void AddHierarchyElements(Map map, bool createEnvironment = true)
        {
            // create environment
            if (createEnvironment)
            {
                CreateEnvironmentElement();
            }
            
            // create brick elements
            for (int i = 0; i < map.Bricks.Count; i++)
            {
                CreateHierarchyElement(map.Bricks[i]);
            }
        }
        
        // Creates a new hierarchy element
        public static void CreateHierarchyElement(Brick brick, bool environment = false)
        {
            GameObject elementGO = Instantiate(instance.HierarchyElementPrefab, instance.HierarchyElementRoot);
            elementGO.name = brick.Name;

            HierarchyElement element = elementGO.GetComponent<HierarchyElement>();
            element.ID = brick.ID;
            element.Position = HierarchyElements.Count + 1;
            
            element.Label.SetText(brick.Name);
            element.SetIcon(instance.BrickSprite, brick.Color);
            
            element.SelectButton.onClick.AddListener(delegate { HierarchyElementClicked(element, brick); });
            
            elementGO.SetActive(true);
            
            HierarchyElements.Add(element);
        }

        public static void CreateEnvironmentElement()
        {
            if (instance.EnvironmentHierarchyElement == null)
            {
                GameObject elementGO = Instantiate(instance.HierarchyElementPrefab, instance.HierarchyElementRoot);
                elementGO.name = "Environment Element";
                
                HierarchyElement element = elementGO.GetComponent<HierarchyElement>();
                element.ID = Guid.Empty;
                element.Position = 0;
            
                element.Label.SetText("Environment");
                element.SetIcon(instance.EnvironmentSprite, Color.white);
                
                element.SelectButton.onClick.AddListener(delegate { HierarchyElementClicked(element); });

                elementGO.SetActive(true);

                instance.EnvironmentHierarchyElement = element;
            }
        }

        // Removes the specified hierarchy element
        public static void RemoveHierarchyElement(Guid id)
        {
            HierarchyElement element = GetHierarchyElement(id);
            if (element != null)
            {
                Destroy(element.gameObject);
                HierarchyElements.Remove(element);
            }
        }

        public static void RemoveAllHierarchyElements()
        {
            for (int i = 0; i < HierarchyElements.Count; i++)
            {
                if (HierarchyElements[i] != null)
                    Destroy(HierarchyElements[i].gameObject);
            }
        }

        // Moves a hierarchy element
        public static void MoveHierarchyElement(Guid id, int position)
        {
            HierarchyElement element = GetHierarchyElement(id);
            if (element != null)
            {
                // Clamp position
                position = Mathf.Clamp(position, 0, HierarchyElements.Count);

                // Change element position
                element.Position = position;

                // Now we have to update all the elements ahead of this one :]
                for (int i = 0; i < HierarchyElements.Count; i++)
                {
                    if (HierarchyElements[i].Position >= position)
                    {
                        HierarchyElements[i].Position++;
                    }
                }
                
                // Finally, refresh hierarchy
                ArrangeHierarchyElements();
            }
        }

        public static void UpdateHierarchyElement(Guid id, string name, Sprite icon, Color color)
        {
            HierarchyElement element = GetHierarchyElement(id);
            if (element != null)
            {
                element.Label.SetText(name);
                element.SetIcon(icon, color);
            }
        }

        public static void HierarchyElementSelectState(HierarchyElement element, bool selected)
        {
            element.Selected = selected;
            element.Background.color = selected ? instance.HierarchyElementColorSelected : instance.HierarchyElementColor;
        }

        // Hierarchy Element click callback
        // If brick is null, element is assumed to be the environment element
        public static void HierarchyElementClicked(HierarchyElement element, Brick brick = null)
        {
            // select element
            if (MapEditor.addToSelection)
            {
                if (brick != null && instance.EnvironmentHierarchyElement.Selected)
                    return; // cant select brick & environment at the same time

                if (brick == null && MapEditor.SelectedBricks.Count > 0)
                    return; // see above
                
                if (element.Selected)
                {
                    // deselect element
                    HierarchyElementSelectState(element, false);
                    
                    if (brick != null)
                        MapEditor.DeselectBrick(brick, true);
                }
                else
                {
                    // add to selection
                    HierarchyElementSelectState(element, true);
                    
                    if (brick != null)
                        MapEditor.SelectBrick(brick, true, true);
                }
            }
            else
            {
                // select only this element
                MapEditor.DeselectAllBricks(true);
                SetAllHierarchyElementsSelected(false);
                HierarchyElementSelectState(element, true);
                
                if (brick != null)
                    MapEditor.SelectBrick(brick, false, true);
            }

            // update inspector
            if (brick == null)
            {
                // environment
                SetInspector(EditorMain.OpenedMap);
            }
            else
            {
                // brige :)
                SetInspector(MapEditor.SelectedBricks);
            }
        }

        public static void SetAllHierarchyElementsSelected(bool selected, bool includeEnvironment = true)
        {
            for (int i = 0; i < HierarchyElements.Count; i++)
            {
                HierarchyElementSelectState(HierarchyElements[i], selected);
            }
            
            if (includeEnvironment)
                HierarchyElementSelectState(instance.EnvironmentHierarchyElement, selected);
        }

        /// <summary>
        /// Returns hierarchy element with matching id.
        /// Null if none are found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static HierarchyElement GetHierarchyElement(Guid id)
        {
            for (int i = 0; i < HierarchyElements.Count; i++)
            {
                if (HierarchyElements[i].ID == id)
                {
                    return HierarchyElements[i];
                }
            }

            return null;
        }
        
        // ===================
        // Inspector Functions
        // ===================

        // Display map properties
        public static void SetInspector(Map map)
        {
            // clear inspector
            SetInspector();
            
            // enable map elements
            for (int i = 0; i < instance.InspectorEnvironmentElements.Length; i++)
            {
                instance.InspectorEnvironmentElements[i].gameObject.SetActive(true);
            }
            
            // YIKES hardcoded values!
            instance.InspectorEnvironmentElements[0].SetValue(map.AmbientColor);
            instance.InspectorEnvironmentElements[1].SetValue(map.BaseplateColor);
            instance.InspectorEnvironmentElements[2].SetValue(map.SkyColor);
            instance.InspectorEnvironmentElements[3].SetValue(map.BaseplateSize);
            instance.InspectorEnvironmentElements[4].SetValue(map.SunIntensity);
        }
        
        // Display Brick Properties
        public static void SetInspector(List<Brick> bricks)
        {
            // clear inspector
            SetInspector();

            // make sure there are actually bricks selected
            if (bricks.Count == 0) return;
            
            // enable brick elements
            for (int i = 0; i < instance.InspectorBrickElements.Length; i++)
            {
                instance.InspectorBrickElements[i].gameObject.SetActive(true);
            }
            
            // set properties to reflect last selected brick
            // single brick selected
            Brick lastBrick = bricks[bricks.Count - 1];
            instance.InspectorBrickElements[0].SetValue(lastBrick.Name);
            instance.InspectorBrickElements[1].SetValue(lastBrick.Position);
            instance.InspectorBrickElements[2].SetValue(lastBrick.Scale);
            instance.InspectorBrickElements[3].SetValue(lastBrick.Rotation.y);
            instance.InspectorBrickElements[4].SetValue(lastBrick.Color);
            instance.InspectorBrickElements[5].SetValue((int) lastBrick.Shape);
            instance.InspectorBrickElements[6].SetValue(lastBrick.Model);
            instance.InspectorBrickElements[7].SetValue(lastBrick.Collision);
        }

        // Display nothing
        public static void SetInspector()
        {
            // Iterate through all inspector elements and disable
            // TODO: find better way?
            
            for (int i = 0; i < instance.InspectorEnvironmentElements.Length; i++)
            {
                instance.InspectorEnvironmentElements[i].gameObject.SetActive(false);
            }
            
            for (int i = 0; i < instance.InspectorBrickElements.Length; i++)
            {
                instance.InspectorBrickElements[i].gameObject.SetActive(false);
            }
        }
        
        // ==================
        // Color Picker Stuff
        // ==================

        private static void ColorPickerUpdated(Color newColor, ColorPicker.ColorPickerMode mode)
        {
            switch (mode)
            {
                case ColorPicker.ColorPickerMode.Ambient:
                    EditorMain.OpenedMap.AmbientColor = newColor;
                    MapBuilder.SetEnvironment(EditorMain.OpenedMap);
                    break;
                case ColorPicker.ColorPickerMode.Baseplate:
                    EditorMain.OpenedMap.BaseplateColor = newColor;
                    MapBuilder.SetEnvironment(EditorMain.OpenedMap);
                    break;
                case ColorPicker.ColorPickerMode.Sky:
                    EditorMain.OpenedMap.SkyColor = newColor;
                    MapBuilder.SetEnvironment(EditorMain.OpenedMap);
                    break;
                case ColorPicker.ColorPickerMode.Brick:
                    MapEditor.ChangeSelectionColor(newColor);
                    break;
            }
        }
        
        // ====================
        // General UI Functions
        // ====================

        /// <summary>
        /// Edits anchors of Hierarchy and Inspector to adjust height
        /// </summary>
        /// <param name="position"></param>
        public static void SetHierarchyInspectorDivider(float position)
        {
            Vector2 hierarchyAnchor = instance.HierarchyTransform.anchorMin;
            hierarchyAnchor.y = position;
            instance.HierarchyTransform.anchorMin = hierarchyAnchor;
            
            Vector2 inspectorAnchor = instance.InspectorTransform.anchorMax;
            inspectorAnchor.y = position;
            instance.InspectorTransform.anchorMax = inspectorAnchor;
        }

        public static void ShowScreenSelectionBox()
        {
            instance.SelectionBox.gameObject.SetActive(true);
            instance.SelectionBox.rectTransform.sizeDelta = new Vector2(0, 0);
        }

        public static void UpdateScreenSelectionBox(Vector2 start, Vector2 end)
        {
            float width = end.x - start.x;
            float height = end.y - start.y;

            instance.SelectionBox.rectTransform.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            instance.SelectionBox.rectTransform.anchoredPosition = start + new Vector2(width / 2f, height / 2f);
        }

        public static void HideScreenSelectionBox()
        {
            instance.SelectionBox.gameObject.SetActive(false);
        }

    }
}