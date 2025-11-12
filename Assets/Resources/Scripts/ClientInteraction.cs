using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClientInteraction : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI playerText;

    [SerializeField]
    TextMeshProUGUI clientText;

    [SerializeField]
    UnityAndGeminiV3 gemini;

    [SerializeField]
    TextMeshProUGUI responseButtonText1;
    [SerializeField]
    TextMeshProUGUI responseButtonText2;
    [SerializeField]
    TextMeshProUGUI responseButtonText3;

    int price = 50;
    float bargainedPercentage = 0.8f;
    bool nextLine = false;
    int replyNumber = 0;
    public void runNextLine(int number) { nextLine = true; replyNumber = number; }

    List<Character> predefinedCharacters = new List<Character>() {
        new Character("Goob", "elder man", Effect.PainReduction, 3, true),
    };

    Character currentCharacter;


    public void InteractWithClient()
    {
        //currentCharacter = GetRandom(predefinedCharacters);
        currentCharacter = new Character("Goob", "elder man", Effect.PainReduction, 3, true);
        StartCoroutine(clientInteractionCR());
        //StartInteraction();
    }

    void StartInteraction()
    {
        string botInstructions = $"You are a new client, a {currentCharacter.type} named {currentCharacter.name} visiting gem smith creating " +
            $"magical amulets in their workshop. You want the gem smith to create you an amulet that has " +
            $"{getStr(currentCharacter.wantedEffect)} as its magical property. ";

        gemini.botInstructions = botInstructions;
        playerText.text = "Welcome in, how may I help you?";
        clientText.text = "Client says...";

        print("Bot instructions: " + botInstructions);
        // Try to connect to Gemini in at the start of the interaction
        gemini.SendChat();
        // Comment the line above and uncomment these two for always running default dialog
        //gemini.connectionFailure = true;
        //gemini.connectionAttemptFin = true;
    }


    // Client interaction singular loop
    IEnumerator clientInteractionCR()
    {
        SetSameReply("...");
        // Do the start of the interaction
        StartInteraction();

        // Wait for the connection attempt to end
        yield return new WaitUntil(() => gemini.connectionAttemptFin);
        // If couldn't connect to Gemini
        if (gemini.connectionFailure)
        {
            clientText.text = "<color=red><size=30>Gemini unavailable, running default dialog.</color></size>\n\n" +
                $"Hi! My name is {currentCharacter.name}, I'm a(n) {currentCharacter.type}.\n" +
                $"Could you craft me a gem that has an effect of {getStr(currentCharacter.wantedEffect)}?";
        }

        // Wait for the player to press one of the given buttons
        yield return new WaitUntil(() => nextLine);

        nextLine = false;
        bool end = false;

        playerText.text = $"Sure thing! That will cost {price} coins.";
        gemini.botInstructions = "";

        // If client is likely to argue on the pricing, give Gemini the instruction to do so
        if (currentCharacter.bargainingTimes > 0)
        {
            gemini.botInstructions = $"Try to bargain the price down to {(bargainedPercentage * price).ToString()}.";
            SetReplies(bargainReplies);
            if (gemini.connectionFailure)
            {
                clientText.text = $"I would like to pay {bargainedPercentage * price} instead.";
            }
        }

        int counter = 0;

        // Bargaining loop (goes max. as many times as the character has bargainingTimes set)
        while (currentCharacter.bargainingTimes > 0)
        {
            currentCharacter.bargainingTimes--;
            counter++;

            // Send request for further interaction only if there was no connection failure before this point
            if (!gemini.connectionFailure)
            {
                gemini.SendChat();
                yield return new WaitUntil(() => gemini.connectionAttemptFin);

                // If the connection was lost during the interaction
                if (gemini.connectionFailure)
                {
                    clientText.text = "<color=red><size=30>ERROR: Gemini left the chat :(</color></size>\n\n" +
                        "AAAH! I- MILK- I LEFT MILK ON THE STOVE!! Sorry, gotta go, bye!!\n";
                    break;
                }
            }

            // Wait for the player to press one of the given buttons
            yield return new WaitUntil(() => nextLine);
            nextLine = false;
            clientText.text = "";
            gemini.botInstructions = "";

            // Depending on what the player chose
            switch (replyNumber)
            {
                case 0: // DISAGREEING
                    playerText.text = "I cannot agree on that price.";
                    // If the character still has bargainingTimes > 0 after the -1 decrease
                    if (currentCharacter.bargainingTimes > 0)
                    {
                        if (gemini.connectionFailure)
                        {
                            // Depending on which time it is that the character is insisting on lowering the price
                            clientText.text = counter == 1 ? "Please?" : "Pretty please?";
                        }
                        gemini.botInstructions = "Insist on your proposition. ";

                    }
                    else
                    {
                        if (gemini.connectionFailure)
                        {
                            clientText.text = $"Okay, fine :<\n";
                        }
                        gemini.botInstructions = "Agree on the given price. ";
                        end = true;
                    }
                    break;

                case 1: // COMPROMISING
                    playerText.text = $"How about we meet halfway and set the price to {((bargainedPercentage * price + price)/2)}?";
                    // Instruct Gemini according to the character's likelyToCompromise value
                    if (currentCharacter.likelyToCompromise)
                    {
                        if (gemini.connectionFailure)
                        {
                            clientText.text = "Sure! :)\n";
                        }
                        gemini.botInstructions = "Agree on the compromise. ";
                        end = true;
                    }
                    else
                    {
                        if (gemini.connectionFailure)
                        {
                            clientText.text = "Pretty please?";
                        }
                        gemini.botInstructions = "Insist on your proposition. ";
                    }
                    break;

                case 2: // AGREEING
                    playerText.text = "Alright then.";
                    // End interaction
                    end = true;
                    break;
            }

            if (end)
            {
                break;
            }
        }

        // Set up a closing interaction
        gemini.botInstructions += "Finish the interaction.";
        if (gemini.connectionFailure)
        {
            clientText.text += "Thank you!";
        }
        else
        {
            gemini.SendChat();
        }
        SetSameReply("Finish");

        // Wait for the player to press one of the given buttons
        yield return new WaitUntil(() => nextLine);
        nextLine = false;

        // Switch off the client interaction panel
        gameObject.SetActive(false);
        GameManager.Instance.UnlockPlayer();
    }


    // Setting replies on response buttons
    void SetReplies((string reply1, string reply2, string reply3) replies)
    {
        responseButtonText1.text = replies.reply1;
        responseButtonText2.text = replies.reply2;
        responseButtonText3.text = replies.reply3;
    }

    // Setting replies on response buttons
    void SetSameReply(string reply)
    {
        responseButtonText1.text = reply;
        responseButtonText2.text = reply;
        responseButtonText3.text = reply;
    }
    (string reply1, string reply2, string reply3) bargainReplies = ("Disagree", "Compromise", "Agree");

    // Do losowania czegoœ randomowego z listy
    T GetRandom<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    string getStr(Effect effect)
    {
        switch (effect)
        {
            case Effect.PainReduction:
                return "pain reduction";
            case Effect.Metamorphosis:
                return "metamorphosis";
            default:
                return "healing";
        }
    }

}
