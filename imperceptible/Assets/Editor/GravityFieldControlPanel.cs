#if true
using System;
using Imperceptible.Components;
using Imperceptible.Components.Authoring;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;

namespace Imperceptible.Editor {
	[CustomEditor(typeof(AuthorComponentGravityField))]
	[CanEditMultipleObjects]
	public class GravityFieldControlPanel : UnityEditor.Editor {
		private GravityFieldType _currentType     = GravityFieldType.Parallel;
		private bool             _showScrollPanel;
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			
			var gravityField = (AuthorComponentGravityField) target;
			var physicsShape = gravityField.GetComponent<PhysicsShapeAuthoring>();

			if (gravityField.GravType != _currentType) {
				_currentType = gravityField.GravType;
				switch (_currentType) {
					case GravityFieldType.Parallel:
						physicsShape.SetBox(new BoxGeometry());
						break;
					case GravityFieldType.Sphere:
						physicsShape.SetSphere(new SphereGeometry(), quaternion.identity);
						break;
					default:
						physicsShape.SetBox(new BoxGeometry());
						break;
				}

				AuthorComponentGravityField.OnUpdateGravityFieldViewer();
			}

			_showScrollPanel = EditorGUILayout.Foldout(_showScrollPanel, "Custom Imperceptible Tooling");
			if (_showScrollPanel) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Draw Gravity Tools");
				AuthorComponentGravityField.DrawGravityFields = EditorGUILayout.Toggle(AuthorComponentGravityField.DrawGravityFields);
				EditorGUILayout.EndHorizontal();
			}
		}
		
		public void OnSceneGUI() {
			if (!AuthorComponentGravityField.DrawGravityFields) return;
			var t = (target as AuthorComponentGravityField);

			EditorGUI.BeginChangeCheck();
			Handles.matrix = t.transform.localToWorldMatrix;
			Vector3 pos = Handles.PositionHandle(t.GravityAcceleration, quaternion.identity);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, "Move point");
				t.GravityAcceleration = pos;
			}
		}
	}
}
#endif