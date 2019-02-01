using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MG.MDV
{
    [CustomEditor( typeof( TextAsset ) )]
    public class MarkdownEditor : Editor
    {
        public GUISkin Skin;

        MarkdownViewer mViewer;

        private static List<string> mExtensions = new List<string> { ".md", ".markdown" };

        protected void OnEnable()
        {
            var content = ( target as TextAsset ).text;
            var path    = AssetDatabase.GetAssetPath( target );

            var ext = Path.GetExtension( path ).ToLower();

            if( mExtensions.Contains( ext ) )
            {
                mViewer = new MarkdownViewer( Skin, path, content );
                EditorApplication.update += UpdateRequests;
            }
        }

        protected void OnDisable()
        {
            if( mViewer != null )
            {
                EditorApplication.update -= UpdateRequests;
                mViewer = null;
            }
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
            if( mViewer != null )
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
