﻿
namespace UnityEngine.Rendering.Toon.Universal.Samples
{
	public class Rotator : MonoBehaviour
	{

		private Vector3 pivot = Vector3.zero;
		public float speed = 20.0f;

		// Use this for initialization
		void Awake()
		{
			pivot = transform.position;

			if (Random.value > 0.5)
            {
				speed = -speed;
            }
		}

		// Update is called once per frame
		void Update()
		{
			transform.RotateAround(pivot, Vector3.up, speed * Time.deltaTime);
		}
	}
}

