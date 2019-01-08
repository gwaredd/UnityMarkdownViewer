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
    // TODO: variants - rich text, powerUI (or other HTML renderer), "side" browser (chromium)
    // TODO: test what happens when we have multiple TextAsset editors ...
    //          https://github.com/roydejong/chromium-unity-server
    //          https://ultralig.ht/


    [CustomEditor( typeof( TextAsset ) )]
    public class MarkdownViewer : Editor
    {
        public GUISkin Skin;


        //------------------------------------------------------------------------------

        private Editor mDefaultEditor;

        private void DrawDefault()
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

        MarkdownDocument mDoc;
        RendererMarkdown mRenderer;
        MarkdownPipeline mPipeline;
        bool             mRaw = false;

        public override void OnInspectorGUI()
        {
            // has target?

            var asset = target as TextAsset;

            if( asset == null )
            {
                DrawDefault();
                return;
            }

            // TODO: configurable extensions?
            // file extension is .md

            var path = AssetDatabase.GetAssetPath( target );
            var ext  = Path.GetExtension( path );

            if( ".md".Equals( ext, StringComparison.OrdinalIgnoreCase ) == false )
            {
                DrawDefault();
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

        void DrawIMGUI()
        {
            GUI.skin            = Skin;
            GUI.enabled         = true;
            GUI.backgroundColor = Color.white;

            mRaw = GUILayout.Toggle( mRaw, "Raw" );

            GUILayout.Space( 10.0f );

            if( mRaw )
            {
                DrawDefault();
            }
            else
            {
                ClearBackground();
                mRenderer.Render( mDoc );
            }
        }

        //------------------------------------------------------------------------------

        void ClearBackground()
        {
            var r = GUILayoutUtility.GetRect(0,0);

            if( Event.current.type == EventType.Repaint )
            {
                const float margin = 4.0f;
                GUI.DrawTexture( new Rect( 0, r.y - margin, EditorGUIUtility.currentViewWidth, Screen.height ), EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill );
            }
        }
    }
}
