////////////////////////////////////////////////////////////////////////////////

using UnityEditor;
using System;
using System.Reflection;

namespace MG
{
    public class MyBrowserWindow
    {
        [MenuItem( "Tools/My Browser Window" )]
        static void OpenWindow()
        {
            /// <see cref="UnityEditor.Web.WebViewEditorWindow.Create"/>

            var type   = Type.GetType( "UnityEditor.Web.WebViewEditorWindowTabs, UnityEditor" );
            var method = type.GetMethod( "Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy );
            method.MakeGenericMethod( type ).Invoke( null, new object[] { "My Browser", "https://www.google.com",  200, 400, 1920, 1080 } );
        }
    }
}
