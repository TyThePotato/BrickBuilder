using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorHistory : MonoBehaviour
{
    private static List<EditorAction> actionHistory = new List<EditorAction>();
    private static int currentHistoryIndex = -1; // -1 means there are no actions in the history

    public static void AddToHistory (EditorAction a) {
        if (currentHistoryIndex < actionHistory.Count-1) {
            // we are not at the end of the action list, so we have to remove all the elements after the current index
            List<EditorAction> sublist = actionHistory.GetRange(0, currentHistoryIndex + 1);
            actionHistory.Clear();
            actionHistory = sublist;
        }
        actionHistory.Add(a);
        currentHistoryIndex++;
        Debug.Log("added to history");
    }

    public static void ClearHistory () {
        actionHistory.Clear();
        currentHistoryIndex = -1;
    }

    public static EditorAction GetPreviousAction () {
        if (currentHistoryIndex == -1) return null; // there are no more actions to get
        EditorAction a = actionHistory[currentHistoryIndex];
        currentHistoryIndex--;
        return a;
    }

    public static EditorAction GetNextAction () {
        if (currentHistoryIndex == actionHistory.Count - 1) return null; // we are currently at the last added action, cant go forward
        currentHistoryIndex++;
        return actionHistory[currentHistoryIndex];
    }

    public static void HandleAction (EditorAction action, bool redo) {
        if (redo) {
            switch (action.type) {
                case EditorAction.ActionType.EnvironmentChanged:
                    // set map environment settings to new settings
                    EnvironmentChanged ec = action as EnvironmentChanged;
                    EditorMain.instance.LoadedMap.AmbientColor = ec.newAmbientColor;
                    EditorMain.instance.LoadedMap.BaseplateColor = ec.newBaseplateColor;
                    EditorMain.instance.LoadedMap.SkyColor = ec.newSkyColor;
                    EditorMain.instance.LoadedMap.BaseplateSize = ec.newBaseplateSize;
                    EditorMain.instance.LoadedMap.SunIntensity = ec.newSunIntensity;
                    MapBuilder.instance.UpdateEnvironment(EditorMain.instance.LoadedMap);
                    break;
                case EditorAction.ActionType.ElementsAdded:
                    // add elements
                    ElementsAdded ea = action as ElementsAdded;
                    if (ea.bricksAdded != null) {
                        for (int i = 0; i < ea.bricksAdded.Length; i++) {
                            EditorUI.instance.ImportBrick(ea.bricksAdded[i].ToBrick(false));
                        }
                    }
                    if (ea.groupsAdded != null) {
                        for (int i = 0; i < ea.groupsAdded.Length; i++) {
                            EditorUI.instance.ImportGroup(ea.groupsAdded[i].ToGroup(false));
                        }
                    }
                    break;
                case EditorAction.ActionType.ElementsChanged:
                    // set element settings to new settings
                    ElementsChanged elc = action as ElementsChanged;
                    for (int i = 0; i < elc.oldBricks.Length; i++) {
                        EditorMain.instance.SetBrickProperties(elc.newBricks[i], elc.oldBricks[i].ID);
                        EditorUI.instance.UpdateElement(elc.oldBricks[i].ID);
                    }
                    EditorUI.instance.UpdateInspector();
                    break;
                case EditorAction.ActionType.ElementsRemoved:
                    // remove elements
                    ElementsRemoved er = action as ElementsRemoved;
                    if (er.bricksRemoved != null) {
                        for (int i = 0; i < er.bricksRemoved.Length; i++) {
                            EditorUI.instance.DeleteElement(er.bricksRemoved[i].ID);
                        }
                    }
                    if (er.groupsRemoved != null) {
                        for (int i = 0; i < er.groupsRemoved.Length; i++) {
                            EditorUI.instance.DeleteElement(er.groupsRemoved[i].ID);
                        }
                    }
                    break;
                case EditorAction.ActionType.GenericMapChange:
                    // load new saved map
                    break;
            }
        } else {
            switch (action.type) {
                case EditorAction.ActionType.EnvironmentChanged:
                    // revert map environment settings to old settings
                    EnvironmentChanged ec = action as EnvironmentChanged;
                    EditorMain.instance.LoadedMap.AmbientColor = ec.oldAmbientColor;
                    EditorMain.instance.LoadedMap.BaseplateColor = ec.oldBaseplateColor;
                    EditorMain.instance.LoadedMap.SkyColor = ec.oldSkyColor;
                    EditorMain.instance.LoadedMap.BaseplateSize = ec.oldBaseplateSize;
                    EditorMain.instance.LoadedMap.SunIntensity = ec.oldSunIntensity;
                    MapBuilder.instance.UpdateEnvironment(EditorMain.instance.LoadedMap);
                    break;
                case EditorAction.ActionType.ElementsAdded:
                    // remove added elements
                    ElementsAdded ea = action as ElementsAdded;
                    if (ea.bricksAdded != null) {
                        for (int i = 0; i < ea.bricksAdded.Length; i++) {
                            EditorUI.instance.DeleteElement(ea.bricksAdded[i].ID);
                        }
                    }
                    if (ea.groupsAdded != null) {
                        for (int i = 0; i < ea.groupsAdded.Length; i++) {
                            EditorUI.instance.DeleteElement(ea.groupsAdded[i].ID);
                        }
                    }
                    break;
                case EditorAction.ActionType.ElementsChanged:
                    // revert element settings to old settings
                    ElementsChanged elc = action as ElementsChanged;
                    for (int i = 0; i < elc.newBricks.Length; i++) {
                        EditorMain.instance.SetBrickProperties(elc.oldBricks[i], elc.newBricks[i].ID);
                        EditorUI.instance.UpdateElement(elc.newBricks[i].ID);
                    }
                    EditorUI.instance.UpdateInspector();
                    break;
                case EditorAction.ActionType.ElementsRemoved:
                    // add removed elements
                    ElementsRemoved er = action as ElementsRemoved;
                    if (er.bricksRemoved != null) {
                        for (int i = 0; i < er.bricksRemoved.Length; i++) {
                            EditorUI.instance.ImportBrick(er.bricksRemoved[i].ToBrick(false));
                        }
                    }
                    if (er.groupsRemoved != null) {
                        for (int i = 0; i < er.groupsRemoved.Length; i++) {
                            EditorUI.instance.ImportGroup(er.groupsRemoved[i].ToGroup(false));
                        }
                    }
                    break;
                case EditorAction.ActionType.GenericMapChange:
                    // load old saved map
                    GenericMapChange gmc = action as GenericMapChange;
                    break;
            }
        }
    }

    public static void Undo () {
        EditorAction ea = GetPreviousAction();
        if (ea != null) {
            HandleAction(ea, false);
        }
    }

    public static void Redo () {
        EditorAction ea = GetNextAction();
        if (ea != null) {
            HandleAction(ea, true);
        }
    }
}

public class EditorAction {

    public ActionType type;

    public enum ActionType {
        EnvironmentChanged,
        ElementsAdded,
        ElementsChanged,
        ElementsRemoved,
        GenericMapChange
    }
}

public class EnvironmentChanged : EditorAction {
    public Color oldAmbientColor;
    public Color oldBaseplateColor;
    public Color oldSkyColor;
    public int oldBaseplateSize;
    public int oldSunIntensity;

    public Color newAmbientColor;
    public Color newBaseplateColor;
    public Color newSkyColor;
    public int newBaseplateSize;
    public int newSunIntensity;

    public void SetOld (Map m) {
        oldAmbientColor = m.AmbientColor;
        oldBaseplateColor = m.BaseplateColor;
        oldSkyColor = m.SkyColor;
        oldBaseplateSize = m.BaseplateSize;
        oldSunIntensity = m.SunIntensity;
    }

    public void SetNew(Map m) {
        newAmbientColor = m.AmbientColor;
        newBaseplateColor = m.BaseplateColor;
        newSkyColor = m.SkyColor;
        newBaseplateSize = m.BaseplateSize;
        newSunIntensity = m.SunIntensity;
    }
}

public class ElementsAdded : EditorAction {
    public BrickData[] bricksAdded;
    public GroupData[] groupsAdded;
}

public class ElementsChanged : EditorAction {
    public BrickData[] oldBricks;
    public GroupData[] oldGroups;

    public BrickData[] newBricks;
    public GroupData[] newGroups;
}

public class ElementsRemoved : EditorAction {
    public BrickData[] bricksRemoved;
    public GroupData[] groupsRemoved;
}

public class GenericMapChange : EditorAction {
    public Map oldMap;
    public Map newMap;
}