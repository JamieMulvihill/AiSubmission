using System;


public class DNA<T>
{
    public T[] Genes { get; private set; }
    public float Fitness { get; private set; }

    private Random random;
    private Func<float, T> getRandomGene;
    private Func<int, T> getInitGene;
    private Func<int, float> fitnessFunction;
    public int generation = 0;
    int index_;
    public float maxMutationProability = 5;
    public DNA(int index, int size, Random random, Func<float, T> getRandomGene, Func<int, T> getInitGene, Func<int,float> fitnessFunction, bool shouldInitGenes = true)
    {
        Genes = new T[size];
        this.random = random;
        this.getRandomGene = getRandomGene;
        this.getInitGene = getInitGene;
        this.fitnessFunction = fitnessFunction;

        index_ = index;
        if (shouldInitGenes)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                Genes[i] = getInitGene(index_);
            }
        }
    }

    public float CalculateFitness(int index, int selectionProcess)
    {
        Fitness = fitnessFunction(index);

        return Fitness;
    }

    public DNA<T> Crossover(DNA<T> otherParent)
    {
        DNA<T> child = new DNA<T>(index_, Genes.Length, random, getRandomGene, getInitGene, fitnessFunction, shouldInitGenes: false);

        for (int i = 0; i < Genes.Length; i++)
        {
            child.Genes[i] = UnityEngine.Random.Range(0f,0.1f) < 0.05 ? Genes[i] : otherParent.Genes[i];
        }

        return child;
    }

    public DNA<T>[] SinglePointCrossover(DNA<T> otherParent, int pointOfCrossover)
    {
        DNA<T>[] children = new DNA<T>[2];

        DNA<T> childA = new DNA<T>(index_, Genes.Length, random, getRandomGene, getInitGene, fitnessFunction, shouldInitGenes: false);
        DNA<T> childB = new DNA<T>(index_, Genes.Length, random, getRandomGene, getInitGene, fitnessFunction, shouldInitGenes: false);

        childA.Genes = Genes;
        childB.Genes = otherParent.Genes;

        for (int i = 0; i < Genes.Length; i++) {
            if (i >= pointOfCrossover) {
                childA.Genes[i] = otherParent.Genes[i];
                childB.Genes[i] = Genes[i];
            }
        }

        children[0] = childA;
        children[1] = childB;

        return children;
    }

    public DNA<T>[] MultiplePointCrossover(DNA<T> otherParent)
    {
        DNA<T>[] children = new DNA<T>[2];

        DNA<T> childA = new DNA<T>(index_, Genes.Length, random, getRandomGene, getInitGene, fitnessFunction, shouldInitGenes: false);
        DNA<T> childB = new DNA<T>(index_, Genes.Length, random, getRandomGene, getInitGene, fitnessFunction, shouldInitGenes: false);

        childA.Genes = Genes;
        childB.Genes = otherParent.Genes;
        int endCrossOverPoint = UnityEngine.Random.Range(1, Genes.Length);
        int startCrossOverPoint = UnityEngine.Random.Range(0, endCrossOverPoint);

        for (int i = 0; i < Genes.Length; i++)
        {
            if (i >= startCrossOverPoint && i <= endCrossOverPoint)
            {
                childA.Genes[i] = otherParent.Genes[i];
                childB.Genes[i] = Genes[i];
            }
        }

        children[0] = childA;
        children[1] = childB;

        return children;
    }

    public void Mutate(float mutationRate)
    {
        float[] newGenes = new float[6];
        Genes.CopyTo(newGenes, 0);

        for (int i = 0; i < Genes.Length; i++)
        {
            if (UnityEngine.Random.value < mutationRate)
            {

                Genes[i] = getRandomGene(i);

            }
        }
    }
}