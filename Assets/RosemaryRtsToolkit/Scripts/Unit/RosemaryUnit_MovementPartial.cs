using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

namespace Cresspresso.Rosemary
{
	public partial class RosemaryUnit
	{
		public NavMeshAgent agent { get; private set; }
		public bool canMove => agent;

		[Header("Movement")]
		public float agentUpdatePeriod = 0.3f;
		public float followRange = 4.0f;
		public float attackFollowRange = 7.0f;

		[SerializeField]
		private TargetPoint m_targetToFollow;
		public TargetPoint targetToFollow {
			get => m_targetToFollow;
			set
			{
				m_targetToFollow = value;
				UpdateDestination();
			}
		}

		private RosemaryUnit m_targetUnit = null;
		public RosemaryUnit targetUnit {
			get
			{
				if (m_targetUnit && !m_targetUnit.isActiveAndEnabled)
				{
					m_targetUnit = null;
					isAttacking = false;
				}
				return m_targetUnit;
			}
			private set
			{
				m_targetUnit = !value || !value.isActiveAndEnabled ? null : value;
			}
		}
		public bool isAttacking { get; private set; } = false;

		public Vector3 destination {
			get
			{
				if (!targetUnit)
				{
					return targetToFollow.position ?? transform.position;
				}
				else
				{
					var pos = targetUnit.centre;
					var vec = centre - pos;
					var maxRange = isAttacking ? attackFollowRange : followRange;
					if (vec.sqrMagnitude < maxRange * maxRange)
					{
						return transform.position;
					}
					else
					{
						return pos + vec.normalized * maxRange;
					}
				}
			}
		}

		public void UpdateDestination()
		{
			if (agent)
			{
				agent.destination = destination;
			}
		}

		private void OnEnableMovementPartial()
		{
			if (!agent)
			{
				agent = GetComponent<NavMeshAgent>();
			}

			if (spawn.spawner.spawnMode != SpawnMode.PreExisting)
			{
				targetToFollow = TargetPoint.None;
			}

			UpdateDestination();
			co_setDestination = StartCoroutine(Co_SetDestination());
		}

		private void OnDisableMovementPartial()
		{
			StopCoroutine(co_setDestination);
		}

		private Coroutine co_setDestination;
		private IEnumerator Co_SetDestination()
		{
			while (true)
			{
				yield return new WaitForSeconds(agentUpdatePeriod * UnityEngine.Random.Range(0.5f, 1.0f));
				UpdateDestination();
			}
		}

#if UNITY_EDITOR
		private void OnDrawGizmosSelectedMovementPartial()
		{
			switch (targetToFollow.mode)
			{
				default:
				case TargetPoint.Mode.None:
					break;
				case TargetPoint.Mode.Point:
					Gizmos.color = Color.blue;
					Gizmos.DrawLine(transform.position, destination);
					break;
				case TargetPoint.Mode.Transform:
					Gizmos.color = Color.green;
					Gizmos.DrawLine(transform.position, destination);
					break;
			}
		}
#endif

		public void CommandStop()
		{
			targetToFollow = TargetPoint.None;
			targetUnit = null;
			isAttacking = false;
		}

		public void CommandMove(Vector3 destination)
		{
			targetToFollow = TargetPoint.Point(destination);
			targetUnit = null;
			isAttacking = false;
		}

		public void CommandFollow(RosemaryUnit other)
		{
			targetToFollow = TargetPoint.Transform(other.centreTransform);
			targetUnit = other;
			isAttacking = false;
		}

		public void CommandAttack(RosemaryUnit other)
		{
			targetToFollow = TargetPoint.Transform(other.centreTransform);
			targetUnit = other;
			isAttacking = true;
			foreach (var aimer in GetComponentsInChildren<RosemaryAimer>())
			{
				aimer.target = targetUnit;
			}
		}
	}
}
