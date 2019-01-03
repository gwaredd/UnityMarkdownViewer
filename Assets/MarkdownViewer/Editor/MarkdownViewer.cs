////////////////////////////////////////////////////////////////////////////////
//------------------------------------------------------------------------------

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

        public override void OnInspectorGUI()
        {
            // get file extension

            var path = AssetDatabase.GetAssetPath( target );
            var ext  = Path.GetExtension( path );

            if( ".md".Equals( ext, StringComparison.OrdinalIgnoreCase ) == false )
            {
                base.OnInspectorGUI();
                return;
            }

            // do the thing ...


            Debug.Log( "#path# " + path );
            base.OnInspectorGUI();
        }
    }
}
