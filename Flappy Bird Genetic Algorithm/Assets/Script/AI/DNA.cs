using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA 
{
    public List<float> genes = new List<float>();
    public DNA(int genomeLength = 50)
    {
        for(int i = 0; i < genomeLength; i++)
        {
            genes.Add(Random.Range(-1.0f, 1.0f));
        }
    }
    public DNA(DNA parent, DNA partner, float mutationRate = 0.01f)
    {
        for (int i = 0; i < parent.genes.Count; i++)
        {
            float mutationChance = Random.Range(0.0f, 1.0f);
            if(mutationChance <= mutationRate)
            {
                genes.Add(Random.Range(-1.0f, 1.0f));
            }
            else
            {
                int chance = Random.Range(0, 2);
                if(chance == 0)
                    genes.Add(parent.genes[i]);  
                else
                    genes.Add(partner.genes[i]);
            }
        }
    }
}
