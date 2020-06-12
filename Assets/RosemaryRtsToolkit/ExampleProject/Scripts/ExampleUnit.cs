#if false
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cresspresso.Rosemary;
using System.Diagnostics.Tracing;

namespace Cresspresso
{
	namespace ExampleProject
	{
		public class ExampleUnit : MonoBehaviour//, IUnit, ISpawn
		{
			public Vector3 position => transform.position;

			private ITarget m_asTarget;
			public ITarget asTarget {
				get
				{
					if (m_asTarget == null)
					{
						m_asTarget = new TransformTarget(transform);
					}
					return m_asTarget;
				}
			}

			[SerializeField]
			private float m_speed;
			public float speed {
				get => m_speed;
				set => m_speed = value;
			}

			[SerializeField]
			private float m_maxHealth = 100.0f;
			public float maxHealth {
				get => m_maxHealth;
				set => m_maxHealth = value;
			}

			[SerializeField]
			private UnityEvent m_onSelected = new UnityEvent();
			public UnityEvent onSelected => m_onSelected;

			[SerializeField]
			private UnityEvent m_onDeselected = new UnityEvent();
			public UnityEvent onDeselected => m_onDeselected;

#pragma warning disable CS0649
			[SerializeField]
			private RosemaryTurretMotor m_rootMotor;
#pragma warning restore CS0649
			public RosemaryTurretMotor rootMotor => m_rootMotor;

#pragma warning disable CS0649
			[SerializeField]
			private Transform m_rootMotorIdleTargetTransform;
#pragma warning restore CS0649
			private ITarget m_rootMotorIdleTarget;
			public ITarget rootMotorIdleTarget {
				get
				{
					if (m_rootMotorIdleTarget == null)
					{
						m_rootMotorIdleTarget = new TransformTarget(m_rootMotorIdleTargetTransform);
					}
					return m_rootMotorIdleTarget;
				}
			}



			public ISpawner spawner { get; set; }
			public IFaction faction { get; set; }
			public float health { get; set; }

			public ITarget destinationTarget { get; private set; }
			public IUnit enemyUnit { get; private set; }



			public void OnSpawned(ISpawnEventArgs eventArgs)
			{
				transform.position = eventArgs.position;
				transform.rotation = eventArgs.rotation;
				health = maxHealth;
				destinationTarget = null;
				OnDeselected();
				gameObject.SetActive(true);
			}

			public void OnDespawned()
			{
				gameObject.SetActive(false);
			}

			private void Update()
			{
				var targetPosition = destinationTarget?.position;
				if (targetPosition.HasValue)
				{
					// rotate
					if (Vector3.Distance(transform.position, targetPosition.Value) < 0.001f)
					{
						rootMotor.target = rootMotorIdleTarget;
					}
					else
					{
						rootMotor.target = destinationTarget;
					}

					// move
					transform.position = Vector3.MoveTowards(
						transform.position,
						targetPosition.Value,
						speed * Time.fixedDeltaTime);
				}
			}

			public void OnSelected(ICommander commander)
			{
				m_onSelected.Invoke();
			}

			public void OnDeselected()
			{
				m_onDeselected.Invoke();
			}

			public void OnCommand(ICommander commander, ICommandEventArgs eventArgs)
			{
				if (eventArgs is IAttackCommandEventArgs)
				{
					var ourArgs = (IAttackCommandEventArgs)eventArgs;
					destinationTarget = ourArgs.unit.asTarget;
				}
				else if (eventArgs is IMoveCommandEventArgs)
				{
					var ourArgs = (IMoveCommandEventArgs)eventArgs;
					destinationTarget = new PointTarget(ourArgs.destination);
				}
				else
				{
					throw new System.InvalidOperationException("Unknown Command");
				}
			}

			public void Damage(IDamageEventArgs eventArgs)
			{
				var ourEventArgs = (ExampleDamageEventArgs)eventArgs;
				if (ourEventArgs.heal)
				{
					health = Mathf.Min(health + ourEventArgs.damage, maxHealth);
				}
				else
				{
					health = Mathf.Max(health - ourEventArgs.damage, 0);
					if (health == 0)
					{
						Destroy(gameObject);
					}
				}
			}
		}
	}
}
#endif