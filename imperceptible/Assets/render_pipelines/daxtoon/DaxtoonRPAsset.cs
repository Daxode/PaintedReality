using UnityEngine;
using UnityEngine.Rendering;

namespace com.daxode.render_pipelines.daxtoon {
	[CreateAssetMenu(menuName = "Rendering/DaxtoonRPAsset")]
	public class DaxtoonRPAsset : RenderPipelineAsset {
		// This data can be defined in the Inspector for each Render Pipeline Asset
		public Color                   exampleColor;
		public string                  exampleString;
		public RenderPipelineResources renderPipelineResources;
		
		// Unity calls this method before rendering the first frame.
		// If a setting on the Render Pipeline Asset changes, Unity destroys the current Render Pipeline Instance and calls this method again before rendering the next frame.
		protected override RenderPipeline CreatePipeline() {
			// Instantiate the Render Pipeline that this custom SRP uses for rendering, and pass a reference to this Render Pipeline Asset.
			// The Render Pipeline Instance can then access the configuration data defined above.
			return new DaxtoonRPInstance(this);
		}
	}
}