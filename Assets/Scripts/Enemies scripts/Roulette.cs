using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette
{
    //La ruleta es usada para que en determinado tiempo los enemigos continuen patrullando o queden en idle

    public string Execute(Dictionary<string, int> dic) //Ruleta, me pide un diccionario de strings e ints 
    {
        int total = 0;
        foreach (var item in dic) //Añado cada value del diccionario, y lo sumo con el int creado
        {
            total += item.Value;
        }
        int random = Random.Range(0, total);//Hago un random para tomar un valor random 
        foreach (var item in dic) //para cada valor en el diccionario
        {
            random = random - item.Value; //el random va a ser igual al random - el value del item del diccionario
            if (random < 0) //el random tiene que ser menor a 0 para que de 
            {
                return item.Key; //Devuelvo el string del diccionario 
            }
        }
        return "";
    }
}
