using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject bouncerPrefab;
    public GameObject fanPrefab;

    public int bouncerInvCount;
    public int fanInvCount;

    public TextMeshProUGUI bouncerText;
    public TextMeshProUGUI fanText;

    private Button bouncerButton;
    private Button fanButton;

    private void Start()
    {
        bouncerButton = bouncerText.GetComponentInParent<Button>();
        fanButton = fanText.GetComponentInParent<Button>();

        UpdateCounterDisplays();
    }

    public void SpawnObject(string name)
    {
        if (name == "Bouncer")
        {
            bouncerInvCount--;
            UpdateCounterDisplays();
            Instantiate(bouncerPrefab, new Vector3(0, -1000, 0), Quaternion.identity);
        }

        if (name == "Fan")
        {
            fanInvCount--;
            UpdateCounterDisplays();
            Instantiate(fanPrefab, new Vector3(0, -1000, 0), Quaternion.identity);
        }
    }

    public void DestroyObject(string name)
    {
        if (name == "Bouncer")
        {
            bouncerInvCount++;
            UpdateCounterDisplays();
        }

        if (name == "Fan")
        {
            fanInvCount++;
            UpdateCounterDisplays();
        }
    }

    private void UpdateCounterDisplays()
    {
        bouncerText.text = "x" + bouncerInvCount;
        fanText.text = "x" + fanInvCount;

        if(bouncerInvCount == 0)
            bouncerButton.interactable = false;
        else
            bouncerButton.interactable = true;

        if (fanInvCount == 0)
            fanButton.interactable = false;
        else
            fanButton.interactable = true;
    }
}
