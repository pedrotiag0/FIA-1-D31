using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class TournamentSelection : SelectionMethod
{
	private int tournamentSize;

	public TournamentSelection(int tournamentSize) : base()
	{
		this.tournamentSize = tournamentSize;
	}

	public override List<Individual> selectIndividuals(List<Individual> oldpop, int num)
	{
		if (oldpop.Count < tournamentSize)
		{
			throw new System.Exception("The population size is smaller than the tournament size.");
		}

		List<Individual> selectedInds = new List<Individual>();
		for (int i = 0; i < num; i++)
		{
			selectedInds.Add(tournamentSelection(oldpop,tournamentSize).Clone());
		}

		return selectedInds;
	}

	public Individual tournamentSelection(List<Individual> population, int tournamentSize)
	{
		//* Algoritmo 2 - permite selecionar o melhor individuo num torneio de tournamentSize individuos. *//
		Individual best = null;
		int i = 0;
		Individual ind;
		for(i = 0; i < tournamentSize; i++)
        {
			ind = population[Random.Range(0, population.Count - 1)];
            /* Maximização */
            if ((best == null) || (ind.Fitness > best.Fitness))
            {
				best = ind.Clone();
            }
        }
		return best;
		throw new System.NotImplementedException();
	}
}
