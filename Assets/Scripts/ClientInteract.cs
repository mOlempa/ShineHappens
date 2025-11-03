using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientInteract : MonoBehaviour
{
    List<string> clientTypes = new List<string>() { "elder woman", "elder man", "adventurer" };
    List<string> gemProperties = new List<string>() { "pain reduction", "one time healing", "metamorphosis" };

    private void Start()
    {
        
    }

    public void startInteraction()
    {
        int timesBargaining = 3;

        string prompt = $"You are a new client visiting gem smith creating magical amulets in their workshop. " +
        $"You are a {clientTypes[Random.Range(0, clientTypes.Count)]}. " +
        $"You want the gem smith to create you an amulet that has {gemProperties[Random.Range(0, gemProperties.Count)]} as " +
        $"its magical property. Generate a response to the gem smith saying 'Welcome in, how may I help you?'";
    }

    public void bargain()
    {
        string prompt = $"You want the price to lower by  ";
    }

    public void endInteraction()
    {
        string prompt = $"Generate a response to '' ";
    }

}
