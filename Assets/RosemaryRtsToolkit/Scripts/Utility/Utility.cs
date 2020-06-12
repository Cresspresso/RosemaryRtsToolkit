using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	/// <summary>
	/// Static utility functions for scripting in Unity.
	/// </summary>
	/// <changelog>
	///		<log date="08/05/2020"
	///		version="1.0"
	///		author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public static class Utility
	{
		/// <summary>
		/// Executes the action.
		/// If an exception is thrown it is logged to <see cref="Debug.LogException(Exception)"/>.
		/// This function does not throw.
		/// </summary>
		/// <changelog>
		///		<log date="08/05/2020"
		///		version="1.0"
		///		author="Elijah Shadbolt">
		///			Initial version.
		///		</log>
		/// </changelog>
		public static void TryCatchLog(Action action)
		{
			try
			{
				action();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		/// <summary>
		/// Executes the action.
		/// If an exception is thrown it is logged to <see cref="Debug.LogException(Exception, UnityEngine.Object)"/>.
		/// This function does not throw.
		/// </summary>
		/// <changelog>
		///		<log date="15/05/2020"
		///		version="1.0"
		///		author="Elijah Shadbolt">
		///			Initial version.
		///		</log>
		/// </changelog>
		public static void TryCatchLog(Action action, UnityEngine.Object context)
		{
			try
			{
				action();
			}
			catch (Exception e)
			{
				Debug.LogException(e, context);
			}
		}

		public static int Cycle(int value, int length)
		{
			value %= length;
			return value < 0 ? value + length : value;
		}



		public static void RosemaryResetLocal(this Transform t)
		{
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
		}

		public static void RosemarySetParent(
			this Transform t,
			Transform parent,
			Vector3 localPosition,
			Quaternion localRotation,
			Vector3 localScale)
		{
			t.localPosition = localPosition;
			t.localRotation = localRotation;
			t.localScale = localScale;
			t.SetParent(parent, false);
		}

		public static void RosemarySetParent(
			this Transform t,
			Transform parent,
			Vector3 localPosition,
			Quaternion localRotation)
		=> RosemarySetParent(t, parent, localPosition, localRotation, Vector3.one);

		public static void RosemarySetParent(
			this Transform t,
			Transform parent,
			Vector3 localPosition)
		=> RosemarySetParent(t, parent, localPosition, Quaternion.identity, Vector3.one);

		public static void RosemarySetParent(
			this Transform t,
			Transform parent)
		=> RosemarySetParent(t, parent, Vector3.zero, Quaternion.identity, Vector3.one);
	}
}
