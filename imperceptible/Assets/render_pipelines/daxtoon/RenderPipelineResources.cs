using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.daxode.render_pipelines.daxtoon {
	[CreateAssetMenu(fileName = "Daxtoon RenderPipelineResource", menuName = "Rendering/Daxtoon/")]
	public class RenderPipelineResources : ScriptableObject {
		[Serializable, ReloadGroup]
		public sealed class ShaderResources {
			// Defaults
			[Reload("DaxtoonUnlit.shader")]
			public Shader UnlitShader;
		}

		[Serializable, ReloadGroup]
		public sealed class MaterialResources {
			
		}

		[Serializable, ReloadGroup]
		public sealed class TextureResources {
			// // Debug
			// [Reload("Runtime/RenderPipelineResources/Texture/DebugFont.tga")]
			// public Texture2D debugFontTex;
			//
			// // Post-processing
			// [Reload(new[] {
			// 	"Runtime/RenderPipelineResources/Texture/FilmGrain/Thin01.png",
			// 	"Runtime/RenderPipelineResources/Texture/FilmGrain/Thin02.png",
			// })]
			// public Texture2D[] filmGrainTex;
		}

		[Serializable, ReloadGroup]
		public sealed class ShaderGraphResources { }

		[Serializable, ReloadGroup]
		public sealed class AssetResources { 
			//Area Light Emissive Meshes
			//[Reload("Runtime/RenderPipelineResources/Mesh/Cylinder.fbx")]
			//public Mesh emissiveCylinderMesh;
		}

		public ShaderResources      shaders;
		public MaterialResources    materials;
		public TextureResources     textures;
		public ShaderGraphResources shaderGraphs;
		public AssetResources       assets;
	}

#if UNITY_EDITOR
	[UnityEditor.CustomEditor(typeof(RenderPipelineResources))]
	class RenderPipelineResourcesEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			DrawDefaultInspector();

			// Add a "Reload All" button in inspector when we are in developer's mode
			if (!UnityEditor.EditorPrefs.GetBool("DeveloperMode") || !GUILayout.Button("Reload All")) return;
			
			foreach (var field in typeof(RenderPipelineResources).GetFields())
				field.SetValue(target, null);

			ResourceReloader.ReloadAllNullIn(target,"Assets/render_pipelines/daxtoon");
		}
	}
#endif
}