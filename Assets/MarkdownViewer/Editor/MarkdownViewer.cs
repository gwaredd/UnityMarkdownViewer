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

        //------------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            // has target?

            var asset = target as TextAsset;

            if( asset == null )
            {
                base.OnInspectorGUI();
                return;
            }

            // file extension is .md

            var path = AssetDatabase.GetAssetPath( target );
            var ext  = Path.GetExtension( path );

            if( ".md".Equals( ext, StringComparison.OrdinalIgnoreCase ) == false )
            {
                base.OnInspectorGUI();
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
                Debug.Log( block.ToString() );
            }

            // HtmlRenderer

            GUILayout.Label( Markdown.ToHtml( asset.text ), Skin.label );
        }
    }
}
