using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsGameManager : MonoBehaviour
{
	List<Thrower> _throwers;
	//List<Target> _targets;

	public void AddThrower(Thrower thrower) => _throwers.Add(thrower);
	public void RemoveThrower(Thrower thrower) => _throwers.Add(thrower);

	//public void AddTarget(Target target) => _targets.Add(target);
	//public void RemoveTarget(Target target) => _targets.Add(target);

}