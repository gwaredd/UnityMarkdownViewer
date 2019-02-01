using System;
using UnityEditor;
using UnityEngine;

namespace MG.MDV
{
    [CustomEditor( typeof( TextAsset ) )]
    public class MarkdownEditor : Editor
    {
        public GUISkin Skin;

        MarkdownViewer mViewer;

        protected void OnEnable()
        {
            var content = ( target as TextAsset ).text;
            var path    = AssetDatabase.GetAssetPath( target );

            mViewer = new MarkdownViewer( Skin, path, content );

            if( mViewer.IsMarkdown )
            {
                EditorApplication.update += UpdateRequests;
            }
        }

        protected void OnDisable()
        {
            if( mViewer.IsMarkdown )
            {
                EditorApplication.update -= UpdateRequests;
            }

            mViewer = null;
        }

        void UpdateRequests()
        {
            if( mViewer != null && mViewer.Update() )
            {
                Repaint();
            }
        }


        //------------------------------------------------------------------------------

        public override bool UseDefaultMargins()
        {
            return false;
        }

        public override void OnInspectorGUI()
        {
            if( mViewer.IsMarkdown )
            {
                mViewer.Draw();
            }
            else
            {
                DrawDefaultEditor();
            }
        }


        //------------------------------------------------------------------------------

        private Editor mDefaultEditor;

        void DrawDefaultEditor()
        {
            mDefaultEditor = mDefaultEditor ?? CreateEditor( target, Type.GetType( "UnityEditor.TextAssetInspector, UnityEditor" ) );

            if( mDefaultEditor != null )
            {
                mDefaultEditor.OnInspectorGUI();
            }
        }
    }
}
