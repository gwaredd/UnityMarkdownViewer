////////////////////////////////////////////////////////////////////////////////

using System.IO;
using UnityEditor;

namespace MG.MDV
{
    [CustomEditor( typeof( DefaultAsset ), true )]
    public class MarkdownViewerFallback : MarkdownViewer
    {
        private new void OnEnable()
        {
            mContent = File.ReadAllText( AssetDatabase.GetAssetPath( target ) );
            base.OnEnable();
        }
    }
}

