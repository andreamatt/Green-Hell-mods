using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Graphics
{
	public class Graphics : MonoBehaviour
	{
		PostProcessLayer post_layer;

		private void Start() {
			Main.settings = Settings.Load();
		}

		private void Update() {
			var changed = false;

			if (post_layer == null) {
				getEffects();

				if (post_layer != null) {
					post_layer.antialiasingMode = Main.settings.Antialiasing;
				}
			}
			else {
				if (Input.GetKeyDown(KeyCode.J)) {
					var current = post_layer.antialiasingMode;
					Main.settings.Antialiasing = post_layer.antialiasingMode = (PostProcessLayer.Antialiasing)(((int)current + 1) % 4);
					Main.Logger.Log($"AntiAliasing changed from {current} to {post_layer.antialiasingMode}");

					changed = true;
				}
			}

			if (changed) {
				Main.Save();
			}
		}

		private void getEffects() {
			if (Camera.main != null) {
				post_layer = Camera.main.GetComponent<PostProcessLayer>();
			}
		}

	}
}
