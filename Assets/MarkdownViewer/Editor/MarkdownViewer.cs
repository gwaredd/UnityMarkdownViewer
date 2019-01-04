////////////////////////////////////////////////////////////////////////////////

using Markdig;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MG
{
    [CustomEditor( typeof( TextAsset ) )]
    public class MarkdownViewer : Editor
    {
        public GUISkin Skin;

        private Editor mDefaultEditor;

        // TODO: powerUI to render markdown? or markdown interpretted as unity rich text / GUI

        // TODO: test what happens when we have multiple TextAsset editors ...


        //------------------------------------------------------------------------------

        private void DrawDefault()
        {
            if( mDefaultEditor == null )
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach( var assembly in assemblies )
                {
                    var type = assembly.GetType( "UnityEditor.TextAssetInspector" );

                    if( type != null )
                    {
                        mDefaultEditor = Editor.CreateEditor( target, type );
                        break;
                    }
                }
            }

            if( mDefaultEditor != null )
            {
                mDefaultEditor.OnInspectorGUI();
            }
        }

        //------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            // has target?

            var asset = target as TextAsset;

            if( asset == null )
            {
                DrawDefault();
                return;
            }

            // file extension is .md

            var path = AssetDatabase.GetAssetPath( target );
            var ext  = Path.GetExtension( path );

            if( ".md".Equals( ext, StringComparison.OrdinalIgnoreCase ) == false )
            {
                DrawDefault();
                return;
            }

            // TODO: ...
            // Configure the pipeline with all advanced extensions active
            //var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            //var result = Markdown.ToHtml("This is a text with some *emphasis*", pipeline);

            // do the thing ...

            var doc = Markdown.Parse( asset.text );

            foreach( var block in doc )
            {
                //Debug.Log( block.ToString() );
            }

            // HtmlRenderer

            GUILayout.Label( Markdown.ToHtml( asset.text ), Skin.label );
        }
    }
}
