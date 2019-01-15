////////////////////////////////////////////////////////////////////////////////
/**/

using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MG.MDV
{
    public static class ProcessFont
    {
        const string ProcessFontMenu = "Assets/Extract Font Info";

        [MenuItem( ProcessFontMenu, true )]
        static bool ValidateExtractFontInfo()
        {
            return ( Selection.activeObject as Font ) != null;
        }

        [MenuItem( ProcessFontMenu )]
        public static void ExtractFontInfo()
        {
            Selection.objects.ToList().ForEach( o => ExtractFont( o as Font ) );
            AssetDatabase.Refresh();
        }

        public static void ExtractFont( Font font )
        {
            if( font == null )
            {
                return;
            }

            var nl     = Environment.NewLine;
            var indent = new string( ' ', 12 );

            var path   = AssetDatabase.GetAssetPath( font );
            var name   = Path.GetFileNameWithoutExtension( path ).Replace( "-", "" );

            var text   = new StringBuilder();

            text.Append( mHeader.Replace( "#NAME#", $"Font{name}" ) );

            text.Append( $"{indent}Name    = \"{font.name}\";{nl}" );
            text.Append( $"{indent}Size    = {font.fontSize}.0f;{nl}" );
            text.Append( $"{indent}Advance = new Dictionary<int, float>(){nl}" );
            text.Append( $"{indent}{{{nl}" );


            indent = new string( ' ', 16 );
            var col = 0;

            text.Append( indent );

            foreach( var info in font.characterInfo )
            {
                var e = $"{{{info.index},{info.advance}}},";

                text.Append( e );
                col += e.Length;

                if( col > 110 )
                {
                    text.Append( $"{nl}{indent}" );
                    col = 0;
                }
            }

            text.Append( mFooter );

            // save

            var dir  = Path.GetDirectoryName( Path.GetDirectoryName( path ) );
            var file = $"{dir}\\Layout\\Font{name}.cs";

            Debug.Log( $"Saving {file}" );
            File.WriteAllText( file, text.ToString() );
        }

        //------------------------------------------------------------------------------

        static string mHeader = @"////////////////////////////////////////////////////////////////////////////////
// !!! AUTO-GENERATED FILE !!!

using System.Collections.Generic;

namespace MG.MDV
{
    public class #NAME# : FontInfo
    {
        public #NAME#()
        {
";

        //------------------------------------------------------------------------------

        static string mFooter = @"
            };
        }
    }
}
";
    }
}

/**/
