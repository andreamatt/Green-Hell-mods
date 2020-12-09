using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;

namespace Graphics
{
    public static class Main
	{
		public static bool enabled;
		public static Harmony harmony;
		public static Graphics graphics;
		public static UnityModManager.ModEntry modEntry;
		public static Settings settings;

		public static UnityModManager.ModEntry.ModLogger Logger => modEntry.Logger;

		static bool Load(UnityModManager.ModEntry modEntry) {
			Main.modEntry = modEntry;
			Main.Logger.Log("Load");
			modEntry.OnToggle = OnToggle;
			modEntry.OnUnload = Unload;
			//modEntry.OnSaveGUI = OnSave;

			return true;
		}

		static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
			Main.Logger.Log("OnToggle " + value.ToString());
			if (enabled == value) {
				return true;
			}

			enabled = value;

			if (value) {
				// create harmony instance
				harmony = new Harmony(modEntry.Info.Id);

				// patch
				harmony.PatchAll(Assembly.GetExecutingAssembly());

				// instantiate menu and XLGraphics monobehaviour
				graphics = (new GameObject()).AddComponent<Graphics>();
				GameObject.DontDestroyOnLoad(graphics.gameObject);
			}
			else {
				// unpatch
				harmony.UnpatchAll(harmony.Id);

				// destroy menu and XLGraphics monobehaviour
				GameObject.DestroyImmediate(graphics.gameObject);
			}
			Main.Logger.Log("Loaded");
			return true;
		}

		static void OnSave(UnityModManager.ModEntry modEntry) {
			Save();
		}

		public static void Save() {
			settings.Save();
		}

		static bool Unload(UnityModManager.ModEntry modEntry) {
			Main.Logger.Log("Unload");

			return true;
		}
	}
}
