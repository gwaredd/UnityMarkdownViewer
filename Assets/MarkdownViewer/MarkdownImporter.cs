using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;

namespace MG.MDV
{
    [ScriptedImporter( 1, "markdown" )]
    public class MarkdownAssetImporter : ScriptedImporter
    {
        public override void OnImportAsset( AssetImportContext ctx )
        {
            var md = new TextAsset( File.ReadAllText( ctx.assetPath ) );

            ctx.AddObjectToAsset( "main", md );
            ctx.SetMainObject( md );
        }
    }
}
