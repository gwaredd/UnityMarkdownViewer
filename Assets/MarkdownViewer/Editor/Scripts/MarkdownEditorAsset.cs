using UnityEditor;
using UnityEngine;

namespace MG.MDV
{
    [CustomEditor( typeof( MarkdownAsset ) )]
    public class MarkdownEditorAsset : Editor
    {
        public GUISkin Skin;

        MarkdownViewer mViewer;

        protected void OnEnable()
        {
            var content = ( target as MarkdownAsset ).Text;
            var path    = AssetDatabase.GetAssetPath( target );

            mViewer = new MarkdownViewer( Skin, path, content );
            EditorApplication.update += UpdateRequests;
        }

        protected void OnDisable()
        {
            EditorApplication.update -= UpdateRequests;
            mViewer = null;
        }

        public override bool UseDefaultMargins()
        {
            return false;
        }

        public override void OnInspectorGUI()
        {
            mViewer.Draw();
        }


        void UpdateRequests()
        {
            if( mViewer.Update() )
            {
                Repaint();
            }
        }
    }
}
