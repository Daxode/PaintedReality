using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Authoring;
using UnityEngine;

namespace Imperceptible.Components.Authoring {
	public class AuthorComponentGravityField : MonoBehaviour, IConvertGameObjectToEntity {
		[SerializeField] private GravityFieldType gravType;
		[SerializeField] private Vector3          gravityAcceleration;
		[SerializeField] private int              priority;

		public GravityFieldType GravType            => gravType;
		public Vector3 GravityAcceleration {
			get => gravityAcceleration;
			set => gravityAcceleration = value;
		}

		public int              Priority            => priority;

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
			dstManager.AddComponentData(entity, new ComponentGravityField {
				GravityFieldType = gravType,
				GravityAcceleration = gravityAcceleration,
				Priority = priority
			});
		}

#if UNITY_EDITOR
		private static bool           _drawGravityFields;
		private static SortedSet<int> _priorities = new SortedSet<int>();

		public static bool DrawGravityFields {
			get => _drawGravityFields;
			set {
				if (value != _drawGravityFields)
					OnUpdateGravityFieldViewer();
				_drawGravityFields = value;
			}
		}


		private static AuthorComponentGravityField[] gravFields;
		public static void OnUpdateGravityFieldViewer() {
			gravFields = FindObjectsOfType<AuthorComponentGravityField>();
			_priorities.Clear();
			foreach (var gravityField in gravFields) {
				_priorities.Add(gravityField.Priority);
			}
		}

		private void OnDrawGizmos() {
			if (!DrawGravityFields) return;
			Gizmos.matrix = transform.localToWorldMatrix;
			var physShape = GetComponent<PhysicsShapeAuthoring>();


			var index           = _priorities.TakeWhile(priorityLoop => Priority != priorityLoop).Count();
			var priorityPercent = index / (float) (_priorities.Count-1);
				
			Gizmos.color = priorityPercent < .5 ? 
				Color.Lerp (Color.green, Color.yellow, priorityPercent*2) : 
				Color.Lerp (Color.yellow, Color.red, (priorityPercent-.5f)*2);

			Gizmos.DrawRay(Vector3.zero, GravityAcceleration);
			
			switch (gravType) {
				case GravityFieldType.Parallel:
					Gizmos.DrawWireCube(Vector3.zero, physShape.GetBoxProperties().Size);
					break;
				case GravityFieldType.Sphere:
					var idk = new quaternion();
					Gizmos.DrawWireSphere(Vector3.zero, physShape.GetSphereProperties(out idk).Radius);
					break;
			}
		}
#endif	
	}
}