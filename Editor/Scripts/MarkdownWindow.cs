using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;

namespace MG.MDV
{
    public class MarkdownWindow : EditorWindow
    {
        private TextAsset mMarkdownFile;
        private string mLoadedContent;
        private MarkdownViewer mViewer;
        private Vector2 mScrollPosition;
        private bool mSyncSelection = true;

        private GUISkin mSkinLight;
        private GUISkin mSkinDark;

        [MenuItem("Window/Markdown Viewer Window")]
        public static void ShowWindow()
        {
            var window = GetWindow<MarkdownWindow>("Markdown Viewer");
            window.Show();
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(EntityId instanceID, int line)
        {
            var obj = EditorUtility.EntityIdToObject(instanceID);

            if (obj is TextAsset textAsset)
            {
                var path = AssetDatabase.GetAssetPath(textAsset);
                var ext = Path.GetExtension(path).ToLower();
                if (ext == ".md" || ext == ".markdown")
                {
                    var window = GetWindow<MarkdownWindow>("Markdown Viewer");
                    window.LoadMarkdownFile(textAsset);
                    window.Show();
                    return true;
                }
            }
            return false;
        }

        private void OnEnable()
        {
            LoadSkins();
            EditorApplication.update += UpdateRequests;
            
            if (mMarkdownFile != null)
            {
                ReloadCurrentFile();
            }
            else if (Selection.activeObject is TextAsset textAsset)
            {
                var path = AssetDatabase.GetAssetPath(textAsset);
                var ext = Path.GetExtension(path).ToLower();
                if (ext == ".md" || ext == ".markdown")
                {
                    LoadMarkdownFile(textAsset);
                }
            }
        }

        private void OnDisable()
        {
            EditorApplication.update -= UpdateRequests;
            mViewer = null;
        }

        private void UpdateRequests()
        {
            if (mViewer != null && mViewer.Update())
            {
                Repaint();
            }

            // Auto-reload if file contents changed on disk
            if (mMarkdownFile != null && mMarkdownFile.text != mLoadedContent)
            {
                ReloadCurrentFile();
            }
        }

        private void OnSelectionChange()
        {
            if (mSyncSelection)
            {
                if (Selection.activeObject is TextAsset textAsset)
                {
                    var path = AssetDatabase.GetAssetPath(textAsset);
                    var ext = Path.GetExtension(path).ToLower();
                    if (ext == ".md" || ext == ".markdown")
                    {
                        LoadMarkdownFile(textAsset);
                        Repaint();
                    }
                }
            }
        }

        public void LoadMarkdownFile(TextAsset file)
        {
            if (mMarkdownFile == file && mViewer != null)
            {
                return;
            }

            mMarkdownFile = file;
            ReloadCurrentFile();
        }

        private void ReloadCurrentFile()
        {
            LoadSkins();

            if (mMarkdownFile != null)
            {
                var path = AssetDatabase.GetAssetPath(mMarkdownFile);
                mLoadedContent = mMarkdownFile.text;
                var skin = Preferences.DarkSkin ? mSkinDark : mSkinLight;
                mViewer = new MarkdownViewer(skin, path, mLoadedContent, () => position.width);
            }
            else
            {
                mViewer = null;
                mLoadedContent = null;
            }
        }

        private void LoadSkins()
        {
            if (mSkinLight == null)
            {
                var path = AssetDatabase.GUIDToAssetPath("81f272e4a921a5448a30c0e40b4ea003");
                if (!string.IsNullOrEmpty(path))
                {
                    mSkinLight = AssetDatabase.LoadAssetAtPath<GUISkin>(path);
                }
            }
            if (mSkinDark == null)
            {
                var path = AssetDatabase.GUIDToAssetPath("8611e9e5923b6084997203d4997b4976");
                if (!string.IsNullOrEmpty(path))
                {
                    mSkinDark = AssetDatabase.LoadAssetAtPath<GUISkin>(path);
                }
            }
        }

        private void OnGUI()
        {
            DrawToolbar();

            if (mMarkdownFile == null)
            {
                DrawDragAndDropZone();
                return;
            }

            if (mViewer == null)
            {
                ReloadCurrentFile();
            }

            if (mViewer == null)
            {
                EditorGUILayout.HelpBox("Failed to initialize Markdown Viewer.", MessageType.Error);
                return;
            }

            mScrollPosition = EditorGUILayout.BeginScrollView(mScrollPosition);
            mViewer.Draw();
            EditorGUILayout.EndScrollView();
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                var newFile = (TextAsset)EditorGUILayout.ObjectField(mMarkdownFile, typeof(TextAsset), false, GUILayout.Width(250));
                if (newFile != mMarkdownFile)
                {
                    if (newFile == null)
                    {
                        LoadMarkdownFile(null);
                    }
                    else
                    {
                        var path = AssetDatabase.GetAssetPath(newFile);
                        var ext = Path.GetExtension(path).ToLower();
                        if (ext == ".md" || ext == ".markdown")
                        {
                            LoadMarkdownFile(newFile);
                        }
                    }
                }

                GUILayout.Space(5);

                mSyncSelection = GUILayout.Toggle(mSyncSelection, "Sync Selection", EditorStyles.toolbarButton);

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Reload", EditorStyles.toolbarButton))
                {
                    ReloadCurrentFile();
                }

                if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
                {
                    LoadMarkdownFile(null);
                }
            }
        }

        private void DrawDragAndDropZone()
        {
            var rect = GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUI.Box(rect, "Drag and drop a Markdown (.md) file here", new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                normal = { textColor = Color.gray }
            });

            var evt = Event.current;
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!rect.Contains(evt.mousePosition))
                        break;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        foreach (var draggedObject in DragAndDrop.objectReferences)
                        {
                            if (draggedObject is TextAsset textAsset)
                            {
                                var path = AssetDatabase.GetAssetPath(textAsset);
                                var ext = Path.GetExtension(path).ToLower();
                                if (ext == ".md" || ext == ".markdown")
                                {
                                    LoadMarkdownFile(textAsset);
                                    Repaint();
                                    break;
                                }
                            }
                        }
                    }
                    evt.Use();
                    break;
            }
        }
    }
}
