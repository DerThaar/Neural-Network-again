using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer
{
	public List<Neuron> Neurons { get; set; }
	public string Name { get; set; }
	public double Weight { get; set; }

	public Layer(int count, double initialWeight, string name = "")
	{
		Neurons = new List<Neuron>();
		for (int i = 0; i < count; i++)
		{
			Neurons.Add(new Neuron());
		}
		Weight = initialWeight;
		Name = name;
	}

	public void Compute(double learningRate, double delta)
	{
		foreach (var neuron in Neurons)
		{
			neuron.Compute(learningRate, delta);
		}
	}

	public void Log()
	{
		Debug.Log($"{0}, Weight: {1}");
	}
}
