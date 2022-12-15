#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class DevTool : EditorWindow
{
    [MenuItem("Tools/Dev Tool")]
    public static void ShowExample()
    {
        DevTool wnd = GetWindow<DevTool>();
        wnd.titleContent = new GUIContent("DevTool");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Utils/Tools/UIToolkit/DevTool/DevTool.uxml");
        VisualElement treeFromUXML = visualTree.Instantiate();
        root.Add(treeFromUXML);

        SetupButtonsHandlers();
    }

    private void SetupButtonsHandlers()
    {
        Button screenshotToolBtn = rootVisualElement.Q<Button>("screenshotBtn");
        Button vfxActivatorBtn = rootVisualElement.Q<Button>("vfxActivatorBtn");
        Button teleportBtn = rootVisualElement.Q<Button>("teleportBtn");
        Button multiPlacingBtn = rootVisualElement.Q<Button>("multiPlacingBtn");

        screenshotToolBtn.RegisterCallback<ClickEvent>((ClickEvent evt) => { ScreenshotTool.ShowEditor();  });
        vfxActivatorBtn.RegisterCallback<ClickEvent>((ClickEvent evt) => { VFXActivatorTool.ShowEditor(); });
        teleportBtn.RegisterCallback<ClickEvent>((ClickEvent evt) => { TeleportTool.ShowEditor(); });
        multiPlacingBtn.RegisterCallback<ClickEvent>((ClickEvent evt) => { MultipleObjectPlacing.ShowEditor(); });
    }
}
#endif