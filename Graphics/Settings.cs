﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.PostProcessLayer;

namespace Graphics
{
	public class Settings
	{
		public Antialiasing Antialiasing = Antialiasing.SubpixelMorphologicalAntialiasing;

		public Task Save() {
			return Task.Run(() => {
				var filepath = $"{Main.modEntry.Path}Settings.json";
				try {
					using (var writer = new StreamWriter(filepath)) {
						var json = JsonConvert.SerializeObject(this, Formatting.Indented);
						writer.Write(json);
					}
				}
				catch (Exception e) {
					Main.Logger.Log($"Can't save {filepath}. ex: {e}");
				}
			});
		}

		public static Settings Load() {
			var filepath = $"{Main.modEntry.Path}Settings.json";
			Settings settings = null;
			if (File.Exists(filepath)) {
				try {
					using (var reader = new StreamReader(filepath)) {
						var json = reader.ReadToEnd();
						settings = JsonConvert.DeserializeObject<Settings>(json);
						if (settings == null) {
							Main.Logger.Error("Could not read settings, creating new");
							settings = new Settings();
						}
					}
				}
				catch (Exception e) {
					Main.Logger.Log($"Can't read {filepath}. ex: {e}");
				}
			}
			else {
				Main.Logger.Log($"No settings found, using defaults");
				settings = new Settings();
			}
			return settings;
		}
	}
}
