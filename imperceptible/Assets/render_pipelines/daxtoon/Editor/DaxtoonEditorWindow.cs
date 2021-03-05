using UnityEditor;
using UnityEngine;

namespace com.daxode.render_pipelines.daxtoon.Editor {
	public class DaxtoonEditorWindow : EditorWindow {
		[MenuItem("Window/Rendering/Daxtoon")]
		private static void ShowWindow() {
			var window = GetWindow<DaxtoonEditorWindow>();
			window.titleContent = new GUIContent("Daxtoon");
			window.Show();
		}

		private bool _enableDeveloperMode;
		private void OnGUI() {
			_enableDeveloperMode = EditorGUILayout.BeginToggleGroup("Developer mode", _enableDeveloperMode);
			EditorPrefs.SetBool("DeveloperMode", _enableDeveloperMode);
		}
	}
}