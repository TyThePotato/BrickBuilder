using RuntimeGizmos;
using SFB; // Standalone File Browser
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Utils;

public class EditorUI : MonoBehaviour
{
    public static EditorUI instance;
    public EditorMain main;

    // ui stuff
    public Canvas MainCanvas;
    public CanvasScaler MainCanvasScaler;

    public Image[] MenuButtons;
    public GameObject[] ToolbarButtons;

    public GameObject Sidebar;
    public RectTransform AxisGizmo;
    public TMP_Text EditorInfo;

    public Sprite SelectedMenuButtonSprite;
    public Sprite UnselectedMenuButtonSprite;

    public GameObject InputBlocker;

    public AddToGroupMenu groupMenu;
    public BrickGroup[] groupMenuGroups;

    public GameObject avatarImporterGO;
    public AvatarImporter avatarImporter;
    public TMP_InputField avatarIDField;
    public Toggle avatarToolToggle;

    public RectTransform TransformPropertyLabel;
    public TMP_Text TransformPropertyLabelAxis;
    public TMP_Text TransformPropertyLabelValue;

    public TMP_InputField FileField;
    public int FileFieldType;

    // 3D Stuff
    public LayerMask SelectionLayerMask;
    public LayerMask BrickRaycastMask;
    public float SelectionDistance = 500f;

    public TransformGizmo gizmo;

    // hierarchy stuff

    public Transform Hierarchy;

    public Color SelectedHierarchyElementColor;
    public Sprite SelectedHierarchyElementSprite;
    public Color UnselectedHierarchyElementColor;

    public Sprite[] HierarchyIcons;

    public GameObject HierarchyElementTemplate;
    public GameObject GroupHierarchyElementTemplate;
    public Dictionary<int, HierarchyElement> HierarchyElements = new Dictionary<int, HierarchyElement>();
    public List<HierarchyElement> SelectedElements = new List<HierarchyElement>();
    public bool WorldIsSelected = false; // self explanatory

    // inspector stuff
    public GameObject MismatchedElementsLabel;
    public InspectorElement[] WorldInspectorElements;
    public InspectorElement[] BrickInspectorElements;
    public InspectorElement[] GroupInspectorElements;

    public ColorPicker colorPicker;
    public GameObject colorPickerGO;
    public ColorPickerTarget colorPickerTarget;

    // preferences stuff
    public GameObject Preferences;

    public TMP_InputField StudSnapField;
    public TMP_InputField RotationSnapField;
    public Toggle CopySuffixToggle;
    public Toggle CopyToWorkshopFormatToggle;

    public Slider CameraSpeedSlider;
    public TMP_Text CameraSpeedLabel;

    public Slider CameraSensitivitySlider;
    public TMP_Text CameraSensitivityLabel;

    public Slider FOVSlider;
    public TMP_Text FOVLabel;

    public Slider ViewDistanceSlider;
    public TMP_Text ViewDistanceLabel;

    public Toggle AutosaveToggle;

    public Slider AutosaveRateSlider;
    public TMP_Text AutosaveRateLabel;

    public Toggle FramelimiterToggle;
    public TMP_InputField FramelimiterField;

    public Slider ScreenshotSizeMultiplierSlider;
    public TMP_Text ScreenshotSizeMultiplierLabel;

    public TMP_Text[] HotkeyLabels;
    public Toggle[] HotkeyToggles;

    public TMP_Text PlayerPathLabel;
    public TMP_Text ServerPathLabel;

    // not ui stuff
    public PostProcessLayer ppl; // for screenshots
    public PostProcessVolume ppv; // for screenshots

    private int frameCount = 0;
    private float dt = 0.0f;
    public float fps = 0.0f;
    public float updateRate = 4.0f;

    private void Awake() {
        // singleton
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }

        // events
        main.MapLoaded.AddListener(InitializeUI);
        ColorPicker.colorChanged.AddListener(ColorChanged);
        BBInputManager.keyRebinded.AddListener(KeyRebinded);

        // hotkeys
        BBInputManager.Controls.EditorKeys.NewBrick.performed += ctx => NewBrickButton();
        BBInputManager.Controls.EditorKeys.DeleteBrick.performed += ctx => DeleteSelection();
        BBInputManager.Controls.EditorKeys.SaveFile.performed += ctx => SaveFileButton(false);
        BBInputManager.Controls.EditorKeys.OpenFile.performed += ctx => OpenFileButton(false);
        BBInputManager.Controls.EditorKeys.SelectAll.performed += ctx => SelectAllElements();
        BBInputManager.Controls.EditorKeys.Cancel.performed += ctx => {
            DeselectAllElements();
        };
        BBInputManager.Controls.EditorKeys.Copy.performed += ctx => CopySelection();
        BBInputManager.Controls.EditorKeys.Paste.performed += ctx => PasteSelection();
        BBInputManager.Controls.EditorKeys.Undo.performed += ctx => Undo();
        BBInputManager.Controls.EditorKeys.Redo.performed += ctx => Redo();
        BBInputManager.Controls.EditorKeys.Translate.performed += ctx => ChangeGizmo(0);
        BBInputManager.Controls.EditorKeys.Scale.performed += ctx => ChangeGizmo(1);
        BBInputManager.Controls.EditorKeys.Rotate.performed += ctx => ChangeGizmo(2);

        LoadPreferences();
    }

    public void Update() {
        // Fps Counter
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0f / updateRate) {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
        int brickCount = main.LoadedMap.Bricks.Count;
        EditorInfo.text = $"{brickCount} Bricks | {(int)fps} FPS | BrickBuilder " + Application.version;

        // Disable controls if typing in an InputField
        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null) {
            if (BBInputManager.ControlsEnabled) {
                BBInputManager.DisableControls();
            }
        } else {
            if (!BBInputManager.ControlsEnabled) {
                BBInputManager.EnableControls();
            }
        }

        // Click Detection
        if (Mouse.current.leftButton.wasPressedThisFrame) { // lmb clicked
            if (!gizmo.isTransforming) { // not using handles
                if (!EventSystem.current.IsPointerOverGameObject()) { // not over ui
                    TrySelectBrick();
                }
            }
        }

        // Update inspector when rotating bricks
        if (gizmo.isTransforming) {
            if (SelectedElements.Count > 0) {
                UpdateInspector();
            }
        }

        // move tooltips and stuff to mouse
        if (TransformPropertyLabel.gameObject.activeSelf) {
            Vector2 targetPos = Mouse.current.position.ReadValue() + new Vector2(15, -15);
            targetPos = targetPos.Clamp(new Vector2(0,TransformPropertyLabel.rect.height * MainCanvas.scaleFactor), new Vector2(Screen.width - TransformPropertyLabel.rect.width * MainCanvas.scaleFactor, Screen.height));
            TransformPropertyLabel.position = targetPos;
        }
    }

    public void InitializeUI () {
        PopulateHierarchy();
    }

    public void ChangeMenu (int index) {
        for (int i = 0; i < MenuButtons.Length; i++) {
            if (i == index) {
                MenuButtons[i].sprite = SelectedMenuButtonSprite;
                ToolbarButtons[i].SetActive(true);
            } else {
                MenuButtons[i].sprite = UnselectedMenuButtonSprite;
                ToolbarButtons[i].SetActive(false);
            }    
        }
    }

    // File buttons

    public void NewFileButton() {
        main.NewMap();
    }

    public void OpenFileButton (bool ui) {
        if (ui) {
            if (Keyboard.current.leftCtrlKey.isPressed) {
                FileInputField(0);
            } else {
                main.OpenMap();
            }
            
        } else {
            main.OpenMap();
        }
    }

    public void SaveFileButton(bool ui) {
        if (ui) {
            if (Keyboard.current.leftCtrlKey.isPressed) {
                FileInputField(1);
            } else {
                main.SaveMap();
            }
            
        } else {
            main.SaveMap();
        }
    }

    public void FileInputField (int type) {
        FileField.gameObject.SetActive(true);
        FileFieldType = type;
    }

    public void FileInputFieldEnter () {
        if (FileFieldType == 0) {
            main.OpenMap(FileField.text);
        } else if (FileFieldType == 1) {
            main.SaveMap(FileField.text);
        }
        FileField.gameObject.SetActive(false);
    }

    // Tools buttons

    public void ScreenshotButton () {
        StartCoroutine(Screenshot());
    }

    public IEnumerator Screenshot () {
        // figure out a good screenshot name
        DateTime date = DateTime.Now;
        string fileName = date.ToString("MMM d yyyy H mm");
        string extension = ".png";
        int screenshotNumber = 0;
        for (int i = 0; i < int.MaxValue; i++) {
            if (System.IO.File.Exists(fileName + extension)) {
                screenshotNumber++;
                extension = $" {screenshotNumber}.png";
            } else {
                break;
            }
        }
        // actually take screenshot now
        yield return null;
        MainCanvas.enabled = false; // disable canvas for screenshot
        ppl.enabled = false; // disable postprocesslayer for screenshot multiplier
        ppv.enabled = false; // disable postprocessvolume for screenshot multiplier
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot(fileName+extension, SettingsManager.Settings.ScreenshotSizeMultiplier);
        yield return null;
        MainCanvas.enabled = true; // reenable now that we are done
        ppl.enabled = true; // see above
        ppv.enabled = true; // see above
    }

    // View Buttons

    public void RotateSunButton () {
        MapBuilder.instance.rotateSun = !MapBuilder.instance.rotateSun;
    }

    public void ToggleShadowsButton () {
        if (MapBuilder.instance.sun.shadows == LightShadows.None) {
            MapBuilder.instance.sun.shadows = LightShadows.Hard;
        } else {
            MapBuilder.instance.sun.shadows = LightShadows.None;
        }
    }

    public void ToggleAmbientLightButton () {
        ColorGrading cg = MapBuilder.instance.postProcessProfile.GetSetting<ColorGrading>();
        cg.enabled.value = !cg.enabled.value; // toggle
    }

    public void ToggleSideBarButton () {
        Sidebar.SetActive(!Sidebar.activeSelf);
        if (Sidebar.activeSelf) {
            AxisGizmo.anchoredPosition = new Vector2(-288f, AxisGizmo.anchoredPosition.y);
        } else {
            AxisGizmo.anchoredPosition = new Vector2(0f, AxisGizmo.anchoredPosition.y);
        }
    }

    // 3D

    public void TrySelectBrick () {
        RaycastHit hit;
        Ray ray = MapBuilder.instance.mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out hit, SelectionDistance, SelectionLayerMask)) {
            // hit a brick
            BrickGO bg = hit.collider.GetComponent<BrickGO>();
            SelectHierarchyElement(HierarchyElements[bg.brick.ID], false);
        }
    }

    // Hierarchy

    public void PopulateHierarchy () {
        // world element
        AddHierarchyElement(null);

        // rest of the elements
        for (int i = 0; i < main.LoadedMap.MapElements.Count; i++) {
            if (main.LoadedMap.MapElements.TryGetValue(i, out object obj)) {
                AddHierarchyElement(obj);
            }
        }
    }

    public void AddHierarchyElement (object Object) {
        if (Object is Brick) {
            // brick element
            Brick b = Object as Brick;
            if (HierarchyElements.ContainsKey(b.ID)) return;

            GameObject element = Instantiate(HierarchyElementTemplate);
            HierarchyElement he = element.GetComponent<HierarchyElement>();

            he.Set(Map.ElementType.Brick, HierarchyIcons[1], b.Name, element, b);

            // register click event
            he.SelectionButton.onClick.AddListener(delegate { SelectHierarchyElement(he, true); });

            if (b.Parent != null) {
                HierarchyElements[b.Parent.ID].Add(he);
                element.transform.SetParent(HierarchyElements[b.Parent.ID].transform, false);
            } else {
                element.transform.SetParent(Hierarchy, false);
            }

            HierarchyElements.Add(b.ID, he);
        } else if (Object is BrickGroup) {
            // group element
            BrickGroup g = Object as BrickGroup;
            if (HierarchyElements.ContainsKey(g.ID)) return;

            GameObject element = Instantiate(GroupHierarchyElementTemplate);
            HierarchyElement he = element.GetComponent<HierarchyElement>();

            he.Set(Map.ElementType.Group, HierarchyIcons[2], g.Name, element, g);

            // register click event
            he.SelectionButton.onClick.AddListener(delegate { SelectHierarchyElement(he, true); });

            if (g.Parent != null) {
                HierarchyElements[g.Parent.ID].Add(he);
                element.transform.SetParent(HierarchyElements[g.Parent.ID].transform, false);
            } else {
                element.transform.SetParent(Hierarchy, false);
            }

            HierarchyElements.Add(g.ID, he);
        } else if (Object == null) {
            // probably world element
            if (HierarchyElements.ContainsKey(-1)) return;

            GameObject element = Instantiate(HierarchyElementTemplate);
            element.transform.SetParent(Hierarchy, false);
            HierarchyElement he = element.GetComponent<HierarchyElement>();

            // register click event
            he.SelectionButton.onClick.AddListener(delegate { SelectHierarchyElement(he, true); });

            he.Set(Map.ElementType.World, HierarchyIcons[0], "Environment", element, null);
            HierarchyElements.Add(-1, he);
        }
    }

    public void RemoveHierarchyElement (HierarchyElement element, bool removeFromSelected = true) {
        DeselectHierarchyElement(element, removeFromSelected, false);
        int id = -1;
        if (element.Type == Map.ElementType.Brick) {
            Brick b = element.AssociatedObject as Brick;
            id = b.ID;
        } else if (element.Type == Map.ElementType.Group) {
            BrickGroup g = element.AssociatedObject as BrickGroup;
            id = g.ID;
        }
        if (id != -1) {
            HierarchyElements.Remove(id); // remove element from element dictionary
            Destroy(element.gameObject); // delete element gameobject
        }
    }

    public void RemoveAllHierarchyElements () {
        ClearGizmo();
        SelectAllElements();
        DeleteSelection();

        // remove world element
        RemoveHierarchyElement(HierarchyElements[-1], true);
    }

    public void ParentHierarchyElement (HierarchyElement child, HierarchyElement parent) {
        if (parent == null) { // visually unparent
            if (child.Type == Map.ElementType.Brick) {
                Brick b = child.AssociatedObject as Brick;
                if (b.Parent != null) {
                    HierarchyElements[b.Parent.ID].Remove(b.ID); // remove brick id from parent element's id list
                }
                child.transform.SetParent(Hierarchy, false); // move element to hierarchy
            } else if (child.Type == Map.ElementType.Group) {
                BrickGroup g = child.AssociatedObject as BrickGroup;
                if (g.Parent != null) {
                    HierarchyElements[g.Parent.ID].Remove(g.ID); // remove group id from parent element's id list
                }
                child.transform.SetParent(Hierarchy, false); // move element to hierarchy
            }
        } else if (parent.Type == Map.ElementType.Group) { // visually parent to group
            if (child.Type == Map.ElementType.Brick) {
                Brick b = child.AssociatedObject as Brick;
                if (b.Parent != null) {
                    HierarchyElements[b.Parent.ID].Remove(b.ID); // remove brick id from parent element's id list
                }
                parent.Add(child);
                child.transform.SetParent(parent.transform, false); // move element to group in hierarchy
            } else if (child.Type == Map.ElementType.Group) {
                BrickGroup g = child.AssociatedObject as BrickGroup;
                if (g.Parent != null) {
                    HierarchyElements[g.Parent.ID].Remove(g.ID); // remove group id from parent element's id list
                }
                parent.Add(child);
                child.transform.SetParent(parent.transform, false); // move element to group in hierarchy

            }
        }
    }

    public void UpdateHierarchyElement (HierarchyElement element) {
        if (element.Type == Map.ElementType.Brick) {
            element.Label.text = (element.AssociatedObject as Brick).Name;
        } else if (element.Type == Map.ElementType.Group) {
            element.Label.text = (element.AssociatedObject as BrickGroup).Name;
        }
    }

    public void SelectHierarchyElement(HierarchyElement element, bool selectingFromHierarchy, bool overrideSelectType = false, bool addToSelection = false, bool deselectIfSelected = true, bool updateInspector = true) {
        // determine if we are selecting just this or adding to current selection
        bool add = overrideSelectType ? addToSelection : Keyboard.current.leftCtrlKey.isPressed;
        bool selectInBetween = selectingFromHierarchy ? false : Keyboard.current.leftShiftKey.isPressed && add;

        if (!add) {
            DeselectAllElements();
        }

        if (SelectedElements.Contains(element)) {
            // element is already selected
            if (add && deselectIfSelected) {
                // deselect if holding shift
                DeselectHierarchyElement(element);
                return;
            }
        } else {
            // element is not already selected

            if (WorldIsSelected) {
                // world is already selected, cant select other bricks
                return;
            }

            if (element.Type == Map.ElementType.World) {
                // attempting to select world
                if (SelectedElements.Count > 0) {
                    // there are other bricks in the hierarchy, dont add
                    return;
                }
                WorldIsSelected = true;
                ClearGizmo();
            }

            SelectedElements.Add(element);

            if(element.Type == Map.ElementType.Brick) {
                Brick b = element.AssociatedObject as Brick;
                gizmo.AddTarget(b.gameObject.transform);
                b.Selected = true;
                if (selectInBetween) SelectAllBetween();
            } else if (element.Type == Map.ElementType.Group) {
                // select children of group too
                BrickGroup g = element.AssociatedObject as BrickGroup;
                for (int i = 0; i < g.Children.Count; i++) {
                    if (HierarchyElements.ContainsKey(g.Children[i])) {
                        SelectHierarchyElement(HierarchyElements[g.Children[i]], true, true, true, false);
                    }
                }
            }
        }

        // Visually select element
        element.SelectionImage.color = SelectedHierarchyElementColor;
        element.SelectionImage.sprite = SelectedHierarchyElementSprite;
        if (element.Type == Map.ElementType.Brick) {
            Brick b = element.AssociatedObject as Brick;
            b.brickGO.SetOutline(true); // outline gameobject
            b.gameObject.layer = 2; // change to ignore raycast layer so we dont accidentally click it while trying to use the handles
        }

        if (updateInspector)
            UpdateInspector();
    }

    public void DeselectHierarchyElement (HierarchyElement element, bool removeFromList = true, bool updateInspector = true) {
        if (removeFromList) SelectedElements.Remove(element);
        element.SelectionImage.color = UnselectedHierarchyElementColor;
        element.SelectionImage.sprite = null;

        if (element.Type == Map.ElementType.Brick) {
            Brick b = element.AssociatedObject as Brick;
            b.Selected = false;
            b.brickGO.SetOutline(false); // outlinen't gameobject
            gizmo.RemoveTarget(b.gameObject.transform);
            b.gameObject.layer = 9; // change to brick layer
        }
        if (element.Type == Map.ElementType.World) WorldIsSelected = false;
        if (updateInspector) UpdateInspector();
    }

    public void SelectAllElements () {
        foreach (KeyValuePair<int, HierarchyElement> kvp in HierarchyElements) {
            if (kvp.Value.Type != Map.ElementType.World)
                SelectHierarchyElement(kvp.Value, true, true, true, false, false);
        }
        UpdateInspector();
    }

    public void DeselectAllElements () {
        if (SelectedElements.Count == 0) return;
        for (int i = 0; i < SelectedElements.Count; i++) {
            DeselectHierarchyElement(SelectedElements[i], false, false);
        }
        SelectedElements.Clear();
        UpdateInspector();
        HideColorPicker();
        ClearGizmo();
    }

    public void SelectAllBetween() {
        if (SelectedElements.Count <= 1) return; // we must have at least 2 elements selected

        // Firstly, we get the 2 corners of the selection
        int selectedBricks = 0; // amount of bricks that are selected
        Vector3 min = Vector3.zero, max = Vector3.zero; // temporary variables
        for (int i = 0; i < SelectedElements.Count; i++) {
            if (SelectedElements[i].Type == Map.ElementType.Brick) {
                Brick b = SelectedElements[i].AssociatedObject as Brick;
                if (selectedBricks == 0) {
                    // initialize min and max vectors
                    min = b.gameObject.transform.position;
                    max = b.gameObject.transform.position;
                }
                selectedBricks++;

                min = Vector3.Min(min, b.gameObject.transform.position - b.Scale.SwapYZ() / 2);
                max = Vector3.Max(max, b.gameObject.transform.position + b.Scale.SwapYZ() / 2);
            }
        }

        // ensure there are 2 or more bricks selected
        if (selectedBricks > 1) {

            Vector3 center = (min + max) / 2; // center of the 2 corners
            Vector3 extents = (max - min).Abs() - new Vector3(0.001f, 0.001f, 0.001f); // total dimensions of the selection, with a tiny amount shaved off to prevent bricks that are *just* outside the box from being detected

            // OverlapBox for speed
            Collider[] hitColliders = Physics.OverlapBox(center, extents / 2);
            for (int i = 0; i < hitColliders.Length; i++) {
                BrickGO bg = hitColliders[i].GetComponent<BrickGO>();
                if (bg != null) {
                    SelectHierarchyElement(HierarchyElements[bg.brick.ID], true, true, true, false, false);
                }
            }
            UpdateInspector();
        }
    }

    public void ShowGroupMenu () {
        if (main.LoadedMap.Groups.Count > 0) {
            groupMenuGroups = main.LoadedMap.Groups.ToArray();
            groupMenu.GroupDropdown.ClearOptions();
            for (int i = 0; i < groupMenuGroups.Length; i++) {
                groupMenu.GroupDropdown.options.Add(new TMP_Dropdown.OptionData(groupMenuGroups[i].Name));
            }
            groupMenu.GroupDropdown.RefreshShownValue();
        } else {
            groupMenu.DisableExistingGroup();
        }
        groupMenu.Show(true);
        SetInputEnabled(false);
    }

    public void GroupApply () {
        BrickGroup targetGroup;
        // get target group
        if (groupMenu.AddToNewGroup()) {
            targetGroup = NewGroup(groupMenu.GroupInputField.text, false);
        } else {
            int index = groupMenu.GroupDropdown.value;
            targetGroup = groupMenuGroups[index];
        }

        // add selection to group
        for (int i = 0; i < SelectedElements.Count; i++) {
            if (SelectedElements[i].Type == Map.ElementType.Brick) {
                Brick b = SelectedElements[i].AssociatedObject as Brick;
                if (b.Parent == null) { // make sure selected item doesnt already have a parent
                    targetGroup.Children.Add(b.ID);
                    b.Parent = targetGroup;
                    ParentHierarchyElement(SelectedElements[i], HierarchyElements[targetGroup.ID]);
                }
            } else if (SelectedElements[i].Type == Map.ElementType.Group) {
                BrickGroup g = SelectedElements[i].AssociatedObject as BrickGroup;
                if (g.Parent == null) { // make sure selected item doesnt already have a parent
                    targetGroup.Children.Add(g.ID);
                    g.Parent = targetGroup;
                    ParentHierarchyElement(SelectedElements[i], HierarchyElements[targetGroup.ID]);
                }
            }
        }

        groupMenu.Show(false); // hide group menu
    }

    public void ShowAvatarMenu () {
        avatarImporterGO.SetActive(true);
        SetInputEnabled(false);
    }

    public void ImportAvatar () {
        string id = avatarIDField.text;
        if (!string.IsNullOrEmpty(id)) {
            AvatarRoot ar = avatarImporter.GetAvatarFromID(id);
            if (ar != null)
                avatarImporter.BuildAvatar(ar, Vector3.zero, avatarToolToggle.isOn);
            HideAvatarMenu();
        }
    }

    public void HideAvatarMenu () {
        avatarIDField.SetTextWithoutNotify("");
        avatarImporterGO.SetActive(false);
        SetInputEnabled(true);
    }

    public void SetInputEnabled (bool enabled) {
        InputBlocker.SetActive(!enabled);
        BBInputManager.SetControls(enabled, true);
    }

    // Inspector

    // update inspector values to reflect currently selected brick[s]
    public void UpdateInspector () {
        if (SelectedElements.Count == 0) {
            // nothing is selected
            ShowInspectorElements(-1); // any number that isnt 0,1,or 2 will just hide everything

            return;
        }

        if (WorldIsSelected) {
            // show properties
            ShowInspectorElements(0);

            // update the elements
            WorldInspectorElements[0].SetColor(main.LoadedMap.AmbientColor, true);
            WorldInspectorElements[1].SetColor(main.LoadedMap.BaseplateColor, true);
            WorldInspectorElements[2].SetColor(main.LoadedMap.SkyColor, true);
            WorldInspectorElements[3].SetInt(main.LoadedMap.BaseplateSize);
            WorldInspectorElements[4].SetInt(main.LoadedMap.SunIntensity);

            return;
        }

        // first we will determine whether or not the selected elements are all the same type of element
        bool mismatchedTypes = false; // whether different types of elements are selected
        Map.ElementType lastType = SelectedElements[0].Type; // element type of the last iterated element
        for (int i = 0; i < SelectedElements.Count; i++) {
            if (SelectedElements[i].Type != lastType) {
                mismatchedTypes = true;
                break;
            }
            lastType = SelectedElements[i].Type;
        }

        if (mismatchedTypes) {
            // mismatched element types
            ShowInspectorElements(-1);
            MismatchedElementsLabel.SetActive(true);
        } else {
            // all the elements are the same type
            if (lastType == Map.ElementType.Brick) {
                // show brick properties
                ShowInspectorElements(1);

                // update the elements
                Brick b = SelectedElements[0].AssociatedObject as Brick;
                BrickInspectorElements[0].SetString(b.Name);
                BrickInspectorElements[1].SetVector3(b.Position);
                BrickInspectorElements[2].SetVector3(b.Scale);
                BrickInspectorElements[3].SetInt(b.Rotation);
                BrickInspectorElements[4].SetColor(b.BrickColor, true);
                colorPicker.SetColor(b.BrickColor, false); // do not invoke color changed event
                BrickInspectorElements[5].SetInt(Mathf.RoundToInt(b.Transparency * 255));
                BrickInspectorElements[6].SetDropdown((int)b.Shape);
                BrickInspectorElements[7].SetBool(b.CollisionEnabled);
                BrickInspectorElements[8].SetString(b.Model);

                // set color picker target
                colorPickerTarget = ColorPickerTarget.Brick;
            } else if (lastType == Map.ElementType.Group) {
                // show group properties
                ShowInspectorElements(2);

                // update the elements
                BrickGroup g = SelectedElements[0].AssociatedObject as BrickGroup;
                GroupInspectorElements[0].SetString(g.Name);
            }
        }
    }

    // update currently selected brick[s] properties to reflect inspector
    public void UpdateSelection (int inspectorElement) {
        if (SelectedElements.Count == 0) {
            return; // no elements selected, return
        }

        ElementsChanged ec = new ElementsChanged();
        ec.type = EditorAction.ActionType.ElementsChanged;

        List<BrickData> originalBricks = new List<BrickData>();
        List<GroupData> originalGroups = new List<GroupData>();
        List<BrickData> modifiedBricks = new List<BrickData>();
        List<GroupData> modifiedGroups = new List<GroupData>();

        for (int i = 0; i < SelectedElements.Count; i++) {
            if (SelectedElements[i].Type == Map.ElementType.Brick) {
                Brick b = SelectedElements[i].AssociatedObject as Brick;
                originalBricks.Add(new BrickData(b));

                switch (inspectorElement) {
                    case 0:
                        string attemptedName = BrickInspectorElements[0].GetString();
                        if (string.IsNullOrWhiteSpace(attemptedName)) {
                            attemptedName = "New Brick";
                            BrickInspectorElements[0].SetString(attemptedName);
                        }
                        b.Name = attemptedName;
                        UpdateHierarchyElement(SelectedElements[i]);
                        break;
                    case 1:
                        b.Position = BrickInspectorElements[1].GetVector3();
                        break;
                    case 2:
                        Vector3 attemptedScale = BrickInspectorElements[2].GetVector3();
                        b.Scale = attemptedScale.Clamp(Vector3.zero, attemptedScale); // disallow negative scale values
                        b.ClampSize();
                        UpdateInspector(); // update inspector after just incase
                        break;
                    case 3:
                        b.Rotation = BrickInspectorElements[3].GetInt().Mod(360); // keep rotation between 0-359
                        break;
                    case 4:
                        // color is handled by the colorpicker, so this goes ignored
                        break;
                    case 5:
                        int attemptedTransparency = BrickInspectorElements[5].GetInt();
                        b.Transparency = Mathf.Clamp(attemptedTransparency, 0, 255) / 255f; // keep transparency between 0 and 1
                        break;
                    case 6:
                        b.Shape = (Brick.ShapeType)BrickInspectorElements[6].GetDropdown();
                        break;
                    case 7:
                        b.CollisionEnabled = BrickInspectorElements[7].GetBool();
                        break;
                    case 8:
                        string attmeptedModel = BrickInspectorElements[8].GetString();
                        if (CustomModelHelper.IsValidID(attmeptedModel)) { // check if valid model
                            b.Model = BrickInspectorElements[8].GetString();
                            b.UpdateModel();
                        } else {
                            BrickInspectorElements[8].SetString(b.Model); // reset inputfield to original model
                        }
                        break;
                }

                modifiedBricks.Add(new BrickData(b));

                if (inspectorElement == 6) {
                    MapBuilder.instance.UpdateBrickShape(b); // update brick shape
                } else {
                    MapBuilder.instance.UpdateBrick(b); // update brick GO
                }
                

            } else if (SelectedElements[i].Type == Map.ElementType.Group) {
                BrickGroup g = SelectedElements[i].AssociatedObject as BrickGroup;
                originalGroups.Add(new GroupData(g));

                switch (inspectorElement) {
                    case 0:
                        g.Name = GroupInspectorElements[0].GetString();
                        UpdateHierarchyElement(SelectedElements[i]);
                        break;
                }

                modifiedGroups.Add(new GroupData(g));

            } else if (SelectedElements[i].Type == Map.ElementType.World) {
                // no switch statement because there is only 1 world element so you dont need to handle changing individual properties or whatever
                main.LoadedMap.BaseplateSize = WorldInspectorElements[3].GetInt();
                main.LoadedMap.SunIntensity = WorldInspectorElements[4].GetInt();
                MapBuilder.instance.UpdateEnvironment(main.LoadedMap);
            }
        }

        ec.oldBricks = originalBricks.ToArray();
        ec.oldGroups = originalGroups.ToArray();
        ec.newBricks = modifiedBricks.ToArray();
        ec.newGroups = modifiedGroups.ToArray();

        EditorHistory.AddToHistory(ec);
    }

    public void UpdateSelectedElements () {
        for (int i = 0; i < SelectedElements.Count; i++) {
            if (SelectedElements[i].Type == Map.ElementType.Brick) {
                MapBuilder.instance.UpdateBrick(SelectedElements[i].AssociatedObject as Brick);
            }
        }
    }

    public void ShowInspectorElements (int index) {
        for (int i = 0; i < WorldInspectorElements.Length; i++) {
            WorldInspectorElements[i].gameObject.SetActive(index == 0);
        }
        for (int i = 0; i < BrickInspectorElements.Length; i++) {
            BrickInspectorElements[i].gameObject.SetActive(index == 1);
        }
        for (int i = 0; i < GroupInspectorElements.Length; i++) {
            GroupInspectorElements[i].gameObject.SetActive(index == 2);
        }
        MismatchedElementsLabel.SetActive(false);
    }


    public void ShowColorPicker (int target) {
        colorPickerTarget = (ColorPickerTarget)target;

        if (colorPickerTarget == ColorPickerTarget.Ambient) {
            colorPicker.SetColor(main.LoadedMap.AmbientColor, false); // do not invoke color changed event
        } else if (colorPickerTarget == ColorPickerTarget.Baseplate) {
            colorPicker.SetColor(main.LoadedMap.BaseplateColor, false); // do not invoke color changed event
        } else if (colorPickerTarget == ColorPickerTarget.Sky) {
            colorPicker.SetColor(main.LoadedMap.SkyColor, false); // do not invoke color changed event
        }

            colorPickerGO.SetActive(true);
    }

    public void HideColorPicker () {
        colorPickerTarget = ColorPickerTarget.None;
        colorPickerGO.SetActive(false);
    }

    public void ColorChanged (Color color) {
        if (colorPickerTarget == ColorPickerTarget.Brick) {
            if (SelectedElements.Count > 0 && SelectedElements[0].Type == Map.ElementType.Brick) { // if bricks are selected
                for (int i = 0; i < SelectedElements.Count; i++) {
                    if (SelectedElements[i].Type == Map.ElementType.Brick) {
                        (SelectedElements[i].AssociatedObject as Brick).BrickColor = color;
                        MapBuilder.instance.UpdateBrickColor(SelectedElements[i].AssociatedObject as Brick);
                    }
                }
                BrickInspectorElements[4].SetColor(color, true);
                //UpdateSelectedElements();
            }
        } else if (colorPickerTarget == ColorPickerTarget.Ambient) {
            main.LoadedMap.AmbientColor = color;
            WorldInspectorElements[0].SetColor(color, true);
            MapBuilder.instance.UpdateEnvironment(main.LoadedMap);
        } else if (colorPickerTarget == ColorPickerTarget.Baseplate) {
            main.LoadedMap.BaseplateColor = color;
            WorldInspectorElements[1].SetColor(color, true);
            MapBuilder.instance.UpdateEnvironment(main.LoadedMap);
        } else if (colorPickerTarget == ColorPickerTarget.Sky) {
            main.LoadedMap.SkyColor = color;
            WorldInspectorElements[2].SetColor(color, true);
            MapBuilder.instance.UpdateEnvironment(main.LoadedMap);
        } else if (colorPickerTarget == ColorPickerTarget.Team) {
            //TODO
        } else if (colorPickerTarget == ColorPickerTarget.Paintbrush) {
            //TODO
        }
    }

    public enum ColorPickerTarget {
        None,
        Brick,
        Ambient,
        Baseplate,
        Sky,
        Team,
        Paintbrush
    }

    // Editing

    // create new brick (ui button)
    public void NewBrickButton () {
        NewBrick(); // only voids can be called from a ui button so this just calls newbrick and ignores the return
    }

    // create new brick
    public Brick NewBrick () {
        // calculate target position
        Vector3 brickPos = MapBuilder.instance.mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f)).Round();
        // with raycasting
        RaycastHit hit;
        if (Physics.Raycast(MapBuilder.instance.mainCam.transform.position, MapBuilder.instance.mainCam.transform.forward, out hit, SelectionDistance, BrickRaycastMask)) {
            brickPos = hit.point.Round();
        }
        brickPos.x += 1; // to prevent bricks from like being in walls ya know
        brickPos.z -= 2; // to prevent bricks from like being in walls ya know
        brickPos.x *= -1; // flip x axis;

        // create brick object with default values
        Brick b = new Brick();
        b.Name = "New Brick";
        b.Position = brickPos.SwapYZ();
        b.Scale = new Vector3(2, 4, 1);
        b.BrickColor = Color.gray;
        b.Transparency = 1f;
        int id = ++main.LoadedMap.lastID;
        b.ID = id;
        MapBuilder.instance.CreateBrickGameObject(b);

        // add brick to map
        main.LoadedMap.MapElements.Add(id, b);
        main.LoadedMap.Bricks.Add(b);

        // add brick to hierarchy
        AddHierarchyElement(b);
        SelectHierarchyElement(HierarchyElements[id], false, true, false, false, true);

        // add brick to history
        ElementsAdded ea = new ElementsAdded();
        ea.type = EditorAction.ActionType.ElementsAdded;
        ea.bricksAdded = new BrickData[1];
        ea.bricksAdded[0] = new BrickData(b);
        EditorHistory.AddToHistory(ea);

        return b;
    }

    public void ImportBrick (Brick b, bool addToSelection = false) {
        int id = ++main.LoadedMap.lastID;
        b.ID = id;
        MapBuilder.instance.CreateBrickGameObject(b);

        // add brick to map
        main.LoadedMap.MapElements.Add(id, b);
        main.LoadedMap.Bricks.Add(b);

        // add brick to hierarchy
        AddHierarchyElement(b);
        if (addToSelection) {
            SelectHierarchyElement(HierarchyElements[id], true, true, true, false, false);
        }
        //SelectHierarchyElement(HierarchyElements[id], false, true, false, false, true);
    }

    public void UpdateElement (int ID) {
        HierarchyElement he = HierarchyElements[ID];
        UpdateHierarchyElement(he);
        if (he.Type == Map.ElementType.Brick) {
            MapBuilder.instance.UpdateBrick(he.AssociatedObject as Brick);
        }
    }

    // create new group
    public BrickGroup NewGroup (string name = "New Group", bool select = true) {
        //main.SaveMapState();
        BrickGroup g = new BrickGroup();
        g.Name = name;
        int id = ++main.LoadedMap.lastID;
        g.ID = id;

        // add group to map
        main.LoadedMap.MapElements.Add(id, g);
        main.LoadedMap.Groups.Add(g);

        // add group to hierarchy
        AddHierarchyElement(g);
        if(select)
            SelectHierarchyElement(HierarchyElements[id], false, true, false, false, true);

        // add group to history
        ElementsAdded ea = new ElementsAdded();
        ea.type = EditorAction.ActionType.ElementsAdded;
        ea.groupsAdded = new GroupData[1];
        ea.groupsAdded[0] = new GroupData { Name=g.Name, ID=g.ID };
        EditorHistory.AddToHistory(ea);

        return g;
    }

    public void ImportGroup (BrickGroup g) {
        int id = ++main.LoadedMap.lastID;
        g.ID = id;

        // add group to map
        main.LoadedMap.MapElements.Add(id, g);
        main.LoadedMap.Groups.Add(g);

        // add group to hierarchy
        AddHierarchyElement(g);
    }

    // delete selected selection
    public void DeleteSelection () {
        ClearGizmo();
        ElementsRemoved er = new ElementsRemoved();
        er.type = EditorAction.ActionType.ElementsRemoved;
        List<BrickData> removedBricks = new List<BrickData>();
        List<GroupData> removedGroups = new List<GroupData>();
        for (int i = 0; i < SelectedElements.Count; i++) {
            if (SelectedElements[i].Type == Map.ElementType.Brick) {
                Brick b = SelectedElements[i].AssociatedObject as Brick;
                removedBricks.Add(new BrickData(b));
                deleteBrick(b);
                RemoveHierarchyElement(SelectedElements[i], false);
                main.LoadedMap.Bricks.Remove(b);
                main.LoadedMap.MapElements.Remove(b.ID);
            } else if (SelectedElements[i].Type == Map.ElementType.Group) {
                BrickGroup g = SelectedElements[i].AssociatedObject as BrickGroup;
                removedGroups.Add(new GroupData(g));
                deleteBrickBroup(g);
                RemoveHierarchyElement(SelectedElements[i], false);
                main.LoadedMap.Groups.Remove(g);
                main.LoadedMap.MapElements.Remove(g.ID);
            }
        }
        SelectedElements.Clear();
        UpdateInspector();
        HideColorPicker();

        er.bricksRemoved = removedBricks.ToArray();
        er.groupsRemoved = removedGroups.ToArray();
        EditorHistory.AddToHistory(er);
    }

    public void DeleteElement (int id) {
        ClearGizmo();
        DeselectAllElements();
        HierarchyElement he = HierarchyElements[id];
        if (he.Type == Map.ElementType.Brick) {
            Brick b = he.AssociatedObject as Brick;
            deleteBrick(b);
            RemoveHierarchyElement(he, true);
            main.LoadedMap.Bricks.Remove(b);
            main.LoadedMap.MapElements.Remove(b.ID);
        } else if (he.Type == Map.ElementType.Group) {
            BrickGroup g = he.AssociatedObject as BrickGroup;
            deleteBrickBroup(g);
            RemoveHierarchyElement(he, true);
            main.LoadedMap.Groups.Remove(g);
            main.LoadedMap.MapElements.Remove(g.ID);
        }
    }

    private void deleteBrick (Brick b) {
        gizmo.RemoveTarget(b.gameObject.transform);
        Destroy(b.gameObject);
        if (main.LoadedMap.lastID == b.ID) main.LoadedMap.lastID--; // decrement last id
    }

    private void deleteBrickBroup (BrickGroup g) {
        // TODO
        // if (main.LoadedMap.lastID == g.ID) main.LoadedMap.lastID--; // decrement last id
    }

    public void CopySelection () {
        List<Brick> bricksToCopy = new List<Brick>();
        for (int i = 0; i < SelectedElements.Count; i++) {
            if (SelectedElements[i].Type == Map.ElementType.Brick) {
                bricksToCopy.Add(SelectedElements[i].AssociatedObject as Brick);
            }
        }
        MapClipboard.instance.Copy(bricksToCopy.ToArray());
    }

    public void PasteSelection () {
        DeselectAllElements();
        //main.SaveMapState();
        BrickList pastedBricks = MapClipboard.instance.ParseClipboard();
        if (pastedBricks.bricks != null && pastedBricks.bricks.Length > 0) {
            for (int i = 0; i < pastedBricks.bricks.Length; i++) {
                Brick b = pastedBricks.bricks[i].ToBrick();
                int id = ++main.LoadedMap.lastID;
                b.ID = id;

                MapBuilder.instance.CreateBrickGameObject(b); // create brick GO

                // add brick to map
                main.LoadedMap.MapElements.Add(id, b);
                main.LoadedMap.Bricks.Add(b);

                // add brick to hierarchy
                AddHierarchyElement(b);
                SelectHierarchyElement(HierarchyElements[id], true, true, true, false, false);
            }
            UpdateInspector();
        }
    }

    public void Undo () {
        EditorHistory.Undo();
    }

    public void Redo () {
        EditorHistory.Redo();
    }

    public void ChangeGizmo (int index) {
        gizmo.SetType(index);
    }

    public void ClearGizmo () {
        if (SelectedElements.Count == 0) return;
        gizmo.ClearTargets();
    }

    // Settings

    public void ShowPreferences () {
        Preferences.SetActive(true);
        SetInputEnabled(false);
    }

    public void HidePreferences () {
        Preferences.SetActive(false);
        SetInputEnabled(true);
    }

    public void SavePreferencesButton () {
        // Editor
        SettingsManager.Settings.StudSnap = float.Parse(StudSnapField.text, CultureInfo.InvariantCulture);
        SettingsManager.Settings.RotationSnap = int.Parse(RotationSnapField.text, CultureInfo.InvariantCulture);
        SettingsManager.Settings.CopySuffix = CopySuffixToggle.isOn;
        SettingsManager.Settings.CopyToWorkshopFormat = CopyToWorkshopFormatToggle.isOn;

        // Personalization
        SettingsManager.Settings.CameraSpeed = (int)CameraSpeedSlider.value;
        SettingsManager.Settings.CameraSensitivity = CameraSensitivitySlider.value;
        SettingsManager.Settings.FOV = (int)FOVSlider.value; // casting float to int floors it
        SettingsManager.Settings.ViewDistance = (int)ViewDistanceSlider.value;
        SettingsManager.Settings.Autosave = AutosaveToggle.isOn;
        SettingsManager.Settings.AutosaveRate = (int)AutosaveRateSlider.value;
        main.ResetAutosaveRate();
        SettingsManager.Settings.Framelimiter = FramelimiterToggle.isOn;
        SettingsManager.Settings.Framelimit = int.Parse(FramelimiterField.text, CultureInfo.InvariantCulture);
        SettingsManager.Settings.ScreenshotSizeMultiplier = (int)ScreenshotSizeMultiplierSlider.value;
        main.ApplySettings();

        // Keybinds
        for (int i = 0; i < BBInputManager.RebindableKeys.Count; i++) {
            SettingsManager.Settings.EnabledHotkeys[i] = HotkeyToggles[i].isOn;
            int bindingIndex = 2;
            if (BBInputManager.RebindableKeys[i].name == "Translate" || BBInputManager.RebindableKeys[i].name == "Scale" || BBInputManager.RebindableKeys[i].name == "Rotate") bindingIndex = 0;
            SettingsManager.Settings.HotkeyPaths[i] = BBInputManager.RebindableKeys[i].bindings[bindingIndex].effectivePath;
        }

        SettingsManager.SaveSettings();
    }

    public void LoadPreferences () {

        // editor
        StudSnapField.SetTextWithoutNotify(SettingsManager.Settings.StudSnap.ToString());
        RotationSnapField.SetTextWithoutNotify(SettingsManager.Settings.RotationSnap.ToString());
        CopySuffixToggle.isOn = SettingsManager.Settings.CopySuffix;
        CopyToWorkshopFormatToggle.isOn = SettingsManager.Settings.CopyToWorkshopFormat;

        // personalization
        Camera c = MapBuilder.instance.mainCam;
        EditorCamera ec = c.GetComponent<EditorCamera>();

        ec.movementSpeed = SettingsManager.Settings.CameraSpeed;
        CameraSpeedSlider.SetValueWithoutNotify(SettingsManager.Settings.CameraSpeed);
        CameraSpeedLabel.text = SettingsManager.Settings.CameraSpeed.ToString();

        ec.cameraSpeed = SettingsManager.Settings.CameraSensitivity;
        CameraSensitivitySlider.SetValueWithoutNotify(SettingsManager.Settings.CameraSensitivity);
        CameraSensitivityLabel.text = SettingsManager.Settings.CameraSensitivity.ToString();

        c.fieldOfView = SettingsManager.Settings.FOV;
        FOVSlider.SetValueWithoutNotify(SettingsManager.Settings.FOV);
        FOVLabel.text = SettingsManager.Settings.FOV.ToString();

        c.farClipPlane = SettingsManager.Settings.ViewDistance;
        ViewDistanceSlider.SetValueWithoutNotify(SettingsManager.Settings.ViewDistance);
        ViewDistanceLabel.text = SettingsManager.Settings.ViewDistance.ToString();

        AutosaveToggle.SetIsOnWithoutNotify(SettingsManager.Settings.Autosave);

        AutosaveRateSlider.SetValueWithoutNotify(SettingsManager.Settings.AutosaveRate);
        AutosaveRateLabel.text = SettingsManager.Settings.AutosaveRate.ToString();

        FramelimiterToggle.SetIsOnWithoutNotify(SettingsManager.Settings.Framelimiter);
        //FramelimiterLabel.text = "Framelimiter (" + Screen.currentResolution.refreshRate + " FPS)";
        FramelimiterField.SetTextWithoutNotify(SettingsManager.Settings.Framelimit.ToString());

        ScreenshotSizeMultiplierSlider.SetValueWithoutNotify(SettingsManager.Settings.ScreenshotSizeMultiplier);
        ScreenshotSizeMultiplierLabel.text = SettingsManager.Settings.ScreenshotSizeMultiplier + "x";

        // Hotkeys
        for (int i = 0; i < BBInputManager.RebindableKeys.Count; i++) {
            (bool, string) inputSettings = BBInputManager.GetSettings(i);
            HotkeyToggles[i].isOn = inputSettings.Item1;
            HotkeyLabels[i].SetText(InputControlPath.ToHumanReadableString(inputSettings.Item2, InputControlPath.HumanReadableStringOptions.OmitDevice));
        }

        // advanced
        PlayerPathLabel.text = SettingsManager.Settings.BrickHillPlayerPath;
        ServerPathLabel.text = SettingsManager.Settings.NodeHillServerPath;
    }

    public void CameraSpeedSliderUpdated () {
        MapBuilder.instance.mainCam.GetComponent<EditorCamera>().movementSpeed = CameraSpeedSlider.value;
        CameraSpeedLabel.text = CameraSpeedSlider.value.ToString();
    }

    public void CameraSensitivitySliderUpdated () {
        float sensitivity = CameraSensitivitySlider.value.Round(10);
        CameraSensitivitySlider.SetValueWithoutNotify(sensitivity);
        MapBuilder.instance.mainCam.GetComponent<EditorCamera>().cameraSpeed = sensitivity;
        CameraSensitivityLabel.text = sensitivity.ToString();
    }

    public void FOVSliderUpdated () {
        MapBuilder.instance.mainCam.fieldOfView = FOVSlider.value;
        FOVLabel.text = FOVSlider.value.ToString();
    }

    public void ViewDistanceSliderUpdated () {
        MapBuilder.instance.mainCam.farClipPlane = ViewDistanceSlider.value;
        ViewDistanceLabel.text = ViewDistanceSlider.value.ToString();
    }

    public void AutosaveSliderUpdated () {
        AutosaveRateLabel.text = AutosaveRateSlider.value.ToString();
    }

    public void ScreenshotSizeMultiplierSliderUpdated () {
        ScreenshotSizeMultiplierLabel.text = ScreenshotSizeMultiplierSlider.value.ToString() + "x";
    }

    public void RebindHotkeyButton (int button) {
        HotkeyLabels[button].SetText("Press a Key...");
        BBInputManager.StartRebind(BBInputManager.RebindableKeys[button]);
    }

    public void KeyRebinded (InputAction action) {
        int index = BBInputManager.RebindableKeys.FindIndex(a => a == action);
        int bindingIndex = 2;
        if (action.name == "Translate" || action.name == "Scale" || action.name == "Rotate") bindingIndex = 0;
        Debug.Log(action.bindings[bindingIndex].effectivePath);
        HotkeyLabels[index].SetText(InputControlPath.ToHumanReadableString(action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice));
    }

    public void SetPlayerPath () {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open Player.exe", "", "exe", false);
        if (paths.Length > 0) {
            SettingsManager.Settings.BrickHillPlayerPath = paths[0];
            TestModeManager.PlayerPath = paths[0];
            PlayerPathLabel.text = paths[0];
            SettingsManager.SaveSettings();
        }
    }

    public void SetServerPath () {
        string[] paths = StandaloneFileBrowser.OpenFolderPanel("Open node-hill server folder", "", false);
        if (paths.Length > 0) {
            SettingsManager.Settings.NodeHillServerPath = paths[0];
            TestModeManager.ServerPath = paths[0];
            ServerPathLabel.text = paths[0];
            SettingsManager.SaveSettings();
        }
    }

    /*
    public void RebindShortcut (int actionID) {
        // perhaps this should be defined outside the function?
        InputAction[] actions = new InputAction[] {
            BBInputManager.Controls.EditorKeys.NewBrick,
            BBInputManager.Controls.EditorKeys.DeleteBrick,
            BBInputManager.Controls.EditorKeys.Copy,
            BBInputManager.Controls.EditorKeys.Paste,
            BBInputManager.Controls.EditorKeys.Duplicate,
            BBInputManager.Controls.EditorKeys.SaveFile,
            BBInputManager.Controls.EditorKeys.OpenFile,
            BBInputManager.Controls.EditorKeys.Translate,
            BBInputManager.Controls.EditorKeys.Scale,
            BBInputManager.Controls.EditorKeys.Rotate
        };

        BBInputManager.DisableControls();
        InputAction action = actions[actionID];
        action.ChangeBinding(action.bindings[2]).WithPath("<Keyboard>/m");
        BBInputManager.EnableControls();
        
    }
    */
}
