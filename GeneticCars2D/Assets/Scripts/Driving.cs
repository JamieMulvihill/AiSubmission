using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Driving : MonoBehaviour
{
    [Header("Text Labels")]
    [SerializeField] Text generationText;
    [SerializeField] Text medianFitnessText;
    [SerializeField] Text currentFitnessText;
    [SerializeField] Text selectionProcessText;
    [SerializeField] Text liveCarText;
    [SerializeField] Text mutationText;
    [Header("Button Text")]
    [SerializeField] Text buttonText;

    private Follow follow;
    public Spawn spawner;
    private GeneticAglorithm<float> geneticAlgorithm;
    public List<float> bestPrerformanceList;
    public List<DNA<float>> orderedCars;
    public float mutationRate = 0.01f;
    int selectionProcessCompleted = 0;
    System.Random random;
    int dnaSize = 6;
    float longestRun;
    bool running = false;
    int initVariableIndex = 0;
    public float timeScale = 3f;
    public int selectionProcess = 2;
    int liveCars;
    float medianFitness;
    // Start is called before the first frame update
    void Start()
    {
       
        geneticAlgorithm = new GeneticAglorithm<float>(1, 20, dnaSize, selectionProcess, random, GetRandomGene, GetInitGene, FitnessFunction, mutationRate: mutationRate);

        bestPrerformanceList = new List<float>();
        

        orderedCars = new List<DNA<float>>();

        generationText.text = geneticAlgorithm.Generation.ToString();
        medianFitnessText.text = geneticAlgorithm.BestFitness.ToString("F2");
        currentFitnessText.text = Camera.main.transform.position.x.ToString("F2");
        selectionProcessText.text = "Random Parent Selection";
        mutationText.text = mutationRate.ToString("F2");
        liveCars = 20 - spawner.deadCars;
        liveCarText.text = liveCars.ToString() + "/20";
    }

    void Update()
    {
        if (running)
        {
            Time.timeScale = timeScale;
            {//if (running)
             //{
             //    // If the agents are currently jumping, stop the script if the ALL enter the DeadZone
             //    if (agentManager.AreAgentsJumping())
             //    {
             //        if (agentManager.AllAgentsTouchedDeadZone())
             //        {
             //            this.enabled = false;
             //        }
             //    }
             //    // Make the agents jump if they are not jumping
             //    else
             //    {
             //        agentManager.UpdateAgentJumpingStrength(ga.Population);
             //        ga.NewGeneration();
             //        bestJump = ga.BestFitness;
             //        agentManager.MakeAgentsJump();
             //    }
             //}
            }
            {
                //for (int i = 0; i < spawner[0].cars.Length; i++)
                //{
                //    if (spawner[0].cars[i].transform.position.x > spawner[0].bestRun.x)
                //    {

                //        spawner[0].bestRun = spawner[0].cars[i].transform.position;
                //        follow.target = spawner[0].cars[i].transform;
                //    }
                //}
            }

            EndOfGeneration();
            if (spawner.bestCar)
            {
                if (spawner.bestCar.transform.position.x > longestRun)
                {
                    longestRun = spawner.bestCar.transform.position.x;
                    follow.target = spawner.bestCar.transform;
                }
                generationText.text = geneticAlgorithm.Generation.ToString();
                medianFitnessText.text = medianFitness.ToString();
                currentFitnessText.text = spawner.bestCar.transform.position.x.ToString("F2");
                mutationText.text = mutationRate.ToString("F2");
                liveCars = 20 - spawner.deadCars;
                liveCarText.text = liveCars.ToString() + "/20";
                ChooseSelectionProcess((float)selectionProcess);
            }
        }

        {
            //if (spawner[0].deadCars == spawner[0].cars.Length) {
            //    bestPrerformanceList.Add(spawner[0].bestRun.x);
            //    geneticAlgorithm.CalculateFitness();
            //    orderedCars = geneticAlgorithm.Population;
            //    orderedCars.Sort((DNA<float> a, DNA<float> b) => {
            //        return a.Fitness > b.Fitness ? -1 : 1;
            //    });

            //    float medianFitness = (orderedCars[(orderedCars.Count - 1) / 2].Fitness + orderedCars[(orderedCars.Count + 1) / 2].Fitness)/2; 

            //    WriteToFile(bestPrerformanceList.Count.ToString(), medianFitness.ToString(), "CarPerformance.csv");
            //    geneticAlgorithm.NewGeneration();
            //    spawner[0].deadCars = 0;
            //    NextGeneration();
            //    spawner[0].bestRun = new Vector2(0, 0);
            //}
        }
    }
    private float GetRandomGene(float gene)
    {
        int i = (int)gene;
        gene = spawner.cars[i].GetComponent<CarConstructor>().GeneRandomizer(gene);

        //second form of mutation by multiplying the genes but keeping them near to the passed on value
        //gene *= Random.Range(.75f, 1.25f);

        return gene;

    }
    private float GetInitGene(int index)
    {
        float[] initGenes = spawner.cars[index].GetComponent<CarConstructor>().genes;
        int returnValue = initVariableIndex;
        initVariableIndex++;
        if (initVariableIndex > 5) {
            initVariableIndex = 0;
        }
        return (initGenes[returnValue]);
    }

    private float FitnessFunction(int index) {
        float score = 0;

        score = spawner.cars[index].transform.position.x;

        return score;
    }

    private void NextGeneration() {
        for (int i = 0; i < spawner.cars.Length; i++) {
            spawner.cars[i].GetComponent<CarConstructor>().genes = geneticAlgorithm.Population[i].Genes;
            spawner.cars[i].GetComponent<CarConstructor>().Construct();
            spawner.cars[i].transform.position = new Vector3(spawner.transform.position.x, spawner.transform.position.y, 0);
            spawner.cars[i].GetComponent<CarController>().deathCheckTime = 0;
            spawner.cars[i].gameObject.SetActive(true);
            follow.target = spawner.cars[i].transform;

        }
    }
    public static void WriteToFile(string ID, string resultA, string resultB, string resultC, string filePath) {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true)) {
            file.WriteLine(ID + "," + resultA + "," + resultB + "," + resultC);
        }
    }
    private void EndOfGeneration()
    {

        if (spawner.deadCars == spawner.cars.Length) {

            bestPrerformanceList.Add(spawner.bestRun.x);
            geneticAlgorithm.CalculateFitness();
            orderedCars = geneticAlgorithm.Population;
            orderedCars.Sort((DNA<float> a, DNA<float> b) =>
            {
                return a.Fitness > b.Fitness ? -1 : 1;
            });

            medianFitness = (orderedCars[(orderedCars.Count - 1) / 2].Fitness + orderedCars[(orderedCars.Count + 1) / 2].Fitness) / 2;
            Debug.Log(medianFitness);
            geneticAlgorithm.NewGeneration(selectionProcess);
            spawner.deadCars = 0;
            NextGeneration();
            spawner.bestRun = new Vector2(0, 0);
            longestRun = 0;
            string temp, tempB;
            temp = "0";
            tempB = "0";
            WriteToFile(bestPrerformanceList.Count.ToString(), medianFitness.ToString(), temp, tempB, "CarPerformance.csv");
        }

    }
    public void onButtonClick()
    {
        // Pause and Start the algorithm
        if (running)
        {
            running = false;
            /* Do Stuff here to stop current run*/
            for (int i = 0; i < spawner.cars.Length; i++) {
                spawner.isRunning = false;
                GameObject deadCar = spawner.cars[i];
                Destroy(deadCar);//.SetActive(false);
                spawner.cars[i] = null;
                spawner.bestCar = null;
                follow.target = spawner.transform;
                longestRun = 0;
            }
            if (buttonText)
            {
                buttonText.text = "Start";
            }
        }
        else
        {
            Restart();
            running = true;
            if (buttonText)
            {
                buttonText.text = "Stop";
            }
        }
    }
    public void SetMutationRate(float mutation) {
        mutationRate = mutation;
    }

    public void ChooseSelectionProcess(float value) {

        selectionProcess = (int)value;
        switch (selectionProcess) {
            case 0:
                selectionProcessText.text = "Roulette Wheel Slection";
                break;
            case 1:
                selectionProcessText.text = "Above Median Selection";
                break;
            case 2:
                selectionProcessText.text = "Best with Each Above Median";
                break;
            case 3:
                selectionProcessText.text = "Single Point Crossover";
                break;
            case 4:
                selectionProcessText.text = "Multiple Point Crossover";
                break;
        }
    }
    public void Restart() {
        spawner.SpawnCars();
        follow = Camera.main.GetComponent<Follow>();
        geneticAlgorithm = new GeneticAglorithm<float>(1, 20, dnaSize, selectionProcess, random, GetRandomGene, GetInitGene, FitnessFunction, mutationRate: mutationRate);
    }
}
