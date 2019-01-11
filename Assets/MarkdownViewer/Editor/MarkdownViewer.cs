////////////////////////////////////////////////////////////////////////////////

using Markdig;
using Markdig.Syntax;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MG.MDV
{
    [CustomEditor( typeof( TextAsset ) )]
    public class MarkdownViewer : Editor
    {
        public GUISkin Skin;
        public Texture Placeholder;


        //------------------------------------------------------------------------------

        private Editor mDefaultEditor;

        private void DrawDefaultEditor()
        {
            if( mDefaultEditor == null )
            {
                mDefaultEditor = CreateEditor( target, Type.GetType( "UnityEditor.TextAssetInspector, UnityEditor" ) );
            }

            if( mDefaultEditor != null )
            {
                GUI.skin = null;
                mDefaultEditor.OnInspectorGUI();
            }
        }

        //------------------------------------------------------------------------------

        public override bool UseDefaultMargins()
        {
            return false;
        }

        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();

            if( Event.current.type == EventType.Repaint )
            {
                mHeaderHeight = GUILayoutUtility.GetLastRect().height;
            }
        }


        //------------------------------------------------------------------------------

        MarkdownDocument mDoc;
        RendererMarkdown mRenderer;
        MarkdownPipeline mPipeline;
        bool             mRaw = false;
        float            mHeaderHeight = 0.0f;

        public override void OnInspectorGUI()
        {
            // has target?

            var asset = target as TextAsset;

            if( asset == null )
            {
                DrawDefaultEditor();
                return;
            }

            // TODO: configurable extensions?
            // file extension is .md

            var path = AssetDatabase.GetAssetPath( target );
            var ext  = Path.GetExtension( path );

            if( ".md".Equals( ext, StringComparison.OrdinalIgnoreCase ) == false )
            {
                DrawDefaultEditor();
                return;
            }

            ParseDocument();
            DrawIMGUI();
        }


        //------------------------------------------------------------------------------

        void ParseDocument()
        {
            if( mDoc != null )
            {
                return;
            }

            // TODO: look at pipeline options ...
            mPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            mRenderer = new RendererMarkdown();
            mPipeline.Setup( mRenderer );

            mDoc = Markdown.Parse( ( target as TextAsset ).text, mPipeline );
        }


        //------------------------------------------------------------------------------

        Vector2 mScrollPos;

        void DrawIMGUI()
        {
            GUI.skin    = Skin;
            GUI.enabled = true;

            GUI.DrawTexture( new Rect( 0.0f, mHeaderHeight, Screen.width, Screen.height ), EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill, false );

            var size    = 20.0f;
            var padding = 8.0f;
            mRaw = GUI.Toggle( new Rect( Screen.width - padding - size, mHeaderHeight + padding, size, size ), mRaw, "" );

            if( mRaw )
            {
                var style   = Skin.GetStyle( "pre" );
                var content = new GUIContent( ( target as TextAsset ).text );
                var height  = style.CalcHeight( content, Screen.width );

                mScrollPos = GUILayout.BeginScrollView( mScrollPos );
                EditorGUILayout.SelectableLabel( content.text, style, GUILayout.Height( height ) );
                GUILayout.EndScrollView();
            }
            else
            {
                mRenderer.Setup( mHeaderHeight, Placeholder );
                mRenderer.Render( mDoc );
            }
        }
    }
}
