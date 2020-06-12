using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	public class DebugSelectionCount : MonoBehaviour
	{
		private void Update()
		{
			//Debug.Log(string.Join(", ", RosemarySelection.instance.units.Select(u => u.name)), this);
		}
	}
}
