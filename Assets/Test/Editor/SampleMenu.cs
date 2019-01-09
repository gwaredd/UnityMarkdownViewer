using UnityEditor;
using UnityEngine;

namespace MG
{
    class MyMenus
    {
        [MenuItem( "Tools/My Custom Action" )]
        public static void OnMyCustomAction()
        {
            Debug.Log( "OnMyCustomAction" );
        }
    }
}
