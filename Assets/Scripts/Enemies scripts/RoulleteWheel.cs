using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class RoulleteWheel<T> 
{
    
    
    int ProbabilityToDropSomething=100;

  
    public T ProbabilityCalculator(List<Tuple<int,T>> list)
    {
        int dropChance = UnityEngine.Random.Range(0, 101);
        if (dropChance >ProbabilityToDropSomething)
        {
            return default(T);
        }
        else
        {
            int weight=0;
            
            for (int i = 0; i < list.Count; i++)    
            {
               
                weight += list[i].Item1;
            }
            int randomValue = UnityEngine.Random.Range(0, weight); 
           
            for (int j = 0; j < list.Count; j++)     
            {
                if (randomValue < list[j].Item1)
                {
                    
                    return list[j].Item2; 
                }
                randomValue -= list[j].Item1; 
                
            }

            return default(T);
        }

    }
    

}
