////////////////////////////////////////////////////////////////////////////////

using UnityEditor;

namespace MG
{
    class MyEditorWindow : EditorWindow
    {
        [MenuItem( "Tools/My Editor Window" )]
        public static void ShowWindow()
        {
            GetWindow( typeof( MyEditorWindow ) );
        }

        void OnGUI()
        {
            // TODO: draw GUI
        }
    }
}

