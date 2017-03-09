using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
	#region Public Properties

	// Add wherever transform it is needed here to share particles' position
	// You should put the transform on the mesh first, then attach it to this script

	public Transform particleBodyPos;
	public Transform particleHeadPos;
	public Transform particleFootPos;

	public Transform particleWeaponPos;

	#endregion

	#region Public C# Properties

	public Transform ParticleBodyPos
	{
		get
		{
			return (this.particleBodyPos) ? this.particleBodyPos : null;
		}
	}

	public Transform ParticleHeadPos
	{
		get
		{
			return (this.particleHeadPos) ? this.particleHeadPos : null;
		}
	}

	public Transform ParticleFootPos
	{
		get
		{
			return (this.particleFootPos) ? this.particleFootPos : null;
		}
	}

	public Transform ParticleWeaponPos
	{
		get
		{
			return (this.particleWeaponPos) ? this.particleWeaponPos : null;
		}
	}

	#endregion
}
