using System;
using System.Collections.Generic;
using System.Collections;

namespace ComponentArchitecture
{
	public class Entity
	{
		private List<Component> m_components = new List<Component>();
		private BitArray m_componentBitset = new BitArray(Constants.MAX_COMPONENTS);
		private bool m_isActive = true;

		public UInt32 ID;
		public bool IsActive
		{
			get => m_isActive;
		}

		public void Update()
		{
			foreach (Component c in m_components)
			{
				c.Update();
			}
		}

		public void Draw()
		{
			foreach (Component c in m_components)
			{
				c.Draw();
			}
		}

		public void Destroy()
		{
			m_isActive = false;
		}

		public bool HasComponent<T>() where T : Component
		{
			return m_componentBitset[ComponentTools.GetComponentTypeID<T>()];
		}

		public T AddComponent<T>() where T : Component, new()
		{
			if (HasComponent<T>())
				return null;

			T component = new T() { Owner = this };

			m_componentBitset[ComponentTools.GetComponentTypeID<T>()] = true;
			m_components.Add(component);

			component.Init();
			return component;
		}

		public Component GetComponent<T>() where T : Component
		{
			return m_components.Find(c => c.GetType() == typeof(T));
		}
	}
}
