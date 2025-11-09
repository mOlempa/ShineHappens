using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientInteraction : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI playerText;

    [SerializeField]
    UnityAndGeminiV3 gemini;

    [SerializeField]
    TextMeshProUGUI responseButtonText1;
    [SerializeField]
    TextMeshProUGUI responseButtonText2;
    [SerializeField]
    TextMeshProUGUI responseButtonText3;

    bool nextLine = false;
    int replyNumber = 0;
    public void runNextLine(int number) { nextLine = true; replyNumber = number; }

    List<Character> predefinedCharacters = new List<Character>() {
        new Character("Goob", "elder man", Effect.PainReduction, 0, true),
    };

    Character currentCharacter;

    private void Start()
    {
        gameObject.SetActive(false);
    }


    public void InteractWithClient()
    {
        currentCharacter = GetRandom(predefinedCharacters);
        //StartCoroutine(clientInteractionCR());
        StartInteraction();
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

    void StartInteraction()
    {
        string botInstructions = $"You are a new client, a {currentCharacter.type} named {currentCharacter.name} visiting gem smith creating " +
            $"magical amulets in their workshop. You want the gem smith to create you an amulet that has " +
            $"{getStr(currentCharacter.wantedEffect)} as its magical property. ";

        gemini.botInstructions = botInstructions;
        playerText.text = "Welcome in, how may I help you?";

        print("Bot instructions: " + botInstructions);
        gemini.SendChat();
    }

    void SetReplies((string reply1, string reply2, string reply3) replies)
    {
        responseButtonText1.text = replies.reply1;
        responseButtonText2.text = replies.reply2;
        responseButtonText3.text = replies.reply3;
    }

    void SetSameReply(string reply)
    {
        responseButtonText1.text = reply;
        responseButtonText2.text = reply;
        responseButtonText3.text = reply;
    }
    (string reply1, string reply2, string reply3) bargainReplies = ("Disagree", "Compromise", "Agree");


    IEnumerator clientInteractionCR()
    {
        // Do the start of the interaction
        StartInteraction();
        print("<color=lime>Starting wait for button press</color>");
        // Wait for the player to press one of the given buttons
        yield return new WaitUntil(() => nextLine);
        print("finished waiting");

        nextLine = false;
        bool end = false;

        playerText.text = "Sure thing! That will cost 50 coins.";

        // If client is likely to argue on the pricing, give Gemini the instruction to do so
        if (currentCharacter.bargainingTimes > 0)
        {
            gemini.botInstructions = "Try to bargain the price.";
            SetReplies(bargainReplies);
        }

        // Bargaining loop (goes max. as many times as the character has bargainingTimes set)
        while (currentCharacter.bargainingTimes > 0)
        {
            currentCharacter.bargainingTimes--;
            gemini.SendChat();

            // Wait for the player to press one of the given buttons
            yield return new WaitUntil(() => nextLine);
            nextLine = false;

            // Depending on what the player chose
            switch (replyNumber)
            {
                case 0: // DISAGREEING
                    playerText.text = "I cannot agree on that price.";
                    // If the character still has bargainingTimes > 0 after the -1 decrease
                    if(currentCharacter.bargainingTimes > 0)
                    {
                        gemini.botInstructions = "Insist on your proposition.";
                    }
                    break;

                case 1: // COMPROMISING
                    playerText.text = "How about we meet halfway?";
                    // Instruct Gemini according to the character's likelyToCompromise value
                    if (currentCharacter.likelyToCompromise)
                    {
                        gemini.botInstructions = "Agree on the compromise.";
                    }
                    else
                    {
                        gemini.botInstructions = "Insist on your proposition.";
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
        gemini.botInstructions += "Agree on the price.";
        gemini.SendChat();
        SetSameReply("Finish");

        // Wait for the player to press one of the given buttons
        yield return new WaitUntil(() => nextLine);
        nextLine = false;

        // Switch off the client interaction panel
        gameObject.SetActive(false);
    }


    // Do losowania czegoœ randomowego z listy
    T GetRandom<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

}
