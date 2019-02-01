using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;

namespace MG.MDV
{
    [ScriptedImporter( 1, "markdown" )]
    public class MarkdownImporter : ScriptedImporter
    {
        public override void OnImportAsset( AssetImportContext ctx )
        {
            var md = ScriptableObject.CreateInstance<MarkdownObject>();
            md.Text = File.ReadAllText( ctx.assetPath );

            ctx.AddObjectToAsset( "contents", md );
            ctx.SetMainObject( md );
        }
    }
}
