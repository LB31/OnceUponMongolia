using UnityEngine;

namespace Helper.Singletons
{
	public sealed class Singleton<T> : MonoBehaviour	where T : Component
	{
		private static T instance;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<T> ();
					if (instance == null)
					{
						GameObject obj = new GameObject ();
						instance = obj.AddComponent<T> ();
					}
				}
				return instance;
			}
		}

		private void Awake ()
		{
			if (!Application.isPlaying)
			{
				return;
			}

			instance = this as T;			
		}
	}
}
