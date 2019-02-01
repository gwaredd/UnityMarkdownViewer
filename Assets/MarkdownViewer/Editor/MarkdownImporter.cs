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
            var md = ScriptableObject.CreateInstance<MarkdownAsset>();
            md.Text = File.ReadAllText( ctx.assetPath );

            ctx.AddObjectToAsset( "contents", md );
            ctx.SetMainObject( md );
        }
    }
}
