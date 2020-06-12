using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.IO;
using System;
using UnityEngine.Diagnostics;

namespace Cresspresso.Rosemary.Editor
{
	public class CombatUnitBuilder
	{
		private string unitName = "New Combat Unit";
		private string deathEffectSpawnerName = "@Death Effect Spawner";
		private string prefabDirectory = "Assets/Rosemary Wizard/Test/Test2";

		//[MenuItem("GameObject/Rosemary/Combat Unit and Spawner")]
		public static void Create()
			=> new CombatUnitBuilder().CreateImpl();

		private void CreateImpl()
		{
			EnsureAssetDirectoryExists(prefabDirectory);

			Undo.IncrementCurrentGroup();
			Undo.SetCurrentGroupName("Combat Unit named " + unitName);
			var undoGroupIndex = Undo.GetCurrentGroup();
			try
			{
				CreateSpawnerGo();
			}
			finally
			{
				Undo.CollapseUndoOperations(undoGroupIndex);
			}
		}

		private string GetPrefabPath(int i)
		{
			var filename = unitName;
			if (i != 0)
			{
				filename += $" ({i})";
			}
			filename += ".prefab";
			return Path.Combine(prefabDirectory, filename);
		}

		private GameObject spawnerGo;
		private RosemaryUnitSpawner spawnerComp;
		private void CreateSpawnerGo()
		{
			spawnerGo = new GameObject(unitName + " Spawner");
			Undo.RegisterCreatedObjectUndo(spawnerGo, "");

			if (Selection.activeGameObject && !EditorUtility.IsPersistent(Selection.activeGameObject))
			{
				Undo.SetTransformParent(spawnerGo.transform, Selection.activeGameObject.transform, "");
			}
			spawnerGo.transform.RosemaryResetLocal();

			Selection.activeGameObject = spawnerGo;

			spawnerComp = Undo.AddComponent<RosemaryUnitSpawner>(spawnerGo);

			CreateUnitGo();
		}

		private void EnsureAssetDirectoryExists(string directory)
		{
			if (string.IsNullOrEmpty(directory)) { throw new System.ArgumentException("Invalid directory path"); }

			if (!AssetDatabase.IsValidFolder(directory))
			{
				var parent = Path.GetDirectoryName(directory);
				EnsureAssetDirectoryExists(parent);
				AssetDatabase.CreateFolder(parent, Path.GetFileName(directory));
			}
		}

		private GameObject unitGo;
		private GameObject unitPrefab;
		private RosemaryUnit unitComp;
		private void CreateUnitGo()
		{
			unitGo = new GameObject(unitName);
			Undo.RegisterCreatedObjectUndo(unitGo, "");
			Undo.SetTransformParent(unitGo.transform, spawnerGo.transform, "");
			unitGo.transform.RosemaryResetLocal();

			Undo.AddComponent<RosemarySpawn>(unitGo);
			unitComp = Undo.AddComponent<RosemaryUnit>(unitGo);
			Undo.AddComponent<NavMeshAgent>(unitGo);

			unitComp.deathEffectSpawnerName = deathEffectSpawnerName;

			CreateCentreGo();
			CreateHullGo();

			unitPrefab = PrefabUtility.SaveAsPrefabAssetAndConnect(
				unitGo,
				Path.Combine(prefabDirectory, $"{unitName}.prefab"),
				InteractionMode.UserAction);
			spawnerComp.InternalSetPrefab(unitPrefab.GetComponent<RosemarySpawn>());
		}

		GameObject hullGo;
		private void CreateHullGo()
		{
			hullGo = new GameObject("Hull");
			Undo.RegisterCreatedObjectUndo(hullGo, "");
			Undo.SetTransformParent(hullGo.transform, unitGo.transform, "");
			hullGo.transform.RosemaryResetLocal();

			CreateTurretGo();
			CreateHullModel();
		}

		GameObject hullModelGo;
		private void CreateHullModel()
		{
			hullModelGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
			hullModelGo.name = "Hull Model (Cube)";
			Undo.RegisterCreatedObjectUndo(hullModelGo, "");
			Undo.SetTransformParent(hullModelGo.transform, hullGo.transform, "");
			hullModelGo.transform.RosemaryResetLocal();
		}

		GameObject centreGo;
		private void CreateCentreGo()
		{
			centreGo = new GameObject("Centre");
			Undo.RegisterCreatedObjectUndo(centreGo, "");
			Undo.SetTransformParent(centreGo.transform, unitGo.transform, "");
			centreGo.transform.RosemaryResetLocal();

			unitComp.centreTransform = centreGo.transform;

			var controllableSelectedEffectGo = CreateSelectedEffectGo(true);
			unitComp.controllableSelectionIndicator = controllableSelectedEffectGo;

			var uncontrollableSelectedEffectGo = CreateSelectedEffectGo(false);
			unitComp.uncontrollableSelectionIndicator = uncontrollableSelectedEffectGo;
		}

		private GameObject CreateSelectedEffectGo(bool controllable)
		{
			var go = new GameObject("Controllable Selected Effect");
			Undo.RegisterCreatedObjectUndo(go, "");
			Undo.SetTransformParent(go.transform, centreGo.transform, "");
			go.transform.RosemaryResetLocal();

			var ps = Undo.AddComponent<ParticleSystem>(go);
			var colorOverLifetimeModule = ps.colorOverLifetime;
			var c = colorOverLifetimeModule.color;
			c.mode = ParticleSystemGradientMode.Color;
			c.color = controllable ? Color.green : Color.gray;
			colorOverLifetimeModule.color = c;
			var renderer = ps.GetComponent<ParticleSystemRenderer>();
			if (!renderer.material)
			{
				Utility.TryCatchLog(() =>
				{
					const string path = "Resources/unity_builtin_extra/Default-Particle.mat";
					renderer.material = (Material)AssetDatabase.LoadMainAssetAtPath(path);
				});
			}

			return go;
		}

		private GameObject turretGo;
		private void CreateTurretGo()
		{
			turretGo = new GameObject("Turret");
			Undo.RegisterCreatedObjectUndo(turretGo, "");
			Undo.SetTransformParent(turretGo.transform, hullGo.transform, "");
			turretGo.transform.RosemaryResetLocal();

			CreateTurretModelGo();
			CreateGunMantletGo();
		}

		private void CreateTurretModelGo()
		{
			var turretModelGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
			turretModelGo.name = "Turret Model (Cube)";
			Undo.RegisterCreatedObjectUndo(turretModelGo, "");
			Undo.SetTransformParent(turretModelGo.transform, turretGo.transform, "");
			turretModelGo.transform.RosemaryResetLocal();
		}

		private GameObject gunMantletGo;
		private void CreateGunMantletGo()
		{
			gunMantletGo = new GameObject("Gun Mantlet");
			Undo.RegisterCreatedObjectUndo(gunMantletGo, "");
			Undo.SetTransformParent(gunMantletGo.transform, turretGo.transform, "");
			gunMantletGo.transform.RosemaryResetLocal();

			CreateProjectileSpawnPointGo();
			CreateGunBarrelModelGo();
		}

		GameObject projectileSpawnPointGo;
		private void CreateProjectileSpawnPointGo()
		{
			projectileSpawnPointGo = new GameObject("Projectile Spawn Point");
			Undo.RegisterCreatedObjectUndo(projectileSpawnPointGo, "");
			Undo.SetTransformParent(projectileSpawnPointGo.transform, gunMantletGo.transform, "");
			projectileSpawnPointGo.transform.RosemaryResetLocal();
			projectileSpawnPointGo.transform.localPosition = Vector3.forward * 1.0f;
		}

		GameObject gunBarrelModelGo;
		private void CreateGunBarrelModelGo()
		{
			gunBarrelModelGo = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			gunBarrelModelGo.name = "Gun Barrel Model (Cylinder)";
			Undo.RegisterCreatedObjectUndo(gunBarrelModelGo, "");
			Undo.SetTransformParent(gunBarrelModelGo.transform, gunMantletGo.transform, "");
			gunBarrelModelGo.transform.RosemaryResetLocal();
			gunBarrelModelGo.transform.localRotation = Quaternion.Euler(90, 0, 0);
			gunBarrelModelGo.transform.localScale = new Vector3(0.1f, 1.0f, 0.1f);
		}
	}
}
