using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Utilities;

struct Limits
{
    public float max;
    public float min;
}

public class TimingMinigame : MonoBehaviour
{
    [SerializeField]
    GameObject container, greenArea, yellowArea, redAreaBottom, redAreaTop, orangeArea;

    Limits yLimits;
    Limits greenLimits;
    Limits yellowLimits;
    Limits orangeLimits;
    //Limits redLimits;
    float step = 10;
    float stepChange = 0.1f;
    float center;

    bool goDown = false;
    bool stop = false;

    float deltaTime;

    GemAnimationScript gemAnimation;

    void Start()
    {
        center = transform.position.y;
        float containerHeight = container.GetComponent<RectTransform>().rect.height;
        yLimits.max = transform.position.y + containerHeight / 2;
        yLimits.min = transform.position.y - containerHeight / 2;
        float greenHeight = greenArea.GetComponent<RectTransform>().rect.height;
        greenLimits.max = transform.position.y + greenHeight / 2;
        greenLimits.min = transform.position.y - greenHeight / 2;
        float yellowHeight = yellowArea.GetComponent<RectTransform>().rect.height;
        yellowLimits.max = transform.position.y + yellowHeight / 2;
        yellowLimits.min = transform.position.y - yellowHeight / 2;
        float orangeHeight = orangeArea.GetComponent<RectTransform>().rect.height;
        orangeLimits.max = transform.position.y + orangeHeight / 2;
        orangeLimits.min = transform.position.y - orangeHeight / 2;

        gemAnimation = GameManager.Instance.currentGem.GetComponent<GemAnimationScript>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (stop)
            {
                stop = false;
            }
            else
            {
                stop = true;
                if(transform.position.y < greenLimits.max && transform.position.y > greenLimits.min)
                {
                    print("<color=lime>YOU WON :D</color>");
                    gemAnimation.switchAnimation(GemStabilityLevel.Stable);
                    GameManager.Instance.gemParticles.playGemExhale();
                }
                else if(transform.position.y < yellowLimits.max && transform.position.y > yellowLimits.min)
                {
                    print("<color=yellow>Good :)</color>");
                    gemAnimation.switchAnimation(GemStabilityLevel.Wavering);
                }
                else if (transform.position.y < orangeLimits.max && transform.position.y > orangeLimits.min)
                {
                    print("<color=orange>Well... At least you didn't loose</color>");
                    gemAnimation.switchAnimation(GemStabilityLevel.Disrupted);
                }
                else
                {
                    print("<color=red>You lost :(</color>");
                    gemAnimation.switchAnimation(GemStabilityLevel.Unstable);
                    GameManager.Instance.gemParticles.playGemPuff();
                }
            }

        }

        deltaTime = Time.deltaTime * 100;

        if (stop)
        {
            return;
        }

        if (goDown)
        {
            if (transform.position.y > center)
            {
                step += stepChange * deltaTime;
            }
            else
            {
                step -= stepChange * deltaTime;
            }

            if (transform.position.y > yLimits.min)
            {
                transform.position -= new Vector3(0, step) * deltaTime;
            }
            else
            {
                goDown = false;
            }
        }
        else
        {
            if (transform.position.y < center)
            {
                step += stepChange * deltaTime;
            }
            else
            {
                step -= stepChange * deltaTime;
            }

            if (transform.position.y < yLimits.max)
            {
                transform.position += new Vector3(0, step) * deltaTime;
            }
            else
            {
                goDown = true;
            }

        }
        //print("step = " + step);
        //print("time.delatime = " + Time.deltaTime);
    }

}
