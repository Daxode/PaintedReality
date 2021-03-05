using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.Rendering;

namespace com.daxode.render_pipelines.daxtoon {
	public class DaxtoonRPInstance : RenderPipeline {
		// Use this variable to a reference to the Render Pipeline Asset that was passed to the constructor
		private                 DaxtoonRPAsset _rpAsset;
		private                 RenderGraph    _mRenderGraph;
		private static readonly int            MainTexture = Shader.PropertyToID("_MainTexture");
		private static readonly int            FloatParam  = Shader.PropertyToID("_FloatParam");

		private void InitializeRenderGraph(ScriptableRenderContext renderContext) {
			_mRenderGraph = new RenderGraph("DaxtoonRG");

			var renderGraphParams = new RenderGraphParameters() {
				scriptableRenderContext = renderContext,
				commandBuffer = CommandBufferPool.Get(""),
				currentFrameIndex = _frameCount
			};

			_mRenderGraph.Begin(renderGraphParams);
		}

		private void CleanupRenderGraph() {
			_mRenderGraph.Cleanup();
			_mRenderGraph = null;
		}

		//The constructor has an instance of the DaxtoonRPAsset class as its parameter.
		public DaxtoonRPInstance(DaxtoonRPAsset asset) {
			_rpAsset = asset;
		}

		private class MyRenderPassData {
			public float         Parameter;
			public Material      Material;
			public TextureHandle InputTexture;
			public TextureHandle OutputTexture;
		}

		private void AddMyRenderPass(RenderGraph renderGraph, TextureHandle inputTexture, float parameter,
		                                      Material    material) {
			using (var builder = renderGraph.AddRenderPass<MyRenderPassData>("My Render Pass", out var passData)) {
				passData.Parameter = parameter;
				passData.Material = material;

				// Tells the graph that this pass will read inputTexture.
				passData.InputTexture = builder.ReadTexture(inputTexture);

				// Creates the output texture.
				var output = renderGraph.CreateTexture(new TextureDesc(Vector2.one, true, true) {
					colorFormat = GraphicsFormat.R8G8B8A8_UNorm, clearBuffer = true, clearColor = Color.black,
					name = "Output"
				});

				// Tells the graph that this pass will write this texture and needs to be set as render target 0.
				passData.OutputTexture = builder.UseColorBuffer(output, 0);

				builder.SetRenderFunc((MyRenderPassData data, RenderGraphContext ctx) => {
					// Setup material for rendering
					var materialPropertyBlock = ctx.renderGraphPool.GetTempMaterialPropertyBlock();
					materialPropertyBlock.SetTexture(MainTexture, data.InputTexture);
					CoreUtils.DrawFullScreen(ctx.cmd, data.Material, materialPropertyBlock);
				});
			}
		}

		private class SetFinalTargetPassData {
			public TextureHandle FinalTarget;
		}
		
		private void SetFinalTarget(RenderGraph renderGraph, TextureHandle inputTexture) {
			using (var builder = renderGraph.AddRenderPass<SetFinalTargetPassData>("Set Final Target", out var passData)) {
				// Tells the graph that this pass will read inputTexture.
				passData.FinalTarget = builder.ReadTexture(inputTexture);

				builder.SetRenderFunc((SetFinalTargetPassData data, RenderGraphContext ctx) => {
					CoreUtils.SetRenderTarget(ctx.cmd, data.FinalTarget);
				});
			}
		}

		private int _frameCount;
		private void DoFrameCount(Camera[] cameras) {
#if UNITY_EDITOR
			var newCount = _frameCount;
			if (cameras.Any(c => c.cameraType != CameraType.Preview)) {
				newCount++;
			}
#else
            int newCount = Time.frameCount;
#endif
			_frameCount = newCount;
		}

		private TextureHandle CreateColorBuffer(RenderGraph renderGraph, Camera cam, bool msaa) {
			FastMemoryDesc colorFastMemDesc;
			colorFastMemDesc.inFastMemory = true;
			colorFastMemDesc.residencyFraction = 1.0f;
			colorFastMemDesc.flags = FastMemoryFlags.SpillTop;

			return renderGraph.CreateTexture(
				new TextureDesc(Vector2.one, true, true) {
					colorFormat = GraphicsFormat.B10G11R11_UFloatPack32,
					enableRandomWrite = !msaa,
					bindTextureMS = msaa,
					enableMSAA = msaa,
					clearBuffer = false,
					clearColor = Color.green,
					name = msaa ? "CameraColorMSAA" : "CameraColor"
				  , fastMemoryDesc = colorFastMemDesc
				});
		}

		// Unity calls this method once per frame for each CameraType that is currently rendering.
		protected override void Render(ScriptableRenderContext renderContext, Camera[] cameras) {
			BeginFrameRendering(renderContext, cameras);
			DoFrameCount(cameras);
			InitializeRenderGraph(renderContext);

			var daxtoonUnlitMaterial = CoreUtils.CreateEngineMaterial(_rpAsset.renderPipelineResources.shaders.UnlitShader);

			foreach (var camera in cameras) {
				var targetTexture = camera.targetTexture;
				var targetId = targetTexture
					? targetTexture
					: new RenderTargetIdentifier(BuiltinRenderTextureType.CameraTarget);

				var backBuffer  = _mRenderGraph.ImportBackbuffer(targetId);
				var colorBuffer = CreateColorBuffer(_mRenderGraph, camera, true);

				AddMyRenderPass(_mRenderGraph, backBuffer, 5, daxtoonUnlitMaterial);
				SetFinalTarget(_mRenderGraph, backBuffer);
			}
			
			_mRenderGraph.Execute();
			_mRenderGraph.EndFrame();
			EndFrameRendering(renderContext, cameras);
			CleanupRenderGraph();
		}
	}
}