using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
	public Layer[] Layers;
	public int NumberOfLayers;
	public float Bias;
	public int[] NumberOfNeurons;


	public LayerMask layer;
	public GameObject[] Sensors;
	Rigidbody rb;
	Vector3 startPos;

	public class Layer
	{
		public Neuron[] Neurons;
	}

	public class Neuron
	{
		public float Value { get; set; }
		public float[] Weights { get; set; }
	}

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		startPos = transform.position;
		BuildNetwork();		
	}

	private void Update()
	{
		BuildNetwork();
		for (int i = 0; i < Layers[0].Neurons.Length; i++)
		{
			RaycastHit hitInfo;
			Ray ray = new Ray(transform.position, Sensors[i].transform.up);
			if(Physics.Raycast(ray, out hitInfo, 100f, layer))
			{
				Vector3 hitPoint = hitInfo.point;
				Layers[0].Neurons[i].Value = (hitPoint - transform.position).magnitude * 0.01f;
			}
			else
			{
				Layers[0].Neurons[i].Value = 1f;
			}
		}		

		rb.velocity = ((transform.right * Layers[Layers.Length - 1].Neurons[0].Value) +
							(transform.up * Layers[Layers.Length - 1].Neurons[1].Value));
									
		//PrintTest();
	}

	public void BuildNetwork()
	{
		if (NumberOfNeurons.Length != NumberOfLayers) { Debug.LogWarning("Neurons.Length and Number of Layers are not the same!"); }

		Layers = new Layer[NumberOfLayers];

		for (int i = 0; i < Layers.Length; i++)
		{
			Layers[i] = new Layer();
			Layers[i].Neurons = new Neuron[NumberOfNeurons[i]];

			for (int j = 0; j < Layers[i].Neurons.Length; j++)
			{
				Layers[i].Neurons[j] = new Neuron();
				if (Layers[i] == Layers[0])
				{
					Layers[i].Neurons[j].Weights = new float[0];
					Layers[i].Neurons[j].Value = 1f;
				}
				else
				{
					Layers[i].Neurons[j].Weights = new float[Layers[i - 1].Neurons.Length];

					float val = 0f;
					for (int k = 0; k < Layers[i].Neurons[j].Weights.Length; k++)
					{
						Layers[i].Neurons[j].Weights[k] = Layers[i - 1].Neurons[k].Value * UnityEngine.Random.Range(-1f, 1f);
						val += Layers[i].Neurons[j].Weights[k];
					}

					Layers[i].Neurons[j].Value = val - Bias;
				}
			}
		}
	}

	private void PrintTest()
	{
		int inputCount = 1;
		int outputCount = 1;
		for (int i = 0; i < Layers[0].Neurons.Length; i++)
		{
			print($"Input-Neuron {inputCount} = {Layers[0].Neurons[i].Value}");
			inputCount++;
		}
		for (int i = 0; i < Layers[Layers.Length - 1].Neurons.Length; i++)
		{
			print($"Output-Neuron {outputCount} = {Layers[Layers.Length - 1].Neurons[i].Value}");
			outputCount++;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		transform.position = startPos;
		print("test");
		for (int i = 1; i < Layers.Length; i++)
		{
			for (int j = 0; j < Layers[i].Neurons.Length; j++)
			{
				for (int k = 0; k < Layers[i].Neurons[j].Weights.Length; k++)
				{
					Layers[i].Neurons[j].Weights[k] *= UnityEngine.Random.Range(-2f, 2f);
				}
			}
		}
	}
}
