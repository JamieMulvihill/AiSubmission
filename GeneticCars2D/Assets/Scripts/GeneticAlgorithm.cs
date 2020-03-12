using System;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAglorithm<T>
{

    public List<DNA<T>> Population { get; private set; }
    public int Generation { get; private set; }
    public float BestFitness { get; private set; }
    public T[] BestGenes { get; private set; }

    public List<DNA<T>> aboveAverageGenes;

    DNA<T> best;
    int parentChoice = 0;
    int bestDNAIndex = 0;
    int completedCrossovers = 0;
    public float MutationRate;
    float averageDNAFitness = 0;
    private System.Random random;
    private float fitnessSum;
    public int selectionProcess = 0;
    int id;

    public GeneticAglorithm(int ID, int populationSize, int dnaSize, int selectionChoice, System.Random random, Func<float, T> getRandomGene, Func<int, T> getInitGene, Func<int, float> fitnessFunction, float mutationRate = 0.01f)
    {
        Generation = 1;
        MutationRate = mutationRate;
        selectionProcess = selectionChoice;
        Population = new List<DNA<T>>();
        aboveAverageGenes = new List<DNA<T>>();
        this.random = random;
        id = ID;
        BestGenes = new T[dnaSize];

            
        for (int i = 0; i < 20; i++)
        {
            Population.Add(new DNA<T>(i, dnaSize, random, getRandomGene, getInitGene, fitnessFunction, shouldInitGenes: true));
        }
    }

    public void NewGeneration(int process)
    {
        
        if (Population.Count <= 0)
        {
            return;
        }

        CalculateFitness();

        List<DNA<T>> newPopulation = new List<DNA<T>>();

        if (process == 0)
        {
            Debug.Log("RandomParent");
            for (int i = 0; i < 20; i++)
            {
                
                DNA<T> parent1 = RouletteWheelSlection();
                DNA<T> parent2 = RouletteWheelSlection();

                DNA < T> child = parent1.Crossover(parent2);

                child.Mutate(MutationRate);

                newPopulation.Add(child);
            }
        }

        //}
        else if (process == 1)
        {
            Debug.Log("MedianParent");
            Population.Sort((DNA<T> a, DNA<T> b) =>
            {
                return a.Fitness > b.Fitness ? -1 : 1;
            });

            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    DNA<T> child = HigherThanMedianSelection(i);
                    child.Mutate(MutationRate);

                    newPopulation.Add(child);
                }
            }
        }

        else if (process == 2)
        {
            Debug.Log("BestParent");
            Population.Sort((DNA<T> a, DNA<T> b) =>
            {
                return a.Fitness > b.Fitness ? -1 : 1;
            });
            List<DNA<T>> betterThenMedian = new List<DNA<T>>(Population);
            betterThenMedian.RemoveRange(Population.Count / 2, Population.Count / 2);

            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    DNA<T> parent1 = best;
                    //DNA<T> parent2 = FittestParentCrossoverSelection(i);
                    DNA<T> parent2 = betterThenMedian[i];

                    DNA<T> child = parent1.Crossover(parent2);

                    child.Mutate(MutationRate);

                    newPopulation.Add(child);
                }
            }   
        }

        else if (process == 3){

            Debug.Log("SinglePoint");
            Population.Sort((DNA<T> a, DNA<T> b) =>
            {
                return a.Fitness > b.Fitness ? -1 : 1;
            });
            List<DNA<T>> betterThenMedian = new List<DNA<T>>(Population);
            betterThenMedian.RemoveRange(Population.Count / 2, Population.Count / 2);
            int corssoverPoint = UnityEngine.Random.Range(1, 4);
            //for (int j = 0; j < 2; j++)
            //{
                for (int i = 0; i < 10; i++)
                {
                    DNA<T> parent1 = best;
                    DNA<T> parent2 = betterThenMedian[i];

                    DNA<T>[] children = new DNA<T>[2];
                    children = parent1.SinglePointCrossover(parent2, corssoverPoint);


                    for (int k = 0; k < 2; k++)
                    {
                        children[k].Mutate(MutationRate);

                        newPopulation.Add(children[k]);
                    }
                }
        }
        //}*/
        else if (process == 4)
        {

            Debug.Log("MultiPoint");
            Population.Sort((DNA<T> a, DNA<T> b) =>
            {
                return a.Fitness > b.Fitness ? -1 : 1;
            });
            List<DNA<T>> betterThenMedian = new List<DNA<T>>(Population);
            betterThenMedian.RemoveRange(Population.Count / 2, Population.Count / 2);
            int corssoverPoint = UnityEngine.Random.Range(1, 5);

          
            for (int i = 0; i < 10; i++)
            {

                DNA<T> parent1 = best;
                DNA<T> parent2 = betterThenMedian[i];

                DNA<T>[] children = new DNA<T>[2];
                children = parent1.MultiplePointCrossover(parent2);


                for (int k = 0; k < 2; k++)
                {
                    children[k].Mutate(MutationRate);
                    newPopulation.Add(children[k]);
                }
            }
        }

        Population = newPopulation;

        Generation++;
        aboveAverageGenes.Clear();
    }

    public void CalculateFitness()
    {
        fitnessSum = 0;

        //initially set the best DNA
        best = Population[0];

        // loop through the population
        // check the current DNA fitness is greater than current best
        // if so set best to current DNA
        for (int i = 0; i < Population.Count; i++)
        {
            fitnessSum += Population[i].CalculateFitness(i, id); // 

            if (Population[i].Fitness > best.Fitness)
            {
                best = Population[i];
                bestDNAIndex = i;
            }
        }

        BestFitness = best.Fitness;
        best.Genes.CopyTo(BestGenes, 0);
    }

    private DNA<T> ChooseParent(int otherParent)
    {
        double averageFitness = fitnessSum / Population.Count;
        for (int i = 0; i < Population.Count; i++)
        {
            if (i != otherParent)
            {
                if (averageFitness < Population[i].Fitness)
                {
                    return Population[i];
                } 
            }
            parentChoice++;
        }

        return best;
    }
    private DNA<T> FittestParentCrossoverSelection(int genesIndex){

        DNA<T> parent = Population[genesIndex];
        return parent;
    }

    private DNA<T> HigherThanMedianSelection(int crossOvers){

        List<DNA<T>> betterThenMedian = new List<DNA<T>>(Population);
        betterThenMedian.RemoveRange(Population.Count / 2, Population.Count / 2);
        
         int otherParent = UnityEngine.Random.Range(0, betterThenMedian.Count);

         while (otherParent == crossOvers) {
             otherParent = UnityEngine.Random.Range(0, betterThenMedian.Count);
         }

        return betterThenMedian[crossOvers].Crossover(betterThenMedian[otherParent]);  
    }
    private DNA<T> RouletteWheelSlection() {
        float wheelSlice = UnityEngine.Random.Range(0, fitnessSum);
        float currentTotal = 0f;
        int selectedParent = 0;

        for (int i = 0; i < Population.Count; ++i) {
            currentTotal += Population[i].Fitness;
            if (currentTotal > wheelSlice) {
                selectedParent = i;
                break;
            }
        }
        return Population[selectedParent];
    }

    private DNA<T> ChooseRandomParent()
    {
        double randomNumber = UnityEngine.Random.value * fitnessSum;

        for (int i = 0; i < Population.Count; i++)
        {
            if (randomNumber <= Population[i].Fitness)
            {
                return Population[i];
            }

            randomNumber -= Population[i].Fitness;
        }

        return Population[random.Next(0, Population.Count)];
    }

}
