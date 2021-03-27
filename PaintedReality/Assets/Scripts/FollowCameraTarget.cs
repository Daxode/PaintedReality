using System.Linq;
using com.daxode.Components;
using com.daxode.Components.Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace com.daxode {
	public class FollowCameraTarget : MonoBehaviour {
		private Entity        _cameraTargetEntity;
		private EntityManager _entityManager;
		
		// Start is called before the first frame update
		private void Awake() {
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		// Update is called once per frame
		private void LateUpdate() {
			var queryTargets    = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<CameraFollowTarget>());
			var entitiesPlayer = queryTargets.ToEntityArray(Allocator.Temp);
			if (entitiesPlayer.Length == 0) return;

			var maxPriorityTarget = new CameraFollowTarget{Priority = -1};
			foreach (var entity in entitiesPlayer) {
				var currentFollowTarget = _entityManager.GetComponentData<CameraFollowTarget>(entity);
				if (currentFollowTarget <= maxPriorityTarget) continue;
				_cameraTargetEntity = entity;
				maxPriorityTarget = currentFollowTarget;
			}
			
			if (_cameraTargetEntity == Entity.Null) return;
			
			var playerLtw = _entityManager.GetComponentData<LocalToWorld>(_cameraTargetEntity);
			var transform1 = transform;
			transform1.position = playerLtw.Position;
			transform1.rotation = playerLtw.Rotation;
		}
	}
}