using System;
using UnityEngine;
using System.Collections.Generic;

namespace SimpleManager
{
	/////////////////////////////////////////////////
	// BASE CLASSES
	/////////////////////////////////////////////////
	public interface IManaged{
		void OnCreated();
		void OnDestroyed();
	}

	public abstract class Manager<T> : MonoBehaviour
	{
		protected readonly List<T> ManagedObjects = new List<T>();
		protected readonly List<T> ObjectsAwaitingDestruction = new List<T>();

		public abstract T Create();

		public abstract void Destroy(T o);

		public T Find(Predicate<T> predicate)
		{
			return ManagedObjects.Find(predicate);
		}

		public List<T> FindAll(Predicate<T> predicate)
		{
			return ManagedObjects.FindAll(predicate);
		}

		public void ClearDestroyedObjects(){
			
			foreach (T t in ObjectsAwaitingDestruction) {
				ManagedObjects.Remove (t);
			}

			ObjectsAwaitingDestruction.Clear ();
		}
	}
}