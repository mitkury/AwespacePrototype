using UnityEngine;
using System.Collections;

namespace Awespace {

	public static class HComponent {

		public static T Create<T>() where T : Component {
			return HComponent.Create<T>("GameObject");
		}
		
		public static T Create<T>(string name) where T : Component {
			GameObject gameObject = new GameObject(name);
			
			// In case if creating a 'standard' component such as Transform.
			T component = gameObject.GetComponent<T>();
			
			// If creating not a 'standard' component then add a requared one to the gameObject.
			component = component != null ? component : gameObject.AddComponent<T>();
			
			return component;
		}
	}

}
