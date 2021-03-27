using com.daxode.Components.Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace com.daxode {
	public class FollowPlayer : MonoBehaviour {
		private Entity        _playerEntity;
		private EntityManager _entityManager;
		private float         _waitTime = 0.1f;
		
		// Start is called before the first frame update
		private void Awake() {
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		// Update is called once per frame
		private void LateUpdate() {
			if (_playerEntity == Entity.Null) {
				if (_waitTime <= 0) {
					var queryPlayer    = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<TagPlayer>());
					var entitiesPlayer = queryPlayer.ToEntityArray(Allocator.Temp);
					if (entitiesPlayer.Length == 0) return;
					_playerEntity = entitiesPlayer[0];
				} else {
					_waitTime -= Time.deltaTime;
					return;
				}
			}

			var playerLTW = _entityManager.GetComponentData<LocalToWorld>(_playerEntity);
			transform.position = playerLTW.Position;
			transform.rotation = playerLTW.Rotation;
		}
	}
}