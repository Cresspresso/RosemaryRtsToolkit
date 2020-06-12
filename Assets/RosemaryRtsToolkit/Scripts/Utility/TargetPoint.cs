using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Represents a <see cref="Vector3"/> position in world space,
	///			which can be retrieved as an optional with <see cref="position"/>.
	///		</para>
	///		<para>
	///			It can be a fixed point,
	///			or follow a <see cref="UnityEngine.Transform"/> as it moves,
	///			or use the fallback argument.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="08/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	[System.Serializable]
	public struct TargetPoint
	{
		public enum Mode { None, Point, Transform, }
		public Mode mode;
		public Transform transform;
		public Vector3 point;

		public TargetPoint(Mode mode, Transform transform, Vector3 point)
		{
			this.mode = mode;
			this.transform = transform;
			this.point = point;
		}

		public TargetPoint(Mode mode, TargetPoint other)
		{
			this.mode = mode;
			transform = other.transform;
			point = other.point;
		}

		public static TargetPoint None => new TargetPoint();
		public static TargetPoint NoneWith(TargetPoint other) => new TargetPoint(Mode.None, other);
		public static TargetPoint Point(Vector3 point) => new TargetPoint(Mode.Point, null, point);
		public static TargetPoint Transform(Transform transform) => new TargetPoint(Mode.Transform, transform, default);

		/// <summary>
		/// <para>
		///		Returns the destination position in world space, or <see langword="null"/>.
		/// </para>
		/// <para>
		///		If <see cref="mode"/> is <see cref="Mode.None"/>,
		///		or <see cref="mode"/> is <see cref="Mode.Transform"/>
		///		and <see cref="transform"/> property is <see langword="null"/>,
		///		returns <see langword="null"/>.
		/// </para>
		/// <para>
		///		If <see cref="mode"/> is <see cref="Mode.Point"/>, returns <see cref="point"/> property.
		/// </para>
		/// <para>
		///		If <see cref="mode"/> is <see cref="Mode.Transform"/>
		///		and <see cref="transform"/> is not <see langword="null"/>,
		///		returns <see cref="transform"/> property.
		/// </para>
		/// </summary>
		/// <changelog>
		///		<log date="08/05/2020"
		///		version="1.0"
		///		author="Elijah Shadbolt">
		///			Initial version.
		///		</log>
		/// </changelog>
		public Vector3? position {
			get
			{
				switch (mode)
				{
					default:
					case Mode.None:
						return null;
					case Mode.Point:
						return point;
					case Mode.Transform:
						return transform && transform.gameObject.activeInHierarchy
							? (Vector3?)transform.position
							: null;
				}
			}
		}

		public bool hasPosition {
			get
			{
				switch (mode)
				{
					default:
					case Mode.None:
						return false;
					case Mode.Point:
						return true;
					case Mode.Transform:
						return transform && transform.gameObject.activeInHierarchy;
				}
			}
		}
	}
}
