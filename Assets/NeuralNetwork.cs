using UnityEngine;

public class NeuralNetwork
{
	public Layer[] Layers;
	public int NumberOfLayers;
	public float Bias;
	public int[] NumberOfNeurons;

	public class Layer
	{
		public Neuron[] Neurons;
	}

	public class Neuron
	{
		public float Value { get; set; }
		public float[] Weights { get; set; }
		public float[] WeightValues { get; set; }
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
					Layers[i].Neurons[j].WeightValues = new float[Layers[i].Neurons[j].Weights.Length];

					float val = 0f;
					for (int k = 0; k < Layers[i].Neurons[j].Weights.Length; k++)
					{
						Layers[i].Neurons[j].WeightValues[k] = Random.Range(-10f, 10f);
						Layers[i].Neurons[j].Weights[k] = Layers[i - 1].Neurons[k].Value * Layers[i].Neurons[j].WeightValues[k];
						val += Layers[i].Neurons[j].Weights[k];
					}

					Layers[i].Neurons[j].Value = Mathf.Max(0f, val - Bias);
				}
			}
		}
	}

	public void UpdateNetwork()
	{
		for (int i = 0; i < Layers.Length; i++)
		{
			for (int j = 0; j < Layers[i].Neurons.Length; j++)
			{
				if (Layers[i] == Layers[0])
				{
					continue;
				}
				else
				{
					float val = 0f;
					for (int k = 0; k < Layers[i].Neurons[j].Weights.Length; k++)
					{
						Layers[i].Neurons[j].Weights[k] = Layers[i - 1].Neurons[k].Value * Layers[i].Neurons[j].WeightValues[k];
						val += Layers[i].Neurons[j].Weights[k];
					}

					Layers[i].Neurons[j].Value = Mathf.Max(0f, val - Bias);
				}
			}
		}
	}

	public void BackProp(Vector3 velocity, Vector3 corrector)
	{
		float x = velocity.x - corrector.x;
		float y = velocity.y - corrector.y;

		float xCorr = Mathf.Pow((Layers[Layers.Length - 1].Neurons[0].Value - x), 2f);
		float yCorr = Mathf.Pow((Layers[Layers.Length - 1].Neurons[1].Value - y), 2f);

		float cost = xCorr + yCorr;

		for (int i = Layers.Length - 1; i > 0; i--)
		{
			if (i == Layers.Length - 1)
			{
				for (int j = 0; j < Layers[i].Neurons.Length; j++)
				{
					for (int k = 0; k < Layers[i].Neurons[j].WeightValues.Length; k++)
					{
						Layers[i].Neurons[j].WeightValues[k] += cost;
					}
				}
			}
			else
			{
				float hiddenCost = 0f;
				for (int j = 0; j < Layers[i + 1].Neurons.Length; j++)
				{
					hiddenCost += Layers[i].Neurons[j].Value;
				}

				for (int j = 0; j < Layers[i].Neurons.Length; j++)
				{
					for (int k = 0; k < Layers[i].Neurons[j].WeightValues.Length; k++)
					{
						Layers[i].Neurons[j].WeightValues[k] += cost;
					}
				}
			}
		}
	}
}
