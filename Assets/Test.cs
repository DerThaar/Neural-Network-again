using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	NeuralNetwork network;
	Rigidbody rb;
	public GameObject[] Sensors;
	public LayerMask layer;
	public float Range;
	public float speed;
	public int NumberOfLayers;
	public int[] NumberOfNeurons;
	public float Bias;
	Vector3 corrector;
	Vector3 pos;

	void Start()
	{
		pos = transform.position;
		rb = GetComponent<Rigidbody>();
		network = new NeuralNetwork();
		network.NumberOfLayers = NumberOfLayers;
		network.NumberOfNeurons = NumberOfNeurons;
		network.Bias = Bias;
		network.BuildNetwork();
	}

	private void Update()
	{
		float[] inputs = GetSensorInput();
		for (int i = 0; i < Sensors.Length; i++)
		{
			network.Layers[0].Neurons[i].Value = inputs[i];
		}
		network.UpdateNetwork();
		CalcMovement();
	}

	float[] GetSensorInput()
	{
		float[] inputs = new float[Sensors.Length];

		for (int i = 0; i < Sensors.Length; i++)
		{
			RaycastHit hit;
			Ray ray = new Ray(transform.position, Sensors[i].transform.up);
			if (Physics.Raycast(ray, out hit, Range, layer))
			{
				inputs[i] = (transform.position - hit.point).magnitude;
			}
			else
			{
				inputs[i] = Range;
			}
		}

		return inputs;
	}

	void CalcMovement()
	{
		Vector3 velocity = Vector3.zero;
		velocity.x = network.Layers[network.Layers.Length - 1].Neurons[0].Value;
		velocity.y = network.Layers[network.Layers.Length - 1].Neurons[1].Value;

		print(network.Layers[network.Layers.Length - 1].Neurons[0].Value);
		print(network.Layers[network.Layers.Length - 1].Neurons[1].Value);

		rb.velocity = velocity * Time.deltaTime * speed;
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < Sensors.Length; i++)
		{
			Gizmos.DrawRay(Sensors[i].transform.position, Sensors[i].transform.up * Range);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		corrector = Vector3.Reflect(corrector, collision.contacts[0].normal);
		network.BackProp(rb.velocity, corrector);
		transform.position = pos;
	}
}
