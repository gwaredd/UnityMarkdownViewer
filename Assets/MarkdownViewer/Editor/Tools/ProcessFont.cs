////////////////////////////////////////////////////////////////////////////////
/*

using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MG.MDV
{
    [CustomEditor( typeof( Font ) )]
    public class ProcessFont : Editor
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = true;

            if( GUILayout.Button( "PreProcess" ) == false )
            {
                return;
            }

            var font = target as Font;

            var text = new StringBuilder();

            text.Append( $"info.Name    = \"{font.name}\";\n" );
            text.Append( $"info.Size    = {font.fontSize};\n" );
            text.Append( $"info.Advance = new Dictionary<int, float>() {{\n" );
            text.Append( "    " );

            var col = 0;

            foreach( var info in font.characterInfo )
            {
                var e = $"{{ {info.index}, {info.advance} }},";

                text.Append( e );
                col += e.Length;

                if( col > 110 )
                {
                    text.Append( "\n    " );
                    col = 0;
                }
            }

            text.Append( "\n};\n" );

            File.WriteAllText( "Temp\\MDF.cs", text.ToString() );
            Debug.Log( "done" );
        }
    }
}

/**/
