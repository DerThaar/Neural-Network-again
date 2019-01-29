using System.Collections.Generic;

public class Pulse
{
	public double Value { get; set; }
}

public class Dendrite
{
	public Pulse InputPulse { get; set; }
	public double SynapticWeight { get; set; }
	public bool Learnable { get; set; } = true;
}

public class Neuron
{
	public List<Dendrite> Dendrites { get; set; }
	public Pulse OutputPulse { get; set; }
	private double weight;

	public Neuron()
	{
		Dendrites = new List<Dendrite>();
		OutputPulse = new Pulse();
	}

	public void Fire()
	{
		OutputPulse.Value = Sum();
		OutputPulse.Value = Activation(OutputPulse.Value);
	}

	public void Compute(double leaningRate, double delta)
	{
		weight += leaningRate * delta;
		foreach (var terminal in Dendrites)
		{
			terminal.SynapticWeight = weight;
		}
	}

	private double Sum()
	{
		double computeValue = 0.0f;
		foreach (var d in Dendrites)
		{
			computeValue += d.InputPulse.Value * d.SynapticWeight;
		}
		return computeValue;
	}

	private double Activation(double input)
	{
		double threshhold = 1;
		return input >= threshhold ? 0 : threshhold;
	}
}