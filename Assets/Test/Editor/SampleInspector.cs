////////////////////////////////////////////////////////////////////////////////

using UnityEditor;
using UnityEngine;

namespace MG
{
    public class MyScriptableObject : ScriptableObject
    {
    }

    [CustomEditor( typeof( MyScriptableObject ) )]
    public class MyCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            // TODO: draw custom GUI
        }
    }
}
