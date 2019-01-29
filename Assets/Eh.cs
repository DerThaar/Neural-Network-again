using System.Collections.Generic;
using UnityEngine;

public class singleNeuron
{

	public int numInputs;
	public List<double> vecWeights = new List<double>();

	public singleNeuron(int f_NumInputs)
	{
		for (int i = 0; i < f_NumInputs + 1; i++)
		{
			vecWeights.Add(Random.Range(-1f, 1f));
		}
		numInputs = f_NumInputs;
	}

}
public class NeuronLayer
{

	public int NumNeurons;
	public List<singleNeuron> vecNeurons = new List<singleNeuron>();

	public NeuronLayer(int f_NumNeurons, int numInputsPerNeuron)
	{

		for (int i = 0; i < f_NumNeurons; i++)
		{
			vecNeurons.Add(new singleNeuron(numInputsPerNeuron));
		}
		NumNeurons = f_NumNeurons;
	}

}
public class NeuralNet
{
	int numInputs;
	int numOutputs;
	int numHiddenLayers;
	int neuronsPerHiddenLayer;
	List<NeuronLayer> vecLayers = new List<NeuronLayer>();
	bool sigmoidtr;
	public NeuralNet(bool sig, int numInputSet, int numOutputsSet, int numHiddenLayersSet, int neuronsPerHiddenLayerSet)
	{
		sigmoidtr = sig;
		numInputs = numInputSet;
		numOutputs = numOutputsSet;
		numHiddenLayers = numHiddenLayersSet;
		neuronsPerHiddenLayer = neuronsPerHiddenLayerSet;
		CreateNet();
	}

	public void CreateNet()
	{

		if (numHiddenLayers > 0)
		{

			vecLayers.Add(new NeuronLayer(neuronsPerHiddenLayer, numInputs));

			for (int i = 0; i < numHiddenLayers - 1; i++)
			{
				vecLayers.Add(new NeuronLayer(neuronsPerHiddenLayer, neuronsPerHiddenLayer));
			}

			vecLayers.Add(new NeuronLayer(numOutputs, neuronsPerHiddenLayer));
		}
		else
		{
			vecLayers.Add(new NeuronLayer(numOutputs, numInputs));
		}

	}
	public List<double> GetWeights()
	{

		List<double> weights = new List<double>();
		for (int i = 0; i < numHiddenLayers + 1; i++)
		{

			for (int j = 0; j < vecLayers[i].NumNeurons; j++)
			{

				for (int k = 0; k < vecLayers[i].vecNeurons[j].numInputs; ++k)
				{

					weights.Add(vecLayers[i].vecNeurons[j].vecWeights[k]);

				}
			}

		}
		return weights;

	}
	public void PutWeights(List<double> weights)
	{

		int cWeight = 0;
		for (int i = 0; i < numHiddenLayers + 1; i++)
		{

			for (int j = 0; j < vecLayers[i].NumNeurons; j++)
			{

				for (int k = 0; k < vecLayers[i].vecNeurons[j].numInputs; ++k)
				{

					vecLayers[i].vecNeurons[j].vecWeights[k] = weights[cWeight++];

				}
			}

		}
		return;

	}
	public int GetNumberofWeights()
	{

		int weights = 0;
		for (int i = 0; i < numHiddenLayers + 1; i++)
		{

			for (int j = 0; j < vecLayers[i].NumNeurons; j++)
			{

				for (int k = 0; k < vecLayers[i].vecNeurons[j].numInputs; k++)
				{

					weights++;

				}
			}

		}
		return weights;

	}
	public double Sigmoid(double netinput, double response)
	{

		return (1 / (1 + System.Math.Exp((double)(-netinput / (double)response))));

	}
	public List<double> cycle(List<double> inputs)
	{
		List<double> outputs = new List<double>();
		int cWeight = 0;

		for (int i = 0; i < numHiddenLayers + 1; i++)
		{

			if (i > 0)
			{
				inputs.Clear();
				for (int y = 0; y < outputs.Count; y++)
				{

					inputs.Add(outputs[y]);

				}

			}
			outputs.Clear();
			cWeight = 0;

			for (int j = 0; j < vecLayers[i].NumNeurons; j++)
			{

				double netinputs = 0;
				int f_NumInputs = 1 + vecLayers[i].vecNeurons[j].numInputs;
				cWeight = 0;

				for (int k = 0; k < f_NumInputs - 1; k++)
				{

					netinputs += vecLayers[i].vecNeurons[j].vecWeights[k] * inputs[cWeight++];
				}
				netinputs += vecLayers[i].vecNeurons[j].vecWeights[f_NumInputs - 1] * (-1);
				if (sigmoidtr == true)
				{
					outputs.Add(Sigmoid(netinputs, -1));
				}
				else
				{
					if (netinputs > 0)
					{
						outputs.Add(1);
					}
					else outputs.Add(0);
				}
			}
		}

		return outputs;
	}
}