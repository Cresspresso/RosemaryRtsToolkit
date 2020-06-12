
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	[RequireComponent(typeof(LineRenderer))]
	public class RosemaryDestinationIndicator : MonoBehaviour
	{
		public RosemaryUnit unit;
		public LineRenderer lr;
		public Gradient colorsMove = new Gradient();
		public Gradient colorsAttack = new Gradient();
		public Gradient colorsFollow = new Gradient();

		private void Awake()
		{
			unit = GetComponentInParent<RosemaryUnit>();
			lr = GetComponent<LineRenderer>();
		}

		private void LateUpdate()
		{
			lr.SetPosition(1, transform.InverseTransformPoint(unit.destination));
			if (unit.isAttacking)
			{
				lr.colorGradient = colorsAttack;
			}
			else if (unit.targetUnit)
			{
				lr.colorGradient = colorsFollow;
			}
			else
			{
				lr.colorGradient = colorsMove;
			}
		}
	}
}
