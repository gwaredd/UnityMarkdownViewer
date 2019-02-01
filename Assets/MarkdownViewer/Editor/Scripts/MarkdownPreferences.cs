////////////////////////////////////////////////////////////////////////////////

using UnityEditor;
using UnityEngine;

namespace MG.MDV
{
    public class Preferences
    {
        private static readonly string KeyJIRA = "MG/MDV/JIRA";
        private static readonly string KeyHTML = "MG/MDV/HTML";

        private static string mJIRA        = string.Empty;
        private static bool   mStripHTML   = true;
        private static bool   mPrefsLoaded = false;

        public static string JIRA      { get { LoadPrefs(); return mJIRA; } }
        public static bool   StripHTML { get { LoadPrefs(); return mStripHTML; } }

        private static void LoadPrefs()
        {
            if( !mPrefsLoaded )
            {
                mJIRA        = EditorPrefs.GetString( KeyJIRA, "" );
                mStripHTML   = EditorPrefs.GetBool( KeyHTML, true );
                mPrefsLoaded = true;
            }
        }

        // TODO: obsolete in 2019
        [PreferenceItem( "Markdown" )]
        private static void DrawPreferences()
        {
            LoadPrefs();

            mJIRA      = EditorGUILayout.TextField( "JIRA URL", mJIRA );
            mStripHTML = EditorGUILayout.Toggle( "Strip HTML", mStripHTML );

            if( GUI.changed )
            {
                EditorPrefs.SetString( KeyJIRA, mJIRA );
                EditorPrefs.SetBool( KeyHTML, mStripHTML );
            }
        }
    }
}
