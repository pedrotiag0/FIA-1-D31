using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MetaHeuristic;
using Random = UnityEngine.Random;

public class GeneticIndividual : Individual {


	public GeneticIndividual(int[] topology, int numberOfEvaluations, MutationType mutation) : base(topology, numberOfEvaluations, mutation) {
	}

	public override void Initialize () 
	{
		for (int i = 0; i < totalSize; i++)
		{
			genotype[i] = Random.Range(-1.0f, 1.0f);
		}
	}

   public override void Initialize(NeuralNetwork nn)
    {
        int count = 0;
        for (int i = 0; i < topology.Length - 1; i++)
        {
            for (int j = 0; j < topology[i]; j++)
            {
                for (int k = 0; k < topology[i + 1]; k++)
                {
                    genotype[count++] = nn.weights[i][j][k];
                }

            }
        }
    }

    public override Individual Clone()
    {
        GeneticIndividual new_ind = new GeneticIndividual(this.topology, this.maxNumberOfEvaluations, this.mutation);

        genotype.CopyTo(new_ind.genotype, 0);
        new_ind.fitness = this.Fitness;
        new_ind.evaluated = false;

        return new_ind;
    }


    public override void Mutate(float probability)
    {
        switch (mutation)
        {
            case MetaHeuristic.MutationType.Gaussian:
                MutateGaussian(probability);
                break;
            case MetaHeuristic.MutationType.Random:
                MutateRandom(probability);
                break;
        }
    }
    public void MutateRandom(float probability)
    {
        for (int i = 0; i < totalSize; i++)
        {
            if (Random.Range(0.0f, 1.0f) < probability)
            {
                genotype[i] = Random.Range(-1.0f, 1.0f);
            }
        }
    }

    
    public void MutateGaussian(float probability)
    {
        /* Algoritmo 1 */
        float Mean = 0.0f;
        float Stdev = 0.5f;
        int i = 0;
        float aux;
        //Random r = new Random();
        for (i = 0; i < genotype.Length; i++)
        {
            aux = 2.0f * Random.Range(0f, 1f) - 1.0f;
            if (aux < probability )
            {
                genotype[i] = genotype[i] + NextGaussian(Mean, Stdev);
            }
        }
        throw new System.NotImplementedException();
    }

    public override void Crossover(Individual partner, float probability)
    {
        /* YOUR CODE HERE! */
        /* Nota: O crossover deverá alterar ambos os indivíduos */
        float aux = 2.0f * Random.Range(0f, 1f) - 1.0f;
        int crossoverPoint = Random.Range(0, genotype.Length);
        if (aux < probability)
        {
            float aux2;
            for(int i = crossoverPoint; i < genotype.Length; i++)
            {

                aux2 = partner.getGenotype(i);
                partner.setGenotype(i, genotype[i]);
                genotype[i] = aux2;
            }
        }
        
        throw new System.NotImplementedException();
    }


}
